﻿using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Relations;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Student> Students { get; set; }

        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }


        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }


        public Group()
        {
            Students = new List<Student>();
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }
    }
}
