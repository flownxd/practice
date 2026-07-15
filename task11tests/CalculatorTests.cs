using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void CreateCalculator_ShouldExecuteAddCorrectly()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(15, calculator.Add(12, 3));
    }

    [Fact]
    public void CreateCalculator_ShouldExecuteMinusCorrectly()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(7, calculator.Minus(10, 3));
    }

    [Fact]
    public void CreateCalculator_ShouldExecuteMulCorrectly()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(18, calculator.Mul(2, 9));
    }

    [Fact]
    public void CreateCalculator_ShouldExecuteDivCorrectly()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(2, calculator.Div(6, 3));
    }

    [Fact]
    public void CreateCalculator_ShouldWorkWithNegativeNumbers()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(-5, calculator.Add(-2, -3));
        Assert.Equal(5, calculator.Minus(2, -3));
        Assert.Equal(6, calculator.Mul(-2, -3));
    }

    [Fact]
    public void CreateCalculator_ShouldWorkWithZero()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Equal(5, calculator.Add(5, 0));
        Assert.Equal(5, calculator.Minus(5, 0));
        Assert.Equal(0, calculator.Mul(5, 0));
    }

    [Fact]
    public void CreateCalculator_ShouldThrowOnDivByZero()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.Throws<DivideByZeroException>(() => calculator.Div(10, 0));
    }

    [Fact]
    public void CreateCalculator_ShouldImplementICalculatorInterface()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        Assert.IsAssignableFrom<ICalculator>(calculator);
    }

    [Fact]
    public void CreateCalculator_ShouldReturnNewInstanceEachTime()
    {
        ICalculator calc1 = CalculatorGenerator.CreateCalculator();
        ICalculator calc2 = CalculatorGenerator.CreateCalculator();
        Assert.NotSame(calc1, calc2);
    }

    [Fact]
    public void CreateCalculator_AllMethodsWorkTogether()
    {
        ICalculator calculator = CalculatorGenerator.CreateCalculator();
        
        Assert.Equal(15, calculator.Add(12, 3));
        Assert.Equal(7, calculator.Minus(10, 3));
        Assert.Equal(18, calculator.Mul(2, 9));
        Assert.Equal(2, calculator.Div(6, 3));
    }
}