using System.Threading;

namespace task14;

public static class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        double result = 0;
        object lockObject = new object();
        
        Barrier barrier = new Barrier(threadsNumber);
        
        Thread[] threads = new Thread[threadsNumber];
        double segmentLength = (b - a) / threadsNumber;
        
        for (int i = 0; i < threadsNumber; i++)
        {
            double segmentStart = a + i * segmentLength;
            double segmentEnd = segmentStart + segmentLength;
            
            Thread thread = new Thread(() =>
            {
                double localResult = 0;
                
                for (double x = segmentStart; x < segmentEnd; x += step)
                {
                    double fx = function(x);
                    double fxNext = function(x + step);
                    localResult += (fx + fxNext) * step / 2;
                }
                
                lock (lockObject)
                {
                    result += localResult;
                }
                
                barrier.SignalAndWait();
            });
            
            threads[i] = thread;
            thread.Start();
        }
        
        for (int i = 0; i < threadsNumber; i++)
        {
            threads[i].Join();
        }
        
        return result;
    }
}