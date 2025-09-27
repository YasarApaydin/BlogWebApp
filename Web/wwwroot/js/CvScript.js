// Global variables
let experienceCount = 0;
let educationCount = 0;
let skillCount = 0;
let languageCount = 0;

// Photo upload handler
function handlePhotoUpload(event) {
    const file = event.target.files[0];
    if (file) {
        const reader = new FileReader();
        const preview = document.getElementById('photoPreview');
        const placeholder = document.getElementById('photoPlaceholder');
        const cvPhoto = document.getElementById('cv-profile-photo');

        // Add loading state
        document.querySelector('.cv-photo-preview').classList.add('loading');

        reader.onload = function (e) {
            // Remove loading state
            document.querySelector('.cv-photo-preview').classList.remove('loading');

            // Update form preview
            preview.src = e.target.result;
            preview.style.display = 'block';
            placeholder.style.display = 'none';

            // Update CV preview
            cvPhoto.innerHTML = `<img src="${e.target.result}" alt="Profile Photo" style="width: 100%; height: 100%; object-fit: cover;">`;
        };

        reader.readAsDataURL(file);
    }
}

// Update CV preview
function updatePreview() {
    const fields = ['fullName', 'jobTitle', 'email', 'phone', 'address', 'website', 'summary'];

    fields.forEach(field => {
        const input = document.getElementById(field);
        const preview = document.getElementById(`preview-${field === 'fullName' ? 'name' : field === 'jobTitle' ? 'title' : field}`);

        if (input && preview) {
            const value = input.value.trim();
            if (value) {
                preview.textContent = value;
                preview.style.opacity = '1';
            } else {
                // Set default values
                const defaults = {
                    name: 'Ahmet Yılmaz',
                    title: 'Yazılım Geliştirici',
                    email: 'ahmet@example.com',
                    phone: '+90 555 123 4567',
                    address: 'İstanbul, Türkiye',
                    website: 'linkedin.com/in/ahmetyilmaz',
                    summary: '5+ yıl deneyimli yazılım geliştirici olarak modern web teknolojileri konusunda uzmanım. React, Node.js ve JavaScript ekosistemi ile çalışmaktan keyif alıyorum. Takım çalışmasına yatkın, problem çözme odaklı ve sürekli öğrenmeye açık bir profesyonelim.'
                };

                const key = field === 'fullName' ? 'name' : field === 'jobTitle' ? 'title' : field;
                preview.textContent = defaults[key] || '';
                preview.style.opacity = '0.7';
            }
        }
    });
}

// Add experience
function addExperience() {
    experienceCount++;
    const container = document.getElementById('experiences');
    const experienceDiv = document.createElement('div');
    experienceDiv.className = 'cv-dynamic-item';
    experienceDiv.innerHTML = `
        <div class="cv-item-header">
            <span class="cv-item-title">Deneyim ${experienceCount}</span>
            <button class="cv-remove-btn" onclick="removeItem(this, updateExperiencePreview)">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <line x1="18" y1="6" x2="6" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    <line x1="6" y1="6" x2="18" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                Sil
            </button>
        </div>
        <input type="text" class="cv-input-field" placeholder="Pozisyon" onchange="updateExperiencePreview()">
        <input type="text" class="cv-input-field" placeholder="Şirket" onchange="updateExperiencePreview()">
        <div class="cv-input-group">
            <input type="text" class="cv-input-field" placeholder="Başlangıç Tarihi" onchange="updateExperiencePreview()">
            <input type="text" class="cv-input-field" placeholder="Bitiş Tarihi" onchange="updateExperiencePreview()">
        </div>
        <textarea class="cv-textarea-field" placeholder="Açıklama" rows="3" onchange="updateExperiencePreview()"></textarea>
        <label class="cv-checkbox-label">
            <input type="checkbox" onchange="updateExperiencePreview()">
            Halen devam ediyor
        </label>
    `;
    container.appendChild(experienceDiv);
    updateExperiencePreview();
}

// Add education
function addEducation() {
    educationCount++;
    const container = document.getElementById('education');
    const educationDiv = document.createElement('div');
    educationDiv.className = 'cv-dynamic-item';
    educationDiv.innerHTML = `
        <div class="cv-item-header">
            <span class="cv-item-title">Eğitim ${educationCount}</span>
            <button class="cv-remove-btn" onclick="removeItem(this, updateEducationPreview)">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <line x1="18" y1="6" x2="6" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    <line x1="6" y1="6" x2="18" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                Sil
            </button>
        </div>
        <input type="text" class="cv-input-field" placeholder="Bölüm/Derece" onchange="updateEducationPreview()">
        <input type="text" class="cv-input-field" placeholder="Okul/Üniversite" onchange="updateEducationPreview()">
        <div class="cv-input-group">
            <input type="text" class="cv-input-field" placeholder="Başlangıç Tarihi" onchange="updateEducationPreview()">
            <input type="text" class="cv-input-field" placeholder="Bitiş Tarihi" onchange="updateEducationPreview()">
        </div>
        <label class="cv-checkbox-label">
            <input type="checkbox" onchange="updateEducationPreview()">
            Halen devam ediyor
        </label>
    `;
    container.appendChild(educationDiv);
    updateEducationPreview();
}

