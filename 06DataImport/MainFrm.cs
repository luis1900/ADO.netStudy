using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _06DataImport
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "文本文件|*.txt";

                // 只有当用户点击了“确定”后，才执行后面的操作
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1.哪怕用户选了文件，也先显示在文本框里
                    this.txtFile.Text = ofd.FileName;

                    // 2. 只有选中文件了，才调用导入方法
                    // 用一个变量 msg 接收 ImportData 返回的提示文字
                    string msg = ImportData(ofd.FileName);

                    // 3. 弹出提示框显示结果
                    MessageBox.Show(msg, "操作结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // 1. 把 void 改成 string，这样方法就能返回消息了
        private string ImportData(string fileName)
        {
            int successCount = 0; // 定义一个计数器
            string strCon = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

            // 添加 try-catch 用于捕获数据库连接错误
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(strCon))
                {
                    sqlConnection.Open();
                    string temp = string.Empty;
                    using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8))
                    {
                        reader.ReadLine(); // 跳过表头
                        while (!string.IsNullOrEmpty(temp = reader.ReadLine()))
                        {
                            // 兼容中文逗号，防止报错
                            var str = temp.Split(new char[] { ',', '，', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            // 如果数据不完整（少于2列），跳过这一行
                            if (str.Length < 2) continue;

                            using (SqlCommand cmd = sqlConnection.CreateCommand())
                            {
                                cmd.CommandText = "INSERT INTO [LOGIN] (LoginUserName, LoginPassWord) VALUES (@f1, @f2)";
                                cmd.Parameters.AddWithValue("@f1", str[0].Trim()); // 去空格
                                cmd.Parameters.AddWithValue("@f2", str[1].Trim()); // 去空格

                                // 执行插入
                                cmd.ExecuteNonQuery();

                                // 插入成功，计数器 +1
                                successCount++;
                            }
                        }
                    }
                }
                // 循环结束后，返回成功的消息
                return $"导入成功！共插入 {successCount} 条数据。";
            }
            catch (Exception ex)
            {
                // 如果出错，返回错误信息
                return $"导入失败：{ex.Message}";
            }
        }
    }
}
