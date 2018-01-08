$("#simpleMessage").html("Simple Page loaded and its script added this message.");
$("body").on("click", "#simpleButton", function (event) {
    if($.dialog)
        $("<div>Hello World From Simple Script!</div>").dialog({ title: "Simple Demo Plugin Dialog", width: 450, close: function (event, ui) { $(this).dialog("destroy").remove(); } });
    else
        $("<div>Hello World From Simple Script!</div>").alert();
}).on("click", "#SimpleWidgetButton", function () {
    if ($.dialog)
        $("<div>Hello World From Simple Widget!</div>").dialog({ title: "Simple Demo Plugin Dialog", width: 450, close: function (event, ui) { $(this).dialog("destroy").remove(); } });
    else if($(".simplePlg-alert").length == 0)
        $("<div class='alert alert-info simplePlg-alert'><button type='button' class='close' data-dismiss='alert' aria-hidden='true'>&times;</button>Hello World From Simple Widget!</div>").appendTo("body").alert();
});