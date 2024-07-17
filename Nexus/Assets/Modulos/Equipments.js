define(["jquery"], function ($) {
    $(document).ready(function () {
        $("#btnEquipmentImport_save").click(function () {
            var existeArchivo = ($("#txtEquipmentImport_file"))[0].files[0];
            if (!existeArchivo) {
                $.notiMsj.Notificacion({ Mensaje: "Please enter the required information", Tipo: "info", Error: null });
            } else {
                fn_importData_Equipments();
            }
        });
    });
    function fn_importData_Equipments() {
        $("#pgbEquipmentImport_progress").removeClass("w-100");
        $("#pgbEquipmentImport_progress").attr("aria-valuenow", "0");
        var frmDatos = new FormData();
        frmDatos.append("archivo", ($("#txtEquipmentImport_file"))[0].files[0]);
        frmDatos.append("hoja", "Hoja1");
        ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {
            console.log(ajax);
            if (ajax.status) {
                if (ajax.status == 200 && (ajax.readyState == 4)) {
                    $.notiMsj.Notificacion({ Mensaje: "Ok", Tipo: "success", Error: null });
                    $("#pgbEquipmentImport_progress").removeClass("w-100");
                    $("#pgbEquipmentImport_progress").attr("aria-valuenow", "0");
                    $("#btnEquipmentImport_save").removeClass("btn-loading");
                }
            }
        }
        ajax.upload.addEventListener("progress", function (event) {

            var percent = (event.loaded / event.total) * 100;
            ////**percent** variable can be used for modifying the length of your progress bar.
            $("#pgbEquipmentImport_progress").addClass("w-" + percent + "");
            $("#pgbEquipmentImport_progress").attr("aria-valuenow", "" + percent + "");
            console.log(percent);

        });
        ajax.open("POST", '/Equipments/import_equipments', true);
        ajax.send(frmDatos);
    }
});