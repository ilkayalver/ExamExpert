using ExamExpert.Bussinees.Interfaces;
using ExamExpert.Common.Models;
using ExamExpert.Common.ViewModels;
using ExamExpert.Data.Entities;
using ExamExpert.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamExpert.Bussinees.Services
{
    public class ExamService : IExamService
    {
        private readonly IExamRepository _examRepo;
        private readonly IQuestionRepository _questionRepo;

        public ExamService(IExamRepository examRepo, IQuestionRepository questionRepo)
        {
            _examRepo = examRepo;
            _questionRepo = questionRepo;
        }

        #region GenerateExam
        public bool GenerateExam(GenerateExamVM examVM, int tecaherId)
        {
            //questionsand typings are received into examVM

            var exam = new Exam
            {
                WiredTypingTitle = examVM.Exam.WiredTypingTitle,
                WiredTypingContent = examVM.Exam.WiredTypingContent,
                UserId = tecaherId,
                Questions = new List<Question>(),
                CreateDate = DateTime.Now
            };

            foreach (var question in examVM.Exam.Questions)
            {
                exam.Questions.Add(new Question
                {
                    Choice_A = question.Choice_A,
                    Choice_B = question.Choice_B,
                    Choice_C = question.Choice_C,
                    Choice_D = question.Choice_D,
                    CorrectChoice = question.CorrectChoice,
                    CreateDate = DateTime.Now,
                    QuestionText = question.QuestionText,
                });
            }

            return _examRepo.Add(exam);

        }
        #endregion

        #region GetAllExamsOfTeacher
        public List<ExamModel> GetAllExamsByTeacherId(int teacherId)
        {
            var allExamsOfTeacher = _examRepo.GetAll().Where(e => e.UserId == teacherId).ToList();
            var examModel = new List<ExamModel>();

            if (allExamsOfTeacher != null)
            {

                for (int i = 0; i < allExamsOfTeacher.Count; i++)
                {
                    examModel.Add(new ExamModel
                    {
                        ExamId = allExamsOfTeacher[i].Id,
                        WiredTypingTitle = allExamsOfTeacher[i].WiredTypingTitle.ToString(),
                        WiredTypingContent = allExamsOfTeacher[i].WiredTypingContent.ToString(),
                        UserId = Convert.ToInt32(allExamsOfTeacher[i].UserId),
                        Questions = new List<QuestionModel>(),
                        CreateDate = allExamsOfTeacher[i].CreateDate
                    });
                }

                for (int j = 0; j < examModel.Count; j++)
                {
                    for (int k = 0; k < allExamsOfTeacher[j].Questions.Count; k++)
                    {
                        examModel[j].Questions.Add(new QuestionModel
                        {
                            QuestionId = allExamsOfTeacher[j].Questions[k].Id,
                            QuestionText = allExamsOfTeacher[j].Questions[k].QuestionText,
                            Choice_A = allExamsOfTeacher[j].Questions[k].Choice_A,
                            Choice_B = allExamsOfTeacher[j].Questions[k].Choice_B,
                            Choice_C = allExamsOfTeacher[j].Questions[k].Choice_C,
                            Choice_D = allExamsOfTeacher[j].Questions[k].Choice_D,
                            CorrectChoice = allExamsOfTeacher[j].Questions[k].CorrectChoice
                        });
                    }
                }
                return examModel;
            }
            else
            {
                examModel = null;
                return examModel;
            }
        }
        #endregion

        #region DeleteExam
        public string DeleteExamById(int examId)
        {
            string message = null;
            var willDeleteExam = _examRepo.GetAll().Where(e => e.Id == examId).FirstOrDefault();
            if (willDeleteExam != null)
            {
                var questionsOfWillDeleteExam = _questionRepo.GetAll().Where(q => q.ExamId == examId).ToList();
                if (questionsOfWillDeleteExam != null)
                {
                    for (int i = 0; i < questionsOfWillDeleteExam.Count; i++)
                    {
                        _questionRepo.Delete(questionsOfWillDeleteExam[i].Id);
                    }
                    _examRepo.Delete(examId);

                    message = willDeleteExam.CreateDate + " " + "tarihinde oluşturduğunuz" + " " + willDeleteExam.WiredTypingTitle + " " + "başlığını içeren sınav silinmiştir.";
                    return message;
                }
            }
            message = "failed";
            return message;
        }
        #endregion

        #region GetExamPaperForCheckOperation
        public ExamModel GetExamByIdForCheckingOperation(int examId)
        {
            var examDatas = _examRepo.GetAll().Where(x => x.Id == examId).FirstOrDefault();

            var examModel = new ExamModel();
            examModel.WiredTypingTitle = examDatas.WiredTypingTitle;
            examModel.WiredTypingContent = examDatas.WiredTypingContent;

            for (int i = 0; i < examDatas.Questions.Count; i++)
            {
                examModel.Questions.Add(new QuestionModel
                {
                    QuestionId = examDatas.Questions[i].Id,
                    QuestionText = examDatas.Questions[i].QuestionText,
                    Choice_A = examDatas.Questions[i].Choice_A,
                    Choice_B = examDatas.Questions[i].Choice_B,
                    Choice_C = examDatas.Questions[i].Choice_C,
                    Choice_D = examDatas.Questions[i].Choice_D,
                    CorrectChoice = examDatas.Questions[i].CorrectChoice
                });
            }

            examModel.UserId = examDatas.UserId;
            examModel.CreateDate = examDatas.CreateDate;

            return examModel;
        }
        #endregion

        #region CheckAnswers
        public List<bool> CheckAnswersOfExam(List<CheckExamVM> questionIdsAndUserAnswerChoices)
        {
            List<bool> results = new List<bool>();

            var questions = _questionRepo.GetAll();
            foreach (var questionIdAndUserAnswerChoice in questionIdsAndUserAnswerChoices)
            {
                if (questions.
                    Where(x => x.Id == questionIdAndUserAnswerChoice.QuestionId).
                    FirstOrDefault().CorrectChoice == questionIdAndUserAnswerChoice.Answer)
                {
                    results.Add(true);
                }
                else
                {
                    results.Add(false);
                }
            }

            return results;
        }
        #endregion

    }
}