// Add skill
function addSkill() {
    skillCount++;
    const container = document.getElementById('skills');
    const skillDiv = document.createElement('div');
    skillDiv.className = 'cv-dynamic-item';
    skillDiv.innerHTML = `
        <div class="cv-item-header">
            <span class="cv-item-title">Yetenek ${skillCount}</span>
            <button class="cv-remove-btn" onclick="removeItem(this, updateSkillsPreview)">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <line x1="18" y1="6" x2="6" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    <line x1="6" y1="6" x2="18" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                Sil
            </button>
        </div>
        <input type="text" class="cv-input-field" placeholder="Yetenek Adı" onchange="updateSkillsPreview()">
        <input type="range" class="cv-input-field" min="0" max="100" value="50" onchange="updateSkillsPreview()">
        <span class="cv-range-value">50%</span>
    `;
    container.appendChild(skillDiv);

    // Add event listener for range input
    const rangeInput = skillDiv.querySelector('input[type="range"]');
    const rangeValue = skillDiv.querySelector('.cv-range-value');
    rangeInput.addEventListener('input', function () {
        rangeValue.textContent = this.value + '%';
        updateSkillsPreview();
    });

    updateSkillsPreview();
}

// Add language
function addLanguage() {
    languageCount++;
    const container = document.getElementById('languages');
    const languageDiv = document.createElement('div');
    languageDiv.className = 'cv-dynamic-item';
    languageDiv.innerHTML = `
        <div class="cv-item-header">
            <span class="cv-item-title">Dil ${languageCount}</span>
            <button class="cv-remove-btn" onclick="removeItem(this, updateLanguagesPreview)">
                <svg width="12" height="12" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <line x1="18" y1="6" x2="6" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                    <line x1="6" y1="6" x2="18" y2="18" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                </svg>
                Sil
            </button>
        </div>
        <input type="text" class="cv-input-field" placeholder="Dil" onchange="updateLanguagesPreview()">
        <select class="cv-select-field" onchange="updateLanguagesPreview()">
            <option value="1">Başlangıç</option>
            <option value="2">Temel</option>
            <option value="3">Orta</option>
            <option value="4">İleri</option>
            <option value="5">Ana Dil</option>
        </select>
    `;
    container.appendChild(languageDiv);
    updateLanguagesPreview();
}

// Remove item
function removeItem(button, updateFunction) {
    const item = button.closest('.cv-dynamic-item');
    item.style.transform = 'translateX(-100%)';
    item.style.opacity = '0';

    setTimeout(() => {
        item.remove();
        updateFunction();
    }, 300);
}

// Update experience preview
function updateExperiencePreview() {
    const container = document.getElementById('experiences');
    const previewContainer = document.getElementById('preview-experiences');
    const items = container.querySelectorAll('.cv-dynamic-item');

    let html = '';
    items.forEach(item => {
        const inputs = item.querySelectorAll('input, textarea');
        const position = inputs[0].value || 'Pozisyon';
        const company = inputs[1].value || 'Şirket';
        const startDate = inputs[2].value || 'Başlangıç';
        const endDate = inputs[3].value || 'Bitiş';
        const description = inputs[4].value || 'Açıklama';
        const isCurrent = inputs[5].checked;

        const dateText = isCurrent ? `${startDate} - Devam` : `${startDate} - ${endDate}`;

        html += `
            <div class="cv-experience-item">
                <div class="cv-experience-header">
                    <div class="cv-experience-info">
                        <div class="cv-experience-title">${position}</div>
                        <div class="cv-experience-company">${company}</div>
                    </div>
                    <div class="cv-experience-date">${dateText}</div>
                </div>
                <div class="cv-experience-description">${description}</div>
            </div>
        `;
    });

    previewContainer.innerHTML = html;

    // Show/hide section
    const section = document.getElementById('experience-section');
    section.style.display = html ? 'block' : 'none';
}

