using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder
	{
		public override void VisitMethodHeadingNode(MethodHeadingNode node)
		{
			Debug.Assert(node != null);
			Debug.Assert(node.NameNode != null);

			// Note: This fails on some methods for reasons I have not figured out yet.
			//Debug.Assert(node.NameNode is BinaryOperationNode);

			var methodName = node.NameNode.ToCode();
			var returnType = node.ReturnTypeNode?.ToCode();

			var idents = new List<string>();
			var parameterSymbols = new List<ParameterSymbol>();

			ModKind? lastModKind = null;
			var lastParamType = "";

			// TODO: Parse single-param methods.
			foreach (var paramItemNode in node.ParameterListNode.Items.Select(item => item.ItemNode))
			{
				//Debug.Assert(paramItemNode.TypeNode != null);
				if (paramItemNode.TypeNode == null)
				{
					Console.WriteLine("Skipping untyped method param, we don't handle this yet.");
					lastModKind = null;
					lastParamType = "";
					continue;
				}

				lastParamType = paramItemNode.TypeNode.ToCode();

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
					parameterSymbols.Add(new ParameterSymbol(ident)
					{
						Type = lastParamType,
						ModKind = lastModKind.Value,
						Ordinal = parameterSymbols.Count,
						DefaultValue = paramItemNode.DefaultValueNode?.ToCode(),
					});

				idents.Clear();
				lastModKind = null;
				lastParamType = "";
			}

			var paramArr = parameterSymbols.ToArray();

			// TODO:
			//
			// Do this much earlier.
			// If `existingMethodSymbol` is valid then just update it instead of
			// constructing a new MethodSymbol.

			MethodImplementationSymbol methodImplSymbol;
			if (CurrentUnitSymbol.MethodImplementationsByName.TryGetValue(methodName, out methodImplSymbol))
				methodImplSymbol.Parameters = paramArr;
			else
			{
				methodImplSymbol = new MethodImplementationSymbol(methodName)
				{
					Parameters = paramArr,
					ReturnType = returnType,
				};

				CurrentUnitSymbol.MethodImplementationsByName.Add(methodName, methodImplSymbol);
			}
		}
	}
}