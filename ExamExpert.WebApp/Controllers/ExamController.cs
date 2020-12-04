using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamExpert.Bussinees.Interfaces;
using ExamExpert.Common.Models;
using ExamExpert.Common.ViewModels;
using ExamExpert.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamExpert.WebApp.Controllers
{
    public class ExamController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWiredHtmlAgilityService _wiredAPIService;
        private readonly IExamService _examService;

        public ExamController(UserManager<User> userManager, IWiredHtmlAgilityService wiredAPIService, IExamService examService)
        {
            _userManager = userManager;
            _wiredAPIService = wiredAPIService;
            _examService = examService;
        }

        // GET: Exam
        public ActionResult GenerateExam()
        {
            List<WiredTypingsListVM> wiredTypingsVM = new List<WiredTypingsListVM>();
            wiredTypingsVM = _wiredAPIService.GetLastFiveTypings();

            return View(wiredTypingsVM);
        }


        [HttpPost]
        public async Task<ActionResult> GenerateExam([FromBody]GenerateExamVM generateExamVM)
        {
            if (User.Identity.Name != null)
            {
                var teacherFromUsers = await _userManager.FindByNameAsync(User.Identity.Name);
                if (teacherFromUsers != null)
                {
                    var isSuccess = _examService.GenerateExam(generateExamVM, teacherFromUsers.Id);

                    if (isSuccess == true)
                    {
                        return Ok();
                    }
                }
            }
            return BadRequest();
        }


        public async Task<ActionResult> ListExams()
        {
            if (User.Identity.Name != null)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {
                    //examListVM = _examService.GetAllExamsByTeacherId(user.Id);
                    var listExamModel = _examService.GetAllExamsByTeacherId(user.Id);

                    if (listExamModel != null)
                    {
                        var examListVM = new List<ExamListVM>();

                        for (int i = 0; i < listExamModel.Count; i++)
                        {
                            examListVM.Add(new ExamListVM
                            {
                                ExamOrderNumber = i + 1,
                                ExamPaper = new ExamModel()
                            });

                            examListVM[i].ExamPaper.ExamId = listExamModel[i].ExamId;
                            examListVM[i].ExamPaper.WiredTypingTitle = listExamModel[i].WiredTypingTitle;
                            examListVM[i].ExamPaper.WiredTypingContent = listExamModel[i].WiredTypingContent;
                            examListVM[i].ExamPaper.UserId = listExamModel[i].UserId;
                            examListVM[i].ExamPaper.CreateDate = listExamModel[i].CreateDate.Date;

                            for (int j = 0; j < listExamModel[i].Questions.Count; j++)
                            {
                                examListVM[i].ExamPaper.Questions.Add(new QuestionModel
                                {
                                    QuestionText = listExamModel[i].Questions[j].QuestionText,
                                    Choice_A = listExamModel[i].Questions[j].Choice_A,
                                    Choice_B = listExamModel[i].Questions[j].Choice_B,
                                    Choice_C = listExamModel[i].Questions[j].Choice_C,
                                    Choice_D = listExamModel[i].Questions[j].Choice_D,
                                    CorrectChoice = listExamModel[i].Questions[j].CorrectChoice
                                });
                            }

                        }
                        return View(examListVM);
                    }
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public JsonResult DeleteExam(int examId)
        {
            string messageOfDeleteOperation = null;
            if (User.Identity.Name != null)
            {
                if (messageOfDeleteOperation != "failed")
                {
                    messageOfDeleteOperation = _examService.DeleteExamById(examId);
                    return Json(new { messageOfDeleteOperation = messageOfDeleteOperation });
                }
                else
                {
                    messageOfDeleteOperation = "Bir Hata Oluştu !";
                    return Json(new { messageOfDeleteOperation = messageOfDeleteOperation });
                }

            }
            messageOfDeleteOperation = "Kullanıcı Giriş Yapmamış !";
            return Json(new { messageOfDeleteOperation = messageOfDeleteOperation });
        }


        public ActionResult CheckExam(int examId) //examId will be sended from ListExams page.
        {
            var examPaper = new ExamPaperVM();
            var examModel = _examService.GetExamByIdForCheckingOperation(examId);

            examPaper.Exam.WiredTypingTitle = examModel.WiredTypingTitle;
            examPaper.Exam.WiredTypingContent = examModel.WiredTypingContent;

            for (int i = 0; i < examModel.Questions.Count; i++)
            {
                examPaper.Exam.Questions.Add(new QuestionModel
                {
                    QuestionId = examModel.Questions[i].QuestionId,
                    QuestionText = examModel.Questions[i].QuestionText,
                    Choice_A = examModel.Questions[i].Choice_A,
                    Choice_B = examModel.Questions[i].Choice_B,
                    Choice_C = examModel.Questions[i].Choice_C,
                    Choice_D = examModel.Questions[i].Choice_D,
                    CorrectChoice = examModel.Questions[i].CorrectChoice
                });
            }

            examPaper.Exam.UserId = examModel.UserId;
            examPaper.Exam.CreateDate = examModel.CreateDate;

            return View(examPaper);
        }

        [HttpPost]
        public JsonResult CheckExam([FromBody]List<CheckExamVM> answers) //examPaperDatas will be sended from CheckExam page.
        {
            if (User.Identity.Name != null)
            {
                return Json(new { results = _examService.CheckAnswersOfExam(answers) });
            }
            return Json(new { results = "error" });
        }
    }
}