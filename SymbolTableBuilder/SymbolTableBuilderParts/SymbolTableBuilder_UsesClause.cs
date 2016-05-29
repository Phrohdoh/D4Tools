using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;
using System.Diagnostics;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
	{
		public override void VisitUsesClauseNode(UsesClauseNode node)
		{
			Debug.Assert(CurrentUnitSectionKind != UnitSectionKind.Unknown);

			var itemNodes = node.UnitListNode.Items.Select(din => din.ItemNode);
			var itemNames = itemNodes.Select(un => un.NameNode.ToCode());

			if (CurrentUnitSectionKind == UnitSectionKind.Interface)
			{
				Debug.Assert(CurrentUnitSymbol.HasInterfaceSection);
				CurrentUnitSymbol.InterfaceSectionSymbol.UsesClauseSymbol = new UsesClauseSymbol(itemNames, CurrentUnitSymbol.InterfaceSectionSymbol);
			}
			else if (CurrentUnitSectionKind == UnitSectionKind.Implementation)
			{
				Debug.Assert(CurrentUnitSymbol.HasImplementationSection);
				CurrentUnitSymbol.ImplementationSectionSymbol.UsesClauseSymbol = new UsesClauseSymbol(itemNames, CurrentUnitSymbol.ImplementationSectionSymbol);
			}

			base.VisitUsesClauseNode(node);
		}
	}
}