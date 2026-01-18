/**
 * Chi Tiết Sản Phẩm - UX Enhanced
 * File này chứa logic cho trang chi tiết sản phẩm với UX cải tiến
 */

// Biến toàn cục cho trang chi tiết
let selectedColor = null;
let selectedSize = null;
let selectedVariant = null;
let basePrice = 0;
let variants = [];

// Khởi tạo trang chi tiết sản phẩm
function initProductDetail(productVariants, productBasePrice) {
    variants = productVariants;
    basePrice = productBasePrice;

    const colorRadios = document.querySelectorAll('input[name="selectedColor"]');
    const sizeRadios = document.querySelectorAll('input[name="selectedSize"]');

    // Xử lý khi chọn màu sắc
    colorRadios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            selectedColor = e.target.value;
            document.getElementById('selectedColorText').innerText = `- ${selectedColor}`;
            updateAvailableSizes();

            // Reset size khi đổi màu
            selectedSize = null;
            document.getElementById('selectedSizeText').innerText = '';
            sizeRadios.forEach(r => r.checked = false);
            checkSelection();
        });
    });

    // Xử lý khi chọn size
    sizeRadios.forEach(radio => {
        radio.addEventListener('change', (e) => {
            selectedSize = e.target.value;
            document.getElementById('selectedSizeText').innerText = `- ${selectedSize}`;
            checkSelection();
        });
    });
}

// Cập nhật các size khả dụng dựa trên màu đã chọn
function updateAvailableSizes() {
    const sizeRadios = document.querySelectorAll('input[name="selectedSize"]');
    const availableSizes = variants
        .filter(v => v.mausac === selectedColor)
        .map(v => v.size);

    sizeRadios.forEach(radio => {
        const sizeOption = radio.closest('.variant-option-size');
        if (availableSizes.includes(radio.value)) {
            sizeOption.classList.remove('disabled');
            radio.disabled = false;
        } else {
            sizeOption.classList.add('disabled');
            radio.disabled = true;
            radio.checked = false;
        }
    });
}

// Kiểm tra điều kiện để mở khóa nút mua hàng
function checkSelection() {
    const qtySelector = document.querySelector('.quantity-selector');
    const qtyButtons = document.querySelectorAll('.quantity-selector .qty-btn');
    const qtyInput = document.getElementById('SoLuong');
    const btnAddToCart = document.getElementById('btnAddToCart');
    const btnBuyNow = document.getElementById('btnBuyNow');
    const variantInfo = document.getElementById('variant-info');
    const productPrice = document.getElementById('productPrice');
    const priceVariation = document.getElementById('price-variation');

    if (selectedColor && selectedSize) {
        selectedVariant = variants.find(v => v.mausac === selectedColor && v.size === selectedSize);

        if (selectedVariant) {
            qtySelector.classList.remove('disabled');
            qtyButtons.forEach(b => b.disabled = false);
            qtyInput.value = 1;

            btnAddToCart.disabled = false;
            btnAddToCart.innerHTML = '<i class="fas fa-cart-plus"></i> Thêm vào giỏ hàng';

            btnBuyNow.disabled = false;

            // Cập nhật giá theo biến thể
            const finalPrice = basePrice + selectedVariant.chenhLechGia;
            productPrice.innerText = finalPrice.toLocaleString('vi-VN') + '₫';

            if (selectedVariant.chenhLechGia > 0) {
                priceVariation.innerText = `+${selectedVariant.chenhLechGia.toLocaleString('vi-VN')}₫`;
                priceVariation.className = 'text-warning fw-bold';
            } else {
                priceVariation.innerText = '';
            }

            variantInfo.innerHTML = `✅ Bạn đã chọn: <strong>Size ${selectedVariant.size} - ${selectedVariant.mausac}</strong>. (Còn ${selectedVariant.soLuongTon} sản phẩm)`;
            variantInfo.style.display = 'block';
        } else {
            resetControls();
        }
    } else {
        resetControls();
    }
}

