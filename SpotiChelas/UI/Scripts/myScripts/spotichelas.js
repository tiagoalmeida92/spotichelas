function initDialog(dialogid, buttonclass, elem, url) {
    var dlg = $(dialogid);
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
    $(buttonclass).click(function (e) {
        dlg.dialog("close");
        var id = $(this).closest(elem).data("trackid");
        dlg.load(url+'/'+id, new function(){
            dlg.dialog("open");
        });
        e.preventDefault();
    });
}

function initAddToPlaylists(dropdownclass, url) {
    $(dropdownclass).click(function (e) {
        var playlistId = $(this).data("playlistid");
        var trackId = $(this).closest("td").data("trackid");
        $.post(url, { "playlistid": playlistId, "trackId": trackId });
        e.preventDefault();
    });
}