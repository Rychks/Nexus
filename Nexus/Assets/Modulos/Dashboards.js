define(["jquery"], function ($) {
    $(document).ready(function () {
        $("#slcDashboard_dashboar_type").generarLista({ URL: "/Dashboards/get_list_dashboard_type" });
        $("#slcDasboard_department").generarLista({ URL: "/Department/get_department_list" });
        fn_get_dashboard();
        $("#tblDashboards tbody").on('click', "a[data-registro=Editar]", function () {
            var ID = $(this).parents("tr").find("[data-registro=ID]").html();
            $("#mdlDashboards_Upsert_title").html("Actualizar Dashboard")
            $.post("/Dashboards/get_Dashboard_info", { id_dashboard: ID }).done(function (res) {
                if (res != "") {
                    $.each(res, function (i, item) {

                        $("#txtDashboard_upsert_id").val(item.id_dashboard);
                        $("#txtDashboard_upsert_titulo").val(item.title);
                        $("#txtDashboard_upsert_code_department").val(item.code_department);
                        $("#txtDashboard_upsert_link").val(item.link);
                        $("#txtDashboard_upsert_is_enable").val(item.is_enable);
                        $("#txtDashboard_upsert_id_departamento").generarLista({ URL: "/Department/get_department_list", Seleccion: item.id_department });
                        $("#txtDashboard_upsert_id_dashboard_tipo").generarLista({ URL: "/Dashboards/get_list_dashboard_type", Seleccion: item.id_dashboard_type });
                        $('#txtDashboard_upsert_img_preview').attr('src', '/Assets/img/Dashboards/' + item.previus_image);

                    });
                    $("#mdlDashboards_Upsert").modal("show");
                }
            }).fail(function (error) { $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Mostrar_informacion_error'), Tipo: "danger", Error: error }); });
        });
        $("#btnDashboard_add").click(function () {
            $("#txtDashboard_upsert_id_departamento").generarLista({ URL: "/Department/get_department_list"});
            $("#txtDashboard_upsert_id_dashboard_tipo").generarLista({ URL: "/Dashboards/get_list_dashboard_type"});
            $.auxFormulario.limpiarCampos({ Seccion: $("#frmDashboards_upsert") });
            $("#mdlDashboards_Upsert_title").html("Registrar Dashboard")
            $("#mdlDashboards_Upsert").modal("show");
        })
        $("#btnDashboard_upsert_save").click(function () {
            $.auxFormulario.camposVacios({
                Seccion: $("#frmDashboards_upsert"),
                Excepciones: ['txtDashboard_upsert_id','txtDashboard_upsert_img'],
                NoVacio: function () {
                    fn_upsert_dashboard()
                },
            });        
        })
        $("#btnDashboards_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Dashboards")
            });
            fn_get_dashboard();
        });
        $("#btnDashboards_Search").click(function () {
            fn_get_dashboard();
        })
        $("#txtDashboard_upsert_id_departamento").change(function () {
            let id_department = $(this).val();
            fn_get_department_department_code(id_department);
        })
        $("#pgdDashboards").paginado({
            Tabla: $("#tblDashboards"),
            Version: 2,
            Funcion: fn_get_dashboard
        });
        $("#txtDashboard_upsert_img").change(function () {
            readURL(this);
        });
        $("#txtDashboard_title").on('keypress', function (e) {
            if (e.which == 13) {
                fn_get_dashboard();
            }
        })
        $(document).on('keyup', function (e) {
            if (e.key == "Escape") {
                $('#btnDashboards_clean').click();
            }
        });
        $("#tblDashboards tbody").on('click', "a[data-registro=Accesos]", function () {
            var ID = $(this).parents("tr").find("[data-registro=ID]").html();
            $("#mdlDashboards_access").modal("show");
            //$.post("/Dashboards/get_Dashboard_info", { id_dashboard: ID }).done(function (res) {
            //    if (res != "") {
            //        $.each(res, function (i, item) {

            //            $("#txtDashboard_upsert_id").val(item.id_dashboard);
            //            $("#txtDashboard_upsert_titulo").val(item.title);
            //            $("#txtDashboard_upsert_code_department").val(item.code_department);
            //            $("#txtDashboard_upsert_link").val(item.link);
            //            $("#txtDashboard_upsert_is_enable").val(item.is_enable);
            //            $("#txtDashboard_upsert_id_departamento").generarLista({ URL: "/Department/get_department_list", Seleccion: item.id_department });
            //            $("#txtDashboard_upsert_id_dashboard_tipo").generarLista({ URL: "/Dashboards/get_list_dashboard_type", Seleccion: item.id_dashboard_type });
            //            $('#txtDashboard_upsert_img_preview').attr('src', '/Assets/img/Dashboards/' + item.previus_image);

            //        });
            //        $("#mdlDashboards_Upsert").modal("show");
            //    }
            //}).fail(function (error) { $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Mostrar_informacion_error'), Tipo: "danger", Error: error }); });
        });
        $("#slcDashboards_access_level").change(function () {
            var option = $(this).val();
            if (option != "-1") {
                if (option == "2") {

                }
            }
        })
    });

    function fn_get_department_department_code(id_department) {
        var url = "/Department/get_department_department_code";
        var data_post = { id_department: id_department };
        $.post(url, data_post).done(function (res) {
            $("#txtDashboard_upsert_code_department").val(res);
        }).fail(function (error) {
            console.log(error)
        })
    }
    function fn_upsert_dashboard() {
        var frmDatos = new FormData();
        frmDatos.append("id_dashboard", $("#txtDashboard_upsert_id").val());
        frmDatos.append("link", $("#txtDashboard_upsert_link").val());
        frmDatos.append("title", $("#txtDashboard_upsert_titulo").val());
        frmDatos.append("id_department", $("#txtDashboard_upsert_id_departamento option:selected").val());
        frmDatos.append("code_department", $("#txtDashboard_upsert_code_department").val());
        frmDatos.append("is_enable", $("#txtDashboard_upsert_is_enable option:selected").val());
        var existeArchivo = ($("#txtDashboard_upsert_img"))[0].files[0];
        if (!existeArchivo) {
            frmDatos.append("img", null);
        } else {
            frmDatos.append("img", ($("#txtDashboard_upsert_img"))[0].files[0]);
        }        
        frmDatos.append("id_dashboard_type", $("#txtDashboard_upsert_id_dashboard_tipo option:selected").val());
        $("#btnDashboard_upsert_save").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/Dashboards/upsert_dashboard",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                    $("#mdlDashboards_Upsert").modal("hide");
                    $.auxFormulario.limpiarCampos({ Seccion: $("#frmDashboards_upsert") });
                }
                fn_get_dashboard();
                $("#btnDashboard_upsert_save").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $("#mdlDashboards_Upsert").modal("hide");
                $("#btnDashboard_upsert_save").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#txtDashboard_upsert_img_preview').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
    function fn_get_dashboard(Pagina) {
        var id_department = $("#slcDasboard_department option:selected").val();
        var title = $("#txtDashboard_title").val();
        var id_dashboard_type = $("#slcDashboard_dashboar_type option:selected").val();
        var is_enable = $("#slcDashboard_is_enable option:selected").val();
        if (id_department == "-1") { id_department = null; }
        if (title == "") { title = null; }
        if (id_dashboard_type == "-1") { id_dashboard_type = null; }
        if (is_enable == "-1") { is_enable = null; }
        var Datos = { id_department: id_department, title: title, id_dashboard_type: id_dashboard_type, is_enable: is_enable, Index: Pagina };
        var Botones = '<div class="item-action dropdown"><a href="javascript:void(0)" data-toggle="dropdown" class="icon" title="Options"><i class="fas fa-bars" style="z-index:-99 !important;"></i></a>' +
            '<div class="dropdown-menu dropdown-menu-right">' +
            '<a href="javascript:void(0)" class="dropdown-item" data-registro="Editar" style="font-family: Arial;"><i class="dropdown-icon fas fa-edit"></i>  Editar registro </a>'+
           // '<a href="javascript:void(0)" class="dropdown-item" data-registro="Accesos" style="font-family: Arial;"><i class="dropdown-icon fa-solid fa-user-lock"></i>  Editar accesos </a>'+
            '</div></div>';
        var accesoEditar = "";
        $.mostrarInfo({
            URLindex: "/Dashboards/get_TotalPag_dashboards",
            URLdatos: "/Dashboards/get_table_dashboards",
            Datos: Datos,
            Version: 2,
            Tabla: $("#tblDashboards"),
            Paginado: $("#pgdDashboards"),
            Mostrar: function (i, item) {
                var estatus = '<span class="badge badge-light">Activo</span>';
                if (item.is_enable != "1") {
                    estatus = '<span class="badge badge-light">Inactivo</span>';
                } else {
                    estatus = '<span class="badge badge-light">Activo</span>';
                }
                $("#tblDashboards").find("tbody").append(
                    $('<tr>')
                        .append($('<td data-registro="ID" style="display:none">').append(item.id_dashboard))
                        .append($('<td>').append(item.RowNumber))
                        .append($('<td>').append(item.title))
                        .append($('<td>').append(item.dashboard_type))
                        .append($('<td>').append(estatus))
                        .append($('<td>').append(item.insert_time_stamp))
                        .append($('<td>').append(item.update_time_stamp))
                        .append($('<td>').append(Botones))
                );
            }
        });
    }
});