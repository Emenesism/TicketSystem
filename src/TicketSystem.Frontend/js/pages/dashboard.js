import { getApiBaseUrl, getToken, clearSession } from '../core/config.js';

let API = getApiBaseUrl();
let TOKEN = getToken();

function clearSessionAndRedirect() {
    clearSession();
    window.location.href = 'index.html';
}

async function refreshAccessToken() {
    try {
        const res = await fetch(`${API}/auth/admin/refresh`, { method: 'POST', credentials: 'include' });
        if (res.ok) {
            const data = await res.json();
            const newToken = data.token || data.Token || '';
            if (newToken) { TOKEN = newToken; localStorage.setItem('ts_token', newToken); return true; }
        }
    } catch (err) { console.error('Refresh failed:', err); }
    return false;
}

async function authenticatedFetch(url, options = {}) {
    if (!options.headers) options.headers = {};
    options.headers['Authorization'] = TOKEN.startsWith('Bearer') ? TOKEN : `Bearer ${TOKEN}`;
    options.credentials = 'include';
    let response = await fetch(url, options);
    if (response.status === 401) {
        const success = await refreshAccessToken();
        if (success) {
            options.headers['Authorization'] = TOKEN.startsWith('Bearer') ? TOKEN : `Bearer ${TOKEN}`;
            response = await fetch(url, options);
        } else { clearSessionAndRedirect(); return null; }
    }
    return response;
}

async function load() {
    const state = document.getElementById('state');
    const tbody = document.getElementById('tbody');
    const errorBar = document.getElementById('errorBar');
    tbody.innerHTML = '';
    state.innerHTML = '<div class="spinner"></div><p>Loading sessions…</p>';
    state.style.display = 'block';
    errorBar.style.display = 'none';

    try {
        const res = await authenticatedFetch(`${API}/dashboard`);
        if (!res) return;
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        const sessions = await res.json();
        state.style.display = 'none';
        if (!sessions || !sessions.length) {
            state.innerHTML = '<p>No active sessions found.</p>';
            state.style.display = 'block';
            return;
        }
        sessions.forEach(s => {
            const tr = document.createElement('tr');
            const id = s.isAdmin ? s.adminId : s.userId;
            const lastUsed = s.lastTimeUsed ? new Date(s.lastTimeUsed).toLocaleString() : '—';
            const expiresAt = s.expiresAt ? new Date(s.expiresAt).toLocaleString() : '—';
            tr.innerHTML = `
                <td><span class="badge ${s.isAdmin ? 'badge-admin' : 'badge-user'}">${s.isAdmin ? 'ADMIN' : 'USER'}</span></td>
                <td><code>${id || '—'}</code></td>
                <td>${s.ipAddress || '—'}</td>
                <td class="muted" title="${s.userAgent}" style="max-width:280px;overflow:hidden;text-overflow:ellipsis;white-space:nowrap">${s.userAgent || '—'}</td>
                <td class="muted">${lastUsed}</td>
                <td class="muted">${expiresAt}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (err) {
        state.style.display = 'none';
        errorBar.textContent = `Failed to load sessions: ${err.message}`;
        errorBar.style.display = 'block';
    }
}

window.load = load;
document.addEventListener('DOMContentLoaded', load);
