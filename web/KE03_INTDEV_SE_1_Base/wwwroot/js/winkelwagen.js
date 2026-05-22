// Shopping Cart Management
const SHIPPING_COST = 4.99;
let cart = [];

// Load cart from localStorage on page load
document.addEventListener('DOMContentLoaded', function() {
    loadCartFromStorage();
    updateCartDisplay();
});

/**
 * Add a product to the cart
 * @param {number} productId - The product ID
 * @param {string} productName - The product name
 * @param {number} productPrice - The product price
 */
function addToCart(productId, productName, productPrice) {
    // Check if product already exists in cart
    const existingItem = cart.find(item => item.id === productId);

    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({
            id: productId,
            name: productName,
            price: parseFloat(productPrice),
            quantity: 1
        });
    }

    saveCartToStorage();
    updateCartDisplay();

    // Close modal
    const modal = bootstrap.Modal.getInstance(document.getElementById('addProductModal'));
    if (modal) {
        modal.hide();
    }

    // Show brief notification
    showNotification(`${productName} added to cart!`);
}

/**
 * Remove a product from the cart
 * @param {number} productId - The product ID to remove
 */
function removeFromCart(productId) {
    cart = cart.filter(item => item.id !== productId);
    saveCartToStorage();
    updateCartDisplay();
}

/**
 * Update the quantity of a product in the cart
 * @param {number} productId - The product ID
 * @param {number} quantity - The new quantity
 */
function updateQuantity(productId, quantity) {
    const item = cart.find(item => item.id === productId);
    if (item) {
        if (quantity <= 0) {
            removeFromCart(productId);
        } else {
            item.quantity = quantity;
            saveCartToStorage();
            updateCartDisplay();
        }
    }
}

/**
 * Update the cart display on the page
 */
function updateCartDisplay() {
    const emptyMessage = document.getElementById('emptyMessage');
    const cartItemsContainer = document.getElementById('cartItemsContainer');
    const itemCount = document.getElementById('itemCount');
    const subtotalPrice = document.getElementById('subtotalPrice');
    const shippingPrice = document.getElementById('shippingPrice');
    const totalPrice = document.getElementById('totalPrice');

    if (cart.length === 0) {
        emptyMessage.style.display = 'flex';
        cartItemsContainer.style.display = 'none';
        itemCount.textContent = '0';
        subtotalPrice.textContent = '€0.00';
        shippingPrice.textContent = '€0.00';
        totalPrice.textContent = '€0.00';
    } else {
        emptyMessage.style.display = 'none';
        cartItemsContainer.style.display = 'flex';

        // Clear previous items
        cartItemsContainer.innerHTML = '';

        // Add items to cart display
        cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            const cartItemHTML = `
                <div class="cart-item">
                    <div class="cart-item-image">
                        <img src="https://aleatori.cat/random" alt="${item.name}">
                    </div>
                    <div class="cart-item-details">
                        <h4>${item.name}</h4>
                        <p>€${item.price.toFixed(2)}</p>
                    </div>
                    <div class="cart-item-quantity">
                        <button class="qty-btn qty-minus" onclick="updateQuantity(${item.id}, ${item.quantity - 1})">−</button>
                        <input type="number" class="qty-input" value="${item.quantity}" onchange="updateQuantity(${item.id}, parseInt(this.value))" min="0">
                        <button class="qty-btn qty-plus" onclick="updateQuantity(${item.id}, ${item.quantity + 1})">+</button>
                    </div>
                    <div class="cart-item-total">
                        €${itemTotal.toFixed(2)}
                    </div>
                    <span class="cart-item-remove" onclick="removeFromCart(${item.id})" title="Remove from cart">×</span>
                </div>
            `;
            cartItemsContainer.innerHTML += cartItemHTML;
        });

        // Calculate totals
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        const subtotal = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        const shipping = cart.length > 0 ? SHIPPING_COST : 0;
        const total = subtotal + shipping;

        itemCount.textContent = totalItems;
        subtotalPrice.textContent = `€${subtotal.toFixed(2)}`;
        shippingPrice.textContent = `€${shipping.toFixed(2)}`;
        totalPrice.textContent = `€${total.toFixed(2)}`;
    }
}

/**
 * Save cart to localStorage
 */
function saveCartToStorage() {
    localStorage.setItem('cart', JSON.stringify(cart));
}

/**
 * Load cart from localStorage
 */
function loadCartFromStorage() {
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
        try {
            cart = JSON.parse(savedCart);
        } catch (e) {
            console.error('Error loading cart from storage:', e);
            cart = [];
        }
    }
}

/**
 * Show a brief notification
 * @param {string} message - The message to display
 */
function showNotification(message) {
    const notification = document.createElement('div');
    notification.style.cssText = `
        position: fixed;
        top: 100px;
        right: 20px;
        background: #00ff66;
        color: black;
        padding: 16px 24px;
        border-radius: 8px;
        font-weight: 700;
        z-index: 9999;
        animation: slideIn 0.3s ease-out;
    `;
    notification.textContent = message;
    document.body.appendChild(notification);

    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease-out';
        setTimeout(() => notification.remove(), 300);
    }, 3000);
}

// Add CSS animations
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(400px);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }

    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(400px);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);
