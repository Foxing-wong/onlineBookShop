/**
 * Created by an.han on 13-12-17.
 */
window.onload = function () {
    if (!document.getElementsByClassName) {
        document.getElementsByClassName = function (cls) {
            var ret = [];
            var els = document.getElementsByTagName('*');
            for (var i = 0, len = els.length; i < len; i++) {

                if (els[i].className.indexOf(cls + ' ') >= 0 || els[i].className.indexOf(' ' + cls + ' ') >= 0 || els[i].className.indexOf(' ' + cls) >= 0) {
                    ret.push(els[i]);
                }
            }
            return ret;
        }
    }
    var table = document.getElementById('cartTable'); // 购物车表格
    var selectInputs = document.getElementsByClassName('check'); // 所有勾选框
    var checkAllInputs = document.getElementsByClassName('check-all') // 全选框
    var tr = table.children[1].rows; //行
    var selectedTotal = document.getElementById('selectedTotal'); //已选商品数目容器
    var priceTotal = document.getElementById('priceTotal'); //总计
    var deleteAll = document.getElementById('deleteAll'); // 删除全部按钮
    var selectedViewList = document.getElementById('selectedViewList'); //浮层已选商品列表容器
    var selected = document.getElementById('selected'); //已选商品
    var foot = document.getElementById('foot');

    // 更新总数和总价格，已选浮层
    function getTotal() {
        var seleted = 0;
        var price = 0;
        var HTMLstr = '';
        for (var i = 0, len = tr.length; i < len; i++) {
            if (tr[i].getElementsByTagName('input')[0].checked) {
                tr[i].className = 'on';
                seleted += parseInt(tr[i].getElementsByTagName('input')[1].value);
                price += parseFloat(tr[i].cells[4].innerHTML);
                HTMLstr += '<div><img src="' + tr[i].getElementsByTagName('img')[0].src + '"><span class="del" index="' + i + '">取消选择</span></div>'
            }
            else {
                tr[i].className = '';
            }
        }

        selectedTotal.innerHTML = seleted;
        priceTotal.innerHTML = price.toFixed(2);
        selectedViewList.innerHTML = HTMLstr;

        if (seleted == 0) {
            foot.className = 'foot';
        }
    }

    // 计算单行价格
    function getSubtotal(tr) {
        var cells = tr.cells;
        var price = cells[2]; //单价
        var subtotal = cells[4]; //小计td
        var countInput = tr.getElementsByTagName('input')[1]; //数目input
        var span = tr.getElementsByTagName('span')[1]; //-号
        //写入HTML
        subtotal.innerHTML = (parseInt(countInput.value) * parseFloat(price.innerHTML)).toFixed(2);
        //如果数目只有一个，把-号去掉
        if (countInput.value == 1) {
            span.innerHTML = '';
        } else {
            span.innerHTML = '-';
        }
    }

    // 点击选择框
    for (var i = 0; i < selectInputs.length; i++) {
        selectInputs[i].onclick = function () {
            if (this.className.indexOf('check-all') >= 0) { //如果是全选，则吧所有的选择框选中
                for (var j = 0; j < selectInputs.length; j++) {
                    selectInputs[j].checked = this.checked;
                }
            }
            if (!this.checked) { //只要有一个未勾选，则取消全选框的选中状态
                for (var i = 0; i < checkAllInputs.length; i++) {
                    checkAllInputs[i].checked = false;
                }
            }
            getTotal();//选完更新总计
        }
    }

    //为每行元素添加事件
    for (var i = 0; i < tr.length; i++) {
        //将点击事件绑定到tr元素
        tr[i].onclick = function (e) {
            var e = e || window.event;
            var el = e.target || e.srcElement; //通过事件对象的target属性获取触发元素
            var cls = el.className; //触发元素的class
            var countInout = this.getElementsByTagName('input')[1]; // 数目input
            var value = parseInt(countInout.value); //数目
            //通过判断触发元素的class确定用户点击了哪个元素
            switch (cls) {
                case 'add': //点击了加号
                    countInout.value = value + 1;
                    getSubtotal(this);
                    var sysID = countInout.id;
                    if (http == null) return;
                    http = getHttp();
                    var url = "../../home/updateCartNum?sysID=" + sysID + "&newNum=" + countInout.value;
                    http.open("GET", url, false);
                    http.send(null);
                    if (http.responseText!="okay")
                    {
                        layer.msg(http.responseText);
                        var newSysNum = http.responseText.replace(/[^0-9]/ig, "");
                        countInout.value = newSysNum;
                        getSubtotal(this);
                    }
                    break;
                case 'reduce': //点击了减号
                    if (value > 1) {
                        countInout.value = value - 1;
                        getSubtotal(this);
                        var sysID = countInout.id;
                        if (http == null) return;
                        http = getHttp();
                        var url = "../../home/updateCartNum?sysID=" + sysID + "&newNum=" + countInout.value;
                        http.open("GET", url, false);
                        http.send(null);
                        if (http.responseText != "okay") {
                            layer.msg(http.responseText);
                            var newSysNum = http.responseText.replace(/[^0-9]/ig, "");
                            countInout.value = newSysNum;
                            getSubtotal(this);
                        }
                        
                    }
                    break;
               /* case 'delete': //点击了删除
                    layer.confirm('您确定从购物车删除该商品？', {
                        btn: ['确定，我去意已决', '让我再想想'],
                        shade: false
                    }, function () {
                        this.parentNode.removeChild(this);
                    }, function () {
                    });
                    var conf = confirm('纭畾鍒犻櫎姝ゅ晢鍝佸悧锛�');
                    if (conf) {
                        this.parentNode.removeChild(this);
                    }
                    break;*/
            }
            getTotal();
        }

        // 给数目输入框绑定keyup事件
        tr[i].getElementsByTagName('input')[1].onkeyup = function () {
            var val = parseInt(this.value);
            if (isNaN(val) || val <= 0) {
                val = 1;
            }
            if (this.value != val) {
                this.value = val;
            }
            var sysID = this.id;
            if (http == null) return;
            http = getHttp();
            var url =  "../../home/updateCartNum?sysID=" + sysID + "&newNum=" + countInout.value;
            http.open("GET", url, false);
            http.send(null);
            if (http.responseText != "okay") {
                layer.msg(http.responseText);
                var newSysNum = http.responseText.replace(/[^0-9]/ig, "");
                countInout.value = newSysNum;
            }
            
            getSubtotal(this.parentNode.parentNode); //更新小计
            getTotal(); //更新总数
        }
    }
    function updateCartCallBack()
    {
        if (http.readyState == 4 ) {
            layer.msg(http.responseText);
        }
    }

    // 默认全选
    checkAllInputs[0].checked = true;
    checkAllInputs[0].onclick();
}
function buyCarDelete(bid, second) {
    deleteObj = second;
    layer.confirm('您确定从购物车删除该商品？', {
        btn: ['确定，我去意已决', '让我再想想'],
        shade: false
    }, function () {
        if (http == null) return;
        http = getHttp();
        http.onreadystatechange = deleteCallBack;
        var url = "../../serviceAPI/buyCarDelete.ashx?sysID=" + bid;
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
var deleteObj;
var http = getHttp();
function deleteCurrentRow(obj) {
    var tr = obj.parentNode.parentNode;
    var tbody = tr.parentNode;
    tbody.removeChild(tr);
}
function deleteCallBack() {
    if (http.readyState == 4) {
        layer.msg(http.responseText, {
            icon: 1,
            time: 2000
        }, function () {
            if (http.responseText == "从购物车移除成功") {
                deleteCurrentRow(deleteObj);
                var oldonload = window.onload;
                oldonload();
            }
        });
    }
}
function userSubmit()
{
    document.getElementById('myBuyCar').action = "http://www.baidu.com";
    document.getElementById("myBuyCar").submit();
}