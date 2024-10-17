define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_AuditTrail();
        $("#btnAuditTrail_Buscar").click(function () {
            fn_AuditTrail();
        });
        $("#btnAuditTrail_Limpiar").click(function () {
            $("#txtAuditTrail_Usuario").val("");
            $("#txtAuditTrail_Accion").prop("selectedIndex", 0);
            $("#txtAuditTrail_Fecha1").val("");
            $("#txtAuditTrail_Fecha2").val("");
            fn_AuditTrail();
        });
        $("#pgdAudit").paginado({
            Tabla: $("#tblAuditTrail"),
            Version: 2,
            Funcion: fn_AuditTrail
        });
    });
    function fn_AuditTrail(Pagina) {
        var opc = 1;
        var usuario = $("#txtAuditTrail_Usuario").val();
        var accion = $("#txtAuditTrail_Accion option:selected").text();
        var fecha1 = $("#txtAuditTrail_Fecha1").val();
        var fecha2 = $("#txtAuditTrail_Fecha2").val();
        if (fecha1 == "") { fecha1 = null; }
        if (fecha2 == "") { fecha2 = null; }
        if (usuario == "") { usuario = null; }
        if (accion == -1 || accion == "Select" || accion == "Seleccione") { accion = null; }
        if (accion == "Update" || accion == "Modificación") { accion = 'M'; }
        if (accion == "Insert" || accion == "Inserción") { accion = 'I'; }
        if (accion == "Delete" || accion == "Eliminación") { accion = 'E'; }
        if (fecha1 !== null && fecha2 === null) {
            opc = 2;
        } else if (fecha1 === null && fecha2 !== null) {
            opc = 1;
        } else {
            opc = 3;
        }
        var datos = { Fecha1: fecha1, Fecha2: fecha2, Usuario: usuario, Accion: accion, index: Pagina };
        //if (opc === 1) {
        //    Datos = { Fecha1: fecha2, Fecha2: fecha2, Usuario: usuario, Accion: accion, index: Pagina }
        //} else if (opc === 3) {
        //    Datos = { Fecha1: fecha1, Fecha2: fecha2, Usuario: usuario, Accion: accion, index: Pagina };
        //} else {
        //    Datos = { Fecha1: fecha1, Fecha2: fecha1, Usuario: usuario, Accion: accion, index: Pagina };
        //}
        $.mostrarInfo({
            URLindex: "/AuditTrail/get_TotalPag_AuditTrail",
            URLdatos: "/AuditTrail/get_audittrail_table",
            Datos: datos,
            Version: 2,
            Tabla: $("#tblAuditTrail"),
            Paginado: $("#pgdAudit"),
            Mostrar: function (i, item) {
                var Accion = "";
                if (item.Accion == "I") {
                    Accion = "Inserción";
                } else if (item.Accion == "M") {
                    Accion = "Moficación";
                } else if (item.Accion == "E") {
                    Accion = "Omisión";
                }
                $("#tblAuditTrail").find("tbody").append(
                    $('<tr>')
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.Fecha))
                        .append($('<td>').append(item.Usuario))
                        .append($('<td>').append(Accion))
                        .append($('<td>').append(item.Anterior))
                        .append($('<td>').append(item.Actual))
                        .append($('<td>').append(item.Justificacion))
                );
            }
        });
    }
});