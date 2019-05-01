using LexicAnalyzer;
using SyntaxAnalyzer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
	class Program
	{
		static Lexer l = new Lexer("1.txt", "2.txt", "separs.txt", "Multi.txt");
		static Syntaxer s = new Syntaxer(l.LexemString, l.Identifiers, l.Keywords);

		static void Error()
		{
			//s.Tree.Write();
			Console.ReadKey();
			Environment.Exit(0);
		}

		static void Main(string[] args)
		{
			
			var x =Directory.GetCurrentDirectory();
			l.Analyze();
			l.WriteLexemStringToConsole();
			s.OnError += ErrorHandler;
			s.Analyze();
			//s.Tree.Write();
			Console.ReadKey();
		}

		static void ErrorHandler()
		{
			Console.WriteLine(s.Error);
			Console.ReadKey();
			Environment.Exit(1);
		}
	}
}
