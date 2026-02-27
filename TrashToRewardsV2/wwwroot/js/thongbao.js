// Kiểm tra thông báo mới
function kiemTraThongBaoMoi() {
    const loaiNguoiDungElement = document.getElementById('loaiNguoiDung');
    const maElement = document.getElementById('ma');
    if (!loaiNguoiDungElement || !maElement) return;

    const loaiNguoiDung = loaiNguoiDungElement.value;
    const ma = maElement.value;
    if (!loaiNguoiDung || !ma) return;

    fetch(`/api/ThongBaoAPI/KiemTraThongBaoMoi?loaiNguoiDung=${loaiNguoiDung}&ma=${ma}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const alertsDropdown = document.getElementById('alertsDropdown');
            if (!alertsDropdown) return;

            // Tìm badge hiện tại hoặc tạo mới nếu chưa có
            let badgeElement = alertsDropdown.querySelector('.badge-counter');

            if (data > 0) {
                if (badgeElement) {
                    badgeElement.textContent = data > 99 ? '99+' : data;
                    badgeElement.style.display = 'inline-block';
                } else {
                    const newBadge = document.createElement('span');
                    newBadge.className = 'position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger badge-counter';
                    newBadge.textContent = data > 99 ? '99+' : data;
                    alertsDropdown.appendChild(newBadge);
                }
            } else if (badgeElement) {
                badgeElement.style.display = 'none';
            }
        })
        .catch(error => {
            console.error('Error checking notifications:', error);
        });
}

// Thêm hidden inputs vào layout
function addHiddenInputs() {
    const isLoggedIn = document.querySelector('meta[name="isLoggedIn"]')?.content === 'true';
    if (!isLoggedIn) return;

    const loaiNguoiDung = document.querySelector('meta[name="userRole"]')?.content;
    const ma = document.querySelector('meta[name="userId"]')?.content;
    if (!loaiNguoiDung || !ma) return;

    if (document.getElementById('loaiNguoiDung') && document.getElementById('ma')) return;

    const hiddenInputsDiv = document.createElement('div');
    hiddenInputsDiv.style.display = 'none';

    const loaiNguoiDungInput = document.createElement('input');
    loaiNguoiDungInput.id = 'loaiNguoiDung';
    loaiNguoiDungInput.type = 'hidden';
    loaiNguoiDungInput.value = loaiNguoiDung;

    const maInput = document.createElement('input');
    maInput.id = 'ma';
    maInput.type = 'hidden';
    maInput.value = ma;

    hiddenInputsDiv.appendChild(loaiNguoiDungInput);
    hiddenInputsDiv.appendChild(maInput);
    document.body.appendChild(hiddenInputsDiv);
}

// Cập nhật danh sách thông báo trong dropdown
function capNhatDanhSachThongBao() {
    const loaiNguoiDung = document.getElementById('loaiNguoiDung')?.value;
    const ma = document.getElementById('ma')?.value;
    if (!loaiNguoiDung || !ma) return;

    // Có thể mở rộng logic ở đây để gọi API cập nhật danh sách mới
}

// Khởi tạo và thiết lập kiểm tra định kỳ
document.addEventListener('DOMContentLoaded', function () {
    addHiddenInputs();
    kiemTraThongBaoMoi();

    setInterval(kiemTraThongBaoMoi, 30000); // Kiểm tra mỗi 30 giây

    const alertsDropdown = document.getElementById('alertsDropdown');
    if (alertsDropdown) {
        alertsDropdown.addEventListener('click', function () {
            capNhatDanhSachThongBao();
        });
    }
});
document.querySelectorAll(".btn-mark-read-nd").forEach(btn => {
    btn.addEventListener("click", function () {
        const id = this.getAttribute("data-id");
        fetch(`/api/ThongBaoAPI/DanhDauDocND/${id}`, {
            method: "POST",
            headers: {
                'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
            }
        }).then(res => {
            if (res.ok) location.reload();
        });
    });
});
document.querySelectorAll(".btn-danh-dau-doc-nv").forEach(button => {
    button.addEventListener("click", async function () {
        const id = this.getAttribute("data-id");
        const response = await fetch(`/api/ThongBaoAPI/DanhDauDocNV/${id}`, {
            method: "POST"
        });
        if (response.ok) {
            const item = this.closest(".list-group-item");
            item.classList.remove("fw-bold");
            item.querySelector(".badge")?.remove();
            this.remove();
        }
    });
});


