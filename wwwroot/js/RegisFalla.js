document.addEventListener("DOMContentLoaded", function () {
    var TipoClas = document.getElementById('TipoClas');
    var forllantas = document.getElementById('forllantas');
    var TipTicket = document.getElementById('TipTicket');
    var Remolque = document.getElementById('Remolque');
    var Tractor = document.getElementById('Tractor');
    var Numope = document.getElementById('Numope');
    var NomOpe = document.getElementById('NomOpe');
    var Eco = document.getElementById('Eco');
    var selAli = document.getElementById('selAli');
    var ClTpOp = document.getElementById('ClTpOp');
    var Ruta = document.getElementById('Ruta');
    var Alia = document.getElementById('Alias');
    var Numero = document.getElementById('Numero');
    var ClOp = document.getElementById('ClOp');
    var desclTpOp = document.getElementById('desclTpOp');
    var NomTipOp = document.getElementById('NomTipOp');
    var ClaveTipoEquipo = document.getElementById('ClaveTipoEquipo');
    var cvTipoequipo = document.getElementById('cvTipoequipo');
    function fillValues(selectedIndex, targetValue, targetText) {
        targetValue.value = selectedIndex !== -1 ? selectedIndex.options[selectedIndex.selectedIndex].value : '';
        targetText.value = selectedIndex !== -1 ? selectedIndex.options[selectedIndex.selectedIndex].text : '';
    }
    Ruta.disabled = true;
    Numope.addEventListener('change', function () {
        NomOpe.value = Numope.value;
    });
    NomOpe.addEventListener('change', function () {
        Numope.value = NomOpe.value;
    });
    TipTicket.addEventListener('change', function ()
    {
        if (TipTicket.value == 1) {
            Tractor.style.display = "block";
            Remolque.style.display = "none";

        }
        else if (TipTicket.value == 2) {
            Tractor.style.display = "none";
            Remolque.style.display = "block";
            //ClaveTipoEquipo.selectedIndex = 0;
        }
        else
        {
            Tractor.style.display = "none";
            Remolque.style.display = "none";
        }
    });
    TipoClas.addEventListener('change', function () {
        if (TipoClas.value == 2) {
            forllantas.style.display = "block";
        }
        else {
            forllantas.style.display = "none";
        }
    });
    Eco.addEventListener('change', function () {
        selAli.value = Eco.value;
        ClTpOp.value = Eco.value;
        desclTpOp.value = Eco.value;
        var valorClTpOp = ClTpOp.value;
        ClTpEquipo.value = Eco.value;

        fillValues(Eco, ClTpOp, Numero);
        fillValues(selAli, ClTpOp, Alia);
        fillValues(ClTpOp, ClTpOp, ClOp);

        Ruta.value = '[Selecciona]';
        Ruta.disabled = true;
        NomTipOp.value = desclTpOp.options[desclTpOp.selectedIndex].text;
        ClaveTipoEquipo.value = ClTpEquipo.options[ClTpEquipo.selectedIndex].text;
        cvTipoequipo.value = ClaveTipoEquipo.value;
        if (NomTipOp.value == 'SEPOMEX') {
            Ruta.disabled = false;
        }

    });
    selAli.addEventListener('change', function () {
        Eco.value = selAli.value;
        ClTpOp.value = selAli.value;
        desclTpOp.value = selAli.value;
        var valorClTpOp = ClTpOp.value;
        ClTpEquipo.value = Eco.value;

        fillValues(Eco, ClTpOp, Numero);
        fillValues(selAli, ClTpOp, Alia);
        fillValues(ClTpOp, ClTpOp, ClOp);
        Ruta.value = '[Selecciona]';
        Ruta.disabled = true;
        NomTipOp.value = desclTpOp.options[desclTpOp.selectedIndex].text;
        ClaveTipoEquipo.value = ClTpEquipo.options[ClTpEquipo.selectedIndex].text;
        cvTipoequipo.value = ClaveTipoEquipo.value;
        if (NomTipOp.value == 'SEPOMEX') {
            Ruta.disabled = false;
        }
    });
});
document.addEventListener("DOMContentLoaded", function () {

    var remo1 = document.getElementById('opcionesRemolque1');
    var remo2 = document.getElementById('opcionesRemolque2');
    var Remolque1 = document.getElementById('Remolque1');
    var Remolque2 = document.getElementById('Remolque2');
    remo1.addEventListener('change', function () {
        Remolque1.value = remo1.options[remo1.selectedIndex].text;
    });
    remo2.addEventListener('change', function () {
        Remolque2.value = remo2.options[remo2.selectedIndex].text;
    });

    
});

