import { apiRequest } from '../core/api.js';
import { formatDateTime, getValue } from '../core/utils.js';
import { openModal, closeModal } from '../core/utils.js';
import { createTicketCard } from '../components/ticketCard.js';

let currentTicketId = null;

export function getCurrentTicketId() { return currentTicketId; }
export function setCurrentTicketId(id) { currentTicketId = id; }

export function renderTickets(containerId, tickets, isUnassigned = false, onOpenDetail) {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';
    if (!tickets || tickets.length === 0) {
        container.innerHTML = '<p style="text-align: center; color: var(--text-muted); padding: 20px;">No tickets found.</p>';
        return;
    }
    tickets.forEach(ticket => container.appendChild(createTicketCard(ticket, isUnassigned, onOpenDetail)));
}

export async function loadUnassignedTickets(role, onLogout, showErrorFn, renderFn) {
    try {
        const response = await apiRequest('/ticket/not-assign', { method: 'GET' }, role, onLogout);
        if (!response) return;
        const tickets = await response.json();
        if (renderFn) renderFn(tickets, true);
    } catch (err) {
        if (showErrorFn) showErrorFn('Failed to load unassigned tickets');
    }
}

export async function loadMyTickets(role, onLogout, showErrorFn, renderFn) {
    try {
        const response = await apiRequest('/ticket/admin', { method: 'GET' }, role, onLogout);
        if (!response) return;
        const tickets = await response.json();
        if (renderFn) renderFn(tickets, false);
    } catch (err) {
        if (showErrorFn) showErrorFn('Failed to load assigned tickets');
    }
}

export async function loadUserTickets(role, onLogout, renderFn) {
    try {
        const response = await apiRequest('/ticket/user', { method: 'GET' }, role, onLogout);
        if (!response) return;
        const tickets = await response.json();
        if (renderFn) renderFn(tickets, false);
    } catch (err) {
        console.error('Failed to load tickets', err);
    }
}

export async function openTicketDetail(ticket, isUnassigned, role, userId, onLogout, hideUnassignedAdminChat = true) {
    currentTicketId = getValue(ticket, 'id');
    const titleEl = document.getElementById('modalTicketTitle');
    const metaEl = document.getElementById('modalTicketMeta');
    const statusTag = document.getElementById('modalTicketStatus');
    const assignSection = document.getElementById('adminAssignSection');
    const chatInputArea = document.getElementById('chatInputArea');
    const solveBtn = document.getElementById('solveTicketBtn');

    if (titleEl) titleEl.textContent = getValue(ticket, 'title') || 'Untitled';
    if (metaEl) metaEl.textContent = `Created on ${formatDateTime(getValue(ticket, 'createdAt'))}`;

    const tIsSolved = getValue(ticket, 'isSolved') !== undefined ? getValue(ticket, 'isSolved') : getValue(ticket, 'IsSolved');
    if (statusTag) {
        statusTag.textContent = tIsSolved ? 'Solved' : 'Open';
        statusTag.className = `status-tag ${tIsSolved ? 'status-solved' : 'status-open'}`;
    }

    const isAdmin = role === 'admin';
    if (assignSection) assignSection.style.display = isAdmin && isUnassigned ? 'block' : 'none';
    if (chatInputArea) chatInputArea.style.display = (hideUnassignedAdminChat && isUnassigned && isAdmin) || tIsSolved ? 'none' : 'flex';
    if (solveBtn) solveBtn.style.display = isAdmin && !isUnassigned && !tIsSolved ? 'block' : 'none';

    await loadMessages(role, onLogout);
    openModal('ticketDetailModal');
}

export async function loadMessages(role, onLogout) {
    const chatMessages = document.getElementById('chatMessages');
    if (!chatMessages) return;
    chatMessages.innerHTML = '<p style="text-align: center; color: var(--text-muted);">Loading messages...</p>';
    try {
        const response = await apiRequest('/message/all', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ticketId: currentTicketId })
        }, role, onLogout);
        if (!response) return;
        const messages = await response.json();
        renderMessages(messages, role);
    } catch (err) {
        chatMessages.innerHTML = 'Error loading messages.';
    }
}

function renderMessages(messages, role) {
    const chatMessages = document.getElementById('chatMessages');
    if (!chatMessages) return;
    chatMessages.innerHTML = '';
    const userId = window.currentUserId || '';
    messages.forEach(msg => {
        const div = document.createElement('div');
        const isSentByMe = msg.senderId === userId;
        const classes = ['message'];
        if (isSentByMe) classes.push('sent');
        else classes.push(role === 'user' ? 'admin' : 'received');
        div.className = classes.join(' ');
        div.innerHTML = `${msg.content}${isSentByMe ? `<div class="delete-msg" onclick="window.deleteMessage('${msg.id}')">✖</div>` : ''}`;
        chatMessages.appendChild(div);
    });
    chatMessages.scrollTop = chatMessages.scrollHeight;
}

export async function sendMessage(role, onLogout, loadMessagesFn) {
    const input = document.getElementById('messageInput');
    if (!input) return;
    const content = input.value.trim();
    if (!content) return;
    try {
        const response = await apiRequest('/message', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ content, ticketId: currentTicketId })
        }, role, onLogout);
        if (response && response.ok) {
            input.value = '';
            if (loadMessagesFn) await loadMessagesFn(role, onLogout);
        }
    } catch (err) {
        console.error(err);
    }
}

export async function deleteMessage(messageId, role, onLogout, loadMessagesFn) {
    if (!confirm('Delete this message?')) return;
    try {
        const response = await apiRequest('/message', {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ messageId })
        }, role, onLogout);
        if (response && response.ok) {
            if (loadMessagesFn) await loadMessagesFn(role, onLogout);
        }
    } catch (err) {
        console.error(err);
    }
}

export async function assignTicketToMe(role, onLogout, loadUnassignedFn, closeModalFn) {
    try {
        const response = await apiRequest('/ticket/assign', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ticketId: currentTicketId })
        }, role, onLogout);
        if (response && response.ok) {
            if (closeModalFn) closeModalFn('ticketDetailModal');
            if (loadUnassignedFn) await loadUnassignedFn();
        }
    } catch (err) {
        console.error(err);
    }
}

export async function solveTicket(role, onLogout, loadMyTicketsFn, closeModalFn) {
    try {
        const response = await apiRequest('/ticket/solve', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ ticketId: currentTicketId })
        }, role, onLogout);
        if (response && response.ok) {
            if (closeModalFn) closeModalFn('ticketDetailModal');
            if (loadMyTicketsFn) await loadMyTicketsFn();
        }
    } catch (err) {
        console.error(err);
    }
}

export async function submitTicket(title, role, onLogout, onSuccess, closeModalFn) {
    if (!title.trim()) return;
    try {
        const response = await apiRequest('/ticket', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title })
        }, role, onLogout);
        if (response && response.ok) {
            if (closeModalFn) closeModalFn('createTicketModal');
            if (onSuccess) onSuccess();
        } else {
            const data = await response.json();
            alert(data.message || 'Error creating ticket');
        }
    } catch (err) {
        alert('Request failed');
    }
}
