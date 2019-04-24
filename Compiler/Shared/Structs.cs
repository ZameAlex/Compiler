using System;
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
		public Error(int _line, int _column, string _message)
		{
			line = _line;
			column = _column;
			message = _message;
		}
	}
	#endregion Structs
}
