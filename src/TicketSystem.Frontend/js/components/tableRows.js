import { formatDate, formatDateTime, getInitial, getValue } from '../core/utils.js';

export function createUserRow(user) {
    const tr = document.createElement('tr');
    const name = getValue(user, 'name') || 'Unknown';
    const username = getValue(user, 'username') || 'unknown';
    const createdAt = getValue(user, 'createdAt');

    tr.innerHTML = `
        <td><div class="user-cell"><div class="avatar-small avatar-list">${getInitial(name)}</div><div style="font-weight: 600;">${name}</div></div></td>
        <td><span class="username-badge">@${username}</span></td>
        <td class="date-cell">${createdAt ? formatDate(createdAt) : '—'}</td>
        <td><span style="color: var(--success);">● Active</span></td>
    `;

    return tr;
}

export function createAdminRow(admin) {
    const tr = document.createElement('tr');
    const name = getValue(admin, 'name') || 'Admin';
    const username = getValue(admin, 'username') || 'admin';
    const isSuper = getValue(admin, 'isSuperAdmin') || getValue(admin, 'IsSuperAdmin');

    tr.innerHTML = `
        <td><div class="user-cell"><div class="avatar-small avatar-list" style="background: rgba(236, 72, 153, 0.2); color: #ec4899;">${getInitial(name)}</div><div style="font-weight: 600;">${name}</div></div></td>
        <td><span class="username-badge">@${username}</span></td>
        <td><span class="status-tag ${isSuper ? 'status-solved' : 'status-open'}" style="font-size: 0.7rem;">${isSuper ? 'SuperAdmin' : 'Admin'}</span></td>
        <td><span style="color: var(--success); font-size: 0.85rem;">● Active</span></td>
    `;

    return tr;
}

export function createSessionRow(session) {
    const tr = document.createElement('tr');
    const isAdmin = getValue(session, 'isAdmin') || getValue(session, 'IsAdmin');
    const userAgent = getValue(session, 'userAgent') || getValue(session, 'UserAgent') || 'Unknown Device';
    const ipAddress = getValue(session, 'ipAddress') || getValue(session, 'IpAddress') || '?.?.?.?';
    const lastUsed = getValue(session, 'lastTimeUsed') || getValue(session, 'LastTimeUsed');

    tr.innerHTML = `
        <td style="font-family: monospace;">${ipAddress}</td>
        <td title="${userAgent}" style="max-width: 300px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">${userAgent}</td>
        <td><span class="status-tag ${isAdmin ? 'status-solved' : 'status-open'}">${isAdmin ? 'Admin' : 'User'}</span></td>
        <td style="font-size: 0.85rem; color: var(--text-muted);">${formatDateTime(lastUsed)}</td>
    `;

    return tr;
}
