function showAlertSuccess(alertId) {
    var alertBox = document.getElementById(alertId);
    if (alertBox) {
        alertBox.classList.add("show-alert");

        setTimeout(() => {
            alertBox.classList.remove("show-alert");

            // Opsiyonel olarak DOM'dan tamamen silmek istersen:
            setTimeout(() => {
                alertBox.remove();
            }, 500); // animasyon süresiyle eşle
        }, 3000);
    }
}
