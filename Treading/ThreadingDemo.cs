using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_311.Treading
{
    internal class ThreadingDemo
    {
        public void Run()
        {
            new Thread(ThreadMethod).Start();
            Console.WriteLine("Thereading DEMO");
            sum = 100;
            for (int i = 0; i < 12; i++)
            {
                new Thread(CalcOneMonth).Start();
            }


        }

        private void ThreadMethod()
        {
            Console.WriteLine("Hello from thread 1");
        }

        private double sum;

        private void CalcOneMonth()
        {
            Thread.Sleep(300);
            double percent = 10.0;
            sum *= 1.0 + percent / 100;

            Console.WriteLine("Calc: + {0}%->{1:F1} g",percent , sum);
        }

    }
}
