using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.Models
{
    public class Classes
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string IdCode { get; set; }
        public ICollection<UsersClass> Students { get; set; }
        public ICollection<QuizList> QuizList { get; set; }
        public Guid? TeacherId { get; set; }
        public Users Teacher { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class UsersClass
    {
        public Guid ClassId { get; set; }
        public Classes Class { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; }
    }
}
