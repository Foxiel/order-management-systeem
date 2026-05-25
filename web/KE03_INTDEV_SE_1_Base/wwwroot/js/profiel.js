document.addEventListener("DOMContentLoaded", async function () {
    const isLoggedIn = localStorage.getItem("isLoggedIn");
    const username = localStorage.getItem("username");

    const notLoggedIn = document.getElementById("notLoggedIn");
    const profileContainer = document.getElementById("profileContainer");
    const editProfileBtn = document.getElementById("editProfileBtn");
    const saveProfileBtn = document.getElementById("saveProfileBtn");
    const profileMessage = document.getElementById("profileMessage");

    const fields = [
        "customerName",
        "customerEmail",
        "customerPhone",
        "street",
        "houseNumber",
        "postalCode",
        "city",
        "country",
        "addressType"
    ];

    if (isLoggedIn !== "true" || !username) {
        return;
    }

    await loadProfile();

    editProfileBtn.addEventListener("click", function () {
        setFieldsDisabled(false);

        editProfileBtn.style.display = "none";
        saveProfileBtn.style.display = "inline-block";

        profileMessage.textContent = "Je kunt je gegevens nu aanpassen.";
    });

    saveProfileBtn.addEventListener("click", async function () {
        await saveProfile();
    });

    async function loadProfile() {
        try {
            const response = await fetch(`/api/account/profile/${username}`);

            if (!response.ok) {
                profileMessage.textContent = "Profielgegevens konden niet worden opgehaald.";
                return;
            }

            const profile = await response.json();

            notLoggedIn.style.display = "none";
            profileContainer.style.display = "block";

            document.getElementById("customerId").value = profile.customerId ?? "";
            document.getElementById("addressId").value = profile.addressId ?? "";

            document.getElementById("customerName").value = profile.customerName ?? "";
            document.getElementById("customerEmail").value = profile.customerEmail ?? "";
            document.getElementById("customerPhone").value = profile.customerPhone ?? "";

            document.getElementById("street").value = profile.street ?? "";
            document.getElementById("houseNumber").value = profile.houseNumber ?? "";
            document.getElementById("postalCode").value = profile.postalCode ?? "";
            document.getElementById("city").value = profile.city ?? "";
            document.getElementById("country").value = profile.country ?? "";
            document.getElementById("addressType").value = profile.addressType ?? "";
        }
        catch (error) {
            console.error(error);
            profileMessage.textContent = "Er ging iets mis bij het laden van je profiel.";
        }
    }

    async function saveProfile() {
        const profile = {
            customerId: parseInt(document.getElementById("customerId").value),
            customerName: document.getElementById("customerName").value,
            customerEmail: document.getElementById("customerEmail").value,
            customerPhone: document.getElementById("customerPhone").value,

            addressId: parseInt(document.getElementById("addressId").value),
            street: document.getElementById("street").value,
            houseNumber: document.getElementById("houseNumber").value,
            postalCode: document.getElementById("postalCode").value,
            city: document.getElementById("city").value,
            country: document.getElementById("country").value,
            addressType: document.getElementById("addressType").value
        };

        try {
            const response = await fetch("/api/account/profile", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(profile)
            });

            if (!response.ok) {
                profileMessage.textContent = "Opslaan is mislukt.";
                return;
            }

            const result = await response.json();

            if (result.success === true) {
                profileMessage.textContent = "Profielgegevens zijn opgeslagen.";

                setFieldsDisabled(true);

                editProfileBtn.style.display = "inline-block";
                saveProfileBtn.style.display = "none";
            }
            else {
                profileMessage.textContent = "Er zijn geen gegevens aangepast.";
            }
        }
        catch (error) {
            console.error(error);
            profileMessage.textContent = "Er ging iets mis tijdens het opslaan.";
        }
    }

    function setFieldsDisabled(disabled) {
        fields.forEach(id => {
            document.getElementById(id).disabled = disabled;
        });
    }
});