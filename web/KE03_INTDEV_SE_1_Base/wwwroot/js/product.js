(function () {
    const storageKey = 'cartItems';
    const FEEDBACK_DURATION = 1400; // ms

    function getCart() {
        try {
            return JSON.parse(localStorage.getItem(storageKey) || '[]');
        } catch (e) {
            console.error('Failed to parse cartItems from localStorage', e);
            return [];
        }
    }

    function saveCart(cart) {
        try {
            localStorage.setItem(storageKey, JSON.stringify(cart));
        } catch (e) {
            console.error('Failed to save cartItems to localStorage', e);
        }
    }

    function addToCart(ean, amount) {
        if (!ean) return;
        let cart = getCart();
        const existing = cart.find(i => i.id === ean);
        if (existing) {
            existing.amount = (existing.amount || 0) + amount;
        } else {
            cart.push({ id: ean, amount: amount });
        }
        saveCart(cart);
    }

    function createFeedbackElement(message) {
        const el = document.createElement('span');
        el.className = 'product-feedback-badge';
        el.textContent = message;
        el.style.position = 'absolute';
        el.style.top = '8px';
        el.style.right = '8px';
        el.style.background = 'rgba(0,0,0,0.8)';
        el.style.color = '#fff';
        el.style.padding = '6px 10px';
        el.style.borderRadius = '14px';
        el.style.fontSize = '0.875rem';
        el.style.zIndex = 30;
        el.style.opacity = '0';
        el.style.transition = 'opacity 160ms ease, transform 200ms ease';
        el.style.transform = 'translateY(-6px)';
        return el;
    }

    function showFeedbackNear(button, message) {
        if (!button) return;

        const container = button.closest('.mb-4') || button.parentElement || document.body;
        if (!container) return;

        const prevPosition = container.style.position;
        if (!prevPosition || prevPosition === '') {
            container.style.position = 'relative';
        }

        const badge = createFeedbackElement(message);
        container.appendChild(badge);

        requestAnimationFrame(() => {
            badge.style.opacity = '1';
            badge.style.transform = 'translateY(0)';
        });

        setTimeout(() => {
            badge.style.opacity = '0';
            badge.style.transform = 'translateY(-6px)';
            setTimeout(() => {
                if (badge.parentElement) badge.parentElement.removeChild(badge);
                if (!prevPosition) container.style.position = '';
            }, 220);
        }, FEEDBACK_DURATION);
    }

    function safeParseInt(value, fallback = 1) {
        const n = parseInt(String(value).replace(/[^\d-]/g, ''), 10);
        return Number.isFinite(n) ? n : fallback;
    }

    function attachHandlers() {
        const qtyInput = document.getElementById('qty');
        const decBtn = document.getElementById('decreaseQty');
        const incBtn = document.getElementById('increaseQty');
        const addBtn = document.getElementById('addToCartBtn');

        // Ensure input starts with a valid number
        if (qtyInput && !qtyInput.value) qtyInput.value = '1';

        // Block direct editing: prevent wheel and key input that could change value
        if (qtyInput) {
            // Prevent mouse wheel changing focus/input value
            qtyInput.addEventListener('wheel', function (e) {
                e.preventDefault();
            }, { passive: false });

            // Prevent keyboard input that could change value
            qtyInput.addEventListener('keydown', function (e) {
                // Allow tab/navigation keys but prevent numeric input and arrows
                const blockedKeys = ['ArrowUp', 'ArrowDown', 'Home', 'End', 'PageUp', 'PageDown'];
                if (blockedKeys.includes(e.key)) {
                    e.preventDefault();
                } else {
                    // block typing (letters/numbers/backspace/etc)
                    e.preventDefault();
                }
            });

            // Also prevent paste/drag/drop into the field
            qtyInput.addEventListener('paste', function (e) { e.preventDefault(); });
            qtyInput.addEventListener('drop', function (e) { e.preventDefault(); });
            qtyInput.addEventListener('input', function () {
                // In case some browser allows input, sanitize to at least 1
                const val = safeParseInt(qtyInput.value, 1);
                qtyInput.value = String(Math.max(1, val));
            });
        }

        if (decBtn && qtyInput) {
            decBtn.addEventListener('click', function () {
                const current = safeParseInt(qtyInput.value, 1);
                const v = Math.max(1, current - 1);
                qtyInput.value = String(v);
            });
        }

        if (incBtn && qtyInput) {
            incBtn.addEventListener('click', function () {
                const current = safeParseInt(qtyInput.value, 1);
                const v = Math.max(1, current + 1);
                qtyInput.value = String(v);
            });
        }

        if (addBtn && qtyInput) {
            addBtn.addEventListener('click', function (ev) {
                ev.preventDefault();
                const ean = this.dataset.productEan;
                const amount = Math.max(1, safeParseInt(qtyInput.value, 1));

                addToCart(ean, amount);

                // temporary button state change
                const originalText = addBtn.innerHTML;
                addBtn.disabled = true;
                addBtn.innerHTML = '✓ Toegevoegd';

                showFeedbackNear(addBtn, 'Toegevoegd');

                setTimeout(() => {
                    addBtn.disabled = false;
                    addBtn.innerHTML = originalText;
                }, FEEDBACK_DURATION);
            });
        }

        // Expose helper if other scripts need to add programmatically
        window.productCart = {
            getCart,
            saveCart,
            addToCart
        };
    }

    document.addEventListener('DOMContentLoaded', attachHandlers);
})();