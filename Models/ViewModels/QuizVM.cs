using ModelsShared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.ViewModels
{
    public class QuizVM
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public Guid QuizListId { get; set; }
        public QuizList QuizList { get; set; }
        public ICollection<AnswersVM> Answers { get; set; }

    }
    public class AnswersVM
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public bool IsAnswerTrue { get; set; }
        public Guid QuizId { get; set; }
        public QuizVM Quiz { get; set; }
    }
}
