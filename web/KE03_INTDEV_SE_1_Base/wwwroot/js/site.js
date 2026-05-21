// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

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