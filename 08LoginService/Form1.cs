using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static _08LoginService.LoginModel;

namespace _08LoginService
{
    public partial class Form1 : Form
    {
        private LoginServiceCore _service = new LoginServiceCore();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string u = txtUserName.Text.Trim();
            string p = txtPwd.Text.Trim();
            if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                lblMsg.Text = "请输入完整的账号密码"; // Use Text property instead of Content
                lblMsg.ForeColor = System.Drawing.Color.Red; // Use ForeColor property instead of Foreground
                return;
            }
            // 1. 调用服务，拿到结果对象
            LoginResult result = _service.Login(u, p);

            // 2. 根据“状态码”决定界面怎么显示 (Switch case)
            switch (result.Status)
            {
                case LoginStatus.Success:
                    lblMsg.ForeColor = System.Drawing.Color.Green; // Corrected property
                    lblMsg.Text = "登录成功！正在跳转..."; // Corrected property
                                                  // TODO: 打开主窗口
                    break;

                case LoginStatus.Locked:
                    lblMsg.ForeColor = System.Drawing.Color.Red; // Corrected property
                    MessageBox.Show(result.Message, "安全警告"); // 弹窗警告
                    lblMsg.Text = result.Message; // Corrected property
                    break;

                case LoginStatus.PasswordIncorrect:
                    lblMsg.ForeColor = System.Drawing.Color.OrangeRed; // Corrected property
                    lblMsg.Text = result.Message; // Corrected property
                    txtPwd.Clear(); // 贴心地帮用户清空密码框
                    txtPwd.Focus();
                    break;

                case LoginStatus.UserNotFound:
                    lblMsg.ForeColor = System.Drawing.Color.Red; // Corrected property
                    lblMsg.Text = result.Message; // Corrected property
                    break;

                case LoginStatus.Error:
                    MessageBox.Show(result.Message); // 系统错误直接弹窗
                    break;
            }
        }
    }
}
