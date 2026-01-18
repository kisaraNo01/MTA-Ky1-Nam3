// Price Validation for Search Forms
// Validates price inputs and provides clear error messages

(function () {
    'use strict';

    // Configuration
    const CONFIG = {
        MIN_PRICE: 0,
        MAX_PRICE: 100000000, // 100 triệu VNĐ
        ERROR_DISPLAY_TIME: 5000 // 5 seconds
    };

    // Initialize validation when DOM is ready
    document.addEventListener('DOMContentLoaded', function () {
        initializePriceValidation();
    });

    function initializePriceValidation() {
        // Find all forms with price inputs
        const forms = document.querySelectorAll('form');

        forms.forEach(form => {
            const giaMinInput = form.querySelector('input[name="giaMin"]');
            const giaMaxInput = form.querySelector('input[name="giaMax"]');

            if (giaMinInput || giaMaxInput) {
                setupPriceValidation(form, giaMinInput, giaMaxInput);
            }
        });
    }

    function setupPriceValidation(form, giaMinInput, giaMaxInput) {
        // Add validation on submit
        form.addEventListener('submit', function (e) {
            if (!validatePriceInputs(giaMinInput, giaMaxInput)) {
                e.preventDefault();
                return false;
            }
        });

        // Add real-time validation
        if (giaMinInput) {
            addInputValidation(giaMinInput, 'Giá từ');
        }
        if (giaMaxInput) {
            addInputValidation(giaMaxInput, 'Giá đến');
        }

        // Add cross-field validation on blur
        if (giaMinInput && giaMaxInput) {
            giaMinInput.addEventListener('blur', () => validatePriceRange(giaMinInput, giaMaxInput));
            giaMaxInput.addEventListener('blur', () => validatePriceRange(giaMinInput, giaMaxInput));
        }
    }

    function addInputValidation(input, fieldName) {
        // Remove any existing error display
        const existingError = input.parentElement.querySelector('.price-error-message');
        if (existingError) {
            existingError.remove();
        }

        // Add input event listener for real-time validation
        input.addEventListener('input', function () {
            clearError(input);

            const value = input.value.trim();

            // Allow empty values
            if (value === '') {
                return;
            }

            // Check if it's a valid number
            if (!/^\d+$/.test(value)) {
                showError(input, `${fieldName} phải là số nguyên dương. Ví dụ: 100000, 500000, 1000000`);
                return;
            }

            const numValue = parseInt(value, 10);

            // Check minimum value
            if (numValue < CONFIG.MIN_PRICE) {
                showError(input, `${fieldName} phải lớn hơn hoặc bằng ${formatPrice(CONFIG.MIN_PRICE)} VNĐ`);
                return;
            }

            // Check maximum value
            if (numValue > CONFIG.MAX_PRICE) {
                showError(input, `${fieldName} không được vượt quá ${formatPrice(CONFIG.MAX_PRICE)} VNĐ`);
                return;
            }
        });

        // Add blur event to format the number
        input.addEventListener('blur', function () {
            const value = input.value.trim();
            if (value !== '' && /^\d+$/.test(value)) {
                // Keep the raw number, don't format with commas in the input
                // This ensures the form submission works correctly
                input.value = parseInt(value, 10).toString();
            }
        });
    }

    function validatePriceRange(giaMinInput, giaMaxInput) {
        const minValue = giaMinInput.value.trim();
        const maxValue = giaMaxInput.value.trim();

        // If both are empty, no validation needed
        if (minValue === '' && maxValue === '') {
            return true;
        }

        // If only one is filled, that's okay
        if (minValue === '' || maxValue === '') {
            return true;
        }

        // Both are filled, check if they're valid numbers
        if (!/^\d+$/.test(minValue) || !/^\d+$/.test(maxValue)) {
            return false;
        }

        const min = parseInt(minValue, 10);
        const max = parseInt(maxValue, 10);

        // Check if min is greater than max
        if (min > max) {
            showError(giaMaxInput, `Giá đến (${formatPrice(max)} VNĐ) phải lớn hơn hoặc bằng Giá từ (${formatPrice(min)} VNĐ)`);
            return false;
        }

        // Clear any existing errors if validation passes
        clearError(giaMinInput);
        clearError(giaMaxInput);
        return true;
    }

    function validatePriceInputs(giaMinInput, giaMaxInput) {
        let isValid = true;

        // Validate giaMin
        if (giaMinInput) {
            const minValue = giaMinInput.value.trim();
            if (minValue !== '') {
                if (!/^\d+$/.test(minValue)) {
                    showError(giaMinInput, 'Giá từ phải là số nguyên dương. Ví dụ: 100000, 500000, 1000000');
                    isValid = false;
                } else {
                    const min = parseInt(minValue, 10);
                    if (min < CONFIG.MIN_PRICE) {
                        showError(giaMinInput, `Giá từ phải lớn hơn hoặc bằng ${formatPrice(CONFIG.MIN_PRICE)} VNĐ`);
                        isValid = false;
                    } else if (min > CONFIG.MAX_PRICE) {
                        showError(giaMinInput, `Giá từ không được vượt quá ${formatPrice(CONFIG.MAX_PRICE)} VNĐ`);
                        isValid = false;
                    }
                }
            }
        }

        // Validate giaMax
        if (giaMaxInput) {
            const maxValue = giaMaxInput.value.trim();
            if (maxValue !== '') {
                if (!/^\d+$/.test(maxValue)) {
                    showError(giaMaxInput, 'Giá đến phải là số nguyên dương. Ví dụ: 100000, 500000, 1000000');
                    isValid = false;
                } else {
                    const max = parseInt(maxValue, 10);
                    if (max < CONFIG.MIN_PRICE) {
                        showError(giaMaxInput, `Giá đến phải lớn hơn hoặc bằng ${formatPrice(CONFIG.MIN_PRICE)} VNĐ`);
                        isValid = false;
                    } else if (max > CONFIG.MAX_PRICE) {
                        showError(giaMaxInput, `Giá đến không được vượt quá ${formatPrice(CONFIG.MAX_PRICE)} VNĐ`);
                        isValid = false;
                    }
                }
            }
        }

        // Validate range if both inputs exist and are valid
        if (isValid && giaMinInput && giaMaxInput) {
            isValid = validatePriceRange(giaMinInput, giaMaxInput);
        }

        // Show toast notification if validation fails
        if (!isValid && typeof showToast === 'function') {
            showToast('Vui lòng kiểm tra lại thông tin giá nhập vào', 'error');
        }

        return isValid;
    }

    function showError(input, message) {
        // Clear any existing error
        clearError(input);

        // Add error class to input
        input.classList.add('is-invalid');

        // Create error message element
        const errorDiv = document.createElement('div');
        errorDiv.className = 'price-error-message invalid-feedback d-block';
        errorDiv.innerHTML = `<i class="fas fa-exclamation-circle"></i> ${message}`;

        // Insert error message after input
        input.parentElement.appendChild(errorDiv);

        // Focus on the input
        input.focus();
    }

    function clearError(input) {
        // Remove error class
        input.classList.remove('is-invalid');

        // Remove error message
        const errorDiv = input.parentElement.querySelector('.price-error-message');
        if (errorDiv) {
            errorDiv.remove();
        }
    }

    function formatPrice(price) {
        return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }

    // Expose validation function globally for manual use
    window.validatePriceInputs = validatePriceInputs;
})();
