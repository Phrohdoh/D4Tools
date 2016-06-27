namespace D4Tools.SymbolTableBuilder
{
	public enum FieldKind
	{
		Unknown,
		Any,

		ArrayOf,
		FileOf,
		Pointer,
		SimpleType,
	}

	public interface IFieldSymbol
	{
		FieldKind FieldKind { get; }
	}

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

		public SymbolKind SymbolKind => SymbolKind.UnitOrMethodLocal;
		public FieldKind FieldKind { get; internal set; }

		public string Type { get; internal set; }
		public bool HasType => !string.IsNullOrWhiteSpace(Type);
	}
}