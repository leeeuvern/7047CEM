
using AxiePro.Models.Payment;
using AxieProBackground.Models.Axies;
using AxieProBackground.Models.Logs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxieProBackground
{
    public partial class DataContext : DbContext
    {
      

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BattleHistory> BattleHistory { get; set; }

        public virtual DbSet<AxieTeam> AxieTeam { get; set; }
        public virtual DbSet<AxieTeamFighters> AxieTeamFighters { get; set; }
        public virtual DbSet<Axie> Axie { get; set; }

        public virtual DbSet<MissingAxie> MissingAxie { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        public virtual DbSet<Membership> Membership { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BattleHistory>(entity =>
            {
                entity.ToTable("battlehistory");
     

            });


            modelBuilder.Entity<Axie>(entity =>
            {
                entity.ToTable("axie");

            });
            modelBuilder.Entity<AxieTeam>(entity =>
            {
                entity.ToTable("axieteam");

            });


            modelBuilder.Entity<AxieTeamFighters>(entity =>
            {
                entity.ToTable("axieteamfighters");

            });


            modelBuilder.Entity<MissingAxie>(entity =>
            {
                entity.ToTable("missingaxie");

            });



            modelBuilder.Entity<Membership>(entity =>
            {
                entity.ToTable("membership");

            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("paymenttransaction");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
