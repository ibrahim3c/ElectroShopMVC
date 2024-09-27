var dtable;
$(document).ready(function () {
    loadData();
});

function loadData() {
    dtable = $("#product-table").DataTable({ // Correct method is DataTable, not dataTalb
        "ajax": {
            url: "/Admin/Products/GetProducts", // Use a comma, not a semicolon
            type: "GET", // Optional, but makes the method explicit
           
        },
        "columns": [ // Correct spelling of "columns"
            { "data": "name" },
            { "data": "description" },
            { "data": "price" },
            { "data": "category" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/Admin/Products/Edit/${data}" class="btn btn-success btn-sm me-2">
                            <i class="fas fa-edit"></i> Edit
                        </a>

                        <a onclick="DeleteProduct(event)" data-product-id="${data}" class="btn btn-danger btn-sm" style="color:white">
                            <i class="fas fa-trash"></i> Delete
                        </a>`;
                }
            }
        ]
    });

}
