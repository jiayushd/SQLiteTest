using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Data;

namespace SQLiteTest.ViewModel
{
    public class TaskAbstract
    {
        string taskID;
        string attorneySeries;
        string clientID;
        string caseDocumentName;
        string taskName;
        SolidColorBrush statusColor;
        int daysLeft;
        SolidColorBrush daysColor;
        string extraInfo;

        public string TaskID { get => taskID; set => taskID = value; }
        public string AttorneySeries { get => attorneySeries; set => attorneySeries = value; }
        public string ClientID { get => clientID; set => clientID = value; }
        public string CaseDocumentName { get => caseDocumentName; set => caseDocumentName = value; }
        public string TaskName { get => taskName; set => taskName = value; }
        public SolidColorBrush StatusColor { get => statusColor; set => statusColor = value; }
        public int DaysLeft { get => daysLeft; set => daysLeft = value; }
        public SolidColorBrush DaysColor { get => daysColor; set => daysColor = value; }
        public string ExtraInfo { get => extraInfo; set => extraInfo = value; }



        public TaskAbstract(DataRow dr)
        {
            try
            {
                taskID = dr["案件任务ID"].ToString();
                attorneySeries = dr["我方文号"].ToString();
                clientID = dr["客户名称"].ToString();
                caseDocumentName = dr["案件名称"].ToString();
                taskName = dr["申请类型"].ToString() + dr["任务名称"].ToString() + dr["任务属性"].ToString();
                extraInfo = dr["案件备注"].ToString();
                //确定状态表示颜色
                switch (dr["代理人处理状态"].ToString())
                {
                    case "完成:完成":
                        statusColor = new SolidColorBrush(Colors.Green);
                        break;
                    case "返稿:客户确认中":
                    case "撰写:客户长期未确认":
                        statusColor = new SolidColorBrush(Colors.CornflowerBlue);
                        break;
                    case "送件:递交中":
                        statusColor = new SolidColorBrush(Colors.LightGreen);
                        break;
                    default:
                        statusColor = new SolidColorBrush(Colors.OrangeRed);
                        break;
                }
                //确定剩余天数
                if (dr["任务名称"].ToString() == "OA答复" || dr["任务名称"].ToString() == "驳回提复审（先请客户确认）")
                {
                    daysLeft = ((DateTime)dr["官方期限"] - DateTime.Now.Date).Days;
                }
                else
                {
                    if (dr["初稿期限(内)"].ToString() != "")
                    {
                        daysLeft = ((DateTime)dr["初稿期限(内)"] - DateTime.Now.Date).Days;
                    }
                    else
                    {
                        daysLeft = -999;
                    }
                }
                //确定根据天数确定颜色
                if (daysLeft > 0)
                {
                    daysColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    daysColor = new SolidColorBrush(Colors.OrangeRed);
                }
            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }
}
