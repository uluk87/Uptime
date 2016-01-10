$(document).ready(function () {
    var activePage = $(".search-results > table").data("active-page");
    show(activePage);

    function show(pageNumber) {
        $(".search-results > table > tbody tr").hide();
        $(".search-results > table > tbody tr[data-page='" + pageNumber + "']").show();
    }

    $(".pagination a").click(function (e) {
        var action = $(this).data("action");
        if (action == "first") {
            action = "1";
        }
        $(".search-results > table").data("active-page", action);
        show(action)
        e.preventDefault();
        return false;
    });
});