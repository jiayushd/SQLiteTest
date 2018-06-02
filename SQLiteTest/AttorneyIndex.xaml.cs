using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using SQLiteTest.ViewModel;

namespace SQLiteTest.SubWindows
{
    /// <summary>
    /// AttorneyIndex.xaml 的交互逻辑
    /// </summary>
    public partial class AttorneyIndex : Window
    {
        DataSet ds = new DataSet();
        //AttorneyStatus ats=new  AttorneyStatus();
        //ViewModel.AttorneyIndex ai;
        public AttorneyIndex()
        {
            InitializeComponent();
            
            dpStart.SelectedDate = DateTime.Now.Date.AddDays(-DateTime.Now.Date.Day + 1);
            dpEnd.SelectedDate = DateTime.Now.Date;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //DAL.TaskInfo taskInfos = new DAL.TaskInfo();
            //ds = taskInfos.GetAll();
            
            ////spStatus.DataContext = ats;

            //DAL.DepartmentMember members = new DAL.DepartmentMember();
            //DataSet dsmem = members.GetAll();
            ////MessageBox.Show(dsmem.Tables[0].Rows[0]["在职状态"].ToString());
            //List<MemberTree> department = new List<MemberTree> (){ GetAllMembers(dsmem) };
            //memberTree.ItemsSource = department;
            
        }


        #region 私有方法

        /// <summary>
        /// 将ds转换为案件详情taskdetails
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
        
