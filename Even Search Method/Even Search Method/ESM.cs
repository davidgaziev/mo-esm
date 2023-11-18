using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace Even_Search_Method
{
     public class ESMResult {
            public int Cond { get; set; }
            public int K { get; set; }
            public decimal YF1 { get; set; }
            public decimal X1 { get; set; }
            public Stopwatch Swatch { get; set; }


        public ESMResult(int cond, int k, decimal yF1, decimal x1, Stopwatch swatch)
        {
            Cond = cond;
            K = k;
            YF1 = yF1;
            X1 = x1;
            Swatch = swatch;
        }

        public void Deconstruct(out int cond, out int k, out decimal yf1, out decimal x1, out Stopwatch swatch)
        {
            cond = Cond;
            k = K;
            yf1 = YF1;
            x1 = X1;
            swatch = Swatch;
        }
    }
    public class ESM
    {
        public string Function { get; set; }
        public decimal X0 { get; set; }
        public decimal H { get; set; }
        public decimal Tol { get; set; }
        public int MaxIterations { get; set; }
        public double MaxTime { get; set; }
        public TextBox TextBox3 { get; set; }
        public TextBox TextBox9 { get; set; }
        public ProgressBar ProgressBar1 { get; set; }
        public Stopwatch Swatch { get; set; }

        public ESM(string func, decimal x0, decimal h, decimal tol, int maxIterations,
                    double maxTime, TextBox textBox3, TextBox textBox9, ProgressBar progressBar1, Stopwatch swatch)
        {
            Function = func;
            X0 = x0;
            H = h;
            Tol = tol;
            MaxIterations = maxIterations;
            MaxTime = maxTime;
            TextBox3 = textBox3;
            TextBox9 = textBox9;
            ProgressBar1 = progressBar1;
            Swatch = swatch;
        }
        public ESMResult findMinExtremum()
        {
            int k = 0, cond = 0, condKMax = 0, condTMax = 0;
            decimal x1, YF0, YF1;
            double ElapsedSec;

            var checks = new Checks();

            YF0 = aziretParser.ParserDecimal.Compute(Function, X0);
            Swatch.Start();
            
            do
            {
                k++;

                if (k >= MaxIterations)
                {
                    (MaxIterations, condKMax) = checks.iterations(k, MaxIterations, condKMax, Swatch, TextBox3);
                }

                ProgressBar1.Visible = true;
                ProgressBar1.Maximum = k + 1;
                ProgressBar1.Value = k;

                x1 = X0 + H;
                YF1 = aziretParser.ParserDecimal.Compute(Function, x1);

                if (YF1 >= YF0)
                {
                    if (k == 1)
                    {
                        cond = 1;
                    }
                    x1 = X0;
                    YF1 = YF0;
                }
                else
                {
                    X0 = x1;
                    YF0 = YF1;
                    x1 = X0 + H;
                    YF1 = aziretParser.ParserDecimal.Compute(Function, x1);
                }

                ElapsedSec = Swatch.Elapsed.TotalSeconds;

                if (ElapsedSec > MaxTime)
                {
                    Swatch.Stop();
                    (MaxTime, condTMax) = checks.time(MaxTime, condTMax, Swatch, TextBox9);
                }

            } while (cond == 0 && Math.Abs(x1 - X0) >= Tol && condKMax == 0 && condTMax == 0);

            var result = new ESMResult(cond, k, YF1, x1, Swatch);

            return result;
        }
        public ESMResult findMaxExtremum()
        {
            int k = 0, cond = 0, condKMax = 0, condTMax = 0;
            decimal x1, YF0, YF1;
            double ElapsedSec;

            var checks = new Checks();

            YF0 = aziretParser.ParserDecimal.Compute(Function, X0);
            Swatch.Start();

            do
            {
                k++;

                if (k >= MaxIterations)
                {
                    (MaxIterations, condKMax) = checks.iterations(k, MaxIterations, condKMax, Swatch, TextBox3);
                }

                ProgressBar1.Visible = true;
                ProgressBar1.Maximum = k + 1;
                ProgressBar1.Value = k;

                x1 = X0 + H;
                YF1 = aziretParser.ParserDecimal.Compute(Function, x1);

                if (YF1 <= YF0)
                {
                    if (k == 1)
                    {
                        cond = 1;
                    }
                    x1 = X0;
                    YF1 = YF0;
                }
                else
                {
                    X0 = x1;
                    YF0 = YF1;
                    x1 = X0 + H;
                    YF1 = aziretParser.ParserDecimal.Compute(Function, x1);
                }

                ElapsedSec = Swatch.Elapsed.TotalSeconds;

                if (ElapsedSec > MaxTime)
                {
                    Swatch.Stop();
                    (MaxTime, condTMax) = checks.time(MaxTime, condTMax, Swatch, TextBox9);
                }

            } while (cond == 0 && Math.Abs(x1 - X0) >= Tol && condKMax == 0 && condTMax == 0);

            var result = new ESMResult(cond, k, YF1, x1, Swatch);

            return result;
        }
    }
}
