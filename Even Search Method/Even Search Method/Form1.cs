using parserDecimal.Parser;
using System;
using System.Windows.Forms;
using System.IO; 
using Microsoft.Office.Interop.Excel; 
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Extreme.Mathematics.Calculus.OrdinaryDifferentialEquations;

namespace Even_Search_Method
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string func = ""; //функция

        private void button1_Click(object sender, EventArgs e)
        {
            Computer computer = new Computer();
            
            decimal x0, x1 = 0, tol, H, YF0, YF1 = 0, F1, F2;
            double tMax;
            int max, k = 0, cond = 0, condKMax = 0, condTMax = 0;

            func = comboBox1.Text;
            func = func.ToLower();
            Stopwatch swatch = new Stopwatch();          
            try
            {
                x0 = decimal.Parse(textBox1.Text);
                YF0 = computer.Compute(func, x0);
                tol = Convert.ToDecimal(Convert.ToDouble(textBox2.Text));
                max = int.Parse(textBox3.Text);
                H = Convert.ToDecimal(Convert.ToDouble(textBox7.Text));
                tMax = Convert.ToDouble(textBox9.Text);
            }
            catch
            {
                MessageBox.Show("Проверьте входные данные.", 
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (max <= 0)
            {
                MessageBox.Show("Значение итерации должно быть больше единицы!",
                 "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tol <= 0)
            {
                MessageBox.Show("Погрешность должна быть больше нуля",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(H <= 0 || H > tol)
            {
                MessageBox.Show("Шаг поиска должен быть больше нуля и меньше или равен погрешности",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if(tMax <= 0)
            {
                MessageBox.Show("Лимит времени должен быть больше нуля",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox8.Clear();
                textBox10.Clear();
                textBox11.Clear();
                label12.Visible = false;

                if (radioButton1.Checked)
                {
                    YF0 = aziretParser.ParserDecimal.Compute(func, x0);
                    swatch.Start(); //начало подсчета времени
                    do
                    {
                        
                        k = k + 1;

                        if (k == max)
                        {
                            swatch.Stop();
                            DialogResult result = MessageBox.Show("Решение не может быть найдено с данной погрешностью \r\n" +
                                "из-за лимита количества итераций. \r\n" +
                                "\r\n" +
                                "Хотите увеличить количество итераций?", "Внимание",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                max = max + max;
                                textBox3.Text = max.ToString();
                                //button1_Click(sender, e);
                            }else
                            {
                                condKMax = 1;                                                 
                            }
                            swatch.Start();
                        }

                        progressBar1.Visible = true;
                        progressBar1.Maximum = k + 1;
                        progressBar1.Value = k;
                        x1 = x0 + H;
                        YF1 = aziretParser.ParserDecimal.Compute(func, x1);

                        if (YF1 >= YF0)
                        {
                            if (k == 1)
                            {
                                cond = 1;
                            }
                            x1 = x0;
                            YF1 = YF0;
                        }
                        else
                        {
                            x0 = x1;
                            YF0 = YF1;
                            x1 = x0 + H;
                            YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                        }

                    } while (cond == 0 && Math.Abs(x1 - x0) > 0 && condKMax == 0);

                    swatch.Stop();

                    if(swatch.Elapsed.TotalSeconds > tMax)
                    {
                        DialogResult result = MessageBox.Show("Решение не может быть найдено\r\n" +
                            "из-за лимита времени.\r\n" +
                            "\r\n" +
                            "Хотите увеличить лимит времени?", "Внимание",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            tMax = tMax + tMax;
                            textBox9.Text = tMax.ToString();
                            button1_Click(sender, e);
                        }else
                        {
                            condTMax = 1;
                        }
                    }

                    progressBar1.Value = k;
                    progressBar1.Visible = true;
                    progressBar1.Value = 0;

                    if (cond == 1)
                    {
                        label12.ForeColor = System.Drawing.Color.DarkRed;
                        DialogResult result = MessageBox.Show("Метод не выполнил ни одну итерацию, \r\n" +
                            "поскольку начальное значение уже является \r\n" +
                            "оптимальным или находится справа от оптимальной. \r\n" +
                            "Начальное значение должно быть слева от оптимального." +
                            "\r\n" + 
                            "Хотите проверить график функции?", "Внимание",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            button3_Click(sender, e);
                        }
                    }

                    if(cond == 0)
                    {
                        textBox8.Text = (swatch.Elapsed).ToString();
                        textBox4.Text = x1.ToString("N28");
                        textBox5.Text = YF1.ToString("0E0");
                        textBox6.Text = k.ToString();

                        F1 = computer.Compute(func, x1 - tol);
                        textBox10.Text = F1.ToString("0E0");
                        F2 = computer.Compute(func, x1 + tol);
                        textBox11.Text = F2.ToString("0E0");

                        if (YF1 <= F1 && YF1 <= F2)
                        {
                            label12.ForeColor = System.Drawing.Color.Green;
                            label12.Visible = true;
                            label12.Text = "Вывод:\r\n" +
                                "Решение было найдено с допустимой погрешностью = " + tol.ToString("0E0") + ",\r\n которая <= Tolerance\r\n" +
                                "Результат X* является правильным минимумом так как: \r\n" +
                                "1. f(X*) <= f(X*–Tolerance) " + " и " + "f(X*) <= f(X*+Tolerance)\r\n" +
                                "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(YF1 - F1)  + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(YF1 - F2);
                        }
                        else
                        {
                            label12.ForeColor = System.Drawing.Color.DarkRed;
                            label12.Visible = true;
                            label12.Text = "Вывод:\r\n" +
                                "Решение было найдено с допустимой погрешностью\r\n" +
                                "Результат X* не является правильным минимумом так как: \r\n" +
                                "1. f(X*) <= f(X*–Tolerance)" + " и " + "f(X*) >= f(X*+Tolerance)\r\n" +
                                "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(YF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(YF1 - F2);
                        }
                    }
                }

                else if (radioButton2.Checked) 
                {
                    YF0 = computer.Compute(func, x0);
                    swatch.Start(); //начало подсчета времени

                    do
                    {
                        k = k + 1;

                        if (k == max)
                        {
                            DialogResult result = MessageBox.Show("Решение не может быть найдено с данной погрешностью \r\n" +
                                "из-за лимита количества итераций. \r\n" +
                                "\r\n" +
                                "Хотите увеличить количество итераций?", "Внимание",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                max = max + max;
                                textBox3.Text = max.ToString();
                                button1_Click(sender, e);
                            }
                            else
                            {
                                condKMax = 1;
                            }
                        }

                        progressBar1.Visible = true;
                        progressBar1.Maximum = k + 1;
                        progressBar1.Value = k;
                        x1 = x0 + H;
                        YF1 = aziretParser.ParserDecimal.Compute(func, x1);

                        if (YF1 <= YF0)
                        {
                            if (k == 1)
                            {
                                cond = 1;
                            }
                            x1 = x0;
                            YF1 = YF0;
                        }
                        else
                        {
                            x0 = x1;
                            YF0 = YF1;
                            x1 = x0 + H;
                            YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                        }

                    } while (cond == 0 & Math.Abs(x1 - x0) > 0 & condKMax == 0);

                    swatch.Stop();

                    if (swatch.Elapsed.TotalSeconds > tMax)
                    {
                        DialogResult result = MessageBox.Show("Решение не может быть найдено\r\n" +
                            "из-за лимита времени.\r\n" +
                            "\r\n" +
                            "Хотите увеличить лимит времени?", "Внимание",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            tMax = tMax + tMax;
                            textBox9.Text = tMax.ToString();
                            button1_Click(sender, e);
                        }
                    }

                    progressBar1.Visible = false;
                    progressBar1.Value = 0;

                    if (cond == 1)
                    {
                        DialogResult result = MessageBox.Show("Метод не выполнил ни одну итерацию, \r\n" +
                           "поскольку начальное значение уже является \r\n" +
                           "оптимальной или находится справо от оптимальной. \r\n" +
                           "\r\n" +
                           "Хотите проверить график функции?", "Внимание",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            button3_Click(sender, e);
                        }
                    }

                    if (cond == 0 && condKMax == 0 && condTMax == 0)
                    {
                        textBox8.Text = (swatch.Elapsed).ToString();
                        textBox4.Text = x1.ToString("N28");
                        textBox5.Text = YF1.ToString("0E0");
                        textBox6.Text = k.ToString();

                        F1 = computer.Compute(func, x1 - tol);
                        textBox10.Text = F1.ToString("0E0");
                        F2 = computer.Compute(func, x1 + tol);
                        textBox11.Text = F2.ToString("0E0");

                        if (YF1 >= F1 && YF1 >= F2)
                        {
                            label12.ForeColor = System.Drawing.Color.Green;
                            label12.Visible = true;
                            label12.Text = "Вывод:\r\n" +
                               "Решение было найдено с допустимой погрешностью\r\n" +
                               "Результат X* является правильным максимумом так как:\r\n" +
                               "1. f(X*) >= f(X*–Tolerance) " +" И " + "f(X*) >= f(X*+Tolerance) \r\n" +
                               "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(YF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(YF1 - F2);
                        }
                        else
                        {
                            label12.ForeColor = System.Drawing.Color.DarkRed;
                            label12.Visible = true;
                            label12.Text = "Вывод:\r\n" +
                            "Решение было найдено с допустимой погрешностью\r\n" +
                            "Результат X* не является правильным максимумом так как:" +
                            "1. f(X*) >= f(X*–Tolerance) \r\n" + " И " + "f(X*) <= f(X*+Tolerance) \r\n" +
                            "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(YF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(YF1 - F2);
                        }
                    }                    
                }
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();
            textBox10.Clear();
            textBox11.Clear();
            label12.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mySheet = Path.Combine(System.Windows.Forms.Application.StartupPath, "Grafic.xlsx");
        
            decimal b, c;

            Excel.Application ExcelApp = new Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(mySheet);
            Worksheet ws = (Worksheet)wb.ActiveSheet;

            ExcelApp.Visible = true;
            
            func = comboBox1.Text;
            ws.Cells[2, 2] = func;
            func = func.Replace("exp", "!");
            func = func.Replace("x", "D4"); 
            func = "=" + func.Replace("!", "exp");
            ws.Cells[4, 9] = textBox1.Text; 
            b = decimal.Parse(textBox1.Text);
            c = Math.Abs(b) + 3;
            ws.Cells[4, 10] = c;
            ws.Range["E4", "E10003"].Value = func;

        }
        private void button4_Click(object sender, EventArgs e)
        {

            string mySheet = Path.Combine(System.Windows.Forms.Application.StartupPath, "Grafic.xlsx");
            Excel.Application ExcelApp = new Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = ExcelApp.Workbooks.Open(mySheet);

            Worksheet sh = (Worksheet)wb.ActiveSheet;

            Microsoft.Office.Interop.Excel.Range cell = sh.Cells[4, 9] as Excel.Range;
            string value = cell.Value2.ToString();
            textBox1.Text = value;

          
            wb.Close();

            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox10.Clear();
            textBox11.Clear();
            label12.Visible = false;
        }

     
    }
}
