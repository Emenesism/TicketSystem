export function setAdminTab(tab, views, navs) {
    Object.keys(views).forEach((key) => {
        const view = document.getElementById(views[key]);
        const nav = document.getElementById(navs[key]);
        if (view) view.style.display = key === tab ? 'block' : 'none';
        if (nav) nav.classList.toggle('active', key === tab);
    });
}

export function setPageHeader(titleId, subtitleId, title, subtitle) {
    const titleEl = document.getElementById(titleId);
    const subtitleEl = document.getElementById(subtitleId);
    if (titleEl) titleEl.textContent = title;
    if (subtitleEl) subtitleEl.textContent = subtitle;
}

export function showSuperAdminNavigation(isSuperAdmin) {
    const adminsNav = document.getElementById('navAdmins');
    const dashboardNav = document.getElementById('navDashboard');
    if (adminsNav) adminsNav.style.display = isSuperAdmin ? 'flex' : 'none';
    if (dashboardNav) dashboardNav.style.display = isSuperAdmin ? 'flex' : 'none';
}
