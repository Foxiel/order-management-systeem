const FEEDBACK_DURATION = 1500;

document.addEventListener('DOMContentLoaded', function () {
    const qtyInput = document.getElementById('qty');
    const decBtn = document.getElementById('decreaseQty');
    const incBtn = document.getElementById('increaseQty');
    const addBtn = document.querySelector('.add-to-cart-btn');

    // Decrease button
    if (decBtn && qtyInput) {
        decBtn.addEventListener('click', function () {
            const current = parseInt(qtyInput.value) || 1;
            qtyInput.value = Math.max(1, current - 1);
        });
    }

    // Increase button
    if (incBtn && qtyInput) {
        incBtn.addEventListener('click', function () {
            const current = parseInt(qtyInput.value) || 1;
            qtyInput.value = current + 1;
        });
    }

    // Add to cart button
    if (addBtn && qtyInput) {
        addBtn.addEventListener('click', function (ev) {
            ev.preventDefault();
            const ean = this.dataset.productId;
            const amount = Math.max(1, parseInt(qtyInput.value) || 1);

            const product = {
                id: ean,
                name: this.dataset.productName,
                price: Number(this.dataset.productPrice),
                amount: amount
            };

            let cartItems = JSON.parse(localStorage.getItem(STORAGE_KEY)) || [];
            const existingItem = cartItems.find(item => item.id === product.id);

            if (existingItem) {
                existingItem.amount += amount;
            } else {
                cartItems.push(product);
            }

            localStorage.setItem(STORAGE_KEY, JSON.stringify(cartItems));
            window.dispatchEvent(new Event("cartItemsChanged"));

            const originalHTML = addBtn.innerHTML;
            addBtn.disabled = true;
            addBtn.innerHTML = '✓ Toegevoegd';

            setTimeout(() => {
                addBtn.disabled = false;
                addBtn.innerHTML = originalHTML;
                qtyInput.value = 1;
            }, FEEDBACK_DURATION);
        });
    }
});
