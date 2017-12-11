using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiCalculator
{
    static class LinearAlgorithms
    {
        public static int N { get; set; }

        public static double GetArcSin()
        {
            var x = 0.5;
            double[] res = new double[N];
            for (int i = 0; i < N; i++)
            {
                res[i] = 2 * (Math.Asin(Math.Sqrt(1 - Math.Pow(x, 2))) + Math.Abs(Math.Asin(x)));
            }
            return res.Average();
        }

        public static double GetIntegral()
        {
            double sum = 0;
            double step = 1.0 / N;
            for (int i = 0; i < N; i++)
            {
                var x = (i + 0.5) * step;
                sum += 4 / (1 + x * x);
            }
            sum *= step;
            return sum;
        }

        public static double GetMonteCarlo()
        {
            double a = 1000;
            Random rnd = new Random();
            int counter = 0;           
            for (int i = 0; i < N; i++)
            {
                var x = rnd.Next(-500, 500);
                var y = rnd.Next(-500, 500);
                if (y * y <= Circle(x, a / 2))
                    counter++;
            }
            return (4 * counter / (double)N);           
        }
        private static double Circle(double x, double radius)
        {
            return radius * radius - x * x;
        }

    }
}
