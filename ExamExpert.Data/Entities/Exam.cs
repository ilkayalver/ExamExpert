using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public class Exam : SqlLiteBaseEntity
    {
        public string WiredTypingTitle { get; set; }
        public string WiredTypingContent { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<Question> Questions { get; set; }
    }
}
