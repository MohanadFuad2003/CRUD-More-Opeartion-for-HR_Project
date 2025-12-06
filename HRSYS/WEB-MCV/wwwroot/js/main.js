document.addEventListener("DOMContentLoaded", () => {
    initializeSmoothTransitions();
    
    addElementAnimations();
});


function initializeSmoothTransitions() {
    document.body.style.transition = "opacity 0.3s ease";
    
    document.querySelectorAll("a:not([target])").forEach(link => {
        link.addEventListener("click", function(e) {
            const href = this.getAttribute("href");
            if (href && !href.startsWith("#")) {
                document.body.style.opacity = "0.7";
            }
        });
    });
    
    window.addEventListener("pageshow", () => {
        document.body.style.opacity = "1";
    });
}

function addElementAnimations() {
    const pageTitle = document.querySelector(".page-title");
    if (pageTitle) {
        pageTitle.style.animation = "slideInLeft 0.6s ease forwards";
    }

    const pageSubtitle = document.querySelector(".page-subtitle");
    if (pageSubtitle) {
        pageSubtitle.style.animation = "slideInRight 0.6s ease 0.1s forwards";
        pageSubtitle.style.opacity = "0";
    }

    document.querySelectorAll(".card").forEach((card, index) => {
        card.style.animation = `slideUp 0.6s ease ${index * 0.1}s forwards`;
        card.style.opacity = "0";
    });

    document.querySelectorAll(".form-field").forEach((field, index) => {
        field.style.animation = `slideUp 0.6s ease ${index * 0.05}s forwards`;
        field.style.opacity = "0";
    });
}

