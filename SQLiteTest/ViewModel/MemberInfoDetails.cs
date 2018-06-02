using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace SQLiteTest.ViewModel
{
    public class MemberInfoDetails
    {
        public string Account { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string SubCompany { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public bool IsOnJob { get; set; }
        public DateTime? ProfessionStartDate { get; set; }
        public DateTime? CompanyEntryDate { get; set; }
        public string WorkingExperience { get; set; }

        public MemberInfoDetails()
        {
            Account = "";
            Name = "";
            PhoneNumber = "";
            Email = "";
            SubCompany = "";
            Department = "";
            Group = "";
            IsOnJob =true;
            ProfessionStartDate = null;
            CompanyEntryDate = null;
            WorkingExperience = "";
        }

        /// <summary>
        /// 将输入的datarow转换成人员信息详情
        /// </summary>
        /// <param name="dr"></param>
        public MemberInfoDetails(DataRow dr)
        {
            try
            {
                Account = dr["系统账号"].ToString();
                Name = dr["姓名"].ToString();
                PhoneNumber = dr["电话"].ToString();
                Email = dr["邮箱"].ToString();
                SubCompany = dr["分公司"].ToString();
                Department = dr["部门"].ToString();
                Group = dr["组别"].ToString();
                IsOnJob = (bool)dr["在职状态"];
                if (dr["入行时间"].ToString()!="")
                {
                    ProfessionStartDate = (DateTime?)dr["入行时间"];
                }
                if (dr["入职时间"].ToString() != "")
                {
                     CompanyEntryDate = (DateTime?)dr["入职时间"];
                }
                WorkingExperience = dr["职业经历"].ToString();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
