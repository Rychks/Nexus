define(["jquery"], function ($) {
    $(document).ready(function () {
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
            console.log(table);
            $("#pnlSettings_dashboard").empty();
            $.each(table, function (i, item) {

                if (item.previus_image != "") {
                    img = item.previus_image;
                }
                $("#pnlSettings_dashboard").append(
                    '<div class="col-lg-4">'
                    +'<input type="text" name="functionID" style="display:none;" value="' + item.id_dashboard + '" />'
                    + '<label class="imagecheck mb-4">'
                    + '<input name="' + item.id_dashboard+'" type="checkbox"  class="imagecheck-input">'
                    + '<figure class="imagecheck-figure">'
                    + '<img src="/Assets/img/Dashboards/' + img + '" alt="}" class="imagecheck-image mx-auto d-block">'
                    + '<p class="caption">' + item.title + '</p>'
                    + '</figure>'
                    + '</label>'
                    + '</div>'
                );
            })
            fn_get_dashboard_list();
           
        }).fai(function (error) {
            console.log(error);
        });
    }
    function fn_get_dashboard_list() {
        var url = "/Settings/get_dashboard_list"
        $.post(url).done(function (data) {
            console.log(data)
            $.each(data, function (i, item) {
                var val = $("#pnlSettings_dashboard div").find("input[value='" + item.id_dashboard + "']");
                console.log(val)
                var ckeck = $(val).parent("div").find("input[type='checkbox']");
                $(ckeck).prop("checked", true);
            })
        }).fail(function (e) {
            console.log(e);
        })
    }
});