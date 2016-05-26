using System;
using DGrok.Framework;
using System.Collections.Generic;

namespace D4Tools.SymbolTableBuilder
{
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
	}
}