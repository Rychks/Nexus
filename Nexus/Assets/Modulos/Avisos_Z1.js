define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_Avisos_z1();
        $("#btnAvisos_z1_file_save").click(function () {
            var existeArchivo = ($("#txtAvisos_z1_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
            }
        });
        $("#btnAvisos_z1_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Avisos_z1")
            });
            fn_get_Avisos_z1();
        });
        $("#btnAvisos_z1_Search").click(function () {
            fn_get_Avisos_z1();
        })

        $("#pgdAvisos_z1").paginado({
            Tabla: $("#tblAvisos_z1"),
            Version: 2,
            Funcion: fn_get_Avisos_z1
        });
    });
    function fn_importData() {
        //$("#pgbProjectImport_progress").removeClass("w-100");
        //$("#pgbProjectImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtAvisos_z1_file"))[0].files[0]);
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
        ajax.open("POST", '/Mantenimiento/import_avisos_Z1', true);
        ajax.send(frmDatos);
    }
    function fn_get_Avisos_z1(Pagina) {
        var number = $("#txtAvisos_z1_number").val();
        if (number == "") { number = null; }
        var Datos = { number: number, Index: Pagina };
        var accesoEditar = "";
        $.mostrarInfo({
            URLindex: "/Mantenimiento/get_TotalPag_avisos_z1",
            URLdatos: "/Mantenimiento/get_avisos_z1_table",
            Datos: Datos,
            Version: 2,
            Tabla: $("#tblAvisos_z1"),
            Paginado: $("#pgdAvisos_z1"),
            Mostrar: function (i, item) {
                $("#tblAvisos_z1").find("tbody").append(
                    $('<tr>')
                        .append($('<td data-registro="ID" style="display:none">').append(item.ID))
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.centro_costos))
                        .append($('<td>').append(item.ubicacion_tecnica))
                        .append($('<td>').append(item.aviso))
                        .append($('<td>').append(item.fecha_aviso))
                        .append($('<td>').append(item.clase_aviso))
                        .append($('<td>').append(item.descripcion))
                        .append($('<td>').append(item.orden))
                        .append($('<td>').append(item.modificado_por))
                        .append($('<td>').append(item.clase_trabajo))
                        .append($('<td>').append(item.status_sistema))
                        .append($('<td>').append(item.status_usuario))
                        .append($('<td>').append(item.prioridad))
                );
            }
        });
    }
});