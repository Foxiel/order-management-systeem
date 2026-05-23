
// PROMOS
document.addEventListener('DOMContentLoaded', function () {

        const promos = [
    {
        title: "<h3>AA batterijen  1+1 GRATIS</h3>",
    description: "Alle AA baterijen is de 2e gratis."
            },
    {
        title: "<h3>AUTO ONDERDELEN</h3>",
    description: "Tot 30% korting op geselecteerde auto onderdelen."
            },
    {
        title: "<h3>EMP ACTIE</h3>",
    description: "2 halen = 1 gratis op alle EMP's."
            }
    ];

    let index = 0;

    const title = document.getElementById("heroTitle");
    const desc = document.getElementById("heroDesc");

    function renderPromo() {
        title.innerHTML = promos[index].title;
        desc.innerHTML = promos[index].description;
        }

    document.getElementById("prevBtn")
    .addEventListener("click", function () {

        index--;

    if (index < 0) {
        index = promos.length - 1;
                }

    renderPromo();
            });

    document.getElementById("nextBtn")
    .addEventListener("click", function () {

        index++;

                if (index >= promos.length) {
        index = 0;
                }

    renderPromo();
            });

    renderPromo();
});

// categorieen

const grid = document.getElementById("categoryGrid");

const nextBtn = document.querySelector(".right-arrow");
const prevBtn = document.querySelector(".left-arrow");

let currentIndex = 0;

const visibleCards = 3;
const totalCards = grid.children.length;

function updateSlider() {

    const cardWidth =
        grid.children[0].offsetWidth + 20;

    grid.style.transform =
        `translateX(-${currentIndex * cardWidth}px)`;
}

nextBtn.addEventListener("click", () => {

    currentIndex++;

    if (currentIndex > totalCards - visibleCards) {
        currentIndex = 0;
    }

    updateSlider();

});

prevBtn.addEventListener("click", () => {

    currentIndex--;

    if (currentIndex < 0) {
        currentIndex = totalCards - visibleCards;
    }

    updateSlider();

});