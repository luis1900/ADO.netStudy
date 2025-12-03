using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _04UserDemo
{
    public partial class UserSignup: Form
    {
        public UserSignup()
        {
            InitializeComponent();
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtUserName.Text.Trim()) || string.IsNullOrEmpty(txtPwd.Text.Trim()))
            {
                MessageBox.Show("用户名和密码不能为空");
                return;
            }

            string str = "server=.;uid=sa;pwd=123456;Database=OADB";
            //string str = ConnectionGetStringHelper.GetConnectionString(); 

            using (SqlConnection connection = new SqlConnection(str))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [dbo].[LOGIN]([LoginUserName], [LoginPassWord]) VALUES (@LoginUserName, @LoginPassWord)";
                        command.Parameters.AddWithValue("@LoginUserName", txtUserName.Text.Trim());
                        command.Parameters.AddWithValue("@LoginPassWord", txtPwd.Text.Trim());

                        int rows = command.ExecuteNonQuery();
                        if (rows >= 1)
                        {
                            MessageBox.Show("注册成功");
                        }
                        else
                        {
                            MessageBox.Show("fail");
                        }

                    }
                }
                catch (SqlException ex)
                {
                    // 记录异常日志
                    Console.WriteLine("数据库操作异常: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    
                }
            }
        }
    }
}
