var $j = jQuery.noConflict();

var dataPropietario = {};
var data = {};

$j(document).ready(function() {

    CargasIniciales(null, null);

    function CargasIniciales(sender, args) {

        $j("#btnGuardar,#btnVerTodos,#btnAgregarSuit,#dataPropietario,#editSuite,#nuevaSuite,#updateSuite,#btnUpdateVariables,#ErrorTipo1,#ErrorTipo1Top,#ErrorTipo2,#ErrorTipo2Top").hide();

        // Hotel
        $j.ajax({
            method: "POST",
            url: "../../handlers/HandlerPropietario.ashx",
            data: { Action: 4 }
        }).done(function (listDeptos) {
            $j.each(listDeptos, function (i, item) {
                $j('#selectHotel').append($j('<option>', {
                    value: item.IdHotel,
                    text: item.Name
                }));
            });
            selectHotel_onchange();
        });

        // Depto
        $j.ajax({
            method: "POST",
            url: "../../handlers/HandlerPropietario.ashx",
            data: { Action: 2 }
        }).done(function (listDeptos) {            
            $j.each(listDeptos, function (i, item) {
                $j('#selectDepto').append($j('<option>', {
                    value: item.IdDepto,
                    text: item.Name
                }));
            });
            selectDepto_onchange();
        });

        // Banco
        $j.ajax({
            method: "POST",
            url: "../../handlers/HandlerPropietario.ashx",
            data: { Action: 7 }
        }).done(function (listBanco) {
            $j.each(listBanco, function (i, item) {
                $j('#selectBank').append($j('<option>', {
                    value: item.IdBanco,
                    text: item.Name
                }));
            });
        });

        $j("#modalBuscadorPropietario").dialog({
            width: 1000,
            autoOpen: false,
            resizable: false,
            show: "slow",
            modal: true,
            height: "auto",
            buttons: {
                "Aceptar": function () {
                    $j("[id$='btnAceptar']").click();
                    $j(this).dialog("close");
                },
                "Cancelar": function () {
                    $j("[id$='btnCancelar']").click();
                    $j(this).dialog("close");
                }
            }
        }).parent().appendTo($j("form:first")).css('z-index', '1005');
    }

});

function GuardarPropietario() {
    if (isValid()) {
        var isValidVariables = validarVariables();
        if (isValidVariables) {

            dataPropietario = {};
            getDataPropietario();

            $j.ajax({
                method: "POST",
                url: "../../handlers/HandlerPropietario.ashx",
                data: { Action: 0, data: JSON.stringify(dataPropietario) }
            }).done(function (response) {
                if (response.Ok) {
                    alert(response.Succes);
                    $j("#btnVerTodos").click();
                }                    
                else {
                    alert(response.Error);
                    console.log(response.ErrorExeption);
                }
            });
        }
    }
}

function getDataPropietario() {
    // Propietario
    dataPropietario = {};
    dataPropietario.IdPropietario = $j("#idPropietario").val();
    dataPropietario.PrimeroNombre = $j("[id$=txtNombre]").val();
    dataPropietario.SegundoNombre = $j("[id$=xtNombreSegundo]").val();
    dataPropietario.PrimerApellido = $j("[id$=txtApellidoPrimero]").val();
    dataPropietario.SegundoApellido = $j("[id$=txtApellidoSegundo]").val();
    dataPropietario.TipoPersona = $j("[id$=ddlTipoPersona]").val();
    dataPropietario.TipoDocumento = $j("[id$=ddlTipoDocumento]").val();
    dataPropietario.NumIdentificacion = $j("[id$=txtNumIdentificacion]").val();
    dataPropietario.Activo = $j("[id$=chActivo]").is(":checked");
    dataPropietario.IdCiudad = $j("#selectCity").val();
    dataPropietario.Correo = $j("[id$=txtCorreo]").val();
    dataPropietario.Correo2 = $j("[id$=txtCorreo2]").val();
    dataPropietario.Correo3 = $j("[id$=txtCorreo3]").val();
    dataPropietario.Direccion = $j("[id$=txtDireccion]").val();
    dataPropietario.Telefono1 = $j("[id$=txtTel1]").val();
    dataPropietario.Telefono2 = $j("[id$=txtTel2]").val();    
    dataPropietario.NombreContacto = $j("[id$=txtNombreContacto]").val();
    dataPropietario.TelContacto = $j("[id$=txtTelContacto]").val();
    dataPropietario.CorreoContacto = $j("[id$=txtCorreoContacto]").val();
    dataPropietario.EsRetenedor = $j("[id$=cbEsRetenedor]").is(":checked");

    if ($j('#editSuite').is(':visible')) {
        // Suite
        dataPropietario.IdSuit = $j("#selectSuite").val();
        dataPropietario.IdBanco = $j("#selectBank").val();
        dataPropietario.NumCuenta = $j("#txtNumCuenta").val();
        dataPropietario.NumEstadias = $j("#txtEstadias").val();
        dataPropietario.TipoCuenta = $j("#selectTipoCuenta").val();
        dataPropietario.Titular = $j("#txtTitularCuenta").val();
        dataPropietario.Activo = true;
        // Variables
        dataPropietario.ListaVariables = [];
        dataPropietario.ListaVariables = getDataVariables();
    }
}

