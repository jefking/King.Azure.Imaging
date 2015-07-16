namespace King.Azure.Imaging.WebJob
{
    using System;
    using King.Azure.Imaging.Models;
    using King.Azure.Imaging.Tasks;
    using Microsoft.Azure;
    using Microsoft.Azure.WebJobs;

    public class Program
    {
        public static void Main()
        {
            var webJobsDashboard = CloudConfigurationManager.GetSetting("AzureWebJobsDashboard");
            var webJobsStorage = CloudConfigurationManager.GetSetting("AzureWebJobsStorage");
            var storageAcc = CloudConfigurationManager.GetSetting("StorageAccount");

            if (string.IsNullOrWhiteSpace(webJobsStorage) || string.IsNullOrWhiteSpace(storageAcc) || string.IsNullOrWhiteSpace(webJobsDashboard))
            {
                Console.WriteLine("Please add the Azure Storage account credentials ['StorageAccount' & 'AzureWebJobsStorage' & 'AzureWebJobsDashboard'] in App.config");
                Console.Read();
                return;
            }
            else
            {
                var config = new TaskConfiguration()
                {
                    ConnectionString = storageAcc,
                    StorageElements = new StorageElements(),
                };

                var factory = new ImageTaskFactory();
                foreach (var task in factory.Initialize(config))
                {
                    task.Start();
                }

                var host = new JobHost();
                host.RunAndBlock();
            }
        }
    }
}