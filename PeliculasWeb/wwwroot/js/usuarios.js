$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsuarios').DataTable({

        "ajax": {
            "url": "/Usuarios/GetAllUsuarios",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id"},
            { "data": "userName"},
            { "data": "passwordHash" } 
        ]
    });
}
