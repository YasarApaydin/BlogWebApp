/**
 * GitAnalytics - Unified JavaScript
 * Modern, clean JavaScript for all pages
 */

// Global variables
let charts = {};

// Initialize everything when DOM loads
document.addEventListener('DOMContentLoaded', () => {
    initPreloader();
    initNavigation();
    initBackToTop();
    initModals();
    initForms();
    initAnalyzer();
    initCharts();
    initAnimations();
});

/**
 * Preloader functionality
 */
function initPreloader() {
    const preloader = document.getElementById('preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            preloader.classList.add('fade-out');
            setTimeout(() => {
                preloader.style.display = 'none';
            }, 500);
        });
    }
}

/**
 * Navigation functionality
 */
function initNavigation() {
    const header = document.querySelector('.header');
    const navLinks = document.querySelectorAll('.nav-link');

    // Sticky header effect
    if (header) {
        window.addEventListener('scroll', () => {
            if (window.scrollY > 50) {
                header.classList.add('scrolled');
            } else {
                header.classList.remove('scrolled');
            }
        });
    }

    // Active navigation link
    const currentLocation = location.pathname;
    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (currentLocation.endsWith(linkPath) ||
            (currentLocation === '/' && linkPath === 'index.html')) {
            link.classList.add('active');
        } else {
            link.classList.remove('active');
        }
    });

    // Mobile menu toggle
    const navbarToggler = document.querySelector('.navbar-toggler');
    const navbarCollapse = document.querySelector('.navbar-collapse');

    if (navbarToggler && navbarCollapse) {
        navbarToggler.addEventListener('click', () => {
            navbarCollapse.classList.toggle('show');
        });

        // Close mobile menu when clicking outside
        document.addEventListener('click', (e) => {
            if (!navbarToggler.contains(e.target) && !navbarCollapse.contains(e.target)) {
                navbarCollapse.classList.remove('show');
            }
        });
    }
}

/**
 * Back to top button
 */
function initBackToTop() {
    let backToTopButton = document.querySelector('.back-to-top');

    if (!backToTopButton) {
        backToTopButton = document.createElement('button');
        backToTopButton.className = 'back-to-top';
        backToTopButton.innerHTML = '<i class="fas fa-chevron-up"></i>';
        backToTopButton.setAttribute('aria-label', 'Back to top');
        document.body.appendChild(backToTopButton);
    }

    // Show/hide on scroll
    window.addEventListener('scroll', () => {
        if (window.scrollY > 300) {
            backToTopButton.classList.add('show');
        } else {
            backToTopButton.classList.remove('show');
        }
    });

    // Scroll to top functionality
    backToTopButton.addEventListener('click', (e) => {
        e.preventDefault();
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}

/**
 * Modal functionality
 */
function initModals() {
    // Login form
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', handleLoginForm);
    }

    // Register form
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', handleRegisterForm);
    }
}

function handleLoginForm(e) {
    e.preventDefault();

    const email = document.getElementById('loginEmail')?.value;
    const password = document.getElementById('loginPassword')?.value;

    if (!email || !password) {
        showNotification('Please fill in all fields.', 'warning');
        return;
    }

    // Simulate login (demo purposes)
    showNotification('Login successful! (Demo mode)', 'success');

    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('loginModal'));
    if (modal) {
        modal.hide();
    }
}

function handleRegisterForm(e) {
    e.preventDefault();

    const fullName = document.getElementById('fullName')?.value;
    const email = document.getElementById('registerEmail')?.value;
    const password = document.getElementById('registerPassword')?.value;
    const confirmPassword = document.getElementById('confirmPassword')?.value;

    if (!fullName || !email || !password || !confirmPassword) {
        showNotification('Please fill in all fields.', 'warning');
        return;
    }

    if (password !== confirmPassword) {
        showNotification('Passwords do not match!', 'danger');
        return;
    }

    // Simulate registration (demo purposes)
    showNotification('Registration successful! (Demo mode)', 'success');

    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('registerModal'));
    if (modal) {
        modal.hide();
    }
}

/**
 * Form functionality
 */
function initForms() {
    // Contact form
    const contactForm = document.getElementById('contactForm');
    if (contactForm) {
        contactForm.addEventListener('submit', handleContactForm);
    }

    // Newsletter forms
    const newsletterForms = document.querySelectorAll('.newsletter-form');
    newsletterForms.forEach(form => {
        form.addEventListener('submit', handleNewsletterForm);
    });

    // Post creation form
    const postForm = document.getElementById('postForm');
    if (postForm) {
        initPostCreation();
    }
}

