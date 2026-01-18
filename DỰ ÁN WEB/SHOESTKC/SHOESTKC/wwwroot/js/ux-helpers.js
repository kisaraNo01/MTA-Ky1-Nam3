/**
 * UX Helper Functions
 * Các hàm helper đã được cập nhật để sử dụng Toast, Loading và ConfirmDialog
 */

// ===== GIỎ HÀNG =====

// Cập nhật số lượng sản phẩm trong giỏ hàng
window.updateQuantity = function (id, newQty) {
    if (newQty < 1) return;

    Loading.show('Đang cập nhật...');

    let formData = new FormData();
    formData.append('id', id);
    formData.append('SoLuong', newQty);

    fetch('/GioHang/UpdateQuantity', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success('Cập nhật thành công');
                setTimeout(() => location.reload(), 500);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi cập nhật giỏ hàng.');
            console.error('Error:', error);
        });
};

// Xóa sản phẩm khỏi giỏ hàng
window.removeItem = async function (id) {
    const confirmed = await ConfirmDialog.show({
        title: 'Xóa sản phẩm',
        message: 'Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?',
        confirmText: 'Xóa',
        cancelText: 'Hủy',
        type: 'warning'
    });

    if (!confirmed) return;

    Loading.show('Đang xóa...');

    let formData = new FormData();
    formData.append('id', id);

    fetch('/GioHang/Remove', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 500);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra.');
            console.error('Error:', error);
        });
};

// Xóa toàn bộ giỏ hàng
window.clearCart = async function () {
    const confirmed = await ConfirmDialog.show({
        title: 'Xóa toàn bộ giỏ hàng',
        message: 'Bạn có chắc muốn xóa toàn bộ sản phẩm trong giỏ hàng?',
        confirmText: 'Xóa hết',
        cancelText: 'Hủy',
        type: 'danger'
    });

    if (!confirmed) return;

    Loading.show('Đang xóa...');

    fetch('/GioHang/Clear', {
        method: 'POST',
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 500);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra.');
            console.error('Error:', error);
        });
};

// ===== ĐƠN HÀNG =====

// Hủy đơn hàng
window.cancelOrder = async function (orderId) {
    const confirmed = await ConfirmDialog.show({
        title: 'Hủy đơn hàng',
        message: 'Bạn có chắc chắn muốn hủy đơn hàng này?',
        confirmText: 'Hủy đơn',
        cancelText: 'Quay lại',
        type: 'warning'
    });

    if (!confirmed) return;

    Loading.show('Đang hủy đơn hàng...');

    fetch(`/Order/Cancel/${orderId}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 1000);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi hủy đơn hàng.');
            console.error('Error:', error);
        });
};

// ===== ADMIN - SẢN PHẨM =====

// Xóa sản phẩm (Admin)
window.deleteSanPham = async function (id, trangThai) {
    const isVisible = trangThai === 'true';

    const confirmed = await ConfirmDialog.show({
        title: isVisible ? 'Ẩn sản phẩm' : 'Xóa sản phẩm',
        message: isVisible
            ? 'Sản phẩm đang hiển thị. Bạn muốn ẩn sản phẩm này?'
            : 'Sản phẩm đã ẩn. Bạn có muốn XÓA VĨNH VIỄN sản phẩm này không?',
        confirmText: isVisible ? 'Ẩn' : 'Xóa vĩnh viễn',
        cancelText: 'Hủy',
        type: isVisible ? 'warning' : 'danger'
    });

    if (!confirmed) return;

    Loading.show(isVisible ? 'Đang ẩn sản phẩm...' : 'Đang xóa sản phẩm...');

    fetch(`/SanPham/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 1000);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi xóa sản phẩm.');
            console.error('Error:', error);
        });
};

// ===== ADMIN - DANH MỤC =====

