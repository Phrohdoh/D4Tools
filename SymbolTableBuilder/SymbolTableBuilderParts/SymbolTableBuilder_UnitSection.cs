using System;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
	{
		public UnitSectionKind CurrentUnitSectionKind { get; internal set; } = UnitSectionKind.Unknown;

		public override void VisitUnitSectionNode(UnitSectionNode node)
		{
			var name = node.HeaderKeywordNode.Text;

			if (string.Equals(name, "interface", StringComparison.InvariantCultureIgnoreCase))
				OnVisitUnitInterface(node);
			else if (string.Equals(name, "implementation", StringComparison.InvariantCultureIgnoreCase))
				OnVisitUnitImplementation(node);

			base.VisitUnitSectionNode(node);
		}

		void OnVisitUnitInterface(UnitSectionNode interfaceNode)
		{
			// NOTE:
			//
			// Here we are throwing away any existing interface section.
			// Instead we should add declarations to the existing symbol,
			//   or track multiple symbols (keyed by definition location?)
			CurrentUnitSymbol.InterfaceSectionSymbol = new UnitInterfaceSectionSymbol();
			CurrentUnitSectionKind = UnitSectionKind.Interface;
		}

		void OnVisitUnitImplementation(UnitSectionNode implementationNode)
		{
			// NOTE:
			//
			// Here we are throwing away any existing implementation section.
			// Instead we should add declarations to the existing symbol,
			//   or track multiple symbols (keyed by definition location?)
			CurrentUnitSymbol.ImplementationSectionSymbol = new UnitImplementationSectionSymbol();
			CurrentUnitSectionKind = UnitSectionKind.Implementation;
		}
	}
}