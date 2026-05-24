//Gemaakt door 
//Aangepast door Tristan. aapassing voor local storage met winkelwagen. dit werkt nog niet
document.addEventListener("DOMContentLoaded", function () {
    const buttons = document.querySelectorAll(".add-to-cart-btn");
    const storageKey = "cartItems";

    buttons.forEach(button => {
        button.addEventListener("click", function () {
            const product = {
                id: this.dataset.productId,
                name: this.dataset.productName,
                price: Number(this.dataset.productPrice.replace(",", ".")),
                image: this.dataset.productImage,
                amount: 1
            };

            let cartItems = JSON.parse(localStorage.getItem(storageKey)) || [];

            const existingItem = cartItems.find(item => item.id === product.id);

            if (existingItem) {
                existingItem.amount += 1;
            } else {
                cartItems.push(product);
            }

            localStorage.setItem(storageKey, JSON.stringify(cartItems));

            this.textContent = "Toegevoegd ✓";

            setTimeout(() => {
                this.textContent = "Aan wagen toevoegen";
            }, 1500);
        });
    });
});