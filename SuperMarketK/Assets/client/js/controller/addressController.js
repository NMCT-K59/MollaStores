var address = {
    init: function () {
        address.loadProvince();
        address.event();
    },
    event: function () {
        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                address.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');
            }
        });

        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                address.loadPhuongXa(parseInt(id));
            }
            else {
                $('#ddlWard').html('');
            }
        });

        $('#ddlWard').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                var districtID = parseInt($('#ddlDistrict').val());
                address.loadTienShip(parseInt(id), districtID);
            }
        });

    },
    loadProvince: function () {
        var html = '';
        $.ajax({
            url: '/Cart/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Choose province --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.ID + '">' + item.Name + ' </option>'
                    });
                    $('#ddlProvince').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        var html = '';
        $.ajax({
            url: '/Cart/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Choose district --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.ID + '">' + item.Name + ' </option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadPhuongXa: function (id) {
        var html = '';
        $.ajax({
            url: '/Cart/LoadPhuongXa',
            type: "POST",
            data: { districtID: id },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var html = '<option value = "">-- Choose wards --</option>';
                    var data = responce.data;
                    $.each(data, function (i, item) {
                        html += '<option value  ="' + item.WardCode + '">' + item.WardName + ' </option>'
                    });
                    $('#ddlWard').html(html);
                }
            }
        })
    },
    loadTienShip: function (id, districtID) {
        var html = '';
        $.ajax({
            url: '/Cart/getTienShip',
            type: "POST",
            data: { wardCode: id, districtID: districtID },
            dataType: "json",
            success: function (responce) {
                if (responce.status == true) {
                    var labelTongTien = $("#tongTien");
                    var labelTienHang = $("#tienHang");
                    if (isFreeship == false) {
                        var label = $("#txtShipment");
                        var html = format2(responce.data) + '<span> $</span>';
                        let labelDiscount = 0;
                        if (typeof ($('#txtDiscount').html()) !== 'undefined') {
                            labelDiscount = parseInt($('#txtDiscount').html());
                        }
                        var tongTien = responce.data + parseInt(labelTienHang.text().substring(1)) - labelDiscount;
                        // var tongTien = responce.data + parseInt(labelTienHang.text().substring(0, labelTienHang.text().indexOf('VNĐ')))*1000;
                        var htmlTongTien = format2(tongTien) + '<span> $</span>';
                        labelTongTien.html(htmlTongTien);
                        label.html(html);
                    } else {
                        var tongTien = parseInt(labelTienHang.text().substring(1));
                        var htmlTongTien = format2(tongTien) + '<span> $</span>';
                        labelTongTien.html(htmlTongTien);
                    }
                }
            }
        })
    }
}
address.init();

function format2(n) {
    return n.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
}