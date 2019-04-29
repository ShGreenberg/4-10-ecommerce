$(() => {

    $("#products-row").on('click', '.product', function () {
        console.log('hi');
        $("#mod-name").val($(this).data("name"));
        $("#mod-price").val($(this).data("price"));
        $("#mod-desc").val($(this).data("desc"));
        $("#mod-id").val($(this).data("id"));
        $("#mod-cat-id").val($(this).data("cat-id"));
        $("#mod-image").attr('src', `/productImages/${$(this).data('pic')}`);
        const image = $(this).data('pic');
        $("#mod-image-holder").val(image);

    });

    $("#try-button").on('click', function () {
        console.log('in try-button');
    });



});