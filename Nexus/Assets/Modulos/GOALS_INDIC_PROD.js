define(["jquery"], function ($) {
    $(document).ready(function () {
        $("#btnGOALS_INDIC_PROD_file_save").click(function () {
            var existeArchivo = ($("#txtGOALS_INDIC_PROD_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
            }
        });
    });
    function fn_importData() {
        $("#pgbGOALS_INDIC_PROD_Import_progress").removeClass("w-100");
        $("#pgbGOALS_INDIC_PROD_Import_progress").attr("aria-valuenow", "0");
        $("#btnGOALS_INDIC_PROD_file_save").addClass("btn-progress");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtGOALS_INDIC_PROD_file"))[0].files[0]);
        frmDatos.append("hoja", "Hoja1");
        ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {

            if (ajax.status) {
                if (ajax.status == 200 && (ajax.readyState == 4)) {

                    $.notiMsj.Notificacion({ Mensaje: "Information Saved Correctly", Tipo: "success", Error: null });
                    $("#pgbGOALS_INDIC_PROD_Import_progress").removeClass("w-100");
                    $("#pgbGOALS_INDIC_PROD_Import_progress").attr("aria-valuenow", "0");
                    $("#btnGOALS_INDIC_PROD_file_save").removeClass("btn-progress");
                    //fn_obtenerTablaProyecto();
                    //$("#mdlProject_Import").modal("hide");

                    //To do tasks if any, when upload is completed
                }
            }
        }
        ajax.upload.addEventListener("progress", function (event) {

            var percent = (event.loaded / event.total) * 100;
            //**percent** variable can be used for modifying the length of your progress bar.
            $("#pgbGOALS_INDIC_PROD_Import_progress").addClass("w-" + percent + "");
            $("#pgbGOALS_INDIC_PROD_Import_progress").attr("aria-valuenow", "" + percent + "");
        });
        ajax.open("POST", '/Mantenimiento/import_GOALS_INDIC_PROD', true);
        ajax.send(frmDatos);
    }
});