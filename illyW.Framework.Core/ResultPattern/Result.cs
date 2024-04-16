using System;
using System.Collections.Generic;

namespace illyW.Framework.Core.ResultPattern;

public class Result : IResult
{
    private List<string> InternalErrors { get; set; }

    public IReadOnlyCollection<string> Errors => InternalGetErrors.AsReadOnly();
    public bool IsSuccessful { get; internal set; } = true;
    
    public void AddError(string errorMessage)
    {
        InternalGetErrors.Add(errorMessage);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        InternalGetErrors.AddRange(errors);
    }

    public void Succeed()
    {
        IsSuccessful = true;
    }

    public void Fail()
    {
        IsSuccessful = false;
    }
    
    private List<string> InternalGetErrors => InternalErrors ??= new List<string>();
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; set; }
}