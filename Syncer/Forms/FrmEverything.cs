using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace chenz
{
    public partial class FrmEverything : Form
    {
        private List<NamePath> _listPath;
        private List<NamePath> _listFile;

        private DateTime _lastSearchInputTime = DateTime.Now;
        private bool _selected = false;     //已经完成排序
        private bool _selecting = false;    //排序函数正在运行中
        private System.Timers.Timer _timerSelect;

        private List<NamePath> _listSelected;
        private bool _readed = false;

        private int _cmsRowIndex = -1;

        private Func<FrmEverythingInputInfo> _funcGetInput;
        private Action<int> _actFreshDvRowCount;
        private Action<List<NamePath>> _actReading;
        private Action<int> _actExit;

        public FrmEverything()
        {
            InitializeComponent();
            //Gets user inputs delegate.
            _funcGetInput = () =>
            {
                FrmEverythingInputInfo i = new FrmEverythingInputInfo();
                i.ContainPath = cbContainPath.Checked;
                i.ExpInput = tbInput.Text;
                i.UseRegex = cbRegex.Checked;
                return i;
            };
            //FreshDvRowCount delegate.
            _actFreshDvRowCount = i =>
            {
                dgView.RowCount = i;
            };
            //Reading finished delegate.
            _actReading = list =>
            {
                _listPath = list.Where(np => np.Path.EndsWith(@"\")).ToList();
                _listFile = list.Where(np => !np.Path.EndsWith(@"\")).ToList();
                FrmEverythingInputInfo info = Invoke(_funcGetInput) as FrmEverythingInputInfo;
                FreshSelected(info);
                lblReadingStatus.Text = @"文件整理完成";
                _readed = true;
                cbRegex.Enabled = true;
                cbContainPath.Enabled = true;
            };
            //Exit delegate.
            _actExit = i =>
            {
                MessageBox.Show(@"未能读取文件信息！");
                Close();
            };
            //Read File msgs background.
            new Thread(() =>
            {
                List<NamePath> listNamePath;
                if (DriveHelper.GetNamePathList(out listNamePath))
                {
                    this.Invoke(_actReading, new object[] { listNamePath });
                }
                else
                {
                    this.Invoke(_actExit, new object[] {0});
                }
            }).Start();

            _timerSelect = new System.Timers.Timer(500);
            _timerSelect.Elapsed += (sender, args) =>
            {
                if (!_readed) return;
                if (_selected) return;
                if ((DateTime.Now - _lastSearchInputTime).TotalSeconds < 0.8) return;

                FrmEverythingInputInfo info = Invoke(_funcGetInput) as FrmEverythingInputInfo;
                FreshSelected(info);
            };
            _timerSelect.Start();
        }

        private void FreshSelected(FrmEverythingInputInfo info)
        {
            if (_selecting) return;

            _selecting = true;
            string exp = info.ExpInput;
            bool containPath = info.ContainPath;


            Invoke(_actFreshDvRowCount, new object[] {0});

            if (string.IsNullOrWhiteSpace(exp))
            {
                _listSelected = new List<NamePath>();
                if (containPath) _listSelected.AddRange(_listPath);
                _listSelected.AddRange(_listFile);
            }
            else
            {
                _listSelected = new List<NamePath>();

                if (info.UseRegex)
                {
                    Regex regex = new Regex(exp);
                    if (containPath)
                    {
                        _listSelected.AddRange(_listPath.Where(np=> regex.IsMatch(np.Name)));
                    }
                    _listSelected.AddRange(_listFile.Where(np => regex.IsMatch(np.Name)));

                }
                else
                {
                    if (containPath)
                    {
                        _listSelected.AddRange(_listPath.Where(np => np.Name.ToUpper().Contains(exp.ToUpper())));
                    }
                    _listSelected.AddRange(_listFile.Where(np => np.Name.ToUpper().Contains(exp.ToUpper())));
                }
            }

            _selected = true;
            _selecting = false;
            Invoke(_actFreshDvRowCount, new object[] { _listSelected.Count });
        }


        private void tbInput_TextChanged(object sender, EventArgs e)
        {
            if (!_readed) return;

            _lastSearchInputTime = DateTime.Now;
            _selected = false;
        }

        private void dgView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            NamePath np = _listSelected[e.RowIndex];
            switch (e.ColumnIndex)
            {
                case 0:
                    e.Value = np.Name;
                    break;
                case 1:
                    e.Value = np.Path;
                    int index = np.Path.LastIndexOf(@"\");
                    e.Value = np.Path.Substring(0, index);
                    break;
                case 2:
                    if (np.Path.EndsWith(@"\"))
                    {
                        e.Value = string.Empty;
                        break;
                    }
                    if (!File.Exists(np.Path))
                    {
                        e.Value = string.Empty;
                        break;
                    }
                    e.Value = new FileInfo(np.Path).Length;
                    break;
                case 3:
                    if (np.Path.EndsWith(@"\"))
                    {
                        e.Value = string.Empty;
                        break;
                    }
                    if (!File.Exists(np.Path))
                    {
                        e.Value = string.Empty;
                        break;
                    }
                    e.Value = new FileInfo(np.Path).LastAccessTime;
                    break;
            }
        }

        private void cbContainPath_CheckedChanged(object sender, EventArgs e)
        {
            FrmEverythingInputInfo info = Invoke(_funcGetInput) as FrmEverythingInputInfo;
            FreshSelected(info);
        }

        private void dgView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!_readed || !_selected) return;

            dgView.CurrentCell = dgView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.Button == MouseButtons.Right && e.ColumnIndex == 0)
            {
                _cmsRowIndex = e.RowIndex;
                cmsFile.Show(this, this.PointToClient(Control.MousePosition));
            }
        }

        private void dgView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!_readed || !_selected) return;

            dgView.CurrentCell = dgView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (e.Button != MouseButtons.Left) return;
        }

        private void OpenOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_cmsRowIndex < 0 || _cmsRowIndex >= _listSelected.Count) return;

            NamePath np = _listSelected[_cmsRowIndex];
            if (np.Path.EndsWith(@"\")) OpenPathPToolStripMenuItem_Click(sender, e);
            else Process.Start(np.Path);
        }

        private void OpenPathPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_cmsRowIndex < 0 || _cmsRowIndex >= _listSelected.Count) return;

            NamePath np = _listSelected[_cmsRowIndex];
            if (np.Path.EndsWith(@"\"))
            {
                Process.Start(@"Explorer.exe", np.Path);
            }
            else
            {
                int length = np.Path.Length - np.Name.Length - 1;
                Process.Start(@"Explorer.exe", np.Path.Substring(0, length));
            }
        }

        private void CopyFileNameCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_cmsRowIndex < 0 || _cmsRowIndex >= _listSelected.Count) return;

            NamePath np = _listSelected[_cmsRowIndex];
            Clipboard.SetDataObject(np.Path);
        }

        private void cbRegex_CheckedChanged(object sender, EventArgs e)
        {
            cbContainPath_CheckedChanged(sender, e);
        }
    }
}
