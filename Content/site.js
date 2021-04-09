// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var contadorArticulos = 0;
var contadorDesarrollo = 0;
var contadorObraAdmin = 0;

$(document).ready(function () {

    var i = 0;
    var i2 = 0;
    var i3 = 0;
    


    $('#selectSoftware').on('click', function () {


        if ($(this).val() == "") {
            $('#tab_logic_d')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_d')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }

        }

    });

    var i2 = 0;

    //function for the select in the Libros section
    //function for the select in the Libros section
    $('#selectObra').on('click', function () {


        if ($(this).val() == "") {
            $('#tab_logic_obra')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_obra')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }

        }

    });

    $('#selectObraAdmin').on('click', function () {
        $("#campo").val($(this).val());

        if ($(this).val() == "") {
            $('#tab_logic')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }

        }
    });

    $('#selectObraAdminDesarrollo').on('click', function () {

        if ($(this).val() == "") {
            $('#tab_logic_admin')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_admin')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }
        }
    });

    $('#selectObraArte').on('click', function () {
        if ($(this).val() == "") {
            $('#tab_logic')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }
        }
    });

    


});




var array1 = {};
var array2 = {};
var array3 = {};
var array4 = {};
var array5 = {};
var array6 = {};
var array7 = {};
var array8 = {};
var array9 = {};
var array10 = {};


function recorrerAutoresLibro(contador, row, array) {

    for (var j = 0; j < contador; j++) {
        var string1 = 'nombre' + row + 'autor' + j;
        var nombre = document.getElementById(string1).value;
        array.nombre = nombre;

        var string2 = 'correo' + row + 'autor' + j;
        var correo = document.getElementById(string2).value;
        array.correo = correo;
    }

}

function aceptarAutoresLibro() {

    recorrerAutoresLibro(conta1, 0, array1);
    recorrerAutoresLibro(conta2, 1, array2);
    recorrerAutoresLibro(conta3, 2, array3);
    recorrerAutoresLibro(conta4, 3, array4);
    recorrerAutoresLibro(conta5, 4, array5);
    recorrerAutoresLibro(conta6, 5, array6);
    recorrerAutoresLibro(conta7, 6, array7);
    recorrerAutoresLibro(conta8, 7, array8);
    recorrerAutoresLibro(conta9, 8, array9);
    recorrerAutoresLibro(conta10, 9, array10);

    console.log(array1);
    console.log(array2);
    console.log(array3);
    console.log(array4);
    console.log(array5);
    console.log(array6);
    console.log(array7);
    console.log(array8);
    console.log(array9);
    console.log(array10);


}

function recorrerAutoresArticulo() {
    var array;
    for (var j = 0; j < contadorArticulos; j++) {
        if (j == 0) {
            array = array1;
        }
        if (j == 1) {
            array = array2;
        }
        if (j == 2) {
            array = array3;
        }
        if (j == 3) {
            array = array4;
        }
        if (j == 4) {
            array = array5;
        }
        if (j == 5) {
            array = array6;
        }
        if (j == 6) {
            array = array7;
        }
        if (j == 7) {
            array = array8;
        }
        if (j == 8) {
            array = array9;
        }
        if (j == 9) {
            array = array10;
        }
        var string1 = 'nombreArticulo' + j;
        console.log("string1 " + string1);
        var nombre = document.getElementsByName(string1)[0].value;
        console.log("nombre " + document.getElementsByName(string1)[0].value);
        array.nombre = nombre;

        var string2 = 'mailArticulo' + j;
        console.log("string2 " + string2);
        var correo = document.getElementsByName(string2)[0].value;
        console.log("correo " + document.getElementsByName(string2)[0].value);
        array.correo = correo;

        var string3 = 'distArticulo' + j;
        console.log("string3 " + string3);
        var distribucion = document.getElementsByName(string3)[0].value;
        console.log("distribucion " + document.getElementsByName(string3)[0].value);
        array.distribucion = parseFloat(distribucion);
    }

}