$(document).ready(function () {
    var ChckRemolMostrar = document.getElementById('ChckRemolMostrar');
    var txtchekRemol = document.getElementById('txtchekRemol');
    var divremo = document.getElementById('divremo');
    ChckRemolMostrar.addEventListener('change', function () {
        if (this.checked) {
            txtchekRemol.value = true;
            divremo.style.display = "block";
        }
        else {
            txtchekRemol.value = false;
            divremo.style.display = "none";
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

var map = null;
var marker = null;
function PintaMapa(inputLngValue, inputLatValue, DirGPS) {
    if (!map) {
        map = L.map('map').setView([inputLatValue, inputLngValue], 15);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
    }

    if (marker) {
        map.removeLayer(marker);
    }

    marker = L.marker([inputLatValue, inputLngValue]).addTo(map);
    marker.bindPopup("Última ubicación reportada:\n" + DirGPS).openPopup();
    map.setView([inputLatValue, inputLngValue], 15);
}
function EnvioUbica() {
    var Numero = document.getElementById('Numero').value;
    var Alias = document.getElementById('Alias').value;
    var ClOp = document.getElementById('ClOp').value;
    var cvEmp = document.getElementById('cvEmp').value;
    var inputLng = document.getElementById('inputLng');
    var inputLat = document.getElementById('inputLat');
    var DirGPS = document.getElementById('DirGPS');
    var FechGPS = document.getElementById('FechGPS');
    var map = document.getElementById('map');

    var url = new URL('https://webportal.tum.com.mx/wsstmdv/api/execspxor');
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    var raw = JSON.stringify({
        "data": {
            "bdCc": 5,
            "bdSch": "dbo",
            "bdSp": "SPQRY_UltimaPosicion"
        },
        "filter": [
            {
                "property": "ClaveEmpresa",
                "value": cvEmp
            },
            {
                "property": "Unidad",
                "value": Numero
            },
            {
                "property": "Alias",
                "value": Alias
            },
            {
                "property": "ClaveOperacion",
                "value": ClOp
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
                var mensaje = document.getElementById('mensaje').value;
                mensaje = obj;
                if (mensaje !== '') {
                    toastr.error(mensaje);
                }
            }
            else if (obj.data.length == 0) {
                var mensaje = document.getElementById('mensaje').value;
                var TiGpS = document.getElementById('TiGpS');
                mensaje = 'No se encuentrar datos GPS';
                inputLat.value = "";
                inputLng.value = "";
                DirGPS.value = "";
                FechGPS.value = "";
                map.style.display = "none";
                TiGpS.style.display = "none";
                if (mensaje !== '') {
                    toastr.error(mensaje);
                }
            } 
            else {
                inputLat.value = obj.data[0].UltimaPosicion[0].Latitud;
                inputLng.value = obj.data[0].UltimaPosicion[0].Longitud;
                DirGPS.value = obj.data[0].UltimaPosicion[0].Position;
                FechGPS.value = obj.data[0].UltimaPosicion[0].SendTime;
                document.getElementById('DirGPSHidden').value = DirGPS.value;
                document.getElementById('FechGPSHidden').value = FechGPS.value;
                const lat = parseFloat(inputLat.value);
                const lng = parseFloat(inputLng.value);
                if (!isNaN(lat) && !isNaN(lng)) {
                    var TiGpS = document.getElementById('TiGpS');
                    map.style.display = "block";
                    TiGpS.style.display = "block";
                    PintaMapa(lng, lat, DirGPS.value);
                }
                else {
                    var mensaje = document.getElementById('mensaje').value;
                    mensaje = "Error al obtener coordenadas válidas."
                    if (mensaje !== '') {
                        toastr.error(mensaje);
                    }
                }
            }
        })
        .catch(error => console.log("error", error));
}


