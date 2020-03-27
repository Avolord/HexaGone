let main_login_button_y;
let login_form_shown = false;

function checkScroll() {
    var startY = $('.navbar').height() * 2; //The point where the navbar changes in px

    if ($(window).scrollTop() > startY) {
        $('.navbar').addClass("scrolled");
    } else {
        $('.navbar').removeClass("scrolled");
    }

    if ($(window).scrollTop() > main_login_button_y && login_form_shown === false) {
        $('#navbar_login_btn').stop(true, false);
        $('.login-btn').stop(true, false);
        $('#navbar_login_btn').fadeIn("slow");
        $('.login-btn').fadeOut("slow");

    } else if(login_form_shown===false) {
        $('#navbar_login_btn').stop(true, false);
        $('.login-btn').stop(true, false);
        $('#navbar_login_btn').fadeOut("slow");
        $('.login-btn').fadeIn("slow");
    }
}    

$('#btnLogin').on('click', function () {
    login_form_shown = true;
    $('#btnLogin').fadeOut('fast');
    $('#divForm').fadeIn('fast');
});

$(function () {
    main_login_button_y = $('.login-btn').position().top;

    if ($('.navbar').length > 0) {
        $(window).on("scroll load resize", function () {
            checkScroll();
        });
    }
});

function scroll_to_top_and_show_form() {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    login_form_shown = true;
    $('#btnLogin').fadeOut('fast');
    $('#divForm').fadeIn('fast');
    $('#navbar_login_btn').stop(true, false);
    $('#navbar_login_btn').fadeOut("slow");
}

