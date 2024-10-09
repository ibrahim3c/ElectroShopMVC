var dtable;
$(document).ready(function () {
    loadData();
});

function loadData() {
    dtable = $("#order-table").DataTable({ // Correct method is DataTable, not dataTalb
        "ajax": {
            url: "/Admin/Orders/GetOrders", // Use a comma, not a semicolon
            type: "GET", // Optional, but makes the method explicit
           
        },
        "columns": [ // Correct spelling of "columns"
            { "data": "name" },
            { "data": "orderStatus" },
            { "data": "orderTotal" },
            { "data": "phoneNumber" },
            { "data": "email" },
            {
                "data": "orderID",
                "render": function (data) {
                    return `
                        <a href="/Admin/Orders/Details/${data}" class="btn btn-success btn-sm me-2">
                            <i class="fas fa-edit"></i> Details
                        </a>
                        `;
                      
                }
            }
        ]
    });

}
