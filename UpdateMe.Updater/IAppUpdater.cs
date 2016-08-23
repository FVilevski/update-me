using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.Updater
{
    public interface IAppUpdater : IDisposable
    {
        /// <summary>
        /// Returns the current version of the application,if application install is invalid it returns null
        /// </summary>
        NuGet.SemanticVersion CurrentVersion
        {
            get;
        }
        /// <summary>
        /// Checks if new application version is available 
        /// </summary>
        /// <returns>true if new update is available, false if not update is available or something fails</returns>
        Task<bool> HasNewUpdateAsync();
        /// <summary>
        /// Downloads neweast application version
        /// </summary>
        /// <param name="restartAfterUpdate">If set to true application will restart to the newest version</param>
        /// <param name="progress">Action for updating the update progress</param>
        /// <returns>true if appliocation is updated, false if something went wrong</returns>
        Task<bool> UpdateAppAsync(bool restartAfterUpdate = true, Action<int> progress = null);

        void SetRunOnWindowsStartup(string arguments = null);

        /// <summary>
        /// Closes current application and starts the newest version
        /// </summary>
        void RestartApplication(string arguments = null);
    }
}
