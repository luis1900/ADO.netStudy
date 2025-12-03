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
    public partial class UserLoginFrm: Form
    {
        public UserLoginFrm()
        {
            InitializeComponent();
        }

        private void btbLogin_Click(object sender, EventArgs e)
        {
            //第一步 先进行校验数据
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) || string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MessageBox.Show("请输入正确的用户名和密码");
                return;
            }
            //连接数据库 做查询
            string str = "server=.;uid=sa;pwd=123456;Database=OADB";
            //创建一个连接对象
            using(SqlConnection conn = new SqlConnection(str))
            {
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();


                    // 这一行存在SQL注入风险，并且字段名txtPassword应为LoginPassword（假设表结构如此）
                    // string sql = string.Format("SELECT COUNT(1) FROM LOGIN WHERE LoginUserName ='{0}' and txtPassword = '{1}'",txtUsername.Text,txtPassword.Text);

                    // 推荐使用参数化查询，避免SQL注入，并修正字段名
                    cmd.CommandText = "SELECT COUNT(1) FROM LOGIN WHERE LoginUserName = @username AND LoginPassword = @password";
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
                    //string sql = string.Format("SELECT COUNT(1) FROM LOGIN WHERE LoginUserName ='{0}' and txtPassword = '{1}'",txtUsername.Text,txtPassword.Text);

                    // cmd的职责就是执行脚本
                    object result = cmd.ExecuteScalar();
                    int rows = int.Parse(result.ToString());
                    if(rows >= 1)
                    {
                        MessageBox.Show("登录成功");
                    }
                    else 
                    {
                        MessageBox.Show("fail");
                    }


                }
            }


        }
    }
}
