using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MVcInterview.Models.CandidateDetailsT
{
    public partial class JeetuContext : DbContext
    {
        public JeetuContext()
        {
        }

        public JeetuContext(DbContextOptions<JeetuContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CandidateDetailsT> CandidateDetailsTs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=Jeetu; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidateDetailsT>(entity =>
            {
                entity.HasKey(e => e.CandidateId);

                entity.ToTable("CandidateDetails_T");

                entity.Property(e => e.CandidateExperience)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateMailId)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CandidatePhoneNo)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateSkills).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
