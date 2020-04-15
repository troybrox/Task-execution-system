using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Specialty
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NumberName { get; set; }


        public List<Group> Groups { get; set; }

        public List<SubjectSpecialtyItem> SubjectSpecialtyItems { get; set; }


        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }


        public Specialty()
        {
            Groups = new List<Group>();
            SubjectSpecialtyItems = new List<SubjectSpecialtyItem>();
        }
    }
}
