﻿window.onload = function () {
    document.getElementById("spinner-overlay").style.display = "none"; 
};

document.addEventListener("DOMContentLoaded", function () {
    var links = document.querySelectorAll(".carga");
    var pagin = document.querySelectorAll(".page-link");
    links.forEach(function (link) {
        link.addEventListener("click", function () {
            document.getElementById("spinner-overlay").style.display = "block";
        });
    });

    pagin.forEach(function (link) {
        link.addEventListener("click", function () {
            document.getElementById("spinner-overlay").style.display = "block";
        });
    });
});

$(document).ready(function () {
    document.getElementById('archivo').addEventListener('change', function (event) {
        var file = event.target.files[0];
        var mimeType = file.type;

        // Verificar si el archivo es una imagen o un video
        if (mimeType.includes('image') || mimeType.includes('video')) {
            var reader = new FileReader();

            reader.onload = function () {
                var image = new Image();

                image.onload = function () {
                    var width = this.width;
                    var height = this.height;

                    // Comprimir el archivo si es una imagen
                    if (mimeType.includes('image')) {
                        new Compressor(file, {
                            quality: 0.6, // Calidad de compresión (valor entre 0 y 1)
                            maxWidth: 800, // Ancho máximo de la imagen comprimida
                            maxHeight: 600, // Altura máxima de la imagen comprimida
                            success: function (result) {
                                // Aquí puedes hacer algo con el archivo comprimido, como subirlo al servidor
                                alert('Archivo comprimido:', result);
                                var formData = new FormData();
                                formData.append('file', result);

                                fetch('//webportal/ImagTest', {
                                    method: 'POST',
                                    body: formData
                                })
                                    .then(response => {
                                        if (response.ok) {
                                            alert('Archivo subido exitosamente');
                                        } else {
                                            alert('Error al subir el archivo');
                                        }
                                    })
                                    .catch(error => {
                                        alert('Error de red:', error);
                                    });
                            }
                        });
                    } else {
                        // No es una imagen, puedes hacer algo diferente aquí
                        alert('No es una imagen');
                    }
                };

                image.src = reader.result;
            };

            reader.readAsDataURL(file);

        } else {
            // No es una imagen ni un video, puedes mostrar un mensaje de error o hacer algo diferente aquí
            alert('Formato de archivo no compatible');
        }
    });
});


$(document).ready(function () {
    var ctx = document.getElementById('barChart').getContext('2d');

    var barChart = new Chart(ctx, {
        type: 'polarArea',
        data: {
            labels: [
                $('#Name-Esta-1').val(),
                $('#Name-Esta-2').val(),
                $('#Name-Esta-3').val(),
                $('#Name-Esta-4').val(),
                $('#Name-Esta-5').val()
            ],
            datasets: [{
                label: 'Reporte Mensual', // Etiqueta de la leyenda
                data: [
                    parseInt($('#Esta-1').val()),
                    parseInt($('#Esta-2').val()),
                    parseInt($('#Esta-3').val()),
                    parseInt($('#Esta-4').val()),
                    parseInt($('#Esta-5').val())
                ], // Datos para las barras
                //data: [12, 16, 7, 3, 14], // Datos para las barras
                backgroundColor: [ 'rgb(255, 99, 132)','rgb(75, 192, 192)','rgb(255, 205, 86)','rgb(201, 203, 207)','rgb(54, 162, 235)']// color de cada uno de las rebanadas
                //backgroundColor: 'rgba(54, 162, 235, 0.2)', // Color de fondo de las barras
                //borderColor: 'rgba(54, 162, 235, 1)', // Color del borde de las barras
                //borderWidth: 1 // Ancho del borde de las barras
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true // Empezar en cero en el eje y
                    }
                }]
            }
        }
    });
});
$(document).ready(function () {
    $('#FehTick').datetimepicker({
        //format: 'm/d/Y'
        format: 'Y/m/d'
    });
    $('#TiempAsig').datetimepicker({
        format: 'Y/m/d H:i'
    });
    $('#datetimepicker').datetimepicker({
        format: 'm/d/Y H:i'
    });
    $('#FechEstima').datetimepicker({
        format: 'm/d/Y H:i'
    });
    $('#FehInicio').datetimepicker({
        format: 'Y/m/d',
        maxDate: "1M",
        onClose: function (selectedDate) {
            var endDate = new Date(selectedDate);
            endDate.setDate(endDate.getDate() + 31);
            $('#FehFin').datetimepicker('option', 'maxDate', endDate);

            var FehFin = document.getElementById('FehFin');
            let fechaprueba = new Date(endDate);
            FehFin.value = fechaprueba.getFullYear() + '/' + (fechaprueba.getMonth() + 1) + '/' + fechaprueba.getDate();    
        }
    });
    //$('#FehFin').datetimepicker({
    //    format: 'Y/m/d',
    //    minDate: "-1M"
    //});
});
function cal(numTicket) {
    var Inp = "FechEstima-" + numTicket;
    var expand = document.getElementById(Inp);
    $(expand).datetimepicker({
        format: 'Y/m/d H:i'
    });
}
function calendario(numTicket) {
    var Tiem = "TiempAsig-" + numTicket;
    var TiempAsig = document.getElementById(Tiem);
    $(TiempAsig).datetimepicker({
        format: 'Y/m/d H:i'
    });
}

