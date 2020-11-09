﻿using System;
using System.Collections.Generic;
using Blaise.Case.Nisra.Processor.Enums;
using Blaise.Case.Nisra.Processor.Interfaces.Mappers;
using Blaise.Case.Nisra.Processor.Interfaces.Services;
using Blaise.Case.Nisra.Processor.Models;
using log4net;
using Moq;
using NUnit.Framework;

namespace Blaise.Case.Nisra.Processor.Tests.Unit.MessageHandler
{
    public class MessageHandlerTests
    {
        private Mock<ILog> _loggingMock;
        private Mock<IBucketService> _bucketServiceMock;
        private Mock<IProcessFilesService> _processFilesServiceMock;
        private Mock<ICaseMapper> _mapperMock;

        private readonly string _message;
        private readonly NisraCaseActionModel _actionModel;
        private readonly List<string> _availableFilesFromBucket;
        private readonly List<string> _downloadedFilesFromBucket;

        private global::Blaise.Case.Nisra.Processor.MessageHandler.MessageHandler _sut;


        public MessageHandlerTests()
        {
            _message = "Message";
            _actionModel = new NisraCaseActionModel { Action = ActionType.Process};
            _availableFilesFromBucket = new List<string> { "File1.bdbx", "File2.bdbx" };
            _downloadedFilesFromBucket = new List<string> { "File1.bdbx", "File2.bdbx" };
        }

        [SetUp]
        public void SetUpTests()
        {
            _loggingMock = new Mock<ILog>();

            _bucketServiceMock = new Mock<IBucketService>();

            _processFilesServiceMock = new Mock<IProcessFilesService>();

            _mapperMock = new Mock<ICaseMapper>();
            _mapperMock.Setup(m => m.MapToNisraCaseActionModel(_message)).Returns(_actionModel);

            _sut = new global::Blaise.Case.Nisra.Processor.MessageHandler.MessageHandler(
                _loggingMock.Object,
                _bucketServiceMock.Object,
                _processFilesServiceMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void Given_Process_Action_Is_Not_Set_When_I_Call_HandleMessage_Then_True_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);

            _actionModel.Action = ActionType.NotSupported;

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_Process_Action_Is_Not_Set_When_I_Call_HandleMessage_Then_Nothing_Is_Processed()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);

            _actionModel.Action = ActionType.NotSupported;

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.VerifyNoOtherCalls();
            _processFilesServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_Files_Are_Available_When_I_Call_HandleMessage_Then_True_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_Files_Are_Available_When_I_Call_HandleMessage_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.Verify(v => v.GetListOfAvailableFilesInBucket(), Times.Once);
            _bucketServiceMock.Verify(v => v.DownloadFilesFromBucket(_availableFilesFromBucket), Times.Once);
            _processFilesServiceMock.Verify(v => v.ProcessFiles(_downloadedFilesFromBucket), Times.Once);
            _bucketServiceMock.Verify(v => v.MoveProcessedFilesToProcessedFolder(_availableFilesFromBucket));

            _bucketServiceMock.VerifyNoOtherCalls();
            _processFilesServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_No_Files_Are_Available_in_Bucket_When_I_Call_HandleMessage_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(new List<string>());

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.Verify(v => v.GetListOfAvailableFilesInBucket(), Times.Once);
            _processFilesServiceMock.Verify(v => v.ProcessFiles(_availableFilesFromBucket), Times.Never);
        }

        [Test]
        public void Given_No_Files_Are_Available_in_Bucket_When_I_Call_HandleMessage_Then_True_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(new List<string>());

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_No_Files_Are_Available_When_I_Call_HandleMessage_Then_Nothing_Is_Processed()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(new List<string>());

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.Verify(v => v.GetListOfAvailableFilesInBucket(), Times.Once);

            _bucketServiceMock.VerifyNoOtherCalls();
            _processFilesServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_No_Files_Are_Downloaded_From_The_Bucket_When_I_Call_HandleMessage_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(new List<string>());

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.Verify(v => v.GetListOfAvailableFilesInBucket(), Times.Once);
            _processFilesServiceMock.Verify(v => v.ProcessFiles(_availableFilesFromBucket), Times.Never);
        }

        [Test]
        public void Given_No_Files_Are_Downloaded_From_The_Bucket_When_I_Call_HandleMessage_Then_True_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(new List<string>());

            //act
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_No_Files_Are_Downloaded_From_The_Bucket_When_I_Call_HandleMessage_Then_Nothing_Is_Processed()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(new List<string>());

            //act
            _sut.HandleMessage(_message);

            //assert
            _bucketServiceMock.Verify(v => v.GetListOfAvailableFilesInBucket(), Times.Once);
            _bucketServiceMock.Verify(v => v.DownloadFilesFromBucket(_availableFilesFromBucket), Times.Once);

            _bucketServiceMock.VerifyNoOtherCalls();
            _processFilesServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_An_Error_Occurs_When_Getting_Files_From_The_Bucket_When_I_Call_HandleMessage_Then_False_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Throws(new Exception());

            //act 
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_An_Error_Occurs_When_Getting_Files_From_The_Bucket_When_I_Call_HandleMessage_Then_Exception_Is_Handled_Correctly()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Throws(new Exception());

            //act && assert
            Assert.DoesNotThrow(() => _sut.HandleMessage(_message));
        }

        [Test]
        public void Given_An_Error_Occurs_When_Getting_Files_From_The_Bucket_When_I_Call_HandleMessage_Then_Nothing_Is_Processed()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Throws(new Exception());

            //act && assert
            _sut.HandleMessage(_message);

            _processFilesServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Given_An_Error_Occurs_When_Processing_Files_When_I_Call_HandleMessage_Then_Exception_Is_Handled_Correctly()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);
            _processFilesServiceMock.Setup(c => c.ProcessFiles(_downloadedFilesFromBucket)).Throws(new Exception());

            //act && assert
            Assert.DoesNotThrow(() => _sut.HandleMessage(_message));
        }

        [Test]
        public void Given_An_Error_Occurs_When_Processing_Files_When_I_Call_HandleMessage_Then_False_Is_Returned()
        {
            //arrange
            _bucketServiceMock.Setup(c => c.GetListOfAvailableFilesInBucket()).Returns(_availableFilesFromBucket);
            _bucketServiceMock.Setup(c => c.DownloadFilesFromBucket(_availableFilesFromBucket)).Returns(_downloadedFilesFromBucket);
            _processFilesServiceMock.Setup(c => c.ProcessFiles(_downloadedFilesFromBucket)).Throws(new Exception());

            //act 
            var result = _sut.HandleMessage(_message);

            //assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result);
        }
    }
}