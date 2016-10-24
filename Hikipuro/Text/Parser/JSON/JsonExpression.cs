using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON 全体の処理用.
	/// ファイルの先頭から開始する.
	/// </summary>
	class JsonExpression : Expression<JsonContext> {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public void Interpret(JsonContext context) {
			//Console.WriteLine("JsonExpression.Interpret()");

			Token<TokenType> token = context.Current;
			switch (token.Type) {
			case TokenType.OpenBrace: {
					// { で始まっていたらオブジェクトを作成
					ObjectExpression exp = new ObjectExpression();
					exp.Interpret(context);
					context.JsonObject = exp.Value;
					break;
				}
			case TokenType.OpenBracket: {
					// [ で始まっていたら配列を作成
					ArrayExpression exp = new ArrayExpression();
					exp.Interpret(context);
					context.JsonObject = exp.Value;
					break;
				}
			default:
				throw new InterpreterException("JsonExpression.Interpret() Error");
			}

			// さらに要素が続く場合はエラー
			if (context.Current.IsLast == false) {
				throw new InterpreterException("JsonExpression.Interpret() Error");
			}
		}
	}
}
