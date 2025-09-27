$(document).ready(function () {
    const passwordInput = document.getElementById('Password');
    const confirmPasswordInput = document.getElementById('ConfirmPassword');
    const strengthMeter = document.getElementById('strengthMeter');
    const strengthText = document.getElementById('strengthText');
    const passwordMatch = document.getElementById('passwordMatch');
    const passwordRequirements = document.getElementById('passwordRequirements');
    const requirements = {
        length: document.getElementById('length'),
        uppercase: document.getElementById('uppercase'),
        lowercase: document.getElementById('lowercase'),
        number: document.getElementById('number'),
        special: document.getElementById('special')
    };

    // Şifre gereksinimlerini göster/gizle
    passwordInput.addEventListener('focus', () => {
        passwordRequirements.style.display = 'block';
    });
    passwordInput.addEventListener('blur', () => {
        passwordRequirements.style.display = 'none';
    });

    // Şifre gereksinimleri kontrolü
    function checkPasswordRequirements(password) {
        const checks = {
            length: password.length >= 8,
            uppercase: /[A-Z]/.test(password),
            lowercase: /[a-z]/.test(password),
            number: /[0-9]/.test(password),
            special: /[@$!%*?&]/.test(password)
        };

        Object.keys(checks).forEach(req => {
            requirements[req].innerHTML = checks[req]
                ? `✓ ${requirements[req].textContent.slice(2)}`
                : `✗ ${requirements[req].textContent.slice(2)}`;
            requirements[req].className = checks[req] ? 'valid' : '';
        });

        return Object.values(checks).filter(Boolean).length;
    }

    // Şifre gücü göstergesi
    function checkPasswordStrength(password) {
        const strength = checkPasswordRequirements(password);
        const strengthMap = {
            0: { text: 'Çok Zayıf', color: '#dc2626', width: '20%' },
            1: { text: 'Zayıf', color: '#ea580c', width: '40%' },
            2: { text: 'Orta', color: '#d97706', width: '60%' },
            3: { text: 'İyi', color: '#65a30d', width: '80%' },
            4: { text: 'Güçlü', color: '#16a34a', width: '90%' },
            5: { text: 'Çok Güçlü', color: '#059669', width: '100%' }
        };

        const result = strengthMap[strength];
        strengthMeter.style.width = result.width;
        strengthMeter.style.backgroundColor = result.color;
        strengthText.textContent = `Şifre Gücü: ${result.text}`;
        strengthText.style.color = result.color;
    }

    // Şifre eşleşme kontrolü
    function checkPasswordMatch() {
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;

        if (confirmPassword) {
            if (password === confirmPassword) {
                passwordMatch.textContent = 'Şifreler eşleşiyor';
                passwordMatch.className = 'password-match success';
            } else {
                passwordMatch.textContent = 'Şifreler eşleşmiyor';
                passwordMatch.className = 'password-match error';
            }
        } else {
            passwordMatch.textContent = '';
        }
    }

    // Göz ikonu ile şifre göster/gizle
    window.togglePassword = function (inputId) {
        const input = document.getElementById(inputId);
        const button = input.nextElementSibling;
        const icon = button.querySelector('i');

        if (input.type === 'password') {
            input.type = 'text';
            icon.classList.remove('fa-eye');
            icon.classList.add('fa-eye-slash');
        } else {
            input.type = 'password';
            icon.classList.remove('fa-eye-slash');
            icon.classList.add('fa-eye');
        }
    };

    // Basit toast bildirimi
    function showNotification(message, type) {
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;

        const icon = document.createElement('i');
        icon.className = type === 'success' ? 'fas fa-check-circle' : 'fas fa-exclamation-circle';

        const text = document.createElement('span');
        text.textContent = message;

        notification.appendChild(icon);
        notification.appendChild(text);
        document.body.appendChild(notification);

        setTimeout(() => notification.classList.add('show'), 100);

        setTimeout(() => {
            notification.classList.remove('show');
            setTimeout(() => notification.remove(), 500);
        }, 3000);
    }

    // jQuery Validate custom method: Şifre kuralları
    $.validator.addMethod("pwcheck", function (value) {
        return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(value);
    }, "Şifre en az 8 karakter, 1 büyük harf, 1 küçük harf, 1 sayı ve 1 özel karakter içermelidir.");

    if (!$("#signupForm").data('validator')) {
        try {
            $("#signupForm").validate({
                ignore: ":hidden",
                rules: {
                    FirstName: "required",
                    LastName: "required",
                    PhoneNumber: "required",
                    Email: {
                        required: true,
                        email: true
                    },
                    Password: {
                        required: true,
                        pwcheck: true
                    },
                    ConfirmPassword: {
                        required: true,
                        equalTo: "#Password"
                    },
                    TermsAccepted: "required"  
                },
                messages: {
                    FirstName: "Adınızı giriniz",
                    LastName: "Soyadınızı giriniz",
                    PhoneNumber: "Telefon numaranızı giriniz",
                    Email: {
                        required: "E-posta giriniz",
                        email: "Geçerli bir e-posta giriniz"
                    },
                    Password: {
                        required: "Şifre giriniz"
                    },
                    ConfirmPassword: {
                        required: "Şifre tekrarı giriniz",
                        equalTo: "Şifreler eşleşmiyor"
                    },
                    TermsAccepted: "Kullanım şartlarını kabul etmelisiniz"
                },
                submitHandler: function (form) {
                    // işlemler...
                    form.submit();
                }
            });
        } catch (error) {
            console.error("Validate hata:", error);
        }
    }

    // Input eventleri: şifre gücü ve eşleşme kontrolü
    passwordInput.addEventListener('input', () => {
        checkPasswordStrength(passwordInput.value);
        checkPasswordMatch();
    });

    confirmPasswordInput.addEventListener('input', checkPasswordMatch);
});
