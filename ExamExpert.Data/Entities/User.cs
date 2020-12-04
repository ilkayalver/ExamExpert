using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public class User : IdentityUser<int>, ISqlLiteBaseEntity
    {
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