function handleContactForm(e) {
    e.preventDefault();

    const formData = new FormData(e.target);
    const data = Object.fromEntries(formData);

    // Validate required fields
    const requiredFields = ['firstName', 'lastName', 'email', 'message'];
    const missingFields = requiredFields.filter(field => !data[field]);

    if (missingFields.length > 0) {
        showNotification('Please fill in all required fields.', 'warning');
        return;
    }

    // Simulate form submission
    showNotification('Message sent successfully! We\'ll get back to you soon.', 'success');
    e.target.reset();
}

function handleNewsletterForm(e) {
    e.preventDefault();

    const email = e.target.querySelector('input[type="email"]')?.value;

    if (!email) {
        showNotification('Please enter your email address.', 'warning');
        return;
    }

    // Simulate newsletter subscription
    showNotification('Successfully subscribed to newsletter!', 'success');
    e.target.reset();
}

/**
 * Analyzer functionality
 */
function initAnalyzer() {
    const analyzerForm = document.getElementById('analyzerForm');
    const githubInput = document.querySelector('.github-input');
    const repoTags = document.querySelectorAll('.repo-tag');
    const tabButtons = document.querySelectorAll('.tab-btn');
    const analysisResults = document.getElementById('analysisResults');

    // Handle analyzer form submission
    if (analyzerForm) {
        analyzerForm.addEventListener('submit', handleAnalyzerForm);
    }

    // Handle repo tag clicks
    repoTags.forEach(tag => {
        tag.addEventListener('click', (e) => {
            e.preventDefault();
            const repo = tag.getAttribute('data-repo');
            if (githubInput) {
                githubInput.value = repo;
                if (analyzerForm) {
                    analyzerForm.dispatchEvent(new Event('submit'));
                }
            }
        });
    });

    // Handle tab switching
    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            const tabId = button.getAttribute('data-tab');
            switchTab(tabId);
        });
    });

    // Check URL parameters for auto-analysis
    const urlParams = new URLSearchParams(window.location.search);
    const repoParam = urlParams.get('repo');

    if (repoParam && githubInput) {
        githubInput.value = repoParam;
        showAnalysisResults();
    }
}

async function handleAnalyzerForm(e) {
    e.preventDefault();

    const githubInput = document.querySelector('.github-input');
    const repo = githubInput?.value.trim();

    if (!repo) {
        showNotification('Please enter a GitHub repository URL.', 'warning');
        return;
    }

    const githubRegex = /^(https?:\/\/)?(github\.com\/)?[\w\-\.]+\/[\w\-\.]+\/?$/;
    if (!githubRegex.test(repo)) {
        showNotification('Please enter a valid GitHub repository URL.', 'danger');
        return;
    }
    try {
        const analyzeBtn = document.querySelector('.analyze-btn');
        analyzeBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Analyzing...';
        analyzeBtn.disabled = true;

        const response = await fetch(`/GithubAnalyzer/GetRepoInfo?repoUrl=${encodeURIComponent(repo)}`);
        const data = await response.json(); // sadece bir kez json() çağrılır

        if (!response.ok) {
            const errorMessage = data.error || "Bilinmeyen hata oluştu.";
            showNotification(errorMessage, 'danger');
            return;
        }

        // Repo bilgilerini doldur
        document.querySelector('.project-name').textContent = data.Repo.name;
        document.querySelector('.project-description').textContent = data.Repo.description;
        document.querySelector('.project-logo').src = data.Repo.owner.avatar_Url;
        document.querySelector('.meta-item:nth-child(1)').innerHTML = `<i class="fas fa-star"></i> ${data.Repo.stargazers_Count} Stars`;
        document.querySelector('.meta-item:nth-child(2)').innerHTML = `<i class="fas fa-code-branch"></i> ${data.Repo.forks_Count} Forks`;
        document.querySelector('.meta-item:nth-child(4)').innerHTML = `<i class="fas fa-clock"></i> Last update: ${new Date(data.Repo.updated_At).toLocaleDateString()}`;

        // Diller: Chart güncelle
        updateLanguageChart(data.Languages);

        // Contributors
        const contributorList = document.querySelector('#contributors .metric-list');
        if (contributorList && Array.isArray(data.Contributors)) {
            contributorList.innerHTML = data.Contributors.slice(0, 3).map(c => `
            <li class="metric-item">
              <span class="metric-label">${c.login}</span>
              <span class="metric-value">${c.contributions} commits</span>
            </li>`).join('');
        }

        // Sonuçları göster
        const analysisResults = document.getElementById('analysisResults');
        if (analysisResults) {
            analysisResults.style.display = 'block';
            analysisResults.scrollIntoView({ behavior: 'smooth' });
        }

        showNotification('Analysis complete!', 'success');
    } catch (err) {
        console.error(err);
        showNotification('Unexpected error: ' + err.message, 'danger');
    } finally {
        const analyzeBtn = document.querySelector('.analyze-btn');
        analyzeBtn.innerHTML = '<i class="fas fa-search"></i> Analyze Repository';
        analyzeBtn.disabled = false;
    }

}

