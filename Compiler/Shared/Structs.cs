﻿using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace Shared.Structs
{
	#region Structs
	public struct Lexem
	{
		public int line;
		public int column;
		public int code;
		public string value;
		public LexemType type;
	}

	public struct Error
	{
		public int line;
		public int column;
		public string message;
		public Error(int _line, int _column, string _message="")
		{
			line = _line;
			column = _column;
			message = _message;
		}
	}

	public struct SyntaxError
	{
		private SyntaxErrorTypes errorType;
		private Error errorDefinition;
		public SyntaxError(SyntaxErrorTypes type, Error error)
		{
			errorType = type;
			errorDefinition = error;
			var message = new StringBuilder();
			switch (type)
			{
				case SyntaxErrorTypes.Semicolon:
					message.Append(@""";""");
					break;
				case SyntaxErrorTypes.Stick:
					message.Append(@"""|""");
					break;
				case SyntaxErrorTypes.OpenBracket:
					message.Append(@"""(""");
					break;
				case SyntaxErrorTypes.CloseBracket:
					message.Append(@""")""");
					break;
				case SyntaxErrorTypes.Colon:
					message.Append(@""":""");
					break;
				default:
					message.Append(Enum.GetName(typeof(SyntaxErrorTypes), type).ToUpper());
					break;
			}
			message.Append(" expected!\n");
			errorDefinition.message = message.ToString();
		}

		public override string ToString()
		{
			return $"Error in [{errorDefinition.line},{errorDefinition.column}]. {errorDefinition.message}";
		}

	}

	#endregion Structs
}
