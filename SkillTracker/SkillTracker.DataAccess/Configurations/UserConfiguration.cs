using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillTracker.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

// пока просто, далее можно дополнить
namespace SkillTracker.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(32);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(32);
            builder.Property(u => u.Patronymic).HasMaxLength(32);
        }
    }
}
