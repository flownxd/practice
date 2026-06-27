using Xunit;
using task05;
using System;

namespace task05tests;

public class TestClass
{
    public int PublicField;
    private string _privateField = "test";   
    public int Property { get; set; }

    public void Method() { }
    
    public string GetPrivateField() => _privateField;
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
        Assert.Contains("PublicField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_ReturnsTrueForAttributedClass()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        var hasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(hasAttribute);
    }

    [Fact]
    public void HasAttribute_ReturnsFalseForNonAttributedClass()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var hasAttribute = analyzer.HasAttribute<SerializableAttribute>();

        Assert.False(hasAttribute);
    }

    [Fact]
    public void GetMethodParams_ReturnsParametersAndReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methodParams = analyzer.GetMethodParams("Method");

        Assert.Contains("return Void", methodParams);
    }

    [Fact]
    public void GetMethodParams_ReturnsEmptyForNonExistentMethod()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methodParams = analyzer.GetMethodParams("NonExistentMethod");

        Assert.Empty(methodParams);
    }

    [Fact]
    public void GetPublicMethods_IncludesPropertyAccessors()
    {   
    var analyzer = new ClassAnalyzer(typeof(TestClass));
    var methods = analyzer.GetPublicMethods().ToList();

    Assert.Contains("Method", methods);
    Assert.Contains("get_Property", methods);
    Assert.Contains("set_Property", methods);
    Assert.Contains("GetPrivateField", methods);
    Assert.Equal(4, methods.Count);
    }

   [Fact]
    public void GetAllFields_Count_IsCorrect()
    {
    var analyzer = new ClassAnalyzer(typeof(TestClass));
    var fields = analyzer.GetAllFields().ToList();

    Assert.Equal(3, fields.Count);
    }

    [Fact]
    public void GetProperties_Count_IsCorrect()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties().ToList();

        Assert.Single(properties); 
    }
}