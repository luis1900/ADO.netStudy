using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08LoginService
{
    public class LoginModel
    {
        // 1. 定义所有可能的登录结果（状态码）
        public enum LoginStatus
        {
            Success,            // 成功
            UserNotFound,       // 用户不存在
            PasswordIncorrect,  // 密码错误
            Locked,             // 被锁定
            Error               // 系统异常（数据库挂了等）
        }

        // 2. 定义一个对象，专门用来在 Service 和 UI 之间传话
        public class LoginResult
        {
            public LoginStatus Status { get; set; } // 结果状态
            public string Message { get; set; }     // 给用户看的具体提示（比如“还剩2次”）
        }
    }
}
