using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.Updater
{
    public class AppUpdater : IAppUpdater
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

        private readonly IUpdateManager _updateManager;

        public AppUpdater(IUpdateManager updateManager)
        {
            _updateManager = updateManager;
        }

        public async Task<bool> HasNewUpdateAsync()
        {
            bool hasUpdate = false;
            try
            {
                UpdateInfo info = await _updateManager.CheckForUpdate();

                hasUpdate = !this.CurrentVersion.Equals(info.FutureReleaseEntry.Version);

            }
            catch (System.Net.WebException ex)
            {
                System.Diagnostics.Trace.WriteLine($"HasNewUpdate failed with web expetion {ex.Message}");
                hasUpdate = false;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"HasNewUpdate failed with unexpected error {ex.Message}");
                hasUpdate = false;
            }
            return hasUpdate;
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
                System.Diagnostics.Trace.WriteLine($"UpdateApp failed with web expetion {ex.Message}");
                hasUpdated = false;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"UpdateApp failed with unexpected error {ex.Message}");
                hasUpdated = false;
            }
            return hasUpdated;
        }

        public void RestartApplication(string arguments = null)
        {
            UpdateManager.RestartApp(arguments);
            this.Dispose();
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

        public void Dispose()
        {
            _updateManager.Dispose();
        }
    }
}
