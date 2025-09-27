



$(document).ready(function () {
   
    var totalArticleCountUrl = app.Urls.totalArticleCountUrl;
    var totalCategoryCountUrl = app.Urls.totalCategoryCountUrl;
    var totalUsersCountUrl = app.Urls.totalUsersCountUrl;
    var totalCommentsCountUrl = app.Urls.totalCommentsCountUrl;
    var totalGithubsCountUrl = app.Urls.totalGithubsCountUrl;


    $.ajax({
        type: "GET",
        url: totalCommentsCountUrl,
        dataType: "json",
        success: function (data) {
            $("h2#totalCommentsCount").append(data);
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
            $("h2#totalArticleCount").append(data);
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
            $("h2#totalUsersCount").append(data);
        },
        error: function () {
            toastr.error("Toplam kullanıcı yüklenirken hata oluştu", "Hata");
        }

    });




    $.ajax({
        type: "GET",
        url: totalCategoryCountUrl,
        dataType: "json",
        success: function (data) {
            $("h2#totalCategoryCount").append(data);
        },
        error: function () {
            toastr.error("Kategori Sayısı Yüklenirken Hata Oluştu", "Hata");
        }

    });
});
