using System;

namespace Hikipuro.Text.Interpreter {
	/// <summary>
	/// インタープリタのエラー発生時に使用する.
	/// 主に Expression.Interpret() 内で使用する.
	/// </summary>
	public class InterpreterException : ApplicationException {
		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="message">メッセージ.</param>
		public InterpreterException(string message) : base(message) {
		}
	}
}
