document.addEventListener('DOMContentLoaded', function () {
    const passwordForm = document.getElementById('passwordChangeForm');
    const currentPasswordInput = document.getElementById('currentPassword');
    const newPasswordInput = document.getElementById('newPassword');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const submitBtn = document.getElementById('submitBtn');

    const requirements = {
        length: {
            element: document.getElementById('lengthReq'),
            test: password => password.length >= 8
        },
        uppercase: {
            element: document.getElementById('uppercaseReq'),
            test: password => /[A-Z]/.test(password)
        },
        lowercase: {
            element: document.getElementById('lowercaseReq'),
            test: password => /[a-z]/.test(password)
        },
        number: {
            element: document.getElementById('numberReq'),
            test: password => /\d/.test(password)
        },
        special: {
            element: document.getElementById('specialReq'),
            test: password => /[!@#$%^&*(),.?":{}|<>]/.test(password)
        }
    };

    function checkPasswordStrength(password) {
        let score = 0;
        Object.values(requirements).forEach(req => {
            if (req.test(password)) {
                score += 20;
                req.element.classList.add('met');
                req.element.querySelector('i').className = 'fas fa-check';
            } else {
                req.element.classList.remove('met');
                req.element.querySelector('i').className = 'fas fa-times';
            }
        });
        return score;
    }

    function updatePasswordStrength(password) {
        const strengthFill = document.getElementById('strengthFill');
        const strengthText = document.getElementById('strengthText');

        if (!password) {
            strengthFill.style.width = '0%';
            strengthFill.className = 'strength-fill';
            strengthText.textContent = 'Şifre gücü';
            strengthText.className = 'strength-text';
            return false;
        }

        const score = checkPasswordStrength(password);
        strengthFill.style.width = score + '%';

        if (score < 40) {
            strengthFill.className = 'strength-fill weak';
            strengthText.textContent = 'Zayıf';
            strengthText.className = 'strength-text weak';
        } else if (score < 60) {
            strengthFill.className = 'strength-fill fair';
            strengthText.textContent = 'Orta';
            strengthText.className = 'strength-text fair';
        } else if (score < 80) {
            strengthFill.className = 'strength-fill good';
            strengthText.textContent = 'İyi';
            strengthText.className = 'strength-text good';
        } else {
            strengthFill.className = 'strength-fill strong';
            strengthText.textContent = 'Güçlü';
            strengthText.className = 'strength-text strong';
        }

        return score === 100;
    }

    function checkPasswordMatch() {
        const matchElement = document.getElementById('passwordMatch');
        const newPassword = newPasswordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        if (!confirmPassword) {
            matchElement.textContent = '';
            matchElement.className = 'password-match';
            return false;
        }

        if (newPassword === confirmPassword) {
            matchElement.textContent = '✓ Şifreler eşleşiyor';
            matchElement.className = 'password-match match';
            return true;
        } else {
            matchElement.textContent = '✗ Şifreler eşleşmiyor';
            matchElement.className = 'password-match no-match';
            return false;
        }
    }

    function validateForm() {
        const currentPassword = currentPasswordInput.value.trim();
        const newPassword = newPasswordInput.value.trim();
        const confirmPassword = confirmPasswordInput.value.trim();

        const isStrongPassword = updatePasswordStrength(newPassword);
        const passwordsMatch = checkPasswordMatch();

        const isValid = currentPassword && newPassword && confirmPassword && isStrongPassword && passwordsMatch;

        submitBtn.disabled = !isValid;
        return isValid;
    }

    // Girişlere dinleme ekle
    currentPasswordInput.addEventListener('input', validateForm);
    newPasswordInput.addEventListener('input', () => {
        updatePasswordStrength(newPasswordInput.value);
        validateForm();
    });
    confirmPasswordInput.addEventListener('input', () => {
        checkPasswordMatch();
        validateForm();
    });

    // Sayfa açıldığında kontrol
    validateForm();
});

// Şifre göster/gizle butonu
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const icon = document.getElementById(inputId + 'Icon');

    if (input.type === 'password') {
        input.type = 'text';
        icon.className = 'fas fa-eye-slash';
    } else {
        input.type = 'password';
        icon.className = 'fas fa-eye';
    }
}
