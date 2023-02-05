using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.ViewModels
{
    public class QuizListVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Result { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public QuizTimeType QuizTimeType { get; set; }
        public QuizType QuizType { get; set; }
        public bool IsMarker { get; set; }
        public Guid ClassesId { get; set; }
        public string IdCode { get; set; }
    }
}
