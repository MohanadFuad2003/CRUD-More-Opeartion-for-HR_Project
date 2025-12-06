function showToast(type, message) {
    const toast = document.getElementById("toast");
    if (!toast) return;

    const icons = {
        success: '<i class="fa-solid fa-circle-check"></i>',
        danger: '<i class="fa-solid fa-circle-xmark"></i>',
        warning: '<i class="fa-solid fa-triangle-exclamation"></i>',
        info: '<i class="fa-solid fa-circle-info"></i>'
    };

    toast.className = "toast-shell";
    toast.classList.add(type);
    toast.innerHTML = `
        <span class="toast-icon">${icons[type] ?? icons.info}</span>
        <span class="toast-body">${message}</span>
    `;

    requestAnimationFrame(() => {
        toast.classList.add("show");
    });

    setTimeout(() => {
        toast.classList.remove("show");
    }, 3600);
}

document.addEventListener("DOMContentLoaded", () => {
    const toast = document.getElementById("toast");
    if (toast) {
        toast.addEventListener("click", () => {
            toast.classList.remove("show");
        });
    }
});

