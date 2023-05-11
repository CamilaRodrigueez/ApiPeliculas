var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblCategorias').DataTable({

        "ajax": {
            "url": "/Categorys/GetAllCategorys",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            { "data": "nombre", "width": "40%" },
            { "data": "fechaCrecion", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Categorys/Update/${data}" class="btn btn-warning text-white" style="cursor-pointer;"><i class="fa-solid fa-pen-to-square"></i> Editar</a>
                            &nbsp;
                             <a onclick=Delete("/Categorys/Delete/${data}") class="btn btn-danger text-white" id="btnDelete" style="cursor-pointer;"><i class="fa-solid fa-trash-can"></i> Borrar</a>
                            </div>`;
                }, "width": "20%"
            }
        ]
    });
}