function aceptarAutoresArticulo() {
    array1 = {};
    array2 = {};
    array3 = {};
    array4 = {};
    array5 = {};
    array6 = {};
    array7 = {};
    array8 = {};
    array9 = {};
    array10 = {};
    recorrerAutoresArticulo();

    console.log(array1);
    console.log(array2);
    console.log(array3);
    console.log(array4);
    console.log(array5);
    console.log(array6);
    console.log(array7);
    console.log(array8);
    console.log(array9);
    console.log(array10);

    var data1 = JSON.stringify(array1);
    $.when(saveAutorArticulo(data1)).then(function (response) {
        console.log(response);
    }).fail(function (err) {
        console.log(err);
    });



function saveAutorArticulo(data) {
    return $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: "/Articulo/CreateArticulo",
        data: JSON.stringify({
            nombre: "Andrea",
            correo: "12345",
            distribucion: 0.5
        }),
        success: function (result) {
            alert(result);
            location.reload();
        },
        error: function (err) {
            console.log(err.responseText);
        }
    });
}

function recorrerAutoresDesarrollo() {
    var array;
    for (var j = 0; j < contadorDesarrollo; j++) {
        if (j == 0) {
            array = array1;
        }
        if (j == 1) {
            array = array2;
        }
        if (j == 2) {
            array = array3;
        }
        if (j == 3) {
            array = array4;
        }
        if (j == 4) {
            array = array5;
        }
        if (j == 5) {
            array = array6;
        }
        if (j == 6) {
            array = array7;
        }
        if (j == 7) {
            array = array8;
        }
        if (j == 8) {
            array = array9;
        }
        if (j == 9) {
            array = array10;
        }
        var string1 = 'nombreDesarrollo' + j;
        console.log("string1 " + string1);
        var nombre = document.getElementsByName(string1)[0].value;
        console.log("nombre " + document.getElementsByName(string1)[0].value);
        array.nombre = nombre;

        var string2 = 'mailDesarrollo' + j;
        console.log("string2 " + string2);
        var correo = document.getElementsByName(string2)[0].value;
        console.log("correo " + document.getElementsByName(string2)[0].value);
        array.correo = correo;

        var string3 = 'distDesarrollo' + j;
        console.log("string3 " + string3);
        var distribucion = document.getElementsByName(string3)[0].value;
        console.log("distribucion " + document.getElementsByName(string3)[0].value);
        array.distribucion = distribucion;


    }

}

function aceptarAutoresDesarrollo() {
    array1 = {};
    array2 = {};
    array3 = {};
    array4 = {};
    array5 = {};
    array6 = {};
    array7 = {};
    array8 = {};
    array9 = {};
    array10 = {};
    recorrerAutoresDesarrollo();

    console.log(array1);
    console.log(array2);
    console.log(array3);
    console.log(array4);
    console.log(array5);
    console.log(array6);
    console.log(array7);
    console.log(array8);
    console.log(array9);
    console.log(array10);


}

function recorrerAutoresObraAdmin() {
    var array;
    for (var j = 0; j < contadorObraAdmin; j++) {
        if (j == 0) {
            array = array1;
        }
        if (j == 1) {
            array = array2;
        }
        if (j == 2) {
            array = array3;
        }
        if (j == 3) {
            array = array4;
        }
        if (j == 4) {
            array = array5;
        }
        if (j == 5) {
            array = array6;
        }
        if (j == 6) {
            array = array7;
        }
        if (j == 7) {
            array = array8;
        }
        if (j == 8) {
            array = array9;
        }
        if (j == 9) {
            array = array10;
        }
        var string1 = 'nombreObraAdmin' + j;
        console.log("string1 " + string1);
        var nombre = document.getElementsByName(string1)[0].value;
        console.log("nombre " + document.getElementsByName(string1)[0].value);
        array.nombre = nombre;

        var string2 = 'mailObraAdmin' + j;
        console.log("string2 " + string2);
        var correo = document.getElementsByName(string2)[0].value;
        console.log("correo " + document.getElementsByName(string2)[0].value);
        array.correo = correo;

        var string3 = 'distObraAdmin' + j;
        console.log("string3 " + string3);
        var distribucion = document.getElementsByName(string3)[0].value;
        console.log("distribucion " + document.getElementsByName(string3)[0].value);
        array.distribucion = distribucion;


    }

}

function aceptarAutoresObraAdmin() {
    array1 = {};
    array2 = {};
    array3 = {};
    array4 = {};
    array5 = {};
    array6 = {};
    array7 = {};
    array8 = {};
    array9 = {};
    array10 = {};
    recorrerAutoresObraAdmin();

    console.log(array1);
    console.log(array2);
    console.log(array3);
    console.log(array4);
    console.log(array5);
    console.log(array6);
    console.log(array7);
    console.log(array8);
    console.log(array9);
    console.log(array10);


}
}
