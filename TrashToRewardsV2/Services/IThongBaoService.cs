using Microsoft.EntityFrameworkCore;
using TrashToRewardsV2.Models;

namespace TrashToRewardsV2.Services
{
    public interface IThongBaoService
    {
        Task ThongBaoThayDoiPhanThuongAsync(string hanhDong, string tenQua);
        Task ThongBaoThayDoiLoaiPheLieuAsync(string hanhDong, string tenLoai);
        Task ThongBaoThayDoiDonViThuGomAsync(string hanhDong, string tenDV);
        Task ThongBaoThayDoiKhoQuaThuGomAsync(string hanhDong, string tenKQ);

        Task ThongBaoPhieuDatLichMoiAsync(int maPhieuDL);
        Task ThongBaoXacNhanDatLichAsync(int maPhieuDL);
        Task ThongBaoHuyPhieuDatLichAsync(int maPhieuDL);
        Task ThongBaoHoanThanhThuGomAsync(int maPhieuDL, float tongDiem);
        Task ThongBaoHuyPhieuDatLichQuaHanAsync(int maPhieuDL);


        Task ThongBaoNDAsync(int maND, string noiDung);
        Task ThongBaoNVAsync(int maNV, string noiDung);
        Task ThongBaoNVGQAsync(int maNVGQ, string noiDung);
        Task DanhDauTatCaLaDaDocNDAsync(int maND);
        Task DanhDauTatCaLaDaDocNVAsync(int maNV);
        Task DanhDauTatCaLaDaDocNVGQAsync(int maNVGQ);

        public Task DanhDauThongBaoDaDocNDAsync(int maTBND);
        public Task DanhDauThongBaoDaDocNVAsync(int maTBNV);
        public Task DanhDauThongBaoDaDocNVGQAsync(int maTBNVGQ);

    }

    public class ThongBaoService : IThongBaoService
    {
        private readonly ApplicationDbContext _context;

        public ThongBaoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ThongBaoNDAsync(int maND, string noiDung)
        {
            var tb = new THONGBAOND
            {
                MaND = maND,
                NoiDungND = noiDung,
                NgayTaoND = DateTime.Now,
                DaDoc = false
            };
            await _context.THONGBAOND.AddAsync(tb);
            await _context.SaveChangesAsync();
        }

        public async Task ThongBaoNVAsync(int maNV, string noiDung)
        {
            var tb = new THONGBAONV
            {
                MaNV = maNV,
                NoiDungNV = noiDung,
                NgayTaoNV = DateTime.Now,
                DaDoc = false
            };
            await _context.THONGBAONV.AddAsync(tb);
            await _context.SaveChangesAsync();
        }
        public async Task ThongBaoNVGQAsync(int maNVGQ, string noiDung)
        {
            var tb = new THONGBAONVGQ
            {
                MaNVGQ = maNVGQ,
                NoiDungNVGQ = noiDung,
                NgayTaoNVGQ = DateTime.Now,
                DaDoc = false
            };
            await _context.THONGBAONVGQ.AddAsync(tb);
            await _context.SaveChangesAsync();
        }

        public async Task DanhDauTatCaLaDaDocNDAsync(int maND)
        {
            var ds = await _context.THONGBAOND.Where(t => t.MaND == maND && !t.DaDoc).ToListAsync();
            ds.ForEach(t => t.DaDoc = true);
            await _context.SaveChangesAsync();
        }

        public async Task DanhDauTatCaLaDaDocNVAsync(int maNV)
        {
            var ds = await _context.THONGBAONV.Where(t => t.MaNV == maNV && !t.DaDoc).ToListAsync();
            ds.ForEach(t => t.DaDoc = true);
            await _context.SaveChangesAsync();
        }
        public async Task DanhDauTatCaLaDaDocNVGQAsync(int maNVGQ)
        {
            var ds = await _context.THONGBAONVGQ.Where(t => t.MaNVGQ == maNVGQ && !t.DaDoc).ToListAsync();
            ds.ForEach(t => t.DaDoc = true);
            await _context.SaveChangesAsync();
        }
        public async Task ThongBaoThayDoiPhanThuongAsync(string hanhDong, string tenQua)
        {
            string noiDung = $"Phần thưởng '{tenQua}' đã được {hanhDong}.";
            var ndIds = await _context.NGUOIDUNG.Select(n => n.MaND).ToListAsync();
            foreach (var maND in ndIds)
            {
                await ThongBaoNDAsync(maND, noiDung);
            }
            var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
            foreach (var maNV in nvIds)
            {
                await ThongBaoNVAsync(maNV, noiDung);
            }
            var nvgqIds = await _context.NHANVIENGQ.Select(n => n.MaNVGQ).ToListAsync();
            foreach (var maNVGQ in nvgqIds)
            {
                await ThongBaoNVGQAsync(maNVGQ, noiDung);
            }


        }

        public async Task ThongBaoThayDoiLoaiPheLieuAsync(string hanhDong, string tenLoai)
        {
            string noiDung = $"Loại phế liệu '{tenLoai}' đã được {hanhDong}.";

            var ndIds = await _context.NGUOIDUNG.Select(n => n.MaND).ToListAsync();
            foreach (var maND in ndIds)
            {
                await ThongBaoNDAsync(maND, noiDung);
            }
            var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
            foreach (var maNV in nvIds)
            {
                await ThongBaoNVAsync(maNV, noiDung);
            }
            var nvgqIds = await _context.NHANVIENGQ.Select(n => n.MaNVGQ).ToListAsync();
            foreach (var maNVGQ in nvgqIds)
            {
                await ThongBaoNVGQAsync(maNVGQ, noiDung);
            }
        }

