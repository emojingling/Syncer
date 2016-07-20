using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace chenz
{
    public partial class FileInfoBox : UserControl
    {
        public FileInfoBox(int num)
        {
            InitializeComponent();

            Num = num;
        }

        [Category("自定义"), Description("序号")]
        public int Num
        {
            get { return Convert.ToInt32(lblNum.Text); }
            set { lblNum.Text = value.ToString(); }
        }

        [Category("自定义"), Description("文件名")]
        public string FileName
        {
            get { return tbFileName.Text; }
            set { tbFileName.Text = value; }
        }

        [Category("自定义"), Description("文件路径")]
        public string FilePath
        {
            get { return tbSelectPath.Text; }
            set { tbSelectPath.Text = value; }
        }

        public string FullPath
        {
            get { return FilePath + "\\" + FileName; }
            set
            {
                int index = value.LastIndexOf('\\');
                FilePath = value.Substring(0, index);
                int lengthFileName = value.Length - index - 1;
                FileName = value.Substring(index + 1, lengthFileName);
            }
        }

        [Category("自定义事件"), Description("点击选择路径按钮时发生")]
        public EventHandler ButtonSelectPathClick;

        [Category("自定义事件"), Description("点击添加按钮时发生")]
        public EventHandler ButtonAddClick;

        [Category("自定义事件"), Description("点击删除按钮时发生")]
        public EventHandler ButtonDeleteClick;

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            if (ButtonSelectPathClick != null) ButtonSelectPathClick(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ButtonAddClick != null) ButtonAddClick(sender, e);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ButtonDeleteClick != null) ButtonDeleteClick(sender, e);
        }

        public void Clear()
        {
            FileName = string.Empty;
            FilePath = string.Empty;
        }
    }
}
