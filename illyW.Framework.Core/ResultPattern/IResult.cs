using System;
using System.Collections.Generic;

namespace illyW.Framework.Core.ResultPattern;

public interface IResult
{
    IReadOnlyCollection<string> Errors { get; }

    bool IsSuccessful { get; }

    void Succeed();
    void Fail(string error = null);
}

public interface IResult<T> : IResult
{
    T Data { get; }
    
    void Succeed(T data);
}