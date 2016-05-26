namespace D4Tools.SymbolTableBuilder
{
	public class LocalSymbol : ISymbol
	{
		public string Name { get; internal set; }

		/// <summary>
		/// Returns `Local` of the type `SymbolKind`.
		/// </summary>
		public SymbolKind SymbolKind => SymbolKind.Local;

		public string Type { get; internal set; }

		public LocalSymbol(string name)
		{
			Name = name;
		}
	}
}