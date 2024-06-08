$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/order/GetOrderList"
        },
        "columns": [
            { "data": "id" },
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "email" },
            { "data": "city" },
            { "data": "orderDate"},
            { "data": "orderTotal" },
            { "data": "orderStatus" },
            {
                "data": "id",
                "render": (id) => `<a href="/order/details/${id}" class="btn btn-success">GO</a>`
            }
        ]
    });
}