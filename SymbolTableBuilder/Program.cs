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
			catch (FileNotFoundException ex)
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
				Console.WriteLine($"Parsed unit {unitName} with {unitSymbol.MethodImplementationsByName.Count} Method Impls.");

				foreach (var methodImpl in unitSymbol.GetMethodImplementations())
				{
					Console.WriteLine($">> {methodImpl.Name}");

					Console.WriteLine($"Param count: {methodImpl.ParameterCount} (has Parameters? {methodImpl.HasParameters})");
					Console.WriteLine($"Local count: {methodImpl.LocalCount} (has Locals? {methodImpl.HasLocals})");

					if (methodImpl.ParameterCount != 0)
						foreach (var parameter in methodImpl.Parameters)
							Console.WriteLine($"{parameter.Name}: {parameter.Type} (optional? {parameter.IsOptional.ToString().ToLower()})");

					if (methodImpl.LocalCount != 0)
						foreach (var local in methodImpl.Locals)
							Console.WriteLine($"{local.Name}: {local.Type}");
				}
			}
		}
	}
}
