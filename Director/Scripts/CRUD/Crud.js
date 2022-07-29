function fnInsert() {
    var name = $("#name").val();
    var lName = $("#lName").val();
    var pNumber = $("#pNumber").val();
    var tc = $("#tc").val();


    if (tcController(tc)) {
        //Ajax metodu kullanarak gönderdiğimiz veriyi database'e kaydedeceğiz.
        $.ajax({
            type: "POST",
            url: "/Home/InsertFields/",    //url bir denetleyciyi aramak için kullanılır. Home denetleyicisini çağırıyoruz.
            datatype: "json",
            contentType: "application/json: charset=utf-8",
            data: JSON.stringify({ name: name, lName: lName, pNumber: pNumber, tc: tc }),   //index.html deki verileri kullanılır.
            success: function (json1) {
                window.location.replace("/Home/Index/");
                ViewData();
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
    } else {
        alert("T.C kimlik numarası geçersizdir.");
    }
    
}
function tcController(tc) {
    var tekToplam = 0, ciftToplam = 0, toplam = 0;
    for (var i = 0; i < 9; i++) {
        if (i % 2 == 0) {
            tekToplam += Number(tc[i]);
        } else {
            console.log(tc[i])
            ciftToplam += Number(tc[i]);
        }
        toplam += Number(tc[i]);
    }
    console.log(tekToplam);
    console.log(ciftToplam);
    console.log(toplam);
    console.log(Number(tc[9]));
    tekToplam *= 7;
    console.log(tc[0] == 0);
    console.log((tekToplam - ciftToplam) % 10)
    console.log(tc[9])
    console.log(((toplam + tc[9]) % 10));
    


    if (tc[0] == 0 || tc[9] != (tekToplam - ciftToplam) % 10 || ((toplam + Number(tc[9])) % 10) != tc[10] || tc.length < 11) {
        return false;
    } else {
        return true;
    }
}

$(document).ready(function () { //bu fonksiyon sayfa yüklendiğinde yüklenir.
    ViewData();
    $("#update").hide();
    function ViewData() {

        $.ajax({
            type: "POST",
            url: "/Home/ViewData/",
            datatype: "json",
            contentType: "application/json: charset=utf-8",
            data: JSON.stringify({}),
        success: function (json1) {
            /*debugger*/
            var tableload = json1.html;
            var dataset = eval("[" + tableload + "]");
            $('#ViewValues').DataTable({
                retrieve: true, //

                ordering: false,
                data: dataset,
                columns: [
                    { title: "ID" },
                    { title: "Ad" },
                    { title: "Soyadı" },
                    { title: "Telefon Numarası" },
                    { title: "T.C.No" },
                    { title: "Durum" }, //Delete, Edit
                ]
            });
        },
        failure: function (errMsg) {
            alert(errMsg);
        }      
        });
    }
});



function fnDelete(id) {
    $.ajax({
        type: "POST",
        url: "/Home/DeleteData/",
        datatype: "json",
        contentType: "application/json: charset=utf-8",
        data: JSON.stringify({ id :id }),
        success: function (json) {
            window.location.replace("/Home/Index/");
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function fnEdit(id) {
    $.ajax({
        type: "POST",
        url: "/Home/EditData/",
        datatype: "json",
        contentType: "application/json: charset=utf-8",
        data: JSON.stringify({ id: id }),
        success: function (json) {
            var arrval = json.htmlValues;
            $("#name").val(arrval[0]);
            $("#lName").val(arrval[1]);
            $("#pNumber").val(arrval[2]);
            $("#tc").val(arrval[3]);
            $("#id").val(arrval[4]);

            $("#Btn").hide();
            $("#update").show();

        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}

function fnUpdate() {
    var name = $("#name").val();
    var lName = $("#lName").val();
    var pNumber = $("#pNumber").val();
    var tc = $("#tc").val();
    var id = $("#id").val();

    $.ajax({
        type: "POST",
        url: "/Home/UpdateFields/",
        datatype: "json",
        contentType: "application/json: charset=utf-8",
        data: JSON.stringify({ name: name, lName: lName, pNumber: pNumber, tc: tc, id: id}),
        success: function (json1) {
            ViewData();
            window.location.replace("/Home/Index/");
        },
        failure: function (errMsg) {
            alert(errMsg);
        }
    });
}