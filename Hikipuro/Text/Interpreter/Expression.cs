namespace Hikipuro.Text.Interpreter {
	/// <summary>
	/// Interpreter パターンの Expression.
	/// Context の値を渡して使用する.
	/// 現在の状態を表す変数等は Context に保存する.
	/// </summary>
	/// <typeparam name="ContextType">Context の型.</typeparam>
	public interface Expression<ContextType> where ContextType : class {
		/// <summary>
		/// 評価用のメソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		void Interpret(ContextType context);
	}
}
