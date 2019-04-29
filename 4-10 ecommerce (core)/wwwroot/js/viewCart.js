$(() => {

    const fillCart = () => {
        $("#myCart").empty();
        $.get('/home/getcartitems', function (products) {
            products.forEach(product => $("#myCart").append(`<div class="col-sm-3 col-lg-3 col-md-3">
            <div class="thumbnail">
                <img style="width: 200px; height:150px " src="/productImages/${product.image}" alt="">
                <div class="caption">
                    <h4 class="pull-right">$${product.price}.00</h4>

                    <h4>
                        <a href="/home/productpage?id=${product.id}">${product.name}</a >
                    </h4>
                    <input type="text" style="width: 20px" class="pull-right quantity" data-quantity="${product.id}" 
                                value="${ product.quantity }"/>
                    <label class="pull-right">Quantity</label>

                    <p>${product.description}</p >
                </div>
            </div>
            <button  data-id="${product.id}"  class="btn btn-danger remove">Remove</button>
            <button  data-id="${product.id}"  data-quantity="" class="btn btn-success edit">Edit Quantity</button>
        </div>`))

        });
    }

    fillCart();

    $("#myCart").on('click', '.remove', function () {
        const id = $(this).data('id');
        $.post("/home/removefromcart", { id }, function () {
            fillCart();
        });
    });

    $("#myCart").on('click', '.edit', function () {
        const guess = $("#myCart").find(".quantity");
        const id = $(this).data('id');
        const guess2 = $(`[data-quantity="${id}"]`).val();
        const cartItem = {
            productId: $(this).data('id'),
            cartId: $(this).data('quantity'),
            quantity: guess2
        }
        
        console.log(cartItem);
        
        $.post("/home/editquantity", cartItem, function () {
            fillCart();
        });
    });

});