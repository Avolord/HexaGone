//Kais functions

let canvas = document.getElementById('hexmap');
let ctx = canvas.getContext('2d');
ctx.fillStyle = 'red';
ctx.font = '20px Arial';

canvas.width = window.innerWidth;
canvas.height = window.innerHeight;

console.log(canvas.width, canvas.height);

canvas.addEventListener('mousemove', function (event) {
    myFunction(event);
});

function myFunction(e) {
    var x = e.clientX;
    var y = e.clientY;
    var coor = 'Coordinates: (' + x + ',' + y + ')';
    $('#info_bar_global_resources div#gold span.info_bar_data_value').html('x: ' + x);
    $('#info_bar_global_resources div#silver span.info_bar_data_value').html('y: ' + y);

    // ctx.fillText('Banane', x, y);
    // ctx.fillRect(x, y, 10, 10);
}

$('#test_button').on('click', function () {
    //$('.stat_bar').fadeToggle('fast');
    $('#info_bar_local_resources').slideToggle();
    info_bar_local_resources_active = !info_bar_local_resources_active;
});

let user_is_ready = false;

$('.game_ready_button').on('click', function () {
    if (user_is_ready) {
        $(this).removeClass('btn-success');
        $(this).addClass('btn-danger');
        $('.game_ready_button h2').html('Not Ready!');
    } else {
        $(this).removeClass('btn-danger');
        $(this).addClass('btn-success');
        $('.game_ready_button h2').html('Ready!');
    }

    user_is_ready = !user_is_ready;
});

$('.action_list_delete_button').on('click', function () {
    $(this).parent().remove();
});

$(function () {
    $('#info_bar_local_resources').slideUp(0);
    info_bar_local_resources_active = false;
    $('.burger_menu_container').addClass('toggle_burger_menu');
});

let dragged;
let id;
let index;
let indexDrop;
let list;

$('.action_list').on('dragstart', ({ target }) => {
    dragged = target;
    id = target.id;
    list = target.parentNode.children;
    for (let i = 0; i < list.length; i += 1) {
        if (list[i] === dragged) {
            index = i;
        }
    }
});

$('.action_list').on('dragover', (event) => {
    event.preventDefault();
});

$('.action_list').on('drag', (event) => { });

$('.action_list').on('drop', ({ target }) => {
    if ($(target).hasClass('action_list_item') && target.id !== id) {
        dragged.remove(dragged);
        for (let i = 0; i < list.length; i += 1) {
            if (list[i] === target) {
                indexDrop = i;
            }
        }
        //console.log(index, indexDrop);
        if (index > indexDrop) {
            target.before(dragged);
        } else {
            target.after(dragged);
        }
    }
});

let menu_bar_active = true;
let menu_bar_height_max = 0;

function toggle_menu_bar(burger_menu) {
    console.log(1);
    $(burger_menu).toggleClass('toggle_burger_menu');

    if (menu_bar_active) {
        menu_bar_height_max = $('.menu_bar').height();

        $('.menu_bar_header div.menu_bar_title').css('visibility', 'hidden');
        $('.menu_bar').animate({ height: '50px', width: '80px', borderBottomRightRadius: '25px' }, 500, function () {
            $('.menu_bar').children().hide();
            $('.menu_bar_header div.menu_bar_title').hide();
            $('.menu_bar_header').show();
        });
    } else {
        $('.menu_bar').children().show();
        $('.menu_bar_header div.menu_bar_title').show();
        $('.menu_bar_header div.menu_bar_title').css('visibility', 'visible');
        $('.menu_bar').animate(
            { height: menu_bar_height_max, width: '25%', borderBottomRightRadius: '0px' },
            500,
            function () {
                $(this).height('auto');
            }
        );
    }

    menu_bar_active = !menu_bar_active;
}