$(document).ready(function () {
    $('#FEG').datetimepicker({
        format: 'Y/m/d H:i'
    });
    $('#FSG').datetimepicker({
        format: 'Y/m/d H:i'
    });
    $('#Fecha').datetimepicker({
        format: 'Y/m/d'
    });
});
function calendarioFEG(CVR) {
    var Inp = "FEG-" + CVR;
    var expand = document.getElementById(Inp);
    $(expand).datetimepicker({
        format: 'Y/m/d H:i'
    });
}
function calendarioFSG(CVR) {
    var Inp = "FSG-" + CVR;
    var expand = document.getElementById(Inp);
    $(expand).datetimepicker({
        format: 'Y/m/d H:i'
    });
}


document.addEventListener("DOMContentLoaded", function () {
    const checkbox = document.getElementById("checkboxMostrarOcultar");
    const llegadaInputs = document.querySelectorAll(".llegada-input");

    function toggleLLegadaElements() {
        llegadaInputs.forEach(function (input) {
            input.style.display = checkbox.checked ? "block" : "none";
            input.style.width = "135pt";
        });
    }

    toggleLLegadaElements();
    checkbox.addEventListener("change", toggleLLegadaElements);

    $('#check-all-Llegadas').change(function () {
        if ($(this).is(':checked')) {

            $('.checkbox-group-Llegadas').prop('checked', true);
        } else {

            $('.checkbox-group-Llegadas').prop('checked', false);
        }
    });
    $('#check-all-Salidas').change(function () {
        if ($(this).is(':checked')) {

            $('.checkbox-group-Salidas').prop('checked', true);
        } else {
            $('.checkbox-group-Salidas').prop('checked', false);
        }
    })
});

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
