namespace myscreenshot
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.wB = new System.Windows.Forms.WebBrowser();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(223, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "抓圖";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // wB
            // 
            this.wB.Location = new System.Drawing.Point(5, 54);
            this.wB.MinimumSize = new System.Drawing.Size(20, 20);
            this.wB.Name = "wB";
            this.wB.Size = new System.Drawing.Size(516, 333);
            this.wB.TabIndex = 1;
            this.wB.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.wB_Navigated);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(472, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(49, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Go";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(428, 22);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "https://3wa.tw";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.wB);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "神奇的抓圖機V0.1";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.WebBrowser wB;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
    }
}

