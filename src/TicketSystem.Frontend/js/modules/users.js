import { apiRequest } from '../core/api.js';
import { createUserRow } from '../components/tableRows.js';
import { renderPaginationControls } from '../components/pagination.js';

let currentPage = 1;
let currentLimit = 10;
let currentFilterType = 'all';
let currentFilterDate = null;

export function getCurrentPage() { return currentPage; }
export function setCurrentPage(p) { currentPage = p; }
export function getCurrentLimit() { return currentLimit; }
export function setCurrentLimit(l) { currentLimit = l; }

export function renderUsers(data, tableBodyId, emptyStateId, paginationContainerId) {
    const tableBody = document.getElementById(tableBodyId);
    const emptyState = document.getElementById(emptyStateId);
    const paginationContainer = document.getElementById(paginationContainerId);
    if (!tableBody) return;
    tableBody.innerHTML = '';

    const items = data.items || (Array.isArray(data) ? data : [data]);
    const totalCount = data.totalCount || items.length;
    const totalPages = data.totalPages || 1;

    if (!items || items.length === 0) {
        if (emptyState) emptyState.style.display = 'block';
        if (paginationContainer) paginationContainer.style.display = 'none';
        return;
    }
    if (emptyState) emptyState.style.display = 'none';

    items.forEach(user => tableBody.appendChild(createUserRow(user)));

    renderPagination(totalCount, totalPages, paginationContainerId);
}

function renderPagination(totalCount, totalPages, containerId) {
    renderPaginationControls({
        containerId,
        infoId: 'paginationInfo',
        pageButtonsId: 'pageButtons',
        prevButtonId: 'prevPageBtn',
        nextButtonId: 'nextPageBtn',
        limitSelectId: 'limitSelect',
        currentPage,
        currentLimit,
        totalCount,
        totalPages,
    });
}

export async function loadUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn) {
    if (showLoadingFn) showLoadingFn(true);
    let endpoint;

    if (currentFilterType === 'before' && currentFilterDate) {
        endpoint = `/users/before-date?date=${encodeURIComponent(currentFilterDate)}&page=${currentPage}&limit=${currentLimit}`;
    } else if (currentFilterType === 'after' && currentFilterDate) {
        endpoint = `/users/after-date?date=${encodeURIComponent(currentFilterDate)}&page=${currentPage}&limit=${currentLimit}`;
    } else {
        endpoint = `/users/all?page=${currentPage}&limit=${currentLimit}`;
    }

    try {
        const response = await apiRequest(endpoint, { method: 'GET', headers: { 'Content-Type': 'application/json' } }, role, onLogout);
        if (!response) return;
        const data = await response.json();
        if (!response.ok) throw new Error(data.message || `Error: ${response.status}`);
        if (renderFn) renderFn(data);
    } catch (err) {
        if (showErrorFn) showErrorFn(err.message);
        if (renderFn) renderFn({ items: [], totalCount: 0, totalPages: 1 });
    } finally {
        if (showLoadingFn) showLoadingFn(false);
    }
}

export async function fetchAllUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn) {
    currentFilterType = 'all';
    currentFilterDate = null;
    currentPage = 1;
    await loadUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn);
}

export async function applyFilters(showLoadingFn, showErrorFn, role, onLogout, renderFn) {
    const nameEl = document.getElementById('filterName');
    const usernameEl = document.getElementById('filterUsername');
    const bDateEl = document.getElementById('filterBeforeDate');
    const aDateEl = document.getElementById('filterAfterDate');
    const name = nameEl ? nameEl.value.trim() : '';
    const username = usernameEl ? usernameEl.value.trim() : '';
    const bDate = bDateEl ? bDateEl.value : '';
    const aDate = aDateEl ? aDateEl.value : '';

    currentPage = 1;

    if (name) {
        const response = await apiRequest(`/users/by-name?name=${encodeURIComponent(name)}`, { method: 'GET' }, role, onLogout);
        if (response && response.ok) { const d = await response.json(); if (renderFn) renderFn(d); }
    } else if (username) {
        const response = await apiRequest(`/users/by-username?username=${encodeURIComponent(username)}`, { method: 'GET' }, role, onLogout);
        if (response && response.ok) { const d = await response.json(); if (renderFn) renderFn(d); }
    } else if (bDate) {
        currentFilterType = 'before';
        currentFilterDate = bDate + 'T00:00:00';
        await loadUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn);
    } else if (aDate) {
        currentFilterType = 'after';
        currentFilterDate = aDate + 'T00:00:00';
        await loadUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn);
    } else {
        await fetchAllUsers(showLoadingFn, showErrorFn, role, onLogout, renderFn);
    }
}

export function clearFilters(fetchAllFn) {
    const n = document.getElementById('filterName'); if (n) n.value = '';
    const u = document.getElementById('filterUsername'); if (u) u.value = '';
    const b = document.getElementById('filterBeforeDate'); if (b) b.value = '';
    const a = document.getElementById('filterAfterDate'); if (a) a.value = '';
    if (fetchAllFn) fetchAllFn();
}

export function goToPage(loadUsersFn) {
    return function(page) {
        currentPage = page;
        loadUsersFn();
    };
}

export function changeLimit(loadUsersFn) {
    return function(limit) {
        currentLimit = parseInt(limit);
        currentPage = 1;
        loadUsersFn();
    };
}
