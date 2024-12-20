(function () {
'use strict'
const forms = document.querySelectorAll('.requires-validation')
Array.from(forms)
  .forEach(function (form) {
    form.addEventListener('submit', function (event) {
      if (!form.checkValidity()) {
        event.preventDefault()
        event.stopPropagation()
      }

      form.classList.add('was-validated')
    }, false)
  })
})()

function createSelect(id, /*size*/) {
    $(id).attr('data-live-search', true);
    $(id).attr('data-size', '10');
    $(id).attr('data-width', '100%');
    //$(id).attr('data-live-search-style', 'contains');
    $(id).attr('data-style', 'btn-info');
    //$(id).css('width', size);
    $(id).selectpicker();
}

function getVirtDir() {
    var Path = location.host;
    var VirtualDirectory;
    if (Path.indexOf("localhost") >= 0 && Path.indexOf(":") >= 0) {
        VirtualDirectory = "";

    }
    else {
        var pathname = window.location.pathname;
        var VirtualDir = pathname.split('/');
        VirtualDirectory = VirtualDir[1];
        VirtualDirectory = '/' + VirtualDirectory;
    }
    return VirtualDirectory;
}
function getModels() {
    //block();
    //blockV2("Cargando información de modelos...");
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getModels.ashx",
        success: function (data) {
            r = jQuery.parseJSON(data);
            if (r.result === "true") {

                $("#selModel").html(r.html);
                //$("#selModel").selectize({
                //    sortField: 'text'
                //});
                createSelect("#selModel");
            }
            else
                alert('No se pudieron obtener los modelos.');
            //removeOverlay();
            //unblock();
            return false;
        },
        error: function () { }
    })
}

    function processBox() {
        //block();
        //blockV2("Cargando información de modelos...");
        //alert("Process");
        $("#form").hide("fade");
        $("#message").html("<div class='alert alert-warning' role='alert' style='background-color:gray;padding:0px;'>"+
                    "<div id='loader' style='margin-left:auto;margin-right:auto;'>" + 
                    "    <div class='loader loader-17'>" +
                    "        <div class='css-square square1'></div>" +
                    "        <div class='css-square square2'></div>" +
                    "        <div class='css-square square3'></div>" +
                    "        <div class='css-square square4'></div>" +
                    "        <div class='css-square square5'></div>" +
                    "        <div class='css-square square6'></div>" +
                    "        <div class='css-square square7'></div>" +
                    "        <div class='css-square square8'></div>" +
                    "    </div>"+
                    "</div>"+
                "</div>");
        $("#message").show("fade");
        
        $.ajax({
            method: "POST",
            url: getVirtDir() + "/Controllers/validateBox.ashx",
            data: {
                model_name: $("#selModel option:selected").text(),
                box_serial: $("#boxSerial").val(),
                pcb_serial: $("#boxPcb").val(),
                user: $("#user").val()
            },
            //async: false,
            success: function (data) {
                r = jQuery.parseJSON(data);
                //alert(r.Message);
                if (r.result === "true") {
                    $.ajax({
                        method: "POST",
                        url: getVirtDir() + "/Controllers/insertBox.ashx",
                        data: {
                            BOX_BARCODE: r.BOX_BARCODE,
                            BOX_NAME: r.BOX_NAME,
                            BOX_NUMBER: r.BOX_NUMBER,
                            BOX_QUANTITY: r.BOX_QUANTITY,
                            MODEL_NAME: r.MODEL_NAME,
                            BOX_INTERNAL_PN: r.BOX_INTERNAL_PN,
                            BOX_CLIENT_PN: r.BOX_CLIENT_PN,
                            BIN: r.BIN,
                            DATEE: r.DATEE,
                            STPACK: r.STPACK
                        },
                        async: false,
                        success: function (data) {
                            r = jQuery.parseJSON(data);
                            //alert(r.Message);
                            if (r.result === "true") {
                                $("#boxSerial").val("");
                                $("#boxPcb").val("");
                                $("#user").val("");
                                $("#boxSerial").focus();
                                $("#message").html("<div class='alert alert-success' role='alert' style='background-color:green;color:white;'> " + "Se libero la caja" + "</div>");
                            }
                            if (r.result === "false") {
                                //$("#selModel option:selected").text();
                                $("#boxSerial").val("");
                                $("#boxPcb").val("");
                                $("#user").val("");
                                $("#boxSerial").focus();
                                //alert(r.Message);
                                if (r.Message === "CAJA DUPLICADA")
                                    $("#message").html("<div class='alert alert-danger parpadea' role='alert'> " + r.Message + "</div>");
                                else
                                    $("#message").html("<div class='alert alert-danger' role='alert'> " + r.Message + "</div>");
                            }
                            //removeOverlay();
                            //unblock();
                        },
                        error: function () { }
                    });
                }
                else {
                    //$("#selModel option:selected").text();
                    $("#boxSerial").val("");
                    $("#boxPcb").val("");
                    $("#user").val("");
                    $("#boxSerial").focus();
                    if (r.Message === "N0US3RF0UND")
                        $("#message").html("<div class='alert alert-danger' role='alert' style='background-color:red;color:white;'>NO SE A CAPTURADO NADA.</div>");
                    else
                        if (r.Message === "CAJA DUPLICADA")
                            $("#message").html("<div class='alert alert-danger parpadea' role='alert' style='background-color:red;color:white;letter-spacing: 10px;font-weight: bold; font-size:20px;'> " + r.Message + "</div>");
                        else
                            $("#message").html("<div class='alert alert-danger' role='alert' style='background-color:red;color:white;'> " + r.Message + "</div>");
                }
                //removeOverlay();
                //unblock();
                $("#form").show("fade");
            },
            error: function () { }
        });
        //$("#message").hide("fade");
        //$("#form").show("fade");

        return false;
}

