using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
	{
		public override void VisitMethodImplementationNode(MethodImplementationNode node)
		{
			CustomVisitMethodHeadingNode(node.MethodHeadingNode, MethodDeclOrImpl.Implementation);

			// We don't want our base to visit the MethodHeadingNode so we must visit these nodes manually.
			Visit(node.FancyBlockNode);
			Visit(node.SemicolonNode);
		}
	}
}