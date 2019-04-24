using System;
using System.Collections.Generic;
using System.Text;

namespace Syntaxer
{
	public class Node
	{
		public string Data { get; protected set; }
		public int Level { get; protected set; }
		public List<Node> Children { get; protected set; }
		public Node(string data, int level)
		{
			if (!String.IsNullOrEmpty(data))
				Data = data;
			if (level >= 0)
				Level = level;
			Children = new List<Node>();
		}

		public void Add(Node item)
		{
			Children.Add(item);
		}

		public override string ToString()
		{
			var result = new StringBuilder();
			for (int i = 0; i < Level; i++)
			{
				result.Append("| ");
			}
			result.Append($"{Data}\n");
			foreach (var item in Children)
			{
				result.Append(item.ToString());
			}
			return result.ToString();
		}
	}
}
