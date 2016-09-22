namespace chenz
{
    partial class FrmEverything
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEverything));
            this.btnBackGround = new System.Windows.Forms.Button();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblReadingStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgView = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastEditDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbContainPath = new System.Windows.Forms.CheckBox();
            this.cmsFile = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenPathPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyFileNameCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbRegex = new System.Windows.Forms.CheckBox();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.cmsFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBackGround
            // 
            this.btnBackGround.Location = new System.Drawing.Point(0, 0);
            this.btnBackGround.Name = "btnBackGround";
            this.btnBackGround.Size = new System.Drawing.Size(1007, 30);
            this.btnBackGround.TabIndex = 0;
            this.btnBackGround.UseVisualStyleBackColor = true;
            // 
            // tbInput
            // 
            this.tbInput.Location = new System.Drawing.Point(5, 4);
            this.tbInput.Margin = new System.Windows.Forms.Padding(0);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(998, 21);
            this.tbInput.TabIndex = 1;
            this.tbInput.TextChanged += new System.EventHandler(this.tbInput_TextChanged);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblReadingStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 520);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1136, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblReadingStatus
            // 
            this.lblReadingStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblReadingStatus.Name = "lblReadingStatus";
            this.lblReadingStatus.Size = new System.Drawing.Size(104, 17);
            this.lblReadingStatus.Text = "文件整理中。。。";
            // 
            // dgView
            // 
            this.dgView.AllowUserToAddRows = false;
            this.dgView.AllowUserToDeleteRows = false;
            this.dgView.AllowUserToResizeRows = false;
            this.dgView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgView.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colPath,
            this.colSize,
            this.colLastEditDate});
            this.dgView.Location = new System.Drawing.Point(3, 31);
            this.dgView.Name = "dgView";
            this.dgView.ReadOnly = true;
            this.dgView.RowTemplate.Height = 23;
            this.dgView.Size = new System.Drawing.Size(1130, 487);
            this.dgView.TabIndex = 3;
            this.dgView.VirtualMode = true;
            this.dgView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgView_CellMouseClick);
            this.dgView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgView_CellMouseDoubleClick);
            this.dgView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgView_CellValueNeeded);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.FillWeight = 80F;
            this.colName.HeaderText = "名称";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colPath
            // 
            this.colPath.DataPropertyName = "Path";
            this.colPath.HeaderText = "路径";
            this.colPath.Name = "colPath";
            this.colPath.ReadOnly = true;
            // 
            // colSize
            // 
            this.colSize.FillWeight = 30F;
            this.colSize.HeaderText = "大小";
            this.colSize.Name = "colSize";
            this.colSize.ReadOnly = true;
            // 
            // colLastEditDate
            // 
            this.colLastEditDate.FillWeight = 40F;
            this.colLastEditDate.HeaderText = "修改时间";
            this.colLastEditDate.Name = "colLastEditDate";
            this.colLastEditDate.ReadOnly = true;
            // 
            // cbContainPath
            // 
            this.cbContainPath.AutoSize = true;
            this.cbContainPath.Enabled = false;
            this.cbContainPath.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.cbContainPath.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.cbContainPath.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cbContainPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbContainPath.Location = new System.Drawing.Point(1064, 7);
            this.cbContainPath.Name = "cbContainPath";
            this.cbContainPath.Size = new System.Drawing.Size(69, 16);
            this.cbContainPath.TabIndex = 4;
            this.cbContainPath.Text = "包含目录";
            this.cbContainPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbContainPath.UseVisualStyleBackColor = true;
            this.cbContainPath.CheckedChanged += new System.EventHandler(this.cbContainPath_CheckedChanged);
            // 
            // cmsFile
            // 
            this.cmsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenOToolStripMenuItem,
            this.OpenPathPToolStripMenuItem,
            this.CopyFileNameCToolStripMenuItem});
            this.cmsFile.Name = "cmsFile";
            this.cmsFile.Size = new System.Drawing.Size(213, 70);
            // 
            // OpenOToolStripMenuItem
            // 
            this.OpenOToolStripMenuItem.Name = "OpenOToolStripMenuItem";
            this.OpenOToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.OpenOToolStripMenuItem.Text = "打开(&O)";
            this.OpenOToolStripMenuItem.Click += new System.EventHandler(this.OpenOToolStripMenuItem_Click);
            // 
            // OpenPathPToolStripMenuItem
            // 
            this.OpenPathPToolStripMenuItem.Name = "OpenPathPToolStripMenuItem";
            this.OpenPathPToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.OpenPathPToolStripMenuItem.Text = "打开路径(&P)";
            this.OpenPathPToolStripMenuItem.Click += new System.EventHandler(this.OpenPathPToolStripMenuItem_Click);
            // 
            // CopyFileNameCToolStripMenuItem
            // 
            this.CopyFileNameCToolStripMenuItem.Name = "CopyFileNameCToolStripMenuItem";
            this.CopyFileNameCToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.CopyFileNameCToolStripMenuItem.Text = "复制完整路径和文件名(&C)";
            this.CopyFileNameCToolStripMenuItem.Click += new System.EventHandler(this.CopyFileNameCToolStripMenuItem_Click);
            // 
            // cbRegex
            // 
            this.cbRegex.AutoSize = true;
            this.cbRegex.Enabled = false;
            this.cbRegex.FlatAppearance.CheckedBackColor = System.Drawing.Color.Silver;
            this.cbRegex.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.cbRegex.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.cbRegex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRegex.Location = new System.Drawing.Point(1013, 7);
            this.cbRegex.Name = "cbRegex";
            this.cbRegex.Size = new System.Drawing.Size(45, 16);
            this.cbRegex.TabIndex = 6;
            this.cbRegex.Text = "正则";
            this.cbRegex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbRegex.UseVisualStyleBackColor = true;
            this.cbRegex.CheckedChanged += new System.EventHandler(this.cbRegex_CheckedChanged);
            // 
            // FrmEverything
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1136, 542);
            this.Controls.Add(this.cbRegex);
            this.Controls.Add(this.cbContainPath);
            this.Controls.Add(this.dgView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tbInput);
            this.Controls.Add(this.btnBackGround);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmEverything";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "全盘搜索";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.cmsFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBackGround;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblReadingStatus;
        private System.Windows.Forms.DataGridView dgView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastEditDate;
        private System.Windows.Forms.CheckBox cbContainPath;
        private System.Windows.Forms.ContextMenuStrip cmsFile;
        private System.Windows.Forms.ToolStripMenuItem OpenOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenPathPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CopyFileNameCToolStripMenuItem;
        private System.Windows.Forms.CheckBox cbRegex;
    }
}