function llenarDiesel(numTick)
{
    var nameChkDisel = 'ChkDisel-' + numTick;
    var txtnameCheckDisel = 'CheckDisel-' + numTick;
    var ChkDisel = document.getElementById(nameChkDisel);
    var txtCheckDisel = document.getElementById(txtnameCheckDisel);
    txtCheckDisel.value = ChkDisel.value;

    if (ChkDisel.checked == true) {
        txtCheckDisel.value = 1;
    }
    else if (ChkDisel.checked == false) {
        txtCheckDisel.value = 0;
    }
}

function llenarGrua(numTick)
{
    var nameChkGrua = 'ChkGrua-' + numTick;
    var txtnameCheckGrua = 'CheckGrua-' + numTick;
    var ChkGrua = document.getElementById(nameChkGrua);
    var txtCheckGrua = document.getElementById(txtnameCheckGrua);
    if (ChkGrua.checked == true) {
        txtCheckGrua.value = 1;
    }
    else if (ChkGrua.checked == false) {
        txtCheckGrua.value = 0;
    }
}

$(document).ready(function () {
    var checkIni = document.getElementById('checkIni');
    var checkIniValue = document.getElementById('checkIniValue').value;
    var checkFin = document.getElementById('checkFin');
    var checkFinValue = document.getElementById('checkFinValue').value;

    if (checkIniValue === 'true') {
        checkIni.checked = true;
    }
    else {
        checkIni.checked = false;
    }
    if (checkFinValue === 'true') {
        checkFin.checked = true;
    }
    else
    {
        checkFin.checked = false;
    }

});
function mostrarllantitas(numTicket)
{
    var selectReclas_Asigna = "Reclas_Asigna-" + numTicket
    var Reclas_Asigna = document.getElementById(selectReclas_Asigna);
    var mostTab = '.llantitas-' + numTicket;

    if (Reclas_Asigna.value == 2) {
        $(mostTab).show();
        $('.headllantitas').show();
    }
    else {
        $(mostTab).hide();
        $('.headllantitas').hide();
    }
}

function asignarTicket(numTicket)
{
    var selectId = "select-" + numTicket;
    var selectElement = document.getElementById(selectId);
    var valorSeleccionado = selectElement.value;
    var botonAsigna = document.getElementById("asigna-" + numTicket);
    var nuevaURL = botonAsigna.getAttribute("href").replace("asp-route-Asigna=", "asp-route-Asigna=" + valorSeleccionado);
    // Asignar la nueva URL al enlace del botón
    botonAsigna.setAttribute("href", nuevaURL);
}
function pinta()
{

    var MenIni = document.getElementById('MenIni');
    var MenFal = document.getElementById('MenFal');
    var MenPorAsig = document.getElementById('MenPorAsig');
    var MenAsig = document.getElementById('MenAsig');
    var MenTipApoyo = document.getElementById('MenTipApoyo');
    var MenRep = document.getElementById('MenRep');
    var MenFin = document.getElementById('MenFin');
}

