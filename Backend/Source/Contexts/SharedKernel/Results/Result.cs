namespace SharedKernel.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, IEnumerable<Error>? errors)
        {
            IsSuccess = isSuccess;
            Errors = [.. (errors ?? [])];
        }

        public static Result Success()
            => new(true, []);

        public static Result Failure(params Error[] errors)
            => new(false, errors);

        public static Result Failure(IEnumerable<Error> errors)
            => new(false, errors);

        public static Result Combine(params Result[] results)
        {
            var errors = results
                .Where(r => r.IsFailure)
                .SelectMany(r => r.Errors)
                .ToArray();

            return errors.Length == 0
                ? Success()
                : Failure(errors);
        }
    }
}