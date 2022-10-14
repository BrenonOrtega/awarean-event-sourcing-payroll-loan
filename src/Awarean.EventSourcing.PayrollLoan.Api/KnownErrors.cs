using Awarean.Sdk.Result;
using static Awarean.Sdk.Result.Error;

public static class KnownErrors
{
    public static readonly Error NullCommand = Create("NULL_COMMAND", "Received command is null");
    public static readonly Error InvalidCommand = Create("INVALID_COMMAND", "Received command is invalid");
}