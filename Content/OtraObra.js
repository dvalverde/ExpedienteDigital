
$(document).ready(function () {

    var i2 = 0;

    //function for the select in the Libros section
    //function for the select in the Libros section
    $('#selectOtraObra').on('click', function () {


        if ($(this).val() == "") {
            $('#tab_logic_otra_obra')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_otra_obra')[0].style.display = '';
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

