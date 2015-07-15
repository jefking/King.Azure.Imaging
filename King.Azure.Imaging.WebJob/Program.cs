namespace King.Azure.Imaging.WebJob
{
    using System;
    using System.Configuration;
    using Microsoft.Azure.WebJobs;

    public class Program
    {
        public static void Main()
        {
            var storageAcc = ConfigurationManager.ConnectionStrings["StorageAccount"].ConnectionString;
            
            if (string.IsNullOrWhiteSpace(storageAcc))
            {
                Console.WriteLine("Please add the Azure Storage account credentials ['StorageAccount'] in App.config");
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