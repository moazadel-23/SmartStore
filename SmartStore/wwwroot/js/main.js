document.addEventListener("DOMContentLoaded", () => {

    // ── Sidebar Toggle ──────────────────────────────────────────
    const sidebar   = document.querySelector(".sidebar");
    const closeBtn  = document.querySelector("#btn");

    if (closeBtn && sidebar) {
        closeBtn.addEventListener("click", () => {
            sidebar.classList.toggle("close");
            menuBtnChange();
        });
    }

    function menuBtnChange() {
        if (!closeBtn) return;
        if (sidebar.classList.contains("close")) {
            closeBtn.classList.replace("bx-arrow-from-right", "bx-arrow-to-right");
        } else {
            closeBtn.classList.replace("bx-arrow-to-right", "bx-arrow-from-right");
        }
    }

    // ── Sidebar Submenus Toggle ──────────────────────────────────
    const submenuToggles = document.querySelectorAll(".submenu-toggle");
    submenuToggles.forEach(toggle => {
        toggle.addEventListener("click", (e) => {
            const parentLi = toggle.parentElement;
            parentLi.classList.toggle("showMenu");
        });
    });

    // ── Mobile Sidebar Toggle ──────────────────────────────────
    const mobileToggle = document.getElementById("mobile-toggle");
    
    if (mobileToggle && sidebar) {
        mobileToggle.addEventListener("click", () => {
            sidebar.classList.toggle("mobile-open");
            document.body.classList.toggle("mobile-sidebar-open");
        });

        // Close sidebar when clicking outside on mobile
        document.addEventListener("click", (e) => {
            if (window.innerWidth <= 768) {
                if (sidebar.classList.contains("mobile-open") && 
                    !sidebar.contains(e.target) && 
                    !mobileToggle.contains(e.target)) {
                    
                    sidebar.classList.remove("mobile-open");
                    document.body.classList.remove("mobile-sidebar-open");
                }
            }
        });
    }

    // ── Active nav link highlight ──────────────────────────────
    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll(".sidebar li a").forEach(link => {
        const href = link.getAttribute("href") || "";
        if (href !== "#" && currentPath.includes(href.toLowerCase().split("/").pop())) {
            link.classList.add("active");
            
            // If inside a submenu, expand parent
            const parentSubmenu = link.closest(".has-submenu");
            if (parentSubmenu) {
                parentSubmenu.classList.add("showMenu");
            }
        }
    });

    // ── 3-Mode Theme System (Light / Dark / Auto) ───────────────
    const body          = document.body;
    const pickerBtn     = document.getElementById("themePickerBtn");
    const pickerIcon    = document.getElementById("themePickerIcon");
    const dropdown      = document.getElementById("themeDropdown");
    const themeOptions  = document.querySelectorAll(".theme-option");
    const systemDarkMQ  = window.matchMedia("(prefers-color-scheme: dark)");

    // Icons for each mode
    const themeIcons = { light: "bx-sun", dark: "bx-moon", auto: "bx-laptop" };

    // Apply dark-mode class based on resolved theme
    function applyTheme(mode) {
        let shouldBeDark = false;
        if (mode === "dark") shouldBeDark = true;
        else if (mode === "auto") shouldBeDark = systemDarkMQ.matches;
        // else light → false

        body.classList.toggle("dark-mode", shouldBeDark);

        // Update picker button icon
        if (pickerIcon) {
            pickerIcon.className = "bx " + themeIcons[mode];
        }

        // Update active state on dropdown options
        themeOptions.forEach(opt => {
            opt.classList.toggle("active", opt.dataset.theme === mode);
        });
    }

    // Initialize: read stored preference or default to "dark"
    const storedTheme = localStorage.getItem("adminTheme") || "dark";
    applyTheme(storedTheme);

    // Listen for system dark mode changes (relevant when mode === "auto")
    systemDarkMQ.addEventListener("change", () => {
        const current = localStorage.getItem("adminTheme") || "light";
        if (current === "auto") applyTheme("auto");
    });

    // Toggle dropdown open/close
    if (pickerBtn && dropdown) {
        pickerBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            const isOpen = dropdown.classList.toggle("open");
            pickerBtn.setAttribute("aria-expanded", isOpen);
        });

        // Close dropdown when clicking outside
        document.addEventListener("click", (e) => {
            if (!dropdown.contains(e.target) && !pickerBtn.contains(e.target)) {
                dropdown.classList.remove("open");
                pickerBtn.setAttribute("aria-expanded", "false");
            }
        });

        // Close on Escape key
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape") {
                dropdown.classList.remove("open");
                pickerBtn.setAttribute("aria-expanded", "false");
            }
        });
    }

    // Handle option clicks
    themeOptions.forEach(opt => {
        opt.addEventListener("click", (e) => {
            e.stopPropagation();
            const chosen = opt.dataset.theme;
            localStorage.setItem("adminTheme", chosen);
            applyTheme(chosen);
            // Close dropdown after selection
            dropdown?.classList.remove("open");
            pickerBtn?.setAttribute("aria-expanded", "false");
        });
    });

    // ── Notification badge pulse ───────────────────────────────
    const badge = document.querySelector(".top-nav .badge");
    if (badge) {
        badge.style.animation = "badgePulse 1.8s infinite";
    }

    // ── Inject badge pulse keyframes ──────────────────────────
    if (!document.getElementById("adminDynamicStyles")) {
        const style = document.createElement("style");
        style.id = "adminDynamicStyles";
        style.textContent = `
          @keyframes badgePulse {
            0%, 100% { transform: scale(1); }
            50%       { transform: scale(1.25); }
          }
          .top-nav .badge { display: flex; align-items: center; justify-content: center; }
          .sidebar li a.active { background: rgba(16, 185, 129, 0.12) !important; border-left: none !important; border-radius: 8px !important; padding-left: 12px !important; }
          .sidebar li a.active .links_name,
          .sidebar li a.active i { color: #10b981 !important; }
        `;
        document.head.appendChild(style);
    }

    // ── Profile Menu Dropdown ──────────────────────────────────
    const profileBtn = document.getElementById("profileAvatarBtn");
    const profileMenu = document.getElementById("profileMenu");

    if (profileBtn && profileMenu) {
        profileBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            profileBtn.classList.toggle("active");
            profileMenu.classList.toggle("open");
        });

        // Close dropdown when clicking outside
        document.addEventListener("click", (e) => {
            if (!profileMenu.contains(e.target) && !profileBtn.contains(e.target)) {
                profileBtn.classList.remove("active");
                profileMenu.classList.remove("open");
            }
        });

        // Close on Escape key
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape") {
                profileBtn.classList.remove("active");
                profileMenu.classList.remove("open");
            }
        });
    }

    // ── Log out click ──────────────────────────────────────────
    const pmLogout = document.getElementById("pmLogout");
    if (pmLogout) {
        pmLogout.addEventListener("click", (e) => {
            e.preventDefault();
            window.location.href = pmLogout.getAttribute("href") || "/";
        });
    }

    // ── Smooth entrance animation for KPI cards ────────────────
    const kpiCards = document.querySelectorAll(".kpi-card");
    kpiCards.forEach((card, i) => {
        card.style.opacity = "0";
        card.style.transform = "translateY(20px)";
        card.style.transition = `opacity 0.5s ease ${i * 100}ms, transform 0.5s ease ${i * 100}ms`;
        requestAnimationFrame(() => {
            setTimeout(() => {
                card.style.opacity = "1";
                card.style.transform = "translateY(0)";
            }, 50);
        });
    });

    // ── Notification Drawer Toggle ──────────────────────────────
    const notificationBtn = document.getElementById("notificationBtn");
    const notificationDrawer = document.getElementById("notificationDrawer");
    const closeNotificationDrawer = document.getElementById("closeNotificationDrawer");
    const drawerOverlay = document.getElementById("drawerOverlay");

    if (notificationBtn && notificationDrawer && drawerOverlay) {
        notificationBtn.addEventListener("click", (e) => {
            e.stopPropagation();
            notificationDrawer.classList.add("open");
            drawerOverlay.classList.add("active");
        });
    }

    if (closeNotificationDrawer && notificationDrawer && drawerOverlay) {
        closeNotificationDrawer.addEventListener("click", () => {
            notificationDrawer.classList.remove("open");
            drawerOverlay.classList.remove("active");
        });
    }

    if (drawerOverlay && notificationDrawer) {
        drawerOverlay.addEventListener("click", () => {
            notificationDrawer.classList.remove("open");
            drawerOverlay.classList.remove("active");
        });
    }

    // ── Notification Tabs click ─────────────────────────────────
    const ndTabs = document.querySelectorAll(".nd-tab");
    ndTabs.forEach(tab => {
        tab.addEventListener("click", () => {
            ndTabs.forEach(t => t.classList.remove("active"));
            tab.classList.add("active");
        });
    });

});
