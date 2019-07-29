namespace SimFeedback.extension
{
    partial class WebExtControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.WebSvcPortBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.HttpPortBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.WebSvcPortBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.HttpPortBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 265);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SFB Web Control Extension";
            this.groupBox1.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(576, 90);
            this.label3.TabIndex = 4;
            this.label3.Text = "Use <tab> to register changes to Port fields.\r\n\r\nReload browser with the HTTP Por" +
    "t below afterwards.";
            this.label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Web Services Port";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // WebSvcPortBox1
            // 
            this.WebSvcPortBox1.Location = new System.Drawing.Point(164, 190);
            this.WebSvcPortBox1.MaxLength = 7;
            this.WebSvcPortBox1.Name = "WebSvcPortBox1";
            this.WebSvcPortBox1.Size = new System.Drawing.Size(100, 26);
            this.WebSvcPortBox1.TabIndex = 2;
            this.WebSvcPortBox1.Text = "8181";
            this.WebSvcPortBox1.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "HTTP Port";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // HttpPortBox1
            // 
            this.HttpPortBox1.Location = new System.Drawing.Point(164, 142);
            this.HttpPortBox1.MaxLength = 7;
            this.HttpPortBox1.Name = "HttpPortBox1";
            this.HttpPortBox1.Size = new System.Drawing.Size(100, 26);
            this.HttpPortBox1.TabIndex = 0;
            this.HttpPortBox1.Text = "8080";
            this.HttpPortBox1.WordWrap = false;
            this.HttpPortBox1.TextChanged += new System.EventHandler(this.HttpPortBox1_TextChanged);
            // 
            // WebExtControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "WebExtControl";
            this.Size = new System.Drawing.Size(409, 271);
            this.Load += new System.EventHandler(this.OnLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox HttpPortBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox WebSvcPortBox1;
        private System.Windows.Forms.Label label3;
    }
}
