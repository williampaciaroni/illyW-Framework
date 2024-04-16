using FluentAssertions;
using illyW.Framework.Core.ResultPattern;
using illyW.Framework.Tests.Shared.Attributes;

namespace illyW.Framework.Tests.Core;

public class ResultFixture
{
    [Fact]
    public void Result_Succeed_Success()
    {
        Result r = new();
        
        r.Succeed();

        r.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Result_Fail_Success()
    {
        Result r = new();
        
        r.Fail();

        r.IsSuccessful.Should().BeFalse();
    }
    
    [Fact]
    public void Result_FailNoError_Success()
    {
        Result r = new();
        
        r.Fail();

        r.IsSuccessful.Should().BeFalse();
        r.Errors.Should().BeEmpty();
    }
    
    [Theory]
    [DefaultAutoData]
    public void Result_FailWithError_Success(string error)
    {
        Result r = new();
        
        r.Fail();
        r.AddError(error);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.First().Should().Be(error);
    }
    
    [Theory]
    [DefaultAutoData]
    public void Result_FailWithErrors_Success(List<string> errors)
    {
        Result r = new();
        
        r.Fail();
        r.AddErrors(errors);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.Should().Equal(errors);
    }
    
    [Theory]
    [DefaultAutoData]
    public void Result_GetData(int data)
    {
        Result<int> r = new();
        
        r.Succeed();
        r.Data = data;

        r.Data.Should().Be(data);
    }
}