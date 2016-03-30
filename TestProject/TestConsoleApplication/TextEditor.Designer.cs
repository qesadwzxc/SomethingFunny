namespace TestConsoleApplication
{
    partial class TextEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtAutoComplete = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtAutoComplete
            // 
            this.txtAutoComplete.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtAutoComplete.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtAutoComplete.Font = new System.Drawing.Font("宋体", 11F);
            this.txtAutoComplete.Location = new System.Drawing.Point(1, -1);
            this.txtAutoComplete.MaxLength = 327670;
            this.txtAutoComplete.Multiline = true;
            this.txtAutoComplete.Name = "txtAutoComplete";
            this.txtAutoComplete.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAutoComplete.Size = new System.Drawing.Size(593, 521);
            this.txtAutoComplete.TabIndex = 0;
            this.txtAutoComplete.TextChanged += new System.EventHandler(this.txtAutoComplete_TextChanged);
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 599);
            this.Controls.Add(this.txtAutoComplete);
            this.Name = "TextEditor";
            this.Text = "TextEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAutoComplete;
    }
}