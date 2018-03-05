using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    static class ParallelAlgorithms
    {
        public static int CountOfThreads{get;set;}
        public static int N { get; set; }

        private static double[] _values;
        public delegate double CalcMethod(int start,int finish);
        private static CalcMethod _calcMethod;

        public static double GetPI(CalcMethod method)
        {
            _calcMethod = method;
            int groups = (N % CountOfThreads == 0) ? CountOfThreads : CountOfThreads + 1;
            Thread[] threads = new Thread[groups];
            _values = new double[groups];
            for (int k = 0; k < groups; k++)
            {
                threads[k] = new Thread(Body);
                threads[k].Start(k);
            }
            for (int k = 0; k < groups; k++)
                threads[k].Join();
            if(_calcMethod==ASinMethod)
                return _values.Average();
            if (_calcMethod == LeibnizMethod)
                return _values.Sum()*4;
            if (_calcMethod == IntegralMethod)
                return _values.Sum();
            if (_calcMethod == MonteCarloMethod)
                return 4 * _values.Sum() / N;
            return -1;
        }
        static void Body(object group)
        {
            int k = (int)group;
            int m = N / CountOfThreads;
            int start = k * m;
            int gr = N % CountOfThreads == 0 ? CountOfThreads: CountOfThreads + 1;
            int finish = (k != gr - 1) ? start + m - 1 : N - 1;
            _values[k] = _calcMethod(start, finish);
        }
        public static double ASinMethod(int start,int finish)
        {
            double res = 0;
            var x = 0.5;
            for (int i = start; i < finish; i++)
            {
                res= 2 * (Math.Asin(Math.Sqrt(1 - Math.Pow(x, 2))) + Math.Abs(Math.Asin(x)));
            }
            return res;
        }
        public static double LeibnizMethod(int start, int finish)
        {
            double res = 0;
            for (int i = start; i < finish; i++)
            {
                var sign = i % 2 == 0 ? 1 : -1;
                res += (double)sign / (2 * i + 1);                
            }
            return res;
        }

        public static double IntegralMethod(int start,int finish)
        {
            double sum = 0;
            double step = 1.0 / N;
            for (int i = start; i < finish; i++)
            {
                var x = (i + 0.5) * step;
                sum += 4 / (1 + x * x);
            }
            sum *= step;
            return sum;
        }

        public static double MonteCarloMethod(int start,int finish)
        {
            double a = 0.5;
            Random rnd = new Random(start);
            int counter = 0;
            for (int i = start; i < finish; i++)
            {
                var x = rnd.NextDouble()*a;
                var y = rnd.NextDouble()*a;
                if (y * y <= Circle(x, a))
                    counter++;
            }
            return counter;
        }
        private static double Circle(double x, double radius)
        {
            return radius * radius - x * x;
        }
    }
}
