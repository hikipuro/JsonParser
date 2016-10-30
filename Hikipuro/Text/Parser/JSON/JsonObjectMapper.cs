using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Hikipuro.Text.Parser.JSON {
	/// <summary>
	/// JsonObject を他の型にマッピングするためのクラス.
	/// </summary>
	public class JsonObjectMapper {
		/// <summary>
		/// JsonObject から値を取得するためのメソッド.
		/// </summary>
		/// <param name="jsonObject"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		delegate object SelectValueMethod(JsonObject jsonObject, string path);

		/// <summary>
		/// JsonObject から値を取得するためのメソッドのリスト.
		/// </summary>
		Dictionary<Type, SelectValueMethod> selectValueMethods;

		/// <summary>
		/// Map() メソッドへの参照.
		/// </summary>
		MethodInfo mapMethodInfo;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public JsonObjectMapper() {
			// Map メソッドを取得しておく
			Type type = typeof(JsonObjectMapper);
			mapMethodInfo = type.GetMethod(
				"Map",
				BindingFlags.NonPublic | BindingFlags.Instance
			);

			// メソッドのリストを準備しておく
			selectValueMethods = new Dictionary<Type, SelectValueMethod>();
			selectValueMethods.Add(typeof(bool), SelectBool);
			selectValueMethods.Add(typeof(byte), SelectByte);
			selectValueMethods.Add(typeof(short), SelectShort);
			selectValueMethods.Add(typeof(int), SelectInt);
			selectValueMethods.Add(typeof(long), SelectLong);
			selectValueMethods.Add(typeof(float), SelectFloat);
			selectValueMethods.Add(typeof(double), SelectDouble);
			selectValueMethods.Add(typeof(decimal), SelectDecimal);
			selectValueMethods.Add(typeof(string), SelectString);
			selectValueMethods.Add(typeof(string[]), SelectStringList);
			selectValueMethods.Add(typeof(List<string>), SelectStringGenericList);
			selectValueMethods.Add(typeof(DateTime), SelectDateTime);
		}

		/// <summary>
		/// JsonObject を他の型にマッピングする.
		/// </summary>
		/// <typeparam name="T">マッピング対象の型.</typeparam>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <returns>マッピングされたオブジェクト.</returns>
		public T Map<T>(JsonObject jsonObject) where T : new() {
			if (jsonObject == null) {
				return new T();
			}
			return Map<T>(jsonObject, null);
		}

		/// <summary>
		/// JsonObject を他の型にマッピングする.
		/// このメソッドは内部的に使用する.
		/// </summary>
		/// <typeparam name="T">マッピング対象の型.</typeparam>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON 内での値へのパス.</param>
		/// <returns>マッピングされたオブジェクト.</returns>
		private T Map<T>(JsonObject jsonObject, string path = null) where T : new() {
			T data = new T();
			Type type = data.GetType();
			FieldInfo[] fieldInfoList = type.GetFields(
				BindingFlags.Instance | BindingFlags.Public
			);

			// フィールドを巡回する
			foreach (var fieldInfo in fieldInfoList) {
				string nextPath = CreatePath(path, fieldInfo.Name);
				Console.WriteLine("nextPath: " + nextPath);
				SetValue(data, jsonObject, nextPath, fieldInfo);
			}
			return data;
		}

		/// <summary>
		/// T 型の値の中に JSON オブジェクトの値をマッピングする.
		/// </summary>
		/// <typeparam name="T">マッピング対象の型.</typeparam>
		/// <param name="data">マッピング対象のオブジェクト.</param>
		/// <param name="jsonObject">値を引き出す JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <param name="fieldInfo">data オブジェクトのフィールド.</param>
		private void SetValue<T>(T data, JsonObject jsonObject, string path, FieldInfo fieldInfo) {
			// selectValueMethods の中にメソッドが定義されている場合
			Type fieldType = fieldInfo.FieldType;
			Console.WriteLine("fieldType: " + fieldType.Name);
			if (selectValueMethods.ContainsKey(fieldType)) {
				SelectValueMethod method = selectValueMethods[fieldType];
				object value = method(jsonObject, path);
				fieldInfo.SetValue(data, value);
				return;
			}

			// それ以外のクラス
			if (fieldType.IsClass) {
				MethodInfo methodInfo = CreateGenericMethod(fieldType);
				object value = null;
				try {
					value = methodInfo.Invoke(this, new object[] { jsonObject, path });
				} catch (TargetInvocationException e) {

				}
				fieldInfo.SetValue(data, value);
			}
		}

		/// <summary>
		/// Map() で使用する, JSON オブジェクト内のパスを作成する.
		/// </summary>
		/// <param name="basePath">ベースパス.</param>
		/// <param name="trailingPath">ベースパスに続くパス.</param>
		/// <returns>作成されたパス.</returns>
		private string CreatePath(string basePath, string trailingPath) {
			StringBuilder path = new StringBuilder();
			if (basePath == null) {
				path.Append(trailingPath);
			} else {
				path.Append(basePath);
				path.Append(".");
				path.Append(trailingPath);
			}
			return path.ToString();
		}

		/// <summary>
		/// Map() メソッドの, 特定の型用のメソッドを作成する.
		/// プログラム内で T 型を指定する方法がないため, このメソッドを使用する.
		/// </summary>
		/// <param name="fieldType">Map() の T に指定したい型.</param>
		/// <returns>特定の型用の Map() メソッド.</returns>
		private MethodInfo CreateGenericMethod(Type fieldType) {
			return mapMethodInfo.MakeGenericMethod(fieldType);
		}

		/// <summary>
		/// Bool の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectBool(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			bool value = false;
			bool.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Byte の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectByte(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			byte value = 0;
			byte.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Short の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectShort(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			short value = 0;
			short.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Int の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectInt(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			int value = 0;
			int.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Long の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectLong(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			long value = 0;
			long.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Float の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectFloat(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			float value = 0;
			float.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Double の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectDouble(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			double value = 0;
			double.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// Decimal の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectDecimal(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			decimal value = 0;
			decimal.TryParse(text, out value);
			return value;
		}

		/// <summary>
		/// 文字列の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectString(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			return text;
		}

		/// <summary>
		/// 文字列リストの値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectStringList(JsonObject jsonObject, string path) {
			List<string> text = new List<string>();
			ArrayList array = jsonObject.Select(path) as ArrayList;
			if (array == null) {
				return text.ToArray();
			}
			foreach (object obj in array) {
				text.Add(obj.ToString());
			}
			return text.ToArray();
		}

		/// <summary>
		/// 文字列リスト (ジェネリック) の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectStringGenericList(JsonObject jsonObject, string path) {
			List<string> text = new List<string>();
			ArrayList array = jsonObject.Select(path) as ArrayList;
			if (array == null) {
				return text.ToArray();
			}
			foreach (object obj in array) {
				text.Add(obj.ToString());
			}
			return text;
		}

		/// <summary>
		/// DateTime 型の値を JSON オブジェクトから取得する.
		/// </summary>
		/// <param name="jsonObject">JSON オブジェクト.</param>
		/// <param name="path">JSON オブジェクト内のパス.</param>
		/// <returns>JSON オブジェクトから取得された値.</returns>
		private object SelectDateTime(JsonObject jsonObject, string path) {
			string text = (string)jsonObject.Select(path);
			DateTime value = DateTime.MinValue;
			DateTime.TryParse(text, out value);
			return value;
		}
	}
}
