using System;
using System.Collections.Generic;

namespace illyW.Framework.Core.ResultPattern;

public interface IResult
{
    IReadOnlyCollection<string> Errors { get; }

    bool IsSuccessful { get; }

    void Succeed();
    void Fail();
}

public interface IResult<out T> : IResult
{
    T Data { get; }
}