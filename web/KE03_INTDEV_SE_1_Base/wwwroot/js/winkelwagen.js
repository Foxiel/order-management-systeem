// gemaakt door Jesse
const SHIPPING_COST = 4.99;

let cartItems = [];

document.addEventListener("DOMContentLoaded", function () {
    loadCartFromStorage();
    updateCartDisplay();
});

function loadCartFromStorage() {
    const savedCart = localStorage.getItem(STORAGE_KEY);

    if (savedCart) {
        try {
            cartItems = JSON.parse(savedCart);
        } catch {
            cartItems = [];
        }
    }
}

function saveCartToStorage() {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(cartItems));
    window.dispatchEvent(new Event("cartItemsChanged"));
}

function removeFromCart(productId) {
    cartItems = cartItems.filter(item => item.id !== productId);
    saveCartToStorage();
    updateCartDisplay();
}

function updateAmount(productId, amount) {
    const item = cartItems.find(item => item.id === productId);

    if (!item) {
        return;
    }

    if (amount <= 0) {
        removeFromCart(productId);
        return;
    }

    item.amount = amount;
    saveCartToStorage();
    updateCartDisplay();
}

function updateCartDisplay() {
    const emptyMessage = document.getElementById("emptyMessage");
    const cartItemsContainer = document.getElementById("cartItemsContainer");
    const itemCount = document.getElementById("itemCount");
    const subtotalPrice = document.getElementById("subtotalPrice");
    const shippingPrice = document.getElementById("shippingPrice");
    const totalPrice = document.getElementById("totalPrice");

    if (!emptyMessage || !cartItemsContainer) {
        return;
    }

    if (cartItems.length === 0) {
        emptyMessage.style.display = "flex";
        cartItemsContainer.style.display = "none";
        cartItemsContainer.innerHTML = "";

        itemCount.textContent = "0";
        subtotalPrice.textContent = "€0.00";
        shippingPrice.textContent = "€0.00";
        totalPrice.textContent = "€0.00";
        return;
    }

    emptyMessage.style.display = "none";
    cartItemsContainer.style.display = "flex";
    cartItemsContainer.innerHTML = "";

    cartItems.forEach(item => {
        const itemTotal = item.price * item.amount;

        const imageUrl = item.image && item.image.trim() !== ""
            ? item.image
            : `https://cataas.com/cat?width=400&height=300&random=${item.id}`;

        cartItemsContainer.innerHTML += `
            <div class="cart-item">
                <div class="cart-item-image">
                    <img src="${imageUrl}" alt="${item.name}">
                </div>

                <div class="cart-item-details">
                    <h4>${item.name}</h4>
                    <p>€${item.price?.toFixed(2)}</p>
                </div>

                <div class="cart-item-quantity">
                    <button class="qty-btn" onclick="updateAmount('${item.id}', ${item.amount - 1})">−</button>

                    <input type="number"
                           class="qty-input"
                           value="${item.amount}"
                           min="1"
                           onchange="updateAmount('${item.id}', parseInt(this.value))">

                    <button class="qty-btn" onclick="updateAmount('${item.id}', ${item.amount + 1})">+</button>
                </div>

                <div class="cart-item-total">
                    €${itemTotal.toFixed(2)}
                </div>

                <span class="cart-item-remove"
                      onclick="removeFromCart('${item.id}')"
                      title="Verwijderen">
                    ×
                </span>
            </div>
        `;
    });

    const totalItems = cartItems.reduce((sum, item) => sum + item.amount, 0);
    const subtotal = cartItems.reduce((sum, item) => sum + item.price * item.amount, 0);
    const shipping = cartItems.length > 0 ? SHIPPING_COST : 0;
    const total = subtotal + shipping;

    itemCount.textContent = totalItems;
    subtotalPrice.textContent = `€${subtotal.toFixed(2)}`;
    shippingPrice.textContent = `€${shipping.toFixed(2)}`;
    totalPrice.textContent = `€${total.toFixed(2)}`;
}