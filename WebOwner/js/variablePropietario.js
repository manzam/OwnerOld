var $j = jQuery.noConflict();

var dataPropietario = {};
var data = {};
var dataSuite = {};
var dataVariable = [];
var textError = '';
var isDataProp = false;

$j(document).ready(function() {

    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargasIniciales);

    CargasIniciales(null, null);

    function CargasIniciales(sender, args) {
        $j("[id$='modalSuit']").dialog({
            width: 1200,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: "auto",
            open: function(event, ui) {
                $j("[id$='btnObtenerVariables']").click();
            },
            buttons: {
                "Aceptar": function() {

                    if (getDataNuevSuite())
                        $j(this).dialog("close");

                },
                "Cancelar": function() {
                    $j("[id$='txtValoresVariables']").val('');
                    $j("[id$='RequiredFieldValidator4']").hide()
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');

        $j("#modalOk").dialog({
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: "auto",
            title: "Eliminar",
            buttons: {
                "Aceptar": function() {
                    $j(this).dialog('close');
                    $j('#' + $j('#ctl00_idCtrl').val()).click();
                },
                "Cancelar": function() {
                    $j('#idCtrl').val('');
                    $j(this).dialog('close');
                }
            }
        });

        $j("#modalBuscadorPropietario").dialog({
            width: 1000,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: "auto",
            buttons: {
                "Aceptar": function() {
                    $j("[id$='btnAceptar']").click();
                    $j(this).dialog("close");
                },
                "Cancelar": function() {
                    $j("[id$='btnCancelar']").click();
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');

        function getDataNuevSuite() {
            var isOK = true;
            dataSuite = {};
            dataSuite.Id = new Date().getTime();
            dataSuite.NomHotel = $j("[id$=ddlHotel]  option:selected").text();
            var nomSuite = $j("[id$=ddlSuit]  option:selected").text().split(' ');
            dataSuite.NomEscritura = nomSuite[1];
            dataSuite.NomSuite = nomSuite[4];

            dataSuite.IdSuite = $j("[id$=ddlSuit]").val();
            dataSuite.IdBanco = $j("[id$=ddlBanco]").val();
            dataSuite.TitularBanco = $j("[id$=txtTitular]").val();
            dataSuite.TipoCuenta = $j("[id$=ddlTipoCuenta]").val();
            dataSuite.NumCuenta = $j("[id$=txtNumCuenta]").val();
            dataSuite.NumEstadias = $j("[id$=txtNumEstadias]").val();
            dataSuite.ListDataVariable = [];

            if (getValorVariable('valorVariables')) {
                dataSuite.ListDataVariable = dataVariable;
                dataPropietario.ListaDataSuite.push(dataSuite);
                AddNuevosSuites();
            } else {
                alert('fail');
                isOK = false;
            }
            return isOK;
        }

    }

});

function mostrarErrorSumCoeficientes(dataError) {
    alert(dataError);
}

function AddNuevosSuites() {

    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerVariable.ashx",
        data: { Action: 2, data: JSON.stringify(dataPropietario) }
    })
    .done(function(res) {
        if (!res.OK) {
            $j("#lbltextoError").text("Error en el guardado");
            $j("#divError").show();
            mostrarErrorSumCoeficientes(res.ErrorDescripcion);
            console.log(res.ERROR);
        } else {
            $j(".nuevas").remove();
            var html = '';
            for (var i = 0; i < dataPropietario.ListaDataSuite.length; i++) {
                html += '<tr class="nuevas">';
                //html += '<td align="center"> <input type="image" src="../../img/23.png" style="height:30px;width:30px;border-width:0px;"> </td >';
                //html += '<td align="center"> <input type="image" src="../../img/126.png" onclick="quitar(' + dataPropietario.ListaDataSuite[i].Id + ');" style="height:30px;width:30px;border-width:0px;"> </td>';

                html += '<td align="center"> - </td >';
                html += '<td align="center"> - </td>';
                html += '<td>' + dataPropietario.ListaDataSuite[i].NomHotel + '</td>';
                html += '<td align="center">' + dataPropietario.ListaDataSuite[i].NomSuite + '</td>';
                html += '<td align="center">' + dataPropietario.ListaDataSuite[i].NomEscritura + '</td>';
                html += '<td align="center">' + dataPropietario.ListaDataSuite[i].NumEstadias + '</td>';
                //html += '<td align="center"> <img src="../../img/126.png" /> </td>';
                html += '</tr >';
            }
            $j("[id$=gvwSuits] tbody").append(html);
        }
    });
}

function quitar(item) {
    alert(item);
}

function GuardarVariables() {

    $j.ajax({
        method: "POST",
        //async: false,
        url: "../../handlers/HandlerVariable.ashx",
        data: { Action: 0, data: JSON.stringify(dataSuite) }
    }).done(function(res) {
        $j("[id$=lbltextoError]").text('');
        $j("[id$=divError]").hide();
        $j("[id$=lbltextoExito]").html('');
        $j("[id$=divExito]").hide();

        if (!res.OK) {
            $j("[id$=lbltextoError]").text(res.ERROR);
            $j("[id$=divError]").show();
            mostrarErrorSumCoeficientes(res.ErrorDescripcion);
            console.log(res.ERROR);
        } else {
            alert('Guardado con exito.');
            $j("[id$=lbltextoExito]").html(res.ERROR);
            $j("[id$=divExito]").show();
        }
    });
}

function getDataPropietario() {
    //dataPropietario = {};
    dataPropietario.IdUsuario = $j("[id$=hiddenIdUsuario]").val();
    dataPropietario.IdPropietario = $j("[id$=HiddenIdPropietario]").val();
    dataPropietario.TipoPersona = $j("[id$=ddlTipoPersona]").val();
    dataPropietario.Activo = $j("[id$=chActivo]").is(":checked");
    dataPropietario.Retencion = $j("[id$=cbEsRetenedor]").is(":checked");
    dataPropietario.Nombre1 = $j("[id$=txtNombre]").val();
    dataPropietario.Nombre2 = $j("[id$=xtNombreSegundo]").val();
    dataPropietario.Apellido1 = $j("[id$=txtApellidoPrimero]").val();
    dataPropietario.Apellido2 = $j("[id$=txtApellidoSegundo]").val();
    dataPropietario.NumIdentificacion = $j("[id$=txtNumIdentificacion]").val();
    dataPropietario.TipoDoc = $j("[id$=ddlTipoDocumento]").val();
    dataPropietario.IdDepto = $j("[id$=ddlDepto]").val();
    dataPropietario.IdCiudad = $j("[id$=ddlCiudad]").val();
    dataPropietario.Direccion = $j("[id$=txtDireccion]").val();
    dataPropietario.Correo1 = $j("[id$=txtCorreo]").val();
    dataPropietario.Correo2 = $j("[id$=txtCorreo2]").val();
    dataPropietario.Correo3 = $j("[id$=txtCorreo3]").val();
    dataPropietario.Tel1 = $j("[id$=txtTel1]").val();
    dataPropietario.Tel2 = $j("[id$=txtTel2]").val();
    dataPropietario.NomContacto = $j("[id$=txtNombreContacto]").val();
    dataPropietario.TelContacto = $j("[id$=txtTelContacto]").val();
    dataPropietario.CorreoContacto = $j("[id$=txtCorreoContacto]").val();

    if (dataPropietario.ListaDataSuite == undefined) {
        dataPropietario.ListaDataSuite = [];
    }
}

function GuardarPropietario() {

    $j("[id$=lbltextoError]").text('');
    $j("[id$=divError]").hide();
    $j("[id$=lbltextoExito]").html('');
    $j("[id$=divExito]").hide();

    getDataPropietario();

    if (validarCampos(dataPropietario)) {
        $j.ajax({
            method: "POST",
            url: "../../handlers/HandlerVariable.ashx",
            data: { Action: 1, data: JSON.stringify(dataPropietario) }
        })
        .done(function(res) {
            if (!res.OK) {
                $j("#lbltextoError").text("Error en el guardado");
                $j("#divError").show();
                console.log(res.ERROR);
            } else {
                $j("[id$=lbltextoExito]").html('Guardado con exito.');
                $j("[id$=divExito]").show();
                setTimeout(function() {
                    $j("[id$=btnVerTodos]").click();
                }, 2000, "JavaScript");

            }
        });
    } else {
        $j("[id$=lbltextoError]").html('Campos obligatorios: ' + textError);
        $j("[id$=divError]").show();
    }
}

function validarCampos(data) {
    var isOK = true;
    isDataProp = true;
    textError = '';
    var tipo = $j("[id$=ddlTipoPersona]").val();

    if (dataPropietario.Nombre1.trim() == '') {
        isOK = false;
        isDataProp = false;
        textError += 'Primer Nombre - '
    }
    if (tipo == 'NATURAL') {
        if (dataPropietario.Apellido1.trim() == '') {
            isOK = false;
            isDataProp = false;
            textError += 'Primer Apellido - '
        }
    }
    if (dataPropietario.NumIdentificacion.trim() == '') {
        isOK = false;
        isDataProp = false;
        textError += 'N° identificación - '
    }
    if ($j("[id$=ddlTipoDocumento]").val().trim() == '0') {
        isOK = false;
        isDataProp = false;
        textError += 'Tipo de Documento - '
    }
    if (dataPropietario.Correo1.trim() == '') {
        isOK = false;
        isDataProp = false;
        textError += 'Correo 1 - '
    }
    //if (dataPropietario.Correo2.trim() == '') {
    //    isOK = false;
    //    isDataProp = false;
    //    textError += 'Correo 2 - '
    //}
    //if (dataPropietario.CorreoContacto.trim() == '') {
    //    isOK = false;
    //    isDataProp = false;
    //    textError += 'Correo Contacto - '
    //}
    return isOK;
}

function getValorVariable(nomClass) {
    var isOK = true;
    dataVariable = [];
    $j("." + nomClass).each(function(index) {
        var id = $j(this).attr('IdValorVariableSuit');
        var valor = $j(this).val();
        var esCond = $j(this).attr('EsValidacion');
        var valorMax = $j(this).attr('ValMax');
        var valorActual = $j(this).attr('valor');
        var nombre = $j(this).attr('NomVariable');
        var idVariable = $j(this).attr('IdVariable');

        //if (esCond == 'true') {
        //    valorActual = parseFloat(valorActual);
        //    valor = parseFloat(valor);
        //    valorMax = parseFloat(valorMax);

        //    if ((valorActual + valor) > valorMax) {
        //        alert('La variable : ' + nombre + ' debe de sumar: ' + valorMax);
        //        isOK = false;
        //    }

        //    if ((valorActual + valor) < valorMax) {
        //        alert('No te olvides que la variable : ' + nombre + ' debe de sumar: ' + valorMax);
        //    }
        //}

        var oVaraible = new Object();
        oVaraible.IdValorVariableSuit = id;
        oVaraible.Valor = valor;
        oVaraible.IdVariable = idVariable;
        oVaraible.EsValidacion = esCond;
        dataVariable.push(oVaraible);

        console.log(id + ' ' + valor);
    });
    return isOK;
}

function AgregarSuite() {

    $j("[id$=lbltextoError]").html('');
    $j("[id$=divError]").hide();
    getDataPropietario();

    if (validarCampos(dataPropietario)) {

        var ID = $j("[id$=HiddenIdPropietario]").val();
        if (isDataProp || ID != '-1') {
            $j("[id$='modalSuit']").dialog("open");
            $j("[id$='txtDescripcionSuit']").val("");
            $j("[id$='txtNumEstadias']").val("");
            $j("[id$='txtTitular']").val("");
            $j("[id$='txtNumCuenta']").val("");
        } else {
            alert('Debe de llenar los datos del propietario');
        }
    } else {
        $j("[id$=lbltextoError]").html('Campos obligatorios: ' + textError);
        $j("[id$=divError]").show();
    }
}


function SaveData() {

    if (getValorVariable('valorVariablesUpdate')) {
        dataSuite.IdBanco = $j("[id$=ddlBancoDetalleUpdate]").val();
        dataSuite.TitularBanco = $j("[id$=txtTitularDetalleUpdate]").val();
        dataSuite.TipoCuenta = $j("[id$=ddlTipoCuentaDetalleUpdate]").val();
        dataSuite.NumCuenta = $j("[id$=txtCuentaDetalleUpdate]").val();
        dataSuite.NumEstadias = $j("[id$=txtNumEstadiasUpdate]").val();

        dataSuite.IdSuitPropietarioSeleccionado = $j("[id$=hiddenIdSuitPropietarioSeleccionado]").val();
        dataSuite.IdSuite = $j("[id$=hiddenIdSuitSeleccionado]").val();
        dataSuite.IdPropietarioSeleccionado = $j("[id$=HiddenIdPropietario]").val();
        dataSuite.IdUsuario = $j("[id$=hiddenIdUsuario]").val();

        dataSuite.ListDataVariable = dataVariable;

        GuardarVariables();
    }
}


