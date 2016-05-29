namespace D4Tools.SymbolTableBuilder
{
	// TODO: Move symbol types to `D4Tools.Symbols` namespace.
	public interface ISymbol
	{
		SymbolKind SymbolKind { get; }

		/* TODO
		Location DefinitionLocation { get; }
		void Accept(SymbolVisitor visitor);
		string ToDisplayString();
		*/
	}

	public enum SymbolKind : byte
	{
		Local,
		MethodDeclaration,
		MethodImplementation,
		Parameter,
		Unit,
		VariableDeclaration,
		UnitInterfaceSection,
		UnitImplementationSection,
	}

	public interface INamedSymbol : ISymbol
	{
		string Name { get; }
	}
}