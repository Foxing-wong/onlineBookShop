function cancelOrder(bid) {
    layer.confirm('您确定从取消订单吗？', {
        btn: ['确定，我去意已决', '让我再想想'],
        shade: false
    }, function () {
        if (http == null) return;
        http = getHttp();
        http.onreadystatechange = cancelCallBack;
        var url = "../serviceAPI/cancelOrder.ashx?orderId=" + bid;
        http.open("GET", url, true);
        http.send(null);
    }, function () {
    });
}
function getHttp() {
    var http = null;
    try {
        if (window.ActiveXObject) http = new ActiveXObject("Microsoft.XMLHTTP");
        else if (window.XMLHttpRequest) http = new XMLHttpRequest();
        else alert("ERROR");
    }
    catch (e) {
        alert(e.description);
    }
    return http;
}
var http = getHttp();
function cancelCallBack() {
    if (http.readyState == 4) {
        layer.msg(http.responseText, {
            icon: 1,
            time: 2000
        }, function () {
            location = location;
        });
    }
}