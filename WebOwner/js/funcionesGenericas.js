function sleep(segundos) {
    segundos = segundos * 1000;
    var date = new Date();
    var curDate = null;
    do { curDate = new Date(); }
    while (curDate - date < segundos);
}

function ventanaOk(ctrl) {
    $j('#ctl00_idCtrl').val($j("#" + ctrl.id).parent().children().eq(1).attr("id"));
    $j("#textoModal").text(ctrl.title);
    $j("#modalOk").dialog('open');
}

function modalOkObservacion(ctrl) {
    $j('#ctl00_idCtrl').val($j("#" + ctrl.id).parent().children().eq(1).attr("id"));
    $j("#textoModalObservacion").text(ctrl.title);
    $j("#modalOkObservacion").dialog('open');
}

function checkAll(idCtrlThis, idCtrlPadre) {

    var ch = idCtrlThis.checked;

    $j('#' + idCtrlPadre + ' input:checkbox').each(function() {
        if (ch) {
            $j(this).attr('checked', 'checked');
            $j(this).prop('checked', true);
        }
        else {
            $j(this).attr('checked', '');
            $j(this).prop('checked', false);
        }
    });
}

function valCheckAll(idCtrlPadre, idCtrl) {
    var conTotal = ($j('#' + idCtrlPadre + ' input:checkbox').length) - 1;
    var conChecked = ($j('#' + idCtrlPadre + ' input:checked').length);

    if (conTotal = conChecked) {
        $j("#idCtrl").attr('checked', 'checked');
        $j("#idCtrl").prop('checked', true);
    }
    else {
        $j("#idCtrl").attr('checked', '');
        $j("#idCtrl").prop('checked', false);
    }
}