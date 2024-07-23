define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_detail_user_templates_table();
        $("#btnHome_template_menu_remove").hide();
        //fn_get_dashboard_list()
        var prevScrollTop = $(window).scrollTop()
        $(window).on('scroll', function (e) {
            // Variable declaration for search container
            var currentScrollTop = $(this).scrollTop()
            if (currentScrollTop >= prevScrollTop && currentScrollTop >= 140 && currentScrollTop <= 250) {
                if ($("#txtHome_id_template").val() == "") {
                    fn_get_dashboard_list()
                }
                
            }
            prevScrollTop = currentScrollTop
        });
        
        $("#mdlHome_template_edit_pnl_dashboard").on("click", ".imagecheck-figure", function () {
            var id_dashboard = $(this).find("[data-registro=id_dashboard]").val();

            var val = $("#mdlHome_template_edit_pnl_dashboard div").find("input[value='" + id_dashboard + "']");
            var ckeck = $(val).parent("div").find("input[type='checkbox']");
            var id_template = $("#txtHome_template_Edit_id").val();
            if ($(ckeck).prop("checked")) {                
                fn_delete_detail_dashboard_templates(id_template, id_dashboard);
            } else {
                fn_insert_detail_dashboard_templates(id_template, id_dashboard);
            }
        });
        $("#btnHome_template_menu_edit").click(function () {
            var id_template = $("#txtHome_id_template").val();
            if (id_template != "") {
                $("#slcHome_template_Edit_dashboard_type").generarLista({ URL: "/Dashboards/get_list_dashboard_type" });
                $("#slcHome_template_Edit_department").generarLista({ URL: "/Department/get_department_list" });
                $("#txtHome_template_Edit_id").val(id_template);
                fn_get_dashboards();
                $("#mdlHome_template_edit").modal("show");
            } else {
                window.location.replace("/Settings/Index");
            }
        });
        $("#btnHome_template_menu_remove").click(function () {
            var id_template = $("#txtHome_id_template").val();
            if (id_template != "") {
                fn_delete_detail_user_templates(id_template);
            }
        });
        $("#btnTemplate_guardar").click(function () {
            fn_insert_dashboards_templates();
        });
        $("#btnTemplate_add_guardar").click(function () {
            var id_template = $("#slcTemplate_add option:selected").val();
            if (id_template != "-1") {
                fn_insert_detail_user_templates(id_template);
            } else {
                $.notiMsj.Notificacion({ Mensaje: "Por favor, ingrese la información requerida", Tipo: "info" });
            }           
        });
        $("#slcHome_opcion_template").change(function () {
            $("#pnlHome_modal_create_template").hide();
            $("#pnlHome_modal_add_template").hide();
            var option = $(this).val();
            if (option > 0) {
                $("#pnlHome_modal_add_template").show();
            } else {
                $("#pnlHome_modal_create_template").show();
            }
        });
        $("#pnlHome_templates_table").on("click", "#btnHome_add_template", function () {
            $("#slcTemplate_add").generarLista({ URL: "/DashboardTemplates/get_dashboards_templates_public_list" });
            $("#slcHome_opcion_template").val("-1")
            $("#pnlHome_modal_create_template").hide();
            $("#pnlHome_modal_add_template").hide();
            $("#mdlHome_add_template").modal("show")
        });
        $("#btn_close_dashboard_modal").click(function () {
            $("#dashboard_modal").modal('hide')
        })
        $("#dashboars_panel").on('click', "img", function () {
            $("#dashboard_modal_panel").empty();
            let code_department = $("#id_department").val();
            var link_dashboard = $(this).attr('name')
            var type = $(this).attr('data-registro');
            console.log(type);
            if (type == 1) {
                $("#dashboard_modal_panel").append(
                    '<div class="col-lg-12">'
                    + '<div class="content-dashboard-modal">'
                    + ' <iframe class="iframe-dashboard-modal" title="ScoreCard"  src="' + link_dashboard +'"&navContentPaneEnabled=false&filterPaneEnabled=false" frameborder="0" allowFullScreen="true"></iframe>'
                    + '</div>'
                    + '</div>'
                );
            } else {
                $("#dashboard_modal_panel").append(
                    '<div class="col-lg-12">'
                    + '<div class="">'
                    + '<script type="module" src="https://ncsa.tableau.intranet.cnb/javascripts/api/tableau.embedding.3.latest.min.js"></script>'
                    + '<tableau-viz id="tableauViz" device="desktop" src="' + link_dashboard + '" toolbar="bottom" hide-tabs>'
                    + '<viz-filter field="Department" value="' + code_department + '"> </viz-filter>'
                    + '</tableau-viz>'
                    + '</div>'
                    + '</div>'
                );
            }
           

            $("#dashboard_modal").modal('show')
            console.log($(this));
        });
        $(".section-button").click(function () {
            $('html, body').animate({
                scrollTop: $("#dashboars_panel").offset().top
            }, 50);
            $("#dashboars_panel").css({ "border": "solid", "border-color": "red" });
            setInterval(function () {
                $("#dashboars_panel").css("border", "none");
            }, 2000);
        })
        $("#pgdHome_templates_table").paginado({
            Tabla: $("#pnlHome_templates_table"),
            Version: 2,
            Funcion: fn_get_detail_user_templates_table
        });
        $("#btnHome_carousel_next").click(function () {
            $("#btnHome_carousel_next_aux").click();
        })
        $("#btnHome_carousel_prev").click(function () {
            $("#btnHome_carousel_prev_aux").click();
        })
        $("#pnlHome_templates_table").on("click", ".plnHome_template_item", function () {
            var id_template = $(this).find("[data-registro=Id]").val();
            var name_template = $(this).find("[data-registro=Name]").val();
            $(".plnHome_template_item .card").removeClass("active_zoom")
            $(".plnHome_template_item .card").removeClass("border-bottom-primary")
            $(".plnHome_template_item .card").addClass("border-left-info")
            $(".plnHome_template_item").addClass("zoom")
            //$(this).find(".card").removeClass("border-left-info")
            $(this).removeClass("zoom")
            //$(this).find(".card").addClass("border-bottom-primary")
            $(this).find(".card").addClass("active_zoom")
            $("#pnlHome_template_dashboards").html(name_template);
            fn_verify_owner_template(id_template);
            fn_get_template_dashboard_list(id_template);
            $("#txtHome_id_template").val(id_template);
            $("#btnHome_template_menu_remove").show();
        });
        $("#btnHome_template_menu_close").click(function () {
            $("#pnlHome_template_dashboards").html("Lista de Dashboards");
            $("#txtHome_id_template").val(null);
            $(".plnHome_template_item .card").removeClass("active_zoom")
            $(".plnHome_template_item .card").removeClass("border-bottom-primary")
            $(".plnHome_template_item .card").addClass("border-left-info")
            $("#btnHome_template_menu_remove").hide();
            fn_get_dashboard_list();
        })
    });

    function fn_verify_owner_template(id_template) {
        var frmDatos = new FormData();
        frmDatos.append("id_template", id_template);
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/verify_owner",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                $("#btnHome_template_menu_edit").hide();
                if (res > 0) {
                    $("#btnHome_template_menu_edit").show();
                }
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_delete_detail_dashboard_templates(id_template, id_dashboard) {
        var frmDatos = new FormData();
        frmDatos.append("id_template", id_template);
        frmDatos.append("id_dashboard", id_dashboard);
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/delete_detail_dashboard_templates",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                    fn_get_template_dashboard_list(id_template);
                }
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_insert_detail_dashboard_templates(id_template, id_dashboard) {
        var frmDatos = new FormData();
        frmDatos.append("id_template", id_template);
        frmDatos.append("id_dashboard", id_dashboard);
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/insert_detail_dashboard_templates",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                    fn_get_template_dashboard_list(id_template);
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
        var id_department = $("#slcHome_template_Edit_department option:selected").val();
        var title = $("#btnHome_template_Edit_Search").val();
        var id_dashboard_type = $("#slcHome_template_Edit_dashboard_type option:selected").val();
        if (id_department == "-1") { id_department = null; }
        if (title == "") { title = null; }
        if (id_dashboard_type == "-1") { id_dashboard_type = null; }
        var Datos = { id_department: id_department, title: title, id_dashboard_type: id_dashboard_type, is_enable: 1, Index: 0, NumRegistros: 1000 };
        let img = "default.png";
        $.post(url, Datos).done(function (table) {
            $("#mdlHome_template_edit_pnl_dashboard").empty();
            $.each(table, function (i, item) {
                if (item.previus_image != "") {
                    img = item.previus_image;
                }
                $("#mdlHome_template_edit_pnl_dashboard").append(
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
            fn_get_dashboard_list_by_tmplate($("#txtHome_id_template").val());

        }).fail(function (error) {
            console.log(error);
        });
    }
    function fn_get_dashboard_list_by_tmplate(id_template) {
        var url = "/DashboardTemplates/get_template_dashboard_list"
        var post_data = { id_template: id_template };
        $.post(url, post_data).done(function (data) {
            $.each(data, function (i, item) {
                var val = $("#mdlHome_template_edit_pnl_dashboard div").find("input[value='" + item.id_dashboard + "']");
                var ckeck = $(val).parent("div").find("input[type='checkbox']");
                $(ckeck).prop("checked", true);
            })
        }).fail(function (e) {
            console.log(e);
        })
    }
    function fn_get_template_dashboard_list(id_template) {
        var url = "/DashboardTemplates/get_template_dashboard_list"
        var post_data = { id_template: id_template };
        $.post(url, post_data).done(function (data) {
            let counter = 0;
            let class_mt = ""
            $("#dashboars_panel").empty();
            $.each(data, function (i, item) {

                $("#dashboars_panel").append(
                    '<div class="col-md-3 ' + class_mt + '" style="">'
                    + '<div class="card preview-dash zoom show_dashboard_prev">'
                    + '<div class="card-body titulo-dashoard">'
                    + '<h5 class="card-title">' + item.title + '</h5>'
                    + '<p class="card-text"></p>'
                    + '</div>'
                    + '<img class="card-img-bottom mx-auto d-block" src="/Assets/img/Dashboards/' + item.previus_image + '" data-registro="' + item.type + '" name="' + item.link + '"   alt="Imagen no disponible">'
                    + '</div>'
                    + '</div>'
                )
                counter = counter + 1;
                if (counter > 3) { class_mt = "mt-3"; }
            })
        }).fail(function (e) {
            console.log(e);
        })
    }
    function fn_get_detail_user_templates_table(Pagina) {
        var Datos = { Index: Pagina };
        const btnHome_new_template = '<div class="col-md-2 zoom mb-4" id="btnHome_add_template">'
                +'<div class="card border-left-success shadow h-100 py-2">'
                    +'<div class="card-body">'
                       + '<div class="row no-gutters align-items-center">'
                          + '<div class="col mr-2">'
                           + '<div class="h5 mb-0 font-weight-bold text-gray-800">Nuevo Template</div>'
                           + '</div>'
                            +'<div class="col-auto">'
                               +'<button class="btn"><i class="fas fa-plus fa-2x text-gray-300"></i></button>'
                            +'</div>'
                       + '</div>'
                    +'</div>'
                +'</div>'
            +'</div>'
        var accesoEditar = "";
        $("#pnlHome_templates_table").empty();
        $("#pnlHome_templates_table").hide();
        $.mostrarInfo({
            URLindex: "/DashboardTemplates/get_TotalPag_detail_user_templates",
            URLdatos: "/DashboardTemplates/get_detail_user_templates_table",
            Datos: Datos,
            Version: 2,
            Tabla: $("#pnlHome_templates_table"),
            Paginado: $("#pgdHome_templates_table"),
            Mostrar: function (i, item) {
                $("#pnlHome_templates_table").append(
                    '<div class="col-md-2 zoom mb-4 plnHome_template_item">'
                    + '<input type="type" style="display:none" data-registro="Name" name="name" value="' + item.name_template + '" />'
                    + '<input type="type" style="display:none"  data-registro="Id" name="name" value="' + item.id_template + '" />'
                    + '<div class="card border-left-info shadow h-100 py-2">'
                    + '<div class="card-body">'
                    + '<div class="row no-gutters align-items-center">'
                    + '<div class="col mr-2">'
                    + '<div class="h5 mb-0 font-weight-bold text-gray-800">' + item.name_template + '</div>'
                    + '</div>'
                    + '<div class="col-auto">'
                    + '<button class="btn"><i class="fas fa-chart-column fa-2x text-gray-300"></i></button>'
                    + '</div>'
                    + '</div>'
                    + '</div>'
                    + '</div>'
                    + '</div>'
                );
            }
        });
        $("#pnlHome_templates_table").append(btnHome_new_template);
        $("#pnlHome_templates_table").fadeIn(2000);	
        gsap.from("#dashboars_panel div", {
            stagger: 1
        });
    }

    function fn_insert_dashboards_templates() {
        var frmDatos = new FormData();
        frmDatos.append("name", $("#txtTemplate_name").val());
        if ($("#cbxTemplate_is_private").prop("checked")) {
            frmDatos.append("is_private", 1);
        } else {
            frmDatos.append("is_private", 0);
        }        
        $("#btnTemplate_guardar").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/insert_dashboards_templates",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                   
                    fn_get_detail_user_templates_table();
                    $("#slcHome_template_Edit_dashboard_type").generarLista({ URL: "/Dashboards/get_list_dashboard_type" });
                    $("#slcHome_template_Edit_department").generarLista({ URL: "/Department/get_department_list" });
                    $("#txtHome_template_Edit_id").val(res.Id);
                    fn_get_dashboards();
                    $("#mdlHome_add_template").modal("hide")
                    $("#mdlHome_template_edit").modal("show");
                }
                $("#btnTemplate_guardar").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_delete_detail_user_templates(id_template) {
        var frmDatos = new FormData();
        frmDatos.append("id_template", id_template);
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/delete_detail_user_templates",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                    fn_get_detail_user_templates_table();
                    $("#btnHome_template_menu_close").click();
                }
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_insert_detail_user_templates(id_template) {
        var frmDatos = new FormData();
        frmDatos.append("id_template", id_template);
        $("#btnTemplate_add_guardar").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/DashboardTemplates/insert_detail_user_templates",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Type == "success") {
                    fn_get_detail_user_templates_table();
                }
                $("#btnTemplate_add_guardar").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Message, Tipo: res.Type });
            },
            error: function (error) {
                $.notiMsj.Notificacion({ Mensaje: "Se produjo un error inesperado", Tipo: "danger", Error: error });
            }
        });
    }
    function fn_get_dashboard_list() {
        var url = "/Settings/get_dashboard_list"
        
        $.post(url).done(function (data) {
            let counter = 0;
            let class_mt = ""
            $("#dashboars_panel").empty();
            $.each(data, function (i, item) {

                $("#dashboars_panel").append(
                    '<div class="col-md-3 ' + class_mt + '" style="">'
                    + '<div class="card preview-dash zoom show_dashboard_prev" >'
                    + '<div class="card-body titulo-dashoard">'
                    + '<h5 class="card-title">' + item.title + '</h5>'
                    + '<p class="card-text"></p>'
                    + '</div>'
                    + '<img class="card-img-bottom mx-auto d-block" src="/Assets/img/Dashboards/' + item.previus_image + '" data-registro="' + item.type + '" name="' + item.link + '"   alt="Imagen no disponible">'
                    + '</div>'
                    + '</div>'
                )
                counter = counter + 1;
                if (counter > 3) { class_mt = "mt-3"; }
            })
            
        }).fail(function (e) {
            console.log(e);
        })
    }
});