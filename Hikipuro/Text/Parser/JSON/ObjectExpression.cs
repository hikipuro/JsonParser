using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON オブジェクトの処理用.
	/// { が出現する位置から開始.
	/// </summary>
	class ObjectExpression : Expression<JsonContext> {
		/// <summary>
		/// 戻り値として使用する JsonObject.
		/// </summary>
		public JsonObject Value;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public void Interpret(JsonContext context) {
			//Console.WriteLine("ObjectExpression.Interpret()");

			// { から始まっているかチェック
			Token<TokenType> token = context.Current;
			switch (token.Type) {
			case TokenType.OpenBrace:
				break;
			default:
				throw new InterpreterException("ObjectExpression.Interpret() Error");
			}

			// 値のリストをチェック
			ArrayList arrayList = new ArrayList();
			bool loop = true;
			while (loop) {
				FieldExpression exp = new FieldExpression();
				exp.Interpret(context);
				arrayList.Add(exp.Value);

				token = context.Next();
				switch (token.Type) {
				case TokenType.Comma:
					break;
				case TokenType.CloseBrace:
					loop = false;
					break;
				default:
					throw new InterpreterException("ObjectExpression.Interpret() Error");
				}
			}

			// 戻り値の設定
			Value = new JsonObject();
			Value.Type = JsonType.Object;
			Value.Value = arrayList;
		}
	}

}
