using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Collections;
using TokenType = Hikipuro.Text.Parser.JSON.JsonParser.TokenType;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// Interpreter パターンの JSON 用コンテキスト.
	/// </summary>
	class JsonContext : Context<Token<TokenType>> {
		/// <summary>
		/// 戻り値.
		/// </summary>
		public JsonObject JsonObject;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source"></param>
		public JsonContext(IEnumerator source) : base(source) {
		}
	}
}
