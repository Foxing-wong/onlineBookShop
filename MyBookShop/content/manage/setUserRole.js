function setUserRole(key,type) {
    if (http == null) return;
    http = getHttp();
    http.onreadystatechange = setUserRoleCallBack;
    var url = "../../Manage/UpdateRole?keys=" + key+"&type="+type;
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
function setUserRoleCallBack() {
    if (http.readyState == 4) {
        layer.msg(http.responseText, {
            time: 2000 //2秒关闭（如果不配置，默认是3秒）
        }, function () {
            window.location.reload();
        });
    }
}