// Update education preview
function updateEducationPreview() {
    const container = document.getElementById('education');
    const previewContainer = document.getElementById('preview-education');
    const items = container.querySelectorAll('.cv-dynamic-item');

    let html = '';
    items.forEach(item => {
        const inputs = item.querySelectorAll('input');
        const degree = inputs[0].value || 'Derece';
        const school = inputs[1].value || 'Okul';
        const startDate = inputs[2].value || 'Başlangıç';
        const endDate = inputs[3].value || 'Bitiş';
        const isCurrent = inputs[4].checked;

        const dateText = isCurrent ? `${startDate} - Devam` : `${startDate} - ${endDate}`;

        html += `
            <div class="cv-education-item">
                <div class="cv-education-header">
                    <div class="cv-education-info">
                        <div class="cv-education-title">${degree}</div>
                        <div class="cv-education-school">${school}</div>
                    </div>
                    <div class="cv-education-date">${dateText}</div>
                </div>
            </div>
        `;
    });

    previewContainer.innerHTML = html;

    // Show/hide section
    const section = document.getElementById('education-section');
    section.style.display = html ? 'block' : 'none';
}

// Update skills preview
function updateSkillsPreview() {
    const container = document.getElementById('skills');
    const previewContainer = document.getElementById('preview-skills');
    const items = container.querySelectorAll('.cv-dynamic-item');

    let html = '';
    items.forEach(item => {
        const nameInput = item.querySelector('input[type="text"]');
        const rangeInput = item.querySelector('input[type="range"]');

        const name = nameInput.value || 'Yetenek';
        const level = rangeInput.value || '50';

        html += `
            <div class="cv-skill-item">
                <div class="cv-skill-name">${name}</div>
                <div class="cv-skill-bar">
                    <div class="cv-skill-progress" style="width: ${level}%"></div>
                </div>
            </div>
        `;
    });

    previewContainer.innerHTML = html;

    // Show/hide section
    const section = document.getElementById('skills-section');
    section.style.display = html ? 'block' : 'none';
}

// Update languages preview
function updateLanguagesPreview() {
    const container = document.getElementById('languages');
    const previewContainer = document.getElementById('preview-languages');
    const items = container.querySelectorAll('.cv-dynamic-item');

    let html = '';
    items.forEach(item => {
        const nameInput = item.querySelector('input[type="text"]');
        const selectInput = item.querySelector('select');

        const name = nameInput.value || 'Dil';
        const level = parseInt(selectInput.value) || 1;

        let dots = '';
        for (let i = 1; i <= 5; i++) {
            const active = i <= level ? 'cv-level-active' : '';
            dots += `<div class="cv-level-dot ${active}"></div>`;
        }

        html += `
            <div class="cv-language-item">
                <span class="cv-language-name">${name}</span>
                <div class="cv-language-level">${dots}</div>
            </div>
        `;
    });

    previewContainer.innerHTML = html;

    // Show/hide section
    const section = document.getElementById('languages-section');
    section.style.display = html ? 'block' : 'none';
}

// Download PDF
function downloadPDF() {
    const element = document.getElementById('cv-container');
    const button = document.querySelector('.cv-download-btn');

    // Show loading state
    button.innerHTML = `
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M12 2v4m0 12v4m9-9h-4M5 12H1m15.364-6.364l-2.828 2.828M8.464 15.536L5.636 18.364M18.364 18.364l-2.828-2.828M8.464 8.464L5.636 5.636" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
        </svg>
        Oluşturuluyor...
    `;
    button.style.pointerEvents = 'none';

    const opt = {
        margin: 0,
        filename: 'cv.pdf',
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: {
            scale: 2,
            useCORS: true,
            scrollX: 0,
            scrollY: 0
        },
        jsPDF: {
            unit: 'mm',
            format: 'a4',
            orientation: 'portrait'
        }
    };

    html2pdf().set(opt).from(element).save().then(() => {
        // Reset button
        button.innerHTML = `
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                <polyline points="7,10 12,15 17,10" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                <line x1="12" y1="15" x2="12" y2="3" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>
            PDF İndir
        `;
        button.style.pointerEvents = 'auto';
    });
}

// Initialize
document.addEventListener('DOMContentLoaded', function () {
    updatePreview();

    // Add smooth scrolling for form sections
    const sections = document.querySelectorAll('.cv-form-section');
    sections.forEach(section => {
        section.addEventListener('click', function () {
            this.scrollIntoView({ behavior: 'smooth', block: 'start' });
        });
    });

    // Add keyboard shortcuts
    document.addEventListener('keydown', function (e) {
        if (e.ctrlKey && e.key === 's') {
            e.preventDefault();
            downloadPDF();
        }
    });
});