define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_capas();
        $("#btnCapas_file_save").click(function () {
            var existeArchivo = ($("#txtCapas_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
            }
        });
        $("#btnCapas_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Capas")
            });
            fn_get_capas();
        });
        $("#btnCapas_Search").click(function () {
            fn_get_capas();
        })

        $("#pgdCapas").paginado({
            Tabla: $("#tblCapas"),
            Version: 2,
            Funcion: fn_get_capas
        });
    });
    function fn_importData() {
        //$("#pgbProjectImport_progress").removeClass("w-100");
        //$("#pgbProjectImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtCapas_file"))[0].files[0]);
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

            //var percent = (event.loaded / event.total) * 100;
            ////**percent** variable can be used for modifying the length of your progress bar.
            //$("#pgbProjectImport_progress").addClass("w-" + percent + "");
            //$("#pgbProjectImport_progress").attr("aria-valuenow", "" + percent + "");
            console.log(percent);

        });
        ajax.open("POST", '/Capas/import_capas', true);
        ajax.send(frmDatos);
    }  
    function fn_get_capas(Pagina) {
        var number = $("#txtCapas_number").val();
        if (number == "") { number = null; }
        var Datos = { number: number, Index: Pagina };
        var accesoEditar = "";
        $.mostrarInfo({
            URLindex: "/Capas/get_TotalPag_capas",
            URLdatos: "/Capas/get_capas_table",
            Datos: Datos,
            Version: 2,
            Tabla: $("#tblCapas"),
            Paginado: $("#pgdCapas"),
            Mostrar: function (i, item) {
                $("#tblCapas").find("tbody").append(
                    $('<tr>')
                        .append($('<td data-registro="ID" style="display:none">').append(item.ID))
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.number))
                        .append($('<td>').append(item.type))
                        .append($('<td>').append(item.start_date))
                        .append($('<td>').append(item.entry_date))
                        .append($('<td>').append(item.short_description))
                        .append($('<td>').append(item.client))
                        .append($('<td>').append(item.deadline_final_aprobal))
                        .append($('<td>').append(item.workflow_status))
                        .append($('<td>').append(item.success))
                );
            }
        });
    }
});