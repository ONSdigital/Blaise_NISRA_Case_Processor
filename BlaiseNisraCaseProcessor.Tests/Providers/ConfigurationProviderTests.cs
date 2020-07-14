﻿using BlaiseNisraCaseProcessor.Providers;
using NUnit.Framework;

namespace BlaiseNisraCaseProcessor.Tests.Providers
{
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        [Test]
        public void Given_I_Call_ProjectId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.ProjectId;

            //assert
            Assert.AreEqual("ProjectIdTest", result);
        }

        [Test]
        public void Given_I_Call_PublishTopicId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.PublishTopicId;

            //assert
            Assert.AreEqual("PublishTopicIdTest", result);
        }

        [Test]
        public void Given_I_Call_SubscriptionTopicId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.SubscriptionTopicId;

            //assert
            Assert.AreEqual("SubscriptionTopicIdTest", result);
        }

        [Test]
        public void Given_I_Call_SubscriptionId_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.SubscriptionId;

            //assert
            Assert.AreEqual("SubscriptionIdTest", result);
        }

        [Test]
        public void Given_I_Call_BucketName_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.BucketName;

            //assert
            Assert.AreEqual("BucketNameTest", result);
        }

        [Test]
        public void Given_I_Call_LocalProcessFolder_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.LocalProcessFolder;

            //assert
            Assert.AreEqual("LocalProcessFolderTest", result);
        }

        [Test]
        public void Given_I_Call_CloudStorageKey_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.CloudStorageKey;

            //assert
            Assert.AreEqual("CloudStorageKeyTest", result);
        }
    }
}
