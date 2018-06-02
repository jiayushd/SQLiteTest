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
using SQLiteTest.SubWindows;
using SQLiteTest.ViewModel;

namespace SQLiteTest.SubWindows
{
    /// <summary>
    /// AttorneyIndex.xaml 的交互逻辑
    /// </summary>
    public partial class WinAttorneyIndex : Window
    {
        List<Task> tasks = new List<Task>();
        public WinAttorneyIndex()
        {
            InitializeComponent();

            dpStart.SelectedDate = DateTime.Now.Date.AddDays(-DateTime.Now.Date.Day + 1);
            dpEnd.SelectedDate = DateTime.Now.Date;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<MemberTree> department = new List<MemberTree>() { GetAllActiveMembers("电学部") };
            memberTree.ItemsSource = department;

        }

        /// <summary>
        /// 获取部门所有在职成员，并以membertree的形式返回
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private MemberTree GetAllActiveMembers(string department)
        {
            MemberTree Department = new MemberTree();
            Department.Name = department;
            Department.NodeType = "专业部";
            Department.Department = department;

            using (mainEntities db = new mainEntities())
            {
                List<staff> staffs = new List<staff>();
                staffs = db.staffs.Where(x => x.部门 == department && x.在职状态 != 0).ToList<staff>(); ;
                foreach (var item in staffs)
                {
                    //新建一个与datarow对应的子树
                    MemberTree Company = new MemberTree();
                    Company.Name = item.分公司;
                    Company.NodeType = "分公司";
                    Company.Department = department;
                    Company.Company = item.分公司;
                    Company.Children = new List<MemberTree>();
                    MemberTree Group = new MemberTree();
                    Group.Name = item.组别;
                    Group.NodeType = "组";
                    Group.Department = department;
                    Group.Company = item.分公司;
                    Group.Group = item.组别;
                    Group.Children = new List<MemberTree>();
                    MemberTree Member = new MemberTree();
                    //Member.Name = dr["姓名"].ToString()+"("+dr["系统账号"].ToString()+")";
                    Member.Name = item.Name;
                    Member.NodeType = "人员";
                    Member.Department = department;
                    Member.Company = item.分公司;
                    Member.Group = item.组别;

                    Member.Account = item.Account;

                    Group.Children.Add(Member);
                    Company.Children.Add(Group);

                    //将子树合并到部门树中去
                    Department.Add(Company);

                }

            }

            return Department;
        }

        private void memberTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MemberTree tvi = (MemberTree)memberTree.SelectedItem;
            List<staff> staffs = new List<staff>();
            switch (tvi.NodeType)
            {
                case "专业部":
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.部门 == tvi.Name).ToList<staff>();
                    }
                    break;
                case "分公司":
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.部门 == tvi.Department && x.分公司 == tvi.Company).ToList<staff>();
                    }
                    break;
                case "组":
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.部门 == tvi.Department && x.分公司 == tvi.Company && x.组别 == tvi.Name).ToList<staff>();
                    }
                    break;
                default:
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.Name == tvi.Name).ToList<staff>();
                    }
                    break;
            }

            using (mainEntities db=new mainEntities())
            {
                foreach (var staff in staffs)
                {
                    List<Task> taskstemp = db.Tasks.Where(x=>x.Attorney==staff.Name).ToList<Task>();
                    tasks = tasks.Concat(taskstemp).ToList();
                }

            }
        }

        private void txtblkCN_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkForeign_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkTodo_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkFirstVirsion_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkAllOA_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkOAin30_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNumofHandled_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNumofExceedlimit_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNumofExceedOutsidelimit_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkFirstVirsionWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkDoneWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNumofDoneOA_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkCheckWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkCheckNewWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNormalCheckWeight_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txtblkNumofEntrusted_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }

    
}
