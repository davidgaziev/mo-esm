using parserDecimal.Parser;
using System;
using System.Windows.Forms;
using System.IO; 
using Microsoft.Office.Interop.Excel; 
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            decimal x0, tol, H, F1, F2, resultYF1, resultX1;
            double tMax;
            int max, resultK, resultCond;

            var checks = new Checks();
            var swatch = new Stopwatch();

            func = comboBox1.Text.ToLower();

            try
            {
                x0 = decimal.Parse(textBox1.Text);
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

            checks.greaterZero(groupBox1);

            if (H > tol)
            {
                MessageBox.Show("Шаг поиска должен быть меньше или равен погрешности",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }   

            clearOutput();

            var solver = new ESM(func, x0, H, tol, max, tMax, textBox3, textBox9, progressBar1, swatch);

            if (radioButton1.Checked)
            {

                (resultCond, resultK, resultYF1, resultX1, swatch) = solver.findMinExtremum();

                swatch.Stop();

                progressBar1.Value = 0;

                if (resultCond == 1)
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
                    return;
                }
             
                (F1, F2) = outputData(resultX1, resultYF1, resultK, tol, swatch);

                if (resultYF1 <= F1 && resultYF1 <= F2)
                {
                    label12.ForeColor = System.Drawing.Color.Green;
                    label12.Visible = true;
                    label12.Text = "Вывод:\r\n" +
                        "Решение было найдено с допустимой погрешностью = " + tol.ToString("0E0") + ",\r\n которая <= Tolerance\r\n" +
                        "Результат X* является правильным минимумом так как: \r\n" +
                        "1. f(X*) <= f(X*–Tolerance) " + " и " + "f(X*) <= f(X*+Tolerance)\r\n" +
                        "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(resultYF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(resultYF1 - F2);
                }
                else
                {
                    label12.ForeColor = System.Drawing.Color.DarkRed;
                    label12.Visible = true;
                    label12.Text = "Вывод:\r\n" +
                        "Решение было найдено с допустимой погрешностью\r\n" +
                        "Результат X* не является правильным минимумом так как: \r\n" +
                        "1. f(X*) <= f(X*–Tolerance)" + " и " + "f(X*) >= f(X*+Tolerance)\r\n" +
                        "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(resultYF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(resultYF1 - F2);
                }
            }
            else if (radioButton2.Checked)
            {
                (resultCond, resultK, resultYF1, resultX1, swatch) = solver.findMaxExtremum();

                swatch.Stop();

                progressBar1.Value = 0;

                if (resultCond == 1)
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

                    return;
                }

                (F1, F2) = outputData(resultX1, resultYF1, resultK, tol, swatch);

                if (resultYF1 >= F1 && resultYF1 >= F2)
                {
                    label12.ForeColor = System.Drawing.Color.Green;
                    label12.Visible = true;
                    label12.Text = "Вывод:\r\n" +
                        "Решение было найдено с допустимой погрешностью\r\n" +
                        "Результат X* является правильным максимумом так как:\r\n" +
                        "1. f(X*) >= f(X*–Tolerance) " + " И " + "f(X*) >= f(X*+Tolerance) \r\n" +
                        "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(resultYF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(resultYF1 - F2);
                }
                else
                {
                    label12.ForeColor = System.Drawing.Color.DarkRed;
                    label12.Visible = true;
                    label12.Text = "Вывод:\r\n" +
                    "Решение было найдено с допустимой погрешностью\r\n" +
                    "Результат X* не является правильным максимумом так как:" +
                    "1. f(X*) >= f(X*–Tolerance) \r\n" + " И " + "f(X*) <= f(X*+Tolerance) \r\n" +
                    "2. sign(f(X*) - f(X* - Tolerance)) == " + Math.Sign(resultYF1 - F1) + " и sign(f(X*) - f(X* + Tolerance)) == " + Math.Sign(resultYF1 - F2);
                }
            }
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            clearOutput();
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

            clearOutput();
        }
        public void clearOutput()
        {
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox8.Clear();
            textBox10.Clear();
            textBox11.Clear();
            label12.Visible = false;
        }
        private void textBox_DefaultBg(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            textBox.BackColor = System.Drawing.Color.White;    
        }

        private (decimal, decimal) outputData(decimal resultX1, decimal resultYF1, int resultK, decimal tol, Stopwatch swatch)
        {
            decimal F1, F2;

            var computer = new Computer();

            textBox8.Text = (swatch.Elapsed).ToString();
            textBox4.Text = resultX1.ToString("N28");
            textBox5.Text = resultYF1.ToString("0E0");
            textBox6.Text = resultK.ToString();

            F1 = computer.Compute(func, resultX1 - tol);
            textBox10.Text = F1.ToString("0E0");
            F2 = computer.Compute(func, resultX1 + tol);
            textBox11.Text = F2.ToString("0E0");

            return (F1, F2);
        }
    }
}
