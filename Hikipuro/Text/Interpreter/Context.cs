using System.Collections;

namespace Hikipuro.Text.Interpreter {
	/// <summary>
	/// Interpreter パターンの Context.
	/// Interpreter の現在の状態を保存しておく.
	/// 要素の参照位置を動かす以外の機能が欲しい場合は, 継承してから使用する.
	/// </summary>
	/// <typeparam name="ValueType">連続した要素の型.</typeparam>
	public class Context<ValueType> where ValueType : class {
		/// <summary>
		/// 値の列挙用.
		/// </summary>
		private IEnumerator enumerator;

		/// <summary>
		/// 現在の値.
		/// </summary>
		private ValueType current;

		/// <summary>
		/// 現在の値を取得する.
		/// </summary>
		public ValueType Current {
			get { return current; }
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		/// <param name="source">IEnumerator 型の列挙可能オブジェクト.</param>
		public Context(IEnumerator source) {
			enumerator = source;
			enumerator.Reset();
			Next();
		}

		/// <summary>
		/// 要素の参照位置を 1 つ次に動かす.
		/// </summary>
		/// <returns>次の要素.</returns>
		public ValueType Next() {
			if (enumerator.MoveNext()) {
				current = (ValueType)enumerator.Current;
			} else {
				current = null;
			}
			return current;
		}
	}
}
