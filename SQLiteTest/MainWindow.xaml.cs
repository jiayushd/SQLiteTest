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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Data.SQLite.Linq;
using SQLiteTest.SubWindows;

namespace SQLiteTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        staff staff = new staff();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dgStaff.ItemsSource = null;
            using (mainEntities db=new mainEntities() )
            {
                dgStaff.ItemsSource = db.staffs.ToList<staff>();
                db.Dispose();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            staff.Account = txtbxAccount.Text;
            staff.Name = txtbxName.Text;
            staff.ProfessionDate = dpkProfesstionDate.DisplayDate.Date.ToString();
            using (mainEntities db = new mainEntities())
            {
                db.staffs.Add(staff);
                db.SaveChanges();
                db.Dispose();
            }
        }

        private void dgStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            staff staff = (staff)dgStaff.SelectedItem;
            txtbxName.Text = staff.Name;
            txtbxAccount.Text = staff.Account;
            //dpkProfesstionDate.DisplayDate = staff.ProfessionDate;
            txtbxTel.Text = staff.电话;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            staff.Account = txtbxAccount.Text;
            staff.Name = txtbxName.Text;
            staff.电话 = txtbxTel.Text;

            using (mainEntities db = new mainEntities())
            {
                db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WinDepartmentMember attorney = new WinDepartmentMember();
            attorney.Show();
        }

        private void BtnGetTaskList_Click(object sender, RoutedEventArgs e)
        {
            dgStaff.ItemsSource = null;
            using (mainEntities db = new mainEntities())
            {
                dgTasks.ItemsSource = db.Tasks.ToList<Task>();
                db.Dispose();
            }
        }

        private void BtnAttorneyIndex_Click(object sender, RoutedEventArgs e)
        {
            WinAttorneyIndex ai = new WinAttorneyIndex();
            ai.Show();
        }
    }
}
