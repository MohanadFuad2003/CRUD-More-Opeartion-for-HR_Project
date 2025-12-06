
(function() {
    function initParallax() {
        const parallaxElements = document.querySelectorAll("[data-parallax]");
        if (!parallaxElements.length) return;

        window.addEventListener("scroll", () => {
            parallaxElements.forEach(element => {
                const speed = parseFloat(element.getAttribute("data-parallax")) || 0.5;
                const offset = window.scrollY * speed;
                element.style.transform = `translateY(${offset}px)`;
            });
        }, { passive: true });
    }

    function initStaggeredAnimations() {
        const staggerContainers = document.querySelectorAll("[data-stagger]");
        staggerContainers.forEach(container => {
            const children = container.children;
            Array.from(children).forEach((child, index) => {
                child.style.animation = `slideUp 0.6s ease ${index * 0.1}s forwards`;
                child.style.opacity = "0";
            });
        });
    }

    function initCounters() {
        const counters = document.querySelectorAll("[data-counter]");
        const options = {
            threshold: 0.5
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const counter = entry.target;
                    const target = parseInt(counter.getAttribute("data-counter"));
                    const duration = 1500;
                    const increment = target / (duration / 16);
                    let current = 0;

                    const timer = setInterval(() => {
                        current += increment;
                        if (current >= target) {
                            current = target;
                            clearInterval(timer);
                        }
                        counter.textContent = Math.floor(current).toLocaleString();
                    }, 16);

                    observer.unobserve(counter);
                }
            });
        }, options);

        counters.forEach(counter => observer.observe(counter));
    }

    function initMouseFollow() {
        const followElements = document.querySelectorAll("[data-mouse-follow]");
        if (!followElements.length) return;

        document.addEventListener("mousemove", (e) => {
            followElements.forEach(element => {
                const rect = element.getBoundingClientRect();
                const x = e.clientX - rect.left - rect.width / 2;
                const y = e.clientY - rect.top - rect.height / 2;
                const distance = Math.sqrt(x * x + y * y);
                
                if (distance < 100) {
                    element.style.transform = `translate(${x * 0.1}px, ${y * 0.1}px)`;
                }
            });
        }, { passive: true });
    }

    function initSmoothScroll() {
        if (!CSS.supports("scroll-behavior", "smooth")) {
            document.querySelectorAll('a[href^="#"]').forEach(anchor => {
                anchor.addEventListener("click", function(e) {
                    e.preventDefault();
                    const target = document.querySelector(this.getAttribute("href"));
                    if (target) {
                        target.scrollIntoView({ behavior: "smooth", block: "start" });
                    }
                });
            });
        }
    }

    function initTooltips() {
        const tooltips = document.querySelectorAll("[data-tooltip]");
        tooltips.forEach(element => {
            element.addEventListener("mouseenter", function() {
                const tooltip = document.createElement("div");
                tooltip.className = "tooltip";
                tooltip.textContent = this.getAttribute("data-tooltip");
                document.body.appendChild(tooltip);

                const rect = this.getBoundingClientRect();
                tooltip.style.top = (rect.top - 10 - tooltip.clientHeight) + "px";
                tooltip.style.left = (rect.left + this.clientWidth / 2 - tooltip.clientWidth / 2) + "px";

                setTimeout(() => tooltip.classList.add("show"), 10);
            });

            element.addEventListener("mouseleave", function() {
                const tooltip = document.querySelector(".tooltip");
                if (tooltip) {
                    tooltip.classList.remove("show");
                    setTimeout(() => tooltip.remove(), 300);
                }
            });
        });
    }

    function initRippleEffect() {
        const buttons = document.querySelectorAll(".btn, .btn-primary, .btn-cta");
        buttons.forEach(button => {
            button.addEventListener("click", function(e) {
                const ripple = document.createElement("span");
                ripple.className = "ripple";
                const rect = this.getBoundingClientRect();
                const x = e.clientX - rect.left;
                const y = e.clientY - rect.top;
                ripple.style.left = x + "px";
                ripple.style.top = y + "px";
                this.appendChild(ripple);

                setTimeout(() => ripple.remove(), 600);
            });
        });
    }

    function initialize() {
        if (document.readyState === "loading") {
            document.addEventListener("DOMContentLoaded", function() {
                initParallax();
                initStaggeredAnimations();
                initCounters();
                initMouseFollow();
                initSmoothScroll();
                initTooltips();
                initRippleEffect();
            });
        } else {
            initParallax();
            initStaggeredAnimations();
            initCounters();
            initMouseFollow();
            initSmoothScroll();
            initTooltips();
            initRippleEffect();
        }
    }

    initialize();
})();
