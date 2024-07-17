define(["jquery"], function ($) {
    $(document).ready(function () {
        $("#btnPnosImport_save").click(function () {
            var existeArchivo = ($("#txtPnosImport_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
            }
        });
        $("#btnCapasImport_save").click(function () {
            var existeArchivo = ($("#txtCapasImport_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData_Capas();
            }
        });
        $("#btnPno_vencimiento_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Pno_vencimiento")
            });
            fn_get_Pno_vencimiento();
        });
        $("#btnPno_vencimiento_Search").click(function () {
            fn_get_Pno_vencimiento();
        })

        $("#pgdPno_vencimiento").paginado({
            Tabla: $("#tblPno_vencimiento"),
            Version: 2,
            Funcion: fn_get_Pno_vencimiento
        });
    });
    function fn_importData() {
        $("#pgbPnosImport_progress").removeClass("w-100");
        $("#pgbPnosImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtPnosImport_file"))[0].files[0]);
        frmDatos.append("hoja", "Hoja1");
        ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {

            if (ajax.status) {
                if (ajax.status == 200 && (ajax.readyState == 4)) {
                    $.notiMsj.Notificacion({ Mensaje: "Ok", Tipo: "success", Error: null });
                    $("#pgbPnosImport_progress").removeClass("w-100");
                    $("#pgbPnosImport_progress").attr("aria-valuenow", "0");
                    $("#btnPnosImport_save").removeClass("btn-loading");
                }
            }
        }
        ajax.upload.addEventListener("progress", function (event) {

            var percent = (event.loaded / event.total) * 100;
            ////**percent** variable can be used for modifying the length of your progress bar.
            $("#pgbPnosImport_progress").addClass("w-" + percent + "");
            $("#pgbPnosImport_progress").attr("aria-valuenow", "" + percent + "");
            console.log(percent);

        });
        ajax.open("POST", '/QA/import_pno_vencimiento', true);
        ajax.send(frmDatos);
    }
    function fn_importData_Capas() {
        $("#pgbCapasImport_progress").removeClass("w-100");
        $("#pgbCapasImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtCapasImport_file"))[0].files[0]);
        frmDatos.append("hoja", "Hoja1");
        ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {
            console.log(ajax);
            if (ajax.status) {
                if (ajax.status == 200 && (ajax.readyState == 4)) {
                    $.notiMsj.Notificacion({ Mensaje: "Ok", Tipo: "success", Error: null });
                    $("#pgbCapasImport_progress").removeClass("w-100");
                    $("#pgbCapasImport_progress").attr("aria-valuenow", "0");
                    $("#btnCapasImport_save").removeClass("btn-loading");
                }
            }
        }
        ajax.upload.addEventListener("progress", function (event) {

            var percent = (event.loaded / event.total) * 100;
            ////**percent** variable can be used for modifying the length of your progress bar.
            $("#pgbCapasImport_progress").addClass("w-" + percent + "");
            $("#pgbCapasImport_progress").attr("aria-valuenow", "" + percent + "");
            console.log(percent);

        });
        ajax.open("POST", '/QA/import_capas', true);
        ajax.send(frmDatos);
    }
    function fn_get_Pno_vencimiento(Pagina) {
        var cwid = $("#txtPno_vencimiento_cwid").val();
        if (cwid == "") { cwid = null; }
        var Datos = { cwid: cwid, Index: Pagina };
        var accesoEditar = "";
        $.mostrarInfo({
            URLindex: "/QA/get_TotalPag_pno_vencimiento",
            URLdatos: "/QA/get_pno_vencimiento_table",
            Datos: Datos,
            Version: 2,
            Tabla: $("#tblPno_vencimiento"),
            Paginado: $("#pgdPno_vencimiento"),
            Mostrar: function (i, item) {
                $("#tblPno_vencimiento").find("tbody").append(
                    $('<tr>')
                        .append($('<td data-registro="ID" style="display:none">').append(item.ID))
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.cwid))
                        .append($('<td>').append(item.last_name))
                        .append($('<td>').append(item.fist_name))
                        .append($('<td>').append(item.super_last_name))
                        .append($('<td>').append(item.super_first_name))
                        .append($('<td>').append(item.item_ID))
                        .append($('<td>').append(item.requiered))
                        .append($('<td>').append(item.departamento))
                );
            }
        });
    }
});