function getDataVariables() {
    var listaVariables = [];
    $j(".vars").each(function () {
        var value = ($j(this).val().trim() == '') ? 0 : $j(this).val().trim();
        listaVariables.push({ IdVariable: $j(this).attr('id'), Valor: value, IdValorVariableSuit: $j(this).attr('idVVS'), IdSuit: $j("#selectSuite").val(), IdPropietario: $j("#IdPropietario").val() });
    });
    return listaVariables;
}

function setDataPropietario(dataPropietario) {
    // Propietario
    $j("[id$=txtNombre]").val(dataPropietario.PrimeroNombre);
    $j("[id$=xtNombreSegundo]").val(dataPropietario.SegundoNombre);
    $j("[id$=txtApellidoPrimero]").val(dataPropietario.PrimerApellido);
    $j("[id$=txtApellidoSegundo]").val(dataPropietario.SegundoApellido);
    $j("[id$=ddlTipoPersona]").val(dataPropietario.TipoPersona);
    $j("[id$=ddlTipoDocumento]").val(dataPropietario.TipoDocumento);
    $j("[id$=txtNumIdentificacion]").val(dataPropietario.NumIdentificacion);
    $j("[id$=chActivo]").prop("checked", dataPropietario.Activo);
    $j("#selectCity").val(dataPropietario.IdCiudad);
    $j("[id$=txtCorreo]").val(dataPropietario.Correo);
    $j("[id$=txtCorreo2]").val(dataPropietario.Correo2);
    $j("[id$=txtCorreo3]").val(dataPropietario.Correo3);
    $j("[id$=txtDireccion]").val(dataPropietario.Direccion);
    $j("[id$=txtTel1]").val(dataPropietario.Telefono1);
    $j("[id$=txtTel2]").val(dataPropietario.Telefono2);
    $j("[id$=txtNombreContacto]").val(dataPropietario.NombreContacto);
    $j("[id$=txtTelContacto]").val(dataPropietario.TelContacto);
    $j("[id$=txtCorreoContacto]").val(dataPropietario.CorreoContacto);
    $j("[id$=cbEsRetenedor]").prop("checked", dataPropietario.EsRetenedor);
    // Suite
    $j("#tblListaSuite").empty();
    var htmlSuites = '';
    dataPropietario.ListaSuite.each(function (item) {
        htmlSuites += '<tr>';
        htmlSuites += `<td style="text-align: center;">
                            <img src="../../img/23.png" style="height: 30px; width: 30px;" onclick="SetDetalleSuite(${item.IdSuitPropietario});" />
                        </td>
                        <td style="text-align: center;">
                            <img src="../../img/126.png" style="height: 30px; width: 30px;" onclick="" />
                        </td>
                        <td>${item.NombreHotel}</td>
                        <td>${item.NumSuit}</td>
                        <td>${item.NumEscritura}</td>
                        <td style="text-align: center;">
                            <input type="checkbox" onclick="ActivarSuite(${item.IdSuitPropietario})" checked="${(item.Activo) ? 'checked' : ''}" />
                        </td>`;
        htmlSuites += '</tr>';
    });
    
    $j("#tblListaSuite").html(htmlSuites);

}

function SetDetalleSuite(idSuitPropietario) {
    $j("#nuevaSuite").hide();
    $j("#updateSuite").show();
    $j("#idSuitePropietario").val(idSuitPropietario);

    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 10, data: idSuitPropietario }
    }).done(function (dataSuite) {
        $j("#editSuite,#btnUpdateVariables").show();
        
        $j("#nombreHotel").text(dataSuite.NombreHotel);
        $j("#nombreSuite").text(dataSuite.NumSuit);
        $j("#selectBank").val(dataSuite.IdBanco);
        $j("#txtNumCuenta").val(dataSuite.NumCuenta);
        $j("#txtEstadias").val(dataSuite.NumEstadias);
        $j("#selectTipoCuenta").val(dataSuite.TipoCuenta);
        $j("#txtTitularCuenta").val(dataSuite.Titular);
        // Variables
        loadVariables(dataSuite.ListaVariables);
    });

}

function UpdateVariables() {
    dataPropietario = {};
    dataPropietario.IdSuitPropietario = $j("#idSuitePropietario").val();
    dataPropietario.IdBanco = $j("#selectBank").val();
    dataPropietario.Titular = $j("#txtTitularCuenta").val();
    dataPropietario.TipoCuenta = $j("#selectTipoCuenta").val();
    dataPropietario.NumCuenta = $j("#txtNumCuenta").val();
    dataPropietario.NumEstadias = $j("#txtEstadias").val();

    dataPropietario.ListaVariables = getDataVariables();

    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 11, data: JSON.stringify(dataPropietario) }
    }).done(function (res) {
        $j("#editSuite").hide();
        $j("#idSuitePropietario").val('-1');
        
        alert('Datos actualizados');
    });
}

