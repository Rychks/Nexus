define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_Pno_vencimiento();
        $("#btnPno_vencimiento_file_save").click(function () {
            var existeArchivo = ($("#txtPno_vencimiento_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
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
        //$("#pgbProjectImport_progress").removeClass("w-100");
        //$("#pgbProjectImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtPno_vencimiento_file"))[0].files[0]);
        frmDatos.append("hoja", "Hoja1");
        ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {

            if (ajax.status) {
                if (ajax.status == 200 && (ajax.readyState == 4)) {
                    $.notiMsj.Notificacion({ Mensaje: "Information Saved Correctly", Tipo: "success", Error: null });
                    //$("#pgbProjectImport_progress").removeClass("w-100");
                    //$("#pgbProjectImport_progress").attr("aria-valuenow", "0");
                    //$("#btnProjectImport_save").removeClass("btn-loading");
                    //fn_obtenerTablaProyecto();
                    //$("#mdlProject_Import").modal("hide");

                    //To do tasks if any, when upload is completed
                }
            }
        }
        ajax.upload.addEventListener("progress", function (event) {

            var percent = (event.loaded / event.total) * 100;
            ////**percent** variable can be used for modifying the length of your progress bar.
            //$("#pgbProjectImport_progress").addClass("w-" + percent + "");
            //$("#pgbProjectImport_progress").attr("aria-valuenow", "" + percent + "");
            console.log(percent);

        });
        ajax.open("POST", '/QA/import_pno_vencimiento', true);
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