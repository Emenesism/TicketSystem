export function renderPaginationControls({
    containerId,
    infoId,
    pageButtonsId,
    prevButtonId,
    nextButtonId,
    limitSelectId,
    currentPage,
    currentLimit,
    totalCount,
    totalPages,
    onPageFunctionName = 'goToPage',
}) {
    const container = document.getElementById(containerId);
    const info = document.getElementById(infoId);
    const pageButtons = document.getElementById(pageButtonsId);
    const prevBtn = document.getElementById(prevButtonId);
    const nextBtn = document.getElementById(nextButtonId);
    const limitSelect = document.getElementById(limitSelectId);
    if (!container) return;

    container.style.display = 'flex';
    if (limitSelect) limitSelect.value = currentLimit;

    const start = (currentPage - 1) * currentLimit + 1;
    const end = Math.min(currentPage * currentLimit, totalCount);
    if (info) info.textContent = `Showing ${start}-${end} of ${totalCount}`;

    if (prevBtn) prevBtn.disabled = currentPage <= 1;
    if (nextBtn) nextBtn.disabled = currentPage >= totalPages;
    if (!pageButtons) return;

    pageButtons.innerHTML = '';
    const maxVisible = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxVisible / 2));
    let endPage = Math.min(totalPages, startPage + maxVisible - 1);

    if (endPage - startPage < maxVisible - 1) {
        startPage = Math.max(1, endPage - maxVisible + 1);
    }

    if (startPage > 1) {
        pageButtons.innerHTML += `<button class="pagination-btn" onclick="window.${onPageFunctionName}(1)">1</button>`;
        if (startPage > 2) {
            pageButtons.innerHTML += '<span style="color: var(--text-muted); padding: 0 4px;">...</span>';
        }
    }

    for (let i = startPage; i <= endPage; i++) {
        pageButtons.innerHTML += `<button class="pagination-btn ${i === currentPage ? 'active' : ''}" onclick="window.${onPageFunctionName}(${i})">${i}</button>`;
    }

    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            pageButtons.innerHTML += '<span style="color: var(--text-muted); padding: 0 4px;">...</span>';
        }
        pageButtons.innerHTML += `<button class="pagination-btn" onclick="window.${onPageFunctionName}(${totalPages})">${totalPages}</button>`;
    }
}