function showAnalysisResults() {
    const analysisResults = document.getElementById('analysisResults');
    const analyzeBtn = document.querySelector('.analyze-btn');

    if (analyzeBtn) {
        analyzeBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Analyzing...';
        analyzeBtn.disabled = true;
    }

    // Simulate analysis delay
    setTimeout(() => {
        if (analysisResults) {
            analysisResults.style.display = 'block';
            analysisResults.scrollIntoView({ behavior: 'smooth' });
        }

        if (analyzeBtn) {
            analyzeBtn.innerHTML = '<i class="fas fa-search"></i> Analyze Repository';
            analyzeBtn.disabled = false;
        }

        // Initialize charts after showing results
        setTimeout(initCharts, 500);

        showNotification('Analysis complete!', 'success');
    }, 2000);
}

function switchTab(tabId) {
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabPanes = document.querySelectorAll('.tab-pane');

    // Remove active class from all tabs
    tabButtons.forEach(btn => btn.classList.remove('active'));
    tabPanes.forEach(pane => pane.classList.remove('active'));

    // Add active class to selected tab
    const selectedButton = document.querySelector(`[data-tab="${tabId}"]`);
    const selectedPane = document.getElementById(tabId);

    if (selectedButton && selectedPane) {
        selectedButton.classList.add('active');
        selectedPane.classList.add('active');
    }
}

/**
 * Charts initialization
 */
function initCharts() {
    if (typeof Chart === 'undefined') return;

   
    initCommitChart();
    initCodeStructureChart();
}

function updateLanguageChart(languages) {
    const chartEl = document.getElementById('languageChart');
    if (!chartEl) return;

    const labels = Object.keys(languages);
    const values = Object.values(languages);

    if (charts.languageChart) charts.languageChart.destroy();

    charts.languageChart = new Chart(chartEl, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: values,
                backgroundColor: ['#F7DF1E', '#3178C6', '#264DE4', '#E34F26', '#6C757D'],
                borderWidth: 0
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { position: 'right' },
                tooltip: {
                    callbacks: {
                        label: function (ctx) {
                            return `${ctx.label}: ${ctx.raw}`;
                        }
                    }
                }
            },
            cutout: '65%'
        }
    });
}

function initCommitChart() {
    const chartEl = document.getElementById('commitChart');
    if (!chartEl) return;

    // Destroy existing chart
    if (charts.commitChart) {
        charts.commitChart.destroy();
    }

    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    const commitData = Array.from({ length: 12 }, () => Math.floor(Math.random() * 90) + 30);

    charts.commitChart = new Chart(chartEl, {
        type: 'line',
        data: {
            labels: months,
            datasets: [{
                label: 'Commits',
                data: commitData,
                backgroundColor: 'rgba(59, 130, 246, 0.1)',
                borderColor: '#3B82F6',
                borderWidth: 2,
                tension: 0.3,
                fill: true,
                pointBackgroundColor: '#3B82F6',
                pointRadius: 4,
                pointHoverRadius: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            },
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: '#1E293B',
                    titleColor: '#E2E8F0',
                    bodyColor: '#E2E8F0',
                    borderColor: '#3B82F6',
                    borderWidth: 1,
                    cornerRadius: 8,
                    displayColors: false
                }
            }
        }
    });
}

