using System;
using DGrok.DelphiNodes;
using DGrok.Framework;

namespace SyntaxFactoryDelphi
{
	public static class SyntaxFactory
	{
		public static IfStatementNode IfStatement(AstNode condition, AstNode thenBody, AstNode elseBody)
		{
			var tokIf = TokenFactory.IfKeyword();
			var tokThen = TokenFactory.ThenKeyword();

			#if DEBUG
			if (condition == null)
				throw new ArgumentNullException($"{nameof(condition)} must not be null.");

			if (thenBody == null)
				throw new ArgumentNullException($"{nameof(thenBody)} must not be null.");
			#endif

			Token tokElse = null;
			if (elseBody != null)
				tokElse = TokenFactory.ElseKeyword();

			return new IfStatementNode(
				ifKeywordNode: tokIf,
				conditionNode: condition,
				thenKeywordNode: tokThen,
				thenStatementNode: thenBody,
				elseKeywordNode: tokElse,
				elseStatementNode: elseBody);
		}
	}

	public static class TokenFactory
	{
		public static Token IfKeyword()
		{
			return new Token(TokenType.IfKeyword, null, "if", null);
		}

		public static Token NotKeyword()
		{
			return new Token(TokenType.NotKeyword, null, "not", null);
		}

		public static Token ThenKeyword()
		{
			return new Token(TokenType.ThenKeyword, null, "then", null);
		}

		public static Token ElseKeyword()
		{
			return new Token(TokenType.ElseKeyword, null, "else", null);
		}

		public static Token ColonEquals()
		{
			return new Token(TokenType.ColonEquals, null, ":=", null);
		}

		public static Token Number(string text)
		{
			return new Token(TokenType.Number, null, text, null);
		}
	}
}