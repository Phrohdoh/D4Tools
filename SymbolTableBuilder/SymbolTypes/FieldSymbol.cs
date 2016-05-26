using System;
namespace D4Tools.SymbolTableBuilder
{
	public class FieldSymbol : IVarSymbol
	{
		public FieldSymbol(string name)
		{
			Name = name;
		}

		public ModKind ModKind { get; }

		public string Name { get; internal set; }

		public string Type { get; internal set; }

		public UnitSymbol ContainingUnit;
		public bool HasContainingUnit => ContainingUnit != null;
	}
}