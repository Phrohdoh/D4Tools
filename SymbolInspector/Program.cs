using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;

namespace D4Tools.SymbolInspector
{
	public class Program
	{
		static void ShowUsage() =>
			Console.WriteLine("SymbolInspector <your-unit.pas>");

		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				ShowUsage();
				return;
			}

			var filename = args[0];
			string text = null;

			try
			{
				text = File.ReadAllText(filename);
			}
			catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
			{
				Console.WriteLine(ex.Message);
				Environment.Exit(1);
			}

			// TODO: Resolve this ugly namespacing.
			var symbolTable = SymbolTableBuilder.SymbolTableBuilder.Create(text, filename);

			foreach (var unitName in symbolTable.UnitSymbols.Keys)
			{
				var unitSymbol = symbolTable.UnitSymbols[unitName];
				var hasInterface = unitSymbol.HasInterfaceSection;

				var comp = SyntaxFactory.CompilationUnit();
				comp = comp.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("System")));

				if (hasInterface && unitSymbol.InterfaceSectionSymbol.HasUsesClause)
				{
					var usingDirectives = new List<UsingDirectiveSyntax>();
					foreach (var uses in unitSymbol.InterfaceSectionSymbol.UsesClauseSymbol.GetUnitNames())
						usingDirectives.Add(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(uses)));

					comp = comp.AddUsings(usingDirectives.ToArray());
				}

				var namespaceBlock = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("WhatsInANamespace"));

				var staticClassDecl = SyntaxFactory.ClassDeclaration(unitSymbol.Name)
					.WithModifiers(SyntaxFactory.TokenList(
						new[] {
							SyntaxFactory.Token(SyntaxKind.PublicKeyword),
							SyntaxFactory.Token(SyntaxKind.StaticKeyword),
						}));

				if (hasInterface && unitSymbol.InterfaceSectionSymbol.HasLocalSymbols)
					foreach (var local in unitSymbol.InterfaceSectionSymbol.LocalSymbols.Where(l => l.HasType))
						staticClassDecl = staticClassDecl.AddMembers(local.ToFieldDeclaration());

				foreach (var methodDecl in unitSymbol.GetMethodImplementations())
					staticClassDecl = staticClassDecl.AddMembers(SyntaxFactory.MethodDeclaration(
						SyntaxFactory.ParseTypeName(methodDecl.IsProcedure ? "void" : methodDecl.ReturnType),
						methodDecl.Name).WithBody(SyntaxFactory.Block()));

				namespaceBlock = namespaceBlock.AddMembers(staticClassDecl);
				comp = comp.AddMembers(namespaceBlock);

				var syntaxNode = Formatter.Format(comp, new AdhocWorkspace());
				var sb = new StringBuilder();

				using (var writer = new StringWriter(sb))
					syntaxNode.WriteTo(writer);

				Console.WriteLine(sb.ToString());
			}
		}
	}
}