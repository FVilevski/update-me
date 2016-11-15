namespace UpdateMe.Updater
{
    using System;
    using System.Threading.Tasks;

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
        /// Downloads newest application version
        /// </summary>
        /// <param name="restartAfterUpdate">If set to true application will restart to the newest version</param>
        /// <param name="progress">Action for updating the update progress</param>
        /// <returns>true if application is updated, false if something went wrong</returns>
        Task<bool> UpdateAppAsync(bool restartAfterUpdate = true, Action<int> progress = null);

        /// <summary>
        /// Downloads newest application versions in background, but do not apply them.
        /// </summary>
        /// <param name="progress">Action for updating the download progress</param>
        /// <returns>True if versions are downloaded, false if something went wrong or nothing was downloaded</returns>
        Task<bool> GetUpdatesInBackgroundAsync(Action<int> progress = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restartAfterUpdate">If set to true application will restart to the newest version</param>
        /// <param name="progress">Action for updating the update progress</param>
        /// <returns>True if application is updated, false if not updated</returns>
        Task<bool> ApplyDownloadedUpdatesAsync(bool restartAfterUpdate = true, string restartArguments = null, Action<int> progress = null);

        void SetRunOnWindowsStartup(string arguments = null);

        /// <summary>
        /// Closes current application and starts the newest version
        /// </summary>
        void RestartApplication(string arguments = null);
    }
}
