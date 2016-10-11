using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PrimaSysMessaging.BusinessLogic;
using BusinessLogic;

public partial class SendMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack == false)
        {
            UserHandler userHandler = new UserHandler();

            chkLstUsers.DataSource = userHandler.GetUserList();
            chkLstUsers.DataTextField = "userid";
            chkLstUsers.DataValueField = "userid";
            chkLstUsers.DataBind();

            if (Request.QueryString["action"] != null &&
                Session["Message"] != null)                
            {
                Message msg = (Message)Session["Message"];

                switch (Request.QueryString["action"].ToString())
                {
                    case "reply":
                        txtSubject.Text = "Re: " + msg.Subject;
                        txtToList.Text = msg.SenderId + ",";
                        FreeTextBox1.Text = Server.HtmlDecode
                            ("<br/><br/><br/> Original Message<hr/> From: " + 
                            msg.SenderId +
                            "<br/> To: " +
                            msg.RecieverId +
                            "<br/> Message contents: " +
                            msg.Body);
                        break;

                    case "forward":
                        txtSubject.Text = "Fw: " + msg.Subject;
                        FreeTextBox1.Text = Server.HtmlDecode("<br/><br/><br/> Original Message<hr/> From: " +
                            msg.SenderId +
                            "<br/> To: " +
                            msg.RecieverId +
                            "<br/> Message contents: " +
                            msg.Body);
                        break;
                }
            }

        }
    }
    protected void btnAddSelected_Click(object sender, EventArgs e)
    {
        //There could be some users already added to the list so check and put a ',' if needed.
        if (txtToList.Text.Trim() != string.Empty && txtToList.Text.Trim()[txtToList.Text.Trim().Length -1] != ',')
        {
            txtToList.Text += ",";
        }
        foreach (ListItem item in chkLstUsers.Items)
        {
            if (item.Selected == true)
            {
                string recieversList = txtToList.Text.Replace(" ", "");
                if (recieversList.Contains(item.Text + ",") == false)
                {
                    txtToList.Text += item.Text + ",";
                }
            }
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (txtToList.Text.Trim() == string.Empty)
        {
            //Set the notification label here
            return;
        }
        
        // Let us get the list of valid users first
        UserHandler userHandler = new UserHandler();
        DataTable table = userHandler.GetUserList();

        //Now get the recievers list entered by user        
        string recieversList = txtToList.Text.Replace(" ", "");
        string[] users = recieversList.Split(new char[] { ','});
        string[] failList = new string[users.Length];
        string[] successList = new string[users.Length];

        int successCount = 0;
        int failCount = 0;

        MessageHandler handler = new MessageHandler();
        foreach (string user in users)
        {
            if (userHandler.IsValidUser(user) == true)
            {
                if (true == handler.SendMessage(user, CookiesWrapper.thisUserID.ToString(), txtSubject.Text, Server.HtmlEncode(FreeTextBox1.Text)))
                {
                    successList[successCount++] = user;
                }
                else
                {
                    failList[failCount++] = user;
                }
            }
            else
            {
                failList[failCount++] = user;
            }
        }        

        Session["SuccessList"] = successList;
        Session["FailList"] = failList;

        Response.Redirect("Confirmation.aspx");        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
