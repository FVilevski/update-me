using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App.Distributors
{
    public class LocalDistributor : IDistributor
    {
        private readonly string _distributeLocation;
        public LocalDistributor(string distributeLocation)
        {
            _distributeLocation = distributeLocation;
            if (!Directory.Exists(_distributeLocation))
            {
                Directory.CreateDirectory(_distributeLocation);
            }
        }

        public string DistributeFile(string file)
        {
            string destination = Path.Combine(_distributeLocation, Path.GetFileName(file));
            File.Copy(file, destination);
            Console.Write($"File {Path.GetFileName(file)} distributed");
            return destination;
        }

        public void DistributeRelease(List<string> files)
        {
            foreach (string file in files)
            {
                File.Copy(file, Path.Combine(_distributeLocation, Path.GetFileName(file)));
                Console.Write($"File {Path.GetFileName(file)} distributed");
            }
            $"All files are distributed to local folder {_distributeLocation}".WriteSuccessToConsole();
        }

        public void DownloadFile(string source, string destination)
        {
            try
            {
                File.Copy(Path.Combine(_distributeLocation, Path.GetFileName(source)), destination);
            }
            catch (Exception ex)
            {
                $"Download of file {source} failed, with error message: {ex.Message}".WriteErrorToConsole();
                throw;
            }
        }

        public bool FileExists(string fileName)
        {
            return System.IO.File.Exists(Path.Combine(_distributeLocation, Path.GetFileName(fileName)));
        }
    }
}
