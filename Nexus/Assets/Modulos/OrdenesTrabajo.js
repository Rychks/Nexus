define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_OrdenesTrabajo();
        $("#btnOrdenesTrabajo_file_save").click(function () {
            var existeArchivo = ($("#txtOrdenesTrabajo_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData();
            }
        });
        $("#btnOrdenesTrabajo_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_OrdenesTrabajo")
            });
            fn_get_OrdenesTrabajo();
        });
        $("#btnOrdenesTrabajo_Search").click(function () {
            fn_get_OrdenesTrabajo();
        })

        $("#pgdOrdenesTrabajo").paginado({
            Tabla: $("#tblOrdenesTrabajo"),
            Version: 2,
            Funcion: fn_get_OrdenesTrabajo
        });
    });
    function fn_importData() {
        //$("#pgbProjectImport_progress").removeClass("w-100");
        //$("#pgbProjectImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtOrdenesTrabajo_file"))[0].files[0]);
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
                } else {
                    console.log(ajax);
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
        ajax.open("POST", '/OrdenesTrabajo/import_ordenes_trabajo', true);
        ajax.send(frmDatos);
    }
    function fn_get_OrdenesTrabajo(Pagina) {
        var number = $("#txtOrdenesTrabajo_number").val();
        if (number == "") { number = null; }
        var Datos = { orden: number, Index: Pagina };
        var accesoEditar = "";
        $.mostrarInfo({
            URLindex: "/OrdenesTrabajo/get_TotalPag_ordenes_trabajo",
            URLdatos: "/OrdenesTrabajo/get_ordenes_trabajo_table",
            Datos: Datos,
            Version: 2,
            Tabla: $("#tblOrdenesTrabajo"),
            Paginado: $("#pgdOrdenesTrabajo"),
            Mostrar: function (i, item) {
                $("#tblOrdenesTrabajo").find("tbody").append(
                    $('<tr>')
                        .append($('<td data-registro="ID" style="display:none">').append(item.ID))
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.orden))
                        .append($('<td>').append(item.aviso))
                        .append($('<td>').append(item.ubicacion_tecnica))
                        .append($('<td>').append(item.denominacion))
                        .append($('<td>').append(item.texto_breve))
                        .append($('<td>').append(item.status_sistema))
                        .append($('<td>').append(item.autor))
                        .append($('<td>').append(item.modificado_por))
                );
            }
        });
    }
});