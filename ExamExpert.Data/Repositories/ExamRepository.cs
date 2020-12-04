using ExamExpert.Data.Entities;
using ExamExpert.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamExpert.Data.Repositories
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        private readonly ExamExpertSQLLiteDbContext _sqliteDBContext;
        public ExamRepository(ExamExpertSQLLiteDbContext sqliteDBContext) : base(sqliteDBContext)
        {
            _sqliteDBContext = sqliteDBContext;
        }

        public new IQueryable<Exam> GetAll()
        {
            return _sqliteDBContext.Exams.Include(x => x.Questions).AsQueryable();
        }

    }
}
