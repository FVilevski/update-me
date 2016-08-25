using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App.Distributors
{
    public interface IDistributor
    {
        void DistributeRelease(List<string> files);

        string DistributeFile(string sourcePath);

        void DownloadFile(string source, string destination);

        bool FileExists(string fileName);
    }
}
