using System;
using System.IO;
using System.Text;
using MakeHappy.Properties;

namespace MakeHappy
{
    class Program
    {
        private static readonly Settings _settings = Settings.Default;

        static void Main(string[] args)
        {
            string filePath = _settings.DataFilePath;
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File doesn't exist: {filePath}");
            }
            string fileContent = File.ReadAllText(filePath);
            var builder = new StringBuilder(fileContent);
            builder.Replace("\"source\"", "\"sourceID\"");
            builder.Replace("\"target\"", "\"targetID\"");
            builder.Insert(0, "var data = ");
            File.WriteAllText(filePath, builder.ToString());
            Console.WriteLine($"{DateTime.Now}: Done");
        }
    }
}
