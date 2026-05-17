export function getApiBaseUrl() {
    return localStorage.getItem('api_url') || 'http://127.0.0.1:5000';
}

export function setApiBaseUrl(url) {
    localStorage.setItem('api_url', url);
}

export function getToken() {
    return localStorage.getItem('ts_token') || '';
}

export function setToken(token) {
    localStorage.setItem('ts_token', token);
}

export function getUserId() {
    return localStorage.getItem('ts_user_id') || '';
}

export function setUserId(id) {
    localStorage.setItem('ts_user_id', id);
}

export function getRole() {
    return localStorage.getItem('ts_role') || 'user';
}

export function setRole(role) {
    localStorage.setItem('ts_role', role);
}

export function getUserData() {
    const raw = localStorage.getItem('ts_user_data');
    if (!raw) return null;
    try { return JSON.parse(raw); } catch { return null; }
}

export function setUserData(data) {
    localStorage.setItem('ts_user_data', JSON.stringify(data));
}

export function clearSession() {
    localStorage.removeItem('ts_token');
    localStorage.removeItem('ts_user_id');
    localStorage.removeItem('ts_role');
    localStorage.removeItem('ts_user_data');
}
