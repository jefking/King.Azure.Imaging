namespace King.Azure.Imaging.WebJob
{
    using System;
    using System.Configuration;
    using Microsoft.Azure.WebJobs;

    public class Program
    {
        public static void Main()
        {
            var webJobsStorage = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString;
            var storageAcc = ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
            
            if (string.IsNullOrWhiteSpace(webJobsStorage) || string.IsNullOrWhiteSpace(storageAcc))
            {
                Console.WriteLine("Please add the Azure Storage account credentials ['StorageAccount' & 'AzureWebJobsStorage'] in App.config");
                Console.Read();
                return;
            }
            else
            {
                var host = new JobHost();
                host.RunAndBlock();
            }
        }
    }
}