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
        if (IsSuccessful)
        {
            throw new Exception("Can not add an error if the result is not in Failed state.");
        }

        InternalGetErrors.Add(errorMessage);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        if (IsSuccessful)
        {
            throw new Exception("Can not add an error if the result is not in Failed state.");
        }
        
        InternalGetErrors.AddRange(errors);
    }

    public void Succeed()
    {
        IsSuccessful = true;
        
        InternalGetErrors.Clear();
    }

    public void Fail(string error = null)
    {
        IsSuccessful = false;

        if (error != null)
        {
            AddError(error);
        }
    }
    
    private List<string> InternalGetErrors => InternalErrors ??= new List<string>();
}

public class Result<T> : Result, IResult<T>
{
    public T Data { get; set; }
    
    public void Succeed(T data)
    {
        Succeed();
        
        Data = data;
    }
}