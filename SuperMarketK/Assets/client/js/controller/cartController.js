let isFreeship = false;
let cartController = function () {
    // delete item on cart
    $('.btn-remove').off('click').on('click', function () {
        $.ajax({
            data: { id: $(this).data('id'), size: $(this).data('size') },
            url: '/Cart/Delete',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/Cart";
                }
            }
        })
    });

    $('.inputNumber').on('input', function (event) {
        let idProduct = $(event.target).data('id');
        let sizeProduct = $(event.target).data('size');
        let quanTity = $(event.target).val();
        $.ajax({
            data: { id: idProduct, quantity: quanTity, size: sizeProduct},
            url: '/Cart/UpdateQuantity',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                $("#total-" + idProduct + "-" + sizeProduct).html("$" + (res.priceItem * quanTity).toFixed(2));
                $("#summary-subtotal-all").html("$" + res.sumTotal);
                $("#summary-subtotal-getall").html("$" + res.sumTotal);
            }
        })
    })

    $('#btnSubmitVoucher').on('click', function () {
        let voucherCode = $('#inputVoucher').val();
        $.ajax({
            data: { voucher: voucherCode },
            url: '/Cart/AddVoucher',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    $('#codeVoucher').html(voucherCode);
                    if (res.data.ResultObj.type == 1) {
                        $('#valueVoucher').html('Freeship');
                    } else if (res.data.ResultObj.type == 2) {
                        let value = $('#summary-subtotal-all').html().substring(1);
                        let newValue = parseInt(value) * res.data.ResultObj.discount_amount / 100;
                        $('#valueVoucher').html(newValue);
                        $("#summary-subtotal-getall").html("$" + (value - newValue));
                    } else if (res.data.ResultObj.type == 3){
                        let value = $('#summary-subtotal-all').html().substring(1);
                      //  let newValue = parseInt(value) - res.data.ResultObj.discount_amount;
                        $('#valueVoucher').html(res.data.ResultObj.discount_amount);
                        $("#summary-subtotal-getall").html("$" + (parseInt(value) - res.data.ResultObj.discount_amount));
                    }
                    // alert(res.message + " - " + res.discount_amount);
                } else {
                    alert(res.message);
                }
            }
        })
    });
}
cartController();

$("body").on("change", "#ddlProvince", function () {
    $("#ProvinceName").val($(this).find("option:selected").text());
});
$("body").on("change", "#ddlDistrict", function () {
    $("#huyen").val($(this).find("option:selected").text());
});
$("body").on("change", "#ddlWard", function () {
    $("#xa").val($(this).find("option:selected").text());
});