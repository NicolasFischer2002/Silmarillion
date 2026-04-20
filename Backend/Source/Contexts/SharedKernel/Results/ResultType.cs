namespace SharedKernel.Results
{
    public sealed class Result<T> : Result
    {
        private readonly T? _value;

        public T Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access Value on a failed Result.");

        private Result(T value) : base(true, [])
        {
            _value = value;
        }

        private Result(IEnumerable<Error> errors) : base(false, errors)
        {
        }

        public static Result<T> Success(T value)
            => new(value);

        public static new Result<T> Failure(params Error[] errors)
            => new(errors);

        public static new Result<T> Failure(IEnumerable<Error> errors)
            => new(errors);

        public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
            => IsSuccess
                ? Result<TOut>.Success(mapper(Value))
                : Result<TOut>.Failure(Errors);

        public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
            => IsSuccess
                ? binder(Value)
                : Result<TOut>.Failure(Errors);

        public TResult Match<TResult>(
            Func<T, TResult> onSuccess,
            Func<IReadOnlyList<Error>, TResult> onFailure)
            => IsSuccess ? onSuccess(Value) : onFailure(Errors);
    }
}