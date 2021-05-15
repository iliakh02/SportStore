'use strict';

$('#menu-toggle').click(function (e) {
    e.preventDefault();
    $('#wrapper').toggleClass('toggled');
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


// Request to change the amount of product into a cart.
$('.cart-item-form').on('submit', function (e) {
    e.preventDefault();

    let formAction = $(this).attr('action');
    
    let itemId = $(this).children('.item-id').attr('value');

    let isValid = true;
    if (formAction.includes('Decrease')) {
        let amount = $(this).parent().find('#cart-item-amount-' + itemId).text();

        isValid = (+amount > 1);
    }
    if (isValid) {
        $.ajax({
            type: 'POST',
            url: formAction,
            data: { id: itemId },
            success: function (responce) {
                $('#cart-item-amount-' + responce['id']).html(responce['amount']);

                $('#product-total-price-' + responce['id']).html(responce['price'].toFixed(2).replace('.', ',') + ' UAN');

                let oldTotalPrice = $('#total-price-value').text().replace(',', '.');
                let different = responce['diffInTotalPrice'];
                let newTotalPrice = +oldTotalPrice + different;
                $('#total-price-value').html(newTotalPrice.toFixed(2).replace('.', ','));

                let cartSize = $('#cart-size').text();
                if (formAction.includes('Decrease')) {
                    cartSize = +cartSize - 1;
                } else {
                    cartSize = +cartSize + 1;
                }
                $('#cart-size').html(cartSize);
            },
            failure: function (response) {
                alert(response);
            },
            error: function (response) {
                alert(response);
            }
        });
    }
});


window.onscroll = function () { myFunction() };

var header = $('#total-order-info');

// Add the sticky class to the header when you reach it's scroll position. Remove 'sticky' when you leave the scroll position.
function myFunction() {
    if (window.pageYOffset > 62) {
        header.addClass('sticky');
        var container = $('.container')[0];
    } else {
        header.removeClass('sticky');
    }
}


// Update order paid's status.
$('.is-payment-order').on('change', function (e) {
    e.preventDefault();

    let orderId = $(this).parent().children('.order-number').attr('value');

    $.ajax('/Orders/UpdatePaymentStatus', {
        type: 'POST',
        dataType: 'json',
        data: { id: orderId },
        success: function (response) {
            $('#payment-status-' + orderId).html((response['isPaid']) ? 'Paid' : 'Not paid');
        },
        error: function (data, status) {
            console.log(data, status);
        }
    });
});
