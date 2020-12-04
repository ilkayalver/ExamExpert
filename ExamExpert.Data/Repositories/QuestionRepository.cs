using ExamExpert.Data.Entities;
using ExamExpert.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Repositories
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        private readonly ExamExpertSQLLiteDbContext _sqliteDBContext;
        public QuestionRepository(ExamExpertSQLLiteDbContext sqliteDBContext) : base(sqliteDBContext)
        {
            _sqliteDBContext = sqliteDBContext;
        }
    }
}
