using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
namespace TrashToRewardsV2.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ADMIN> ADMIN { get; set; }
        public DbSet<NGUOIDUNG> NGUOIDUNG { get; set; }
        public DbSet<NHANVIEN> NHANVIEN { get; set; }
        public DbSet<LOAIPHELIEU> LOAIPHELIEU { get; set; }
        public DbSet<PHANTHUONG> PHANTHUONG { get; set; }
        public DbSet<DONVITHUGOM> DONVITHUGOM { get; set; }
        public DbSet<PHIEUTHUGOM> PHIEUTHUGOM { get; set; }
        public DbSet<PHIEUGHINHAN> PHIEUGHINHAN { get; set; }
        public DbSet<PHIEUDOIQUA> PHIEUDOIQUA { get; set; }
        public DbSet<LICHSUTICHDIEM> LICHSUTICHDIEM { get; set; }
        public DbSet<CTPHIEUTHUGOM> CTPHIEUTHUGOM { get; set; }
        public DbSet<CTPHIEUGHINHAN> CTPHIEUGHINHAN { get; set; }
        public DbSet<CTPHIEUDOIQUA> CTPHIEUDOIQUA { get; set; }
        public DbSet<PHIEUDATLICH> PHIEUDATLICH { get; set; }
        public DbSet<THONGBAOND> THONGBAOND { get; set; }
        public DbSet<THONGBAONV> THONGBAONV { get; set; }
        public DbSet<KHOQUA> KHOQUA { get; set; }
        public DbSet<CTKHOQUA> CTKHOQUA { get; set; }
        public DbSet<NHANVIENGQ> NHANVIENGQ { get; set; }
        public DbSet<THONGBAONVGQ> THONGBAONVGQ { get; set; }
        public DbSet<XACNHAN> XACNHAN { get; set; }

        //public DbSet<UUDAI> UUDAIs { get; set; }
        //public DbSet<CTUUDAI> CTUUDAIs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Định nghĩa khóa chính cho bảng có composite key
            modelBuilder.Entity<CTPHIEUTHUGOM>()
                .HasKey(c => new { c.MaLoaiPL, c.MaPhieu });

            modelBuilder.Entity<CTPHIEUGHINHAN>()
                .HasKey(c => new { c.MaLoaiPL, c.MaPhieuGN });
            modelBuilder.Entity<NGUOIDUNG>()
                .Property(n => n.MaND)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<PHIEUTHUGOM>()
                .HasOne(ptg => ptg.XACNHAN)
                .WithOne(xn => xn.PhieuThuGomLienKet)
                .HasForeignKey<PHIEUTHUGOM>(ptg => ptg.MaXN)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CTPHIEUDOIQUA>()
                .HasKey(c => new { c.MaPDQ, c.MaQua });
            modelBuilder.Entity<CTKHOQUA>()
                .HasKey(ct => new { ct.MaKQ, ct.MaQua });
            

        }
    }
}
