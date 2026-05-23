document.addEventListener('DOMContentLoaded', function () {
    const buttons = document.querySelectorAll('.add-to-cart-btn');

    buttons.forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();

            const productId = parseInt(this.dataset.productId, 10);
            const storageKey = 'cartItems';

            let cartItems = JSON.parse(localStorage.getItem(storageKey)) || [];

            const existingItem = cartItems.find(item => item.id === productId);

            if (existingItem) {
                existingItem.amount += 1;
            } else {
                cartItems.push({
                    id: productId,
                    amount: 1
                });
            }

            localStorage.setItem(storageKey, JSON.stringify(cartItems));

            alert('Product toegevoegd aan winkelmandje');
        });
    });
});