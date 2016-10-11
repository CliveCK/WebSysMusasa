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

public partial class ReadMail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Check whether the proper message is being passed or not
        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        MessageHandler msgHandler = new MessageHandler();

        Message msg = msgHandler.GetMessageDetails(CookiesWrapper.thisUserID.ToString(), Convert.ToInt32(Request.QueryString["id"]));

        //Some one is trying to access a mail that is not supposed to see so kick him out
        if (msg == null)
        {
            Response.Redirect("Default.aspx");
        }
        lblSender.Text = msg.SenderId;
        lblReciever.Text = msg.RecieverId;
        lblSubject.Text = msg.Subject;
        lblBody.Text = Server.HtmlDecode(msg.Body);
        lblDate.Text = msg.Date.ToString();
        Session["Message"] = msg;

    }
    protected void btnDone_Click(object sender, EventArgs e)
    {
        Session.Remove("Message");
        Response.Redirect("Default.aspx");
    }
    protected void btnReply_Click(object sender, EventArgs e)
    {
        Response.Redirect("SendMessage.aspx?action=reply");
    }
    protected void btnForward_Click(object sender, EventArgs e)
    {
        Response.Redirect("SendMessage.aspx?action=forward");
    }
}
