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
using System.Windows.Forms;

namespace chenz
{
    public partial class FrmEverything : Form
    {
        private List<NamePath> _listNamePath;




        public FrmEverything()
        {
            InitializeComponent();

            if (DriveHelper.GetNamePathList(out _listNamePath))
            {
                dgView.DataSource = _listNamePath;
            }
            else
            {
                MessageBox.Show("未能读取文件信息！");
                Close();
            }
        }

        

        


        


    }
}
