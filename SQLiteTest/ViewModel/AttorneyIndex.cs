using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQLiteTest.ViewModel
{
    public class AttorneyIndex
    {
        string name;
        double firstVirsionWeight;//初稿权值
        List<TaskDetails> tdsFirstVirsions;//初稿详情列表
        double doneWeight;//递交权值
        List<TaskDetails> tdsDone;//递交详情列表
        DateTime? startDate;//计算区间起始日期
        DateTime? endDate;//计算区间结束日期
        int numofDoneOA;//递交OA的数量
        List<TaskDetails> tdsDoneOA;//递交OA详情列表
        int numofHandled;//经手新申请案件数量
        List<TaskDetails> tdsHandled;//经手新申请详情列表
        int numofExceedlimit;//超期案件数量
        List<TaskDetails> tdsExceedlimit;//经手新申请详情列表
        int numofExceedOutsidelimit;//超客户期限案件数量
        List<TaskDetails> tdsExceedOutsidelimit;//超客户期限详情列表
        double percentofExceed;
        double percentofExceedOutside;
        int daysofExceedlimit;//超期天数
        int daysofExceedOutsidelimit;//超期天数
        int numofEntrusted;//委托案件量
        List<TaskDetails> tdsEntrusted;//经手新申请详情列表

        public double FirstVirsionWeight { get => firstVirsionWeight; set => firstVirsionWeight = value; }
        public double DoneWeight { get => doneWeight; set => doneWeight = value; }
        public DateTime? StartDate { get => startDate; set => startDate = value; }
        public DateTime? EndDate { get => endDate; set => endDate = value; }
        public int NumofDoneOA { get => numofDoneOA; set => numofDoneOA = value; }
        public int NumofHandled { get => numofHandled; set => numofHandled = value; }
        public int NumofExceedlimit { get => numofExceedlimit; set => numofExceedlimit = value; }
        public int DaysofExceedlimit { get => daysofExceedlimit; set => daysofExceedlimit = value; }
        public string Name { get => name; set => name = value; }
        public double PercentofExceed { get => percentofExceed; set => percentofExceed = value; }
        public List<TaskDetails> TdsFirstVirsions { get => tdsFirstVirsions; set => tdsFirstVirsions = value; }
        public List<TaskDetails> TdsDone { get => tdsDone; set => tdsDone = value; }
        public List<TaskDetails> TdsDoneOA { get => tdsDoneOA; set => tdsDoneOA = value; }
        public List<TaskDetails> TdsHandled { get => tdsHandled; set => tdsHandled = value; }
        public List<TaskDetails> TdsExceedlimit { get => tdsExceedlimit; set => tdsExceedlimit = value; }
        public int NumofEntrusted { get => numofEntrusted; set => numofEntrusted = value; }
        public List<TaskDetails> TdsEntrusted { get => tdsEntrusted; set => tdsEntrusted = value; }
        public int NumofExceedOutsidelimit { get => numofExceedOutsidelimit; set => numofExceedOutsidelimit = value; }
        public List<TaskDetails> TdsExceedOutsidelimit { get => tdsExceedOutsidelimit; set => tdsExceedOutsidelimit = value; }
        public int DaysofExceedOutsidelimit { get => daysofExceedOutsidelimit; set => daysofExceedOutsidelimit = value; }
        public double PercentofExceedOutside { get => percentofExceedOutside; set => percentofExceedOutside = value; }

        public AttorneyIndex()
        {
            firstVirsionWeight = 0;
            doneWeight = 0;
            numofDoneOA = 0;
            numofHandled = 0;
            numofExceedlimit = 0;
            daysofExceedlimit = 0;
            numofEntrusted = 0;
            tdsFirstVirsions = new List<TaskDetails>();
            tdsDone = new List<TaskDetails>();
            tdsDoneOA = new List<TaskDetails>();
            tdsHandled = new List<TaskDetails>();
            tdsExceedlimit = new List<TaskDetails>();
            tdsEntrusted = new List<TaskDetails>();
        }
        public AttorneyIndex(DataSet ds, DateTime? startdate, DateTime? enddate)
        {

            startDate = startdate;
            endDate = enddate;
            firstVirsionWeight = 0;
            doneWeight = 0;
            numofDoneOA = 0;
            numofHandled = 0;
            numofExceedlimit = 0;
            numofExceedOutsidelimit = 0;
            daysofExceedlimit = 0;
            daysofExceedOutsidelimit = 0;
            numofEntrusted = 0;
            tdsFirstVirsions = new List<TaskDetails>();
            tdsDone = new List<TaskDetails>();
            tdsDoneOA= new List<TaskDetails>();
            tdsHandled = new List<TaskDetails>();
            tdsExceedlimit = new List<TaskDetails>();
            tdsExceedOutsidelimit = new List<TaskDetails>();
            tdsEntrusted = new List<TaskDetails>();

            //根据起始日期和结束日期计算区间段内的各种指标

            List<TaskDetails> tds = ConvertDStoDTLS(ds);

            foreach (TaskDetails td in tds)
            {
                //初稿权值累加
                if (td.FirstVirsionDate>=startDate && td.FirstVirsionDate<=endDate)
                {
                    firstVirsionWeight = firstVirsionWeight + td.Weight;
                    tdsFirstVirsions.Add(td);
                    if (td.TaskName == "OA答复")
                    {
                        numofDoneOA = numofDoneOA + 1;
                    }
                }
                //递交权值累加
                if (td.DoneDate >= startDate && td.DoneDate <= endDate && td.ProcessStage== "完成:完成")
                {
                    doneWeight = doneWeight + td.Weight;
                    tdsDone.Add(td);

                }
                //经手新申请数量累加
                if (td.TaskName=="新申请" && ((td.FirstVirsionDate >= startDate && td.FirstVirsionDate <= endDate)||(td.DoneDate >= startDate && td.DoneDate <= endDate)||td.DoneDate.ToString()==""))
                {
                    numofHandled = numofHandled + 1;
                    tdsHandled.Add(td);

                    //超期新申请数量及超期天数累加：完成了初稿，但记录超期
                    if ((td.FirstVirsionDeadlineOutside != null && td.FirstVirsionDate>td.FirstVirsionDeadlineOutside) || td.FirstVirsionDate > td.FirstVirsionDeadlineInternal)
                    {
                        numofExceedlimit = numofExceedlimit + 1;
                        if (td.FirstVirsionDeadlineOutside!=null)
                        {
                            daysofExceedlimit += (td.FirstVirsionDate - td.FirstVirsionDeadlineOutside).Value.Days;
                        }
                        else
                        {
                            daysofExceedlimit += (td.FirstVirsionDate - td.FirstVirsionDeadlineInternal).Value.Days;
                        }
                        tdsExceedlimit.Add(td);
                    }

                    //超期新申请数量累加：未完成初稿，但初稿期限已过。
                    if (td.FirstVirsionDate==null && td.ProcessStage!="不处理:不处理" && td.DoneDate==null && (td.FirstVirsionDeadlineOutside != null && td.FirstVirsionDeadlineOutside < DateTime.Now.Date || td.FirstVirsionDeadlineInternal < DateTime.Now.Date))
                    {
                        numofExceedlimit = numofExceedlimit + 1;
                        if (td.FirstVirsionDeadlineOutside != null)
                        {
                            daysofExceedlimit += (DateTime.Now.Date - td.FirstVirsionDeadlineOutside).Value.Days;
                        }
                        else
                        {
                            daysofExceedlimit += (DateTime.Now.Date - td.FirstVirsionDeadlineInternal).Value.Days;
                        }
                        tdsExceedlimit.Add(td);
                    }

                    //超客户期限的新申请
                    if (td.FirstVirsionDate == null && td.ProcessStage != "不处理:不处理" && td.DoneDate == null && td.FirstVirsionDeadlineOutside != null && td.FirstVirsionDeadlineOutside < DateTime.Now.Date )
                    {
                        numofExceedOutsidelimit = numofExceedOutsidelimit + 1;

                        daysofExceedOutsidelimit += (DateTime.Now.Date - td.FirstVirsionDeadlineOutside).Value.Days;

                        tdsExceedOutsidelimit.Add(td);
                    }
                }

                //委托案件累加
                if (td.EntrustDate >= startDate && td.EntrustDate <= endDate)
                {
                    numofEntrusted += 1;
                    tdsEntrusted.Add(td);
                }

            }

            PercentofExceed =(double) numofExceedlimit / numofHandled;
            PercentofExceedOutside = (double)numofExceedOutsidelimit / numofHandled;
            //PercentofExceed = Math.Round(PercentofExceed,2);

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
