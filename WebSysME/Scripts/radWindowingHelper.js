//This code is used to provide a reference to the radwindow "wrapper"
function GetRadWindow()
{
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;//IE (and Moz az well)
    return oWindow;
}

function OpenRadWindowLookup(lookupUrl, windowName)
{
    var oWnd = radopen (lookupUrl, windowName); 
    oWnd.SetSize(700,400);
}

function OpenRadWindowLookup(lookupUrl, windowName, luHeight, luWidth)
{
    var oWnd = radopen (lookupUrl, windowName); 
    oWnd.SetSize(luWidth, luHeight);
    oWnd.SetModal(true);
} 

function OpenMemberPicker(LinkTypeID)
{
    //Getting rad window manager
    var oManager = GetRadWindowManager();
    //Success. Getting existing window DialogWindow using GetWindowByName
    var oWnd = oManager.GetWindowByName("radwndMemberPicker");
    //Set window modality
    oWnd.Modal = true;
    //Success. Opening window
    oWnd.Show();

    //Show an existing window
    //window.radopen(null, "radwndMemberPicker");
    
    return false;
}
