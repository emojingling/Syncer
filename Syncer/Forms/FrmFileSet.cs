using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace chenz
{
    public partial class FrmFileSet : Form
    {
        private int countBox = 0;
        private List<FileInfoLite> _listFileInfo;
        private int widthBox;
        private int heightBox;
        private int screenHeight;
        private FileInfoBox[] fileInfoBoxs;
        private const int HeightDiv = 6;
        private const int LeftBox = 9;
        private const int TopFirstBox = 42;
        private const int MaxBoxCount = 10;

        /// <summary>文件集名称</summary>
        public string FileSetName
        {
            get { return tbFileSetName.Text; }
            set { tbFileSetName.Text = value; }
        }

        /// <summary>录入的文件信息集合</summary>
        public List<FileInfoLite> ListFileInfo
        {
            get { return _listFileInfo; }
            set { _listFileInfo = value; }
        }

        public FrmFileSet(string fileSetName)
        {
            InitializeComponent();

            FrmInti(fileSetName);
            AddBoxs(2);
            GetFileInfoList();
        }

        public FrmFileSet(string fileSetName, List<string> listFullName)
        {

            InitializeComponent();

            System.Diagnostics.Debug.Assert(listFullName != null);
            FrmInti(fileSetName);
            AddBoxs(listFullName.Count, listFullName);
            GetFileInfoList();
        }

        /// <summary>窗体初始化</summary>
        /// <param name="fileSetName"></param>
        private void FrmInti(string fileSetName)
        {
            if (fileSetName == null) fileSetName = string.Empty;
            FileSetName = fileSetName;
            Screen currentScreen = Screen.FromControl(this);
            screenHeight = currentScreen.Bounds.Height;

            var fileInfoBox = new FileInfoBox(0);
            widthBox = fileInfoBox.Width;
            heightBox = fileInfoBox.Height;
            this.Width = widthBox + 2 * LeftBox + Padding.Left + Padding.Right;

            fileInfoBoxs = new FileInfoBox[MaxBoxCount];
        }

        /// <summary>调整窗体高度</summary>
        private void AdjustFrmHeight()
        {
            Top = (screenHeight - Height) / 2;
        }

        /// <summary>添加输入行</summary>
        /// <param name="count">添加数量</param>
        /// <param name="listFullName">文件完整路径列表</param>
        private void AddBoxs(int count, List<string> listFullName = null)
        {
            int countCanAdd = MaxBoxCount - countBox;
            if (countCanAdd <= 0) return;
            if (count > countCanAdd) count = countCanAdd;

            int fileDonotExist = 0;
            for (int i = 0; i < count; i++)
            {
                if (listFullName == null) AddBox();
                else if (File.Exists(listFullName[i])) AddBox(listFullName[i]);
                else fileDonotExist++;
            }
            if (count - fileDonotExist < 2)
            {
                for (int i = 0; i < 2 - count + fileDonotExist; i++) AddBox();
            }
        }

        /// <summary>添加输入行</summary>
        /// <param name="fullPath">文件完整路径</param>
        private void AddBox(string fullPath = null)
        {
            if (countBox >= 10) return;

            Height += heightBox + HeightDiv;
            int top = TopFirstBox + countBox * (heightBox + HeightDiv) + HeightDiv;
            var fileInfoBox = new FileInfoBox(++countBox);
            fileInfoBox.Left = LeftBox;
            fileInfoBox.Top = top;
            if (fullPath != null && fullPath.Contains('\\')) fileInfoBox.FullPath = fullPath;
            fileInfoBox.ButtonSelectPathClick += SelectPath;
            fileInfoBox.ButtonAddClick += AddBox;
            fileInfoBox.ButtonDeleteClick += DeleteBox;
            fileInfoBoxs[countBox - 1] = fileInfoBox;

            this.Controls.Add(fileInfoBox);

            AdjustFrmHeight();

            Invalidate();
        }

        /// <summary>删除输入行</summary>
        /// <param name="count">被删除的行序号</param>
        private void DeleteBox(int num)
        {
            if (countBox <= 2) return;

            this.Controls.Remove(fileInfoBoxs[num - 1]);

            fileInfoBoxs[num - 1].Dispose();
            fileInfoBoxs[num - 1] = null;
            if (num != countBox)
            {
                for (int i = num; i < countBox; i++)
                {
                    fileInfoBoxs[i - 1] = fileInfoBoxs[i];
                    fileInfoBoxs[i - 1].Num--;
                    fileInfoBoxs[i - 1].Top -= (heightBox + HeightDiv);
                }
                fileInfoBoxs[countBox - 1] = null;
            }

            Height -= (heightBox + HeightDiv);
            countBox--;

            AdjustFrmHeight();

            Invalidate();
        }

        /// <summary>响应自定义控件的选择文件事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPath(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "所有文件|*.*";
            dlg.RestoreDirectory = true;
            dlg.FilterIndex = 1;
            dlg.CheckPathExists = true;
            dlg.Multiselect = false;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlg.SafeFileName;
                string foldPath = dlg.FileName.Substring(0, dlg.FileName.LastIndexOf('\\'));

                Button btnSelectPath = sender as Button;
                System.Diagnostics.Debug.Assert(btnSelectPath != null);
                FileInfoBox fileInfoBox = btnSelectPath.Parent as FileInfoBox;
                System.Diagnostics.Debug.Assert(fileInfoBox != null);
                fileInfoBox.FileName = fileName;
                fileInfoBox.FilePath = foldPath;
            }
        }

        /// <summary>响应自定义控件的添加行事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBox(object sender, EventArgs e)
        {
            AddBox();
        }

        /// <summary>响应自定义控件的删除行事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteBox(object sender, EventArgs e)
        {
            Button btnDelete = sender as Button;
            System.Diagnostics.Debug.Assert(btnDelete != null);
            FileInfoBox fileInfoBox = btnDelete.Parent as FileInfoBox;
            System.Diagnostics.Debug.Assert(fileInfoBox != null);

            DeleteBox(fileInfoBox.Num);
        }

        /// <summary>判断文件名输入控件中的文件名与路径是否存在</summary>
        /// <param name="fileInfoBox">文件名输入控件</param>
        /// <param name="fileInfo">[out]输出参数，输出文件信息。文件不存在时输出null</param>
        /// <returns>文件名与路径是否存在</returns>
        private bool FileExist(FileInfoBox fileInfoBox, out FileInfo fileInfo)
        {
            fileInfo = null;
            if (fileInfoBox == null) return false;

            string foldPath = fileInfoBox.FilePath;
            if (foldPath.EndsWith("\\")) foldPath.Substring(0, foldPath.Length - 1);
            if (!Directory.Exists(foldPath)) return false;
            foldPath += "\\";
            string fullPath = foldPath + fileInfoBox.FileName;
            if (File.Exists(fullPath))
            {
                fileInfo = new FileInfo(fullPath);
                return true;
            }
            return false;
        }

        /// <summary>检查用户输入项</summary>
        /// <returns>输入是否合格</returns>
        private bool CheckUserInput()
        {
            if (string.IsNullOrWhiteSpace(FileSetName))
            {
                MessageBox.Show("请输入文件集名称！");
                return false;
            }

            int validLines = countBox;
            List<string> list = new List<string>(countBox);
            for (int i = 0; i < countBox; i++)
            {
                FileInfoBox fileInfoBox = fileInfoBoxs[i];
                if (string.IsNullOrWhiteSpace(FileSetName) || string.IsNullOrWhiteSpace(FileSetName))
                {
                    validLines--;
                    continue;
                }
                FileInfo fileInfo;
                if (!FileExist(fileInfoBox, out fileInfo))
                {
                    validLines--;
                    continue;
                }
                if (fileInfo != null && list.Contains(fileInfo.FullName))
                {
                    validLines--;
                    fileInfoBox.Clear();
                }
                else
                {
                    list.Add(fileInfo.FullName);
                }
            }
            if (validLines < 2)
            {
                MessageBox.Show("请至少输入两个完整的待同步文件信息！");
                return false;
            }

            return true;
        }

        /// <summary>得到用户输入的文件信息列表</summary>
        private void GetFileInfoList()
        {
            if (ListFileInfo == null) ListFileInfo = new List<FileInfoLite>(MaxBoxCount);
            ListFileInfo.Clear();
            for (int i = 0; i < countBox; i++)
            {
                FileInfoBox fileInfoBox = fileInfoBoxs[i];
                FileInfo fileInfo;
                if (FileExist(fileInfoBox, out fileInfo) && fileInfo != null)
                {
                    ListFileInfo.Add(new FileInfoLite(fileInfo));
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (CheckUserInput() == false) return;
            GetFileInfoList();

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int validLines = countBox;
            for (int i = 0; i < countBox; i++)
            {
                fileInfoBoxs[i].Clear();
            }
        }
    }
}
