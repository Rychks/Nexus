define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_iniUsers();
        $("#btnUsers_clean").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frm_Users")
            });
            fn_get_user_table();
        });
        $("#btnUser_Add").click(function () {
            $("#slcUserN_rol").generarLista({ URL: "/Rol/get_roles_list" });
        });
        $("#slcUsers_is_enable").change(function () {
            fn_get_user_table();
        });
        $("#slcUsers_department").change(function () {
            fn_get_user_table();
        })
        $("#txtUsers_cwid,#txtUsers_fullname").on('keypress', function (e) {
            if (e.which == 13) {
                fn_get_user_table();
            }
        });
        $(document).on('keyup', function (e) {
            if (e.key == "Escape") {
                $('#btnUsers_clean').click();
            }
        });
        $("#btnUsuariosN_Guardar").click(function () {
            $.auxFormulario.camposVacios({
                Seccion: $("#frmUserN"),
                NoVacio: function () {
                    fn_insert_user()
                }
            });
        });
        $("#btnUsuariosM_Guardar").click(function () {
            $.auxFormulario.camposVacios({
                Seccion: $("#frmUsuariosM"),
                NoVacio: function () {
                    $.notiMsj.Confirmacion({
                        Tipo: "MD",
                        Titulo: $.CargarIdioma.Obtener_Texto('txt_Idioma_Confirmacion_modificar_title'),
                        Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Confirmacion_modificar'),
                        BotonSi: $.CargarIdioma.Obtener_Texto('txt_Idioma_Notificacion_SI'),
                        BotonNo: $.CargarIdioma.Obtener_Texto('txt_Idioma_Cancelar'),
                        FuncionV: function () {
                            $.firmaElectronica.MostrarFirma({
                                Justificacion: true,
                                Funcion: fn_actualizar_usuario
                            });
                        }
                    });
                }
            });
        });
        $("#btnUsers_Search").click(function () {
            fn_get_user_table();
        });
        $("#tblUsers table tbody").on('click', "a[data-registro=Editar]", function () {
            var ID = $(this).parents("tr").find("[data-registro=ID]").html();
            $.post("/Usuarios/obtenerUsuario", { ID: ID }).done(function (res) {
                if (res != "") {
                    $.each(res, function (i, item) {
                        $("#txtUsuariosM_ID").val(item.ID);
                        $("#txtUsuariosM_CWID").val(item.CWID);
                        $("#txtUsuariosM_Nombre").val(item.Nombre);
                        $("#txtUsuariosM_App").val(item.App);
                        $("#txtUsuariosM_Correo").val(item.Correo);
                        $("#slcUsuariosM_Departamento").generarLista({ URL: "/Departamento/Lista_Departamentos", Seleccion: item.Departamento });
                        $("#slcUsuariosM_Rol").generarLista({ URL: "/Usuarios/obtener_Lista_Roles", Seleccion: item.Rol });
                        if (item.Activo == 1) {
                            $("#cbxUsuariosM_Activo").prop("checked", true);
                            $("label[for='cbxUsuariosM_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Activado'));
                        } else {
                            $("#cbxUsuariosM_Activo").prop("checked", false);
                            $("label[for='cbxUsuariosM_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Desactivado'));
                        }

                    });
                    $("#mdlUsuarios_Modificar").modal("show");
                }
            }).fail(function (error) { $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Mostrar_informacion_error'), Tipo: "danger", Error: error }); });
        });
        $("#pgdUsers").paginado({
            Tabla: $("#tblUsers"),
            Version: 2,
            Funcion: fn_get_user_table
        });
    });
    function fn_iniUsers() {
        $("#slcUsers_department").generarLista({ URL: "/Department/get_department_list" });
        fn_get_user_table();
        $.matrizAccesos.verificaAcceso({ Elemento: $("#btnUsers_add"), Url: "/Roles/verificarAcceso", FuncionId: 7 });
    }
    function fn_get_user_table(Pagina) {
        var cwid = $("#txtUsers_cwid").val();
        var fullname = $("#txtUsers_fullname").val();
        var department = $("#slcUsers_department option:selected").val();
        var is_enable = $("#slcUsers_is_enable option:selected").val();
        if (cwid == "") { cwid = null; }
        if (fullname == "") { fullname = null; }
        var Datos = { cwid: cwid, fullname: fullname, department: department == -1 ? null : department, is_enable: is_enable == -1 ? null : is_enable, Index: Pagina };
        var accesoEditar = "";
        $.matrizAccesos.validaAcceso({ FuncionId: 6 })
            .then(obj => {
                if (obj.result > 0) {
                    accesoEditar = '<a href="javascript:void(0)" class="dropdown-item" data-registro="Editar"><i class="dropdown-icon fas fa-edit"></i>Edit</a>';
                }

                var Botones = '<div class="item-action dropdown"><a href="javascript:void(0)" data-toggle="dropdown" class="icon" title="Options"><i class="fas fa-bars" style="z-index:-99 !important;"></i></a>' +
                    '<div class="dropdown-menu dropdown-menu-right">' +
                    accesoEditar +
                    '</div></div>'; 7
                $.mostrarInfo({
                    URLindex: "/Users/get_TotalPag_users",
                    URLdatos: "/Users/get_users_table",
                    Datos: Datos,
                    Version: 2,
                    Tabla: $("#tblUsers"),
                    Paginado: $("#pgdUsers"),
                    Mostrar: function (i, item) {

                        var Activo = '<div class="badge badge-success">Activo</div>';
                        if (item.Activo == 0) {
                            Activo = '<div class="badge badge-danger">Inactivo</div>';
                        }

                        //var Botones = '<button class="btn btn-icon btn-primary btnEditar" data-toggle="tooltip"  data-registro="Editar" data-placement="bottom" title="' + $.CargarIdioma.Obtener_Texto('txt_Idioma_Editar') + '" data-original-title="' + $.CargarIdioma.Obtener_Texto('txt_Idioma_Editar') + '"><i class="far fa-edit"></i></button>';
                        $("#tblUsers").find("tbody").append(
                            $('<tr>')
                                .append($('<td data-registro="id_user" style="display:none">').append(item.id_user))
                                .append($('<td>').append(item.RowNumber))
                                .append($('<td>').append(item.cwid))
                                .append($('<td>').append(item.fullname))
                                .append($('<td>').append(item.email))
                                .append($('<td>').append(item.department))
                                .append($('<td>').append(Activo))
                                .append($('<td>').append(Botones))
                        );
                    }
                });
            }).catch(err => console.error(err));
    }
    function fn_get_lists() {
        $("#slcUser_Rol").generarLista({ URL: "/Rol/get_roles_list" });
    }
    function fn_insert_user(Param) {
        var frmDatos = new FormData();
        frmDatos.append("ID", $("#txtUsuariosN_ID").val());
        frmDatos.append("CWID", $("#txtUsuariosN_CWID").val());
        frmDatos.append("Nombre", $("#txtUsuariosN_Nombre").val());
        frmDatos.append("App", $("#txtUsuariosN_App").val());
        frmDatos.append("Correo", $("#txtUsuariosN_Correo").val());
        frmDatos.append("Rol", $("#slcUsuariosN_Rol option:selected").val());
        frmDatos.append("Departamento", $("#slcUsuariosN_Departamento option:selected").val());
        if ($("#cbxUsuariosN_Activo").prop("checked")) {
            frmDatos.append("Activo", 1);
        } else {
            frmDatos.append("Activo", 0);
        }
        //Datos Firma
        frmDatos.append("BYTOST", Param.BYTOST);
        frmDatos.append("ZNACKA", Param.ZNACKA);
        $("#btnFirmaElectronica_Firmar").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/Usuarios/guardarUsuario",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Tipo == "success") {
                    $("#mdlSistema_FirmaElectronica").modal("hide");
                    $("#mdlUsuarios_Agregar").modal("hide");
                    $.auxFormulario.limpiarCampos({ Seccion: $("#frmUsuariosN") });
                }
                fn_Usuarios();
                $("#btnFirmaElectronica_Firmar").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Mensaje, Tipo: res.Tipo });
            },
            error: function (error) {
                $("#mdlSistema_FirmaElectronica").modal("hide");
                $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Informacion_guardar_error'), Tipo: "danger", Error: error });
            }
        });
    }
    function fn_actualizar_usuario(Param) {
        var frmDatos = new FormData();
        frmDatos.append("ID", $("#txtUsuariosM_ID").val());
        frmDatos.append("CWID", $("#txtUsuariosM_CWID").val());
        frmDatos.append("Nombre", $("#txtUsuariosM_Nombre").val());
        frmDatos.append("App", $("#txtUsuariosM_App").val());
        frmDatos.append("Correo", $("#txtUsuariosM_Correo").val());
        frmDatos.append("Rol", $("#slcUsuariosM_Rol option:selected").val());
        frmDatos.append("Departamento", $("#slcUsuariosM_Departamento option:selected").val());
        if ($("#cbxUsuariosM_Activo").prop("checked")) {
            frmDatos.append("Activo", 1);
        } else {
            frmDatos.append("Activo", 0);
        }
        //Datos Firma
        frmDatos.append("BYTOST", Param.BYTOST);
        frmDatos.append("ZNACKA", Param.ZNACKA);
        frmDatos.append("ZMYSEL", Param.ZMYSEL);
        $("#btnFirmaElectronica_Firmar").addClass("btn-progress");
        $.ajax({
            type: "POST",
            url: "/Usuarios/actualizarUsuario",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Tipo == "success") {
                    $("#mdlSistema_FirmaElectronica").modal("hide");
                    $.auxFormulario.limpiarCampos({ Seccion: $("#frmUsuariosM") });
                }
                fn_Usuarios();
                $("#mdlUsuarios_Modificar").modal("hide");
                $("#btnFirmaElectronica_Firmar").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Mensaje, Tipo: res.Tipo, Error: res.Error });
            },
            error: function (error) {
                $("#mdlSistema_FirmaElectronica").modal("hide");
                $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Informacion_guardar_error'), Tipo: "danger", Error: error });
            }
        });
    }
});