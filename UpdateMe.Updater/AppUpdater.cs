namespace UpdateMe.Updater
{
    using Squirrel;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;

    public class AppUpdater : IAppUpdater, IDisposable
    {
        NuGet.SemanticVersion _currentVersion;
        public NuGet.SemanticVersion CurrentVersion
        {
            get
            {
                if (_currentVersion == null)
                {
                    _currentVersion = _updateManager.CurrentlyInstalledVersion();
                }
                return _currentVersion;
            }
        }

        readonly IUpdateManager _updateManager;

        UpdateInfo _downloadedUpdateInfo;

        public AppUpdater(IUpdateManager updateManager)
        {
            _updateManager = updateManager;
            _downloadedUpdateInfo = null;
        }

        public async Task<bool> HasNewUpdateAsync()
        {
            bool hasUpdate = false;
            try
            {
                UpdateInfo info = await _updateManager.CheckForUpdate();
                hasUpdate = IsNewVersionAvailable(info);
            }
            catch (System.Net.WebException ex)
            {
                System.Diagnostics.Trace.WriteLine($"HasNewUpdate failed with web expetion {ex.Message}");
                hasUpdate = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"HasNewUpdate failed with unexpected error {ex.Message}");
                hasUpdate = false;
            }
            return hasUpdate;
        }

        bool IsNewVersionAvailable(UpdateInfo info)
        {
            if (info == null)
            {
                return false;
            }

            return !CurrentVersion.Equals(info.FutureReleaseEntry.Version);
        }

        public async Task<bool> UpdateAppAsync(bool restartAfterUpdate = true, Action<int> progress = null)
        {
            bool hasUpdated = false;
            try
            {
                if (await this.HasNewUpdateAsync())
                {
                    ReleaseEntry info = await _updateManager.UpdateApp(progress);
                    hasUpdated = true;

                    if (restartAfterUpdate)
                    {
                        RestartApplication();
                    }
                }
            }
            catch (System.Net.WebException ex)
            {
                System.Diagnostics.Trace.WriteLine($"UpdateApp failed with web exception {ex.Message}");
                hasUpdated = false;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"UpdateApp failed with unexpected error {ex.Message}");
                hasUpdated = false;
            }
            return hasUpdated;
        }

        public async Task<bool> GetUpdatesInBackgroundAsync(Action<int> progress = null)
        {
            bool hasDownloaded = false;

            try
            {
                UpdateInfo info = await _updateManager.CheckForUpdate();

                if (IsNewVersionAvailable(info))
                {
                    await _updateManager.DownloadReleases(info.ReleasesToApply, progress);
                    hasDownloaded = true;
                    _downloadedUpdateInfo = info;
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine($"GetUpdatesInBackgroundAsync failed with web exception {ex.Message}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"GetUpdatesInBackgroundAsync failed with unexpected error {ex.Message}");
            }

            return hasDownloaded;
        }

        public async Task<bool> ApplyDownloadedUpdatesAsync(bool restartAfterUpdate = true, string restartArguments = null, Action<int> progress = null)
        {
            bool isUpdated = false;

            try
            {
                if (IsNewVersionAvailable(_downloadedUpdateInfo))
                {
                    await _updateManager.ApplyReleases(_downloadedUpdateInfo, progress);
                    isUpdated = true;
                    _downloadedUpdateInfo = null;

                    if (restartAfterUpdate)
                    {
                        RestartApplication(restartArguments); 
                    }
                }
            }
            catch (WebException ex)
            {
                Trace.WriteLine($"ApplyDownloadedUpdatesAsync failed with web exception {ex.Message}");
                _downloadedUpdateInfo = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"ApplyDownloadedUpdatesAsync failed with unexpected error {ex.Message}");
                _downloadedUpdateInfo = null;
            }

            return isUpdated;
        }

        public void RestartApplication(string arguments = null)
        {
            Dispose();
            UpdateManager.RestartApp(arguments);
        }

        public void SetRunOnWindowsStartup(string arguments = null)
        {
            _updateManager.CreateShortcutsForExecutable(
                Assembly.GetEntryAssembly().Location,
                ShortcutLocation.Startup,
                false,
                arguments,
                null);
        }

        #region Dispose
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _updateManager.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AppUpdater() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            //GC.SuppressFinalize(this);
        }
        #endregion
    }
}
