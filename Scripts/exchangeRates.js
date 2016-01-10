
var APP_ID = "";

$(document).ready(function () {
    $.get("https://openexchangerates.org/api/?app_id=" + APP_ID, function (exchangeRates) {
        for (currency in exchangeRates.rates) {
            var rate = exchangeRates.rates[currency];
            var option = $("<option></option>").val(rate).text(currency);
            option.data("currency", currency)
            if (currency == "USD")
                option.attr("selected", "selected")

            $("#exchange-rates").append(option);
        }
    });

    $("#exchange-rates").on('change', function () {
        var rate = parseFloat($(this).val());
        var currency = $(this).data("currency");
        if (!isNaN(rate)) {
            $(".search-results .amount").each(function (index) {
                if ($(this).data("amount") != null) {
                    if (currency == "USD") {
                        $(this).text($(this).data("amount"));
                    }
                    else {
                        var amount = parseFloat($(this).data("amount"));
                        if (!isNaN(amount)) {
                            var number = amount * rate;
                            $(this).text(number.toFixed(2));
                        }
                    }
                }

            });
        }
    });
});