using fullcalendarStore.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fullcalendarStore.Data
{
    public class FullcalendarStoreContext : DbContext
    {
        public FullcalendarStoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<CalendarItem> CalendarItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
