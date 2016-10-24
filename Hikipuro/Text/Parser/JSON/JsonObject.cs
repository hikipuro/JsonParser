using System;
using System.Collections;
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
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.Number;
			jsonObject.Value = double.Parse(value);
			return jsonObject;
		}

		/// <summary>
		/// Bool 型の JsonObject を作成する.
		/// </summary>
		/// <param name="value">文字列で true または false.</param>
		/// <returns>JsonObject.</returns>
		public static JsonObject CreateBool(string value) {
			JsonObject jsonObject = new JsonObject();
			jsonObject.Type = JsonType.Bool;
			jsonObject.Value = bool.Parse(value);
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
		/// 文字列表現に変換する.
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return ToString(0);
		}

		/// <summary>
		/// 文字列表現に変換する.
		/// </summary>
		/// <param name="depth"></param>
		/// <returns></returns>
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
