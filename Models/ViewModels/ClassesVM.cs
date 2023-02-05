using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.ViewModels
{
    public class ClassesVM
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string IdCode { get; set; }
        public Guid TeacherId { get; set; }
        public int NumberOfStudents { get; set; }
    }
}
