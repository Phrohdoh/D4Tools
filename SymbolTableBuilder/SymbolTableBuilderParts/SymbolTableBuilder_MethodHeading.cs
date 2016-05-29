using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder
	{
		List<ParameterSymbol> ResolveParameters(MethodHeadingNode node)
		{
			var ret = new List<ParameterSymbol>();

			var idents = new List<string>();

			ModKind? lastModKind = null;
			var lastParamType = "";

			// TODO: Parse single-param methods.
			foreach (var paramItemNode in node.ParameterListNode.Items.Select(item => item.ItemNode))
			{
				lastParamType = paramItemNode.TypeNode?.ToCode();

				foreach (var delimItem in paramItemNode.NameListNode?.Items)
				{
					var item = delimItem.ItemNode;
					if (item.Type == TokenType.Identifier)
						idents.Add(item.ToCode());
					else
						Console.WriteLine("Unresolved item type: {0}", item.Type);
				}

				switch (paramItemNode.ModifierNode?.Type)
				{
					case TokenType.VarKeyword:
						lastModKind = ModKind.Var;
					break;
					case TokenType.OutSemikeyword:
						lastModKind = ModKind.Out;
					break;
					case TokenType.ConstKeyword:
						lastModKind = ModKind.Const;
					break;
					/*
					case null:
						lastModKind = null;
						break;
					*/
					default:
						lastModKind = ModKind.None;
					break;
				}

				foreach (var ident in idents)
					ret.Add(new ParameterSymbol(ident)
					{
						Type = lastParamType,
						ModKind = lastModKind.Value,
						Ordinal = ret.Count,
						DefaultValue = paramItemNode.DefaultValueNode?.ToCode(),
					});

				idents.Clear();
				lastModKind = null;
				lastParamType = "";
			}

			return ret;
		}

		public enum MethodDeclOrImpl
		{
			Declaration,
			Implementation
		}

		// We can guarantee this is a Declaration because `VisitMethodImplementationNode` does not visit the heading node.
		public override void VisitMethodHeadingNode(MethodHeadingNode node) => CustomVisitMethodHeadingNode(node, MethodDeclOrImpl.Declaration);

		public void CustomVisitMethodHeadingNode(MethodHeadingNode node, MethodDeclOrImpl declOrImpl)
		{
			var methodName = node.NameNode.ToCode();
			var methodType = node.MethodTypeNode.Text;
			var returnType = node.ReturnTypeNode?.ToCode();
			var paramSymbols = ResolveParameters(node).ToArray();

			if (declOrImpl == MethodDeclOrImpl.Declaration)
			{
				MethodDeclarationSymbol symbol;

				if (CurrentUnitSymbol.MethodDeclarationsByName.TryGetValue(methodName, out symbol))
					symbol.Parameters = paramSymbols.ToArray();
				else
				{
					symbol = new MethodDeclarationSymbol(methodName)
					{
						Parameters = paramSymbols,
						ReturnType = returnType,
						MethodType = methodType,
					};
				}

				CurrentUnitSymbol.MethodDeclarationsByName.Add(methodName, symbol);
			}
			else
			{
				MethodImplementationSymbol symbol;

				if (CurrentUnitSymbol.MethodImplementationsByName.TryGetValue(methodName, out symbol))
					symbol.Parameters = paramSymbols;
				else
				{
					symbol = new MethodImplementationSymbol(methodName)
					{
						Parameters = paramSymbols,
						ReturnType = returnType,
						MethodType = methodType,
					};
				}

				CurrentUnitSymbol.MethodImplementationsByName.Add(methodName, symbol);
			}

			base.VisitMethodHeadingNode(node);
		}
	}
}