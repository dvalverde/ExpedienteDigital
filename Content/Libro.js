
$(document).ready(function () {

    var i2 = 0;
  
    //function for the select in the Libros section
    //function for the select in the Libros section
    $('#selectLibros').on('click', function () {
      
        
        if ($(this).val() == "") {
            $('#tab_logic_libro')[0].style.display = 'none';
        } else {
            //desaparecer todas
            $('#tab_logic_libro')[0].style.display = '';
            for (var i = 0; i < 100; i++) {
                var id = "#" + i;
                $(id)[0].style.display = 'none';
            }
            //desaparecer los autores
            for (var j = $(this).val(); j < 10; j++) {
                for (var k = 0; k < 10; k++){
                    $('#cap' + j + 'autor' + k).hide();

                }  
            }
             contAutor1 = 0;
             contAutor2 = 0;
             contAutor3 = 0;
             contAutor4 = 0;
             contAutor5 = 0;
             contAutor6 = 0;
             contAutor7 = 0;
             contAutor8 = 0;
             contAutor9 = 0;
             contAutor10 = 0;
            //aparezco con el select
            for (var i = 0; i < $(this).val(); i++) {
                var id = "#" + i;
                $(id)[0].style.display = '';
            }

        }

    });

});

var contAutor1 = 0;
var contAutor2 = 0;
var contAutor3 = 0;
var contAutor4 = 0;
var contAutor5 = 0;
var contAutor6 = 0;
var contAutor7 = 0;
var contAutor8 = 0;
var contAutor9 = 0;
var contAutor10 = 0;



function addNewOption(value) {
    
/*cap1autor1*/

    console.log(value);
    switch (value) {
        case 0:
            if (contAutor1 <= 10) {
                contAutor1++;
                $('#cap1autor' + contAutor1).show();
                document.getElementById('cap1autor' + contAutor1).required = true;
            }
            break;
        case 1:
            if (contAutor2 <= 10) {
                contAutor2++;
                $('#cap2autor' + contAutor2).show();
            }
            break;
        case 2:
            if (contAutor3 <= 10) {
                contAutor3++;
                $('#cap3autor' + contAutor3).show();
            }
            break;
        case 3:
            if (contAutor4 <= 10) {
                contAutor4++;
                $('#cap4autor' + contAutor4).show();
            }
            break;
        case 4:
            if (contAutor5 <= 10) {
                contAutor5++;
                $('#cap5autor' + contAutor5).show();
            }
            break;
        case 5:
            if (contAutor6 <= 10) {
                contAutor6++;
                $('#cap6autor' + contAutor6).show();
            }
            break;
        case 6:
            if (contAutor7 <= 10) {
                contAutor7++;
                $('#cap7autor' + contAutor7).show();
            }
            break;
        case 7:
            if (contAutor8 <= 10) {
                contAutor8++;
                $('#cap8autor' + contAutor8).show();
            }
            break;
        case 8:
            if (contAutor9 <= 10) {
                contAutor9++;
                $('#cap9autor' + contAutor9).show();
            }
            break;
        case 9:
            if (contAutor10 <= 10) {
                contAutor10++;
                $('#cap10autor' + contAutor10).show();
            }
            break;
    }     
}

function removeOption(value) {
    
    switch (value) {
        case 0:
            if (contAutor1 > 0) {
                $('#cap1autor' + contAutor1).hide();
                contAutor1--;
            }
            break;
        case 1:
            if (contAutor2 > 0) {
                $('#cap2autor' + contAutor2).hide();
                contAutor2--;
            }
            break;
        case 2:
            if (contAutor3 > 0) {
                $('#cap3autor' + contAutor3).hide();
                contAutor3--;
            }
            break;
        case 3:
            if (contAutor4 > 0) {
                $('#cap4autor' + contAutor4).hide();
                contAutor4--;
            }
            break;
        case 4:
            if (contAutor5 > 0) {
                $('#cap5autor' + contAutor5).hide();
                contAutor5--;
            }
            break;
        case 5:
            if (contAutor6 > 0) {
                $('#cap6autor' + contAutor6).hide();
                contAutor6--;
            }
            break;
        case 6:
            if (contAutor7 > 0) {
                $('#cap7autor' + contAutor7).hide();
                contAutor7--;
            }
            break;
        case 7:
            if (contAutor8 > 0) {
                $('#cap8autor' + contAutor8).hide();
                contAutor8--;
            }
            break;
        case 8:
            if (contAutor9 > 0) {
                $('#cap9autor' + contAutor9).hide();
                contAutor9--;
            }
            break;
        case 9:
            if (contAutor10 > 0) {
                $('#cap10autor' + contAutor10).hide();
                contAutor10--;
            }
            break;
    }

    

}

