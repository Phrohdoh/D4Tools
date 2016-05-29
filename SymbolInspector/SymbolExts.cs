using D4Tools.SymbolTableBuilder;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace D4Tools.SymbolInspector
{
	public static class SymbolExts
	{
		public static VariableDeclarationSyntax ToVariableDeclaration(this LocalSymbol local)
		{
			return SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName(local.Type))
				.WithVariables(
					variables: SyntaxFactory.SingletonSeparatedList(
						node: SyntaxFactory.VariableDeclarator(local.Name)
					)
				);
		}

		public static FieldDeclarationSyntax ToFieldDeclaration(this LocalSymbol local)
		{
			return SyntaxFactory.FieldDeclaration(local.ToVariableDeclaration());
		}
	}
}
