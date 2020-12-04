using ExamExpert.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Mappings
{
    public class ExamMap : SqlLiteBaseMap<Exam>
    {
        public override void Configure(EntityTypeBuilder<Exam> builder)
        {
            base.Configure(builder);
            builder.Property(ex => ex.WiredTypingTitle).IsRequired();
            builder.Property(ex => ex.WiredTypingContent).IsRequired();

            builder.HasOne(ex => ex.User).WithMany(ex => ex.Exams).HasForeignKey(ex => ex.UserId);
        }
    }
}
