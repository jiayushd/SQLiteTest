using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteTest.ViewModel
{
    class MemberTree
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string NodeType { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string Group { get; set; }
        public List<MemberTree> Children { get; set; }

        /// <summary>
        /// 将外部的树与本树的子树合并
        /// </summary>
        /// <param name="mt"></param>
        /// <returns></returns>
        public void Add(MemberTree mt)
        {
            //如果没有子树，则将外部树直接加入本树
            if (Children == null)
            {
                Children = new List<MemberTree>();
                Children.Add(mt);
            }
            else//否则
            {
                MemberTree foundTree = new MemberTree();
                //foundTree = Children.Find(delegate (MemberTree p) { return p.Name == mt.Name; });
                foundTree = Children.Find(s => s.Name == mt.Name);
                if (foundTree == null)
                {
                    Children.Add(mt);
                }
                else
                {
                    foundTree.Add(mt.Children[0]);
                }
            }
        }
    }
}
