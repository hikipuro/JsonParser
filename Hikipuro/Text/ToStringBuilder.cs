using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hikipuro.Text {
	/// <summary>
	/// ToString() を簡単に作成するためのクラス.
	/// </summary>
	public class ToStringBuilder {
		public static string Build<T>(T t) {
			StringBuilder text = new StringBuilder();

			// 型名を表示
			Type type = Type.GetType(typeof(T).FullName);
			text.Append("(" + type.Name + ")");
			text.AppendLine(": {");

			// フィールドを巡回する
			foreach (var field in type.GetFields()) {
				// public な変数のみ
				if (field.IsPublic) {
					Type fieldType = field.FieldType;
					object fieldValue = null;

					// 配列の場合
					if (fieldType.IsArray) {
						int index = 0;
						fieldValue = field.GetValue(t);
						if (fieldValue == null || (fieldValue as Array).Length <= 0) {
							text.Append("\t");
							text.Append(field.Name);
							text.Append(": ");
							text.AppendLine("null");
						} else {
							foreach (var value in fieldValue as Array) {
								StringBuilder subArrayText = new StringBuilder();
								subArrayText.Append(field.Name);
								subArrayText.Append("[" + index + "]");
								subArrayText.Append(" (" + value.GetType().Name + "): ");
								subArrayText.Append(value.ToString());
								text.Append(IndentText(subArrayText.ToString()));
								index++;
							}
						}
						continue;
					}

					// プリミティブ型、文字列型、 DateTime 型の場合
					if (fieldType.IsPrimitive
					|| fieldType == typeof(string)
					|| fieldType == typeof(DateTime)) {
						text.Append("\t");
						text.Append(field.Name);
						text.Append(" (" + fieldType.Name + ")");
						text.Append(": ");
						fieldValue = field.GetValue(t);
						if (fieldValue == null) {
							text.AppendLine("null");
						} else {
							text.AppendLine(field.GetValue(t).ToString());
						}
						continue;
					}

					// ユーザ定義型の場合
					StringBuilder subText = new StringBuilder();
					subText.Append(field.Name);
					subText.Append(" ");
					fieldValue = field.GetValue(t);
					if (fieldValue == null) {
						subText.AppendLine("null");
					} else {
						subText.AppendLine(fieldValue.ToString());
					}
					text.Append(IndentText(subText.ToString()));
				}
			}

			text.Append("}");
			return text.ToString();
		}

		// 文字列の各行をインデントする
		private static string IndentText(string text, string indent = "\t") {
			StringBuilder builder = new StringBuilder();
			StringReader reader = new StringReader(text);
			string line;
			while ((line = reader.ReadLine()) != null) {
				builder.AppendLine(indent + line);
			}
			return builder.ToString();
		}
	}
}
