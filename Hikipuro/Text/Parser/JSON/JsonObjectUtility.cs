using System.Collections;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JsonObject で使用するユーティリティ.
	/// </summary>
	class JsonObjectUtility {
		/// <summary>
		/// ドット区切りのパス指定で, JSON 内から値を取得する.
		/// 例: json.data.text -> "test string"
		/// {
		///		"json": {
		///			"data": {
		///				"text": "test string"
		///			}
		///		}
		/// }
		/// </summary>
		/// <param name="jsonObject">値を取り出す JSON オブジェクト.</param>
		/// <param name="path">ドット区切りのパス.</param>
		/// <returns>取り出された値.</returns>
		public static object Select(JsonObject jsonObject, string path) {
			// パスをドットで分割する
			string[] names = path.Split('.');

			// パスの 2 個目以降の内容を再度ドット区切りの文字列にする
			string remain = string.Join(".", names, 1, names.Length - 1);

			// パスが最後に到達した時
			if (names.Length < 1 || path == string.Empty) {
				if (jsonObject.Type == JsonType.Field) {
					JsonObject next = (JsonObject)jsonObject.Value;
					if (next.Type == JsonType.Array) {
						return (ArrayList)next.Value;
					}
					return jsonObject.Value.ToString();
				}
				return jsonObject.ToString();
			}

			// パスが最後に到達していない時
			string name = names[0];
			if (jsonObject.Type == JsonType.String) {
				return jsonObject.Value as string;
			}
			if (jsonObject.Type == JsonType.Field) {
				return Select((JsonObject)jsonObject.Value, path);
			}
			if (jsonObject.Type == JsonType.Object) {
				ArrayList arrayList = (ArrayList)jsonObject.Value;
				foreach (JsonObject item in arrayList) {
					if (item.Name == name) {
						if (item.Type == JsonType.Field) {
							return Select(item, remain);
						} else if (item.Type == JsonType.Object) {
							return Select(item, remain);
						}
					}
				}
			}
			return null;
		}

	}
}
