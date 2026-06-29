// gemaakt door Jesse

const PAYMENT_SHIPPING_COST = 4.99;

document.addEventListener("DOMContentLoaded", function () {
    const savedCart = localStorage.getItem(PAYMENT_STORAGE_KEY);

    let cartItems = [];

    if (savedCart) {
        try {
            cartItems = JSON.parse(savedCart);
        } catch {
            cartItems = [];
        }
    }

    const paymentProducts = document.getElementById("paymentProducts");
    const paymentItemCount = document.getElementById("paymentItemCount");
    const paymentSubtotal = document.getElementById("paymentSubtotal");
    const paymentShipping = document.getElementById("paymentShipping");
    const paymentTotal = document.getElementById("paymentTotal");

    if (!paymentProducts) {
        return;
    }

    if (cartItems.length === 0) {
        paymentProducts.innerHTML = "<p>Je winkelwagen is leeg.</p>";
        paymentItemCount.textContent = "0";
        paymentSubtotal.textContent = "€0.00";
        paymentShipping.textContent = "€0.00";
        paymentTotal.textContent = "€0.00";
        return;
    }

    let subtotal = 0;
    let totalItems = 0;

    paymentProducts.innerHTML = "";

    cartItems.forEach(function (item) {
        const price = Number(item.price);
        const amount = Number(item.amount);
        const itemTotal = price * amount;

        subtotal += itemTotal;
        totalItems += amount;

        paymentProducts.innerHTML += `
            <div class="payment-product-line">
                <div>
                    <strong>${item.name}</strong>
                    <p>${amount} x €${price.toFixed(2)}</p>
                </div>

                <span>€${itemTotal.toFixed(2)}</span>
            </div>
        `;
    });

    const shipping = PAYMENT_SHIPPING_COST;
    const total = subtotal + shipping;

    paymentItemCount.textContent = totalItems;
    paymentSubtotal.textContent = `€${subtotal.toFixed(2)}`;
    paymentShipping.textContent = `€${shipping.toFixed(2)}`;
    paymentTotal.textContent = `€${total.toFixed(2)}`;

    const paymentForm = document.getElementById("paymentForm");
    const cartJsonInput = document.getElementById("cartJsonInput");

    if (paymentForm && cartJsonInput) {
        paymentForm.addEventListener("submit", function () {

            const cart = localStorage.getItem("cartItems");

            if (cart) {
                cartJsonInput.value = cart;
            }
            else {
                cartJsonInput.value = "[]";
            }

        });
    }
});