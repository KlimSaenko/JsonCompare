using System.Drawing;
using System.Windows.Forms;
using TestTask.JsonUtility;
using TestTask.Extensions;
using System;

namespace TestTask
{
    public partial class MainForm : Form
    {
        public MainForm(string[] args)
        {
            InitializeComponent();

            infoButton.Enabled = false;
            StartLogic(args);
        }

        //private readonly string[] Args = { @"C:\Users\User\source\repos\Test\Test\JsonModels\TextFile1.json", @"C:\Users\User\source\repos\Test\Test\JsonModels\TextFile2.json" };
        //private readonly string[] Args = { @"C:\Users\User\compare-files\__fixtures__\file1.json", @"C:\Users\User\compare-files\__fixtures__\file2.json" };
        private async void StartLogic(string[] args)
        {
            var jsonTextLines = await JsonConverter.GetJsonAsFields(args);

            JsonComparer.CompareJson(jsonTextLines[0], jsonTextLines[1]);

            _mismatches = new int[args.Length];

            _mismatches[0] = ShowJsonComparison(jsonTextLines[0], firstTextBox);
            _mismatches[1] = ShowJsonComparison(jsonTextLines[1], secondTextBox);

            Console.WriteLine("Done!");
        }

        private static int[] _mismatches;
        private string _tabs = string.Empty;

        private int ShowJsonComparison(IJsonField[] jsonTextLines, RichTextBox textBox)
        {
            var mismatches = 0;

            foreach (var field in jsonTextLines)
            {
                var propertyString = $"{_tabs}{field.JsonProperty.Name}: ";

                if (!field.SameField)
                {
                    mismatches++;
                    textBox.AppendText(propertyString, Color.IndianRed);
                }
                else textBox.AppendText(propertyString);

                if (field.ChildFields == null) textBox.AppendText($"{field.ValueString},\n");
                else
                {
                    _tabs += "\t";
                    textBox.AppendText("{\n");

                    mismatches += ShowJsonComparison(field.ChildFields, textBox);

                    _tabs = _tabs.Remove(0, 1);
                    textBox.AppendText(_tabs + "}\n");
                }
            }

            infoButton.Enabled = true;

            return mismatches;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var message = $"First JSON has {_mismatches[0]} mismatched fields.\nSecond JSON has {_mismatches[1]} mismatched fields.\n" +
                "(Algorithm compares two JSON fields by their names and types)";

            MessageBox.Show(message, "Comparison Info", MessageBoxButtons.OK);
        }
    }
}
