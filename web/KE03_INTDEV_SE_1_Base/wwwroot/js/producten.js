//Gemaakt door 
//Aangepast door Tristan. aapassing voor local storage met winkelwagen. dit werkt nog niet
document.addEventListener("DOMContentLoaded", function () {
    const buttons = document.querySelectorAll(".add-to-cart-btn");

    buttons.forEach(button => {
        button.addEventListener("click", function () {
            const product = {
                id: this.dataset.productId,
                name: this.dataset.productName,
                price: Number(this.dataset.productPrice.replace(",", ".")),
                image: this.dataset.productImage,
                amount: 1
            };

            let cartItems = JSON.parse(localStorage.getItem(STORAGE_KEY)) || [];

            const existingItem = cartItems.find(item => item.id === product.id);

            if (existingItem) {
                existingItem.amount += 1;
            } else {
                cartItems.push(product);
            }

            localStorage.setItem(STORAGE_KEY, JSON.stringify(cartItems));

            window.dispatchEvent(new Event("cartItemsChanged"));

            this.textContent   = "Toegevoegd";

            setTimeout(() => {
                this.innerHTML = `
                    <svg class="icon-svg-index"
                         xmlns="http://www.w3.org/2000/svg"
                         viewBox="0 0 24 24"
                         fill="none"
                         stroke="currentColor">
                        <path stroke-linecap="round"
                              stroke-linejoin="round"
                              stroke-width="1.8"
                              d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2 6h13" />
                    </svg>
                `;
            }, 1500)
        });
    });
});