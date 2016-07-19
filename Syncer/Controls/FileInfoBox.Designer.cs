namespace chenz
{
    partial class FileInfoBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileInfoBox));
            this.lblNum = new System.Windows.Forms.Label();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.lblBlack1 = new System.Windows.Forms.Label();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.lblBlack2 = new System.Windows.Forms.Label();
            this.tbSelectPath = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.BackColor = System.Drawing.Color.Transparent;
            this.lblNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblNum.Font = new System.Drawing.Font("宋体", 10F);
            this.lblNum.Location = new System.Drawing.Point(3, 8);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(21, 14);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "10";
            // 
            // tbFileName
            // 
            this.tbFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFileName.Font = new System.Drawing.Font("宋体", 12F);
            this.tbFileName.Location = new System.Drawing.Point(32, 5);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(178, 19);
            this.tbFileName.TabIndex = 0;
            // 
            // lblBlack1
            // 
            this.lblBlack1.BackColor = System.Drawing.Color.Black;
            this.lblBlack1.Location = new System.Drawing.Point(32, 25);
            this.lblBlack1.Name = "lblBlack1";
            this.lblBlack1.Size = new System.Drawing.Size(178, 1);
            this.lblBlack1.TabIndex = 3;
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.BackColor = System.Drawing.Color.White;
            this.btnSelectPath.FlatAppearance.BorderSize = 0;
            this.btnSelectPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectPath.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectPath.Image")));
            this.btnSelectPath.Location = new System.Drawing.Point(519, 5);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(25, 21);
            this.btnSelectPath.TabIndex = 2;
            this.btnSelectPath.UseVisualStyleBackColor = false;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // lblBlack2
            // 
            this.lblBlack2.BackColor = System.Drawing.Color.Black;
            this.lblBlack2.Location = new System.Drawing.Point(222, 25);
            this.lblBlack2.Name = "lblBlack2";
            this.lblBlack2.Size = new System.Drawing.Size(322, 1);
            this.lblBlack2.TabIndex = 6;
            // 
            // tbSelectPath
            // 
            this.tbSelectPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSelectPath.Font = new System.Drawing.Font("宋体", 12F);
            this.tbSelectPath.Location = new System.Drawing.Point(223, 5);
            this.tbSelectPath.Name = "tbSelectPath";
            this.tbSelectPath.Size = new System.Drawing.Size(296, 19);
            this.tbSelectPath.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(546, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(25, 21);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(571, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(25, 21);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // FileInfoBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblBlack2);
            this.Controls.Add(this.tbSelectPath);
            this.Controls.Add(this.lblBlack1);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.lblNum);
            this.Name = "FileInfoBox";
            this.Size = new System.Drawing.Size(600, 29);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label lblBlack1;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.Label lblBlack2;
        private System.Windows.Forms.TextBox tbSelectPath;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
    }
}
