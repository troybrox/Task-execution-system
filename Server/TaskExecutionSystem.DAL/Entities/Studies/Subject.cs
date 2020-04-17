using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }


        public Subject()
        {
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }

        public Subject(string name)
        {
            Name = name;
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }
    }
}
