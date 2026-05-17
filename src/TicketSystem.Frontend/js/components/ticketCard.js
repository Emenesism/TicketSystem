import { formatDate, formatDateTime, getValue } from '../core/utils.js';

export function createTicketCard(ticket, isUnassigned, onOpenDetail) {
    const card = document.createElement('div');
    card.className = 'ticket-card animate-fade-in';
    card.onclick = () => onOpenDetail(ticket, isUnassigned);

    const id = getValue(ticket, 'id') || '';
    const title = getValue(ticket, 'title') || 'No Title';
    const createdAt = getValue(ticket, 'createdAt');
    const userName = getValue(ticket, 'userName');
    const isSolved = getValue(ticket, 'isSolved') !== undefined ? getValue(ticket, 'isSolved') : getValue(ticket, 'IsSolved');
    const statusClass = isSolved ? 'status-solved' : 'status-open';
    const statusText = isSolved ? 'Solved' : 'Open';

    card.innerHTML = `
        <div style="display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 8px;">
            <div class="title">${title}</div>
            <span class="status-tag ${statusClass}">${statusText}</span>
        </div>
        <div class="meta">
            <span>${userName ? `By ${userName}` : `ID: ${id.substring(0, 8)}...`}</span>
            <span>${createdAt ? (userName ? formatDateTime(createdAt) : formatDate(createdAt)) : '—'}</span>
        </div>
    `;

    return card;
}
