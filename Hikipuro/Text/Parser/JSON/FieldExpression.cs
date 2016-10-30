using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON のフィールドの処理用.
	/// "name": "value" が出現する位置から開始.
	/// </summary>
	class FieldExpression : Expression<JsonContext> {
		/// <summary>
		/// 戻り値として使用する JsonObject.
		/// </summary>
		public JsonObject Value;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public void Interpret(JsonContext context) {
			//Console.WriteLine("FieldExpression.Interpret()");

			// 文字列から始まっているかチェック
			Token<TokenType> token = context.Current;
			string name = string.Empty;
			switch (token.Type) {
			case TokenType.String:
				name = token.Text;
				name = name.Trim('"');
				break;
			default:
				throw new InterpreterException("FieldExpression.Interpret() Error");
			}

			// 直後にコロンが来るかチェック
			token = context.Next();
			switch (token.Type) {
			case TokenType.Colon:
				break;
			default:
				throw new InterpreterException("FieldExpression.Interpret() Error");
			}

			// 値のチェック
			token = context.Next();
			ValueExpression exp = new ValueExpression();
			exp.Interpret(context);

			// 戻り値の設定
			Value = new JsonObject();
			Value.Type = JsonType.Field;
			Value.Name = name;
			Value.Value = exp.Value;
		}
	}
}
