namespace chenz
{
    partial class FrmFileSearcher
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
            this.btnSelectVolume = new System.Windows.Forms.Button();
            this.lbResults = new System.Windows.Forms.ListBox();
            this.lblSelectedVolume = new System.Windows.Forms.Label();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.btnFindFiles = new System.Windows.Forms.Button();
            this.lblListCount = new System.Windows.Forms.Label();
            this.btnSearchAllFolders = new System.Windows.Forms.Button();
            this.btnQueryUsnJournal = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelectVolume
            // 
            this.btnSelectVolume.Font = new System.Drawing.Font("宋体", 10F);
            this.btnSelectVolume.Location = new System.Drawing.Point(12, 12);
            this.btnSelectVolume.Name = "btnSelectVolume";
            this.btnSelectVolume.Size = new System.Drawing.Size(94, 23);
            this.btnSelectVolume.TabIndex = 0;
            this.btnSelectVolume.Text = "选择磁盘";
            this.btnSelectVolume.UseVisualStyleBackColor = true;
            this.btnSelectVolume.Click += new System.EventHandler(this.btnSelectVolume_Click);
            // 
            // lbResults
            // 
            this.lbResults.FormattingEnabled = true;
            this.lbResults.ItemHeight = 12;
            this.lbResults.Location = new System.Drawing.Point(152, 12);
            this.lbResults.Name = "lbResults";
            this.lbResults.Size = new System.Drawing.Size(736, 448);
            this.lbResults.TabIndex = 5;
            // 
            // lblSelectedVolume
            // 
            this.lblSelectedVolume.AutoSize = true;
            this.lblSelectedVolume.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectedVolume.Location = new System.Drawing.Point(149, 467);
            this.lblSelectedVolume.Name = "lblSelectedVolume";
            this.lblSelectedVolume.Size = new System.Drawing.Size(144, 16);
            this.lblSelectedVolume.TabIndex = 2;
            this.lblSelectedVolume.Text = "当前索引磁盘：C盘";
            this.lblSelectedVolume.Visible = false;
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblElapsedTime.Location = new System.Drawing.Point(324, 467);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(128, 16);
            this.lblElapsedTime.TabIndex = 3;
            this.lblElapsedTime.Text = "执行用时：100ms";
            this.lblElapsedTime.Visible = false;
            // 
            // btnFindFiles
            // 
            this.btnFindFiles.Enabled = false;
            this.btnFindFiles.Font = new System.Drawing.Font("宋体", 10F);
            this.btnFindFiles.Location = new System.Drawing.Point(12, 320);
            this.btnFindFiles.Name = "btnFindFiles";
            this.btnFindFiles.Size = new System.Drawing.Size(94, 23);
            this.btnFindFiles.TabIndex = 2;
            this.btnFindFiles.Text = "查找文件";
            this.btnFindFiles.UseVisualStyleBackColor = true;
            this.btnFindFiles.Click += new System.EventHandler(this.btnFindFiles_Click);
            // 
            // lblListCount
            // 
            this.lblListCount.AutoSize = true;
            this.lblListCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblListCount.Location = new System.Drawing.Point(483, 467);
            this.lblListCount.Name = "lblListCount";
            this.lblListCount.Size = new System.Drawing.Size(104, 16);
            this.lblListCount.TabIndex = 5;
            this.lblListCount.Text = "找到10个文件";
            this.lblListCount.Visible = false;
            // 
            // btnSearchAllFolders
            // 
            this.btnSearchAllFolders.Enabled = false;
            this.btnSearchAllFolders.Font = new System.Drawing.Font("宋体", 10F);
            this.btnSearchAllFolders.Location = new System.Drawing.Point(12, 349);
            this.btnSearchAllFolders.Name = "btnSearchAllFolders";
            this.btnSearchAllFolders.Size = new System.Drawing.Size(94, 23);
            this.btnSearchAllFolders.TabIndex = 3;
            this.btnSearchAllFolders.Text = "文件夹列表";
            this.btnSearchAllFolders.UseVisualStyleBackColor = true;
            this.btnSearchAllFolders.Click += new System.EventHandler(this.btnSearchAllFolders_Click);
            // 
            // btnQueryUsnJournal
            // 
            this.btnQueryUsnJournal.Enabled = false;
            this.btnQueryUsnJournal.Font = new System.Drawing.Font("宋体", 10F);
            this.btnQueryUsnJournal.Location = new System.Drawing.Point(12, 41);
            this.btnQueryUsnJournal.Name = "btnQueryUsnJournal";
            this.btnQueryUsnJournal.Size = new System.Drawing.Size(94, 23);
            this.btnQueryUsnJournal.TabIndex = 1;
            this.btnQueryUsnJournal.Text = "日志信息";
            this.btnQueryUsnJournal.UseVisualStyleBackColor = true;
            this.btnQueryUsnJournal.Click += new System.EventHandler(this.btnQueryUsnJournal_Click);
            // 
            // btnBack
            // 
            this.btnBack.Enabled = false;
            this.btnBack.Font = new System.Drawing.Font("宋体", 10F);
            this.btnBack.Location = new System.Drawing.Point(12, 437);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(94, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "返回";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // FrmFileSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 492);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnQueryUsnJournal);
            this.Controls.Add(this.btnSearchAllFolders);
            this.Controls.Add(this.lblListCount);
            this.Controls.Add(this.btnFindFiles);
            this.Controls.Add(this.lblElapsedTime);
            this.Controls.Add(this.lblSelectedVolume);
            this.Controls.Add(this.lbResults);
            this.Controls.Add(this.btnSelectVolume);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFileSearcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找文件";
            this.Activated += new System.EventHandler(this.FrmFileSearcher_Activated);
            this.Deactivate += new System.EventHandler(this.FrmFileSearcher_Deactivate);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectVolume;
        private System.Windows.Forms.ListBox lbResults;
        private System.Windows.Forms.Label lblSelectedVolume;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Button btnFindFiles;
        private System.Windows.Forms.Label lblListCount;
        private System.Windows.Forms.Button btnSearchAllFolders;
        private System.Windows.Forms.Button btnQueryUsnJournal;
        private System.Windows.Forms.Button btnBack;
    }
}