function initCodeStructureChart() {
    const chartEl = document.getElementById('codeStructureChart');
    if (!chartEl) return;

    // Destroy existing chart
    if (charts.codeStructureChart) {
        charts.codeStructureChart.destroy();
    }

    charts.codeStructureChart = new Chart(chartEl, {
        type: 'radar',
        data: {
            labels: [
                'Modularity',
                'Cohesion',
                'Low Coupling',
                'Code Reuse',
                'Abstraction',
                'Encapsulation'
            ],
            datasets: [{
                label: 'Project Score',
                data: [85, 90, 75, 95, 88, 92],
                backgroundColor: 'rgba(59, 130, 246, 0.2)',
                borderColor: '#3B82F6',
                borderWidth: 2,
                pointBackgroundColor: '#3B82F6',
                pointRadius: 4,
                pointHoverRadius: 6
            }, {
                label: 'Industry Average',
                data: [70, 65, 60, 75, 72, 68],
                backgroundColor: 'rgba(148, 163, 184, 0.2)',
                borderColor: '#94A3B8',
                borderWidth: 2,
                pointBackgroundColor: '#94A3B8',
                pointRadius: 4,
                pointHoverRadius: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                r: {
                    beginAtZero: true,
                    max: 100,
                    ticks: {
                        stepSize: 20
                    }
                }
            },
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

/**
 * Post creation functionality
 */
function initPostCreation() {
    const uploadArea = document.getElementById('uploadArea');
    const featuredImageInput = document.getElementById('featuredImage');
    const imagePreview = document.getElementById('imagePreview');
    const previewImg = document.getElementById('previewImg');
    const removeImageBtn = document.getElementById('removeImage');

    if (uploadArea && featuredImageInput) {
        // Click to upload
        uploadArea.addEventListener('click', () => {
            featuredImageInput.click();
        });

        // Drag and drop
        uploadArea.addEventListener('dragover', (e) => {
            e.preventDefault();
            uploadArea.classList.add('drag-over');
        });

        uploadArea.addEventListener('dragleave', () => {
            uploadArea.classList.remove('drag-over');
        });

        uploadArea.addEventListener('drop', (e) => {
            e.preventDefault();
            uploadArea.classList.remove('drag-over');

            const files = e.dataTransfer.files;
            if (files.length > 0) {
                handleImageUpload(files[0]);
            }
        });

        // File input change
        featuredImageInput.addEventListener('change', (e) => {
            if (e.target.files.length > 0) {
                handleImageUpload(e.target.files[0]);
            }
        });
    }

    // Remove image
    if (removeImageBtn) {
        removeImageBtn.addEventListener('click', () => {
            featuredImageInput.value = '';
            uploadArea.style.display = 'block';
            imagePreview.style.display = 'none';
        });
    }

    function handleImageUpload(file) {
        if (!file.type.startsWith('image/')) {
            showNotification('Please select an image file.', 'warning');
            return;
        }

        if (file.size > 5 * 1024 * 1024) {
            showNotification('Image size should be less than 5MB.', 'warning');
            return;
        }

        const reader = new FileReader();
        reader.onload = (e) => {
            previewImg.src = e.target.result;
            uploadArea.style.display = 'none';
            imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }
}

/**
 * Animations
 */
function initAnimations() {
    // Animate elements on scroll
    const animateOnScroll = () => {
        const elements = document.querySelectorAll('.animate-on-scroll');

        elements.forEach(element => {
            const elementPosition = element.getBoundingClientRect().top;
            const windowHeight = window.innerHeight;

            if (elementPosition < windowHeight - 100) {
                element.classList.add('animated');
            }
        });
    };

    // Initial call and add event listener
    animateOnScroll();
    window.addEventListener('scroll', animateOnScroll);

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;

            e.preventDefault();

            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
}

/**
 * Utility functions
 */
function showNotification(message, type = 'info') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = `
    top: 20px;
    right: 20px;
    z-index: 10000;
    min-width: 300px;
    max-width: 400px;
  `;

    notification.innerHTML = `
    ${message}
    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
  `;

    document.body.appendChild(notification);

    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Window resize handler with debounce
window.addEventListener('resize', debounce(() => {
    // Redraw charts on resize
    Object.values(charts).forEach(chart => {
        if (chart && typeof chart.resize === 'function') {
            chart.resize();
        }
    });
}, 250));

// Handle page visibility change
document.addEventListener('visibilitychange', () => {
    if (!document.hidden) {
        // Page became visible, update any time-sensitive content
        console.log('Page is now visible');
    }
});