function resetControls() {
    const qtySelector = document.querySelector('.quantity-selector');
    const qtyButtons = document.querySelectorAll('.quantity-selector .qty-btn');
    const btnAddToCart = document.getElementById('btnAddToCart');
    const btnBuyNow = document.getElementById('btnBuyNow');
    const variantInfo = document.getElementById('variant-info');
    const productPrice = document.getElementById('productPrice');
    const priceVariation = document.getElementById('price-variation');

    selectedVariant = null;
    qtySelector.classList.add('disabled');
    qtyButtons.forEach(b => b.disabled = true);
    btnAddToCart.disabled = true;
    btnAddToCart.innerHTML = '<i class="fas fa-cart-plus"></i> Chọn Size và Màu để thêm';
    btnBuyNow.disabled = true;
    variantInfo.style.display = 'none';
    productPrice.innerText = basePrice.toLocaleString('vi-VN') + '₫';
    priceVariation.innerText = '';
}

// Thay đổi số lượng
function changeQty(change) {
    const qtyInput = document.getElementById('SoLuong');
    let newQty = parseInt(qtyInput.value) + change;
    const maxStock = selectedVariant ? selectedVariant.soLuongTon : 1;

    if (newQty < 1) newQty = 1;
    if (newQty > maxStock) {
        newQty = maxStock;
        Toast.warning('Số lượng đã đạt tới tối đa trong kho!');
    }
    qtyInput.value = newQty;
}

// Gọi AJAX thêm vào giỏ hàng (CHI TIẾT SẢN PHẨM)
function addToCart() {
    if (!selectedVariant) {
        Toast.warning('Vui lòng chọn màu sắc và size!');
        return;
    }

    const qtyInput = document.getElementById('SoLuong');
    Loading.show('Đang thêm vào giỏ hàng...');

    const params = new URLSearchParams();
    params.append('bienTheId', selectedVariant.id);
    params.append('soLuong', parseInt(qtyInput.value));

    fetch('/GioHang/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
        .then(response => response.json())
        .then(data => {
            Loading.hide();
            if (data.success) {
                Toast.success(data.message);
                if (data.cartCount) {
                    const badge = document.getElementById('cartCount');
                    if (badge) {
                        badge.textContent = data.cartCount;
                        badge.style.display = 'inline-block';
                    }
                }
            } else {
                if (data.requireLogin) {
                    ConfirmDialog.show({
                        title: 'Yêu cầu đăng nhập',
                        message: data.message + '\nBạn có muốn đăng nhập không?',
                        confirmText: 'Đăng nhập',
                        type: 'info'
                    }).then(confirmed => {
                        if (confirmed) {
                            window.location.href = '/Auth/Login';
                        }
                    });
                } else {
                    Toast.error(data.message);
                }
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra trong quá trình thêm vào giỏ hàng!');
            console.error('Error:', error);
        });
}

// Hàm Mua ngay: Thêm vào giỏ hàng và chuyển hướng đến trang thanh toán
function buyNow() {
    if (!selectedVariant) {
        Toast.warning('Vui lòng chọn màu sắc và size!');
        return;
    }

    const qtyInput = document.getElementById('SoLuong');
    Loading.show('Đang xử lý...');

    const params = new URLSearchParams();
    params.append('bienTheId', selectedVariant.id);
    params.append('soLuong', parseInt(qtyInput.value));

    fetch('/GioHang/Add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Chuyển hướng đến trang thanh toán (loading sẽ tự hide khi chuyển trang)
                window.location.href = '/Order/Checkout';
            } else {
                Loading.hide();
                if (data.requireLogin) {
                    ConfirmDialog.show({
                        title: 'Yêu cầu đăng nhập',
                        message: data.message + '\nBạn có muốn đăng nhập không?',
                        confirmText: 'Đăng nhập',
                        type: 'info'
                    }).then(confirmed => {
                        if (confirmed) {
                            window.location.href = '/Auth/Login';
                        }
                    });
                } else {
                    Toast.error(data.message);
                }
            }
        })
        .catch(error => {
            Loading.hide();
            Toast.error('Có lỗi xảy ra trong quá trình xử lý!');
            console.error('Error:', error);
        });
}

console.log('✅ Product Detail UX loaded!');
