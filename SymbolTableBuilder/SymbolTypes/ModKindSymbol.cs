using System;
namespace D4Tools.SymbolTableBuilder
{
	public interface IModKindSymbol : INamedSymbol
	{
		ModKind ModKind { get; }
	}
}