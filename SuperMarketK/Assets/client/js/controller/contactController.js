let contactController = function () {
    $('#btnSend').off('click').on('click', function () {
        var name = $('#txtName').val();
        var email = $('#txtEmail').val();
        var sdt = $('#txtSDT').val();
        var diachi = $('#txtDiaChi').val();
        var tinnhan = $('#txtTinNhan').val();
        $.ajax({
            url: '/Contact/Insert',
            type: 'POST',
            dataType: 'json',
            data: {
                name: name,
                email: email,
                sdt: sdt,
                diachi: diachi,
                tinnhan: tinnhan
            },
            success: function (res) {
                if (res.status == false) {
                    console.log("False");
                }
            }
        });
    });
}
contactController();