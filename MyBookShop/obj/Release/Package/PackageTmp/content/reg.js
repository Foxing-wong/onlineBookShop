function checkUserInfo() {
    if ($("#userName").val() == "") {
        layer.tips('请输入登录账号', '#userName');
        document.getElementById("userName").focus();
        document.getElementById("login_submit").disabled = true;
        return false;
    }
    if ($("#userPwd").val() == "") {
        layer.tips('请输入登录密码', '#userPwd');
        document.getElementById("userPwd").focus();
        return false;
    }
    if ($("#userPwd2").val() == "") {
        layer.tips('请重复登录密码', '#userPwd2');
        document.getElementById("userPwd2").focus();
        return false;
    }
    if ($("#trueName").val() == "") {
        layer.tips('请重复登录密码', '#trueName');
        document.getElementById("trueName").focus();
        return false;
    }
    if ($("#userPwd").val() != $("#userPwd2").val()) {
        layer.tips('两次输入的密码不相同，请核对', '#userPwd2');
        document.regFrm.userPwd.value = "";
        document.regFrm.userPwd2.value = "";
        document.getElementById("userPwd").focus();
        return false;
    }
    if ($("#userPwd").val().trim().length < 6) {
        layer.tips('密码过于简单，请重新输入[请大于6位数]', '#userPwd');
        document.getElementById("userPwd").focus();
        return false;
    }
    else {
        return true;
    }
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
var uname="";
function ckUserName() {
    uname = document.getElementById("userName").value;
    if (http == null) return;
    http = getHttp();
    http.onreadystatechange = ckUserNameCallBack;
    var url = "http://localhost:57018/serviceAPI/checkUserName.ashx?userName=" + uname;
    http.open("GET", url, true);
    http.send(null);

}
function ckUserNameCallBack() {
    if (http.readyState == 4) {
        if (http.responseText == "Y") {
            layer.tips('恭喜，该账号可以使用', '#userName');
        } else {
            layer.tips('抱歉，该账号[' + uname + ']已被使用，请重新注册', '#userName');
            document.regFrm.userName.value = "";
            document.getElementById("userName").focus();
        }
    }

}