namespace JsonParser.Sample {
	partial class Form1 {
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent() {
			this.buttonLoadJson = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// buttonLoadJson
			// 
			this.buttonLoadJson.Location = new System.Drawing.Point(12, 12);
			this.buttonLoadJson.Name = "buttonLoadJson";
			this.buttonLoadJson.Size = new System.Drawing.Size(130, 23);
			this.buttonLoadJson.TabIndex = 3;
			this.buttonLoadJson.Text = "JSONファイルのロード";
			this.buttonLoadJson.UseVisualStyleBackColor = true;
			this.buttonLoadJson.Click += new System.EventHandler(this.buttonLoadJson_Click);
			// 
			// textBox
			// 
			this.textBox.Location = new System.Drawing.Point(12, 41);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(654, 474);
			this.textBox.TabIndex = 4;
			this.textBox.WordWrap = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(679, 527);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.buttonLoadJson);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonLoadJson;
		private System.Windows.Forms.TextBox textBox;
	}
}

