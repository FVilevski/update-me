using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    public class AmazonDistributor : IDistributor
    {
        public const string AWS_REGION = "AWS_REGION";

        private readonly IAmazonS3 _client;
        private readonly string _bucketPath;
        public AmazonDistributor(string bucketPath, IAmazonS3 client)
        {
            _client = client;
            _bucketPath = bucketPath;
        }

        public void DistributeRelease(List<string> files)
        {
            foreach (string file in files)
            {
                DistributeFile(file);
            }
            $"All files are distributed to AWS bucket {_bucketPath}".WriteSuccessToConsole();
        }

        public void DistributeFile(string sourcePath)
        {
            using (Stream fileStream = File.OpenRead(sourcePath))
            {
                Console.Write($"Distributing {Path.GetFileName(sourcePath)}");

                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = _bucketPath,
                    Key = System.IO.Path.GetFileName(sourcePath),
                    CannedACL = S3CannedACL.PublicRead,
                    InputStream = fileStream,
                    AutoCloseStream = true,
                  
                };
                request.StreamTransferProgress += (sender, e) =>
                {
                    Amazon.Runtime.StreamTransferProgressArgs args = (e as Amazon.Runtime.StreamTransferProgressArgs);
                    args.PercentDone.WriteProgressToConsole();

                };
                _client.PutObject(request);
                Console.WriteLine();
            }
        }
    }
}
