using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public class SqlLiteBaseEntity : ISqlLiteBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
