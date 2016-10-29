using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Hikipuro.Text.Parser.JSON;

namespace JsonParser.Sample {
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
				"Sample/JSON/Test1.json",
				Encoding.UTF8
			);
			string text = reader.ReadToEnd();
			reader.Close();

			// JSON ファイルをパース (名前空間がかぶってしまったので, フルパスで..)
			JsonObject json = Hikipuro.Text.Parser.JSON.JsonParser.Parse(text);

			// 画面に表示する
			textBox.Text = json.ToString();
		}
	}
}
