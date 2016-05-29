using System.Collections.Generic;

namespace D4Tools.SymbolTableBuilder
{
	public interface IUsesClauseSymbol : ISymbol
	{
		string[] GetUnitNames();
	}

	public class UsesClauseSymbol
	{
		List<string> unitNames = new List<string>();

		/// <summary>
		/// Gets a document-ordered array of unit names.
		/// </summary>
		public string[] GetUnitNames() => unitNames.ToArray();

		public UnitInterfaceSectionSymbol ContainingInterfaceSymbol { get; }
		public bool IsInInterfaceSection => ContainingInterfaceSymbol != null && ContainingImplementationSymbol == null;

		public UnitImplementationSectionSymbol ContainingImplementationSymbol { get; }
		public bool IsInImplementationSection => ContainingImplementationSymbol != null && ContainingInterfaceSymbol == null;

		public UsesClauseSymbol(IEnumerable<string> initialUnitNames, UnitInterfaceSectionSymbol containingInterfaceSymbol)
		{
			unitNames = new List<string>(initialUnitNames);
			ContainingInterfaceSymbol = containingInterfaceSymbol;
		}

		public UsesClauseSymbol(IEnumerable<string> initialUnitNames, UnitImplementationSectionSymbol containingImplementationSymbol)
		{
			unitNames = new List<string>(initialUnitNames);
			ContainingImplementationSymbol = containingImplementationSymbol;
		}
	}
}