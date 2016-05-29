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

		public static MethodDeclarationSyntax ToMethodDeclaration(this MethodImplementationSymbol impl)
		{
			return SyntaxFactory.MethodDeclaration(
					SyntaxFactory.ParseTypeName(impl.ReturnType ?? "void"),
					impl.Name)
				.WithBody(
					SyntaxFactory.Block()
						.WithCloseBraceToken(
							SyntaxFactory.Token(
								SyntaxFactory.TriviaList(
									SyntaxFactory.Comment(string.Format("// TODO: Method body for '{0}'\r\n", impl.Name))
								),
								SyntaxKind.CloseBraceToken,
								SyntaxFactory.TriviaList()
							)
						)
					);
		}
	}
}