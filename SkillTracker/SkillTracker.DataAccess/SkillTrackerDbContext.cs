using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkillTracker.DataAccess
{
    public class SkillTrackerDbContext : DbContext
    {
        public SkillTrackerDbContext(DbContextOptions<SkillTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.UserEntity> Users { get; set; } = null!;
    }
}
