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

using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using King.Azure.Imaging;

namespace King.Azure.Imaging.WebJob
{
    public class Functions
    {
        /// <summary>
        /// Reads an Order object from the "initialorder" queue
        /// Creates a blob for the specified order which contains the order details
        /// The message in "orders" will be picked up by "QueueToBlob"
        /// </summary>
        public static void MultipleOutput([QueueTrigger("initialorder")] object image)
        {
            //var processor = new Processor();
        }
    }
}