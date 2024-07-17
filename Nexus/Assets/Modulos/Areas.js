﻿define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_areas();
        $("#btnDepartamentos_Limpiar").click(function () {
            $.auxFormulario.limpiarCampos({
                Seccion: $("#frmDepartamentos")
            });
            fn_get_areas();
        });
        $("#cbxDepartamentosN_Activo").click(function () {
            if ($("#cbxDepartamentosN_Activo").prop("checked")) {
                $("label[for='cbxDepartamentosN_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Activado'));
            } else {
                $("label[for='cbxDepartamentosN_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Desactivado'));
            }
        });
        $("#cbxDepartamentosM_Activo").click(function () {
            if ($("#cbxDepartamentosM_Activo").prop("checked")) {
                $("label[for='cbxDepartamentosM_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Activado'));
            } else {
                $("label[for='cbxDepartamentosM_Activo']").html($.CargarIdioma.Obtener_Texto('txt_Idioma_Desactivado'));
            }
        });
        $("#btnDepartamentosN_Guardar").click(function () {
            $.auxFormulario.camposVacios({
                Seccion: $("#frmDepartamentosN"),
                NoVacio: function () {
                    $.firmaElectronica.MostrarFirma({
                        Funcion: fn_registrar_departamento
                    });
                }
            });
        });
        $("#btnDepartamentosM_Guardar").click(function () {
            $.auxFormulario.camposVacios({
                Seccion: $("#frmDepartamentosM"),
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
                                Funcion: fn_actualizar_departamento
                            });
                        }
                    });
                }
            });
        });
        $("#btnDepartamentos_Buscar").click(function () {
            fn_get_areas();
        })
        $("#tblAreas tbody").on('click', "a[data-registro=Editar]", function () {
            var ID = $(this).parents("tr").find("[data-registro=ID]").html();
            $.post("/Areas/get_areas_info", { ID_area: ID }).done(function (res) {
                if (res != "") {
                    $.each(res, function (i, item) {
                        $("#txtU_Area_ID").val(item.ID);
                        $("#txtU_Area_Name").val(item.Name);
                        $("#txtU_Area_FL").val(item.FL);
                        $("#txtU_Area_Plant").val(item.Plant);
                        if (item.Active == 1) {
                            $("#txtU_Area_Status").prop("checked", true);
                            $("label[for='txtU_Area_Status']").html('Active');
                        } else {
                            $("#txtU_Area_Status").prop("checked", false);
                            $("label[for='txtU_Area_Status']").html('Inactive');
                        }
                    });
                    $("#mdlAreas_update").modal("show");
                }
            }).fail(function (error) { $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Mostrar_informacion_error'), Tipo: "danger", Error: error }); });
        });
        $("#pgdDepartamentos").paginado({
            Tabla: $("#tblAreas"),
            Version: 2,
            Funcion: fn_get_areas
        });
    });
    function fn_iniDepartamentos() {
        fn_get_areas();
        $.matrizAccesos.verificaAcceso({ Elemento: $("#btnLineas_Agregar"), Url: "/Rol/verificarAcceso", FuncionId: 8 });
    }
    function fn_get_areas(Pagina) {
        var Name = $("#txtAreas_Name").val();
        var FL = $("#txtAreas_FL").val();
        var Plant = $("#txtAreas_Plant").val();
        var Active = $("#slcAreas_is_enable option:selected").val();
        if (Name == "") { Name = null; }
        if (FL == "") { FL = null; }
        if (Plant == "") { Plant = null; }
        var Datos = { Name: Name, FL: FL, Plant: Plant, is_enable: Active == -1 ? null : Active, Index: Pagina };
        var accesoEditar = "";
        $.matrizAccesos.validaAcceso({ FuncionId: 9 })
            .then(obj => {
                if (obj.result > 0) {
                    accesoEditar = '<a href="javascript:void(0)" class="dropdown-item" data-registro="Editar"><i class="dropdown-icon fas fa-edit"></i>Edit</a>';
                }

                var Botones = '<div class="item-action dropdown"><a href="javascript:void(0)" data-toggle="dropdown" class="icon"  title="Options" data-original-title="Options"><i class="fas fa-bars" style="z-index:-99 !important;"></i></a>' +
                    '<div class="dropdown-menu dropdown-menu-right">' +
                    accesoEditar +
                    '</div></div>'; 7
                $.mostrarInfo({
                    URLindex: "/Areas/get_TotalPag_Areas",
                    URLdatos: "/Areas/get_Table_Areas",
                    Datos: Datos,
                    Version: 2,
                    Tabla: $("#tblAreas"),
                    Paginado: $("#pgdAreas"),
                    Mostrar: function (i, item) {

                        var Activo = '<span class="badge text-bg-success">Active</span>';
                        if (item.Activo == 0) {
                            Activo = '<span class="badge text-bg-danger">Inactive</span>';
                        }
                        $("#tblAreas").find("tbody").append(
                            $('<tr>')
                                .append($('<td data-registro="ID" style="display:none">').append(item.ID))
                                .append($('<td>').append(item.RowNumber))
                                .append($('<td>').append(item.Name))
                                .append($('<td>').append(item.FL))
                                .append($('<td>').append(item.Plant))
                                .append($('<td>').append(Activo))
                                .append($('<td>').append(Botones))
                        );
                    }
                });
            }).catch(err => console.log(err));
    }
    function fn_registrar_departamento(Param) {
        var frmDatos = new FormData();
        frmDatos.append("ID", $("#txtDepartamentosN_ID").val());
        frmDatos.append("Nombre", $("#txtDepartamentosN_Nombre").val());
        if ($("#cbxDepartamentosN_Activo").prop("checked")) {
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
            url: "/Departamento/guardarDepartamento",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Tipo == "success") {
                    $("#mdlSistema_FirmaElectronica").modal("hide");
                    $.auxFormulario.limpiarCampos({ Seccion: $("#frmDepartamentosN") });
                }
                $("#mdlDepartamentos_Agregar").modal("hide");
                fn_Departamentos();
                $("#btnFirmaElectronica_Firmar").removeClass("btn-progress");
                $.notiMsj.Notificacion({ Mensaje: res.Mensaje, Tipo: res.Tipo });
            },
            error: function (error) {
                $("#mdlSistema_FirmaElectronica").modal("hide");
                $.notiMsj.Notificacion({ Mensaje: $.CargarIdioma.Obtener_Texto('txt_Idioma_Informacion_guardar_error'), Tipo: "danger", Error: error });
            }
        });
    }
    function fn_actualizar_departamento(Param) {
        var frmDatos = new FormData();
        frmDatos.append("ID", $("#txtDepartamentosM_ID").val());
        frmDatos.append("Nombre", $("#txtDepartamentosM_Nombre").val());
        if ($("#cbxDepartamentosM_Activo").prop("checked")) {
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
            url: "/Departamento/actualizarDepartamento",
            contentType: false,
            processData: false,
            data: frmDatos,
            success: function (res) {
                if (res.Tipo == "success") {
                    $("#mdlSistema_FirmaElectronica").modal("hide");
                    $.auxFormulario.limpiarCampos({ Seccion: $("#frmDepartamentosM") });
                }
                $("#mdlDepartamentos_Modificar").modal("hide");
                fn_Departamentos();
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