using System.Collections.Generic;
using System.Linq;

namespace D4Tools.SymbolTableBuilder
{
	public interface IUnitSymbol : INamedSymbol
	{
		/// <summary>
		/// Gets an array of <see cref="MethodDeclarationSymbol"/>s with no guarantee of order.
		/// </summary>
		MethodDeclarationSymbol[] GetMethodDeclarations();
		Dictionary<string, MethodDeclarationSymbol> MethodDeclarationsByName { get; }

		/// <summary>
		/// Gets an array of <see cref="MethodImplementationSymbol"/>s with no guarantee of order.
		/// </summary>
		MethodImplementationSymbol[] GetMethodImplementations();
		Dictionary<string, MethodImplementationSymbol> MethodImplementationsByName { get; }
	}

	public class UnitSymbol : IUnitSymbol
	{
		Dictionary<string, MethodDeclarationSymbol> methodDeclarationsByName = new Dictionary<string, MethodDeclarationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodDeclarationSymbol> MethodDeclarationsByName => methodDeclarationsByName;
		public MethodDeclarationSymbol[] GetMethodDeclarations() => methodDeclarationsByName.Values.ToArray();

		Dictionary<string, MethodImplementationSymbol> methodImplementationsByName = new Dictionary<string, MethodImplementationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodImplementationSymbol> MethodImplementationsByName => methodImplementationsByName;
		public MethodImplementationSymbol[] GetMethodImplementations() => methodImplementationsByName.Values.ToArray();

		public UnitInterfaceSectionSymbol InterfaceSectionSymbol { get; internal set; }
		public bool HasInterfaceSection => InterfaceSectionSymbol != null;

		public UnitImplementationSectionSymbol ImplementationSectionSymbol { get; internal set; }
		public bool HasImplementationSection => ImplementationSectionSymbol != null;

		public SymbolKind SymbolKind => SymbolKind.Unit;

		public string Name { get; internal set; }
		public UnitSymbol(string name)
		{
			Name = name;
		}
	}
}