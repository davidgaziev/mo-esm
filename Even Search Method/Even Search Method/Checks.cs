using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;


namespace Even_Search_Method
{
   
    internal class Checks
    {
        public void greaterZero(GroupBox arr)
        {
            TextBox[] textBoxes;
            textBoxes = arr.Controls.OfType<TextBox>().Where(tb => tb.Name != "textBox1").ToArray();
            for (int i = 0; i < textBoxes.Length; i++) 
            {

                TextBox item = textBoxes.GetValue(i) as TextBox;

                decimal value = Convert.ToDecimal(Convert.ToDouble(item.Text));

                if(value <= 0)
                {
                    MessageBox.Show("Значение должно быть больше нуля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    item.BackColor = System.Drawing.Color.Red;
                    i = textBoxes.Length;
                }
            }
        }

        public (int, int) iterations(int k, int max, int condKMax, Stopwatch swatch, TextBox textBox)
        {
            swatch.Stop();
            DialogResult result = MessageBox.Show("Решение не может быть найдено с данной погрешностью \r\n" +
                "из-за лимита количества итераций. \r\n" +
                "\r\n" +
                "Хотите увеличить количество итераций?", "Внимание",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                max += max;
                textBox.Text = max.ToString();
            }
            else
            {
                condKMax = 1;
            }
            swatch.Start();

            return (max, condKMax);
        }

        public (double, int) time(double tMax, int condTMax, Stopwatch swatch, TextBox textBox)
        {
            DialogResult result = MessageBox.Show("Решение не может быть найдено\r\n" +
                                "из-за лимита времени.\r\n" +
                                "\r\n" +
                                "Хотите увеличить лимит времени?", "Внимание",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                tMax = tMax + tMax;
                textBox.Text = tMax.ToString();
                swatch.Start();
            }
            else
            {
                condTMax = 1;
            }

            return (tMax, condTMax);
        }
    }
}
