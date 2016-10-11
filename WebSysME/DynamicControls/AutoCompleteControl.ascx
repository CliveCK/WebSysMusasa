<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AutoCompleteControl.ascx.vb" Inherits="WebSysME.autocomplete" %>
<input id="txtAuto" class="ac_input" type="text" style="width: 200px;" value="" autocomplete="off" runat="server"/><asp:TextBox
    ID="txtValue" runat="server" CssClass="HiddenControl">0</asp:TextBox>

<script type="text/javascript" src=My.Settings.jQueryScriptFile></script>
<script type="text/javascript" src="Frameworks/jQuery/jquery.autocomplete.js"></script>
<script type="text/javascript">
function findValue(li) {
if( li == null ) return alert("No match!");

// if coming from an AJAX call, let's use the CityId as the value
if( !!li.extra ) var sValue = li.extra[0];

// otherwise, let's just display the value in the text box
else var sValue = li.selectValue;
$("input[Purpose='acompletevalue']").val(sValue);
//alert("The value you selected was: " + sValue);
}

function selectItem(li) {
    findValue(li);
}

function formatItem(row) {
return row[0];
}

function lookupAjax(){
var oSuggest = $("input[Purpose='acomplete']")[0].autocompleter;
oSuggest.findValue();
return false;
}

function lookupLocal(){
var oSuggest = $("#CityLocal")[0].autocompleter;

oSuggest.findValue();

return false;
}

$(document).ready(function() {
	$("input[Purpose='acomplete']").autocomplete(
        "Controls/Common/JSON/DataPage.aspx",
		{
            delay:10,
            minChars:2,
            matchSubset:1,
            matchContains:1,
            cacheLength:10,
            onItemSelect:selectItem,
            onFindValue:findValue,
            formatItem:formatItem,
            lineSeparator:']',
            autoFill:true
		}
	);
	
	});
</script>