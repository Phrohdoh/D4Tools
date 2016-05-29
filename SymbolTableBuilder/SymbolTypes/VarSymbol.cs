namespace D4Tools.SymbolTableBuilder
{
	public interface IVariableSymbol : INamedSymbol
	{
		string Type { get; }
		bool HasType { get; }
	}

	public class VariableDeclarationSymbol : IVariableSymbol
	{
		public string Type { get; internal set; }
		public bool HasType => !string.IsNullOrWhiteSpace(Type);

		public SymbolKind SymbolKind => SymbolKind.VariableDeclaration;

		public string Name { get; internal set; }
		public VariableDeclarationSymbol(string name)
		{
			Name = name;
		}
	}
}