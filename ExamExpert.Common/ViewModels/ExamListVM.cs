using ExamExpert.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Common.ViewModels
{
    public class ExamListVM
    {
        public int ExamOrderNumber { get; set; }
        public ExamModel ExamPaper { get; set; } = new ExamModel();
        
    }
}
