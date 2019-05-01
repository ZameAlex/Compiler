using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Enums;
using Shared.Structs;

namespace SyntaxAnalyzer
{
	public class Syntaxer
	{
		private int currentLexem;
		public Node Tree { get; protected set; }
		public SyntaxError Error { get; set; }
		public List<Lexem> LexemString { get; protected set; }
		public Dictionary<string, int> Identifiers { get; protected set; }
		public Dictionary<string,int> Keywords { get; protected set; }
		public event Action OnError;

		public Syntaxer(List<Lexem> lexemString, Dictionary<string, int> identifiers,	Dictionary<string, int> keywords)
		{
			LexemString = lexemString;
			Identifiers = identifiers;
			Keywords = keywords;
			currentLexem = 0;
		}

		public void Analyze()
		{
			int currentLevel = 0;
			SignalProgram(currentLevel);
		}

		#region AnalyzeFunctions
		private void SignalProgram(int currentLevel)
		{
			Tree = new Node("signal_program", currentLevel);
			Tree.Add(new Node("program", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Program(currentLevel + 1);
		}
		private void Program(int currentLevel)
		{
			RiseErrorOrAddNodeToTree(currentLevel, "PROGRAM");
			Tree.Add(new Node("procedure-identifier", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			ProcedureIdentifier(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, ";");
			Tree.Add(new Node("block", currentLevel));
			Tree = Tree.Children[3];
			Block(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, ";");
			Tree = Tree.Parent;
		}
		private void Block(int currentLevel)
		{
			Tree.Add(new Node("declarations", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Declarations(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, "BEGIN");
			Tree.Add(new Node("statement-list", currentLevel));
			Tree = Tree.Children[2];
			StatementList(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, "END");
			Tree = Tree.Parent;
		}
		private void StatementList(int currentLevel)
		{
			Tree.Add(new Node("empty", currentLevel));
			Tree = Tree.Parent;
		}
		private void Declarations(int currentLevel)
		{
			Tree.Add(new Node("procedure-declaration", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			ProcedureDeclarations(currentLevel+1);
			Tree = Tree.Parent;
		}
		private void ProcedureDeclarations(int currentLevel)
		{
			Tree.Add(new Node("procedure", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Procedure(currentLevel + 1);
			if (LexemString[currentLexem + 1].value == "PROCEDURE")
				ProcedureDeclarations(currentLevel);
			else
				Tree.Add(new Node("empty", currentLevel));
			Tree = Tree.Parent;

		}

		private void Procedure(int currentLevel)
		{
			RiseErrorOrAddNodeToTree(currentLevel, "PROCEDURE");
			Tree.Add(new Node("procedure-identifier", currentLevel));
			Tree = Tree.Children[1];
			ProcedureIdentifier(currentLevel + 1);
			if (LexemString[currentLexem + 1].value == ";")
				RiseErrorOrAddNodeToTree(currentLevel, ";");
			else
			{
				Tree.Add(new Node("parameters-list", currentLevel));
				Tree = Tree.Children[2];
				RiseErrorOrAddNodeToTree(currentLevel, ";");
			}
			Tree = Tree.Parent;

		}
		private void ParametersList(int currentLevel)
		{
			RiseErrorOrAddNodeToTree(currentLevel, "(");
			Tree.Add(new Node("declarations-list", currentLevel));
			Tree = Tree.Children[1];
			DeclarationsList(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, ")");
			Tree = Tree.Parent;
		}
		private void DeclarationsList(int currentLevel)
		{
			Tree.Add(new Node("declaration", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Declaration(currentLevel + 1);
			if (LexemString[currentLexem+1].value == ")")
				Tree.Add(new Node("empty", currentLevel));
			else
				DeclarationsList(currentLevel);
			Tree = Tree.Parent;

		}
		private void Declaration(int currentLevel)
		{
			Tree.Add(new Node("variable-identifier", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			VariableIdentifier(currentLevel + 1);
			Tree.Add(new Node("identifiers-list", currentLevel));
			Tree = Tree.Children[1];
			IdentifierList(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, ":");
			Tree.Add(new Node("attribute", currentLevel));
			Tree = Tree.Children[3];
			Attribute(currentLevel + 1);
			Tree.Add(new Node("attribute-list", currentLevel));
			Tree = Tree.Children[4];
			AttributesList(currentLevel + 1);
			RiseErrorOrAddNodeToTree(currentLevel, ";");
			Tree = Tree.Parent;
		}
		private void IdentifierList(int currentLevel)
		{
			RiseErrorOrAddNodeToTree(currentLevel, ",");
			Tree.Add(new Node("variable-identifier", currentLevel));
			Tree = Tree.Children[1];
			VariableIdentifier(currentLevel + 1);
			if (LexemString[currentLexem + 1].value == ",")
				IdentifierList(currentLevel);
			else
				Tree.Add(new Node("empty", currentLevel));
			Tree = Tree.Parent;
		}
		private void AttributesList(int currentLevel)
		{
			Tree.Add(new Node("attribute", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Attribute(currentLevel + 1);
			if (LexemString[currentLexem + 1].value == ";")
				Tree.Add(new Node("empty", currentLevel));
			else
				IdentifierList(currentLevel);
			Tree = Tree.Parent;
		}
		private void Attribute(int currentLevel)
		{
			if (Keywords.ContainsValue(LexemString[currentLexem].code))
				Tree.Add(new Node(LexemString[currentLexem++].value, currentLevel));
			else
				RiseErrorOrAddNodeToTree(currentLevel, "ATTRIBUTE");
			Tree = Tree.Parent;
		}
		private void VariableIdentifier(int currentLevel)
		{
			Tree.Add(new Node("identifier", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Identifier(currentLevel + 1);
			Tree = Tree.Parent;
		}
		private void ProcedureIdentifier(int currentLevel)
		{
			Tree.Add(new Node("identifier", currentLevel));
			Tree = Tree.Children.FirstOrDefault();
			Identifier(currentLevel + 1);
			Tree = Tree.Parent;
		}
		private void Identifier(int currentLevel)
		{
			if (Identifiers.ContainsValue(LexemString[currentLexem].code))
				Tree.Add(new Node(LexemString[currentLexem++].value, currentLevel));
			else
				RiseErrorOrAddNodeToTree(currentLevel, "IDENTIFIER");
			Tree = Tree.Parent;
		}
		#endregion AnalyzeFunctions


		private void RiseErrorOrAddNodeToTree(int currentLevel, string expectedString)
		{
			if (LexemString[currentLexem++].value == expectedString)
			{
				Tree.Add(new Node(expectedString, currentLevel));
			}
			else
			{
				SyntaxErrorTypes type=SyntaxErrorTypes.Begin;
				switch (expectedString)
				{
					case "PROGRAM":
						type = SyntaxErrorTypes.Program;
						break;			
					case "BEGIN":
						type = SyntaxErrorTypes.Begin;
						break;
					case "END":
						type = SyntaxErrorTypes.End;
						break;
					case "PROCEDURE":
						type = SyntaxErrorTypes.Procedure;
						break;
					case "ATTRIBUTE":
						type = SyntaxErrorTypes.Attribute;
						break;
					case "VARIABLE":
						type = SyntaxErrorTypes.Variable;
						break;
					case "IDENTIFIER":
						type = SyntaxErrorTypes.Identifier;
						break;


					case ";":
						type = SyntaxErrorTypes.Semicolon;
						break;
					case ":":
						type = SyntaxErrorTypes.Colon;
						break;
					case "(":
						type = SyntaxErrorTypes.OpenBracket;
						break;
					case ")":
						type = SyntaxErrorTypes.CloseBracket;
						break;
					default:
						break;
				}
				ErrorHandler(type);
			}
		}

		private void ErrorHandler(SyntaxErrorTypes type)
		{
			Error = new SyntaxError(type, new Error(LexemString[currentLexem].line, LexemString[currentLexem].column));
			OnError?.Invoke();
		}
	}
}
