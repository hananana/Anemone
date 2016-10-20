using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.Editor;

namespace CSharpFormat
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            var argsOption = args[0];
            var filePath = args[1];
            if(argsOption == "-r")
            {
                var files = GetCSharpFilePaths(filePath);
                files.ForEach(path => Overwrite(path));
            }
            else if(argsOption == "-s")
            {
                Overwrite(filePath);
            }
            else if(argsOption == "-o")
            {
                var result = FormattedString(filePath);
                Console.WriteLine(result);
            }
		}

        static List<string> GetCSharpFilePaths(string filePath)
        {
            var files = new List<string>();
            var dirs = new List<string>(Directory.GetDirectories(filePath));
            dirs.ForEach(dir => files.AddRange(Directory.GetFiles(dir, "*.cs")));
            return files;
        }

        static void Overwrite(string filePath)
        {
            var result = FormattedString(filePath);
            File.WriteAllText(filePath, result);
        }

        static string FormattedString(string filePath)
        {
			var readed = File.ReadAllText(filePath);
// 			Console.WriteLine("----- before -----");
// 			Console.WriteLine(readed);

           	var option = FormattingOptionsFactory.CreateMono();
            var textEditorOption = new TextEditorOptions();

			var formatter = new CSharpFormatter(option, textEditorOption);
			return formatter.Format(readed);
        }
	}
}
