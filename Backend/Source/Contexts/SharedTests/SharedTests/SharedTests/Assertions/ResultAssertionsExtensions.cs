using SharedKernel.Results;

namespace SharedTests.Assertions
{
    public static class ResultAssertionsExtensions
    {
        public static void ShouldFailWith<T>(this Result<T> result, Error expectedError)
        {
            Assert.IsTrue(result.IsFailure);
            Assert.IsNotNull(result.Errors);
            Assert.HasCount(1, result.Errors);
            Assert.AreEqual(expectedError, result.Errors[0]);
        }

        public static void ShouldSucceed<T>(this Result<T> result)
        {
            Assert.IsTrue(result.IsSuccess);
        }
    }
}