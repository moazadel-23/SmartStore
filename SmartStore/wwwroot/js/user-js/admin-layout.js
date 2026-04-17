/**
 * ================================================================
 *  Admin Layout Loader  -  admin-layout.js
 * ================================================================
 *  بيعمل fetch لملف components/admin-template.html
 *  ويستخرج منه:
 *    - #sidebar      → ينزله مكان #admin-template-placeholder
 *    - .admin-topbar → ينزله في أول #content (فوق #page-content)
 *
 *  طريقة الاستخدام في كل صفحة أدمن:
 *  ====================================
 *
 *  <body>
 *    <div id="admin-wrapper">
 *
 *      <!-- JS بيبدّل الـ placeholder ده بالـ sidebar -->
 *      <div id="admin-template-placeholder"
 *           data-page-title="اسم الصفحة"
 *           data-page-subtitle="وصف اختياري (optional)">
 *      </div>
 *
 *      <!-- JS بيحط الـ navbar فوق page-content -->
 *      <div id="content">
 *        <div id="page-content">
 *          ... محتوى الصفحة هنا ...
 *        </div>
 *      </div>
 *
 *    </div>
 *    <script src="js/admin-layout.js"></script>
 *  </body>
 *
 * ================================================================
 */

(function () {
    'use strict';

    var TEMPLATE_PATH = './components/admin-template.html';

    /* ── Main entry point ─────────────────────────────────────── */
    function init() {
        var placeholder = document.getElementById('admin-template-placeholder');
        if (!placeholder) return; // ليست صفحة أدمن

        var title    = placeholder.getAttribute('data-page-title')    || 'Dashboard';
        var subtitle = placeholder.getAttribute('data-page-subtitle') || '';

        fetch(TEMPLATE_PATH)
            .then(function (res) {
                if (!res.ok) throw new Error('HTTP ' + res.status + ' – Cannot load admin-template.html');
                return res.text();
            })
            .then(function (html) {
                /* ── Parse template ─────────────────────────── */
                var temp = document.createElement('div');
                temp.innerHTML = html;

                var sidebar = temp.querySelector('#sidebar');
                var topbar  = temp.querySelector('.admin-topbar');

                if (!sidebar || !topbar) {
                    console.error('[AdminLayout] Template missing #sidebar or .admin-topbar');
                    return;
                }

                /* ── Inject sidebar: replace placeholder ────── */
                var wrapper = placeholder.parentNode;
                wrapper.insertBefore(sidebar, placeholder);
                placeholder.remove();

                /* ── Inject topbar: prepend into #content ────── */
                var content = document.getElementById('content');
                if (content) {
                    content.insertBefore(topbar, content.firstChild);
                } else {
                    console.warn('[AdminLayout] #content not found – topbar not injected');
                }

                /* ── Set page title / subtitle ───────────────── */
                var titleEl    = document.getElementById('admin-page-title');
                var subtitleEl = document.getElementById('admin-page-subtitle');

                if (titleEl)    { titleEl.textContent = title; }
                if (subtitleEl) {
                    if (subtitle) {
                        subtitleEl.textContent  = subtitle;
                        subtitleEl.style.display = 'block';
                    } else {
                        subtitleEl.style.display = 'none';
                    }
                }

                /* ── Highlight active sidebar link ───────────── */
                var currentPage = window.location.pathname.split('/').pop() || 'admin-dashboard.html';
                var navItems    = document.querySelectorAll('#sidebar ul li[data-page]');
                navItems.forEach(function (li) {
                    li.classList.toggle('active', li.getAttribute('data-page') === currentPage);
                });

                /* ── Mobile sidebar toggle ───────────────────── */
                var toggleBtn = document.querySelector('.sidebar-toggle');
                var sidebarEl = document.getElementById('sidebar');
                var overlayEl = document.querySelector('.sidebar-overlay');

                if (toggleBtn && sidebarEl) {
                    toggleBtn.addEventListener('click', function () {
                        sidebarEl.classList.add('active');
                        if (overlayEl) overlayEl.classList.add('active');
                    });
                }

                if (overlayEl && sidebarEl) {
                    overlayEl.addEventListener('click', function () {
                        sidebarEl.classList.remove('active');
                        overlayEl.classList.remove('active');
                    });
                }
            })
            .catch(function (err) {
                console.error('[AdminLayout]', err.message);
            });
    }

    /* ── Bootstrap ────────────────────────────────────────────── */
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }

})();
