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
        public static double EPS { get; set; } = 0.0001;

        public static double GetLeibniz()
        {
            
            double x = 0;
            for (int i = 0; i < N; i++)
            {
                var sign = i % 2 == 0 ? 1 : -1;
                x += (double)sign / (2 * i + 1);
            }
            return x*4;
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
