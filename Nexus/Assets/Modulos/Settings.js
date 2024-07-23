define(["jquery"], function ($) {
    $(document).ready(function () {
        const counter = 0;
        $("#slcSettings_dashboard_type").generarLista({ URL: "/Dashboards/get_list_dashboard_type" });
        $("#slcSettings_department").generarLista({ URL: "/Department/get_department_list" });
        $("#slcSettings_department").change(function () {
            fn_get_dashboards();
        })
        $("#slcSettings_dashboard_type").change(function () {
            fn_get_dashboards();
        })
        $("#txtSettings_title").on('keypress', function (e) {
            if (e.which == 13) {
                fn_get_dashboards();
            }
        });
        $("#pnlSettings_dashboard").on("click", ".imagecheck-figure", function () {
            var ID = $(this).find("[data-registro=id_dashboard]").val();

            var val = $("#pnlSettings_dashboard div").find("input[value='" + ID + "']");
            var ckeck = $(val).parent("div").find("input[type='checkbox']");
            if ($(ckeck).prop("checked")) {
                fn_delete_user_settings(ID);
            } else {
                fn_insert_user_settings(ID);
            }
        });

        $("#btnSettings_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Settings")
            });
            fn_get_dashboards();
        });
        $(document).on('keyup', function (e) {
            if (e.key == "Escape") {
                $('#btnSettings_clean').click();
            }
        });
        fn_get_dashboards();
    });
    function fn_delete_user_settings(id_dashboard) {
        var frmDatos = new FormData();
        frmDatos.append("id_dashboard", id_dashboard);
        $("#btnDashboard_upsert_save").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/Settings/delete_user_settings",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                }
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_insert_user_settings(id_dashboard) {
        var frmDatos = new FormData();
        frmDatos.append("id_dashboard", id_dashboard);
        $("#btnDashboard_upsert_save").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/Settings/insert_user_settings",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                }
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_get_dashboards() {
        let url = "/Dashboards/get_table_dashboards";
        var id_department = $("#slcSettings_department option:selected").val();
        var title = $("#txtSettings_title").val();
        var id_dashboard_type = $("#slcSettings_dashboard_type option:selected").val();
        if (id_department == "-1") { id_department = null; }
        if (title == "") { title = null; }
        if (id_dashboard_type == "-1") { id_dashboard_type = null; }
        var Datos = { id_department: id_department, title: title, id_dashboard_type: id_dashboard_type, is_enable:1, Index: 0, NumRegistros: 1000 };
        let img = "default.png";
        $.post(url, Datos).done(function (table) {
            $("#pnlSettings_dashboard").empty();
            $.each(table, function (i, item) {
                if (item.previus_image != "") {
                    img = item.previus_image;
                }
                $("#pnlSettings_dashboard").append(
                    '<div class="col-lg-3 item-dashboard-list">'
                    + '<input type="text" name="functionID" style="display:none;" value="' + item.id_dashboard + '" />'
                    + '<label style="width:100% !important;" class="imagecheck mb-4">'
                    + '<input type="checkbox"  class="imagecheck-input">'
                    + '<figure style="" class="imagecheck-figure">'
                    + '<input type="text" data-registro="id_dashboard" style="display:none;" value="' + item.id_dashboard + '" />'
                    + '<img style="max-height:200px; min-height:200px; object-fit: contain;" src="/Assets/img/Dashboards/' + img + '" alt="Imagen no disponible" class="imagecheck-image mx-auto d-block">'
                    + '<p class="caption">' + item.title + '</p>'
                    + '</figure>'
                    + '</label>'
                    + '</div>'
                );
            })
            fn_get_dashboard_list();
           
        }).fail(function (error) {
            console.log(error);
        });
    }
    function fn_get_dashboard_list() {
        var url = "/Settings/get_dashboard_list"
        $.post(url).done(function (data) {
            $.each(data, function (i, item) {
                var val = $("#pnlSettings_dashboard div").find("input[value='" + item.id_dashboard + "']");
                var ckeck = $(val).parent("div").find("input[type='checkbox']");
                $(ckeck).prop("checked", true);
            })
        }).fail(function (e) {
            console.log(e);
        })
    }
});