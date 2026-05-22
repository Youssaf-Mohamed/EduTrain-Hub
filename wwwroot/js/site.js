document.addEventListener("DOMContentLoaded", () => {
    const loader = document.getElementById("appLoader");

    window.addEventListener("load", () => {
        if (!loader) return;
        window.setTimeout(() => loader.classList.add("is-hidden"), 180);
    });

    document.querySelectorAll("a[href]").forEach((link) => {
        const href = link.getAttribute("href");
        const target = link.getAttribute("target");

        if (!href || href.startsWith("#") || href.startsWith("javascript:") || target === "_blank") {
            return;
        }

        link.addEventListener("click", () => {
            if (loader && link.origin === window.location.origin) {
                loader.classList.remove("is-hidden");
            }
        });
    });

    document.querySelectorAll("form").forEach((form) => {
        form.addEventListener("submit", () => {
            if (loader) {
                loader.classList.remove("is-hidden");
            }
        });
    });

    const revealItems = document.querySelectorAll(".stat-card, .nav-card, .table-container, .ops-kpi, .ops-panel, .coverage-row");
    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("is-visible");
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.12 });

    revealItems.forEach((item, index) => {
        item.style.setProperty("--reveal-delay", `${Math.min(index * 35, 280)}ms`);
        observer.observe(item);
    });
});
