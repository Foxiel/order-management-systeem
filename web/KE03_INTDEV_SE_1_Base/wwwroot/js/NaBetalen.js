document.addEventListener("DOMContentLoaded", function () {

    const container = document.getElementById("ordered-products");
    const cartItems = JSON.parse(localStorage.getItem("cartItems")) || [];

    if (cartItems.length === 0) {
        container.innerHTML = "<p>Geen producten gevonden.</p>";
        return;
    }

    cartItems.forEach(item => {

        const product = document.createElement("div");
        product.classList.add("ordered-product-card");

        product.innerHTML = `
                    <div class="ordered-product-image">
                        <img src="${item.image}" alt="${item.name}">
                    </div>

                    <div class="ordered-product-name">
                        ${item.name}
                    </div>
                `;

        container.appendChild(product);
    });

    localStorage.removeItem("cartItems");
});