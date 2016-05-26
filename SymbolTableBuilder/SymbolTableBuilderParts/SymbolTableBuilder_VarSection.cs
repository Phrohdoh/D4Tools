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
		void OnVisitVarSectionForUnit(VarSectionNode node)
		{
			var parentUnitNode = node.ParentNodeOfType<UnitNode>();
			Debug.Assert(parentUnitNode != null);
		}

		public override void VisitVarSectionNode(VarSectionNode node)
		{
			var parentMethodImplNode = node.ParentNodeOfType<MethodImplementationNode>();
			if (parentMethodImplNode == null)
			{
				OnVisitVarSectionForUnit(node);
				return;
			}

			var methodName = parentMethodImplNode.MethodHeadingNode.NameNode.ToCode();

			var idents = new List<string>();
			var localSymbols = new List<LocalSymbol>();

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
					localSymbols.Add(new LocalSymbol(ident)
					{
						Type = lastParamType,
					});

				idents.Clear();
				lastParamType = "";
			}

			var localArr = localSymbols.ToArray();

			MethodImplementationSymbol methodImplSymbol;
			if (CurrentUnitSymbol.MethodImplementationsByName.TryGetValue(methodName, out methodImplSymbol))
				methodImplSymbol.Locals = localArr;
			else // TODO: This should actually be an error, how did we get to the var section without visiting the method heading?
			{
				methodImplSymbol = new MethodImplementationSymbol(methodName) { Locals = localArr };
				CurrentUnitSymbol.MethodImplementationsByName.Add(methodName, methodImplSymbol);
			}
		}
	}
}