window.onload = calcTotalPallets;
function IsNumeric(sText)

{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
   
};

function calcProdSubTotal() {
    
    
    var prodSubTotal = 0;

    $(".row-total-input").each(function(){
    
        var valString = $(this).val() || 0;
        
        prodSubTotal += parseFloat(valString);
                    
    });
        
    $("#product-subtotal").val(prodSubTotal.toFixed(2));

};
    function calcTotalPallets() {

    var totalPallets = 0;

    $(".num-pallets-input").each(function() {
    
        var thisValue = $(this).val();
    
        if ( (IsNumeric(thisValue)) &&  (thisValue != '') ) {
        
            totalPallets += parseFloat(thisValue);
        
        };
    
    });
    
    $("#total-pallets-input").val(totalPallets);

};

    function calcShippingTotal() {
};
function calcOrderTotal() {

    var orderTotal = 0;

    var productSubtotal = $("#product-subtotal").val() || 0;
    var shippingSubtotal = $("#shipping-subtotal").val() || 0;
    if (productSubtotal > 100) {
        $("#shipping-subtotal").val("0.00");
        shippingSubtotal = 0;
    }
    else {
        $("#shipping-subtotal").val("10.00");
        shippingSubtotal = 10;
    }
    var orderTotal = parseFloat(productSubtotal) + parseFloat(shippingSubtotal);
    var orderTotalNice =orderTotal;
    $("#order-total").html(orderTotalNice.toFixed(2));   
};
function onload() {
    var numPallets = $("#num - pallets - input").val();
    var numPrice = $(".price-per-pallet").text();
    alert(numPallets+"XX"+numPrice);
}
window.onload = onload;
$(function () {

    $('.num-pallets-input').blur(function () {

        var $this = $(this);

        var numPallets = $this.val();
        var multiplier = $this
                            .parent().parent()
                            .find("td.price-per-pallet span")
                            .text();

        if ((IsNumeric(numPallets)) && (numPallets != '')) {

            var rowTotal = numPallets * multiplier;

            $this
                .css("background-color", "white")
                .parent().parent()
                .find("td.row-total input")
                .val(rowTotal);

        } else {

            $this.css("background-color", "#ffdcdc");

        };
        calcProdSubTotal();
        calcTotalPallets();
        calcShippingTotal();
        calcOrderTotal();

    });

});