        public async Task ThongBaoThayDoiDonViThuGomAsync(string hanhDong, string tenDV)
        {
            string noiDung = $"Đơn vị thu gom '{tenDV}' đã được {hanhDong}.";

            var ndIds = await _context.NGUOIDUNG.Select(n => n.MaND).ToListAsync();
            foreach (var maND in ndIds)
            {
                await ThongBaoNDAsync(maND, noiDung);
            }
            var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
            foreach (var maNV in nvIds)
            {
                await ThongBaoNVAsync(maNV, noiDung);
            }
            var nvgqIds = await _context.NHANVIENGQ.Select(n => n.MaNVGQ).ToListAsync();
            foreach (var maNVGQ in nvgqIds)
            {
                await ThongBaoNVGQAsync(maNVGQ, noiDung);
            }
        }
        public async Task ThongBaoThayDoiKhoQuaThuGomAsync(string hanhDong, string tenKQ)
        {
            string noiDung = $"Kho quà '{tenKQ}' đã được {hanhDong}.";

            var ndIds = await _context.NGUOIDUNG.Select(n => n.MaND).ToListAsync();
            foreach (var maND in ndIds)
            {
                await ThongBaoNDAsync(maND, noiDung);
            }
            var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
            foreach (var maNV in nvIds)
            {
                await ThongBaoNVAsync(maNV, noiDung);
            }
            var nvgqIds = await _context.NHANVIENGQ.Select(n => n.MaNVGQ).ToListAsync();
            foreach (var maNVGQ in nvgqIds)
            {
                await ThongBaoNVGQAsync(maNVGQ, noiDung);
            }
        }

        public async Task ThongBaoPhieuDatLichMoiAsync(int maPhieuDL)
        {
            var phieuDatLich = await _context.PHIEUDATLICH.Include(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);

            if (phieuDatLich != null)
            {
                string noiDung = $"Phiếu đặt lịch mới #{maPhieuDL} từ khách hàng {phieuDatLich.NGUOIDUNG.HoTen} vào lúc {phieuDatLich.NgayDL:dd/MM/yyyy HH:mm}.";
                var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
                foreach (var maNV in nvIds)
                {
                    await ThongBaoNVAsync(maNV, noiDung);
                }
            }
        }

        public async Task ThongBaoXacNhanDatLichAsync(int maPhieuDL)
        {
            var phieuDatLich = await _context.PHIEUDATLICH.FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);
            if (phieuDatLich != null)
            {
                string noiDung = $"Phiếu đặt lịch #{maPhieuDL} đã được xác nhận. Chúng tôi sẽ đến thu gom vào lúc {phieuDatLich.NgayDL:dd/MM/yyyy HH:mm}.";
                await ThongBaoNDAsync(phieuDatLich.MaND, noiDung);
            }
        }

        public async Task ThongBaoHuyPhieuDatLichAsync(int maPhieuDL)
        {
            var phieuDatLich = await _context.PHIEUDATLICH.Include(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);
            if (phieuDatLich != null)
            {
                string noiDung = $"Phiếu đặt lịch #{maPhieuDL} từ khách hàng {phieuDatLich.NGUOIDUNG.HoTen} đã bị hủy.";
                var nvIds = await _context.NHANVIEN.Select(n => n.MaNV).ToListAsync();
                foreach (var maNV in nvIds)
                {
                    await ThongBaoNVAsync(maNV, noiDung);
                }
            }
        }

        public async Task ThongBaoHoanThanhThuGomAsync(int maPhieuDL, float tongDiem)
        {
            var phieuDatLich = await _context.PHIEUDATLICH.FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);
            if (phieuDatLich != null)
            {
                string noiDung = $"Phế liệu từ phiếu đặt lịch #{maPhieuDL} đã được thu gom. Bạn đã nhận được {tongDiem} điểm!";
                await ThongBaoNDAsync(phieuDatLich.MaND, noiDung);
            }
        }
        public async Task ThongBaoHuyPhieuDatLichQuaHanAsync(int maPhieuDL)
        {
            var phieuDatLich = await _context.PHIEUDATLICH
                .Include(p => p.NGUOIDUNG)
                .FirstOrDefaultAsync(p => p.MaPhieuDL == maPhieuDL);

            if (phieuDatLich != null)
            {
                string noiDung = $"Phiếu đặt lịch #{maPhieuDL} đã bị hủy tự động do quá thời gian xử lý.";
                await ThongBaoNDAsync(phieuDatLich.MaND, noiDung);
            }
        }

        public async Task DanhDauThongBaoDaDocNDAsync(int maTBND)
        {
            var tb = await _context.THONGBAOND.FindAsync(maTBND);
            if (tb != null && !tb.DaDoc)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DanhDauThongBaoDaDocNVAsync(int maTBNV)
        {
            var tb = await _context.THONGBAONV.FindAsync(maTBNV);
            if (tb != null && !tb.DaDoc)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DanhDauThongBaoDaDocNVGQAsync(int maTBNVGQ)
        {
            var tb = await _context.THONGBAONVGQ.FindAsync(maTBNVGQ);
            if (tb != null && !tb.DaDoc)
            {
                tb.DaDoc = true;
                await _context.SaveChangesAsync();
            }
        }

    }
}