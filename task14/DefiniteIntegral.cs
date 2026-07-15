using System.Threading;

namespace task14;

public static class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> func, double step, int threadCount)
    {
        if (step <= 0) throw new ArgumentException("Шаг должен быть > 0", nameof(step));
        if (threadCount < 1) throw new ArgumentException("Потоков должно быть >= 1", nameof(threadCount));

        if (threadCount == 1)
        {
            return SolveSingleThread(a, b, func, step);
        }

        double[] partialResults = new double[threadCount];
        Thread[] workers = new Thread[threadCount];
        
        double range = b - a;
        double chunkSize = range / threadCount;

        for (int i = 0; i < threadCount; i++)
        {
            double start = a + i * chunkSize;
            double end = (i == threadCount - 1) ? b : start + chunkSize;
            
            int index = i; 

            workers[i] = new Thread(() =>
            {
                partialResults[index] = CalculateSegment(start, end, func, step);
            });
            
            workers[i].Start();
        }

        foreach (var t in workers) t.Join();

        double totalSum = 0;
        for (int i = 0; i < partialResults.Length; i++)
        {
            totalSum += partialResults[i];
        }

        return totalSum;
    }

    public static double SolveSingleThread(double a, double b, Func<double, double> func, double step)
    {
        return CalculateSegment(a, b, func, step);
    }

    private static double CalculateSegment(double from, double to, Func<double, double> f, double h)
    {
        double sum = 0;
        double current = from;
        double yLeft = f(current); 

        while (current < to)
        {
            double next = current + h;
            if (next > to) next = to;

            double yRight = f(next);
            sum += (yLeft + yRight) * h / 2.0;

            current = next;
            yLeft = yRight; 
        }
        
        return sum;
    }
}