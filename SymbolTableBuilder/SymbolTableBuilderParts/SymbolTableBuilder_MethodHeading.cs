using System;
using System.Collections.Generic;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
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

		public override void VisitMethodHeadingNode(MethodHeadingNode node)
		{
			var methodName = node.NameNode.ToCode();
			var methodType = node.MethodTypeNode.Text;
			var returnType = node.ReturnTypeNode?.ToCode();
			var paramSymbols = ResolveParameters(node).ToArray();
			var accessLevel = AccessabilityLevel.Unknown;
			var parentAccessability = node.ParentNodeOfType<VisibilitySectionNode>();

			if (parentAccessability != null)
			{
				var accessLevelText = parentAccessability?.VisibilityNode?.VisibilityKeywordNode?.Text;
				if (string.IsNullOrWhiteSpace(accessLevelText))
					Console.WriteLine($"Unknown accessability at {parentAccessability.Location}");
				else if (!Enum.TryParse(accessLevelText, true, out accessLevel))
					Console.WriteLine($"Cannot parse '{accessLevelText}' into an {nameof(AccessabilityLevel)} at {parentAccessability.Location}");
			}

			var methodDecl = new MethodDeclarationSymbol(methodName, accessLevel)
			{
				Parameters = paramSymbols,
				ReturnType = returnType,
				MethodType = methodType,
			};

			var parentTypeDecl = node.ParentNodeOfType<TypeDeclNode>();
			if (parentTypeDecl != null)
			{
				var typeDeclName = parentTypeDecl.NameNode.Text;
				if (typeDeclName.Contains("."))
					typeDeclName = typeDeclName.Substring(typeDeclName.IndexOf('.'));

				TypeDeclarationSymbol typeDecl;
				if (!CurrentUnitSymbol.TryGetTypeDeclaration(typeDeclName, TypeDeclarationKind.Record, out typeDecl))
				{
					typeDecl = new TypeDeclarationSymbol(typeDeclName, TypeDeclarationKind.Record);
					CurrentUnitSymbol.AddTypeDeclaration(typeDecl);
				}

				typeDecl.AddMethodDecl(methodDecl);
				return;
			}

			var parentImplSection = node.ParentNodeOfType<UnitSectionNode>();
			if (parentImplSection.HeaderKeywordNode.Text.ToLower() != "implementation")
			{
				// TODO: Handle this better.
				throw new Exception("Type decls aren't allowed in the `implementation` section.");
			}
		}
	}
}