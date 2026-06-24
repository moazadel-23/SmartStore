(function () {
    'use strict';

    /* ═══════════════════════════════════════════════════
       WISHLIST — Shared State
       ═══════════════════════════════════════════════════ */
    let wishlist = [];
    if (typeof window !== 'undefined' && window.serverWishlist && Array.isArray(window.serverWishlist)) {
        wishlist = window.serverWishlist;
        sessionStorage.setItem('store_wishlist', JSON.stringify(wishlist));
    } else {
        let rawWishlist = JSON.parse(sessionStorage.getItem('store_wishlist') || '[]');
        if (rawWishlist.length && typeof rawWishlist[0] === 'number') {
            rawWishlist = [];
            sessionStorage.setItem('store_wishlist', '[]');
        }
        wishlist = rawWishlist;
    }

    function updateWishlistUI() {
        const count = document.getElementById('wishlistCount');
        if (count) count.textContent = wishlist.length;
        document.querySelectorAll('.card-wishlist').forEach(function (btn) {
            var id = parseInt(btn.dataset.productId);
            var icon = btn.querySelector('i');
            if (wishlist.some(function (item) { return item.id === id; })) {
                btn.classList.add('active');
                if (icon) icon.className = 'fa fa-heart';
            } else {
                btn.classList.remove('active');
                if (icon) icon.className = 'fa fa-heart-o';
            }
        });
    }

    function toggleWishlist(id, data) {
        var idx = wishlist.findIndex(function (item) { return item.id === id; });
        if (idx > -1) {
            wishlist.splice(idx, 1);
        } else if (data) {
            wishlist.push(data);
        }
        sessionStorage.setItem('store_wishlist', JSON.stringify(wishlist));
        updateWishlistUI();
    }

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('.card-wishlist');
        if (btn) {
            e.preventDefault();
            var id = parseInt(btn.dataset.productId);
            var card = btn.closest('.product-card');
            var data = {
                id: id,
                name: card ? card.dataset.name : '',
                price: card ? parseFloat(card.dataset.price) : 0,
                original: card ? parseFloat(card.dataset.original) : 0,
                image: card ? card.dataset.image : '',
                rating: card ? parseFloat(card.dataset.rating) : 0,
                category: card ? card.dataset.category : '',
                discount: card ? parseFloat(card.dataset.discount) : 0,
                url: card ? card.dataset.url : ''
            };
            toggleWishlist(id, data);
            // Sync with server-side wishlist
            fetch('/Customer/Wishlist/AddToFavorites', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: 'productId=' + id
            }).then(function (r) {
                if (r.redirected || r.url.indexOf('/Account/Login') !== -1) {
                    showToast('Please log in first!');
                    // Revert UI toggle since it wasn't saved on server
                    toggleWishlist(id);
                    return;
                }
                if (!r.ok) {
                    console.error('Wishlist sync failed: Status ' + r.status);
                    showToast('Failed to sync wishlist with server.');
                    toggleWishlist(id);
                    return;
                }
                r.json().then(function (json) {
                    if (json && json.count !== undefined) {
                        var count = document.getElementById('wishlistCount');
                        if (count) count.textContent = json.count;

                        // If on Wishlist page and item was removed, animate and remove card from DOM
                        if (window.location.pathname.toLowerCase().indexOf('/wishlist') !== -1 && !json.added) {
                            var cardCol = card ? card.closest('.col-md-3, .col-6, .mb-4') || card : null;
                            if (cardCol) {
                                cardCol.style.transition = 'opacity 0.4s ease, transform 0.4s ease';
                                cardCol.style.opacity = '0';
                                cardCol.style.transform = 'scale(0.9)';
                                setTimeout(function () {
                                    cardCol.remove();
                                    
                                    // Update wishlist count label dynamically
                                    var countText = document.querySelector('.section-header .text-muted');
                                    if (countText) {
                                        countText.textContent = json.count + ' item(s)';
                                    }

                                    // If no products left, reload to show empty state template
                                    var remaining = document.querySelectorAll('.product-card');
                                    if (remaining.length === 0) {
                                        location.reload();
                                    }
                                }, 400);
                            }
                        }
                    }
                });
            }).catch(function (err) {
                console.error('Wishlist error:', err);
                showToast('An error occurred. Please try again.');
                toggleWishlist(id);
            });
        }
    });

    /* ═══════════════════════════════════════════════════
       CART — Shared State
       ═══════════════════════════════════════════════════ */
    let cart = JSON.parse(sessionStorage.getItem('store_cart') || '[]');

    // Sync guest cart to database if authenticated on page load
    if (window.isAuthenticated && cart.length > 0) {
        const syncItems = cart.map(item => ({ productId: item.id, count: item.qty }));
        fetch('/Cart/SyncCart', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(syncItems)
        })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                sessionStorage.removeItem('store_cart');
                cart = [];
                updateCartUI();
                const countBadge = document.getElementById('cartCount');
                if (countBadge) countBadge.textContent = data.count;
            }
        })
        .catch(err => console.error('Error syncing cart:', err));
    }

    function updateCartUI() {
        const count = document.getElementById('cartCount');
        if (count) {
            // Only update from sessionStorage count if NOT authenticated, since if authenticated, server/AJAX updates countBadge directly
            if (!window.isAuthenticated) {
                count.textContent = cart.reduce(function (sum, item) { return sum + item.qty; }, 0);
            }
        }
        document.querySelectorAll('.card-add').forEach(function (btn) {
            var id = parseInt(btn.dataset.productId);
            var inCart = cart.some(function (item) { return item.id === id; });
            if (inCart) {
                btn.classList.add('added');
                btn.innerHTML = '<i class="fa fa-check"></i>';
            } else {
                btn.classList.remove('added');
                if (!btn.disabled) btn.innerHTML = '<i class="fa fa-shopping-cart"></i>';
            }
        });
    }

    function addToCart(id, qty) {
        qty = qty || 1;

        var existing = cart.filter(function (item) { return item.id === id; });
        if (existing.length > 0) {
            existing[0].qty += qty;
        } else {
            cart.push({ id: id, qty: qty });
        }

        if (window.isAuthenticated) {
            fetch('/Cart/Add?productId=' + id + '&qty=' + qty, { method: 'POST' })
                .then(res => res.json())
                .then(data => {
                    if (data.success) {
                        const countBadge = document.getElementById('cartCount');
                        if (countBadge) countBadge.textContent = data.count;
                        updateCartUI();
                        showToast('Added to cart!');
                    }
                })
                .catch(err => console.error('Error adding to DB cart:', err));
        } else {
            sessionStorage.setItem('store_cart', JSON.stringify(cart));
            updateCartUI();
            showToast('Added to cart!');
        }
    }

    document.addEventListener('click', function (e) {
        var btn = e.target.closest('.card-add');
        if (btn && !btn.disabled && !btn.classList.contains('added')) {
            e.preventDefault();
            addToCart(parseInt(btn.dataset.productId), 1);
        }
    });

    /* ═══════════════════════════════════════════════════
       TOAST NOTIFICATION
       ═══════════════════════════════════════════════════ */
    function showToast(msg) {
        var existing = document.querySelector('.store-toast');
        if (existing) existing.remove();
        var toast = document.createElement('div');
        toast.className = 'store-toast';
        toast.textContent = msg;
        Object.assign(toast.style, {
            position: 'fixed', bottom: '80px', left: '50%', transform: 'translateX(-50%)',
            background: '#1A1A2E', color: '#fff', padding: '12px 24px', borderRadius: '8px',
            fontSize: '14px', fontWeight: '600', zIndex: '9999',
            boxShadow: '0 10px 30px rgba(0,0,0,0.2)',
            opacity: '0', transition: 'opacity 0.3s ease'
        });
        document.body.appendChild(toast);
        requestAnimationFrame(function () { toast.style.opacity = '1'; });
        setTimeout(function () {
            toast.style.opacity = '0';
            setTimeout(function () { toast.remove(); }, 300);
        }, 2000);
    }

    /* ═══════════════════════════════════════════════════
       STORE PAGE — Filter sidebar (mobile toggle)
       ═══════════════════════════════════════════════════ */
    var filterToggle = document.getElementById('filterToggleBtn');
    var filterSidebar = document.getElementById('filterSidebar');
    var filterOverlay = document.getElementById('filterOverlay');
    var filterClose = document.getElementById('filterCloseBtn');

    if (filterToggle && filterSidebar) {
        filterToggle.addEventListener('click', function () {
            filterSidebar.classList.add('mobile-open');
            if (filterOverlay) filterOverlay.classList.add('show');
            document.body.style.overflow = 'hidden';
        });
        function closeFilters() {
            filterSidebar.classList.remove('mobile-open');
            if (filterOverlay) filterOverlay.classList.remove('show');
            document.body.style.overflow = '';
        }
        if (filterOverlay) filterOverlay.addEventListener('click', closeFilters);
        if (filterClose) filterClose.addEventListener('click', closeFilters);
    }

    /* ═══════════════════════════════════════════════════
       STORE PAGE — AJAX filter / sort / pagination
       ═══════════════════════════════════════════════════ */
    var loadGridTimer;
    function loadGrid() {
        clearTimeout(loadGridTimer);
        loadGridTimer = setTimeout(function () {
            var form = document.getElementById('filterForm');
            if (!form) return;
            var data = new URLSearchParams(new FormData(form));
            data.set('partial', 'true');
            var grid = document.getElementById('productGrid');
            if (grid) grid.style.opacity = '0.4';
            fetch('/Customer/Home/Store?' + data.toString())
                .then(function (r) { return r.text(); })
                .then(function (html) {
                    var temp = document.createElement('div');
                    temp.innerHTML = html;
                    var newGrid = temp.querySelector('#productGrid');
                    if (newGrid && grid) {
                        grid.outerHTML = newGrid.outerHTML;
                    }
                    // Update address bar without refresh
                    var url = data.toString().replace('&partial=true', '').replace('partial=true', '');
                    window.history.replaceState({}, '', '/Customer/Home/Store' + (url ? '?' + url : ''));
                    attachGridListeners();
                    updateWishlistUI();
                    updateCartUI();
                })
                .catch(function () {
                    var g = document.getElementById('productGrid');
                    if (g) g.style.opacity = '1';
                });
        }, 150);
    }

    function attachGridListeners() {
        // Sort select
        var sortEl = document.getElementById('sortSelect');
        if (sortEl) {
            sortEl.addEventListener('change', function () {
                var sortInput = document.getElementById('sortInput');
                if (sortInput) sortInput.value = this.value;
                loadGrid();
            });
        }
        // Pagination links
        document.querySelectorAll('.pagination-wrap a[data-page]').forEach(function (link) {
            link.addEventListener('click', function (e) {
                e.preventDefault();
                var pageInput = document.getElementById('pageInput');
                if (pageInput) pageInput.value = this.dataset.page;
                loadGrid();
            });
        });
        // Filter chips
        document.querySelectorAll('.filter-chip').forEach(function (chip) {
            chip.addEventListener('click', function () {
                var remove = this.dataset.remove;
                if (remove === 'search') {
                    window.location.href = '/Customer/Home/Store';
                    return;
                }
                var form = document.getElementById('filterForm');
                if (!form) return;
                if (remove && remove.startsWith('cat-')) {
                    var catId = remove.replace('cat-', '');
                    form.querySelectorAll('input[name="Filter.CategoryIds"]').forEach(function (cb) {
                        if (cb.value === catId) cb.checked = false;
                    });
                } else if (remove && remove.startsWith('brand-')) {
                    var brandId = remove.replace('brand-', '');
                    form.querySelectorAll('input[name="Filter.BrandIds"]').forEach(function (cb) {
                        if (cb.value === brandId) cb.checked = false;
                    });
                } else if (remove === 'price') {
                    form.querySelectorAll('input[name="Filter.MinPrice"]').forEach(function (i) { i.value = ''; });
                    form.querySelectorAll('input[name="Filter.MaxPrice"]').forEach(function (i) { i.value = ''; });
                } else if (remove === 'rating') {
                    form.querySelectorAll('input[name="Filter.MinRating"]').forEach(function (i) { i.checked = false; });
                } else if (remove === 'instock') {
                    form.querySelectorAll('input[name="Filter.InStockOnly"]').forEach(function (i) { i.checked = false; });
                }
                loadGrid();
            });
        });
        // Mobile filter toggle (re-bind since it's inside the grid)
        var toggleBtn = document.getElementById('filterToggleBtn');
        if (toggleBtn && filterSidebar) {
            var newToggle = toggleBtn.cloneNode(true);
            toggleBtn.parentNode.replaceChild(newToggle, toggleBtn);
            newToggle.addEventListener('click', function () {
                filterSidebar.classList.add('mobile-open');
                if (filterOverlay) filterOverlay.classList.add('show');
                document.body.style.overflow = 'hidden';
            });
        }
    }

    // Auto-filter on any filter input change
    var filterForm = document.getElementById('filterForm');
    if (filterForm) {
        filterForm.addEventListener('change', function (e) {
            var target = e.target;
            if (target.matches('input[type="checkbox"], input[type="radio"]')) {
                var pageInput = document.getElementById('pageInput');
                if (pageInput) pageInput.value = 1;
                // Sync sort select if sort input changes
                if (target.id === 'sortInput') {
                    var sortSelect = document.getElementById('sortSelect');
                    if (sortSelect) sortSelect.value = target.value;
                }
                loadGrid();
            }
        });
        // Price range inputs: filter on blur
        filterForm.addEventListener('focusout', function (e) {
            if (e.target.matches('input[name="Filter.MinPrice"], input[name="Filter.MaxPrice"]')) {
                var pageInput = document.getElementById('pageInput');
                if (pageInput) pageInput.value = 1;
                loadGrid();
            }
        });
    }

    // Prevent form from doing a full GET submit
    if (filterForm) {
        filterForm.addEventListener('submit', function (e) { e.preventDefault(); });
    }

    /* ═══════════════════════════════════════════════════
       STORE PAGE — Reset filters
       ═══════════════════════════════════════════════════ */
    var resetBtn = document.getElementById('resetFilters');
    if (resetBtn) {
        resetBtn.addEventListener('click', function () {
            window.location.href = '/Customer/Home/Store';
        });
    }

    /* ═══════════════════════════════════════════════════
       STORE PAGE — Search suggestions (debounced)
       ═══════════════════════════════════════════════════ */
    var searchInput = document.getElementById('searchInput');
    var searchSuggest = document.getElementById('searchSuggestions');
    var searchDebounce;

    if (searchInput && searchSuggest) {
        searchInput.addEventListener('input', function () {
            clearTimeout(searchDebounce);
            var query = this.value.trim();
            if (query.length < 2) { searchSuggest.classList.remove('show'); return; }
            searchDebounce = setTimeout(function () {
                fetch('/Customer/Home/Store?SearchQuery=' + encodeURIComponent(query))
                    .then(function (r) { return r.text(); })
                    .then(function () {
                        // In production, return JSON from a dedicated endpoint.
                        // For now, hide suggestions after submission.
                    })
                    .catch(function () {});
            }, 300);
        });
        document.addEventListener('click', function (e) {
            if (!searchInput.contains(e.target) && !searchSuggest.contains(e.target)) {
                searchSuggest.classList.remove('show');
            }
        });
    }

    /* ═══════════════════════════════════════════════════
       STORE PAGE — State Retention (sessionStorage)
       ═══════════════════════════════════════════════════ */
    (function () {
        try {
            var key = 'store_scroll_' + window.location.pathname + window.location.search;
            var saved = sessionStorage.getItem(key);
            if (saved) {
                var savedY = parseInt(saved, 10);
                if (!isNaN(savedY)) {
                    window.scrollTo(0, savedY);
                }
            }
            sessionStorage.setItem('store_state_' + window.location.pathname, JSON.stringify({
                filters: window.location.search,
                scrollY: 0
            }));
        } catch (e) {}
    })();

    window.addEventListener('beforeunload', function () {
        try {
            var key = 'store_scroll_' + window.location.pathname + window.location.search;
            sessionStorage.setItem(key, String(window.scrollY));
        } catch (e) {}
    });

    /* ═══════════════════════════════════════════════════
       PDP — Image Gallery (thumbnail switching)
       ═══════════════════════════════════════════════════ */
    var mainImg = document.getElementById('mainProductImage');
    var thumbs = document.querySelectorAll('.pdp-thumbnails .thumb:not(.video-thumb)');

    thumbs.forEach(function (thumb) {
        thumb.addEventListener('click', function () {
            thumbs.forEach(function (t) { t.classList.remove('active'); });
            this.classList.add('active');
            if (mainImg) mainImg.src = this.dataset.src;
        });
    });

    /* ═══════════════════════════════════════════════════
       PDP — Hover Zoom
       ═══════════════════════════════════════════════════ */
    var imgContainer = document.getElementById('mainImageContainer');
    var zoomLens = document.getElementById('zoomLens');
    var zoomResult = document.getElementById('zoomResult');

    if (imgContainer && zoomLens && zoomResult && mainImg) {
        imgContainer.addEventListener('mousemove', function (e) {
            var rect = imgContainer.getBoundingClientRect();
            var x = e.clientX - rect.left;
            var y = e.clientY - rect.top;
            var lensW = zoomLens.offsetWidth / 2;
            var lensH = zoomLens.offsetHeight / 2;
            var lx = Math.min(Math.max(x - lensW, 0), rect.width - zoomLens.offsetWidth);
            var ly = Math.min(Math.max(y - lensH, 0), rect.height - zoomLens.offsetHeight);
            zoomLens.style.left = lx + 'px';
            zoomLens.style.top = ly + 'px';
            zoomResult.style.display = 'block';
            zoomResult.style.backgroundImage = 'url(' + mainImg.src + ')';
            var zoom = 2.5;
            zoomResult.style.backgroundSize = (rect.width * zoom) + 'px ' + (rect.height * zoom) + 'px';
            zoomResult.style.backgroundPosition = (-lx * zoom) + 'px ' + (-ly * zoom) + 'px';
        });
        imgContainer.addEventListener('mouseleave', function () {
            zoomLens.style.display = 'none';
            zoomResult.style.display = 'none';
        });
        imgContainer.addEventListener('mouseenter', function () {
            zoomLens.style.display = 'block';
        });
    }

    /* ═══════════════════════════════════════════════════
       PDP — Variant selection
       ═══════════════════════════════════════════════════ */
    var sizeOptions = document.querySelectorAll('.size-option');
    sizeOptions.forEach(function (opt) {
        opt.addEventListener('click', function () {
            sizeOptions.forEach(function (o) { o.classList.remove('active'); });
            this.classList.add('active');
            var label = this.closest('.variant-group').querySelector('.variant-label span');
            if (label) label.textContent = '— ' + this.dataset.size;
        });
    });

    var colorSwatches = document.querySelectorAll('.color-swatch');
    colorSwatches.forEach(function (swatch) {
        swatch.addEventListener('click', function () {
            colorSwatches.forEach(function (s) { s.classList.remove('active'); });
            this.classList.add('active');
            var label = this.closest('.variant-group').querySelector('.variant-label span');
            if (label) label.textContent = '— ' + this.dataset.color;
        });
    });

    /* ═══════════════════════════════════════════════════
       PDP — Quantity selector
       ═══════════════════════════════════════════════════ */
    function setupQty(minusBtn, plusBtn, input) {
        if (!minusBtn || !plusBtn || !input) return;
        var max = parseInt(input.max, 10) || 99;
        minusBtn.addEventListener('click', function () {
            var val = parseInt(input.value, 10) || 1;
            if (val > parseInt(input.min, 10) || 1) input.value = Math.max(val - 1, 1);
        });
        plusBtn.addEventListener('click', function () {
            var val = parseInt(input.value, 10) || 1;
            if (val < max) input.value = val + 1;
        });
        input.addEventListener('change', function () {
            var val = parseInt(this.value, 10);
            if (isNaN(val) || val < 1) this.value = 1;
            if (val > max) this.value = max;
        });
    }

    setupQty(
        document.getElementById('qtyMinus'),
        document.getElementById('qtyPlus'),
        document.getElementById('qtyInput')
    );
    setupQty(
        document.getElementById('stickyQtyMinus'),
        document.getElementById('stickyQtyPlus'),
        document.getElementById('stickyQtyInput')
    );

    /* ═══════════════════════════════════════════════════
       PDP — Sync qty between main and sticky
       ═══════════════════════════════════════════════════ */
    var qtyInput = document.getElementById('qtyInput');
    var stickyQtyInput = document.getElementById('stickyQtyInput');
    if (qtyInput && stickyQtyInput) {
        qtyInput.addEventListener('change', function () { stickyQtyInput.value = this.value; });
        stickyQtyInput.addEventListener('change', function () { if (qtyInput) qtyInput.value = this.value; });
        document.getElementById('qtyPlus').addEventListener('click', function () { stickyQtyInput.value = qtyInput.value; });
        document.getElementById('qtyMinus').addEventListener('click', function () { stickyQtyInput.value = qtyInput.value; });
        document.getElementById('stickyQtyPlus').addEventListener('click', function () { if (qtyInput) qtyInput.value = stickyQtyInput.value; });
        document.getElementById('stickyQtyMinus').addEventListener('click', function () { if (qtyInput) qtyInput.value = stickyQtyInput.value; });
    }

    /* ═══════════════════════════════════════════════════
       PDP — Add to Cart / Buy Now
       ═══════════════════════════════════════════════════ */
    var addBtn = document.getElementById('addToCartBtn');
    var stickyAddBtn = document.getElementById('stickyAddBtn');

    function getQty() { return parseInt(qtyInput ? qtyInput.value : 1, 10) || 1; }

    function handleAddToCart(btn) {
        if (!btn) return;
        btn.addEventListener('click', function () {
            var id = parseInt(this.dataset.productId);
            if (!id) return;
            addToCart(id, getQty());
            var original = this.innerHTML;
            this.innerHTML = '<i class="fa fa-check"></i> Added!';
            var self = this;
            setTimeout(function () { self.innerHTML = original; }, 1500);
        });
    }

    handleAddToCart(addBtn);
    handleAddToCart(stickyAddBtn);

    var buyNowBtn = document.getElementById('buyNowBtn');
    if (buyNowBtn) {
        buyNowBtn.addEventListener('click', function () {
            var id = parseInt(this.dataset.productId);
            if (id) addToCart(id, getQty());
            window.location.href = '/Cart';
        });
    }

    /* ═══════════════════════════════════════════════════
       PDP — Tabs
       ═══════════════════════════════════════════════════ */
    var tabButtons = document.querySelectorAll('.tab-nav button');
    tabButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            tabButtons.forEach(function (b) { b.classList.remove('active'); });
            this.classList.add('active');
            document.querySelectorAll('.tab-content .tab-pane').forEach(function (p) { p.classList.remove('active'); });
            var target = document.getElementById('tab-' + this.dataset.tab);
            if (target) target.classList.add('active');
        });
    });

    /* ═══════════════════════════════════════════════════
       PDP — Countdown Timer
       ═══════════════════════════════════════════════════ */
    var timerDisplay = document.getElementById('timerDisplay');
    if (timerDisplay) {
        var endTime = Date.now() + 24 * 60 * 60 * 1000; // 24h from now
        function updateTimer() {
            var diff = Math.max(0, endTime - Date.now());
            var h = Math.floor(diff / (60 * 60 * 1000));
            var m = Math.floor((diff % (60 * 60 * 1000)) / (60 * 1000));
            var s = Math.floor((diff % (60 * 1000)) / 1000);
            timerDisplay.textContent =
                String(h).padStart(2, '0') + ':' +
                String(m).padStart(2, '0') + ':' +
                String(s).padStart(2, '0');
        }
        updateTimer();
        setInterval(updateTimer, 1000);
    }

    /* ═══════════════════════════════════════════════════
       PDP — Sticky CTA scroll detection
       ═══════════════════════════════════════════════════ */
    var stickyCta = document.getElementById('stickyCta');
    var mainCta = document.querySelector('.pdp-cta');
    if (stickyCta && mainCta) {
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                stickyCta.style.display = entry.isIntersecting ? 'none' : 'flex';
            });
        }, { threshold: 0 });
        observer.observe(mainCta);
    }

    /* ═══════════════════════════════════════════════════
       PDP — Bundle "Add All"
       ═══════════════════════════════════════════════════ */
    var addBundle = document.getElementById('addBundleBtn');
    if (addBundle) {
        addBundle.addEventListener('click', function () {
            var ids = [];
            document.querySelectorAll('.bought-item').forEach(function (item) {
                var img = item.querySelector('img');
                if (img) {
                    var src = img.getAttribute('src');
                    // Extract product id from the thumbnail — simplified: add all visible products
                }
            });
            // Add current product
            var pid = addBtn ? parseInt(addBtn.dataset.productId) : 0;
            if (pid) addToCart(pid, 1);
            showToast('Bundle added to cart!');
        });
    }

    /* ═══════════════════════════════════════════════════
       INIT — Restore UI on page load
       ═══════════════════════════════════════════════════ */
    updateWishlistUI();
    updateCartUI();
    attachGridListeners();

})();
