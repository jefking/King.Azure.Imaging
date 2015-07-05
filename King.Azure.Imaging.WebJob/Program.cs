namespace King.Azure.Imaging.WebJob
{
    using System;
    using System.Configuration;
    using Microsoft.Azure.WebJobs;

    public class Program
    {
        public static void Main()
        {
            if (!VerifyConfiguration())
            {
                Console.ReadLine();
                return;
            }
            
            var host = new JobHost();
            host.RunAndBlock();
        }

        private static bool VerifyConfiguration()
        {
            var webJobsDashboard = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
            var webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;

            var configOK = true;
            if (string.IsNullOrWhiteSpace(webJobsDashboard) || string.IsNullOrWhiteSpace(webJobsStorage))
            {
                configOK = false;
                Console.WriteLine("Please add the Azure Storage account credentials in App.config");
            }

            return configOK;
        }
    }
}