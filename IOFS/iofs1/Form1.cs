using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MoreLinq;

namespace iofs1
{
    public partial class Form1 : Form
    {
        const string toDelete = ",.!?...;:-(){}'\"\r\n«»1234567890—";

        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;

            Text = File.OpenText(openFileDialog1.FileName).ReadToEnd();
            toDelete.ForEach(c => Text = Text.Replace(c, ' '));
        }

        private string Text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        private void проверитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<string> words = GetWordsInText().ToArray();
            string[] textWords = GetTextByWords().ToArray();

            var frequencies = words.ToDictionary(w => w, w => textWords.Count(tw => tw.Contains(w)));
            var rangs = frequencies
                       .GroupBy(p => p.Value)
                       .OrderByDescending(g => g.Key)
                       .ToArray();

            Series series1 = new Series();
            Series series2 = new Series();

            int index = 1;
            rangs.ForEach(r =>
                              {
                                  series1.Points.AddXY(r.Key, r.Count());
                                  series2.Points.AddXY(index, r.Key);
                                  index++;
                              });

            chart1.Series.Clear();
            chart1.Series.Add(series1);
            chart2.Series.Clear();
            chart2.Series.Add(series2);

            double C = rangs.SelectMany(r => r.Select((p, i) => (p.Value * i) / (double)textWords.Length))
                            .Average();
            label1.Text = string.Format("Постоянная Зиппа: {0}", C);
        }

        private IEnumerable<string> GetWordsInText()
        {
            return GetTextByWords().Distinct();
        }

        private IEnumerable<string> GetTextByWords()
        {
            return Text.Split(' ')
                       .Select(w => w.Trim())
                       .Where(w => !toDelete.Contains(w));
        }

        private void словарьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var outFile = File.CreateText(@"C:\Users\s.valyushitsky\Desktop\IOFS\iofs1\dict_ru1.txt"))
            {
                var forms = new List<string>();

                File.ReadLines(@"C:\Users\s.valyushitsky\Desktop\IOFS\iofs1\dict_ru.txt")
                    .Where(str => !string.IsNullOrWhiteSpace(str))
                    .Select(str => str.TrimEnd())
                    .ForEach(str =>
                                 {
                                     if (!str.StartsWith(" "))
                                     {
                                         outFile.WriteLine(string.Join("|", forms.Select(f => f.Trim())));
                                         forms.Clear();
                                     }

                                     forms.Add(str);
                                 });
            }
        }
    }
}
