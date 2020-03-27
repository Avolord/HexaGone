let main_login_button_y;

function checkScroll() {
    var startY = $('.navbar').height() * 2; //The point where the navbar changes in px

    if ($(window).scrollTop() > startY) {
        $('.navbar').addClass("scrolled");
    } else {
        $('.navbar').removeClass("scrolled");
    }

    if ($(window).scrollTop() > main_login_button_y) {
        $('#navbar_login_btn').clearQueue();
        $('.login-btn').clearQueue();
        $('#navbar_login_btn').fadeIn("slow");
        $('.login-btn').fadeOut("slow");

    } else {
        $('#navbar_login_btn').clearQueue();
        $('.login-btn').clearQueue();
        $('#navbar_login_btn').fadeOut("slow");
        $('.login-btn').fadeIn("slow");
    }
}


$(function () {
    main_login_button_y = $('.login-btn').position().top;

    if ($('.navbar').length > 0) {
        $(window).on("scroll load resize", function () {
            checkScroll();
        });
    }
});