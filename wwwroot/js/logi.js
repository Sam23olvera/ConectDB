$(document).ready(function () {
    var mensaje = document.getElementById('mensaje').value;
    var guarda = document.getElementById('guarda').value;
    if (mensaje !== '') {
        toastr.error(mensaje);
    }
    if (guarda !== '') {
        toastr.success(guarda);
    }
});

//document.addEventListener("DOMContentLoaded", function () {
//    var selectEmpresa = document.getElementById('selectEmpresa');
//    var cveEmp = document.getElementById('cveEmp');
//    selectEmpresa.addEventListener('change', function () {
//        cveEmp.value = selectEmpresa.value;
//    });
//});
document.getElementById("selectEmpresa").addEventListener("change", function () {

    var selectedValue = this.value;
    document.getElementById("cveEmp").value = selectedValue;
    // Enviar el formulario automáticamente al seleccionar una opción
    document.getElementById("formAcceder").submit();
});
