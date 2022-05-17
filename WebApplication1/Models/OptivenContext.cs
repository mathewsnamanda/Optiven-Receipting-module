using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplication1.Models;

namespace ClientStatements.Models
{
    public partial class OptivenContext : DbContext
    {
       public OptivenContext(DbContextOptions<OptivenContext> options)
            : base(options)
        {
        }
      
        public virtual DbSet<SaveReceipt> Receipts { get; set; } = null!;

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
