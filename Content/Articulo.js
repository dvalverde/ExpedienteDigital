
$(document).ready(function () {

    var i2 = 0;

    //function for the select in the Libros section
    //function for the select in the Libros section
    $('#selectArticulos').on('click', function () {


        if ($(this).val() == "") {
            $('#tab_logic_articulo')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_articulo')[0].style.display = '';
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

