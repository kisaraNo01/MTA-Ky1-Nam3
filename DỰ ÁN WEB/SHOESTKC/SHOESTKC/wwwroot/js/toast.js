/**
 * Modern Toast Notification System
 * Thay thế alert() cơ bản bằng toast đẹp mắt
 */

const Toast = {
    container: null,

    init() {
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.id = 'toast-container';
            this.container.className = 'toast-container';
            document.body.appendChild(this.container);
        }
    },

    show(message, type = 'info', duration = 3000) {
        this.init();

        const toast = document.createElement('div');
        toast.className = `toast toast-${type} toast-show`;

        const icon = this.getIcon(type);

        toast.innerHTML = `
            <div class="toast-icon">${icon}</div>
            <div class="toast-content">
                <div class="toast-message">${message}</div>
            </div>
            <button class="toast-close" onclick="this.parentElement.remove()">
                <i class="fas fa-times"></i>
            </button>
        `;

        this.container.appendChild(toast);

        // Auto remove
        setTimeout(() => {
            toast.classList.add('toast-hide');
            setTimeout(() => toast.remove(), 300);
        }, duration);
    },

    getIcon(type) {
        const icons = {
            success: '<i class="fas fa-check-circle"></i>',
            error: '<i class="fas fa-exclamation-circle"></i>',
            warning: '<i class="fas fa-exclamation-triangle"></i>',
            info: '<i class="fas fa-info-circle"></i>'
        };
        return icons[type] || icons.info;
    },

    success(message, duration = 3000) {
        this.show(message, 'success', duration);
    },

    error(message, duration = 4000) {
        this.show(message, 'error', duration);
    },

    warning(message, duration = 3500) {
        this.show(message, 'warning', duration);
    },

    info(message, duration = 3000) {
        this.show(message, 'info', duration);
    }
};

/**
 * Modern Confirm Dialog
 * Thay thế confirm() cơ bản
 */
const ConfirmDialog = {
    show(options) {
        return new Promise((resolve) => {
            const {
                title = 'Xác nhận',
                message = 'Bạn có chắc chắn muốn thực hiện hành động này?',
                confirmText = 'Xác nhận',
                cancelText = 'Hủy',
                type = 'warning'
            } = options;

            // Remove existing dialog
            const existing = document.getElementById('confirm-dialog');
            if (existing) existing.remove();

            const dialog = document.createElement('div');
            dialog.id = 'confirm-dialog';
            dialog.className = 'confirm-dialog-overlay';

            const icon = this.getIcon(type);

            dialog.innerHTML = `
                <div class="confirm-dialog">
                    <div class="confirm-dialog-icon confirm-dialog-icon-${type}">
                        ${icon}
                    </div>
                    <h3 class="confirm-dialog-title">${title}</h3>
                    <p class="confirm-dialog-message">${message}</p>
                    <div class="confirm-dialog-buttons">
                        <button class="btn-confirm-cancel">${cancelText}</button>
                        <button class="btn-confirm-ok btn-confirm-${type}">${confirmText}</button>
                    </div>
                </div>
            `;

            document.body.appendChild(dialog);

            // Add animation
            setTimeout(() => dialog.classList.add('show'), 10);

            const btnOk = dialog.querySelector('.btn-confirm-ok');
            const btnCancel = dialog.querySelector('.btn-confirm-cancel');

            btnOk.onclick = () => {
                dialog.classList.remove('show');
                setTimeout(() => {
                    dialog.remove();
                    resolve(true);
                }, 300);
            };

            btnCancel.onclick = () => {
                dialog.classList.remove('show');
                setTimeout(() => {
                    dialog.remove();
                    resolve(false);
                }, 300);
            };

            // Click outside to cancel
            dialog.onclick = (e) => {
                if (e.target === dialog) {
                    btnCancel.click();
                }
            };
        });
    },

    getIcon(type) {
        const icons = {
            danger: '<i class="fas fa-exclamation-triangle"></i>',
            warning: '<i class="fas fa-exclamation-circle"></i>',
            info: '<i class="fas fa-info-circle"></i>',
            success: '<i class="fas fa-check-circle"></i>'
        };
        return icons[type] || icons.warning;
    }
};

/**
 * Loading Overlay
 * Hiển thị loading khi đang xử lý
 */
const Loading = {
    overlay: null,

    show(message = 'Đang xử lý...') {
        this.hide(); // Remove existing

        this.overlay = document.createElement('div');
        this.overlay.id = 'loading-overlay';
        this.overlay.className = 'loading-overlay';
        this.overlay.innerHTML = `
            <div class="loading-spinner">
                <div class="spinner"></div>
                <p class="loading-message">${message}</p>
            </div>
        `;

        document.body.appendChild(this.overlay);
        setTimeout(() => this.overlay.classList.add('show'), 10);
    },

    hide() {
        if (this.overlay) {
            this.overlay.classList.remove('show');
            setTimeout(() => {
                if (this.overlay) {
                    this.overlay.remove();
                    this.overlay = null;
                }
            }, 300);
        }
    }
};

/**
 * Button Loading State
 * Thêm loading vào button khi click
 */
function setButtonLoading(button, loading = true) {
    if (loading) {
        button.disabled = true;
        button.dataset.originalHtml = button.innerHTML;
        button.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
        button.classList.add('btn-loading');
    } else {
        button.disabled = false;
        if (button.dataset.originalHtml) {
            button.innerHTML = button.dataset.originalHtml;
        }
        button.classList.remove('btn-loading');
    }
}

// Export to global
window.Toast = Toast;
window.ConfirmDialog = ConfirmDialog;
window.Loading = Loading;
window.setButtonLoading = setButtonLoading;
