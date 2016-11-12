using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
	{
		public UnitSymbol CurrentUnitSymbol;

		public override void VisitUnitNode(UnitNode node)
		{
			UnitSymbol unitSymbol;
			var name = node.UnitNameNode?.Text;
			if (string.IsNullOrWhiteSpace(name))
				return;

			if (!UnitSymbols.TryGetValue(name, out unitSymbol))
			{
				unitSymbol = new UnitSymbol(name);
				UnitSymbols.Add(name, unitSymbol);
			}

			CurrentUnitSymbol = unitSymbol;
			base.VisitUnitNode(node);
		}
	}
}