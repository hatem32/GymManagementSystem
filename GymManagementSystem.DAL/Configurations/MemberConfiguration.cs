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
    internal class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                          .WithOne(HR => HR.Member)
                          .HasForeignKey<HealthRecord>(M => M.MemberId);

            base.Configure(builder);
        }
    }
}
