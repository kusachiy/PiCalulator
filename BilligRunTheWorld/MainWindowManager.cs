using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BilligRunTheWorld
{
    class MainWindowManager:ViewModelBase
    {
        const int COUNT = 100000;
        static int counter = 0;
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

        public MainWindowManager()
        {
            LinearArcsinCommand = new RelayCommand(LinearArcsin);
            LinearIntegralCommand = new RelayCommand(LinearIntegral);
            LinearMonteCarloCommand = new RelayCommand(LinearMonteCarlo);
            ParallelArcsinCommand = new RelayCommand(ParallelArcsin);
            ParallelIntegralCommand = new RelayCommand(ParallelIntegral);
        }

        
        private void LinearArcsin()
        {
            var x = 0.5;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < COUNT; i++)
            {
                LinearValue = (2 * (Math.Asin(Math.Sqrt(1 - Math.Pow(x, 2))) + Math.Abs(Math.Asin(x)))).ToString();
            }
            sw.Stop();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelArcsin()
        {
            
        }

        private void LinearIntegral()
        {
            double sum = 0;
            int N = 10000000;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < N; i++)
            {
                sum += Math.Pow(-1, i) / (2 * i + 1);
            }
            sw.Stop();
            LinearValue = (4*sum).ToString();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelIntegral()
        {
            int N = 10000000;
            double[] array = new double[N];
            Stopwatch sw = new Stopwatch();
            sw.Start();
            double s = 0;
            Parallel.For(0, N, (i) => s += Math.Pow(-1, i) / (2 * i + 1));
            sw.Stop();
            LinearValue = (4*s).ToString();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");
        }
       
        private void LinearMonteCarlo()
        {
            int N = 10000000;
            double a = 1000;
            Random rnd = new Random();
            int counter = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < N; i++)
            {
                var x = rnd.Next(-500, 500);
                var y = rnd.Next(-500, 500);
                if (y * y <= Circle(x, a/2))
                    counter++;
            }
            sw.Stop();
            LinearValue = (4 * counter / (double)N).ToString();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");

        }

        private double Circle(double x,double radius)
        {
            return radius * radius - x * x;
        }
    }
}
