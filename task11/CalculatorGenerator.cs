using System;
using System.Reflection;
using System.Reflection.Emit;

namespace task11;

public static class CalculatorGenerator
{
    public static ICalculator CreateCalculator()
    {
        var assemblyName = new AssemblyName("DynamicCalculator");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicCalculatorModule");
        
        var typeBuilder = moduleBuilder.DefineType("DynamicCalculator", 
            TypeAttributes.Public, 
            typeof(object), 
            new[] { typeof(ICalculator) });
        
        CreateMethod(typeBuilder, "Add", OpCodes.Add);
        CreateMethod(typeBuilder, "Minus", OpCodes.Sub);
        CreateMethod(typeBuilder, "Mul", OpCodes.Mul);
        CreateDivMethod(typeBuilder);
        
        var createdType = typeBuilder.CreateType();
        return (ICalculator)Activator.CreateInstance(createdType);
    }
    
    private static void CreateMethod(TypeBuilder typeBuilder, string methodName, OpCode opCode)
    {
        var methodBuilder = typeBuilder.DefineMethod(methodName,
            MethodAttributes.Public | MethodAttributes.Virtual,
            typeof(int),
            new[] { typeof(int), typeof(int) });
        
        ILGenerator il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldarg_2);
        il.Emit(opCode);
        il.Emit(OpCodes.Ret);
        
        typeBuilder.DefineMethodOverride(methodBuilder, typeof(ICalculator).GetMethod(methodName));
    }
    
    private static void CreateDivMethod(TypeBuilder typeBuilder)
    {
        var methodBuilder = typeBuilder.DefineMethod("Div",
            MethodAttributes.Public | MethodAttributes.Virtual,
            typeof(int),
            new[] { typeof(int), typeof(int) });
        
        ILGenerator il = methodBuilder.GetILGenerator();
        il.Emit(OpCodes.Ldarg_1);
        il.Emit(OpCodes.Ldarg_2);
        il.Emit(OpCodes.Div);
        il.Emit(OpCodes.Ret);
        
        typeBuilder.DefineMethodOverride(methodBuilder, typeof(ICalculator).GetMethod("Div"));
    }
}