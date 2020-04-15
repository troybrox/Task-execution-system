using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Faculty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Group> Groups { get; set; }

        public List<Department> Departments { get; set; }


        public Faculty()
        {
            Groups = new List<Group>();
            Departments = new List<Department>();
        }
    }
}
