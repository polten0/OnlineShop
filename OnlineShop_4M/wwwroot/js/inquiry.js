$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    $('#tblData').DataTable({
        "ajax": {
            "url": "/inquiry/GetInquiryList"
        },
        "columns": [
            { "data": "id" },
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "email" },
            {
                "data": "id",
                "render": (id) => `<a href="/inquiry/details/${id}" class="btn btn-success">GO</a>`
            }
        ]
    });
}