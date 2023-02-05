using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsShared.Models
{
    public class QuizList
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
        public Classes Classes { get; set; }
        public string IdCode { get; set; }
        public ICollection<Quiz> Quizzes { get; set; }
    }
	public enum QuizType
	{
		Normal,
		PresenceAndAbsence
	}

	public class Quiz
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public Guid QuizListId { get; set; }
        public QuizList QuizList { get; set; }
        public ICollection<Answers> Answers { get; set; }
    }
    public class Answers
    { 
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public bool IsAnswerTrue { get; set; }
        public Guid QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
    public class StudentAnswersQustions
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; }
        public Guid QuizListId { get; set; }
        public QuizList QuizList { get; set; }
		public int Result { get; set; }
		public ICollection<StudentQustion> StudentQustion { get; set; }
    }
    public class StudentQustion
    {
        public Guid Id { get; set; }
		public Guid? QustionId { get; set; }
		public Quiz Qustion { get; set; }
		public Guid StudentAnswersQustionsId { get; set; }
        public StudentAnswersQustions StudentAnswersQustions { get; set; }
        public ICollection<StudentAnswer> StudentAnswer { get; set; }
    }
    public class StudentAnswer
    {
        public Guid Id { get; set; }
        public Guid? StudentQustionId { get; set; }
        public StudentQustion StudentQustion { get; set; }
        public Guid AnswerId { get; set; }
        public Answers Answer { get; set; }
    }
    public enum QuizTimeType
    {
        Interval,
        FromTo,
        Unlimited
    }
}
