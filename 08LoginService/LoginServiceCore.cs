using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _08LoginService.LoginModel;

namespace _08LoginService
{
    public class LoginServiceCore
    {
        string strCon = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        public LoginModel.LoginResult Login(string username, string password)
        {
            using (SqlConnection sqlConnection = new SqlConnection(strCon))
            {
                try
                {
                    string sql = "SELECT Id, Password, ErrorTimes, LastErrorDateTime FROM UserInfo WHERE UserName = @u";
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@u", username);

                        // 只有在马上要执行 Execute 的时候，才打开连接
                        sqlConnection.Open();
                        using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                        {
                            // 第一种情况:用户不存在
                            if (!sqlReader.Read())
                            {
                                return new LoginResult { Status = LoginStatus.UserNotFound, Message = "用户名不存在" };
                            }
                            // 读取数据
                            int UserId = (int)sqlReader["Id"];
                            string dbPwd = sqlReader["Password"].ToString();
                            int errTime = sqlReader["ErrorTimes"] == DBNull.Value ? 0 : (int)sqlReader["ErrorTimes"];
                            DateTime? lastErrTime = sqlReader["LastErrorDateTime"] as DateTime?;

                            sqlReader.Close();

                            //第二种情况检查锁定
                            if (errTime >= 3 && lastErrTime != null)
                            {
                                TimeSpan timePassed = DateTime.Now - lastErrTime.Value;
                                if (timePassed.TotalMinutes < 15)
                                {
                                    int waitTime = 15 - (int)timePassed.TotalMinutes;
                                    return new LoginResult
                                    {
                                        Status = LoginStatus.Locked,
                                        Message = $"账号已锁定，请 {waitTime} 分钟后再试"
                                    }; 
                                }
                                else
                                {
                                    // 超过15分钟，内存中逻辑解锁
                                    errTime = 0;
                                }
                            }
                       
                            // 第三种情况 校验密码
                            if(dbPwd == password)
                            {
                                // 密码正确 -> 清零错误次数
                                UpdateState(sqlConnection, UserId, 0, null);
                                return new LoginResult { Status = LoginStatus.Success, Message = "登录成功" };
                            }
                            else
                            {
                                int newTimes = errTime + 1;
                                UpdateState(sqlConnection, UserId, newTimes, DateTime.Now);
                                if (newTimes >= 3)
                                {
                                    return new LoginResult { Status = LoginStatus.Locked, Message = "密码错误3次，账号已锁定15分钟" };
                                }
                                else
                                {
                                    return new LoginResult
                                    {
                                        Status = LoginStatus.PasswordIncorrect,
                                        Message = $"密码错误，还剩 {3 - newTimes} 次机会"
                                    };
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    return new LoginResult { Status = LoginStatus.Error, Message = ex.Message };
                }
            }
        }
        private void UpdateState(SqlConnection sqlConnection, int uid, int times, DateTime? dt)
        {
            string sql = "UPDATE UserInfo SET ErrorTimes = @t, LastErrorDateTime = @d WHERE Id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
            {
                cmd.Parameters.AddWithValue("@t", times);
                cmd.Parameters.AddWithValue("@d", dt ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", uid);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
