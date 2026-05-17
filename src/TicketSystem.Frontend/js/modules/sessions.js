import { apiRequest } from '../core/api.js';
import { createSessionRow } from '../components/tableRows.js';

export async function fetchAllSessions(role, onLogout, showErrorFn) {
    const tableBody = document.getElementById('sessionsTableBody');
    if (!tableBody) return;
    tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px;"><div class="spinner" style="margin: 0 auto;"></div></td></tr>';

    try {
        const response = await apiRequest('/dashboard', { method: 'GET' }, role, onLogout);
        if (!response) return;
        const sessions = await response.json();
        renderSessions(sessions);
    } catch (err) {
        if (showErrorFn) showErrorFn('Failed to fetch sessions');
        tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 20px; color: var(--error);">Error loading sessions.</td></tr>';
    }
}

function renderSessions(sessions) {
    const tableBody = document.getElementById('sessionsTableBody');
    if (!tableBody) return;
    tableBody.innerHTML = '';

    if (!sessions || sessions.length === 0) {
        tableBody.innerHTML = '<tr><td colspan="4" style="text-align: center; padding: 40px; color: var(--text-muted);">No active sessions found.</td></tr>';
        return;
    }

    sessions.forEach(session => tableBody.appendChild(createSessionRow(session)));
}
