using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);
            //builder.Property(a => a.Title).IsRequired();
            //builder.Property(a => a.Description).IsRequired();
            //builder.Property(a => a.DeadLine).IsRequired();
            

            builder.HasMany(a => a.Solutions)
               .WithOne(s => s.Assignment)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Group)
                   .WithMany(g => g.Assignments)
                   .HasForeignKey(a => a.GroupId)
                   .OnDelete(DeleteBehavior.SetNull);

            
        }
    }
}
