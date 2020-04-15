using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Teacher> Teachers { get; set; }


        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }


        public Department()
        {
            Teachers = new List<Teacher>();
        }
    }
}
