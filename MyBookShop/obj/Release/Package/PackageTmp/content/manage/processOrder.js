function processOrder(key) {
    if (http == null) return;
    http = getHttp();
    http.onreadystatechange = processOrderCallBack;
    var url = "http://localhost:57018/Manage/processOrder?key=" + key;
    http.open("GET", url, true);
    http.send(null);
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
function processOrderCallBack() {
    if (http.readyState == 4) {
        layer.msg(http.responseText, {
            time: 2000 //2秒关闭（如果不配置，默认是3秒）
        }, function () {
            window.location.reload();
        });
    }
}
function getOrderContent(id) {
    layer.open({
        type: 2,
        title: "订单详情",
        shadeClose: true,
        shade: 0.8,
        area: ['990px', '600px'],
        content: 'http://localhost:57018/Manage/OrderDetails?key=' + id + ''
    });
}