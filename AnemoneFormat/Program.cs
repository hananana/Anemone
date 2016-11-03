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
		public static void Main (string[] args)
		{
			var argsOption = args [0];
			var filePath = args [1];
			//recursive
			if (argsOption == "-r") {
				var files = GetCSharpFilePaths(filePath);
				files.ForEach(path => {
					var result = FormattedString(path);
					Overwrite(path, result);
				});
			}
            //single
            else if (argsOption == "-s") {
				var result = FormattedString (filePath);
				Overwrite (filePath, result);
			}
            //stdout
            else if (argsOption == "-o") {
				var result = FormattedString (filePath);
				Console.WriteLine (result);
			}
            //from data
            else if (argsOption == "-f") {
				var str = args [1];
				var result = FormattedString(str);
				Console.WriteLine(result);
			}
		}

		static List<string> GetCSharpFilePaths(string filePath)
		{
			var files = new List<string> ();
			var dirs = new List<string> (Directory.GetDirectories(filePath));
			dirs.ForEach(dir => files.AddRange(Directory.GetFiles(dir, "*.cs")));
			return files;
		}

		static void Overwrite(string filePath, string formatted)
		{
			File.WriteAllText(filePath, formatted);
		}

		static string FormattedString(string filePath)
		{
			var readed = File.ReadAllText(filePath);
			var option = FormattingOptionsFactory.CreateAllman();
            option.IndentBlocksInsideExpressions = false;
			var textEditorOption = new TextEditorOptions ();
			var formatter = new CSharpFormatter (option, textEditorOption);
			return formatter.Format(readed);
		}
	}
}
