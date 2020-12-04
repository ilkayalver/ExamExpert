using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Common.Models
{
    public class ExamModel
    {
        public int ExamId { get; set; }
        public string WiredTypingTitle { get; set; }
        public string WiredTypingContent { get; set; }
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
