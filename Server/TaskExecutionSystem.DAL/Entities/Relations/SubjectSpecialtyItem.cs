using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Studies;

namespace TaskExecutionSystem.DAL.Entities.Relations
{
    public class SubjectSpecialtyItem
    {
        public int Id { get; set; }


        public int SubjectId { get; set; }

        public int SpecialtyId { get; set; }


        public Subject Subject { get; set; }

        public Specialty Specialty { get; set; }
    }
}
