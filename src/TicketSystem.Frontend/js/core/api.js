import { getApiBaseUrl, getToken, setToken, clearSession } from './config.js';

let refreshInProgress = false;

async function refreshToken(role) {
    if (refreshInProgress) return false;
    refreshInProgress = true;
    try {
        const endpoint = role === 'admin' ? '/auth/admin/refresh' : '/auth/user/refresh';
        const response = await fetch(`${getApiBaseUrl()}${endpoint}`, {
            method: 'POST',
            credentials: 'include',
        });
        if (response.ok) {
            const data = await response.json();
            const newToken = data.AccessToken || data.accessToken || data.token || '';
            if (newToken) {
                setToken(newToken);
                refreshInProgress = false;
                return true;
            }
        }
    } catch (err) {
        console.error('Token refresh failed:', err);
    }
    refreshInProgress = false;
    return false;
}

export async function apiRequest(endpoint, options = {}, role = 'user', onLogout) {
    const headers = options.headers || {};
    const token = getToken();
    if (token) {
        headers['Authorization'] = token.startsWith('Bearer') ? token : `Bearer ${token}`;
    }
    options.headers = headers;
    options.credentials = 'include';

    let response = await fetch(`${getApiBaseUrl()}${endpoint}`, options);

    if (response.status === 401) {
        const success = await refreshToken(role);
        if (success) {
            headers['Authorization'] = `Bearer ${getToken()}`;
            options.headers = headers;
            response = await fetch(`${getApiBaseUrl()}${endpoint}`, options);
        } else {
            console.log('Session expired. Redirecting to login...');
            if (onLogout) onLogout();
            else clearSession();
            return null;
        }
    }
    return response;
}

export function logout(role, onLogoutComplete) {
    fetch(`${getApiBaseUrl()}/auth/logout`, {
        method: 'POST',
        credentials: 'include',
    }).finally(() => {
        clearSession();
        if (onLogoutComplete) onLogoutComplete();
    });
}

export async function logoutAll(role, onLogoutComplete) {
    if (!confirm('Are you sure you want to log out from all devices?')) return;
    const endpoint = role === 'admin' ? '/auth/admin/revoke-all' : '/auth/user/revoke-all';
    try {
        await apiRequest(endpoint, { method: 'POST', credentials: 'include' }, role, onLogoutComplete);
        console.log('All sessions revoked successfully.');
    } catch (e) {
        console.error('Logout all error', e);
    } finally {
        logout(role, onLogoutComplete);
    }
}
