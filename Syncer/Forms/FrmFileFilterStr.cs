using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace chenz
{
    public partial class FrmFileFilterStr : Form
    {
        public FrmFileFilterStr()
        {
            InitializeComponent();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            tbFilter.ForeColor = tbFilter.Text.Equals("*") ? Color.LightGray : Color.Black;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (0 == String.CompareOrdinal(tbFilter.Text, "*"))
            {
                tbFilter.Text = string.Empty;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
