using System;
using System.Collections.Generic;
using Shared.Enums;
using Shared.Structs;

namespace Syntaxer
{
	public class Syntaxer
	{
		public Node Tree { get; protected set; }
		public List<Lexem> LexemString { get; set; }
		public Dictionary<string,Lexem> Identifiers { get; set; }
		public Dictionary<string,Lexem> Variables { get; set; }

		public Syntaxer(List<Lexem> lexemString, Dictionary<string, Lexem> identifiers, Dictionary<string, Lexem> variables)
		{
			LexemString = lexemString;
			Identifiers = identifiers;
			Variables = variables;
		}

		public void Analyze()
		{
			
		}
	}
}
