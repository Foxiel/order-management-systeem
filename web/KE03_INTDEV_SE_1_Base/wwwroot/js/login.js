//Gemaakt door Tristan

document.addEventListener("DOMContentLoaded", function () {
    const loginPopup = document.getElementById("loginPopup");
    const openLoginBtn = document.getElementById("openLoginBtn");
    const closeLoginBtn = document.getElementById("closeLoginBtn");
    const loginForm = document.getElementById("loginForm");
    const loginMessage = document.getElementById("loginMessage");

    if (!loginPopup || !openLoginBtn || !closeLoginBtn || !loginForm || !loginMessage) {
        return;
    }

    openLoginBtn.addEventListener("click", function () {
        loginPopup.style.display = "flex";
        loginMessage.textContent = "";
    });

    closeLoginBtn.addEventListener("click", function () {
        loginPopup.style.display = "none";
    });

    loginPopup.addEventListener("click", function (event) {
        if (event.target === loginPopup) {
            loginPopup.style.display = "none";
        }
    });

    loginForm.addEventListener("submit", async function (event) {
        event.preventDefault();

        const username = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value;

        if (username === "" || password === "") {
            loginMessage.textContent = "Vul een gebruikersnaam en wachtwoord in.";
            return;
        }

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

            const result = await response.json();

            if (response.ok && result.success === true) {
                localStorage.setItem("isLoggedIn", "true");
                localStorage.setItem("username", username);

                loginMessage.textContent = "Je bent ingelogd.";
                loginPopup.style.display = "none";
            } else {
                loginMessage.textContent = "Gebruikersnaam of wachtwoord is onjuist.";
            }
        } catch (error) {
            loginMessage.textContent = "Er ging iets mis met inloggen.";
        }
    });
});