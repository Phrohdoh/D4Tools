using System.Collections.Generic;

namespace D4Tools.SymbolTableBuilder
{
	public enum TypeDeclarationKind
	{
		Unknown,
		Any,

		ExistingType,
		TypeExistingType,
		EnumValues,
		Expression,
		ExistingTypePointer,
		ArrayOfExistingType,
		Class,
		ClassOfExistingType,
		DispatchInterface,
		FileOfExistingType,
		Function,
		Interface,
		Object,
		Procedure,
		Record,
		SetOfOrdinalValue,

		// TODO: What is this?
		ForwardTypeDeclaration,
	}

	public interface ITypeDeclarationSymbol : INamedSymbol
	{
		TypeDeclarationKind TypeDeclarationKind { get; }
	}

	public class TypeDeclarationSymbol : INamedSymbol
	{
		/// <summary>Returns 'TypeDeclaration' of the 'SymbolKind'.</summary>
		public SymbolKind SymbolKind => SymbolKind.TypeDeclaration;

		public TypeDeclarationKind TypeDeclarationKind { get; internal set; }

		internal void AddMethodDecl(MethodDeclarationSymbol methodDecl) => methodDeclaraions.Add(methodDecl);
		readonly List<MethodDeclarationSymbol> methodDeclaraions = new List<MethodDeclarationSymbol>();

		public MethodDeclarationSymbol[] MethodDeclarationSymbols => methodDeclaraions.ToArray();
		public bool HasMethodDeclarations => methodDeclaraions.Count > 0;

		public string Name { get; internal set; }
		public TypeDeclarationSymbol(string name, TypeDeclarationKind kind)
		{
			Name = name;
			TypeDeclarationKind = kind;
		}
	}
}