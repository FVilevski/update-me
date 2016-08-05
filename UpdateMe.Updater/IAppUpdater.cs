using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.Updater
{
    public interface IAppUpdater
    {
        NuGet.SemanticVersion CurrentVersion
        {
            get;
        }

        Task<bool> HasNewUpdate();

        Task UpdateApp(Action<int> progress = null);
    }
}