$(document).ready(function () {
    var checkIni = document.getElementById('checkIni');
    var checkIniValue = document.getElementById('checkIniValue');
    var checkFin = document.getElementById('checkFin');
    var checkFinValue = document.getElementById('checkFinValue');
    checkIni.addEventListener('change', function () {
        if (this.checked) {
            checkIniValue.value = true;
            checkFin.checked = false;
            checkFinValue.value = false;
        } else {
            checkIniValue.value = false;
            checkFin.checked = true;
            checkFinValue.value = true;
        }
    });

    checkFin.addEventListener('change', function () {
        if (this.checked) {
            checkFinValue.value = true;
            checkIni.checked = false;
            checkIniValue.value = false;
        } else {
            checkFinValue.value = false;
            checkIni.checked = true;
            checkIniValue.value = true;
        }
    });

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
function mostrar(NumTicket) {
    var dat = document.getElementById('datos-' + NumTicket);
    if (dat.style.display === "none") {
        dat.style.display = "block";
    } else {
        dat.style.display = "none";
    }
}

function Exporta()
{
    var Export = document.getElementById('ExportExcel');
    Export.value = "";
    Export.value = true;
}
function Buscar()
{
    var Export = document.getElementById('ExportExcel');
    Export.value = "";
    Export.value = false;
}
function finalizar(cveEmp, ClavUsu, ClavCtRep) {
    var selTBCAT_TipoFalla = document.getElementById('selTBCAT_TipoFalla-' + ClavCtRep);
    var obsMat = document.getElementById('obsMat-' + ClavCtRep);
    var mensaje = document.getElementById('mensaje').value;
    var guarda = document.getElementById('guarda').value;
    var DesFalrel = document.getElementById('DesFalrel-' + ClavCtRep);
    var btnFinal = document.getElementById('btnFinal-' + ClavCtRep);
    var FechaFini = document.getElementById('FechaFini-' + ClavCtRep);

    if (selTBCAT_TipoFalla.value == '[Selecciona]') {
        mensaje = 'No selecciono una Falla';
        toastr.error(mensaje);
    }
    else if (obsMat.value === "")
    {
        mensaje = 'Describa la Falla';
        toastr.error(mensaje);
    }
    else
    {
        var url = new URL('https://webportal.tum.com.mx/wsstmdv/api/execspxor');
        var myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");

        var raw = JSON.stringify({
            "data": {
                "bdCc": 5,
                "bdSch": "dbo",
                "bdSp": "SPUPD_FinRepFalla"
            },
            "filter": [
                {
                    "property": "ClaveEmpresa",
                    "value": cveEmp
                },
                {
                    "property": "ClaveTipFalla",
                    "value": selTBCAT_TipoFalla.value
                },
                {
                    "property": "ObsMantenimiento",
                    "value": obsMat.value
                },
                {
                    "property": "ClaveUser",
                    "value": ClavUsu
                },
                {
                    "property": "NumeroSolicitud",
                    "value": ClavCtRep
                }
            ]
        });

        var requestOptions = {
            method: "POST",
            headers: myHeaders,
            body: raw,
            redirect: "follow"
        };

        fetch(url, requestOptions)
            .then(response => response.text())
            .then(result => {
                const obj = JSON.parse(result);
                //console.log(obj);
                if (obj.data == null) {
                    mensaje = obj.message;
                    if (mensaje !== '') {
                        toastr.error(mensaje);
                    }
                }
                else {
                    FechaFini.value = obj.data[0].Respuesta[0].FechaFinalizacion;
                    DesFalrel.disabled = true;                    
                    selTBCAT_TipoFalla.disabled = true;
                    btnFinal.disabled = true;
                    obsMat.disabled = true;
                    guarda = obj.message;
                    if (guarda !== '') {
                        toastr.success(guarda);
                    }
                }
            })
            .catch(error => console.log("error", error));
    }

}
