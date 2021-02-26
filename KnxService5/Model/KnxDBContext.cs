using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KnxService5.Model
{
    public partial class KnxDBContext : DbContext
    {
        public KnxDBContext()
        {
        }


        public virtual DbSet<DecodedTelegram> DecodedTelegrams { get; set; }
        public virtual DbSet<Home> Homes { get; set; }
        public virtual DbSet<KnxGroupAddress> KnxGroupAddresses { get; set; }
        public virtual DbSet<KnxProcess> KnxProcesses { get; set; }
        public virtual DbSet<KnxTelegram> KnxTelegrams { get; set; }
        public virtual DbSet<RawLog> RawLogs { get; set; }
        public virtual DbSet<Xmlfile> Xmlfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<DecodedTelegram>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.Property(e => e.Tid)
                    .ValueGeneratedNever()
                    .HasColumnName("TID");

                entity.Property(e => e.Data)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FrameFormat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GroupAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SerializedData).HasColumnType("xml");

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SourceAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("date");

                entity.Property(e => e.TimestampS)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Home>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Home");

                entity.Property(e => e.HomeText)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<KnxGroupAddress>(entity =>
            {
                entity.HasKey(e => e.Gid);

                entity.Property(e => e.Central).HasMaxLength(50);

                entity.Property(e => e.Clutch).HasMaxLength(50);

                entity.Property(e => e.DeviceName).HasMaxLength(200);

                entity.Property(e => e.GroupAddress).HasMaxLength(50);

                entity.Property(e => e.Length).HasMaxLength(50);
            });

            modelBuilder.Entity<KnxProcess>(entity =>
            {
                entity.HasKey(e => e.Pid);

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.ProcessIp)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ProcessIP");

                entity.Property(e => e.ProcessName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessedFile)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<KnxTelegram>(entity =>
            {
                entity.HasKey(e => e.Tid);

                entity.Property(e => e.Tid).HasColumnName("TID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FrameFormat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RawData)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Service)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("date");

                entity.Property(e => e.TimestampS)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RawLog>(entity =>
            {
                entity.HasKey(e => e.LogId)
                    .HasName("PK__RawLogs__5E548648AC392246");

                entity.Property(e => e.FrameFormat)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LogTimestamp).HasColumnType("datetime");

                entity.Property(e => e.RawData)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Service)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Xmlfile>(entity =>
            {
                entity.HasKey(e => e.Fid)
                    .HasName("PK_XMLFiles_1");

                entity.ToTable("XMLFiles");

                entity.Property(e => e.Fid).HasColumnName("FID");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProcessingPercentage).HasDefaultValueSql("((0))");

                entity.Property(e => e.Xmldata)
                    .IsRequired()
                    .HasColumnType("xml")
                    .HasColumnName("XMLData");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
