using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_IntegralOfX_FromMinus1To1_With2Threads()
    {
        Func<double, double> func = x => x;
        double result = DefiniteIntegral.Solve(-1, 1, func, 1e-5, 2);
        Assert.Equal(0, result, 3);
    }
    
    [Fact]
    public void Solve_IntegralOfSin_FromMinus1To1_With8Threads()
    {
        Func<double, double> func = x => Math.Sin(x);
        double result = DefiniteIntegral.Solve(-1, 1, func, 1e-5, 8);
        Assert.Equal(0, result, 3);
    }
    
    [Fact]
    public void Solve_IntegralOfX_From0To5_With8Threads()
    {
        Func<double, double> func = x => x;
        double result = DefiniteIntegral.Solve(0, 5, func, 1e-6, 8);
        Assert.Equal(12.5, result, 4);
    }
    
    [Fact]
    public void Solve_IntegralOfConstantFunction()
    {
        Func<double, double> func = x => 2;
        double result = DefiniteIntegral.Solve(0, 3, func, 0.0001, 4);
        Assert.Equal(6, result, 3);
    }
    
    [Fact]
    public void Solve_IntegralOfX2_From0To1()
    {
        Func<double, double> func = x => x * x;
        double result = DefiniteIntegral.Solve(0, 1, func, 0.00001, 4);
        Assert.Equal(0.333, result, 2);
    }
    
    [Fact]
    public void Solve_WithSingleThread()
    {
        Func<double, double> func = x => x;
        double result = DefiniteIntegral.Solve(0, 2, func, 0.0001, 1);
        Assert.Equal(2, result, 3);
    }
    
    [Fact]
    public void Solve_WithMultipleThreads_ProducesConsistentResults()
    {
        Func<double, double> func = x => x * x + 1;
        
        double result1 = DefiniteIntegral.Solve(0, 4, func, 0.00001, 2);
        double result2 = DefiniteIntegral.Solve(0, 4, func, 0.00001, 4);
        double result3 = DefiniteIntegral.Solve(0, 4, func, 0.00001, 8);
        
        Assert.Equal(result1, result2, 3);
        Assert.Equal(result2, result3, 3);
    }
    
    [Fact]
    public void Solve_IntegralOfCos_From0ToPi()
    {
        Func<double, double> func = x => Math.Cos(x);
        double result = DefiniteIntegral.Solve(0, Math.PI, func, 0.00001, 4);
        Assert.Equal(0, result, 2);
    }
}