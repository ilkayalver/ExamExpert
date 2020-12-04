using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public class Role : IdentityRole<int>, ISqlLiteBaseEntity
    {
        public DateTime CreateDate { get; set; }
    }
}
