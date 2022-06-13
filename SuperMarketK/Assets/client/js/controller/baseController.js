﻿let baseController = function () {
    $("#txtKeyword").autocomplete({
        minLength: 0,
        source: function (request, response) {
            $.ajax({
                url: "/Product/ListName",
                dataType: "json",
                data: {
                    q: request.term
                },
                success: function (res) {
                    response(res.data);
                }
            });
        },
        focus: function (event, ui) {
            $("#txtKeyword").val(ui.item.label);
            return false;
        },
        select: function (event, ui) {
            $("#txtKeyword").val(ui.item.label);
            return false;
        }
    })
        .autocomplete().data("uiAutocomplete")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>" + item.label + "</div>")
                .appendTo(ul);
        };
}
/*baseController();*/