using System;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{

	public enum ModKind : byte
	{
		None = 0,

		/// <summary>
		/// 'var' is akin to 'ref' in C#.
		/// </summary>
		Var = 1,

		/// <summary>
		/// 'out' has the same meaning in D4 as it does in C#.
		/// </summary>
		Out = 2,

		/// <summary>
		/// TODO: Investigate what this does
		/// </summary>
		Const = 3,
	}
	
}