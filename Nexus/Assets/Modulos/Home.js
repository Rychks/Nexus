define(["jquery"], function ($) {
    $(document).ready(function () {
        fn_get_dashboard_list()
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
                    + '<div class="form-group">'
                    + ' <iframe style="display:block; height: 100vh;width: 100vw;border:none;" title="ScoreCard"  src="' + link_dashboard +'"&navContentPaneEnabled=false&filterPaneEnabled=false" frameborder="0" allowFullScreen="true"></iframe>'
                    + '</div>'
                    + '</div>'
                );
            } else {
                $("#dashboard_modal_panel").append(
                    '<div class="col-lg-12">'
                    + '<div class="form-group">'
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
    });

    function fn_get_dashboard_list() {
        var url = "/Settings/get_dashboard_list"
        $.post(url).done(function (data) {
            console.log(data)
            $.each(data, function (i, item) {
                $("#dashboars_panel").append(
                    '<div class="col-lg-3 zoom" style="margin-top:15px;">'
                    +'<div class="card preview-dash zoom" style="width: 25rem;">'
                      +'<div class="card-body">'
                            +'<h6 class="card-title">'+item.title+'</h5>'
                            +'<p class="card-text"></p>'
                        +'</div>'
                    + '<img class="card-img-bottom mx-auto d-block" src="/Assets/img/Dashboards/' + item.previus_image + '" data-registro="' + item.type + '" name="' + item.link + '" style="width:100%; height:100%"  alt="Card image cap">'
                    +'</div>'
                    + '</div>'
                    
                    //+'<div class="form-group">'
                    //+ '<img src="/Assets/img/Dashboards/' + item.previus_image + '" class="dashboard_previius_img img-fluid" data-registro="' + item.type +'" name="' + item.link + '"  alt="">'
                    ////+ '<input value=""></p>'
                    ////+ '<h5>Heading</h5 >'
                    ////+ '<p>'
                    ////+ 'Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.'
                    ////+ '</p>'
                    ////+ '</div>'
                    //+ '</div>'
                    //+'</div>'
                    ////'<div class="col-lg-4">'
                    ////+ '<div class="form-group">'
                    ////+ '<script type="module" src="https://ncsa.tableau.intranet.cnb/javascripts/api/tableau.embedding.3.latest.min.js"></script>'
                    ////+ '<tableau-viz id="tableauViz" device="desktop" src="'+item.link+'" toolbar="bottom" hide-tabs>'
                    ////+ '</tableau-viz>'
                    ////+ '</div>'
                    ////+ '</div>'
                )
            })
        }).fail(function (e) {
            console.log(e);
        })
    }
});