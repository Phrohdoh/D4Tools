using System;
using System.Collections.Generic;
using System.Diagnostics;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	// TODO: SymbolTableBuilder should be an implementation detail that consumers know nothing about.
	public partial class SymbolTableBuilder : Visitor
	{
#if CASE_SENSITIVE_SYMBOL_IDENTIFIERS
		public static StringComparer StringComparer = StringComparer.InvariantCulture;
		public static StringComparison StringComparison = StringComparison.InvariantCulture;
#else
		public static StringComparer StringComparer = StringComparer.InvariantCultureIgnoreCase;
		public static StringComparison StringComparison = StringComparison.InvariantCultureIgnoreCase;
#endif

		public bool IsCaseSensitive => StringComparer == StringComparer.InvariantCulture;

		public Dictionary<string, UnitSymbol> UnitSymbols = new Dictionary<string, UnitSymbol>(StringComparer);

		public static SymbolTableBuilder Create(AstNode rootNode)
		{
			var stb = new SymbolTableBuilder();
			stb.Visit(rootNode);
			return stb;
		}

		public static SymbolTableBuilder Create(string sourceText, string filename)
		{
			var parser = Parser.FromText(sourceText, filename, CompilerDefines.CreateStandard(), new FileLoader());
			var rootNode = parser.ParseRule(RuleType.Goal);
			Debug.Assert(rootNode != null);
			return Create(rootNode);
		}
	}
}