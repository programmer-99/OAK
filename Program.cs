using System;
using System.IO;
using System.Reflection;

namespace OAK
{
    class Program
    {
        static void Main(string[] args)
        {
            string testFile = "./test_code.txt";
            string outputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.txt");
          //  string outputFileCsv = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"output.csv");

            var sourceCode = File.ReadAllText(testFile);

            var lexicalAnalyzer = new LexicalAnalyzer();
            var tokens = lexicalAnalyzer.Analyze(sourceCode);

           Console.WriteLine("  TOTAL NUMBER OF TOKENS = " + tokens.Count);

            //Write to file
           

          var  fileText = "";
            foreach (var token in tokens)
            {
                fileText += token.ToString();
            }
            File.WriteAllText(outputFile, fileText);

            //Print to console
            foreach (var t in tokens)
            {
                Console.WriteLine(t.ToString());
            }

            Console.ReadLine();
        }
    }
}
