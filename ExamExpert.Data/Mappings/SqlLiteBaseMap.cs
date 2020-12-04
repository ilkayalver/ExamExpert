using ExamExpert.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamExpert.Data.Mappings
{
    public abstract class SqlLiteBaseMap<T> : IEntityTypeConfiguration<T>, ISqlLiteBaseMap where T : SqlLiteBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreateDate).IsRequired();
        }
    }
}
