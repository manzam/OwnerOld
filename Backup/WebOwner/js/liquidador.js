var $j = jQuery.noConflict();

var yyyy = -1;
var mm = -1;
var numOwners = 0;
var idHotelSel = -1;
var progressbarControl;
var sourceOwner;
var sourceOwnerTmp;
var sourceReglas;
var sourceVValor;
var sourceReglasHotel;
var sourceReglasHotelTmp;
var okLiqHotel = false;

var reglasOrdenadas = [];
var sourceliquidacion = [];
var sourceliquidacionHotel = [];

$j(document).ready(function () {

    $j("#modalLiq").dialog({
        autoOpen: false,
        resizable: false,
        show: "slow",
        modal: true,
        height: "auto",
        title: "Liquidación",
        open: function (event, ui) {
            $j(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });

    $j("#modalLiqInfo").dialog({
        autoOpen: false,
        resizable: false,
        show: "slow",
        modal: true,
        height: "auto",
        buttons: {
            Aceptar: function () {
                $j(this).dialog("close");
            }
        }
    });

    progressbarControl = $j("#progressbar");
    $j("#txtYYYY,#txtFechaDeleteLiq").val((new Date().getFullYear()));
    CargarPropietarios();

    $j("select[id$='ddlHotel']").change(function () {
        CargarPropietarios();
    });

    $j("#ddlMes,#txtYYYY").change(function () {
        CargarPropietarios();
    });

});

// ventanaLiq();
function ClearControl() {
    $j("#lbltextoError,#lbltextoExito,#textmodalLiqInfo,#textModalLiq").text("");
    $j("#divError,#divExito").hide();
}

function GetObjetoliquidacion() {
    var oLiq = {};
    oLiq.IdPropietario = -1;
    oLiq.IdSuit = -1;
    oLiq.IdHotel = idHotelSel;
    oLiq.IdConcepto = -1;
    oLiq.Valor = 0;
    oLiq.Regla = "";
    oLiq.NumIdentificacion = "";
    oLiq.NumEscritura = "";
    oLiq.NumSuite = "";
    oLiq.FullNombre = "";
    oLiq.EsliquidacionHotel = false;
    oLiq.Orden = 0;
    oLiq.OK = true;
    oLiq.ListaError = [];
    oLiq.ListaConceptos = [];
    return oLiq;
}

function validarVariables_HV(itemLista) {
    var isValid = true;
    itemLista.forEach(function (item, index) {
        if (item.NomClass == "varH" || item.NomClass == "varV" || item.NomClass == "")
            isValid = isValid && true;
        else
            isValid = false;
    });
    return isValid;
}

function removeItemFromArr(arr, item) {
    var i = arr.indexOf(item);
    if (i != -1)
        arr.splice(i, 1);
}

function GetValorVariable(tipo, idVariable, idSuit, idOwner, isHotel = false) {

    try {
        var valVariable = null;

        if (tipo == "varC") {
            if (isHotel) {
                valVariable = (sourceliquidacionHotel.filter(r => r.IdConcepto == idVariable))[0].Valor;
            } else {
                var ownerLiqTmp = sourceliquidacion.filter(r => r.IdPropietario == idOwner && r.IdSuit == idSuit);
                if (ownerLiqTmp.length > 0)
                    valVariable = (ownerLiqTmp[0].ListaConceptos.filter(r => r.IdConcepto == idVariable))[0].Valor;
            }
        } else {
            switch (tipo) {
                case "varCO":
                case "varH":
                    valVariable = (sourceVValor.filter(r => r.IdVariable == idVariable))[0].Valor;
                    break;
                case "varV":
                    valVariable = (sourceVValor.filter(r => r.IdVariable == idVariable && r.IdSuit == idSuit && r.IdOwner == idOwner))[0].Valor;
                    break;
                default:
                    break;
            }
        }
        return valVariable;
    } catch (e) {
        return null;
    }
}

function LiquidarHotel() {

    ClearControl();
    $j("#modalLiq").dialog("open");

    var rt = 0;
    sourceliquidacionHotel = [];
    sourceReglasHotel = sourceReglasHotelTmp.slice(0);
    idHotelSel = jQuery("select[id$='ddlHotel']").val();

    while (sourceReglasHotel.length > 0) {
        if (rt > sourceReglasHotel.length - 1) rt = 0;

        var reglaReplace = "";
        var reglaTmp = sourceReglasHotel[rt];

        var reglaReplace = reglaTmp.Regla;
        reglaTmp.ListaConceptos.forEach(function (itemConcepto, index) {
            if (itemConcepto.NomClass != "") {
                var idVarTmp = ((itemConcepto.IdConcepto == -1 ? itemConcepto.IdVariable : itemConcepto.IdConcepto))
                var valor = GetValorVariable(itemConcepto.NomClass, idVarTmp, -1, -1, true);
                if (valor != null)
                    reglaReplace = reglaReplace.replace(itemConcepto.NomVariable, valor);
            }
        });

        try {
            var objConcepto = {};
            objConcepto.IdConcepto = reglaTmp.IdConcepto;
            objConcepto.NomConcepto = reglaTmp.NombreConcepto;
            objConcepto.FechaPeriodoLiquidado = new Date(yyyy, (mm - 1), 1);
            objConcepto.IdHotel = idHotelSel;

            var valorDec = 0;
            valorDec = eval(reglaReplace);
            objConcepto.Valor = parseFloat(valorDec.toFixed(reglaTmp.NumDecimales));

            objConcepto.Regla = reglaTmp.Regla;
            objConcepto.ReglaReplace = reglaReplace;

            sourceliquidacionHotel.push(objConcepto);
            removeItemFromArr(sourceReglasHotel, reglaTmp);
            rt = 0;
        } catch (e) {
            rt = rt + 1;
        }
    }

    MostrarLiquidacionHotel();

    $j("#modalLiq").dialog("close");
}

function ObtenerOrdenEjecucionReglas() {

    // Buscamos que la regla solo tenga Variables tipo varH, varV
    sourceReglas.forEach(function (itemFoundHV, index) {
        if (validarVariables_HV(itemFoundHV.ListaConceptos))
            reglasOrdenadas.push(itemFoundHV);
    });

    // Eliminamos las reglas que no dependen de ninguna otra regla
    reglasOrdenadas.forEach(function (itemRemove, index) {
        removeItemFromArr(sourceReglas, itemRemove);
    });

    // Con el primer propietario ejecutamos las reglas que no depeden de ninguna otra regla
    var ownerFirst = sourceOwner[0];

    var objLiq = GetObjetoliquidacion();
    objLiq.IdPropietario = ownerFirst.IdOwner;
    objLiq.IdSuit = ownerFirst.IdSuite;
    objLiq.IdHotel = idHotelSel;
    objLiq.NumIdentificacion = ownerFirst.NumIdentificacion;
    objLiq.FullNombre = ownerFirst.FullNombre;
    objLiq.NumEscritura = ownerFirst.NumEscritura;
    objLiq.NumSuite = ownerFirst.NumSuite;
    objLiq.FechaPeriodoLiquidado = new Date(yyyy, (mm - 1), 1);

    reglasOrdenadas.forEach(function (itemRegla, index) {
        var objConcepto = {};
        objConcepto.IdConcepto = itemRegla.IdConcepto;
        objConcepto.NomConcepto = itemRegla.NombreConcepto;

        var reglaReplace = itemRegla.Regla;

        itemRegla.ListaConceptos.forEach(function (itemConcepto, index) {
            if (itemConcepto.NomClass != "") {
                var idVarTmp = ((itemConcepto.IdConcepto == -1 ? itemConcepto.IdVariable : itemConcepto.IdConcepto))
                reglaReplace = reglaReplace.replace(itemConcepto.NomVariable, GetValorVariable(itemConcepto.NomClass, idVarTmp, ownerFirst.IdSuite, ownerFirst.IdOwner));
            }
        });

        var valorDec = 0;
        valorDec = eval(reglaReplace);
        objConcepto.Valor = parseFloat(valorDec.toFixed(itemRegla.NumDecimales));
        objConcepto.Regla = itemRegla.Regla;
        objConcepto.Orden = itemRegla.Orden;
        objConcepto.ReglaReplace = reglaReplace;
        objLiq.ListaConceptos.push(objConcepto);
    });

    sourceliquidacion.push(objLiq);

    // Ejucutamos las demas reglas
    var rt = 0;
    while (sourceReglas.length > 0) {
        if (rt > sourceReglas.length - 1) rt = 0;
        var reglaReplace = "";
        var reglaTmp = sourceReglas[rt];

        var reglaReplace = reglaTmp.Regla;
        reglaTmp.ListaConceptos.forEach(function (itemConcepto, index) {
            if (itemConcepto.NomClass != "") {
                var idVarTmp = ((itemConcepto.IdConcepto == -1 ? itemConcepto.IdVariable : itemConcepto.IdConcepto))
                var valor = GetValorVariable(itemConcepto.NomClass, idVarTmp, ownerFirst.IdSuite, ownerFirst.IdOwner);
                if (valor != null)
                    reglaReplace = reglaReplace.replace(itemConcepto.NomVariable, valor);
            }
        });

        try {
            var objConcepto = {};
            objConcepto.IdConcepto = reglaTmp.IdConcepto;
            objConcepto.NomConcepto = reglaTmp.NombreConcepto;

            var valorDec = 0;
            valorDec = eval(reglaReplace);
            objConcepto.Valor = parseFloat(valorDec.toFixed(reglaTmp.NumDecimales));

            objConcepto.Regla = reglaTmp.Regla;
            objConcepto.Orden = reglaTmp.Orden;
            objConcepto.ReglaReplace = reglaReplace;
            objLiq.ListaConceptos.push(objConcepto);

            reglasOrdenadas.push(reglaTmp);
            removeItemFromArr(sourceReglas, reglaTmp);
            rt = 0;
        } catch (e) {
            rt = rt + 1;
        }
    }
}

function EjecutarReglas() {

    for (var i = 1; i < sourceOwner.length; i++) {

        var objLiq = GetObjetoliquidacion();
        objLiq.IdPropietario = sourceOwner[i].IdOwner;
        objLiq.IdSuit = sourceOwner[i].IdSuite;
        objLiq.IdHotel = idHotelSel;
        objLiq.NumIdentificacion = sourceOwner[i].NumIdentificacion;
        objLiq.FullNombre = sourceOwner[i].FullNombre;
        objLiq.NumEscritura = sourceOwner[i].NumEscritura;
        objLiq.NumSuite = sourceOwner[i].NumSuite;
        objLiq.FechaPeriodoLiquidado = new Date(yyyy, (mm - 1), 1);

        sourceliquidacion.push(objLiq);

        reglasOrdenadas.forEach(function (itemRegla, index) {

            var objConcepto = {};
            var reglaReplace = itemRegla.Regla;
            itemRegla.ListaConceptos.forEach(function (itemConcepto, index) {
                if (itemConcepto.NomClass != "") {
                    var idVarTmp = ((itemConcepto.IdConcepto == -1 ? itemConcepto.IdVariable : itemConcepto.IdConcepto))
                    reglaReplace = reglaReplace.replace(itemConcepto.NomVariable, GetValorVariable(itemConcepto.NomClass, idVarTmp, sourceOwner[i].IdSuite, sourceOwner[i].IdOwner));
                }
            });

            try {
                objConcepto.IdConcepto = itemRegla.IdConcepto;
                objConcepto.NomConcepto = itemRegla.NombreConcepto;

                var valorDec = 0;
                valorDec = eval(reglaReplace);
                objConcepto.Valor = parseFloat(valorDec.toFixed(itemRegla.NumDecimales));

                objConcepto.Regla = itemRegla.Regla;
                objConcepto.Orden = itemRegla.Orden;
                objConcepto.ReglaReplace = reglaReplace;
                objLiq.ListaConceptos.push(objConcepto);
            } catch (e) {
                var objError = {};
                objError.NombreConcepto = itemRegla.NombreConcepto
                objError.Regla = itemRegla.Regla;
                objError.ReglaRemplazo = reglaReplace;
                objLiq.ListaError.push(objError);
                objLiq.OK = false;
            }
        });

        Progress();
    }
}

function MostrarLiquidacionHotel() {
    var htmlDetail = "";
    sourceliquidacionHotel.forEach(function (itemLiquidacion, indexLiq) {
        htmlDetail += '<tr>';
        htmlDetail += `<td class="detailCelda">${indexLiq + 1}</td><td class="detailName">${itemLiquidacion.NomConcepto}</td><td class="detailCelda" style="text-align: right;" title=" ${itemLiquidacion.Regla} || ${itemLiquidacion.ReglaReplace} ">${$j.number(itemLiquidacion.Valor, 2, ',', '.')}</td>`;
        htmlDetail += '</tr>';
    });
    $j("#tblResultDetailLiqHotel").empty();
    $j("#tblResultDetailLiqHotel").html(htmlDetail);
    $j("#tabs-1").css("height", "auto");
    $j("#divLiqHotel").show();
    $j('.values').number(true, 2)

}

function MostrarLiquidacion() {

    // Ordenamos las reglas
    reglasOrdenadas.sort(function (a, b) {
        return (a.Orden - b.Orden)
    });

    // Obtenemos los nombre de las columna "Conceptos"
    var htmlColumns = '<tr>';
    htmlColumns += '<th>#</th><th>Propietario</th><th>Nit</th><th>N° Suit</th><th>N° Escritura</th>';

    reglasOrdenadas.forEach(function (itemColumn, index) {
        if (itemColumn.Orden != -1) {
            htmlColumns += `<th>${itemColumn.NombreConcepto}</th>`;
        }
    });
    htmlColumns += '</tr>';
    $j("#tblResultColumnsLiq").empty();
    $j("#tblResultColumnsLiq").html(htmlColumns);

    var htmlDetail = "";
    sourceliquidacion.forEach(function (itemLiquidacion, indexLiq) {
        htmlDetail += '<tr>';
        htmlDetail += `<td style="text-align: center;">${indexLiq + 1}</td><td>${itemLiquidacion.FullNombre}</td><td>${itemLiquidacion.NumIdentificacion}</td><td style="text-align: center;">${itemLiquidacion.NumSuite}</td><td style="text-align: center;">${itemLiquidacion.NumEscritura}</td>`;

        itemLiquidacion.ListaConceptos.sort(function (a, b) { return (a.Orden - b.Orden) });

        itemLiquidacion.ListaConceptos.forEach(function (itemRegla, indexRegla) {
            if (itemRegla.Orden != -1) {
                htmlDetail += `<td style="text-align: right;" title=" ${itemRegla.Regla} || ${itemRegla.ReglaReplace} ">
                                ${ $j.number(itemRegla.Valor, 2, ',', '.')}
                               </td>`;
            }
        });
        htmlDetail += '</tr>';
    });
    $j("#tblResultDetailLiq").empty();
    $j("#tblResultDetailLiq").html(htmlDetail);
    $j("#tabs-2").css("height", "auto");
}

function Progress() {
    var val = progressbarControl.progressbar("value") || 0;
    progressbarControl.progressbar("value", val + 1);
}

function ValidarDatosLiqPropietario() {
    var ok = true;
    // Los conceptos de hotel, ya deben de estar liquidados
    if (!okLiqHotel) {
        $j("#textmodalLiqInfo").text("Se debe de liquidar los conceptos de Hotel primero.");
        $j("#modalLiqInfo").dialog('open');
        ok = false;
    }
    return ok;
}

function ValidarDatosLiqHotel() {
    var ok = true;
    // Las variables de hotel, deben de tener valor
    if (!okLiqHotel) {
        $j("#textmodalLiqInfo").text("Se debe de liquidar los conceptos de Hotel primero.");
        $j("#modalLiqInfo").dialog('open');
        ok = false;
    }
    return ok;
}

function LiquidarTodos() {
    ClearControl();
    $j("#GuardarSel,#GuardarAll").hide();
    if (ValidarDatosLiqPropietario()) {

        sourceOwner = [];
        sourceliquidacion = [];
        reglasOrdenadas = [];

        $j("#tblResultColumnsLiq,#tblResultDetailLiq").empty();
        sourceOwner = sourceOwnerTmp.slice(0);
        ObtenerFuentes();

        $j("#GuardarAll").show();
        $j("#GuardarSel").hide();
    }
}

function LiquidarSeleccionados() {
    ClearControl();
    $j("#GuardarSel,#GuardarAll").hide();
    if (ValidarDatosLiqPropietario()) {

        sourceOwner = [];
        sourceliquidacion = [];
        reglasOrdenadas = [];

        $j("#tblResultColumnsLiq,#tblResultDetailLiq").empty();
        var ownerSelec = $j("input[name='cbOwner']:checked");
        if (ownerSelec.length > 0) {
            $j.each(ownerSelec, function () {
                var idOwner = $j(this).val().split('|')[1];
                var idSuite = $j(this).val().split('|')[0];
                sourceOwner.push(sourceOwnerTmp.filter(o => o.IdOwner == idOwner && o.IdSuite == idSuite)[0]);
            });
            ObtenerFuentes();
            $j("#GuardarAll").hide();
            $j("#GuardarSel").show();
        }
    }
}

function GuardarLiquidarHotel() {
    $j("#modalLiq").dialog("open");
    ClearControl();

    $j.ajax({
        method: "POST",
        //async: false,
        url: "../../handlers/HandlerLiquidacion.ashx",
        data: { ActionType: 3, LiquidacionHotel: JSON.stringify(sourceliquidacionHotel), IdHotel: idHotelSel, YYYY: yyyy, MM: mm }
    })
        .done(function (res) {
            if (!res.OK) {
                $j("#lbltextoError").text("Error en el guardado");
                $j("#divError").show();
                console.log(res.ERROR);
            } else {
                CargarPropietarios(); // Volvemos  a llamar este metodo para cargar las variables liquidadas
                $j("#lbltextoExito").text("Liquidación guardada");
                $j("#divExito").show();
                okLiqHotel = true;
            }
            $j("#modalLiq").dialog("close");
        });
}

function GuardarLiquidarProp(isSel) {
    ClearControl();
    progressbarControl.progressbar("value", false);
    $j("#modalLiq").dialog("open");

    $j.ajax({
        method: "POST",
        //async: false,
        url: "../../handlers/HandlerLiquidacion.ashx",
        data: { ActionType: 4, LiquidacionProp: JSON.stringify(sourceliquidacion), IdHotel: idHotelSel, YYYY: yyyy, MM: mm, IsSel: isSel }
    })
        .done(function (res) {
            if (!res.OK) {
                $j("#lbltextoError").text("Error en el guardado");
                $j("#divError").show();
                console.log(res.ERROR);
            } else {
                $j("#lbltextoExito").text("Liquidación guardada");
                $j("#divExito").show();
            }
            $j("#modalLiq").dialog("close");
        });
}

function DeleteLiq() {
    $j("#textModalLiq").text("Eliminando liquidación");
    $j("#modalLiq").dialog("open");

    $j.ajax({
        method: "POST",
        //async: false,
        url: "../../handlers/HandlerLiquidacion.ashx",
        data: {
            ActionType: 5, IdHotel: jQuery("select[id$='ddlHotelEliminarLiquidacion']").val(),
            YYYY: jQuery("#txtFechaDeleteLiq").val(), MM: jQuery("select[id$='ddlMesDesde']").val()
        }
    })
        .done(function (res) {
            if (!res.OK) {
                $j("#lbltextoError").text("Error en el guardado");
                $j("#divError").show();
                console.log(res.ERROR);
            } else {
                $j("#lbltextoExito").text("Liquidación guardada");
                $j("#divExito").show();
            }
            $j("#modalLiq").dialog("close");
        });
}

function ObtenerFuentes() {
    ClearControl();
    $j("#textModalLiq").text("Obteniendo reglas");
    $j("#modalLiq").dialog("open");

    $j.ajax({
        method: "POST",
        //async: false,
        url: "../../handlers/HandlerLiquidacion.ashx",
        data: { ActionType: 1, IdHotel: jQuery("select[id$='ddlHotel']").val(), YYYY: -1, MM: -1 }
    })
        .done(function (res) {

            $j("#textModalLiq").text("Calculando reglas");

            idHotelSel = parseInt(jQuery("select[id$='ddlHotel']").val());
            sourceReglas = res.Regla;

            // Obtenemos el orden de las reglas
            ObtenerOrdenEjecucionReglas();

            $j("#textModalLiq").text("Ejecutando reglas");
            progressbarControl.progressbar("value", 1);
            Progress();

            // Ejecutamos todas las reglas
            EjecutarReglas();

            // Mostramos liquidacion
            MostrarLiquidacion();

            $j("#modalLiq").dialog("close");
        });
}

function CargarPropietarios() {
    ClearControl();
    $j.ajax({
        method: "POST",
        async: false,
        url: "../../handlers/HandlerLiquidacion.ashx",
        data: { ActionType: 0, IdHotel: jQuery("select[id$='ddlHotel']").val(), MM: jQuery("#ddlMes").val(), YYYY: jQuery("#txtYYYY").val() }
    })
        .done(function (res) {
            $j("#tblResultColumnsLiq,#tblResultDetailLiq").empty();
            sourceOwner = [];
            sourceOwnerTmp = [];

            sourceOwner = res.Owners;
            sourceOwnerTmp = res.Owners;
            sourceReglasHotel = res.Regla;
            sourceReglasHotelTmp = res.Regla;
            sourceVValor = res.VariableValor;

            okLiqHotel = res.OKLiquidacionHotel;
            numOwners = res.Owners.length;

            yyyy = parseInt(jQuery("#txtYYYY").val());
            mm = parseInt(jQuery("#ddlMes").val());

            $j("#divLiqHotel").hide();
            $j("#ownerDetail").empty();
            $j("#hotelConceptos").empty();

            var htmlTmp = '';
            res.Owners.forEach(function (item, index) {
                htmlTmp += `<tr>
                    <td class="detailCelda">${index + 1}</td>
                    <td class="detailName">${item.FullNombre}</td>
                    <td class="detailCelda">${item.NumSuite}</td>                            
                    <td class="detailCelda">${item.NumEscritura}</td>
                    <td class="detailCelda"><input name="cbOwner" class="selOwner" type="checkbox" value="${item.IdSuite}|${item.IdOwner}" /></td>
                </tr>`;
            });
            $j("#ownerDetail").html(htmlTmp);

            var htmlHotelTmp = '';
            res.Regla.forEach(function (item, index) {
                htmlHotelTmp += `<tr>
                                <td class="detailCelda">${index + 1}</td>
                                <td class="detailName">${item.NombreConcepto}</td>
                            </tr>`;
            });
            $j("#hotelConceptos").html(htmlHotelTmp);


            progressbarControl.progressbar({
                value: false,
                max: numOwners
            });

        });
}