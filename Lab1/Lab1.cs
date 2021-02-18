using System;
using System.Collections.Generic;
using System.IO;

//Variant 8 - Piotr Shishkov FAF - 193
// Vn = {S,D,E,J},
// Vt = {a,b,c,d,e},
// P = { S-aD, D-dE, D-bJ, J-cS, E-e, E-aE, D-aE }


namespace LFPC_Lab1
{
	class GrammarValue
	{
		public string TerminalValue;
		public string NonTerminalValue;

		public GrammarValue(string termValue)
		{
			TerminalValue = termValue;
			NonTerminalValue = "#";
		}

		public GrammarValue(string termValue, string nonTermValue)
		{
			TerminalValue = termValue;
			NonTerminalValue = nonTermValue;
		}
	}

	static class Program
	{
		static void Main(string[] args)
		{
			string grammarInput =
				"Vn={S, D, E, J}" +
				"Vt={a, b, c, d, e}" +
				"P={" +
				"1. S-aD" +
				"2. D-dE" +
				"3. D-bJ" +
				"4. J-cS" +
				"5. E-e" +
				"6. E-aE" +
				"7. D-aE}";

			//Converting grammar to dictionary
			var dict = grammarInput.ConvertToDictionary();
			//Convert dictionary to dot visualisation file
			ConvertToDot(dict);

			//Checking grammar
			var input = "ade";
			var nonTermValue = "S";
			CheckGrammar(dict, input, nonTermValue);
		}

		static void CheckGrammar(Dictionary<string, GrammarValue[]> dict, string input, string nonTerminalValue)
		{
			for (int i = 0; i < input.Length; i++)
			{
				if (nonTerminalValue != null)
				{
					GrammarValue[] gValue;
					var hasValue = dict.TryGetValue(nonTerminalValue, out gValue);
					if (hasValue)
					{
						foreach (var value in dict[nonTerminalValue])
						{
							if (value.TerminalValue[0] == input[i]) nonTerminalValue = value.NonTerminalValue;
						}
					}
				}
				if (nonTerminalValue == "#" && (input.Length - 1) - i != 0)
				{
					nonTerminalValue = null;
					break;
				}
			}
			if (nonTerminalValue == null || nonTerminalValue != "#") Console.WriteLine("Rejected");
			else Console.WriteLine("Approved");

		}

		static void ConvertToDot(Dictionary<string, GrammarValue[]> dict)
		{
			string content = "";
			string dotPrefix = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><!--Created with JFLAP 7.1.--><structure>\n" +
			                   "<type>grammar</type>\n";

			content += dotPrefix;
			foreach (var key in dict.Keys)
			{
				foreach (var value in dict[key])
				{
					content += $"<production>\n<left>{key}</left>\n<right>{value.TerminalValue}{(value.NonTerminalValue == "#" ? "" : value.NonTerminalValue)}</right>\n</production>\n";
				}
			}

			content += "</structure>";
			File.WriteAllText("output.jff", content);
		}

		//String extension
		static Dictionary<string, GrammarValue[]> ConvertToDictionary(this string input)
		{
			Dictionary<string, GrammarValue[]> dict = new Dictionary<string, GrammarValue[]>();
			int startPos = input.IndexOf('P');
			string[] prods = input.Substring(startPos).Split('.');
			for (int i = 1; i < prods.Length; i++)
			{
				prods[i] = prods[i].Remove(prods[i].Length-1, 1);
				prods[i] = prods[i].Replace(" ", "");

				if (dict.ContainsKey(prods[i][0] + ""))
				{
					var val = dict[prods[i][0] + ""];
					var length = val.Length;
					var gramArray = val.Clone();
					var temp = new GrammarValue[length + 1];

					for (int j = 0; j < length; j++)
					{
						temp[j] = val[j];
					}

					temp[length] = new GrammarValue(prods[i][2] + "", prods[i].Length == 4 ? prods[i][3] + "" : "#");
					dict[prods[i][0]+""] = temp;
				}
				else
				{
					dict[prods[i][0] + ""] = new GrammarValue[]
						{new GrammarValue(prods[i][2] + "", prods[i].Length == 4 ? prods[i][3] + "" : "#")};
				}
			}

			return dict;
		}
	}
}
