using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON の値の処理用.
	/// フィールドの, コロンの後から開始.
	/// </summary>
	class ValueExpression : Expression<JsonContext> {
		/// <summary>
		/// 戻り値として使用する JsonObject.
		/// </summary>
		public JsonObject Value;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public void Interpret(JsonContext context) {
			//Console.WriteLine("ValueExpression.Interpret()");

			// 型に合わせて JsonObject を作成する
			Token<TokenType> token = context.Next();
			switch (token.Type) {
			case TokenType.String:
				Value = JsonObject.CreateString(token.Text);
				break;
			case TokenType.Number:
				Value = JsonObject.CreateNumber(token.Text);
				break;
			case TokenType.True:
				Value = JsonObject.CreateBool(token.Text);
				break;
			case TokenType.False:
				Value = JsonObject.CreateBool(token.Text);
				break;
			case TokenType.Null:
				Value = JsonObject.CreateNull();
				break;
			case TokenType.OpenBrace: {
					ObjectExpression exp = new ObjectExpression();
					exp.Interpret(context);
					Value = exp.Value;
					break;
				}
			case TokenType.OpenBracket: {
					ArrayExpression exp = new ArrayExpression();
					exp.Interpret(context);
					Value = exp.Value;
					break;
				}
			default:
				throw new InterpreterException("ValueExpression.Interpret() Error");
			}
		}
	}
}
