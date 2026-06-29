//aangepast door Jesse
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

const STORAGE_KEY = "cartItems";

// Tab Visibility Detection - Changes logo color when tab is hidden
document.addEventListener('visibilitychange', function() {
    if (document.hidden) {
        // Tab is hidden - add class to change logo to magenta
        document.body.classList.add('tab-hidden');
        document.title = '⚫ Matrix Inc - Afwezig';  
    } else {
        // Tab is visible - remove class to change logo back to green
        document.body.classList.remove('tab-hidden');
        document.title = '✅ Matrix Inc - Actief';
    }
});

// Set initial state
if (document.hidden) {
    document.body.classList.add('tab-hidden');
    document.title = '⚫ Matrix Inc - Afwezig';
} else {
    document.body.classList.remove('tab-hidden');
    document.title = '✅ Matrix Inc - Actief';
}

function getCartItemCount() {
    const savedCart = localStorage.getItem(STORAGE_KEY);

    if (!savedCart) {
        return 0;
    }

    try {
        const cartItems = JSON.parse(savedCart);

        if (!Array.isArray(cartItems)) {
            return 0;
        }

        return cartItems.reduce((sum, item) => sum + (Number(item.amount) || 0), 0);
    } catch {
        return 0;
    }
}

function updateCartBadge() {
    const cartBadge = document.getElementById("cartBadge");

    if (!cartBadge) {
        return;
    }

    const count = getCartItemCount();
    cartBadge.textContent = count;
    cartBadge.hidden = count === 0;
}

//navbar
// CATEGORY DROPDOWN

const dropdownBtn =
    document.getElementById("categoryDropdownBtn");

const dropdown =
    document.getElementById("categoryDropdown");

dropdownBtn.addEventListener("click", () => {

    dropdown.classList.toggle("active");

});

// sluiten als je ergens anders klikt

document.addEventListener("click", (e) => {

    if (!e.target.closest(".nav-dropdown")) {
        dropdown.classList.remove("active");
    }

});

document.addEventListener("DOMContentLoaded", updateCartBadge);

window.addEventListener("storage", (e) => {
    if (e.key === STORAGE_KEY) {
        updateCartBadge();
    }
});

window.addEventListener("cartItemsChanged", updateCartBadge);