// Xóa danh mục
window.deleteCategory = async function (id) {
    const confirmed = await ConfirmDialog.show({
        title: 'Xóa danh mục',
        message: 'Bạn có chắc chắn muốn xóa danh mục này? Hành động này không thể hoàn tác.',
        confirmText: 'Xóa',
        cancelText: 'Hủy',
        type: 'danger'
    });

    if (!confirmed) return;

    Loading.show('Đang xóa danh mục...');

    fetch(`/Category/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 1000);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi xóa danh mục.');
            console.error('Error:', error);
        });
};

// ===== ADMIN - MÃ KHUYẾN MÃI =====

// Xóa mã khuyến mãi
window.deleteVoucher = async function (id) {
    const confirmed = await ConfirmDialog.show({
        title: 'Xóa mã khuyến mãi',
        message: 'Bạn có chắc chắn muốn xóa mã khuyến mãi này?',
        confirmText: 'Xóa',
        cancelText: 'Hủy',
        type: 'danger'
    });

    if (!confirmed) return;

    Loading.show('Đang xóa mã khuyến mãi...');

    fetch(`/MaKhuyenMai/Delete/${id}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 1000);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi xóa mã khuyến mãi.');
            console.error('Error:', error);
        });
};

// ===== ADMIN - ĐƠN HÀNG =====

// Cập nhật trạng thái đơn hàng
window.updateStatus = async function (orderId) {
    const confirmed = await ConfirmDialog.show({
        title: 'Cập nhật trạng thái',
        message: 'Bạn có chắc chắn muốn cập nhật trạng thái đơn hàng này?',
        confirmText: 'Cập nhật',
        cancelText: 'Hủy',
        type: 'info'
    });

    if (!confirmed) return;

    Loading.show('Đang cập nhật...');

    const trangThaiDonHang = document.getElementById('TrangThaiDonHang').value;
    const trangThaiThanhToan = document.getElementById('TrangThaiThanhToan').value;

    const formData = new FormData();
    formData.append('id', orderId);
    formData.append('TrangThaiDonHang', trangThaiDonHang);
    formData.append('TrangThaiThanhToan', trangThaiThanhToan);

    fetch('/Admin/UpdateOrderStatus', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                setTimeout(() => location.reload(), 1000);
            } else {
                Toast.error(data.message);
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi cập nhật trạng thái.');
            console.error('Error:', error);
        });
};

// ===== THANH TOÁN =====

// Áp dụng mã khuyến mãi
window.applyPromoCode = function () {
    const promoCode = document.getElementById('promoCodeInput').value.trim();
    if (!promoCode) {
        Toast.warning('Vui lòng nhập mã khuyến mãi!');
        return;
    }

    Loading.show('Đang kiểm tra mã...');

    let formData = new FormData();
    formData.append('maCode', promoCode);

    fetch('/Order/ApplyPromoCode', {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                document.getElementById('maKhuyenMaiId').value = data.promoId;

                // Cập nhật UI
                document.getElementById('tamTinh').innerText = formatCurrency(data.tongTien);
                document.getElementById('phiVanChuyen').innerText = formatCurrency(data.phiVanChuyen);
                document.getElementById('tienGiam').innerText = '-' + formatCurrency(data.tienGiam);
                document.getElementById('tongThanhToan').innerText = formatCurrency(data.tongThanhToan);

                // Hiện dòng giảm giá
                document.getElementById('discountRow').style.display = 'flex';

                Toast.success(`✓ ${data.message} Giảm ${formatCurrency(data.tienGiam)}`);
            } else {
                Toast.error(data.message);
                resetPromo();
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra khi áp dụng mã!');
            console.error('Error:', error);
        });
};

// Format currency helper
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN').format(amount) + 'đ';
}

// Reset promo
function resetPromo() {
    document.getElementById('maKhuyenMaiId').value = '';
    document.getElementById('discountRow').style.display = 'none';
}

console.log('✅ UX Helpers loaded successfully!');
