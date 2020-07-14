﻿using System;
using System.Collections.Generic;
using System.Configuration;
using BlaiseNisraCaseProcessor.Interfaces.Providers;

namespace BlaiseNisraCaseProcessor.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public string ProjectId => ConfigurationManager.AppSettings["ProjectId"];

        public string SubscriptionId => ConfigurationManager.AppSettings["SubscriptionId"];

        public string PublishTopicId => ConfigurationManager.AppSettings["PublishTopicId"];

        public string SubscriptionTopicId => ConfigurationManager.AppSettings["SubscriptionTopicId"];

        public string LocalProcessFolder => ConfigurationManager.AppSettings["LocalProcessFolder"];

        public string BucketName => ConfigurationManager.AppSettings["BucketName"];

        public IList<string> IgnoreFilesInBucketList
        {
            get
            {
                var filesToIgnore = ConfigurationManager.AppSettings["IgnoreFilesInBucketList"];

                if (string.IsNullOrWhiteSpace(filesToIgnore))
                {
                    return new List<string>();
                }

                return filesToIgnore.Split(',');
            }
        } 

        public string CloudStorageKey => ConfigurationManager.AppSettings["CloudStorageKey"];

        public string VmName => Environment.MachineName;
    }
}
