using Xunit;
using task04;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }
  
   [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }
   
    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Cruiser_ShouldHaveHigherFirePowerThanFighter()
    {
        var cruiser = new Cruiser();
        var fighter = new Fighter();
        Assert.True(cruiser.FirePower > fighter.FirePower);
    }
    
    [Fact]
    public void Cruiser_Implements_ISpaceship()
    {
        var cruiser = new Cruiser();
        Assert.IsAssignableFrom<ISpaceship>(cruiser);
    }
    
    [Fact]
    public void Fighter_Implements_ISpaceship()
    {
        var fighter = new Fighter();
        Assert.IsAssignableFrom<ISpaceship>(fighter);
    }
    
    [Fact]
    public void Polymorphism_CanStoreAsISpaceship()
    {
        ISpaceship ship1 = new Cruiser();
        ISpaceship ship2 = new Fighter();
        
        Assert.Equal(50, ship1.Speed);
        Assert.Equal(100, ship2.Speed);
    }
   
    [Fact]
    public void Cruiser_Fire_DoesNotThrowException()
    {
        var cruiser = new Cruiser();
        var exception = Record.Exception(() => cruiser.Fire());
        Assert.Null(exception);
    }

    [Fact]
    public void Fighter_Fire_DoesNotThrowException()
    {
        var fighter = new Fighter();
        var exception = Record.Exception(() => fighter.Fire());
        Assert.Null(exception);
    }

    [Fact]
    public void Cruiser_MoveForward_DoesNotThrowException()
    {
        var cruiser = new Cruiser();
        var exception = Record.Exception(() => cruiser.MoveForward());
        Assert.Null(exception);
    }

    [Fact]
    public void Cruiser_Rotate_DoesNotThrowException()
    {
        var cruiser = new Cruiser();
        var exception = Record.Exception(() => cruiser.Rotate(90));
        Assert.Null(exception);
    }

    [Fact]
    public void Fighter_MoveForward_DoesNotThrowException()
    {
        var fighter = new Fighter();
        var exception = Record.Exception(() => fighter.MoveForward());
        Assert.Null(exception);
    }

    [Fact]
    public void Fighter_Rotate_DoesNotThrowException()
    {
        var fighter = new Fighter();
        var exception = Record.Exception(() => fighter.Rotate(90));
        Assert.Null(exception);
    }
}