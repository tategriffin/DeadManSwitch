//wire up list to post when re-arranged.
//the "key" value in "data:" must match the parameter name in the controller (they're both stepOrder)
//"traditional: true" is important
$(function () {

    $("#escalationsteps").sortable({
        handle: '.gripper',
        update: function (event, ui) {
            var order = [];
            $('#escalationsteps').children('li').each(function (idx, elm) {
                order.push(elm.id.split('_')[1]);
            });

            $.ajax({
                type: "POST",
                url: "Action/Reorder/",
                data: { stepOrder: order },
                traditional: true,
                context: document.body,
                success: function (result) {
                    if (result.redirectUrl) {
                        window.location.href = result.redirectUrl;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    bootbox.alert("Reorder failed.");
                }
            });
        }
    });
    $("#escalationsteps").disableSelection();
});
