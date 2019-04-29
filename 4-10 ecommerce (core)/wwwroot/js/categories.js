$(() => {
    const populateTable = () => {
        $.get("/managestock/getcategories", function (categories) {
            $("#cat-table").find("tr:gt(0)").remove();
            categories.forEach(category => {
                $("#cat-table").append(`<tr>
                                            <td>${category.name}</td >
                                            <td><a href="/managestock/Products?categoryId=${category.id}" 
                                                    class="btn btn-link products">Products</a></td>
                                            <td><button class="btn btn-success center-block edit"
                                                    data-id="${category.id}" data-name="${category.name}">Edit</button></td>
                                       </tr >`)
            });
        });

    }

    populateTable();

    $("#add-cat").on('click', function () {
        const name = $("#cat-name").val();
        $.post("/managestock/addcategory", { name }, function () {
            populateTable();
        })
        $("#cat-name").val("");
    });

    $("#cat-table").on('click', '.edit', function () {
        $("#edit-modal").modal();
        const id = $(this).data('id');
        $("#cat-update").val($(this).data('name'));

        $("#btn-update").on('click', function () {
            const category = {
                name: $("#cat-update").val(),
                id: id
            };

            console.log(category.name);
            $.post('/managestock/editcategory', { category }, function () {
                $("#edit-modal").modal('toggle');
                populateTable();
            });

        });

        $("#btn-delete").on('click', function () {
            $.post("/managestock/deletecategory", { id }, function () {
                $("#edit-modal").modal('toggle');
                populateTable();
            });
        });
    });

    
});