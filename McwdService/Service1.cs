using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Collections;
using System.Configuration.Install;
using System.Configuration;
using log4net;

namespace McwdService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new System.Timers.Timer();
            timer1.Interval = 1000 * 60 * 60;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
            timer1.Enabled = true;
            try
            {
                DataSet dsd = new DataSet();
                DataSet dtd = new DataSet();
                dsd = Get_silo_temp();
                dtd = Get_tank_temp();
                decimal[] tt1 = new decimal[36];
                decimal[] tt = new decimal[36];
                if (dsd.Tables[0].Rows.Count == 36)
                {
                    for (int i = 0; i < 36; i++)
                    {
                        tt[i] = Convert.ToDecimal(String.IsNullOrEmpty(dsd.Tables[0].Rows[i]["temp"].ToString()) ? "0" : dsd.Tables[0].Rows[i]["temp"].ToString());
                    }
                }
                else
                {
                    for (int i = 0; i < dsd.Tables[0].Rows.Count; i++)
                    {
                        tt[i] = Convert.ToDecimal(String.IsNullOrEmpty(dsd.Tables[0].Rows[i]["temp"].ToString()) ? "0" : dsd.Tables[0].Rows[i]["temp"].ToString());
                    }
                    for (int k = dsd.Tables[0].Rows.Count; k < 36; k++)
                    {
                        tt[k] = 0;
                    }

                }
                if (dtd.Tables[0].Rows.Count == 36)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        tt1[j] = Convert.ToDecimal(String.IsNullOrEmpty(dtd.Tables[0].Rows[j]["temp"].ToString()) ? "0" : dtd.Tables[0].Rows[j]["temp"].ToString());
                    }
                }
                else
                {
                    for (int j = 0; j < dtd.Tables[0].Rows.Count; j++)
                    {
                        tt1[j] = Convert.ToDecimal(String.IsNullOrEmpty(dtd.Tables[0].Rows[j]["temp"].ToString()) ? "0" : dtd.Tables[0].Rows[j]["temp"].ToString());
                    }
                    for (int s = dtd.Tables[0].Rows.Count; s < 36; s++)
                    {
                        tt1[s] = 0;
                    }
                }
                string nicai = DateTime.Now.ToString("yyyyMMdd") + "安全监测数据";
                if (!Exists(nicai))
                {
                    Addcw(tt, tt1);
                }
                else
                {
                    log.writelog("今日数据已上传");
                }
            }
            catch (Exception ee)
            {
                log.writelog(ee.ToString());
                return;
            }
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DataSet dsd = new DataSet();
                DataSet dtd = new DataSet();
                dsd = Get_silo_temp();
                dtd = Get_tank_temp();
                decimal[] tt1 = new decimal[36];
                decimal[] tt = new decimal[36];
                if (dsd.Tables[0].Rows.Count == 36)
                {
                    for (int i = 0; i < 36; i++)
                    {
                        tt[i] = Convert.ToDecimal(String.IsNullOrEmpty(dsd.Tables[0].Rows[i]["temp"].ToString()) ? "0" : dsd.Tables[0].Rows[i]["temp"].ToString());
                    }
                }
                else
                {
                    for (int i = 0; i < dsd.Tables[0].Rows.Count; i++)
                    {
                        tt[i] = Convert.ToDecimal(String.IsNullOrEmpty(dsd.Tables[0].Rows[i]["temp"].ToString()) ? "0" : dsd.Tables[0].Rows[i]["temp"].ToString());
                    }
                    for (int k = dsd.Tables[0].Rows.Count; k < 36; k++)
                    {
                        tt[k] = 0;
                    }

                }
                if (dtd.Tables[0].Rows.Count == 36)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        tt1[j] = Convert.ToDecimal(String.IsNullOrEmpty(dtd.Tables[0].Rows[j]["temp"].ToString()) ? "0" : dtd.Tables[0].Rows[j]["temp"].ToString());
                    }
                }
                else
                {
                    for (int j = 0; j < dtd.Tables[0].Rows.Count; j++)
                    {
                        tt1[j] = Convert.ToDecimal(String.IsNullOrEmpty(dtd.Tables[0].Rows[j]["temp"].ToString()) ? "0" : dtd.Tables[0].Rows[j]["temp"].ToString());
                    }
                    for (int s = dtd.Tables[0].Rows.Count; s < 36; s++)
                    {
                        tt1[s] = 0;
                    }
                }
                string nicai = DateTime.Now.ToString("yyyyMMdd") + "安全监测数据";
                if (!Exists(nicai))
                {
                    Addcw(tt, tt1);
                }
                else
                {
                    Upcw(tt, tt1, nicai);
                }
            }
            catch (Exception ee)
            {
                log.writelog(ee.ToString());
                return;
            }
        }

        #region 查询红外监测表
        private DataSet Get_silo_temp()
        {
            StringBuilder str = new StringBuilder();
            DateTime today = DateTime.Now;
            string st = today.ToString("yyyy-MM-dd");
            str.Append(" select temp from t_silo_abnormal_temp   ");
            str.Append(" where  DATE(time)='" + st + "' order by time desc limit 36; ");             //"+st+"
            return DBHelperMySQL.Query(str.ToString());
        }
        #endregion

        #region  查询罐壁监测
        private DataSet Get_tank_temp()
        {
            StringBuilder str = new StringBuilder();
            DateTime today = DateTime.Now;
            string st = today.ToString("yyyy-MM-dd");
            str.Append(" select temp from t_tankwalls_history_temp   ");
            str.Append(" where  DATE(time)='" + st + "' order by time desc limit 36; ");              //+ st +
            return DBHelperMySQL.Query(str.ToString());
        }
        #endregion

        #region 检验是否重复上传
        private bool Exists(string beiz)
        {
            StringBuilder str = new StringBuilder();
            str.Append(" select count(1) from qy_meicwdb where beizhu='" + beiz + "'");
            return DBHelperOra.Exists(str.ToString());
        }
        #endregion

        #region 向测温表中同步数据
        private bool Addcw(decimal[] stemp, decimal[] wtemp)
        {
            string oid = Guid.NewGuid().ToString("N");
            DateTime td = DateTime.Now;
            string ts = td.ToString("yyyyMMdd");
            string bz = ts + "安全监测数据";
            StringBuilder str = new StringBuilder();
            str.Append(" insert into qy_meicwdb(");
            str.Append(" qy_meicwdboid,beizhu,riq,");
            for (int aa = 1; aa < 7; aa++)
            {
                str.Append(" A" + aa + "A,");
            }
            for (int ba = 1; ba < 7; ba++)
            {
                str.Append("B" + ba + "A,");
            }
            for (int ca = 1; ca < 7; ca++)
            {
                str.Append("C" + ca + "A,");
            }
            for (int da = 1; da < 7; da++)
            {
                str.Append("D" + da + "A,");
            }
            for (int ea = 1; ea < 7; ea++)
            {
                str.Append("E" + ea + "A,");
            }
            for (int fa = 1; fa < 7; fa++)
            {
                str.Append("F" + fa + "A,");
            }
            for (int ab = 1; ab < 7; ab++)
            {
                str.Append("A" + ab + "B,");
            }
            for (int bb = 1; bb < 7; bb++)
            {
                str.Append("B" + bb + "B,");
            }
            for (int cb = 1; cb < 7; cb++)
            {
                str.Append("C" + cb + "B,");
            }
            for (int db = 1; db < 7; db++)
            {
                str.Append("D" + db + "B,");
            }
            for (int eb = 1; eb < 7; eb++)
            {
                str.Append("E" + eb + "B,");
            }
            for (int fb = 1; fb < 7; fb++)
            {
                str.Append("F" + fb + "B,");
            }
            str.Append(" flag)  ");
            str.Append(" values ");
            str.Append("( '" + oid + "','" + bz + "',sysdate,");
            for (int s = 0; s < 36; s++)
            {
                str.Append(" '" + stemp[s] + "', ");
            }
            for (int w = 0; w < 36; w++)
            {
                str.Append(" '" + wtemp[w] + "', ");
            }
            str.Append(" '1' ) ");
            bool res = DBHelperOra.ExecuteSql(str.ToString()) > 0;
            if (res)
            {
                log.writelog("数据上传成功");
            }
            return res;
        }
        #endregion

        #region 更新测温表数据
        private bool Upcw(decimal[] stemp, decimal[] wtemp, string bz)
        {
            StringBuilder str = new StringBuilder();
            str.Append("  update QY_MEICWDB set ");
            str.Append("  riq=sysdate,");
            str.Append(" A1A='" + stemp[0] + "',");
            str.Append(" A2A='" + stemp[1] + "',");
            str.Append(" A3A='" + stemp[2] + "',");
            str.Append(" A4A='" + stemp[3] + "',");
            str.Append(" A5A='" + stemp[4] + "',");
            str.Append(" A6A='" + stemp[5] + "',");
            str.Append(" B1A='" + stemp[6] + "',");
            str.Append(" B2A='" + stemp[7] + "',");
            str.Append(" B3A='" + stemp[8] + "',");
            str.Append(" B4A='" + stemp[9] + "',");
            str.Append(" B5A='" + stemp[10] + "',");
            str.Append(" B6A='" + stemp[11] + "',");
            str.Append(" C1A='" + stemp[12] + "',");
            str.Append(" C2A='" + stemp[13] + "',");
            str.Append(" C3A='" + stemp[14] + "',");
            str.Append(" C4A='" + stemp[15] + "',");
            str.Append(" C5A='" + stemp[16] + "',");
            str.Append(" C6A='" + stemp[17] + "',");
            str.Append(" D1A='" + stemp[18] + "',");
            str.Append(" D2A='" + stemp[19] + "',");
            str.Append(" D3A='" + stemp[20] + "',");
            str.Append(" D4A='" + stemp[21] + "',");
            str.Append(" D5A='" + stemp[22] + "',");
            str.Append(" D6A='" + stemp[23] + "',");
            str.Append(" E1A='" + stemp[24] + "',");
            str.Append(" E2A='" + stemp[25] + "',");
            str.Append(" E3A='" + stemp[26] + "',");
            str.Append(" E4A='" + stemp[27] + "',");
            str.Append(" E5A='" + stemp[28] + "',");
            str.Append(" E6A='" + stemp[29] + "',");
            str.Append(" F1A='" + stemp[30] + "',");
            str.Append(" F2A='" + stemp[31] + "',");
            str.Append(" F3A='" + stemp[32] + "',");
            str.Append(" F4A='" + stemp[33] + "',");
            str.Append(" F5A='" + stemp[34] + "',");
            str.Append(" F6A='" + stemp[35] + "',");
            str.Append(" A1B='" + wtemp[0] + "',");
            str.Append(" A2B='" + wtemp[1] + "',");
            str.Append(" A3B='" + wtemp[2] + "',");
            str.Append(" A4B='" + wtemp[3] + "',");
            str.Append(" A5B='" + wtemp[4] + "',");
            str.Append(" A6B='" + wtemp[5] + "',");
            str.Append(" B1B='" + wtemp[6] + "',");
            str.Append(" B2B='" + wtemp[7] + "',");
            str.Append(" B3B='" + wtemp[8] + "',");
            str.Append(" B4B='" + wtemp[9] + "',");
            str.Append(" B5B='" + wtemp[10] + "',");
            str.Append(" B6B='" + wtemp[11] + "',");
            str.Append(" C1B='" + wtemp[12] + "',");
            str.Append(" C2B='" + wtemp[13] + "',");
            str.Append(" C3B='" + wtemp[14] + "',");
            str.Append(" C4B='" + wtemp[15] + "',");
            str.Append(" C5B='" + wtemp[16] + "',");
            str.Append(" C6B='" + wtemp[17] + "',");
            str.Append(" D1B='" + wtemp[18] + "',");
            str.Append(" D2B='" + wtemp[19] + "',");
            str.Append(" D3B='" + wtemp[20] + "',");
            str.Append(" D4B='" + wtemp[21] + "',");
            str.Append(" D5B='" + wtemp[22] + "',");
            str.Append(" D6B='" + wtemp[23] + "',");
            str.Append(" E1B='" + wtemp[24] + "',");
            str.Append(" E2B='" + wtemp[25] + "',");
            str.Append(" E3B='" + wtemp[26] + "',");
            str.Append(" E4B='" + wtemp[27] + "',");
            str.Append(" E5B='" + wtemp[28] + "',");
            str.Append(" E6B='" + wtemp[29] + "',");
            str.Append(" F1B='" + wtemp[30] + "',");
            str.Append(" F2B='" + wtemp[31] + "',");
            str.Append(" F3B='" + wtemp[32] + "',");
            str.Append(" F4B='" + wtemp[33] + "',");
            str.Append(" F5B='" + wtemp[34] + "',");
            str.Append(" F6B='" + wtemp[35] + "',");
            str.Append("  flag='1' ");
            str.Append(" where beizhu='" + bz + "' ");
            bool res = DBHelperOra.ExecuteSql(str.ToString()) > 0;
            if (res)
            {
                log.writelog("数据更新成功");
            }
            return res;
        }
        #endregion
    }
}
