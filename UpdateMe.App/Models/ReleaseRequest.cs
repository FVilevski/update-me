using System.Collections.Generic;
using System.IO;

namespace UpdateMe.App
{
    public class RelaseRequest
    {
        public string Version { get; set; }

        public string SourceDir { get; set; }

        public string Title { get; set; }

        public string Authors { get; set; }

        public string AppId { get; set; }

        public string AppIcon { get; set; }

        public string SignCertificatePath { get; set; }

        public string SignCertificatePassword { get; set; }

        public string DistributonPath { get; set; }

        public Models.DistributionTypeEnum DistributonType { get; set; }

        public string NugetPackagePath
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), "UpdateMe", $"{this.AppId}.{this.Version}.nupkg");
            }
        }

        public string ReleasePath
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), "UpdateMe", "Releases");
            }
        }

        public bool Validate(bool printErrors = false)
        {
            List<string> validationErrors = new List<string>();



            if (string.IsNullOrWhiteSpace(this.Version))
            {
                validationErrors.Add("version must be supplied");

            }

            if (string.IsNullOrWhiteSpace(this.Title))
            {
                validationErrors.Add("title must be supplied");

            }

            if (string.IsNullOrWhiteSpace(this.AppId))
            {
                validationErrors.Add("appId must be supplied");
            }

            if(!string.IsNullOrWhiteSpace(this.SourceDir))
            {
                this.SourceDir = Path.GetFullPath(this.SourceDir);
            }

            if (!Directory.Exists(this.SourceDir))
            {
                validationErrors.Add("-source must be valid directory path");
            }

            if (string.IsNullOrWhiteSpace(this.Authors))
            {
                validationErrors.Add("-authors must be valid directory path");
            }

            if (!string.IsNullOrWhiteSpace(this.AppIcon))
            {
                this.AppIcon = Path.GetFullPath(this.AppIcon);
                if (File.Exists(this.AppIcon) == false)
                {
                    validationErrors.Add("appIcon must be a existing file");
                }
            }

            if (!string.IsNullOrWhiteSpace(this.SignCertificatePassword) || !string.IsNullOrWhiteSpace(this.SignCertificatePath))
            {
                this.SignCertificatePath = Path.GetFullPath(this.SignCertificatePath);
                if (File.Exists(this.SignCertificatePath) == false)
                {
                    validationErrors.Add("Sign certificate path must be existing file");
                }


                if (string.IsNullOrWhiteSpace(this.SignCertificatePassword))
                {
                    validationErrors.Add("Sign certificate password must be provided");
                }

            }
            if (printErrors)
            {
                validationErrors.ForEach(c => c.WriteErrorToConsole());
            }
            return validationErrors.Count == 0;

        }



    }
}
