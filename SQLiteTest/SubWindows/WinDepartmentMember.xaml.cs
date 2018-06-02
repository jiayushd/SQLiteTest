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
using SQLiteTest.ViewModel;
using System.Text.RegularExpressions;


namespace SQLiteTest.SubWindows
{
    /// <summary>
    /// WinDepartmentMember.xaml 的交互逻辑
    /// </summary>
    public partial class WinDepartmentMember : Window
    {
        public WinDepartmentMember()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chkbxIsOnJob.IsChecked = false;
            List<MemberTree> department = new List<MemberTree>() { GetAllActiveMembers("电学部") };
            memberTree.ItemsSource = department;
            btnEdit.IsEnabled = false;
            //gridMemberInfo.IsEnabled = false;           
        }

        #region 辅助方法
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
            Department.Department= department;

            using (mainEntities db=new mainEntities())
            {
                List<staff> staffs = new List<staff>();
                staffs = db.staffs.Where(x => x.部门 == department && x.在职状态!=0).ToList<staff>(); ;
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
        /// <summary>
        /// 获取部门所有成员，并以membertree的形式返回
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private MemberTree GetAllMembers(string department)
        {
            MemberTree Department = new MemberTree();
            Department.Name = department;

            using (mainEntities db = new mainEntities())
            {
                List<staff> staffs = new List<staff>();
                staffs = db.staffs.Where(x => x.部门 == department).ToList<staff>(); ;
                foreach (var item in staffs)
                {
                    //新建一个与datarow对应的子树
                    MemberTree Company = new MemberTree();
                    Company.Name = item.分公司;
                    Company.Children = new List<MemberTree>();
                    MemberTree Group = new MemberTree();
                    Group.Name = item.组别;
                    Group.Children = new List<MemberTree>();
                    MemberTree Member = new MemberTree();
                    //Member.Name = dr["姓名"].ToString()+"("+dr["系统账号"].ToString()+")";
                    Member.Name = item.Name;

                    Member.Account = item.Account;

                    Group.Children.Add(Member);
                    Company.Children.Add(Group);

                    //将子树合并到部门树中去
                    Department.Add(Company);

                }

            }

            return Department;
        }
        /// <summary>
        /// 验证新增的代理人信息是否符合要求
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        private string IsFormLegal(staff staff)
        {
            string message = "";
            if (staff.Name == "")
            {
                message += "姓名为空！\n";
            }
            if (staff.Account == "")
            {
                message += "账号为空！\n";
            }

            string pattern = @"^\S\d{5}$";
            if (!Regex.IsMatch(staff.Account, pattern))
            {
                message += "账号不符合规范，应为H+5位数字！\n";
            }

            using (mainEntities db = new mainEntities())
            {
                List<staff> staffs = new List<staff>();
                staffs = db.staffs.Where(x => x.Account == staff.Account).ToList<staff>();
                if (staffs.Count() > 0)
                {
                    message += "账号已被使用！\n";
                }
            }

            return message;
        }
        #endregion

        #region 操作区

        /// <summary>
        /// 选中一个节点时，在列表区展示相应范围的代理人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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
                        staffs = db.staffs.Where(x => x.部门 == tvi.Department && x.分公司==tvi.Company).ToList<staff>();
                    }
                    break;
                case "组":
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.部门 == tvi.Department && x.分公司 == tvi.Company && x.组别==tvi.Name).ToList<staff>();
                    }
                    break;
                default:
                    using (mainEntities db = new mainEntities())
                    {
                        staffs = db.staffs.Where(x => x.Name==tvi.Name ).ToList<staff>();
                    }
                    break;
            }
            dgStaff.ItemsSource = staffs;

        }
        /// <summary>
        /// 选中datagrid中的一项时，在下面的详情区展示更详细的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            staff staff = (staff)dgStaff.SelectedItem;
            gridMemberInfo.DataContext = staff;
            btnEdit.IsEnabled = true;
        }
        /// <summary>
        /// 保存修改信息，会在编辑指令后可见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            staff staff = (staff)gridMemberInfo.DataContext;
            using (mainEntities db=new mainEntities())
            {
                db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
            MessageBox.Show("修改已提交！");
            ////刷新树形数据
            //List<MemberTree> department = new List<MemberTree>() { GetAllMembers(staff.部门) };
            //memberTree.ItemsSource = department;
            //编辑按钮重新可见、保存按钮隐藏、详情区不可用
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            gridMemberInfo.IsEnabled = false;
        }
        /// <summary>
        /// 点击新增按钮后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            //InsertMember im = new InsertMember();
            //im.ds = ds;
            //im.Show();
        }
        /// <summary>
        /// 点击编辑按钮后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            //gridMemberInfo.IsEnabled = true;
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Collapsed;
            btnAdd.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 点击删除按钮后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            staff staff = (staff)gridMemberInfo.DataContext;
            if (MessageBox.Show("是否删除当前代理人","确认删除",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            {
                using (mainEntities db = new mainEntities())
                {
                    var entry = db.Entry(staff);
                    if (entry.State==System.Data.Entity.EntityState.Detached)
                    {
                        db.staffs.Attach(staff);
                    }
                    db.staffs.Remove(staff);
                    db.SaveChanges();
                    db.Dispose();
                }

                //List<MemberTree> department = new List<MemberTree>() { GetAllMembers(staff.部门) };
                //memberTree.ItemsSource = department;

                dgStaff.ItemsSource = null;
                gridMemberInfo.DataContext = null;
            }
        }
        #endregion





        private void ChkbxIsOnJob_Checked(object sender, RoutedEventArgs e)
        {
            List<MemberTree> department = new List<MemberTree>() { GetAllMembers("电学部") };
            memberTree.ItemsSource = department;
        }

        private void ChkbxIsOnJob_Unchecked(object sender, RoutedEventArgs e)
        {
            List<MemberTree> department = new List<MemberTree>() { GetAllActiveMembers("电学部") };
            memberTree.ItemsSource = department;
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //界面控制
            btnAdd.Visibility = Visibility.Collapsed;
            btnSaveAdd.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            txtBoxName.IsReadOnly = false;
            txtBoxID.IsReadOnly = false;

            //数据处理
            staff staff = new staff();
            gridMemberInfo.DataContext = staff;
        }

        private void BtnSaveAdd_Click(object sender, RoutedEventArgs e)
        {
            //界面控制
            btnAdd.Visibility = Visibility.Visible;
            btnSaveAdd.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
            btnDelete.Visibility = Visibility.Visible;
            txtBoxName.IsReadOnly = true;
            txtBoxID.IsReadOnly = true;

            //数据处理
            staff staff = (staff)gridMemberInfo.DataContext;

            if (IsFormLegal(staff)=="")
            {
                using (mainEntities db = new mainEntities())
                {
                    staff.Id = db.staffs.Count() + 1;
                    db.staffs.Add(staff);
                    db.SaveChanges();
                    db.Dispose();
                }
                MessageBox.Show("已添加！");
            }
        }
    }
}
