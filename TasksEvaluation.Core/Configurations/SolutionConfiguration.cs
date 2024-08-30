using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TasksEvaluation.Core.Entities.Business;

namespace TasksEvaluation.Core.Configurations
{
    public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
    {
        public void Configure(EntityTypeBuilder<Solution> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(sol => sol.Student)
               .WithMany(st => st.Solutions)
               .HasForeignKey(sol => sol.StudentId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(sol => sol.Assignment)
               .WithMany(a => a.Solutions)
               .HasForeignKey(sol => sol.AssignmentId)
               .OnDelete(DeleteBehavior.SetNull);

            
            // group has one 


        }
    }
}
