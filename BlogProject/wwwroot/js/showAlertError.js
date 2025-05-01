function showAlertError(alertId) {
    var alertBox = document.getElementById(alertId);
    if (alertBox) {
        alertBox.classList.add("show-alert");

        setTimeout(() => {
            alertBox.classList.remove("show-alert");

            // 0.5 saniye sonra (animasyon bitince) DOM'dan sil
            setTimeout(() => {
                alertBox.remove();
            }, 500);
        }, 3000);
    }
}
