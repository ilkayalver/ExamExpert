using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Common.Models
{
    public class QuestionModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string Choice_A { get; set; }
        public string Choice_B { get; set; }
        public string Choice_C { get; set; }
        public string Choice_D { get; set; }
        public char CorrectChoice { get; set; }
    }
}
