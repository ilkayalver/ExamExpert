using ExamExpert.Common.Models;
using ExamExpert.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Bussinees.Interfaces
{
    public interface IExamService
    {
        bool GenerateExam(GenerateExamVM examVM, int tecaherId);
        List<ExamModel> GetAllExamsByTeacherId(int teacherId);
        string DeleteExamById(int examId);
        ExamModel GetExamByIdForCheckingOperation(int examId);
        List<bool> CheckAnswersOfExam(List<CheckExamVM> data);
    }
}
