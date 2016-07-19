using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            var dtSyncFile = SycerSQLiteHelper.GetDataTable("SyncFile", _conn);
            var dtLinkedFile = SycerSQLiteHelper.GetDataTable("LinkedFile", _conn);

            var dvSyncFile = dtSyncFile.DefaultView;            //对dtSyncFile按ID顺序排序
            dvSyncFile.Sort = "ID Asc";
            dtSyncFile = dvSyncFile.ToTable();

            var dicFileNum = new Dictionary<int, int>();        //每行数据包含的同步文件数量字典
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
            _dtFileList.Columns.Add("ID", typeof(int));
            _dtFileList.Columns.Add("FileSetName", typeof(string));
            _dtFileList.Columns.Add("LastUpdateDate", typeof(string));
            _dtFileList.Columns.Add("FileNum", typeof(int));
            _dtFileList.Columns.Add("UpdateTimes", typeof(int));
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgViewFileList.SelectedRows == null || dgViewFileList.SelectedRows.Count == 0)
                return;


        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {

        }

        private void btnSearcher_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void dgViewFileList_DoubleClick(object sender, EventArgs e)
        {
            if (dgViewFileList.SelectedRows == null || dgViewFileList.SelectedRows.Count == 0)
                return;

            SyncFile syncFile;
            int id = Convert.ToInt32(dgViewFileList.CurrentRow.Cells["colID"].Value);
            if (SycerSQLiteHelper.GetSyncFile(_conn, id, out syncFile) && syncFile != null)
            {
                FrmFileSet frm = new FrmFileSet(syncFile.FileSetName, syncFile.ListFullName);
                frm.ShowDialog();
            }
        }
    }
}
