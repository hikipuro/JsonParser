using Hikipuro.Text.Tokenizer;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON ファイルのパーサ.
	/// デザインパターンの Interpreter パターンを使用してパースする.
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

			// Interpreter パターンでパース
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
			tokenizer.AddPattern(TokenType.NewLine, "\r\n|\r|\n");
			tokenizer.AddPattern(TokenType.Comma, ",");
			tokenizer.AddPattern(TokenType.Colon, ":");
			tokenizer.AddPattern(TokenType.OpenBrace, "{");
			tokenizer.AddPattern(TokenType.CloseBrace, "}");
			tokenizer.AddPattern(TokenType.OpenBracket, @"\[");
			tokenizer.AddPattern(TokenType.CloseBracket, @"\]");
			tokenizer.AddPattern(TokenType.Null, "null");
			tokenizer.AddPattern(TokenType.True, "true");
			tokenizer.AddPattern(TokenType.False, "false");
			tokenizer.AddPattern(TokenType.Number, @"\d+[.]?\d*");
			tokenizer.AddPattern(TokenType.String, @"""((?<=\\)""|[^\r\n""])*""");
			tokenizer.AddPattern(TokenType.Space, @"\s+");

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
