(() => {
    const body = document.body;
    const themeToggle = document.querySelector("[data-theme-toggle]");
    const navToggle = document.querySelector("[data-nav-toggle]");
    const navMenu = document.querySelector("[data-nav-menu]");
    const navbar = document.querySelector("[data-navbar]");

    const THEME_KEY = "hr-theme";

    const applyTheme = (theme) => {
        body.setAttribute("data-theme", theme);
        localStorage.setItem(THEME_KEY, theme);

        const label = document.querySelector(".theme-label");
        if (label) {
            label.textContent = theme === "light" ? "Light" : "Dark";
        }
    };

    const storedTheme = localStorage.getItem(THEME_KEY);
    if (storedTheme) {
        applyTheme(storedTheme);
    } else {
        applyTheme("light");
    }

    themeToggle?.addEventListener("click", () => {
        const nextTheme = body.getAttribute("data-theme") === "light" ? "dark" : "light";
        applyTheme(nextTheme);
    });

    navToggle?.addEventListener("click", () => {
        navMenu?.classList.toggle("open");
        navToggle.classList.toggle("active");
    });

    navMenu?.querySelectorAll("a").forEach(link => {
        link.addEventListener("click", () => {
            navMenu.classList.remove("open");
            navToggle?.classList.remove("active");
        });
    });

    const handleScroll = () => {
        if (!navbar) return;
        const isScrolled = window.scrollY > 10;
        navbar.classList.toggle("is-scrolled", isScrolled);
        navbar.style.background = isScrolled 
            ? "rgba(17, 24, 40, 0.85)" 
            : "transparent";
    };

    document.addEventListener("scroll", handleScroll, { passive: true });
    handleScroll();

    const revealTargets = document.querySelectorAll("[data-reveal]");
    if (revealTargets.length) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("revealed");
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        revealTargets.forEach(el => observer.observe(el));
    }

    const forms = document.querySelectorAll("form");
    forms.forEach(form => {
        form.addEventListener("invalid", (e) => {
            e.preventDefault();
            const input = e.target;
            input.classList.add("is-invalid");
            input.style.borderColor = "#ff6b6b";
        }, true);

        const inputs = form.querySelectorAll("input, select, textarea");
        inputs.forEach(input => {
            input.addEventListener("input", () => {
                if (input.checkValidity()) {
                    input.classList.remove("is-invalid");
                    input.style.borderColor = "";
                }
            });

            input.addEventListener("blur", () => {
                if (!input.checkValidity() && input.value) {
                    input.classList.add("is-invalid");
                    input.style.borderColor = "#ff6b6b";
                }
            });
        });
    });

    const buttons = document.querySelectorAll(".btn, .btn-primary, .btn-cta, .btn-danger, .btn-success");
    buttons.forEach(button => {
        button.addEventListener("mouseenter", function() {
            this.style.animation = "none";
            setTimeout(() => {
                this.style.animation = "";
            }, 10);
        });
    });

    window.addEventListener("load", () => {
        document.body.style.opacity = "1";
    });

    const formInputs = document.querySelectorAll(".form-field input, .form-field select, .form-field textarea");
    formInputs.forEach(input => {
        input.addEventListener("focus", function() {
            this.parentElement.style.transform = "scale(1.02)";
        });

        input.addEventListener("blur", function() {
            this.parentElement.style.transform = "scale(1)";
        });
    });

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", () => {
            body.style.opacity = "1";
        });
    } else {
        body.style.opacity = "1";
    }
})();
