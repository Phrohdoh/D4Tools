namespace D4Tools.SymbolTableBuilder
{
	/// <summary>
	/// Represents a variable (or const) that is 'local' to a method or a unit.
	/// </summary>
	public class LocalSymbol : IVariableSymbol, IModKindSymbol
	{
		public string Name { get; internal set; }
		public LocalSymbol(string name)
		{
			Name = name;
		}

		public ModKind ModKind { get; }

		public SymbolKind SymbolKind => SymbolKind.Local;


		public string Type { get; internal set; }
		public bool HasType => !string.IsNullOrWhiteSpace(Type);
	}
}