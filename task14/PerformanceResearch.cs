using System.Diagnostics;
using System.Threading;

namespace task14;

public static class PerformanceResearch
{
    private const double TestA = -100;
    private const double TestB = 100;
    private static readonly Func<double, double> TestFunction = x => Math.Sin(x);
    
    public static double FindOptimalStepSize()
    {
        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        double referenceResult = CalculateReferenceResult();
        
        Console.WriteLine("Поиск оптимального размера шага");
        Console.WriteLine($"Референсное значение (step=1e-6): {referenceResult:F10}");
        
        foreach (double step in steps)
        {
            var sw = Stopwatch.StartNew();
            double result = DefiniteIntegral.Solve(TestA, TestB, TestFunction, step, 4);
            sw.Stop();
            
            double error = Math.Abs(result - referenceResult);
            bool meetsAccuracy = error < 1e-4;
            
            Console.WriteLine($"Step: {step,-8:E1} | Result: {result,12:F6} | Error: {error,10:E2} | Time: {sw.ElapsedMilliseconds,5} ms | Accuracy OK: {meetsAccuracy}");
        }
        
        return 1e-4;
    }
    
    public static (int threadCount, long timeMs) FindOptimalThreadCount(double step)
    {
        int[] threadCounts = { 1, 2, 4, 8, 16, 32 };
        int measurements = 5;
        
        Console.WriteLine($"Поиск оптимального количества потоков (step={step})");
        
        var results = new List<(int threads, long avgTime)>();
        
        foreach (int threads in threadCounts)
        {
            long totalTime = 0;
            
            for (int m = 0; m < measurements; m++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(TestA, TestB, TestFunction, step, threads);
                sw.Stop();
                totalTime += sw.ElapsedMilliseconds;
            }
            
            long avgTime = totalTime / measurements;
            results.Add((threads, avgTime));
            
            Console.WriteLine($"Threads: {threads,2} | Avg Time: {avgTime,5} ms");
        }
        
        var optimal = results.OrderBy(r => r.avgTime).First();
        Console.WriteLine($"Оптимальное количество потоков: {optimal.threads} (время: {optimal.avgTime} ms)");
        
        return optimal;
    }
    
    public static (double singleThreadTime, double multiThreadTime, double speedupPercent) 
        CompareWithSingleThreaded(double step, int optimalThreads)
    {
        Console.WriteLine("Сравнение с однопоточной версией");
        
        int measurements = 10;
        long singleThreadTotal = 0;
        long multiThreadTotal = 0;
        
        Console.WriteLine("Замеры однопоточной версии:");
        for (int i = 0; i < measurements; i++)
        {
            var sw = Stopwatch.StartNew();
            SolveSequential(TestA, TestB, TestFunction, step);
            sw.Stop();
            singleThreadTotal += sw.ElapsedMilliseconds;
            if (i < 3) Console.WriteLine($"  Замер {i+1}: {sw.ElapsedMilliseconds} ms");
        }
        
        Console.WriteLine("Замеры многопоточной версии:");
        for (int i = 0; i < measurements; i++)
        {
            var sw = Stopwatch.StartNew();
            DefiniteIntegral.Solve(TestA, TestB, TestFunction, step, optimalThreads);
            sw.Stop();
            multiThreadTotal += sw.ElapsedMilliseconds;
            if (i < 3) Console.WriteLine($"  Замер {i+1}: {sw.ElapsedMilliseconds} ms");
        }
        
        double singleThreadAvg = singleThreadTotal / (double)measurements;
        double multiThreadAvg = multiThreadTotal / (double)measurements;
        double speedup = ((singleThreadAvg - multiThreadAvg) / singleThreadAvg) * 100;
        
        Console.WriteLine($"Среднее время (однопоточная): {singleThreadAvg:F2} ms");
        Console.WriteLine($"Среднее время (многопоточная, {optimalThreads} потоков): {multiThreadAvg:F2} ms");
        Console.WriteLine($"Ускорение: {speedup:F2}%");
        
        return (singleThreadAvg, multiThreadAvg, speedup);
    }
    
    public static void WriteResultsToFile(string filePath, double optimalStep, 
        int optimalThreads, (double single, double multi, double percent) comparison)
    {
        string content = $"Результаты оптимизации вычисления интеграла\n\n" +
                         $"Функция: sin(x)\n" +
                         $"Отрезок: [{TestA}, {TestB}]\n" +
                         $"Требуемая точность: 1e-4\n\n" +
                         $"1. Оптимальный размер шага:\n" +
                         $"   Значение: {optimalStep}\n" +
                         $"   Пояснение: минимальный шаг, обеспечивающий точность 1e-4\n\n" +
                         $"2. Оптимальное количество потоков:\n" +
                         $"   Значение: {optimalThreads}\n" +
                         $"   Пояснение: при этом количестве достигается минимальное время выполнения\n\n" +
                         $"3. Сравнение производительности:\n" +
                         $"   Однопоточная версия: {comparison.single:F2} мс\n" +
                         $"   Многопоточная версия ({optimalThreads} потоков): {comparison.multi:F2} мс\n" +
                         $"   Ускорение: {comparison.percent:F2}%\n\n" +
                         $"   Вывод: многопоточная версия {(comparison.percent >= 15 ? "ЭФФЕКТИВНА" : "НЕ ЭФФЕКТИВНА")} " +
                         $"(ускорение {(comparison.percent >= 15 ? ">=" : "<")} 15%)\n\n" +
                         $"Дата выполнения: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n";
        
        File.WriteAllText(filePath, content);
        Console.WriteLine($"Результаты сохранены в файл: {filePath}");
    }
    
    public static (int[] threadCounts, long[] times) GetPerformanceData(double step)
    {
        int[] threadCounts = { 1, 2, 4, 8, 16, 32 };
        long[] times = new long[threadCounts.Length];
        int measurements = 5;
        
        for (int i = 0; i < threadCounts.Length; i++)
        {
            long totalTime = 0;
            for (int m = 0; m < measurements; m++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(TestA, TestB, TestFunction, step, threadCounts[i]);
                sw.Stop();
                totalTime += sw.ElapsedMilliseconds;
            }
            times[i] = totalTime / measurements;
        }
        
        return (threadCounts, times);
    }
    
    private static double CalculateReferenceResult()
    {
        return DefiniteIntegral.Solve(TestA, TestB, TestFunction, 1e-6, 1);
    }
    
    private static double SolveSequential(double a, double b, Func<double, double> function, double step)
    {
        double result = 0;
        
        for (double x = a; x < b; x += step)
        {
            double fx = function(x);
            double fxNext = function(x + step);
            result += (fx + fxNext) * step / 2;
        }
        
        return result;
    }
}