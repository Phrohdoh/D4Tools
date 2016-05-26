using System.Collections.Generic;
using System.Linq;

namespace D4Tools.SymbolTableBuilder
{
	public interface IUnitSymbol : ISymbol
	{
		/// <summary>
		/// Gets an array of <see cref="MethodDeclarationSymbol"/>s with no guarantee of order.
		/// </summary>
		MethodDeclarationSymbol[] GetMethodDeclarations();

		/// <summary>
		/// Gets an array of <see cref="MethodImplementationSymbol"/>s with no guarantee of order.
		/// </summary>
		MethodImplementationSymbol[] GetMethodImplementations();

		Dictionary<string, MethodDeclarationSymbol> MethodDeclarationsByName { get; }
		Dictionary<string, MethodImplementationSymbol> MethodImplementationsByName { get; }
	}

	public class UnitSymbol : IUnitSymbol
	{
		Dictionary<string, MethodDeclarationSymbol> methodDeclarationsByName = new Dictionary<string, MethodDeclarationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodDeclarationSymbol> MethodDeclarationsByName => methodDeclarationsByName;

		Dictionary<string, MethodImplementationSymbol> methodImplementationsByName = new Dictionary<string, MethodImplementationSymbol>(SymbolTableBuilder.StringComparer);
		public Dictionary<string, MethodImplementationSymbol> MethodImplementationsByName => methodImplementationsByName;

		public string Name { get; internal set; }

		public SymbolKind SymbolKind => SymbolKind.Unit;

		public MethodDeclarationSymbol[] GetMethodDeclarations() => methodDeclarationsByName.Values.ToArray();

		public MethodImplementationSymbol[] GetMethodImplementations() => methodImplementationsByName.Values.ToArray();

		public UnitSymbol(string name)
		{
			Name = name;
		}
	}
}