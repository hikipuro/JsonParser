﻿using System.Text.RegularExpressions;

namespace Hikipuro.Text.Tokenizer {
	/// <summary>
	/// トークンのマッチした場所.
	/// </summary>
	/// <typeparam name="TokenType">トークンの種類.</typeparam>
	public class TokenMatch<TokenType> where TokenType : struct {
		/// <summary>
		/// トークンの種類.
		/// </summary>
		public TokenType Type;

		/// <summary>
		/// マッチした文字列.
		/// BeforeAddToken イベントの中で修正できる.
		/// </summary>
		public string Text;

		/// <summary>
		/// マッチした文字列の位置.
		/// </summary>
		public int Index;

		/// <summary>
		/// マッチした文字列の行番号.
		/// </summary>
		public int LineNumber;

		/// <summary>
		/// 行の文字位置.
		/// </summary>
		public int LineIndex;

		/// <summary>
		/// マッチした時に使用した, 正規表現の Match オブジェクト.
		/// </summary>
		public Match Match;

		/// <summary>
		/// 加工前のマッチした文字列.
		/// </summary>
		string rawText;

		/// <summary>
		/// 加工前のマッチした文字列 (読み取り専用).
		/// </summary>
		public string RawText {
			get { return rawText; }
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public TokenMatch() {
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="text">マッチした文字列.</param>
		public TokenMatch(string text) {
			rawText = text;
			this.Text = text;
		}
	}
}
