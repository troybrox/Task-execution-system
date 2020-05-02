using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Task;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }

        public List<TaskModel> Tasks { get; set; }

        public List<Solution> Solutions { get; set; } // [?]

        public List<Group> Groups { get; set; }

        public Subject()
        {
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
            Tasks = new List<TaskModel>();
            Solutions = new List<Solution>();
            Groups = new List<Group>();
        }

        public Subject(string name)
        {
            Name = name;
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
            Tasks = new List<TaskModel>();
            Solutions = new List<Solution>();
            Groups = new List<Group>();
        }
    }
}
