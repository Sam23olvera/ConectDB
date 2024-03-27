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
                "bdSp": "SPQRY_FinRepFalla"
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
