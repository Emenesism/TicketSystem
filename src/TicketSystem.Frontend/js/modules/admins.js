import { apiRequest } from '../core/api.js';
import { createAdminRow } from '../components/tableRows.js';

export async function fetchAllAdmins(role, onLogout, showErrorFn) {
    const tableBody = document.getElementById('adminsTableBody');
    if (!tableBody) return;
    tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px;"><div class="spinner" style="margin: 0 auto;"></div></td></tr>';

    try {
        const response = await apiRequest('/admin/all', { method: 'GET' }, role, onLogout);
        if (!response) return;
        const admins = await response.json();
        renderAdmins(admins);
    } catch (err) {
        if (showErrorFn) showErrorFn('Failed to fetch admins');
        tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px; color: var(--error);">Error loading admins.</td></tr>';
    }
}

function renderAdmins(admins) {
    const tableBody = document.getElementById('adminsTableBody');
    if (!tableBody) return;
    tableBody.innerHTML = '';

    if (!admins || admins.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 40px; color: var(--text-muted);">No admins found.</td></tr>';
        return;
    }

    admins.forEach(admin => tableBody.appendChild(createAdminRow(admin)));
}

export async function submitCreateAdmin(role, onLogout, fetchAllFn, closeModalFn) {
    const name = document.getElementById('adminFullName')?.value.trim();
    const username = document.getElementById('adminUsername')?.value.trim();
    const password = document.getElementById('adminPassword')?.value;

    const selectedRoles = Array.from(document.querySelectorAll('input[name="adminRole"]:checked')).map(cb => cb.value);
    const isSuperAdmin = selectedRoles.includes('SuperAdmin');

    if (!name || !username || !password) { alert('Please fill in all required fields.'); return; }
    if (selectedRoles.length === 0) { alert('Please select at least one role.'); return; }

    if (!selectedRoles.includes('Admin')) selectedRoles.push('Admin');

    try {
        const response = await apiRequest('/auth/admin/create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, username, password, isSuperAdmin, roles: selectedRoles })
        }, role, onLogout);
        if (response && response.ok) {
            alert('Admin created successfully!');
            if (closeModalFn) closeModalFn('createAdminModal');
            if (fetchAllFn) await fetchAllFn();
        } else {
            const data = await response.json();
            alert(data.message || 'Failed to create admin');
        }
    } catch (err) {
        alert('Request failed');
    }
}

export function openCreateAdminModal(openModalFn) {
    const n = document.getElementById('adminFullName'); if (n) n.value = '';
    const u = document.getElementById('adminUsername'); if (u) u.value = '';
    const p = document.getElementById('adminPassword'); if (p) p.value = '';
    const s = document.getElementById('adminIsSuper'); if (s) s.checked = false;
    document.querySelectorAll('input[name="adminRole"]').forEach(cb => cb.checked = cb.value === 'Dashboard');
    if (openModalFn) openModalFn('createAdminModal');
}

export function syncSuperAdminCheckbox(cb) {
    const el = document.getElementById('adminIsSuper');
    if (el) el.checked = cb.checked;
}
