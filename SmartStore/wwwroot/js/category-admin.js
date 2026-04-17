// category-admin.js — all logic is now inline in the View's @section Scripts
// This file is kept for any future shared category utilities.

/**
 * Show a toast notification
 * @param {string} message
 * @param {number} duration ms
 */
function showCatToast(message, duration = 3000) {
    const toast = document.getElementById('catToast');
    if (!toast) return;
    toast.textContent = message;
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), duration);
}
