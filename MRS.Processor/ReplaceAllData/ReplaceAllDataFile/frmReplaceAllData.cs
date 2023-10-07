using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace deleteFile
{
    public partial class frmReplaceAllData : Form
    {
        public frmReplaceAllData()
        {
            InitializeComponent();
        }

        public void ProcessFolder(string strFolderName)
        {
            DirectoryInfo ThuMucNguonDir = new DirectoryInfo(strFolderName);
            if (Directory.Exists(strFolderName))
            {
                try
                {
                    ReplaceFile(ThuMucNguonDir);
                    ReplaceFolder(ThuMucNguonDir);
                    //DeleteDirectory(strFolderName);
                    MessageBox.Show("Thành công");
                }
                catch { }
            }
        }

        public void ReplaceFile(DirectoryInfo directoryInfo)
        {
            try
            {
                
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    if (file.Name.Contains(".dll")) continue;
                    StreamReader reader = new StreamReader(file.FullName);
                    string data = reader.ReadToEnd().Replace(txtOld.Text, txtNew.Text);
                    reader.Close();
                    File.WriteAllText(file.FullName, data, Encoding.UTF8);
                    if ((file.Name.Contains(txtOld.Text)))
                    {
                        string newname = file.Name.Replace(txtOld.Text, txtNew.Text);

                        File.Move(file.FullName, file.DirectoryName + "\\" + newname);

                    }
                    

                }
                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    ReplaceFile(subfolder);
                }
            }
            catch { }
        }
        public void ReplaceFolder(DirectoryInfo directoryInfo)
        {
            try
            {

                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {

                    if ((subfolder.Name.Contains(txtOld.Text)))
                    {
                        string newname = subfolder.Name.Replace(txtOld.Text, txtNew.Text);

                        Directory.Move(subfolder.FullName, subfolder.Parent.FullName + "\\" + newname);

                    }


                }
                foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
                {
                    ReplaceFolder(subfolder);
                }
            }
            catch { }
        }
        public static void DeleteDirectory(string dirPath)
        {
            try
            {
                // Nếu bạn không có quyền truy cập thư mục 'dirPath' 
                // một ngoại lệ UnauthorizedAccessException sẽ được ném ra.
                IEnumerable<string> enums = Directory.EnumerateDirectories(dirPath);


                List<string> dirs = new List<string>(enums);

                foreach (var dir in dirs)
                {

                    DeleteDirectory(dir);
                }
                DirectoryInfo ThuMucNguonDir = new DirectoryInfo(dirPath);
                if (ThuMucNguonDir.GetFiles().Count() == 0 && enums.Count() == 0)
                {
                    ThuMucNguonDir.Delete();
                }

            }
            // Lỗi bảo mật khi truy cập vào thư mục mà bạn không có quyền.
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Can not access directory: " + dirPath);
                Console.WriteLine(e.Message);
            }
        }

        private void txtDir_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                 if (e.KeyCode == Keys.Enter)
                {
                    txtOld.Focus();
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void txtOld_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtNew.Focus();
            }
        }

        private void txtNew_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProcessFolder(txtDir.Text);
            }
        }


    }
}
