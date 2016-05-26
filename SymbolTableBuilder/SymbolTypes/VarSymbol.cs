using System;
namespace D4Tools.SymbolTableBuilder
{
	public interface IVarSymbol
	{
		string Name { get; }
		string Type { get; }
		ModKind ModKind { get; }
	}
}