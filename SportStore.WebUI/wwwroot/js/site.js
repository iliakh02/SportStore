"use strict";

$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $('#product-image')
                .attr('src', e.target.result)
                .width(150)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}


// Request to increase the amount of product into a cart.
$(".increase-cart-item-form").on("submit", function (e) {
    e.preventDefault();

    let formAction = $(this).attr("action");
    
    let itemId = $(this).children(".item-id").attr("value");

    $.ajax({
        type: "POST",
        url: formAction,
        data: { id: itemId },
        success: function (responce) {
            $("#cart-item-amount-" + responce["id"]).html(responce["amount"]);
        },
        failure: function (response) {
            alert(response);
        },
        error: function (response) {
            alert(response);
        }
    });
});


