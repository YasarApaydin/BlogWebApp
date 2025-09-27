



$(document).ready(function () {

    var totalArticleCountUrl = app.Urls.totalArticleCountUrl;

    var totalUsersCountUrl = app.Urls.totalUsersCountUrl;
    var totalCommentsCountUrl = app.Urls.totalCommentsCountUrl;
   


    $.ajax({
        type: "GET",
        url: totalCommentsCountUrl,
        dataType: "json",
        success: function (data) {
            $("div#totalCommentsCount").append(data);
        },
        error: function () {
            toastr.error("Yorum Analizleri yüklenirken hata oluştu", "Hata");
        }


    });




    $.ajax({
        type: "GET",
        url: totalArticleCountUrl,
        dataType: "json",
        success: function (data) {
            $("div#totalArticleCount").append(data);
        },
        error: function () {
            toastr.error("Makale Analizleri yüklenirken hata oluştu", "Hata");
        }


    });


    $.ajax({
        type: "GET",
        url: totalUsersCountUrl,
        dataType: "json",
        success: function (data) {
            $("div#totalUsersCount").append(data);
        },
        error: function () {
            toastr.error("Toplam kullanıcı yüklenirken hata oluştu", "Hata");
        }

    });





});
