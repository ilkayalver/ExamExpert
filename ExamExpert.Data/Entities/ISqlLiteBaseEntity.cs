using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Entities
{
    public interface ISqlLiteBaseEntity
    {
        int Id { get; set; }
        DateTime CreateDate { get; set; }
    }
}
