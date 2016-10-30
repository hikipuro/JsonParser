using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JSON オブジェクト.
	/// JsonParser の戻り値として使用する.
	/// </summary>
	public class JsonObject {
		/// <summary>
		/// オブジェクトの型.
		/// </summary>
		public JsonType Type;

		/// <summary>
		/// オブジェクトの名前.
		/// </summary>
		public string Name;

		/// <summary>
		/// オブジェクトの値.
		/// </summary>
		public object Value;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public JsonObject() {
		}

		/// <summary>
		/// 文字列型の JsonObject を作成する.
		/// </summary>
		/// <param name="value">文字列.</param>
		/// <returns>JsonObject.</returns>
		public static JsonObject CreateString(string value) {
			if (value != null) {
				value = value.Trim('"');
			}
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.String;
			jsonObject.Value = value;
			return jsonObject;
		}

		/// <summary>
		/// 数値型の JsonObject を作成する.
		/// 内部で double 型として扱われる.
		/// </summary>
		/// <param name="value">数値を表す文字列.</param>
		/// <returns>JsonObject.</returns>
		public static JsonObject CreateNumber(string value) {
			double doubleValue = 0;
			double.TryParse(value, out doubleValue);
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.Number;
			jsonObject.Value = doubleValue;
			return jsonObject;
		}

		/// <summary>
		/// Bool 型の JsonObject を作成する.
		/// </summary>
		/// <param name="value">文字列で true または false.</param>
		/// <returns>JsonObject.</returns>
		public static JsonObject CreateBool(string value) {
			bool boolValue = false;
			bool.TryParse(value, out boolValue);
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.Bool;
			jsonObject.Value = boolValue;
			return jsonObject;
		}

		/// <summary>
		/// null 型の JsonObject を作成する.
		/// </summary>
		/// <returns>JsonObject.</returns>
		public static JsonObject CreateNull() {
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.Null;
			jsonObject.Value = null;
			return jsonObject;
		}

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
		/// <param name="path">ドット区切りのパス.</param>
		/// <returns>取り出された値.</returns>
		public object Select(string path) {
			return JsonObjectUtility.Select(this, path);
		}

		/// <summary>
		/// JSON オブジェクトの内容を, 他の型のオブジェクトにマッピングする.
		/// </summary>
		/// <typeparam name="T">マッピング対象の型.</typeparam>
		/// <returns>マッピングされたオブジェクト.</returns>
		public T Map<T>() where T: new() {
			JsonObjectMapper mapper = new JsonObjectMapper();
			return mapper.Map<T>(this);
		}

		/// <summary>
		/// 文字列表現に変換する.
		/// </summary>
		/// <returns>オブジェクトの文字列表現.</returns>
		public override string ToString() {
			return ToString(0);
		}

		/// <summary>
		/// 文字列表現に変換する (内部的に使用する).
		/// </summary>
		/// <param name="depth">オブジェクトの階層.</param>
		/// <returns>オブジェクトの文字列表現.</returns>
		public string ToString(int depth = 0) {
			StringBuilder stringBuilder = new StringBuilder();

			switch (Type) {
			case JsonType.Null:
				stringBuilder.Append("null");
				break;
			case JsonType.Bool:
				stringBuilder.Append(Value.ToString().ToLower());
				break;
			case JsonType.Number:
				stringBuilder.Append(Value.ToString());
				break;
			case JsonType.String:
				stringBuilder.Append(Value.ToString());
				break;
			case JsonType.Field:
				if (Name != null && Name != string.Empty) {
					stringBuilder.AppendFormat("\"{0}\": ", Name);
				}
				stringBuilder.Append(((JsonObject)Value).ToString(depth));
				break;
			case JsonType.Object: {
				ArrayList arrayList = (ArrayList)Value;
				stringBuilder.AppendLine("{");
				foreach (JsonObject item in arrayList) {
					stringBuilder.Append(new string('\t', depth + 1));
					stringBuilder.Append(item.ToString(depth + 1));
					stringBuilder.AppendLine(",");
				}
				int commaIndex = Environment.NewLine.Length + 1;
				stringBuilder.Remove(stringBuilder.Length - commaIndex, 1);
				stringBuilder.Append(new string('\t', depth));
				stringBuilder.Append("}");
				break;
			}
			case JsonType.Array: {
				ArrayList arrayList = (ArrayList)Value;
				stringBuilder.AppendLine("[");
				foreach (JsonObject item in arrayList) {
					stringBuilder.Append(new string('\t', depth + 1));
					stringBuilder.Append(item.ToString(depth + 1));
					stringBuilder.AppendLine(",");
				}
				int commaIndex = Environment.NewLine.Length + 1;
				stringBuilder.Remove(stringBuilder.Length - commaIndex, 1);
				stringBuilder.Append(new string('\t', depth));
				stringBuilder.Append("]");
				break;
			}
			default:
				break;
			}

			return stringBuilder.ToString();
		}
	}
}
