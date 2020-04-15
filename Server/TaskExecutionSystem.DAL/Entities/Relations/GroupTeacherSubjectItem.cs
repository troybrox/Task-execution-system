using System;
using System.Collections.Generic;
using System.Text;

namespace TaskExecutionSystem.DAL.Entities.Relations
{
    public class GroupTeacherSubjectItem
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public int SubjectId { get; set; }
    }
}
