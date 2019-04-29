$(() => {
    $("#add-to-cart").on('click', function () {
        const cartItem = {
            productId: $("#product-id").val(),
            quantity: $("#quantity").val(),
        }

        $.post("/home/addtocart", cartItem, function (added) {
            console.log('done');
            if (!added) {
                $("#already-added").modal('show');
                console.log('already');
            } else {
                $("#myModal").modal('show');
                console.log('add');
            }
        });
    });



});