using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON 配列の処理用.
	/// [ が出現する位置から開始.
	/// </summary>
	class ArrayExpression : Expression<JsonContext> {
		/// <summary>
		/// 戻り値として使用する JsonObject.
		/// </summary>
		public JsonObject Value;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public void Interpret(JsonContext context) {
			//Console.WriteLine("ArrayExpression.Interpret()");

			// [ から始まっているかチェック
			Token<TokenType> token = context.Current;
			switch (token.Type) {
			case TokenType.OpenBracket:
				break;
			default:
				throw new InterpreterException("ArrayExpression.Interpret() Error");
			}

			// 値のリストをチェック
			ArrayList arrayList = new ArrayList();
			bool loop = true;
			while (loop) {
				token = context.Next();
				if (token.Type == TokenType.CloseBracket) {
					loop = false;
					break;
				}

				ValueExpression exp = new ValueExpression();
				exp.Interpret(context);
				arrayList.Add(exp.Value);

				token = context.Next();
				switch (token.Type) {
				case TokenType.Comma:
					break;
				case TokenType.CloseBracket:
					loop = false;
					break;
				default:
					throw new InterpreterException("ArrayExpression.Interpret() Error");
				}
			}

			// 戻り値の設定
			Value = new JsonObject();
			Value.Type = JsonType.Array;
			Value.Value = arrayList;
		}
	}

}
