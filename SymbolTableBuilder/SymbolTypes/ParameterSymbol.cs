namespace D4Tools.SymbolTableBuilder
{
	public class ParameterSymbol : IVariableSymbol
	{
		public string Name { get; internal set; }

		/// <summary>
		/// Returns `Parameter` of the type `SymbolKind`.
		/// </summary>
		public SymbolKind SymbolKind => SymbolKind.MethodParameter;

		public string Type { get; internal set; }
		public bool HasType => Type != null;

		public ModKind ModKind { get; internal set; }

		/// <summary>
		/// Gets the ordinal position of the parameter.
		/// <para>The first parameter has ordinal zero.</para>
		/// </summary>
		public int Ordinal { get; internal set; }

		/// <summary>
		/// A parameter is optional iff the method implementation provides a default value.
		/// Returns `false` iff DefaultValue is null.
		/// </summary>
		// TODO: Replace this with an ArgumentKind enum instance -- http://source.roslyn.io/#Microsoft.CodeAnalysis/Compilation/IExpression.cs,109
		public bool IsOptional => DefaultValue != null;

		/// <summary>
		/// A string representation of the default value.
		/// null indicates that this parameter does not have a default value.
		/// </summary>
		public string DefaultValue { get; internal set; }

		public ParameterSymbol(string name)
		{
			Name = name;
		}
	}
}
