namespace D4Tools.SymbolTableBuilder
{
	public enum UnitSectionKind
	{
		Unknown,
		Interface,
		Implementation,
		Initialization,
		Finalization,
	}

	public interface IUnitSectionSymbol : ISymbol
	{
		UnitSectionKind UnitSectionKind { get; }
	}

	public interface IUnitInterfaceSymbol : IUnitSectionSymbol
	{
		UsesClauseSymbol UsesClauseSymbol { get; }
		bool HasUsesClause { get; }

		MethodDeclarationSymbol[] MethodDeclarationSymbols { get; }
		bool HasMethodDeclarations { get; }

		LocalSymbol[] LocalSymbols { get; }
		bool HasLocalSymbols { get; }

		TypeDeclarationSymbol[] TypeDeclarationSymbols { get; }
		bool HasTypeDeclarationSymbols { get; }
	}

	public class UnitInterfaceSectionSymbol : IUnitInterfaceSymbol
	{
		public UnitSectionKind UnitSectionKind => UnitSectionKind.Interface;

		public UsesClauseSymbol UsesClauseSymbol { get; internal set; }
		public bool HasUsesClause => UsesClauseSymbol != null;

		public SymbolKind SymbolKind => SymbolKind.UnitInterfaceSection;

		public MethodDeclarationSymbol[] MethodDeclarationSymbols { get; internal set; } = new MethodDeclarationSymbol[0];
		public bool HasMethodDeclarations => MethodDeclarationSymbols?.Length > 0;

		public LocalSymbol[] LocalSymbols { get; internal set; } = new LocalSymbol[0];
		public bool HasLocalSymbols => LocalSymbols?.Length > 0;

		public TypeDeclarationSymbol[] TypeDeclarationSymbols { get; internal set; } = new TypeDeclarationSymbol[0];
		public bool HasTypeDeclarationSymbols => TypeDeclarationSymbols?.Length > 0;
	}

	public interface IUnitImplementationSymbol : IUnitSectionSymbol
	{
		UsesClauseSymbol UsesClauseSymbol { get; }
		bool HasUsesClause { get; }
		
		MethodImplementationSymbol[] MethodImplementationSymbols { get; }
		bool HasMethodImplementations { get; }
	}

	public class UnitImplementationSectionSymbol : IUnitImplementationSymbol
	{
		public UnitSectionKind UnitSectionKind => UnitSectionKind.Implementation;

		public UsesClauseSymbol UsesClauseSymbol { get; internal set; }
		public bool HasUsesClause => UsesClauseSymbol != null;

		public SymbolKind SymbolKind => SymbolKind.UnitImplementationSection;

		public MethodImplementationSymbol[] MethodImplementationSymbols { get; internal set; } = new MethodImplementationSymbol[0];
		public bool HasMethodImplementations => MethodImplementationSymbols?.Length > 0;
	}
}