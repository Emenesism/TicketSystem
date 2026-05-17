export function openModal(id) {
    const el = document.getElementById(id);
    if (el) el.style.display = 'flex';
}

export function closeModal(id) {
    const el = document.getElementById(id);
    if (el) el.style.display = 'none';
}

export function showError(alertId, textId, message, timeout = 5000) {
    const alert = document.getElementById(alertId);
    const text = document.getElementById(textId);
    if (text) text.textContent = message;
    if (alert) {
        alert.style.display = 'flex';
        if (timeout) setTimeout(() => { alert.style.display = 'none'; }, timeout);
    }
}

export function showLoading(loadingId, isLoading) {
    const el = document.getElementById(loadingId);
    if (el) el.style.display = isLoading ? 'block' : 'none';
}

export function formatDate(dateStr) {
    if (!dateStr) return '—';
    return new Date(dateStr).toLocaleDateString();
}

export function formatDateTime(dateStr) {
    if (!dateStr) return '—';
    return new Date(dateStr).toLocaleString();
}

export function getInitial(name) {
    return name ? name.charAt(0).toUpperCase() : '?';
}

export function getValue(obj, key) {
    if (!obj) return undefined;
    const direct = obj[key];
    if (direct !== undefined && direct !== null) return direct;
    const camel = key.charAt(0).toUpperCase() + key.slice(1);
    return obj[camel];
}
