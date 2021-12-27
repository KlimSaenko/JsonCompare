using System;
using System.Windows.Forms;
using System.IO;

namespace TestTask
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (!ValidateArguments(args)) return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args));
        }

        private static bool ValidateArguments(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Not enough arguments!");
                return false;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("First json file doesn't exist!");
                return false;
            }

            if (!File.Exists(args[1]))
            {
                Console.WriteLine("Second json file doesn't exist!");
                return false;
            }

            if (string.Equals(args[0], args[1]))
            {
                Console.WriteLine("There two the same json files!");
                return false;
            }

            return true;
        }
    }
}
