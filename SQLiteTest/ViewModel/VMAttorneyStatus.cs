using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.ViewModel
{
    class VMAttorneyStatus
    {
        string name;
        int intCN;
        int intForeign;
        int intTodo;
        int intFirstVirsion;
        int intAllOA;
        int intOAin30;

        List<Task> tasksCN;
        List<Task> tasksForeign;
        List<Task> tasksTodo;
        List<Task> tasksFirstVirsion;
        List<Task> tasksAllOA;
        List<Task> tasksOAin30;


    }
}
