using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using UsnEntry = chenz.WINAPI.UsnEntry;
using USN_JOURNAL_DATA = chenz.WINAPI.USN_JOURNAL_DATA;

namespace chenz
{
    public partial class FrmFileSearcher : Form
    {
        #region Properties

        private NtfsUsnJournal _usnJournal = null;
        public NtfsUsnJournal Journal
        {
            get { return _usnJournal; }
        }

        #endregion

        #region private member variables
        private const int WM_HOTKEY = 0x0312;

        private WINAPI.USN_JOURNAL_DATA _usnCurrentJournalState;

        private double _lbItemX;
        private double _lbItemY;

        #endregion

        #region delegates

        private delegate void TransUsnEntryDelegate(List<UsnEntry> fileList);
        private delegate void TransUsnJournalReturnCodeDelegate(NtfsUsnJournal.UsnJournalReturnCode rtnCode);
        private delegate void TransAnythingDelegate(object anything);

        private delegate void FillListBoxDelegate(NtfsUsnJournal.UsnJournalReturnCode rtnCode, List<UsnEntry> usnEntries, USN_JOURNAL_DATA newUsnState);
        private delegate void FillListBoxWithFilesDelagate(NtfsUsnJournal.UsnJournalReturnCode rtnCode, List<UsnEntry> fileList);
        private delegate void FillListBoxWithFoldersDelegate(NtfsUsnJournal.UsnJournalReturnCode rtnCode, List<UsnEntry> folders);

        #endregion

        #region constructor(s)

        public FrmFileSearcher()
        {
            InitializeComponent();
        }

        #endregion

        #region FormEventHandler

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 101:    //按下的是Ctrl+C
                            {
                                if (lbResults.SelectedItem != null)
                                    Clipboard.SetDataObject(lbResults.SelectedItem);
                                break;
                            }
                        default:
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void FrmFileSearcher_Activated(object sender, EventArgs e)
        {
            //注册热键Ctrl+C，Id号为101。
            WINAPI.RegisterHotKey(Handle, 101, WINAPI.KeyModifiers.Ctrl, Keys.C);
        }

        private void FrmFileSearcher_Deactivate(object sender, EventArgs e)
        {
            //注销Id号为101的热键设定
            WINAPI.UnregisterHotKey(Handle, 101);
        }

        private void btnSelectVolume_Click(object sender, EventArgs e)
        {
            lbResults.DataSource = null;
            lbResults.Items.Clear();

            FrmVolumeSelect frmVolumeSelect = new FrmVolumeSelect();
            frmVolumeSelect.ShowDialog();
            if (frmVolumeSelect.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    _usnJournal = new NtfsUsnJournal(frmVolumeSelect.SelectedDriveInfo);
                }
                catch (Exception ex)
                {
                    lblSelectedVolume.Visible = true;
                    lblElapsedTime.Visible = false;
                    lblSelectedVolume.Text = ex.Message.Contains("Access is denied")
                        ? @"请使用管理员权限重新打开本程序！"
                        : @"未能成功打开磁盘！";
                    return;
                }

                lblSelectedVolume.Visible = true;
                lblElapsedTime.Visible = true;
                lblSelectedVolume.Text = string.Format("当前索引磁盘：{0}盘", frmVolumeSelect.SelectedDriveInfo.Name);
                lblElapsedTime.Text = string.Format("执行用时：{0}ms", NtfsUsnJournal.ElapsedTime.Milliseconds.ToString());

                btnQueryUsnJournal.Enabled = true;
                btnFindFiles.Enabled = true;
                btnSearchAllFolders.Enabled = true;
            }
            else
            {
                lblSelectedVolume.Visible = true;
                lblSelectedVolume.Text = @"未能打开任何磁盘。";
            }
        }

        private void btnFindFiles_Click(object sender, EventArgs e)
        {
            FrmFileFilterStr frm = new FrmFileFilterStr();
            frm.ShowDialog();

            string fileFilter = frm.tbFilter.Text;
            if (frm.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(fileFilter))
            {
                lbResults.DataSource = null;
                lbResults.Items.Clear();

                Cursor = Cursors.WaitCursor;

                Thread usnJournalThread = new Thread(ListFilesAsync);
                usnJournalThread.Start(fileFilter);
            }
        }

        private void btnSearchAllFolders_Click(object sender, EventArgs e)
        {
            lbResults.DataSource = null;
            lbResults.Items.Clear();

            Cursor = Cursors.WaitCursor;

            Thread usnJournalThread = new Thread(ListFoldersAsync);
            usnJournalThread.Start();
        }

