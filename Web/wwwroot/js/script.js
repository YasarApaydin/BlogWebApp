// Örnek kullanıcı verileri
let users = [
    {
        id: 1,
        name: "Ahmet Yılmaz",
        email: "ahmet@example.com",
        role: "admin",
        status: "active"
    },
    {
        id: 2,
        name: "Mehmet Demir",
        email: "mehmet@example.com",
        role: "editor",
        status: "active"
    },
    {
        id: 3,
        name: "Ayşe Kaya",
        email: "ayse@example.com",
        role: "user",
        status: "inactive"
    }
];

// Kullanıcı bilgilerini getir
function getUserById(userId) {
    return users.find(user => user.id === parseInt(userId));
}

// Modal işlemleri
function openEditModal(userId) {
    const modal = document.getElementById('editModal');
    const user = getUserById(userId);

    if (user) {
        // Form alanlarını doldur
        document.getElementById('editUserId').value = userId;
        document.getElementById('editName').value = user.name;
        document.getElementById('editEmail').value = user.email;
        document.getElementById('editRole').value = user.role;
        document.getElementById('editStatus').value = user.status;

        // Modalı göster
        modal.classList.add('active');
    } else {
        alert('Kullanıcı bulunamadı!');
    }
}

function closeEditModal() {
    const modal = document.getElementById('editModal');
    modal.classList.remove('active');
}

function saveChanges(event) {
    event.preventDefault();

    const userId = parseInt(document.getElementById('editUserId').value);
    const userData = {
        id: userId, // ID'yi koruyoruz
        name: document.getElementById('editName').value,
        email: document.getElementById('editEmail').value,
        role: document.getElementById('editRole').value,
        status: document.getElementById('editStatus').value
    };

    // Kullanıcı verisini güncelle
    const userIndex = users.findIndex(user => user.id === userId);
    if (userIndex !== -1) {
        users[userIndex] = userData;

        // Tabloyu güncelle
        updateTable();
        closeEditModal();

        // Başarılı mesajı göster
        alert('Kullanıcı bilgileri başarıyla güncellendi!');
    }
}

// Tabloyu güncelle
function updateTable() {
    const tbody = document.querySelector('.data-table tbody');
    if (!tbody) return;

    tbody.innerHTML = users.map(user => `
        <tr>
            <td>
                <div class="user-cell">
                    <img src="https://ui-avatars.com/api/?name=${encodeURIComponent(user.name)}" alt="${user.name}">
                    <div>
                        <h4>${user.name}</h4>
                        <p>${user.email}</p>
                    </div>
                </div>
            </td>
            <td class="role-cell">
                <span class="badge-role ${user.role}">${user.role}</span>
            </td>
            <td class="status-cell">
                <span class="badge-status ${user.status}">${user.status}</span>
            </td>
            <td>
                <div class="actions">
                    <button class="action-btn edit-btn" onclick="openEditModal(${user.id})">
                        <i class="fas fa-edit"></i>
                        Düzenle
                    </button>
                    <button class="action-btn delete-btn" onclick="deleteUser(${user.id})">
                        <i class="fas fa-trash"></i>
                        Sil
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Kullanıcı silme işlemi
function deleteUser(userId) {
    if (confirm('Bu kullanıcıyı silmek istediğinizden emin misiniz?')) {
        users = users.filter(user => user.id !== userId);
        updateTable();
        alert('Kullanıcı başarıyla silindi!');
    }
}

// Modal dışına tıklandığında kapat
document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('editModal');

    modal.addEventListener('click', function (e) {
        if (e.target === this) {
            closeEditModal();
        }
    });

    // Tabloyu ilk yüklemede güncelle
    updateTable();
}); 