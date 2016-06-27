using System;
using System.Collections.Generic;
using System.Linq;

namespace D4Tools.SymbolTableBuilder
{
	public interface ITypeDeclarationContainer
	{
		TypeDeclarationSymbol[] GetTypeDeclarations();

		bool TryGetTypeDeclaration(string name, TypeDeclarationKind kind, out TypeDeclarationSymbol typeDecl);
		bool HasTypeDeclaration(string name, TypeDeclarationKind kind);
		void AddTypeDeclaration(TypeDeclarationSymbol sym);
	}

	public interface IUnitSymbol : INamedSymbol,
		ITypeDeclarationContainer
	{ }

	public class UnitSymbol : IUnitSymbol
	{
		internal void AddMethodDeclaration(MethodDeclarationSymbol sym) => methodDeclarationsByName.Add(sym.Name, sym);
		readonly Dictionary<string, MethodDeclarationSymbol> methodDeclarationsByName = new Dictionary<string, MethodDeclarationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodDeclarationSymbol> MethodDeclarationsByName => methodDeclarationsByName;
		public MethodDeclarationSymbol[] GetMethodDeclarations() => methodDeclarationsByName.Values.ToArray();

		internal void AddMethodImplementation(MethodImplementationSymbol sym) => methodImplementationsByName.Add(sym.Name, sym);
		readonly Dictionary<string, MethodImplementationSymbol> methodImplementationsByName = new Dictionary<string, MethodImplementationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodImplementationSymbol> MethodImplementationsByName => methodImplementationsByName;
		public MethodImplementationSymbol[] GetMethodImplementations() => methodImplementationsByName.Values.ToArray();

		#region ITypeDeclarationContainer
		readonly Dictionary<string, TypeDeclarationSymbol> typeDeclarationsByName = new Dictionary<string, TypeDeclarationSymbol>(SymbolTableBuilder.StringComparer);
		public void AddTypeDeclaration(TypeDeclarationSymbol sym) => typeDeclarationsByName.Add(sym.Name, sym);

		public bool TryGetTypeDeclaration(string name, TypeDeclarationKind kind, out TypeDeclarationSymbol typeDecl)
		{
			if (!typeDeclarationsByName.TryGetValue(name, out typeDecl))
				return false;

			return kind == TypeDeclarationKind.Any || typeDecl.TypeDeclarationKind == kind;
		}

		public bool HasTypeDeclaration(string name, TypeDeclarationKind kind)
		{
			TypeDeclarationSymbol typeDecl;
			if (!TryGetTypeDeclaration(name, kind, out typeDecl))
				return false;

			return kind == TypeDeclarationKind.Any || typeDecl.TypeDeclarationKind == kind;
		}

		public TypeDeclarationSymbol[] GetTypeDeclarations() => typeDeclarationsByName.Values.ToArray();
		#endregion

		public UnitInterfaceSectionSymbol InterfaceSectionSymbol { get; internal set; }
		public bool HasInterfaceSection => InterfaceSectionSymbol != null;

		public UnitImplementationSectionSymbol ImplementationSectionSymbol { get; internal set; }
		public bool HasImplementationSection => ImplementationSectionSymbol != null;

		public SymbolKind SymbolKind => SymbolKind.UnitDeclaration;

		public string Name { get; internal set; }
		public UnitSymbol(string name)
		{
			Name = name;
		}
	}
}