using Hikipuro.Text.Tokenizer;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON ファイルのパーサ.
	/// </summary>
	public class JsonParser {
		/// <summary>
		/// JSON ファイルで使用するトークンの種類.
		/// </summary>
		public enum TokenType {
			None,
			NewLine,
			Comma,
			Colon,
			OpenBrace,
			CloseBrace,
			OpenBracket,
			CloseBracket,
			Null,
			True,
			False,
			Number,
			String,
			Space
		}

		/// <summary>
		/// JSON ファイルをパース.
		/// </summary>
		/// <param name="text">JSON ファイル.</param>
		/// <returns>JSON オブジェクト.</returns>
		public static JsonObject Parse(string text) {
			// トークンに分解する
			TokenList<TokenType> tokens = Tokenize(text);

			// パース
			JsonContext context = new JsonContext(tokens.GetEnumerator());
			JsonExpression exp = new JsonExpression();
			exp.Interpret(context);
			JsonObject json = context.JsonObject;

			return json;
		}

		/// <summary>
		/// 渡された JSON ファイルを分解する.
		/// </summary>
		/// <param name="text">JSON ファイル.</param>
		/// <returns>トークンのリスト.</returns>
		public static TokenList<TokenType> Tokenize(string text) {
			// Tokenizer オブジェクトを準備する
			Tokenizer<TokenType> tokenizer = new Tokenizer<TokenType>();

			// トークンの分解規則を追加する
			tokenizer.AddPattern(TokenType.NewLine, "\\G\r\n|\r|\n");
			tokenizer.AddPattern(TokenType.Comma, "\\G,");
			tokenizer.AddPattern(TokenType.Colon, "\\G:");
			tokenizer.AddPattern(TokenType.OpenBrace, "\\G{");
			tokenizer.AddPattern(TokenType.CloseBrace, "\\G}");
			tokenizer.AddPattern(TokenType.OpenBracket, @"\G\[");
			tokenizer.AddPattern(TokenType.CloseBracket, @"\G\]");
			tokenizer.AddPattern(TokenType.Null, "\\Gnull");
			tokenizer.AddPattern(TokenType.True, "\\Gtrue");
			tokenizer.AddPattern(TokenType.False, "\\Gfalse");
			tokenizer.AddPattern(TokenType.Number, @"\G\d+[.]?\d*");
			tokenizer.AddPattern(TokenType.String, @"\G""((?<=\\)""|[^\r\n""])*""");
			tokenizer.AddPattern(TokenType.Space, @"\G\s+");

			// リストにトークンを追加する直前に発生するイベント
			// - e.Cancel = true; で追加しない
			tokenizer.BeforeAddToken += (object sender, BeforeAddTokenEventArgs<TokenType> e) => {
				if (e.TokenMatch.Type == TokenType.NewLine) {
					e.Cancel = true;
					return;
				}
				if (e.TokenMatch.Type == TokenType.Space) {
					e.Cancel = true;
					return;
				}
			};

			// トークンに分解する
			TokenList<TokenType> tokens = tokenizer.Tokenize(text);
			return tokens;
		}
	}
}
