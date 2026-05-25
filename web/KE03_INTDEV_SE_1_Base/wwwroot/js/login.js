document.addEventListener("DOMContentLoaded", function () {

    const loginPopup = document.getElementById("loginPopup");
    const openLoginBtn = document.getElementById("openLoginBtn");
    const closeLoginBtn = document.getElementById("closeLoginBtn");
    const loginForm = document.getElementById("loginForm");
    const loginMessage = document.getElementById("loginMessage");

    const loggedInUser = document.getElementById("loggedInUser");
    const logoutBtn = document.getElementById("logoutBtn");
    const ordersBtnContainer = document.getElementById("ordersBtnContainer");

    // Controle of alles bestaat
    if (
        !loginPopup ||
        !openLoginBtn ||
        !closeLoginBtn ||
        !loginForm ||
        !loginMessage ||
        !loggedInUser ||
        !logoutBtn ||
        !ordersBtnContainer
    ) {
        console.error("Login popup elementen niet gevonden.");
        return;
    }

    // Login status updaten
    function updateLoginDisplay() {

        const isLoggedIn = localStorage.getItem("isLoggedIn");
        const usernameStored = localStorage.getItem("username");

        if (isLoggedIn === "true" && usernameStored) {

            // Login knop verbergen
            openLoginBtn.classList.add("d-none");

            // Ingelogde onderdelen tonen
            loggedInUser.classList.remove("d-none");
            logoutBtn.classList.remove("d-none");
            ordersBtnContainer.classList.remove("d-none");

            loggedInUser.textContent =
                `Ingelogd als: ${usernameStored}`;

        }
        else {

            // Login knop tonen
            openLoginBtn.classList.remove("d-none");

            // Ingelogde onderdelen verbergen
            loggedInUser.classList.add("d-none");
            logoutBtn.classList.add("d-none");
            ordersBtnContainer.classList.add("d-none");

            loggedInUser.textContent = "";

        }
    }

    // Direct uitvoeren bij laden pagina
    updateLoginDisplay();

    // Popup openen
    openLoginBtn.addEventListener("click", function () {

        loginPopup.style.display = "flex";
        loginMessage.textContent = "";

    });

    // Popup sluiten
    closeLoginBtn.addEventListener("click", function () {

        loginPopup.style.display = "none";

    });

    // Popup sluiten buiten popup
    loginPopup.addEventListener("click", function (event) {

        if (event.target === loginPopup) {
            loginPopup.style.display = "none";
        }

    });

    // Uitloggen
    logoutBtn.addEventListener("click", function () {

        localStorage.removeItem("isLoggedIn");
        localStorage.removeItem("username");

        updateLoginDisplay();

    });

    // Login submit
    loginForm.addEventListener("submit", async function (event) {

        event.preventDefault();

        const username =
            document.getElementById("username").value.trim();

        const password =
            document.getElementById("password").value;

        loginMessage.textContent =
            "Login wordt gecontroleerd...";

        try {

            const response = await fetch("/api/account/login", {

                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify({
                    username: username,
                    password: password
                })

            });

            // Server fout
            if (!response.ok) {

                loginMessage.textContent =
                    "Server fout tijdens het inloggen.";

                return;
            }

            const result = await response.json();

            // Login succesvol
            if (result.success === true) {

                localStorage.setItem("isLoggedIn", "true");
                localStorage.setItem("username", username);

                updateLoginDisplay();

                loginMessage.textContent =
                    "Succesvol ingelogd";

                setTimeout(() => {

                    loginPopup.style.display = "none";

                }, 1000);

            }
            else {

                loginMessage.textContent =
                    "Gebruikersnaam of wachtwoord is onjuist";

            }

        }
        catch (error) {

            console.error("Login error:", error);

            loginMessage.textContent =
                "Er ging iets mis tijdens het inloggen.";

        }

    });

});