namespace chenz
{
    partial class FrmMain
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgViewFileList = new System.Windows.Forms.DataGridView();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.btnSearcher = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileSetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdateTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgViewFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgViewFileList
            // 
            this.dgViewFileList.AllowUserToAddRows = false;
            this.dgViewFileList.AllowUserToDeleteRows = false;
            this.dgViewFileList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgViewFileList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgViewFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgViewFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colFileSetName,
            this.colLastUpdateDate,
            this.colFileNum,
            this.colUpdateTimes});
            this.dgViewFileList.Location = new System.Drawing.Point(12, 12);
            this.dgViewFileList.Name = "dgViewFileList";
            this.dgViewFileList.ReadOnly = true;
            this.dgViewFileList.RowTemplate.Height = 23;
            this.dgViewFileList.Size = new System.Drawing.Size(778, 390);
            this.dgViewFileList.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdate.Location = new System.Drawing.Point(12, 408);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(153, 43);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "更新所选行";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUpdateAll.Location = new System.Drawing.Point(187, 408);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(153, 43);
            this.btnUpdateAll.TabIndex = 2;
            this.btnUpdateAll.Text = "更新全部行";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // btnSearcher
            // 
            this.btnSearcher.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearcher.Location = new System.Drawing.Point(362, 408);
            this.btnSearcher.Name = "btnSearcher";
            this.btnSearcher.Size = new System.Drawing.Size(153, 43);
            this.btnSearcher.TabIndex = 3;
            this.btnSearcher.Text = "搜索文件";
            this.btnSearcher.UseVisualStyleBackColor = true;
            this.btnSearcher.Click += new System.EventHandler(this.btnSearcher_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExit.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.Location = new System.Drawing.Point(637, 408);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(153, 43);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // colID
            // 
            this.colID.DataPropertyName = "ID";
            this.colID.FillWeight = 40F;
            this.colID.HeaderText = "序号";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            // 
            // colFileSetName
            // 
            this.colFileSetName.DataPropertyName = "FileSetName";
            this.colFileSetName.HeaderText = "文件集名称";
            this.colFileSetName.Name = "colFileSetName";
            this.colFileSetName.ReadOnly = true;
            // 
            // colLastUpdateDate
            // 
            this.colLastUpdateDate.DataPropertyName = "LastUpdateDate";
            this.colLastUpdateDate.FillWeight = 80F;
            this.colLastUpdateDate.HeaderText = "最后更新日期";
            this.colLastUpdateDate.Name = "colLastUpdateDate";
            this.colLastUpdateDate.ReadOnly = true;
            // 
            // colFileNum
            // 
            this.colFileNum.DataPropertyName = "FileNum";
            this.colFileNum.FillWeight = 30F;
            this.colFileNum.HeaderText = "文件数";
            this.colFileNum.Name = "colFileNum";
            this.colFileNum.ReadOnly = true;
            // 
            // colUpdateTimes
            // 
            this.colUpdateTimes.DataPropertyName = "UpdateTimes";
            this.colUpdateTimes.FillWeight = 40F;
            this.colUpdateTimes.HeaderText = "已更新次数";
            this.colUpdateTimes.Name = "colUpdateTimes";
            this.colUpdateTimes.ReadOnly = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 458);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSearcher);
            this.Controls.Add(this.btnUpdateAll);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.dgViewFileList);
            this.Name = "FrmMain";
            this.Text = "FrmMain";
            ((System.ComponentModel.ISupportInitialize)(this.dgViewFileList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgViewFileList;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnUpdateAll;
        private System.Windows.Forms.Button btnSearcher;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdateTimes;
    }
}