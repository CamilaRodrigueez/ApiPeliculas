
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblPeliculas').DataTable({

        "ajax": {
            "url": "/Peliculas/GetAllPeliculas",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id" },
            { "data": "nombre" },
            { "data": "descripcion"  },
            { "data": "clasificacion"  },
            { "data": "duracion" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Peliculas/Update/${data}" class="btn btn-warning text-white" style="cursor-pointer;"><i class="fa-solid fa-pen-to-square"></i> Editar</a>
                            &nbsp;
                             <a onclick=Delete("/Peliculas/Delete/${data}") class="btn btn-danger text-white" id="btnDelete" style="cursor-pointer;"><i class="fa-solid fa-trash-can"></i> Borrar</a>
                            </div>`;
                }, "width": "20%"
            }
        ]
    });
}
