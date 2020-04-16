using TaskExecutionSystem.DAL.Entities.Studies;
using TaskExecutionSystem.DAL.Entities.Identity;

namespace TaskExecutionSystem.DAL.Entities.Relations
{
    public class GroupTeacherSubjectItem
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int TeacherId { get; set; }

        public int SubjectId { get; set; }


        public Group Group { get; set; }

        public Teacher Teacher { get; set; }

        public Subject Subject { get; set; }
    }
}
