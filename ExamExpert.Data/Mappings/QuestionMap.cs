using ExamExpert.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Mappings
{
    public class QuestionMap : SqlLiteBaseMap<Question>
    {
        public override void Configure(EntityTypeBuilder<Question> builder)
        {
            base.Configure(builder);
            builder.Property(ex => ex.QuestionText).IsRequired();
            builder.Property(ex => ex.Choice_A).IsRequired();
            builder.Property(ex => ex.Choice_B).IsRequired();
            builder.Property(ex => ex.Choice_C).IsRequired();
            builder.Property(ex => ex.Choice_D).IsRequired();
            builder.Property(ex => ex.CorrectChoice).IsRequired();

            builder.HasOne(ex => ex.Exam).WithMany(ex => ex.Questions).HasForeignKey(ex => ex.ExamId);
        }
    }
}
