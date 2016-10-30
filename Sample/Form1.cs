using Hikipuro.Text.Parser.JSON;
using JsonParser.Sample.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace JsonParser.Sample {
	using System.Collections;
	using JsonParser = Hikipuro.Text.Parser.JSON.JsonParser;

	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		/// <summary>
		/// JSON ファイルのロードボタンが押された時.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonLoadJson_Click(object sender, EventArgs e) {
			// 
			// Sample/JSON/Test1.json を読み込む
			// 
			// - このファイルは次のサイトからお借りしました
			//   http://json.org/example.html
			// 
			StreamReader reader = new StreamReader(
				"Sample/JSON/Test4.json",
				Encoding.UTF8
			);
			string text = reader.ReadToEnd();
			reader.Close();

			// JSON ファイルをパース
			JsonObject json = JsonParser.Parse(text);

			// 画面に表示する
			//textBox.Text = json.ToString();

			GlossaryData glossary = json.Map<GlossaryData>();
			textBox.Text = glossary.ToString();
		}
	}
}
