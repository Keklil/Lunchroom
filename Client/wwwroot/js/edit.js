$(document).ready(function () {
        var myAlert =document.getElementById('toastNotice');//select id of toast
        var bsAlert = new bootstrap.Toast(myAlert);//inizialize it
        bsAlert.show();//show it
});