/* ---------- 1. Elemanlar ---------- */
const inputs = [...document.querySelectorAll('.kod-kutu')];
const form = document.getElementById('verificationForm');
const verifyBtn = document.getElementById('verifyBtn');
const codeHidden = document.getElementById('codeInput');  // <input type="hidden" asp-for="Code">


/* ---------- 2. Kod giriş alanlarının davranışı ---------- */
inputs.forEach((input, idx) => {
    // Sadece rakam girilsin
    input.addEventListener('input', e => {
        e.target.value = e.target.value.replace(/[^0-9]/g, '');
        if (e.target.value && idx < inputs.length - 1) inputs[idx + 1].focus();
        updateVerifyButton();
    });

    // Backspace’te geri odak
    input.addEventListener('keydown', e => {
        if (e.key === 'Backspace' && !input.value && idx > 0) inputs[idx - 1].focus();
    });

    // Yapıştırma
    input.addEventListener('paste', e => {
        e.preventDefault();
        const paste = e.clipboardData.getData('text').replace(/[^0-9]/g, '').slice(0, 6);
        paste.split('').forEach((char, i) => {
            if (inputs[i]) inputs[i].value = char;
        });
        updateVerifyButton();
    });
});

/* ---------- 3. Form gönderme ---------- */
form.addEventListener('submit', e => {
    const code = inputs.map(i => i.value).join('');
    if (code.length !== 6) {
        e.preventDefault();            // kod eksik → gönderme
        alert('Lütfen 6 haneli kodu giriniz');
        return;
    }
    codeHidden.value = code;         // gizli input’a set et
    // Form normal şekilde Razor POST’a gider
});

/* ---------- 4. Yeniden gönder (isteğe bağlı) ---------- */


/* ---------- 5. Yardımcı ---------- */
function updateVerifyButton() {
    verifyBtn.disabled = inputs.some(i => i.value === '');
}


inputs[0].focus();