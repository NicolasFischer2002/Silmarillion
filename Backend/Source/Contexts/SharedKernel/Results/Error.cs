namespace SharedKernel.Results
{
    public sealed record Error(string Code, string Message, ErrorType Type)
    {
        public static Error Validation(string code, string message)
            => new(code, message, ErrorType.Validation);

        public static Error Conflict(string code, string message)
            => new(code, message, ErrorType.Conflict);

        public static Error NotFound(string code, string message)
            => new(code, message, ErrorType.NotFound);

        public static Error BusinessRule(string code, string message)
            => new(code, message, ErrorType.BusinessRule);

        public static Error Unexpected(string code, string message)
            => new(code, message, ErrorType.Unexpected);
    }
}