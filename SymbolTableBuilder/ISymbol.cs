using System;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public interface ISymbol
	{
		string Name { get; }
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
		UnitVarDecl,
	}
}