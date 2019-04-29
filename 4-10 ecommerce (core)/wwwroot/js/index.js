$(() => {
    $.get("/home/getproducts", { id: 11 }, function (products) {
        $("#prod-row").empty();
        products.forEach(product => populateRow(product));
    });

    const populateRow = product => {
        let shorter = "";
        if (product.description.length > 100) {
            for (let i = 0; i < 100; i++) {
                shorter += (product.description[i]);
            };
            shorter += "...";
        } else {
            shorter = product.description;
        }

        $("#prod-row").append(`<div class="col-sm-4 col-lg-4 col-md-4">
                    <div class="thumbnail">
                        <img  style="width: 300px; height:250px " src="/productImages/${product.image}" alt="">
                            <div class="caption">
                                <h4 class="pull-right">${product.price}</h4>
                                <h4>
                                    <a href="/home/productpage?id=${product.id}">${product.name}</a>
                                </h4>
                                <p>${shorter}</p>
                            </div>
                            </div>
                    </div>`)
    }

    $(".category").on('click', function () {
        const id = $(this).data('id');
        $.get("/home/getproducts", { id }, function (products) {
            $("#prod-row").empty();
            products.forEach(product => populateRow(product));
        });
    });
});