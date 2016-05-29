using System;
using System.Collections.Generic;
using System.Diagnostics;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder
	{
		LocalSymbol[] ResolveVariables(VarSectionNode node)
		{
			var idents = new List<string>();
			var ret = new List<LocalSymbol>();

			var lastParamType = "";

			foreach (var varDecl in node.VarListNode.Items)
			{
				lastParamType = varDecl.TypeNode.ToCode();

				foreach (var delimItem in varDecl?.NameListNode?.Items)
				{
					var item = delimItem.ItemNode;
					if (item.Type == TokenType.Identifier)
						idents.Add(item.ToCode());
					else
						Console.WriteLine("Unresolved item type: {0}", item.Type);
				}

				foreach (var ident in idents)
					ret.Add(new LocalSymbol(ident) { Type = lastParamType });

				idents.Clear();
				lastParamType = "";
			}

			return ret.ToArray();
		}

		void CustomVisitVarSectionNodeInInterfaceSection(VarSectionNode node, UnitSectionNode interfaceNode)
		{
			Debug.Assert(interfaceNode != null);
			Debug.Assert(CurrentUnitSymbol != null);
			Debug.Assert(CurrentUnitSymbol.HasInterfaceSection);

			// TODO: Stop trashing previously-declared variables.
			CurrentUnitSymbol.InterfaceSectionSymbol.LocalSymbols = ResolveVariables(node);
		}

		public override void VisitVarSectionNode(VarSectionNode node)
		{
			var parentMethodImplNode = node.ParentNodeOfType<MethodImplementationNode>();
			if (parentMethodImplNode == null)
				CustomVisitVarSectionNodeInInterfaceSection(node, node.ParentNodeOfType<UnitSectionNode>());
			else
			{
				var methodName = parentMethodImplNode.MethodHeadingNode.NameNode.ToCode();
				var variables = ResolveVariables(node);

				MethodImplementationSymbol methodImplSymbol;
				if (CurrentUnitSymbol.MethodImplementationsByName.TryGetValue(methodName, out methodImplSymbol))
					methodImplSymbol.Locals = variables;
				else // TODO: This should actually be an error, how did we get to the var section without visiting the method heading?
				{
					methodImplSymbol = new MethodImplementationSymbol(methodName) { Locals = variables };
					CurrentUnitSymbol.MethodImplementationsByName.Add(methodName, methodImplSymbol);
				}
			}

			base.VisitVarSectionNode(node);
		}
	}
}