        /// <summary>
        /// 获取部门所有成员，并以membertree的形式返回
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private MemberTree GetAllMembers(DataSet ds)
        {
            MemberTree Department = new MemberTree();
            Department.Name = "电学部";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["部门"].ToString()=="电子部" && dr["在职状态"].ToString()!="False")
                {
                //新建一个与datarow对应的子树
                MemberTree Company = new MemberTree();
                Company.Name = dr["分公司"].ToString();
                Company.Children = new List<MemberTree>();
                MemberTree Group = new MemberTree();
                Group.Name = dr["组别"].ToString();
                Group.Children = new List<MemberTree>();
                MemberTree Member = new MemberTree();
                Member.Name = dr["姓名"].ToString();

                Group.Children.Add(Member);
                Company.Children.Add(Group);

                //将子树合并到部门树中去
                Department.Add(Company);
                }
            }
            return Department;                
        }

        /// <summary>
        /// 获取某个节点上的所有叶子节点，也即所有的代理人
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        private List<MemberTree> GetAllLeafofOneNode(MemberTree mt)
        {
            List<MemberTree> leafs = new List<MemberTree>();

            if (mt.Children!=null)
            {
                foreach (MemberTree mtchild in mt.Children)
                {
                    foreach (MemberTree mtleaf in GetAllLeafofOneNode(mtchild))
                    {
                        leafs.Add(mtleaf);
                    }
                }

            }
            else
            {
                leafs.Add(mt);
            }

            return leafs;
        }

        /// <summary>
        /// 如果是叶子节点，则获取指标详情
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        private ViewModel.AttorneyIndex GetAttorneyIndexofOne(MemberTree mt)
        {
            DataRow[] drs = ds.Tables[0].Select("承办人 LIKE '" + mt.Name + "'");
            DataSet dstemp = new DataSet();
            dstemp = ds.Clone();
            foreach (DataRow dr in drs)
            {
                dstemp.Tables[0].Rows.Add(dr.ItemArray);
            }
            ViewModel.AttorneyIndex aii = new ViewModel.AttorneyIndex(dstemp, dpStart.SelectedDate, dpEnd.SelectedDate);
            aii.Name = mt.Name;
            aii.PercentofExceed= Math.Round(aii.PercentofExceed, 3);
            return aii;
        }

        private AttorneyStatus GetAttorneyStatusofOne(MemberTree mt)
        {
            DataRow[] drs = ds.Tables[0].Select("承办人 LIKE '" + mt.Name + "'");
            DataSet dstemp = new DataSet();
            dstemp = ds.Clone();
            foreach (DataRow dr in drs)
            {
                dstemp.Tables[0].Rows.Add(dr.ItemArray);
            }
            AttorneyStatus ats = new AttorneyStatus(dstemp);
            return ats;
        }


        #endregion


        /// <summary>
        /// 点击节点的处理逻辑：获取该节点下的所有人名，并生成指标数据填入【详情列表】中。
        /// 同时将汇总数据填入【指标详情区】
        /// 同时将案件情况填入【案件情况区】
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //生成指标数据填入【详情列表】中
            List<MemberTree> leafs = new List<MemberTree>();//创建叶子列表用于保存所有的叶子节点
            MemberTree mt = new MemberTree();//创建成员树对象，用于保存所选择的节点数实例
            List<ViewModel.AttorneyIndex> indices = new List<ViewModel.AttorneyIndex>();//创建指标详情列表，用于保存所选择的节点下的所有叶子节点的指标详情
            List<AttorneyStatus> atss = new List<AttorneyStatus>();//创建案件情况列表，用于保存所选择的节点下的所有叶子节点的案件情况

            mt = (MemberTree)memberTree.SelectedItem;
            leafs = GetAllLeafofOneNode(mt);

            foreach (MemberTree leaf in leafs)
            {
                indices.Add(GetAttorneyIndexofOne(leaf));
                atss.Add(GetAttorneyStatusofOne(leaf));
            }

            dgDetail.ItemsSource = indices;

            //将汇总数据填入【指标详情区】
            ViewModel.AttorneyIndex indextotal = new ViewModel.AttorneyIndex();
            foreach (ViewModel.AttorneyIndex index in indices)
            {
                indextotal.FirstVirsionWeight += index.FirstVirsionWeight;
                indextotal.DoneWeight += index.DoneWeight;
                indextotal.NumofDoneOA += index.NumofDoneOA;
                indextotal.NumofHandled += index.NumofHandled;
                indextotal.NumofExceedlimit += index.NumofExceedlimit;
                indextotal.NumofExceedOutsidelimit += index.NumofExceedOutsidelimit;
                indextotal.NumofEntrusted += index.NumofEntrusted;
                indextotal.DaysofExceedlimit += index.DaysofExceedlimit;
                indextotal.DaysofExceedOutsidelimit += index.DaysofExceedOutsidelimit;
                indextotal.TdsFirstVirsions = indextotal.TdsFirstVirsions.Concat(index.TdsFirstVirsions).ToList();
                indextotal.TdsDone = indextotal.TdsDone.Concat(index.TdsDone).ToList();
                indextotal.TdsDoneOA = indextotal.TdsDoneOA.Concat(index.TdsDoneOA).ToList();
                indextotal.TdsEntrusted = indextotal.TdsEntrusted.Concat(index.TdsEntrusted).ToList();
                indextotal.TdsExceedOutsidelimit = indextotal.TdsExceedlimit.Concat(index.TdsExceedOutsidelimit).ToList();
                indextotal.TdsExceedlimit = indextotal.TdsExceedlimit.Concat(index.TdsExceedlimit).ToList();

                indextotal.TdsHandled = indextotal.TdsHandled.Concat(index.TdsHandled).ToList();
            }
            indextotal.PercentofExceed = (double)indextotal.NumofExceedlimit / indextotal.NumofHandled;
            indextotal.PercentofExceedOutside = (double)indextotal.NumofExceedOutsidelimit / indextotal.NumofHandled;

            spIndex.DataContext = indextotal;

            //将汇总数据填入【案件情况区】
            AttorneyStatus atstotal = new AttorneyStatus();
            foreach (AttorneyStatus ats in atss)
            {
                atstotal.IntCN += ats.IntCN;
                atstotal.IntForeign += ats.IntForeign;
                atstotal.IntTodo += ats.IntTodo;
                atstotal.IntFirstVirsion += ats.IntFirstVirsion;
                atstotal.IntAllOA += ats.IntAllOA;
                atstotal.IntOAin30 += ats.IntOAin30;
                atstotal.TdsCN = atstotal.TdsCN.Concat(ats.TdsCN).ToList();
                atstotal.TdsForeign = atstotal.TdsForeign.Concat(ats.TdsForeign).ToList();
                atstotal.TdsFirstVirsions = atstotal.TdsFirstVirsions.Concat(ats.TdsFirstVirsions).ToList();
                atstotal.TdsTodo = atstotal.TdsTodo.Concat(ats.TdsTodo).ToList();
                atstotal.TdsAllOA = atstotal.TdsAllOA.Concat(ats.TdsAllOA).ToList();
                atstotal.TdsOAin30 = atstotal.TdsOAin30.Concat(ats.TdsOAin30).ToList();
            }

            spStatus.DataContext = atstotal;
        }



        #region 个人案件状态区各数字点击后展示相关的列表
        //点击所有OA数字后展示案件列表
        private void TxtblkAllOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsAllOA);
            tl.Show();
        }
        //点击国内数字后展示案件列表
        private void TxtblkCN_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsCN);
            tl.Show();
        }
        //点击涉外数字后展示案件列表
        private void TxtblkForeign_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsForeign);
            tl.Show();
        }
        //点击可处理数字后展示案件列表
        private void TxtblkTodo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsTodo);
            tl.Show();
        }
        //点击初稿数字后展示案件列表
        private void TxtblkFirstVirsion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsFirstVirsions);
            tl.Show();
        }
        //点击30天内OA数字后展示案件列表
        private void TxtblkOAin30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AttorneyStatus ats = (AttorneyStatus)spStatus.DataContext;
            TaskList tl = new TaskList(ats.TdsOAin30);
            tl.Show();
        }
        #endregion

        #region 指标详情区各数字点击后展示相关的列表
        //点击经手数量展示案件列表
        private void TxtblkNumofHandled_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsHandled);
            tl.Show();
        }
        //点击超期数量展示案件列表
        private void TxtblkNumofExceedlimit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsExceedlimit);
            tl.Show();
        }

        ////点击经手数量展示案件列表
        //private void TxtblkDaysofExceedlimit_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
        //    TaskList tl = new TaskList(ai.TdsExceedlimit);
        //    tl.Show();
        //}

        //点击初稿权值展示案件列表
        private void TxtblkFirstVirsionWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsFirstVirsions);
            tl.Show();
        }

        //点击完成OA数量展示案件列表
        private void TxtblkNumofDoneOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsDoneOA);
            tl.Show();
        }

        //点击开案数量展示案件列表
        private void TxtblkNumofEntrusted_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsEntrusted);
            tl.Show();
        }

        private void TxtblkDoneWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.AttorneyIndex ai = (ViewModel.AttorneyIndex)spIndex.DataContext;
            TaskList tl = new TaskList(ai.TdsDone);

            string title = "递交清单_" + txtblkMemberScope.Text+"_"+dpStart.Text+"_to_"+dpEnd.Text;
            tl.Title = title;
            tl.Show();
        }
    }

        #endregion


    
}
