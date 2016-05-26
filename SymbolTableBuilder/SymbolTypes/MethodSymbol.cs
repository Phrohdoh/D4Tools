namespace D4Tools.SymbolTableBuilder
{
	public interface IMethodSymbol : ISymbol
	{
		string ReturnType { get; }
		bool IsProcedure { get; }
		ParameterSymbol[] Parameters { get; }
		int ParameterCount { get; }
		bool HasParameters { get; }
	}

	public class MethodDeclarationSymbol : IMethodSymbol
	{
		public string Name { get; internal set; }

		/// <summary>
		/// Returns `MethodImplementation` of the type `SymbolKind`.
		/// </summary>
		public SymbolKind SymbolKind => SymbolKind.MethodDeclaration;

		public string ReturnType { get; internal set; }

		/// <summary>
		/// Akin to 'void' in C#.
		/// Does not have a return type.
		/// </summary>
		public bool IsProcedure => ReturnType == null;

		// TODO: Make this an ImmutableArray<ParameterSymbol>
		public ParameterSymbol[] Parameters { get; internal set; } = new ParameterSymbol[0];
		public int ParameterCount => Parameters?.Length ?? 0;
		public bool HasParameters => ParameterCount != 0;

		public MethodDeclarationSymbol(string name)
		{
			Name = name;
		}
	}

	public class MethodImplementationSymbol : IMethodSymbol
	{
		public string Name { get; internal set; }

		/// <summary>
		/// Returns `MethodImplementation` of the type `SymbolKind`.
		/// </summary>
		public SymbolKind SymbolKind => SymbolKind.MethodImplementation;

		public string ReturnType { get; internal set; }

		/// <summary>
		/// Akin to 'void' in C#.
		/// Does not have a return type.
		/// </summary>
		public bool IsProcedure => ReturnType == null;

		public ParameterSymbol[] Parameters { get; internal set; } = new ParameterSymbol[0];
		public int ParameterCount => Parameters?.Length ?? 0;
		public bool HasParameters => ParameterCount != 0;

		/// <summary>
		/// An array of locals declared in the `VarDeclSection`.
		/// </summary>
		public LocalSymbol[] Locals { get; internal set; } = new LocalSymbol[0];
		public int LocalCount => Locals?.Length ?? 0;
		public bool HasLocals => LocalCount != 0;

		bool HasAnyParamsWithModKind(ModKind mod)
		{
			if (!HasParameters)
				return false;

			foreach (var parameter in Parameters)
				if (parameter.ModKind == mod)
					return true;

			return false;
		}

		public bool HasOutputParameters => HasAnyParamsWithModKind(ModKind.Out);
		public bool HasVarParameters => HasAnyParamsWithModKind(ModKind.Var);
		public bool HasConstParameters => HasAnyParamsWithModKind(ModKind.Const);

		public MethodImplementationSymbol(string name)
		{
			Name = name;
		}
	}
}