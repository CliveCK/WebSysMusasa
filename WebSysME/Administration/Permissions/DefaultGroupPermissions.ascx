<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DefaultGroupPermissions.ascx.vb" Inherits="WebSysME.DefaultGroupPermissions" %>
<%@ Register Src="~/Controls/ComplementaryListboxes.ascx" TagName="ComplementaryListboxes"
    TagPrefix="uc1" %>
<table width="100%" class="GreyBG" cellspacing="0" >
    <tr>
        <td class="DetailsSection" colspan="3">
            Usergroups</td>
    </tr>
    <tr>
        <td rowspan="2" valign="top" style="width: 170px">
            <asp:ListBox ID="lstRoles" runat="server" Width="170px" AutoPostBack="True" Height="208px" CssClass="form-control"></asp:ListBox></td>
        <td style="width: 14px">
        </td>
        <td style="background-color: whitesmoke">
            <uc1:ComplementaryListboxes ID="ComplementaryListboxes1" runat="server" />
            <asp:Button ID="cmdApply" runat="server" Text="Apply" CssClass="btn btn-default"/></td>
    </tr>
    <tr>
        <td style="width: 14px">
        </td>
        <td>
            </td>
    </tr>
    <tr>
        <td rowspan="1" style="width: 170px" valign="top"> <input type="button" id="showit" class="btn btn-default" value="Edit Selected" onclick="showRecipients(event)" />
          &nbsp; <input type="button" id="Button1" class="btn btn-default" value="Add New" onclick="showNew(event)" /></td>
        <td style="width: 14px">
        </td>
        <td>
            <asp:TextBox ID="txtUserGroupID" runat="server" CssClass="HiddenControl">0</asp:TextBox></td>
    </tr>
    <tr>
        <td rowspan="1" style="width: 170px" valign="top">
           <div style="width:170px; text-align:left; vertical-align: top;" id="picker" > <table width="100%">
                <tr>
                    <td>
            <asp:Label ID="lblDescription" runat="server" Text="Desription:"></asp:Label></td>
                </tr>
                <tr>
                    <td>
            <asp:TextBox ID="txtDescription" runat="server" Width="168px" CssClass="form-control"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
            <asp:Button ID="cmaSave" runat="server" Text="Save" CssClass="btn btn-default"/></td>
                </tr>
            </table></div>
        </td>
        <td style="width: 14px">
        </td>
        <td>
        </td>
    </tr>
</table>
<script language="javascript">

var isCSS, isW3C, isIE4, isNN4, isIE6CSS;

// Initialize upon load to let all browsers establish content objects
function initDHTMLAPI( ) {
    if (document.images) {
        isCSS = (document.body && document.body.style) ? true : false;
        isW3C = (isCSS && document.getElementById) ? true : false;
        isIE4 = (isCSS && document.all) ? true : false;
        isNN4 = (document.layers) ? true : false;
        isIE6CSS = (document.compatMode && document.compatMode.indexOf("CSS1") >= 0) ? 
            true : false;
    }
}

// Set event handler to initialize API
window.onload = initDHTMLAPI;


function seekLayer(doc, name) {
    var theObj;
    for (var i = 0; i < doc.layers.length; i++) {
        if (doc.layers[i].name == name) {
            theObj = doc.layers[i];
            break;
        }
        // dive into nested layers if necessary
        if (doc.layers[i].document.layers.length > 0) {
            theObj = seekLayer(document.layers[i].document, name);
        }
    }
    return theObj;
}
   
// Convert object name string or object reference
// into a valid element object reference
function getRawObject(obj) {
    var theObj;
    if (typeof obj == "string") {
        if (isW3C) {
            theObj = document.getElementById(obj);
        } else if (isIE4) {
            theObj = document.all(obj);
        } else if (isNN4) {
            theObj = seekLayer(document, obj);
        }
    } else {
        // pass through object reference
        theObj = obj;
    }
    return theObj;
}
   
// Convert object name string or object reference
// into a valid style (or NN4 layer) reference
function getObject(obj) {
    var theObj = getRawObject(obj);
    if (theObj && isCSS) {
        theObj = theObj.style;
    }
    return theObj;
}
   
// Position an object at a specific pixel coordinate
function shiftTo(obj, x, y) {
    var theObj = getObject(obj);
    if (theObj) {
        if (isCSS) {
            // equalize incorrect numeric value type
            var units = (typeof theObj.left == "string") ? "px" : 0;
            x =x+ 30;
           // theObj.left = x + units ;
            y=y + 35;
           theObj.top = y + units;
            
        } else if (isNN4) {s
        //x=x+20;
          theObj.moveTo(x,y)
        }
    }
}
   
// Move an object by x and/or y pixels
function shiftBy(obj, deltaX, deltaY) {
    var theObj = getObject(obj);
    if (theObj) {
        if (isCSS) {
            // equalize incorrect numeric value type
            var units = (typeof theObj.left == "string") ? "px" : 0;
            theObj.left = getObjectLeft(obj) + deltaX + units;
            theObj.top = getObjectTop(obj) + deltaY + units;
        } else if (isNN4) {
            theObj.moveBy(deltaX, deltaY);
        }
    }
}
   
