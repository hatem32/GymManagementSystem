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
    internal class BookingCofiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(x => x.Id);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(X => X.Session)
                   .WithMany(X => X.SessionMembers)
                   .HasForeignKey(X => X.SessionId);

            builder.HasOne(X => X.Member)
                   .WithMany(X => X.MemberSessions)
                   .HasForeignKey(X => X.MemberId);

            builder.HasKey(x => new { x.MemberId, x.SessionId });
        }
    }
}
