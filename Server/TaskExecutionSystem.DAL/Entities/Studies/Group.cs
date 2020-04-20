using System;
using System.Collections.Generic;
using System.Text;
using TaskExecutionSystem.DAL.Entities.Identity;
using TaskExecutionSystem.DAL.Entities.Relations;
using TaskExecutionSystem.DAL.Entities.Registration;

namespace TaskExecutionSystem.DAL.Entities.Studies
{
    public class Group
    {
        public int Id { get; set; }

        public string NumberName { get; set; }

        public List<Student> Students { get; set; }

        public List<StudentRegisterRequest> StudentRegisterRequests { get; set; }

        public List<GroupTeacherSubjectItem> GroupTeacherSubjectItems { get; set; }


        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }


        public Group()
        {
            Students = new List<Student>();
            StudentRegisterRequests = new List<StudentRegisterRequest>();
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }

        public Group(string numberName, int facultyId)
        {
            NumberName = numberName;
            FacultyId = facultyId;
            Students = new List<Student>();
            GroupTeacherSubjectItems = new List<GroupTeacherSubjectItem>();
        }
    }
}
