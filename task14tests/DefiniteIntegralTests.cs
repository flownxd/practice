using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Sin_Integral_SymmetricRange_ShouldBeZero()
    {
        double result = DefiniteIntegral.Solve(-1, 1, Math.Sin, 1e-4, 4);
        Assert.Equal(0, result, precision: 3);
    }

    [Fact]
    public void X_Integral_0_to_5_ShouldBe12_5()
    {
        double result = DefiniteIntegral.Solve(0, 5, x => x, 1e-5, 8);
        Assert.Equal(12.5, result, precision: 2);
    }

    [Fact]
    public void MultiThread_ShouldMatchSingleThread_Accuracy()
    {
        double r1 = DefiniteIntegral.Solve(0, 10, Math.Cos, 1e-4, 1);
        double r8 = DefiniteIntegral.Solve(0, 10, Math.Cos, 1e-4, 8);
        Assert.Equal(r1, r8, precision: 2);
    }

    [Fact]
    public void InvalidStep_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => 
            DefiniteIntegral.Solve(0, 1, x => x, -1, 1));
    }
}