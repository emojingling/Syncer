using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace chenz
{
    public partial class FrmMain : Form
    {
        private DataTable _dtFileList;
        private SQLiteConnection _conn;

        public FrmMain()
        {
            InitializeComponent();

            _conn = SycerSQLiteHelper.ConnectMainDb();
            System.Diagnostics.Debug.Assert(_conn != null);
            ReadDataBase();
        }

        private void ReadDataBase()
        {
            var dtSyncFile = MySQLiteHelper.GetDataTable("SyncFile", _conn);
            var dtLinkedFile = MySQLiteHelper.GetDataTable("LinkedFile", _conn);

            var dvSyncFile = dtSyncFile.DefaultView; //对dtSyncFile按ID顺序排序
            dvSyncFile.Sort = "ID Asc";
            dtSyncFile = dvSyncFile.ToTable();

            var dicFileNum = new Dictionary<int, int>(); //每行数据包含的同步文件数量字典
            foreach (DataRow dr in dtLinkedFile.AsEnumerable())
            {
                int id_SyncFile = Convert.ToInt32(dr["ID_SyncFile"]);
                if (dicFileNum.Keys.Contains(id_SyncFile))
                {
                    dicFileNum[id_SyncFile]++;
                }
                else
                {
                    dicFileNum.Add(id_SyncFile, 1);
                }
            }

            _dtFileList = new DataTable();
            _dtFileList.Columns.Add("ID", typeof (int));
            _dtFileList.Columns.Add("FileSetName", typeof (string));
            _dtFileList.Columns.Add("LastUpdateDate", typeof (string));
            _dtFileList.Columns.Add("FileNum", typeof (int));
            _dtFileList.Columns.Add("UpdateTimes", typeof (int));
            _dtFileList.AcceptChanges();

            foreach (DataRow dr in dtSyncFile.AsEnumerable())
            {
                int id = Convert.ToInt32(dr["ID"]);

                DataRow drAdd = _dtFileList.NewRow();
                drAdd["ID"] = id;
                drAdd["FileSetName"] = dr["FileSetName"].ToString();
                drAdd["LastUpdateDate"] = DateTime.FromBinary(Convert.ToInt64(dr["LastUpdateDate"]));
                drAdd["FileNum"] = dicFileNum[id];
                drAdd["UpdateTimes"] = Convert.ToInt32(dr["UpdateTimes"]);
                _dtFileList.Rows.Add(drAdd);
            }
            _dtFileList.AcceptChanges();
            dgViewFileList.DataSource = _dtFileList;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //打开添加数据窗体
            FrmFileSet frm = new FrmFileSet(null);
            if (frm.ShowDialog() != DialogResult.OK) return;
            //向数据库添加数据
            if (!SycerSQLiteHelper.InsertSyncFile(_conn, frm.FileSetName, frm.ListFileInfo))
                MessageBox.Show(@"未能添加新的文件集！", @"文件集添加失败");
            else ReadDataBase();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgViewFileList.SelectedRows == null || dgViewFileList.SelectedCells.Count == 0)
                return;

            SyncFile syncFile;
            var row = dgViewFileList.SelectedCells[0].OwningRow;
            int id = Convert.ToInt32(row.Cells["colID"].Value);
            if (SycerSQLiteHelper.GetSyncFile(_conn, id, out syncFile) && syncFile != null)
            {
                string latestPath;
                DateTime updateTime;
                if (!FileSyncHelper.UpdateFilesToNewest(syncFile.ListFullName, out latestPath, out updateTime) || 
                    string.IsNullOrEmpty(latestPath)) MessageBox.Show(@"同步数据失败！");
                else                //更新数据库文件
                {
                    FileSyncHelper.FreshSyncFile(ref syncFile, latestPath, updateTime, 1);
                    if (!SycerSQLiteHelper.UpdateSyncFile(_conn, syncFile))
                        MessageBox.Show(@"文件已更新，但更新同步信息失败！");
                }
            }
            else
            {
                MessageBox.Show(@"读取同步文件信息失败！");
            }
            ReadDataBase();
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {

        }

        private void btnSearcher_Click(object sender, EventArgs e)
        {
            FrmFileSearcher frm = new FrmFileSearcher();
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void dgViewFileList_DoubleClick(object sender, EventArgs e)
        {
            if (dgViewFileList.SelectedRows == null || dgViewFileList.SelectedCells.Count == 0)
                return;

            SyncFile syncFile;
            var row = dgViewFileList.SelectedCells[0].OwningRow;
            int id = Convert.ToInt32(row.Cells["colID"].Value);
            if (SycerSQLiteHelper.GetSyncFile(_conn, id, out syncFile) && syncFile != null)
            {
                //打开添加数据窗体
                FrmFileSet frm = new FrmFileSet(syncFile.FileSetName, syncFile.ListFullName);
                frm.ShowDialog();
                //向数据库添加数据
                if (!SycerSQLiteHelper.UpdateSyncFile(_conn, frm.FileSetName, frm.ListFileInfo, id))
                    MessageBox.Show(@"未能更新文件集！", @"文件集更新失败");
                else ReadDataBase();

            }
        }
    }
}
