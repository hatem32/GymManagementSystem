using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Configurations
{
    internal class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");


            builder.HasOne(m => m.Plan)
                          .WithMany(p => p.PlanMembers)
                          .HasForeignKey(m => m.PlanId)
                          .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Member)
                   .WithMany(me => me.MemberPlans)
                   .HasForeignKey(m => m.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
