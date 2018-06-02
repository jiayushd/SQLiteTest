using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQLiteTest.ViewModel
{
    public class AttorneyStatus
    {
        string name;
        int intCN;
        int intForeign;
        int intTodo;
        int intFirstVirsion;
        int intAllOA;
        int intOAin30;
        List<TaskDetails> tdsCN;//初稿详情列表
        List<TaskDetails> tdsForeign;//初稿详情列表
        List<TaskDetails> tdsFirstVirsions;//初稿详情列表
        List<TaskDetails> tdsTodo;//初稿详情列表
        List<TaskDetails> tdsOAin30;//初稿详情列表
        List<TaskDetails> tdsAllOA;//初稿详情列表
        //DataSet dsCN;
        //DataSet dsForeign;
        //DataSet dsTodo;
        //DataSet dsFirstVirsion;
        //DataSet dsAllOA;
        //DataSet dsOAin30;

        public int IntCN { get => intCN; set => intCN = value; }
        public int IntForeign { get => intForeign; set => intForeign = value; }
        public int IntTodo { get => intTodo; set => intTodo = value; }
        public int IntFirstVirsion { get => intFirstVirsion; set => intFirstVirsion = value; }
        public string Name { get => name; set => name = value; }
        public int IntAllOA { get => intAllOA; set => intAllOA = value; }
        public int IntOAin30 { get => intOAin30; set => intOAin30 = value; }
        public List<TaskDetails> TdsCN { get => tdsCN; set => tdsCN = value; }
        public List<TaskDetails> TdsForeign { get => tdsForeign; set => tdsForeign = value; }
        public List<TaskDetails> TdsFirstVirsions { get => tdsFirstVirsions; set => tdsFirstVirsions = value; }
        public List<TaskDetails> TdsTodo { get => tdsTodo; set => tdsTodo = value; }
        public List<TaskDetails> TdsOAin30 { get => tdsOAin30; set => tdsOAin30 = value; }
        public List<TaskDetails> TdsAllOA { get => tdsAllOA; set => tdsAllOA = value; }

        //public DataSet DsCN { get => dsCN; set => dsCN = value; }
        //public DataSet DsForeign { get => dsForeign; set => dsForeign = value; }
        //public DataSet DsTodo { get => dsTodo; set => dsTodo = value; }
        //public DataSet DsFirstVirsion { get => dsFirstVirsion; set => dsFirstVirsion = value; }
        //public DataSet DsAllOA { get => dsAllOA; set => dsAllOA = value; }
        //public DataSet DsOAin30 { get => dsOAin30; set => dsOAin30 = value; }

        public AttorneyStatus()
        {            
            intCN = 0;
            intForeign = 0;
            intTodo = 0;
            intFirstVirsion = 0;
            IntAllOA = 0;
            IntOAin30 = 0;
            TdsCN = new List<TaskDetails>();
            TdsForeign = new List<TaskDetails>();
            TdsFirstVirsions = new List<TaskDetails>();
            TdsTodo = new List<TaskDetails>();
            TdsOAin30 = new List<TaskDetails>();
            TdsAllOA = new List<TaskDetails>();
        }

        /// <summary>
        /// 从传入的ds中筛选出各个固定条件下的数量
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="name"></param>
        public AttorneyStatus(DataSet ds)
        {
            intCN = 0;
            intForeign = 0;
            intTodo = 0;
            intFirstVirsion = 0;
            IntAllOA = 0;
            IntOAin30 = 0;
            TdsCN = new List<TaskDetails>();
            TdsForeign = new List<TaskDetails>();
            TdsFirstVirsions = new List<TaskDetails>();
            TdsTodo = new List<TaskDetails>();
            TdsOAin30 = new List<TaskDetails>();
            TdsAllOA = new List<TaskDetails>();

            //根据起始日期和结束日期计算区间段内的各种指标

            List<TaskDetails> tds = ConvertDStoDTLS(ds);

            foreach (TaskDetails td in tds)
            {
                //国内未完结
                if ((td.TaskName=="新申请" ) && td.ProcessStage!= "完成:完成" && td.ProcessStage != "完成:结案" && td.TaskStatus != "代理人-不处理" && td.TaskStatus != "代理人-完成")
                {
                    intCN++;
                    tdsCN.Add(td);
                    //可处理
                    if (td.ProcessStage == "撰写:未处理" || td.ProcessStage == "撰写:撰写中")
                    {
                        intTodo++;
                        tdsTodo.Add(td);
                    }
                }
                //涉外未完结
                if ((td.TaskName == "PCT国际申请" || td.TaskName == "改写" || td.TaskName == "撰写" || td.TaskName == "翻译") && td.ProcessStage != "完成:完成" && td.ProcessStage != "完成:结案" && td.TaskStatus != "代理人-不处理" && td.TaskStatus != "代理人-完成")
                {
                    intForeign++;
                    tdsForeign.Add(td);
                    //可处理
                    if (td.ProcessStage == "撰写:未处理" || td.ProcessStage == "撰写:撰写中")
                    {
                        intTodo++;
                        tdsTodo.Add(td);
                    }
                }

                //初稿
                if ((td.TaskName == "新申请" || td.TaskName == "OA答复") && td.ProcessStage != "返稿:客户确认中" && td.ProcessStage != "撰写:客户长期未确认" && td.DoneDate.ToString()=="")
                {
                    intFirstVirsion++;
                    tdsFirstVirsions.Add(td);
                }
                //全部OA
                if ((td.TaskName == "OA答复" || td.TaskName == "驳回提复审（先请客户确认）" || td.TaskName == "答复补正") && td.ProcessStage != "完成:完成" && td.ProcessStage != "完成:结案" && td.ProcessStage != "不处理-不处理" && td.DoneDate.ToString() == "")
                {
                    intAllOA++;
                    tdsAllOA.Add(td);
                    //30天内OA
                    if ((td.OfficalDeadline-DateTime.Now.Date).Value.Days<=30)
                    {
                        intOAin30++;
                        tdsOAin30.Add(td);
                    }
                }

            }


            #region 旧算法
            //dsAllOA = ds.Clone();
            //dsCN = ds.Clone();
            //dsForeign = ds.Clone();
            //dsTodo = ds.Clone();
            //dsFirstVirsion = ds.Clone();
            //dsOAin30 = ds.Clone();

            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    string siftcondition = dr["任务名称"].ToString();
            //    if (dr["承办人"].ToString() == name)
            //    {
            //        switch (siftcondition)
            //        {
            //            case "新申请":                            
            //                switch (dr["代理人处理状态"].ToString())
            //                {
            //                    case "完成:完成":
            //                    case "完成:结案":
            //                    case "不处理:不处理":
            //                        break;
            //                    case "撰写:未处理":
            //                    case "撰写:撰写中":
            //                        intTodo++;
            //                        break;
            //                    default:
            //                        intCN++;
            //                        break;
            //                }
            //                break;
            //            case "改写":
            //            case "撰写":
            //                intForeign++;
            //                break;
            //            case "OA答复":
            //            case "驳回提复审（先请客户确认）":
            //            case "答复补正":
            //                switch (dr["代理人处理状态"].ToString())
            //                {
            //                    case "完成:完成":
            //                    case "完成:结案":
            //                    case "不处理:不处理":
            //                        break;
            //                    default:
            //                        IntAllOA++;
            //                        dsAllOA.Tables[0].Rows.Add(dr.ItemArray);
            //                        if (((DateTime)dr["官方期限"] - DateTime.Now.Date).Days<=30)
            //                        {
            //                            IntOAin30 ++;
            //                        }
            //                        break;
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //        if (dr["代理人处理状态"].ToString() == "返稿:客户确认中" || dr["代理人处理状态"].ToString() == "撰写:客户长期未确认")
            //        {
            //            intFirstVirsion++;
            //        }
            //    }
            //}

            #endregion
        }


        /// <summary>
        /// 把dataset转换为taskdetails
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<TaskDetails> ConvertDStoDTLS(DataSet ds)
                {
                    //新建摘要对象
                    List<TaskDetails> taskDetails = new List<TaskDetails>();
                    //把国内申请数据转换为摘要数据            
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TaskDetails dtls = new TaskDetails(dr);
                        taskDetails.Add(dtls);
                    }
                    return taskDetails;
                }

    }
}