// Set the z-order of an object
function setZIndex(obj, zOrder) {
    var theObj = getObject(obj);
    if (theObj) {
        theObj.zIndex = zOrder;
    }
}
   
// Set the background color of an object
function setBGColor(obj, color) {
    var theObj = getObject(obj);
    if (theObj) {
        if (isNN4) {
            theObj.bgColor = color;
        } else if (isCSS) {
            theObj.backgroundColor = color;
        }
    }
}
   
// Set the visibility of an object to visible
function show(obj) {
    var theObj = getObject(obj);
    if (theObj) {
        theObj.visibility = "visible";
    }
}
   
// Set the visibility of an object to hidden
function hide(obj) {
    var theObj = getObject(obj);
    if (theObj) {
        theObj.visibility = "hidden";
    }
}
   
// Retrieve the x coordinate of a positionable object
function getObjectLeft(obj)  {
    var elem = getRawObject(obj);
    var result = 0;
    if (document.defaultView) {
        var style = document.defaultView;
        var cssDecl = style.getComputedStyle(elem, "");
        result = cssDecl.getPropertyValue("left");
    } else if (elem.currentStyle) {
        result = elem.currentStyle.left;
    } else if (elem.style) {
        result = elem.style.left;
    } else if (isNN4) {
        result = elem.left;
    }
    return parseInt(result);
}
   
// Retrieve the y coordinate of a positionable object
function getObjectTop(obj)  {
    var elem = getRawObject(obj);
    var result = 0;
    if (document.defaultView) {
        var style = document.defaultView;
        var cssDecl = style.getComputedStyle(elem, "");
        result = cssDecl.getPropertyValue("top");
    } else if (elem.currentStyle) {
        result = elem.currentStyle.top;
    } else if (elem.style) {
        result = elem.style.top;
    } else if (isNN4) {
        result = elem.top;
    }
    return parseInt(result);
}
   
// Retrieve the rendered width of an element
function getObjectWidth(obj)  {
    var elem = getRawObject(obj);
    var result = 0;
    if (elem.offsetWidth) {
        result = elem.offsetWidth;
    } else if (elem.clip && elem.clip.width) {
        result = elem.clip.width;
    } else if (elem.style && elem.style.pixelWidth) {
        result = elem.style.pixelWidth;
    }
    return parseInt(result);
}
   
// Retrieve the rendered height of an element
function getObjectHeight(obj)  {
    var elem = getRawObject(obj);
    var result = 0;
    if (elem.offsetHeight) {
        result = elem.offsetHeight;
    } else if (elem.clip && elem.clip.height) {
        result = elem.clip.height;
    } else if (elem.style && elem.style.pixelHeight) {
        result = elem.style.pixelHeight;
    }
    return parseInt(result);
}
   
// Return the available content width space in browser window
function getInsideWindowWidth( ) {
    if (window.innerWidth) {
        return window.innerWidth;
    } else if (isIE6CSS) {
        // measure the html element's clientWidth
        return document.body.parentElement.clientWidth;
    } else if (document.body && document.body.clientWidth) {
        return document.body.clientWidth;
    }
    return 0;
}
   
// Return the available content height space in browser window
function getInsideWindowHeight( ) {
    if (window.innerHeight) {
        return window.innerHeight;
    } else if (isIE6CSS) {
        // measure the html element's clientHeight
        return document.body.parentElement.clientHeight;
    } else if (document.body && document.body.clientHeight) {
        return document.body.clientHeight;
    }
    return 0;
}

 

function showRecipients(evt) {
    evt = (evt) ? evt : event;
    if (evt) {
    
      if (document.getElementById('<% Response.Write(txtDescription.ClientID) %>').value!= 0){
    
        if (document.getElementById("picker").style.visibility != "visible") {
            var elem = (evt.target) ? evt.target : evt.srcElement;
            var position = getElementPosition(elem.id);
         shiftTo("picker", position.left + elem.offsetWidth, position.top);
          show("picker");
		  
        } else {
            hide("picker");
        }
        }else{
        
        alert('Please select a usergroup!');
        
        }
        
    }
}


function showNew(evt) {
    evt = (evt) ? evt : event;
    if (evt) {
        if (document.getElementById("picker").style.visibility != "visible") {
            var elem = (evt.target) ? evt.target : evt.srcElement;
            var position = getElementPosition(elem.id);
            document.getElementById('<% Response.Write(txtDescription.ClientID) %>').value="";

            shiftTo("picker", position.left + elem.offsetWidth, position.top);
          show("picker");
		  
        } else {
            hide("picker");
        }
    }
}

   function getElementPosition(elemID) {
    var offsetTrail = document.getElementById(elemID);
    var offsetLeft = 0;
    var offsetTop = 0;
    while (offsetTrail) {
        offsetLeft += offsetTrail.offsetLeft;
        offsetTop += offsetTrail.offsetTop;
        offsetTrail = offsetTrail.offsetParent;
    }
    if (navigator.userAgent.indexOf("Mac") != -1 && 
        typeof document.body.leftMargin != "undefined") {
        offsetLeft += document.body.leftMargin;
        offsetTop += document.body.topMargin;
    }
    return {left:offsetLeft, top:offsetTop};
}


</script>

