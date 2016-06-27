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
		UnitOrMethodLocal,
		MethodParameter,
		MethodDeclaration,
		MethodImplementation,
		UnitDeclaration,
		VariableDeclaration,
		UnitInterfaceSection,
		UnitImplementationSection,
		TypeDeclaration,

		ArrayOf,
	}

	public interface INamedSymbol : ISymbol
	{
		string Name { get; }
	}

	public enum AccessabilityLevel
	{
		Unknown,
		Any,

		Public,
		Private,
		Protected,
		Published,
	}

	public interface IAccessabilityLevelSymbol
	{
		AccessabilityLevel AccessabilityLevel { get; }
	}
}