function ActivarSuite(idSuite) {
    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 9, data: idSuite }
    }).done(function (isActive) {
        alert('Suite: ' + (isActive ? 'Activa' : 'Inactiva'));
    });
}

function selectDepto_onchange() {
    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 3, data: $j("#selectDepto").val() }
    }).done(function (listCity) {
        $j('#selectCity').empty();
        $j.each(listCity, function (i, item) {
            $j('#selectCity').append($j('<option>', {
                value: item.IdCity,
                text: item.Name
            }));
        });
    });
}

function selectHotel_onchange() {
    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 5, data: $j("#selectHotel").val() }
    }).done(function (listSuite) {
        $j('#selectSuite').empty();
        $j.each(listSuite, function (i, item) {
            $j('#selectSuite').append($j('<option>', {
                value: item.IdSuite,
                text: `Escritura: ${item.NumSuite} N° Suite: ${item.NumEsc}`
            }));
        });
        selectSuite_onchange();
    });
}

function loadVariables(listaVariables) {
    $j('#tblVariables').empty();
    $j.each(listaVariables, function (i, item) {
        $j('#tblVariables').append(`<tr><td class='textoTabla'>${item.Nombre}</td><td><input type="number" class="vars" id="${item.IdVariable}" idVVS="${item.IdValorVariableSuit}" value="${item.Valor}" /></td></tr>`);
    });
}

function selectSuite_onchange() {
    var data = { IdHotel: $j("#selectHotel").val(), IdSuit: $j("#selectSuite").val() };
    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 6, data: JSON.stringify(data) }
    }).done(function (listVars) {
        $j("#txtDescripcionSuit").val(listVars.NombreSuit);
        loadVariables(listVars.ListaVariables);
    });
}

function loadOwner(IdOwner) {
    $j.ajax({
        method: "POST",
        url: "../../handlers/HandlerPropietario.ashx",
        data: { Action: 8, data: IdOwner }
    }).done(function (dataOwner) {
        $j("#GrillaPropietario,#btnNuevo").hide();
        $j("#dataPropietario,#listaSuite,#btnGuardar,#btnVerTodos,#btnAgregarSuit").show();
        $j("#idPropietario").val(dataOwner.IdPropietario);
        setDataPropietario(dataOwner);
    });
}

function isValid() {
    var isValid = true;
    $j(".requerid").each(function () {
        if ($j(this).attr('type') == 'text') {
            if ($j(this).val().trim() == '') {

                $j(this).addClass("errorRequerid");
                isValid = false;

                if ($j(this).hasClass("dataSuite")) {
                    if (!$j('#editSuite').is(':visible')) {
                        isValid = true;
                    }
                }                
            }
            else
                $j(this).removeClass("errorRequerid");
        }
    });
    return isValid;
}

function validarVariables() {
    $j("#ErrorTipo1,#ErrorTipo1Top,#ErrorTipo2,#ErrorTipo2Top").hide();
    $j("#tblErrorTipo1").empty();
    
    var listaVariables = getDataVariables();
    var isValid = true;

    $j.ajax({
        async: false,
        method: "POST",
        url: "../../handlers/HandlerVariable.ashx",
        data: { Action: 0, data: JSON.stringify(listaVariables) }
    }).done(function (response) {
        if (!response.Ok) {
            isValid = false;
            if (response.TipoValidacion == 1) {
                var htmlError = '';
                $j.each(response.Lista, function (i, item) {
                    htmlError += `<tr><td>${item.Nombre}</td><td>${item.NumIdentificacion}</td><td>${item.Valor}</td>></tr>`;
                });
                $j("#tblErrorTipo1").html(htmlError);
                $j("#ErrorTipo1,#ErrorTipo1Top").show();
            }
            if (response.TipoValidacion == 3) {
                var htmlError = '';
                $j.each(response.Lista, function (i, item) {
                    htmlError += `<tr><td>${item.Nombre}</td><td>${item.NumIdentificacion}</td><td>${item.Valor}</td><td>${item.ValorSuite}</td</tr>`;
                });
                $j("#tblErrorTipo2").html(htmlError);
                $j("#ErrorTipo2,#ErrorTipo2Top").show();
            }
        }
    });

    return isValid;
}

function AgregarSuite() {
    $j("#editSuite,#nuevaSuite").show();
    $j("#updateSuite").hide();
}

function verTodos() {
    $j("#idPropietario,#idSuitePropietario").val("-1");
    $j("#GrillaPropietario,#btnNuevo").show();
    $j("#dataPropietario,#editSuite,#btnGuardar,#btnVerTodos").hide();
}

function nuevo() {
    $j("#dataPropietario input").each(function () {
        if ($j(this).attr('type') == 'text') {
            $j(this).val('');
        }
    });
    $j(".vars").each(function () {
        $j(this).val('0');
    });
    $j("#idPropietario,#idSuitePropietario").val("-1");
    $j("[id$=chActivo]").prop("checked", true);
    $j("#GrillaPropietario,#updateSuite,#listaSuite,#btnNuevo,#btnAgregarSuit").hide();
    $j("#dataPropietario,#editSuite,#nuevaSuite,#btnGuardar,#btnVerTodos").show();
}