        private void btnQueryUsnJournal_Click(object sender, EventArgs e)
        {
            lbResults.DataSource = null;
            lbResults.Items.Clear();

            USN_JOURNAL_DATA journalState = new USN_JOURNAL_DATA();
            NtfsUsnJournal.UsnJournalReturnCode rtn = _usnJournal.GetUsnJournalState(ref journalState);

            lblElapsedTime.Visible = true;
            lblElapsedTime.Text = string.Format("执行用时：{0}ms", NtfsUsnJournal.ElapsedTime.Milliseconds.ToString());
            lblListCount.Text = string.Empty;

            if (rtn == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
            {
                lbResults.Items.AddRange(FormatUsnJournalState(journalState));
            }
            else
            {
                lbResults.Items.Add(string.Format("{0} 执行失败！错误码: {1}。", "GetUsnJournalState()", rtn.ToString()));
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion

        #region functions

        /// <summary>异步查找文件列表。</summary>
        /// <param name="fileFilterObj">文件适配字符串</param>
        private void ListFilesAsync(object fileFilterObj)
        {
            string fileFilter = (string)fileFilterObj;
            List<UsnEntry> fileList;
            var rtnCode = _usnJournal.GetFilesMatchingFilter(fileFilter, out fileList);

            Cursor = Cursors.Arrow;

            FreshSearchResultsInvoke(rtnCode, fileList);
        }

        /// <summary>异步查找全部文件夹列表</summary>
        private void ListFoldersAsync()
        {
            List<UsnEntry> folderList;
            var rtnCode = _usnJournal.GetNtfsVolumeFolders(out folderList);

            Cursor = Cursors.Arrow;

            FreshSearchResultsInvoke(rtnCode, folderList);
        }

        /// <summary>以线程安全的方式刷新列表等查找结果显示控件。</summary>
        /// <param name="rtnCode">查找返回码。</param>
        /// <param name="entryList">显示项列表。</param>
        private void FreshSearchResultsInvoke(NtfsUsnJournal.UsnJournalReturnCode rtnCode, List<UsnEntry> entryList)
        {
            if (rtnCode == NtfsUsnJournal.UsnJournalReturnCode.USN_JOURNAL_SUCCESS)
            {
                if (entryList.Count > 0)
                {
                    lbResults.Invoke(new TransUsnEntryDelegate((list) =>
                    {
                        var names = list.Select(ent => ent.Name);
                        lbResults.DataSource = names.ToList();
                    }), entryList);

                    lblListCount.Invoke(new TransUsnEntryDelegate((list) =>
                    {
                        lblListCount.Visible = true;
                        lblListCount.Text = string.Format("找到{0}条记录", list.Count);
                    }), entryList);

                    string elapsedTime = NtfsUsnJournal.ElapsedTime.Milliseconds.ToString();
                    lblElapsedTime.Invoke(new TransAnythingDelegate((time) =>
                    {
                        lblElapsedTime.Visible = true;
                        lblElapsedTime.Text = string.Format("执行用时：{0}ms", time);
                    }), elapsedTime);
                }
            }
            else
            {
                lblListCount.Invoke(new TransUsnJournalReturnCodeDelegate((code) =>
                {
                    lblListCount.Visible = true;
                    lblListCount.Text = string.Format("查找出现错误，错误码：{0}.", code.ToString());
                }), rtnCode);
            }
        }

        /// <summary>将日志信息转化为字符串。</summary>
        /// <param name="usnCurrentJournalState">当前日志信息</param>
        /// <returns></returns>
        private string[] FormatUsnJournalState(USN_JOURNAL_DATA usnCurrentJournalState)
        {
            List<string> list = new List<string>(8);
            list.Add(string.Format("日志ID: {0}", usnCurrentJournalState.UsnJournalID.ToString("X")));
            list.Add(string.Format("第一个USN: {0}", usnCurrentJournalState.FirstUsn.ToString("X")));
            list.Add(string.Format("下一个USN: {0}", usnCurrentJournalState.NextUsn.ToString("X")));
            list.Add(string.Empty);
            list.Add(string.Format("最小有效USN: {0}", usnCurrentJournalState.LowestValidUsn.ToString("X")));
            list.Add(string.Format("最大USN: {0}", usnCurrentJournalState.MaxUsn.ToString("X")));
            list.Add(string.Format("最大体积: {0}", usnCurrentJournalState.MaximumSize.ToString("X")));
            list.Add(string.Format("单次增长体积: {0}", usnCurrentJournalState.AllocationDelta.ToString("X")));
            return list.ToArray();
        }

        #endregion

    }
}
