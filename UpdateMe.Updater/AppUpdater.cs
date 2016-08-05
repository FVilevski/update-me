using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> HasNewUpdate()
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
            return hasUpdate;
        }

        public async Task UpdateApp(Action<int> progress = null)
        {
            try
            {
                if (await this.HasNewUpdate())
                {
                    ReleaseEntry info = await _updateManager.UpdateApp(progress);
                    _updateManager.Dispose();
                    UpdateManager.RestartApp();
                }
            }
            catch (System.Net.WebException ex)
            {
                System.Diagnostics.Trace.WriteLine($"UpdateApp failed with web expetion {ex.Message}");
            }
        }
    }
}
