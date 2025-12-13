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

namespace _07PrivanceCitySelect
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            //加载数据库数据到程序里面
            string strConn = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;

            try
            {
                //创建连接对象
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {


                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        //打开数据库连接  晚开早闭
                        sqlConnection.Open();
                        sqlCommand.CommandText = @"SELECT [AreaId],[AreaName], [AreaPid] FROM [dbo].[Areafull] WHERE AreaPid = 0;";
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {

                            while (sqlDataReader.Read())
                            {
                                AreaInfo areaInfo = new AreaInfo();
                                areaInfo.AreaId = (int.Parse(sqlDataReader["AreaId"].ToString()));
                                areaInfo.AreaName = sqlDataReader["AreaName"].ToString();
                                areaInfo.AreaPid = (int.Parse(sqlDataReader["AreaPid"].ToString()));
                                // 把省的信息放到combox里面
                                this.cbxPrivance.Items.Add(areaInfo);
                            }

                            this.cbxPrivance.SelectedIndex = 0;



                        }
                    }
                }

            }
            finally
            {

            }

        }

        private void cbxPrivance_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaInfo privanceAreainfo = this.cbxPrivance.SelectedItem as AreaInfo;

            if (privanceAreainfo == null)
            {
                return;
            }
            //加载数据库数据到程序里面
            string strConn = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;

            try
            {
                //创建连接对象
                using (SqlConnection sqlConnection = new SqlConnection(strConn))
                {


                    using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                    {
                        //打开数据库连接  晚开早闭
                        sqlConnection.Open();
                        sqlCommand.CommandText = @"SELECT [AreaId],[AreaName], [AreaPid] FROM [dbo].[Areafull] WHERE AreaPid = " + privanceAreainfo.AreaId;
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            this.cbxCity.Items.Clear();

                            while (sqlDataReader.Read())
                            {

                                AreaInfo areaInfo = new AreaInfo();
                                areaInfo.AreaId = (int.Parse(sqlDataReader["AreaId"].ToString()));
                                areaInfo.AreaName = sqlDataReader["AreaName"].ToString();
                                areaInfo.AreaPid = (int.Parse(sqlDataReader["AreaPid"].ToString()));
                                // 把市的信息放到combox里面
                                this.cbxCity.Items.Add(areaInfo);
                            }

                            this.cbxCity.SelectedIndex = 0;



                        }


                    }


                }
            }
            finally
            {

            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string strConn = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;

            // 弹出保存文件对话框让用户选择保存位置
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Title = "请选择导出文件保存位置";
                saveFileDialog.Filter = "CSV文件 (*.csv)|*.csv|所有文件 (*.*)|*.*";
                saveFileDialog.FileName = "AreaExport.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (SqlConnection sqlConnection = new SqlConnection(strConn))
                    {
                        using (SqlCommand sqlCommand = sqlConnection.CreateCommand())
                        {
                            sqlConnection.Open();
                            sqlCommand.CommandText = "SELECT [AreaId],[AreaName], [AreaPid] FROM [dbo].[Areafull]";

                            using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                            {
                                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                                {
                                    // 写入表头
                                    writer.WriteLine("AreaId,AreaName,AreaPid");
                                    while (dataReader.Read())
                                    {
                                        string line = $"{dataReader["AreaId"]},{dataReader["AreaName"]},{dataReader["AreaPid"]}";
                                        writer.WriteLine(line);
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
