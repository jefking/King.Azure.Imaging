//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using King.Azure.Imaging.Models;
using Microsoft.Azure.WebJobs;

namespace King.Azure.Imaging.WebJob
{
    public class Functions
    {
        private static readonly IVersions versions = new Versions();
        private static readonly string connectionString = "";

        /// <summary>
        /// Image Processing
        /// </summary>
        /// <param name="image">image</param>
        public static void ImageProcessing([QueueTrigger("imaging")] ImageQueued image)
        {
            var processor = new Processor(new DataStore(connectionString), versions.Images);
            processor.Process(image).Wait();
        }
    }
}