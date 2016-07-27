using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateMe.App
{
    class Program
    {
        private static RelaseRequest request = new RelaseRequest();
        private static string _distributionPath = string.Empty;

        static void Main(string[] args)
        {
            try
            {
                ParseArguments(args);

                IDistributor distributor = GetDistributor(_distributionPath);
                if (distributor == null)
                {
                    Environment.Exit(Utils.EXIT_CODE_ERROR_UNEXPECTED);
                    return;
                }
                if (!request.Validate(true))
                {
                    Environment.Exit(Utils.EXIT_CODE_ERROR_INPUT);
                    return;
                }

                UpdateBuilder updateBuilder = new UpdateBuilder(distributor);
                updateBuilder.ReleaseNewVersion(request);
                Environment.Exit(Utils.EXIT_CODE_SUCCESS);
            }
            catch(Exception ex) 
            {
                ex.Message.WriteErrorToConsole();
                Environment.Exit(Utils.EXIT_CODE_ERROR_UNEXPECTED);
                //todo log exception
            }

        }

        private static IDistributor GetDistributor(string distributionPath)
        {
            IDistributor distributor = null;
            if (string.IsNullOrEmpty(distributionPath))
            {
                "Amazon bucket has to be provided".WriteErrorToConsole();
                Environment.Exit(Utils.EXIT_CODE_ERROR_INPUT);
                return distributor;
            }

            string region = Environment.GetEnvironmentVariable(AmazonDistributor.AWS_REGION);
            if (string.IsNullOrWhiteSpace(region))
            {
                $"{AmazonDistributor.AWS_REGION} enviroment variable has to set".WriteErrorToConsole();
                Environment.Exit(Utils.EXIT_CODE_ERROR_INPUT);
                return distributor;
            }

            string awsKey = Environment.GetEnvironmentVariable(Amazon.Runtime.EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_ACCESSKEY);
            if (string.IsNullOrWhiteSpace(awsKey))
            {
                $"{Amazon.Runtime.EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_ACCESSKEY} enviroment variable has to set".WriteErrorToConsole();
                Environment.Exit(Utils.EXIT_CODE_ERROR_INPUT);
                return distributor;
            }

            string awsSecret = Environment.GetEnvironmentVariable(Amazon.Runtime.EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_SECRETKEY);
            if (string.IsNullOrWhiteSpace(awsSecret))
            {
                $"{Amazon.Runtime.EnvironmentVariablesAWSCredentials.ENVIRONMENT_VARIABLE_SECRETKEY} enviroment variable has to set".WriteErrorToConsole();
                Environment.Exit(Utils.EXIT_CODE_ERROR_INPUT);
                return distributor;
            }


            Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(awsKey, awsSecret, Amazon.RegionEndpoint.GetBySystemName(region));

            distributor = new AmazonDistributor(_distributionPath, client);
            return distributor;
        }

        private static void ParseArguments(string[] args)
        {
            OptionSet p = new OptionSet()
            {
                 {
                    "v|version=",
                    "Version of the app that need to be created",
                    v => { request.Version=v; } },
                  {
                    "s|source=",
                    "Source directory from with to get files",
                    s =>
                    {
                          request.SourceDir = s;
                    } },
                  {
                    "t|title=",
                    "Title of the application",
                    t => { request.Title= t; } },
                   {
                    "a|authors=",
                    "Title of the application",
                    a => { request.Authors= a; } },
                   {
                    "id|appId=",
                    "Id of the application",
                    i => { request.AppId= i; } },
                   {
                    "icon|appIcon=",
                    "Path to the app icon",
                    i => { request.AppIcon= i; } },
                    {
                    "cert|signCertPath=",
                    "Path to the certificate path",
                    i => { request.SignCertificatePath= i; } },
                    {
                    "certPass|signCertPass=",
                    "Path to the certificate path",
                    i => { request.SignCertificatePassword= i; }
                },
                     {
                    "dp|distPath=",
                    "Aws bucket path to with the publish should be send",
                    i => { _distributionPath = i; }
                },


               };

            try
            {
                List<string> extra = p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("greet: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `greet --help' for more information.");
            }

        }
    }
}
