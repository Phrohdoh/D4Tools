using System;
using System.IO;
using DGrok.Framework;
using DGrok.DelphiNodes;

namespace SyntaxFactoryDelphi
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var filename = args[0];
			var sourceText = File.ReadAllText(filename);
			var parser = Parser.FromText(sourceText, filename, CompilerDefines.CreateStandard(), new FileLoader());
			var ifNode = parser.ParseRule(RuleType.IfStatement) as IfStatementNode;
			Console.WriteLine(ifNode.Inspect());

			Console.WriteLine(ifNode.ConditionNode.Inspect());

			var cond = ifNode.ConditionNode;
			var then = ifNode.ThenStatementNode as BinaryOperationNode;

			//var newThen = new BinaryOperationNode(then.LeftNode, TokenFactory.ColonEquals(),

			var newIf = SyntaxFactory.IfStatement(cond, null, null);
			Console.WriteLine(newIf.Inspect());
		}
	}
}