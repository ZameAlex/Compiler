using System;
using System.Collections.Generic;
using Shared.Enums;
using Shared.Structs;

namespace Syntaxer
{
	public class Syntaxer
	{
		private int currentLexem;
		public Node Tree { get; protected set; }
		public List<SyntaxError> Errors { get; set; }
		public List<Lexem> LexemString { get; protected set; }
		public Dictionary<string, Lexem> Identifiers { get; protected set; }
		public Dictionary<string, Lexem> Variables { get; protected set; }
		public Dictionary<string,int> Keywords { get; protected set; }
		public Dictionary<string, int> Separators { get; protected set; }

		public Syntaxer(List<Lexem> lexemString, Dictionary<string, Lexem> identifiers, Dictionary<string, Lexem> variables,
		Dictionary<string, int> keywords, Dictionary<string, int> separators)
		{
			LexemString = lexemString;
			Identifiers = identifiers;
			Variables = variables;
			Keywords = keywords;
			Separators = separators;
			currentLexem = 0;
		}

		public void Analyze()
		{
			int currentLevel = 0;
			SignalProgram(currentLevel);
		}

		private void SignalProgram(int currentLevel)
		{
			Tree = new Node("signal_program", currentLevel);
			Tree.Add(new Node("program", currentLevel));
			Tree = Tree.Children[0];
			Program(currentLevel + 1);
		}
		private void Program(int currentLevel)
		{
			if(LexemString[currentLexem++].code==Keywords["PROGRAM"])
			{
				Tree.Add(new Node("PROGRAM", currentLevel));
				Tree = Tree.Children[0];
				ProcedureIdentifier(currentLevel + 1);
				if(LexemString[currentLexem++].code==Separators[";"])
				{
					Tree.Add(new Node(";", currentLevel));
				}
				else
				{
					Errors.Add(new SyntaxError(SyntaxErrorTypes.Semicolon,
					new Error(LexemString[currentLexem].line, LexemString[currentLexem].column)));
				}
			}
		}
		private void Block(int currentLevel)
		{ }
		private void StatementList(int currentLevel)
		{ }
		private void Declarations(int currentLevel)
		{ }
		private void ProcedureDeclarations(int currentLevel)
		{ }
		private void Procedure(int currentLevel)
		{ }
		private void ParametersList(int currentLevel)
		{ }
		private void DeclarationsList(int currentLevel)
		{ }
		private void Declaration(int currentLevel)
		{ }
		private void IdentifierList(int currentLevel)
		{ }
		private void AttributesList(int currentLevel)
		{ }
		private void Attribute(int currentLevel)
		{ }
		private void VariableIdentifier(int currentLevel)
		{ }
		private void ProcedureIdentifier(int currentLevel)
		{ }
		private void Identifier(int currentLevel)
		{ }
		private void String(int currentLevel)
		{ }
		private void Digit(int currentLevel)
		{ }
		private void Letter(int currentLevel)
		{ }
	}
}
