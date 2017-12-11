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
        public int CountOfIterations { get; set; } = 10000000;
        public int CountOfThreads { get; set; } = 4;
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
            LinearAlgorithms.N = CountOfIterations;
            Stopwatch sw = new Stopwatch();
            sw.Start();            
            LinearValue = LinearAlgorithms.GetArcSin().ToString();          
            sw.Stop();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelArcsin()
        {
            ParallelAlgorithms.CountOfThreads = CountOfThreads;
            ParallelAlgorithms.N = CountOfIterations;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelValue = ParallelAlgorithms.GetPI(ParallelAlgorithms.ASinMethod).ToString();
            sw.Stop();            
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");

        }

        private void LinearIntegral()
        {
            LinearAlgorithms.N = CountOfIterations;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LinearValue = $"{LinearAlgorithms.GetIntegral()}";
            sw.Stop();            
            LinearTime = $"{sw.Elapsed.TotalSeconds}";
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");
        }

        private void ParallelIntegral()
        {
            ParallelAlgorithms.CountOfThreads = CountOfThreads;
            ParallelAlgorithms.N = CountOfIterations;
            var sw = new Stopwatch();
            sw.Start();
            ParallelValue = ParallelAlgorithms.GetPI(ParallelAlgorithms.IntegralMethod).ToString(); ;
            sw.Stop();
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");
        }
       
        private void LinearMonteCarlo()
        {
            LinearAlgorithms.N = CountOfIterations;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LinearValue = $"{LinearAlgorithms.GetMonteCarlo()}";
            sw.Stop();
            LinearTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("LinearValue");
            RaisePropertyChanged("LinearTime");

        }
        private void ParallelMonteCarlo()
        {
            ParallelAlgorithms.CountOfThreads = CountOfThreads;
            ParallelAlgorithms.N = CountOfIterations;
            Stopwatch sw = new Stopwatch();
            sw.Start();            
            ParallelValue = ParallelAlgorithms.GetPI(ParallelAlgorithms.MonteCarloMethod).ToString();
            sw.Stop();           
            ParallelTime = sw.Elapsed.TotalSeconds.ToString();
            RaisePropertyChanged("ParallelValue");
            RaisePropertyChanged("ParallelTime");

        }



        private double Circle(double x,double radius)
        {
            return radius * radius - x * x;
        }

        private void SetResult(double result)
        {
            ParallelValue = result.ToString();
        }
    }
}
