using Xunit;
using System.Reflection; 
using task07;
using System;
using System.IO;

namespace task07tests;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
    public void PrintTypeInfo_OutputsCorrectInfo()
    {
    var result = ReflectionHelper.PrintTypeInfo(typeof(SampleClass));
    
    Assert.Contains("Пример класса", result);
    Assert.Contains("1.0", result);
    Assert.Contains("Тестовый метод", result);
    Assert.Contains("Числовое свойство", result);
    }

    [Fact]
    public void DisplayNameAttribute_CanBeAppliedToClass()
    {
        var attr = new DisplayNameAttribute("Test Name");
        Assert.Equal("Test Name", attr.DisplayName);
    }

    [Fact]
    public void VersionAttribute_ToString_ReturnsCorrectFormat()
    {
        var attr = new VersionAttribute(2, 5);
        Assert.Equal("2.5", attr.ToString());
    }

    [Fact]
    public void VersionAttribute_CanBeAppliedToClass()
    {
        var attr = new VersionAttribute(3, 2);
        Assert.Equal(3, attr.Major);
        Assert.Equal(2, attr.Minor);
    }

    [Fact]
    public void SampleClass_HasMultipleAttributes()
    {
        var type = typeof(SampleClass);
        
        var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
        var version = type.GetCustomAttribute<VersionAttribute>();
        
        Assert.NotNull(displayName);
        Assert.NotNull(version);
    }

    [Fact]
    public void Method_WithoutAttributeName_IsNull()
    {
        var method = typeof(SampleClass).GetMethod("AnotherMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        
        Assert.Null(attribute);
    }

    [Fact]
    public void Property_WithoutAttributeName_IsNull()
    {
        var prop = typeof(SampleClass).GetProperty("Name");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        
        Assert.Null(attribute);
    }
}