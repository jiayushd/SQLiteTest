using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQLiteTest.ViewModel
{
    public class TaskDetails:Model.TaskInfo
    {
        string attorneySeries;//我方案号
        string clientSeries;//客户案号
        string applicationID;//申请号
        DateTime? entrustDate;//委托日期
        string salesmanID;//案源人
        string clientID;//客户名称
        string techdocumentName;//开案名称
        string casedocumentName;//案件名称
        string extraInfo;//备注信息
        string appType;//申请类型
        string appStatus;//内部状态
        DateTime? applicationDate;//申请日

        public string AttorneySeries { get => attorneySeries; set => attorneySeries = value; }
        public string ClientSeries { get => clientSeries; set => clientSeries = value; }
        public string ApplicationID { get => applicationID; set => applicationID = value; }
        public DateTime? EntrustDate { get => entrustDate; set => entrustDate = value; }
        public string SalesmanID { get => salesmanID; set => salesmanID = value; }
        public string ClientID { get => clientID; set => clientID = value; }
        public string TechdocumentName { get => techdocumentName; set => techdocumentName = value; }
        public string CasedocumentName { get => casedocumentName; set => casedocumentName = value; }
        public string ExtraInfo { get => extraInfo; set => extraInfo = value; }
        public string AppType { get => appType; set => appType = value; }
        public string AppStatus { get => appStatus; set => appStatus = value; }
        public DateTime? ApplicationDate { get => applicationDate; set => applicationDate = value; }

        public TaskDetails(DataRow dr)
        {
            try
            {
                TaskID = dr["案件任务ID"].ToString();
                TaskName = dr["任务名称"].ToString();//例如新申请、OA答复
                TaskAttribute = dr["任务属性"].ToString();
                TaskCatogry = dr["任务标识"].ToString();
                Attorney = dr["承办人"].ToString();

                if (dr["配案日"].ToString() == "")
                {
                    DistributeDate = null;
                }
                else
                {
                    DistributeDate = (DateTime)dr["配案日"];
                }

                if (dr["初稿期限(内)"].ToString() == "")
                {
                    FirstVirsionDeadlineInternal = null;
                }
                else
                {
                    FirstVirsionDeadlineInternal = (DateTime)dr["初稿期限(内)"];
                }


                if (dr["初稿期限(外)"].ToString() == "")
                {
                    FirstVirsionDeadlineOutside = null;
                }
                else
                {
                    FirstVirsionDeadlineOutside = (DateTime)dr["初稿期限(外)"];
                }

                if (dr["初稿日"].ToString() == "")
                {
                    FirstVirsionDate = null;
                }
                else
                {
                    FirstVirsionDate = (DateTime)dr["初稿日"];
                }


                if (dr["定稿期限(内)"].ToString() == "")
                {
                    DoneDeadlineInternal = null;
                }
                else
                {
                    DoneDeadlineInternal = (DateTime)dr["定稿期限(内)"];
                }

                if (dr["定稿期限(外)"].ToString() == "")
                {
                    DoneDeadlineOutside = null;
                }
                else
                {
                    DoneDeadlineOutside = (DateTime)dr["定稿期限(外)"];
                }

                if (dr["官方期限"].ToString() == "")
                {
                    OfficalDeadline = null;
                }
                else
                {
                    OfficalDeadline = (DateTime)dr["官方期限"];
                }

                if (dr["完成日"].ToString() == "")
                {
                    DoneDate = null;
                }
                else
                {
                    DoneDate = (DateTime)dr["完成日"];
                }

                ProcessStage = dr["代理人处理状态"].ToString();
                TaskStatus = dr["任务状态"].ToString();
                CaseID = dr["Cases.案件ID"].ToString();
                AttorneySeries = dr["我方文号"].ToString();//我方案号
                ClientSeries = dr["客户文号"].ToString();//客户案号
                ApplicationID = dr["申请号"].ToString();//申请号
                EntrustDate = (DateTime)dr["委案日期"];//委托日期
                SalesmanID = dr["跟案人"].ToString();//案源人
                ClientID = dr["客户名称"].ToString();//客户名称
                TechdocumentName = dr["开案名称"].ToString();//开案名称
                CasedocumentName = dr["案件名称"].ToString();//案件名称
                ExtraInfo = dr["案件备注"].ToString();//备注信息
                AppType = dr["申请类型"].ToString();//申请类型，例如发明、实用新型
                AppStatus = dr["内部状态"].ToString();//内部状态

                if (dr["申请日"].ToString() == "")
                {
                    ApplicationDate = null;
                }
                else
                {
                    ApplicationDate = (DateTime)dr["申请日"];
                }

                switch (AppType + TaskName+ TaskAttribute+ TaskCatogry)
                {
                    case "发明新申请":
                    case "发明新申请撰写":
                        Weight = 1;
                        break;
                    case "驳回提复审（先请客户确认）处理过该案之前审查意见答复":                   
                        Weight = 0.8;
                        break;
                    case "实用新型新申请":
                    case "实用新型新申请撰写":
                        Weight = 0.7;
                        break;
                    case "发明OA答复一通实质(S)":
                        Weight = 0.4;
                        break;
                    case "实用新型OA答复一通实质(S)":
                    case "发明OA答复二通实质(S)":
                        Weight = 0.2;
                        break;
                    case "发明OA答复三通实质(S)":
                    case "实用新型OA答复一通形式-非本人失误(XN)":
                    case "实用新型OA答复二通形式-非本人失误(XN)":
                    case "实用新型OA答复三通形式-非本人失误(XN)":
                    case "发明OA答复二通形式-非本人失误(XN)":
                    case "发明OA答复一通形式-非本人失误(XN)":
                    case "发明OA答复三通形式-非本人失误(XN)":
                        Weight = 0.1;
                        break;
                    default:
                        Weight = 0;
                        break;
                }

            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
