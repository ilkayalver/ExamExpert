using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public class Question : SqlLiteBaseEntity
    {
        public string QuestionText { get; set; }
        public string Choice_A { get; set; }
        public string Choice_B { get; set; }
        public string Choice_C { get; set; }
        public string Choice_D { get; set; }
        public char CorrectChoice { get; set; }

        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }
    }
}
