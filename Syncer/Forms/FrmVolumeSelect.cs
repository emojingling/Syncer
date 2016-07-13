using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace chenz
{
    public partial class FrmVolumeSelect : Form
    {
        private DriveInfo _selectedDriveInfo = null;

        private DriveInfo[] _volumes;

        public DriveInfo SelectedDriveInfo
        {
            get { return _selectedDriveInfo; }
            private set { _selectedDriveInfo = value; }
        }

        public FrmVolumeSelect()
        {
            InitializeComponent();

            lbVolumes.Items.Clear();
            _volumes = DriveInfo.GetDrives();
            string strNTFS = @"ntfs";
            foreach (DriveInfo di in _volumes)
            {
                if (di.IsReady && 0 == String.Compare(di.DriveFormat, strNTFS, StringComparison.OrdinalIgnoreCase))
                {
                    lbVolumes.Items.Add(di.Name);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            if (lbVolumes.SelectedItem != null)
            {
                string name = lbVolumes.SelectedItem.ToString();
                foreach (var di in _volumes)
                {
                    if (di.Name.Equals(name))
                    {
                        SelectedDriveInfo = di;
                        break;
                    }
                }

                if (SelectedDriveInfo != null) DialogResult = System.Windows.Forms.DialogResult.OK;
                else DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void lbVolumes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbVolumes.SelectedItem == null) return;

            HandleSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
