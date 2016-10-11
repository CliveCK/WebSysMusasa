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

public partial class Confirmation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["SuccessList"] != null)
        {
            string[] successList = (string[])Session["SuccessList"];
            foreach (string item in successList)
            {
                if (item != null &&
                    item.Trim() != string.Empty)
                {
                    lblSuccess.Text += Server.HtmlDecode(item + "<br/>");
                }
            }
            Session.Remove("SuccessList");
        }

        if (Session["FailList"] != null)
        {
            string[] failList = (string[])Session["FailList"];
            foreach (string item in failList)
            {
                if (item != null &&
                    item.Trim() != string.Empty)
                {
                    lblFail.Text += Server.HtmlDecode(item + "<br/>");
                }
            }
            Session.Remove("FailList");
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
