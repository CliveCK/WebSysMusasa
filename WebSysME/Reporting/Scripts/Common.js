function dateYMD (v) {
    try {
        v = v.match(/(\d+)\D+(\d+)\D+(\d+)/);
        v[2] = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"][v[2] - 1];
        return v.slice(1).reverse().join("-");
    }
    catch(err)
    {
        return v;
    }
}

function formatNumber(num,dec,thou,pnt,curr1,curr2,n1,n2) {var x = Math.round(num * Math.pow(10,dec));if (x >= 0) n1=n2='';var y = (''+Math.abs(x)).split('');var z = y.length - dec; if (z<0) z--; for(var i = z; i < 0; i++) y.unshift('0');y.splice(z, 0, pnt); if(y[0] == pnt) y.unshift('0'); while (z > 3) {z-=3; y.splice(z,0,thou);}var r = curr1+n1+y.join('')+n2+curr2;return r;}

/**
 * Fast prefix-checker.
 */
String.prototype.startsWith = function(prefix) {
  if (this.length < prefix.length) {
    return false;
  }

  if (this.substring(0, prefix.length) == prefix) {
    return true;
  }

  return false;
}


/* Onload Functions 
$(document).ready(function(){

    $("input[Purpose='GeneralDate']").datepicker({dateFormat: 'dd-M-yy'});
    
});*/
