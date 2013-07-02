function initDialog() {
    var dlg = $("#dialog");
    dlg.dialog({
        autoOpen: false,
        width:315,
        buttons: [{
            "text":'x',
            "click": function() {
                $(this).dialog("close");
            },
            "class": 'btn btn-inverse'
        }]

    });
    $(".ui-dialog-titlebar").hide(); //no menu
    $(".spotify-play").click(function (e) {
        dlg.dialog("close");
        var id = $(this).closest("td").data("trackid") || $(this).closest("tr").data("trackid");
        dlg.load("/Play/Track/"+id, new function(){
            dlg.dialog("open");
        });
        e.preventDefault();
    });
}