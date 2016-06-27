using System;
using System.Linq;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace D4Tools.SymbolTableBuilder
{
	public partial class SymbolTableBuilder : Visitor
	{
		public override void VisitFieldSectionNode(FieldSectionNode node)
		{
			foreach (var fieldListNode in node.FieldListNode.Items)
			{
				var identifiers = fieldListNode.NameListNode.Items.Select(din => din.ItemNode.Text).ToArray();
				var fieldDeclKind = FieldKind.Unknown;

				if (DecideFieldDeclKind(fieldListNode.TypeNode, out fieldDeclKind))
				{
					if (!BuildSymbolsForField(fieldListNode.TypeNode, fieldDeclKind))
						Console.WriteLine($"Failed to {nameof(BuildSymbolsForField)} for {fieldDeclKind} at {fieldListNode.Location}");
				}
				else
					Console.WriteLine($"{nameof(fieldListNode)} type '{fieldListNode.TypeNode.ToCode()}' unhandled at {fieldListNode.Location}");

				foreach (var ident in identifiers)
				{
					//Console.WriteLine($"{fieldDeclKind} {ident}");
				}
			}

			base.VisitFieldSectionNode(node);
		}

		bool BuildSymbolsForField(AstNode typeNode, FieldKind declKind)
		{
			switch (declKind)
			{
				case FieldKind.ArrayOf:
					var arrNode = typeNode as ArrayTypeNode;
					if (arrNode == null)
						return false;

					foreach (var itemNode in arrNode.IndexListNode.Items.Select(dim => dim.ItemNode))
					{
						var binOpNode = itemNode as BinaryOperationNode;
						if (binOpNode == null)
							throw new Exception($"This shouldn't happen..{Environment.NewLine}null {nameof(BinaryOperationNode)} near {itemNode.Location}");

						if (binOpNode.OperatorNode.Type == TokenType.DotDot)
						{
							var start = binOpNode.LeftNode;
							var end = binOpNode.RightNode;
							// TODO... Setup ArrayOfSymbol here.
						}
					}

					return true;
			}
			return true;
		}

		bool DecideFieldDeclKind(AstNode typeNode, out FieldKind fieldKind)
		{
			var typeNodeToken = typeNode as Token;
			if (typeNodeToken != null)
			{
				switch (typeNodeToken.Type)
				{
					case TokenType.Identifier:
					case TokenType.StringKeyword: // Why is 'string' a keyword? (Why isn't 'cardinal'?)
						fieldKind = FieldKind.SimpleType;
						return true;
				}
			}

			var arrayTypeNode = typeNode as ArrayTypeNode;
			if (arrayTypeNode != null)
			{
				fieldKind = FieldKind.ArrayOf;
				return true;
			}

			var fileTypeNode = typeNode as FileTypeNode;
			if (fileTypeNode != null)
			{
				fieldKind = FieldKind.FileOf;
				return true;
			}

			var pointerTypeNode = typeNode as PointerTypeNode;
			if (pointerTypeNode != null)
			{
				fieldKind = FieldKind.Pointer;
				return true;
			}

			fieldKind = FieldKind.Unknown;
			return false;
		}
	}
}
