using System;
using System.Diagnostics;
using System.IO;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public class Program
	{
		static void ShowUsage() =>
			Console.WriteLine("SymbolTableBuilder <your-unit.pas>");

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

			var parser = Parser.FromText(text, filename, CompilerDefines.CreateStandard(), new FileLoader());
			AstNode rootNode = null;

			try
			{
				rootNode = parser.ParseRule(RuleType.Goal);
			}
			catch (ParseException ex)
			{
				Console.WriteLine(ex.Message);
				Environment.Exit(2);
			}

			Debug.Assert(rootNode != null);

			var symbolTable = SymbolTableBuilder.Create(rootNode);
			foreach (var unitName in symbolTable.UnitSymbols.Keys)
			{
				var unitSymbol = symbolTable.UnitSymbols[unitName];

				foreach (var methodDecl in unitSymbol.GetMethodDeclarations())
				{
					Console.WriteLine($">> {methodDecl.Name} (decl)");

					Console.WriteLine($"Param count: {methodDecl.ParameterCount} (has Parameters? {methodDecl.HasParameters})");
					foreach (var parameter in methodDecl.Parameters)
						Console.WriteLine($"{parameter.Name}: {parameter.Type} (optional? {parameter.IsOptional})");
				}

				foreach (var methodImpl in unitSymbol.GetMethodImplementations())
				{
					Console.WriteLine($">> {methodImpl.Name} (impl)");

					Console.WriteLine($"Param count: {methodImpl.ParameterCount} (has Parameters? {methodImpl.HasParameters})");
					foreach (var parameter in methodImpl.Parameters)
						Console.WriteLine($"{parameter.Name}: {(parameter.HasType ? parameter.Type : "<none>")} (optional? {parameter.IsOptional})");

					Console.WriteLine($"Local count: {methodImpl.LocalCount} (has Locals? {methodImpl.HasLocals})");
					foreach (var local in methodImpl.Locals)
						Console.WriteLine($"{local.Name}: {local.Type}");
				}
			}
		}
	}
}