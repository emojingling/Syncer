namespace chenz
{
    partial class FrmFileSet
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFileSet = new System.Windows.Forms.Label();
            this.tbFileSetName = new System.Windows.Forms.TextBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnInteliGetPaths = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Font = new System.Drawing.Font("宋体", 12F);
            this.btnOk.Location = new System.Drawing.Point(127, 53);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(109, 32);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCancel.Location = new System.Drawing.Point(363, 53);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(109, 32);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblFileSet
            // 
            this.lblFileSet.AutoSize = true;
            this.lblFileSet.Font = new System.Drawing.Font("宋体", 12F);
            this.lblFileSet.Location = new System.Drawing.Point(12, 9);
            this.lblFileSet.Name = "lblFileSet";
            this.lblFileSet.Size = new System.Drawing.Size(88, 16);
            this.lblFileSet.TabIndex = 2;
            this.lblFileSet.Text = "文件集名称";
            // 
            // tbFileSetName
            // 
            this.tbFileSetName.Font = new System.Drawing.Font("宋体", 12F);
            this.tbFileSetName.Location = new System.Drawing.Point(106, 6);
            this.tbFileSetName.Name = "tbFileSetName";
            this.tbFileSetName.Size = new System.Drawing.Size(230, 26);
            this.tbFileSetName.TabIndex = 0;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Font = new System.Drawing.Font("宋体", 12F);
            this.btnRemove.Location = new System.Drawing.Point(479, 1);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(119, 32);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "清除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnInteliGetPaths
            // 
            this.btnInteliGetPaths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInteliGetPaths.Font = new System.Drawing.Font("宋体", 12F);
            this.btnInteliGetPaths.Location = new System.Drawing.Point(354, 1);
            this.btnInteliGetPaths.Name = "btnInteliGetPaths";
            this.btnInteliGetPaths.Size = new System.Drawing.Size(119, 32);
            this.btnInteliGetPaths.TabIndex = 3;
            this.btnInteliGetPaths.Text = "智能获取路径";
            this.btnInteliGetPaths.UseVisualStyleBackColor = true;
            this.btnInteliGetPaths.Click += new System.EventHandler(this.btnInteliGetPaths_Click);
            // 
            // FrmFileSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 103);
            this.Controls.Add(this.btnInteliGetPaths);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.tbFileSetName);
            this.Controls.Add(this.lblFileSet);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmFileSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmFileSet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFileSet;
        private System.Windows.Forms.TextBox tbFileSetName;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnInteliGetPaths;
    }
}