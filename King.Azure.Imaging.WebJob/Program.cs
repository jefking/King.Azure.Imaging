namespace King.Azure.Imaging.WebJob
{
    using System;
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
                var host = new JobHost();
                host.RunAndBlock();
        }
    }
    }
}