using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PiCalculator
{
    class MainWindowManager : ViewModelBase
    {
        public int CountOfIterations { get; set; } = 1000000;
        private string _linearValue;
        public string LinearValue { get { return _linearValue; }
            set
            { _linearValue = value;
                RaisePropertyChanged("LinearValue");
            } }
        public string LinearTime { get; set; }
        public string ParallelValue { get; set; }
        public string ParallelTime { get; set; }
        public RelayCommand LinearArcsinCommand { get; private set; }
        public RelayCommand ParallelArcsinCommand { get; private set; }
        public RelayCommand LinearIntegralCommand { get; private set; }
        public RelayCommand LinearMonteCarloCommand { get; private set; }
        public RelayCommand ParallelIntegralCommand { get; private set; }
        public RelayCommand ParallelMonteCarloCommand { get; private set; }

        public MainWindowManager()
        {
            LinearArcsinCommand = new RelayCommand(LinearArcsin);
            LinearIntegralCommand = new RelayCommand(LinearIntegral);
            LinearMonteCarloCommand = new RelayCommand(LinearMonteCarlo);
            ParallelArcsinCommand = new RelayCommand(ParallelArcsin);
            ParallelIntegralCommand = new RelayCommand(ParallelIntegral);
            ParallelMonteCarloCommand = new RelayCommand(ParallelMonteCarlo);
        }

        
        private void LinearArcsin()
        {
            var x = 0.5;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < CountOfIterations; i++)
            {
                LinearValue = (2 * (Math.Asin(Math.Sqrt(1 - Math.Pow(x, 2))) + Math.Abs(Math.Asin(x)))).ToString();
            }
            sw.Stop();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelArcsin()
        {
            var x = 0.5;
            Stopwatch sw = new Stopwatch();
            sw.Start();            
            Parallel.For(0, CountOfIterations, (i) => ParallelValue = (2 * (Math.Asin(Math.Sqrt(1 - Math.Pow(x, 2))) + Math.Abs(Math.Asin(x)))).ToString());
            sw.Stop();
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");

        }

        private void LinearIntegral()
        {
            double sum = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            double step = 1.0 / CountOfIterations;
            for (int i = 0; i < CountOfIterations; i++)
            {
                var x = (i + 0.5) * step;
                sum += 4 / (1 + x * x);
            }
            sum *= step;
            sw.Stop();
            LinearValue = $"{sum}";
            LinearTime = $"{sw.Elapsed.TotalSeconds}";
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelIntegral()
        {
            var sum = new double[] {0};
            var sw = new Stopwatch();
            sw.Start();
            var step = 1.0 / CountOfIterations;
            Parallel.For(0, CountOfIterations, i =>
            {
                var x = (i + 0.5) * step;
                lock (sum)
                    sum[0] += 4.0 / (1 + x * x);
            });
            sum[0] *= step;
            sw.Stop();
            ParallelValue = sum[0].ToString();
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");
        }
       
        private void LinearMonteCarlo()
        {
            double a = 1000;
            Random rnd = new Random();
            int counter = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < CountOfIterations; i++)
            {
                var x = rnd.Next(-500, 500);
                var y = rnd.Next(-500, 500);
                if (y * y <= Circle(x, a/2))
                    counter++;
            }
            sw.Stop();
            LinearValue = (4 * counter / (double)CountOfIterations).ToString();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");

        }
        private void ParallelMonteCarlo()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int counter = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();            
            Parallel.For(0, CountOfIterations, (i) =>
            {
                var x = rnd.NextDouble();
                var y = rnd.NextDouble();
                if (y * y <= Circle(x, 0.5))
                    counter++;
            });
            sw.Stop();
            ParallelValue = (4 * counter / (double)CountOfIterations).ToString();
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");

        }



        private double Circle(double x,double radius)
        {
            return radius * radius - x * x;
        }
    }
}
