using Microsoft.Extensions.Logging;
using Moq;

namespace GHD_WebAPI.Tests.TestHelpers
{
    public static class LoggerExtensions
    {
        public static void VerifyLogError<TModel, TException>(
            this Mock<ILogger<TModel>> loggerMock,
            string expectedMessage,
            Times times) where TException : Exception
        {
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString() == expectedMessage),
                    It.Is<Exception>(ex => ex is TException),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times
            );
        }

        public static void VerifyLogWarning<TModel, TException>(
            this Mock<ILogger<TModel>> loggerMock,
            string expectedMessage,
            Times times) where TException : Exception
        {
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString() == expectedMessage),
                    It.Is<Exception>(ex => ex is TException),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times
            );
        }

        public static void VerifyLogWarning<TModel>(
            this Mock<ILogger<TModel>> loggerMock,
            string expectedMessage,
            Times times)
        {
            loggerMock.Verify(
                x => x.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, _) => v.ToString() == expectedMessage),
                    It.Is<Exception>(ex => ex == null),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                times
            );
        }

    }
}
