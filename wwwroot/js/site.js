// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



function checkPasswordStrength() {
    var password = document.getElementById("password").value;
    var strengthBar = document.getElementById("password-strength-bar");
    var strengthText = document.getElementById("password-strength-text");

    var strength = 0;

    if (password.length >= 6) strength += 1;
    if (password.length >= 8) strength += 1;
    if (/[A-Z]/.test(password)) strength += 1;
    if (/[a-z]/.test(password)) strength += 1;
    if (/[0-9]/.test(password)) strength += 1;
    if (/[^A-Za-z0-9]/.test(password)) strength += 1;

    switch (strength) {
        case 0:
            strengthBar.style.width = "0%";
            strengthBar.className = "progress-bar bg-danger";
            strengthText.textContent = "";
            break;
        case 1:
            strengthBar.style.width = "16%";
            strengthBar.className = "progress-bar bg-danger";
            strengthText.textContent = "Şifreniz Çok Zayıf";
            break;
        case 2:
            strengthBar.style.width = "33%";
            strengthBar.className = "progress-bar bg-warning";
            strengthText.textContent = "Şifreniz Zayıf";
            break;
        case 3:
            strengthBar.style.width = "50%";
            strengthBar.className = "progress-bar bg-info";
            strengthText.textContent = "Şifreniz Orta";
            break;
        case 4:
            strengthBar.style.width = "66%";
            strengthBar.className = "progress-bar bg-primary";
            strengthText.textContent = "Şifreniz Güçlü";
            break;
        case 5:
            strengthBar.style.width = "83%";
            strengthBar.className = "progress-bar bg-success";
            strengthText.textContent = "Şifreniz Çok Güçlü";
            break;
        case 6:
            strengthBar.style.width = "100%";
            strengthBar.className = "progress-bar bg-success";
            strengthText.textContent = "Şifreniz Mükemmel";
            break;
    }
}

   