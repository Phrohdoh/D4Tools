namespace D4Tools.SymbolTableBuilder
{
	public interface IVariableSymbol : INamedSymbol
	{
		string Type { get; }
		bool HasType { get; }
	}

	public class VariableDeclarationSymbol : IVariableSymbol, IFieldSymbol
	{
		public string Type { get; internal set; }
		public bool HasType => !string.IsNullOrWhiteSpace(Type);

		public SymbolKind SymbolKind => SymbolKind.VariableDeclaration;
		public FieldKind FieldKind { get; internal set; }

		public string Name { get; internal set; }
		public VariableDeclarationSymbol(string name)
		{
			Name = name;
		}
	}
}