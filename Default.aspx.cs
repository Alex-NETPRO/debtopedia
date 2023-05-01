using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DebtopediaDM
{
    public partial class Default : System.Web.UI.Page
    {
        private AlexRose rj = new AlexRose();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack == false)
            {
                string Schoolidx = Convert.ToString(Session["schoolidx"]);
                if (string.IsNullOrEmpty(Schoolidx) == true || Schoolidx == "")
                { Response.Redirect("Login.aspx"); }
                else
                {
                    schoolid.Text = Schoolidx;
                    DataSet Ds = new DataSet();
                    Ds = rj.SelectDataSet("declare @schoolid as bigint,@term as varchar(100),@termid as bigint set @schoolid = " + schoolid.Text + " set @term = coalesce((select top 1 (firstsecondthird + ' ' + sessionx) from d_terms where schoolid = @schoolid and getdate() between startdate and endtime),'Not in Session') set @termid = coalesce((select top 1 id from d_terms where schoolid = @schoolid and getdate() between startdate and endtime),0) select schoolname,location,states,email,phone,pasword,city,@term as term,(case migrated when 0 then 'NO' else 'YES' end) as Migrate from d_schools where id = @schoolid ");
                    schoolname.Text = Ds.Tables[0].Rows[0]["schoolname"].ToString();
                    termname.Text = Ds.Tables[0].Rows[0]["term"].ToString();
                    string Migrate = Ds.Tables[0].Rows[0]["Migrate"].ToString();
                    if (Migrate.Trim().ToUpper() == "YES") { Button3.Visible = false; }

                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile == true)
            {

                DataTable Dt = new DataTable();
                System.Data.OleDb.OleDbConnection myConnection;
                DataSet DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                string filePath = Server.MapPath("Uploads/" + schoolid.Text.Trim() + ".xlsx");
                FileUpload1.SaveAs(filePath);

                myConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0; Data Source='" + filePath + "';Extended Properties=Excel 12.0;");
                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", myConnection);
                DtSet = new System.Data.DataSet();
                MyCommand.Fill(DtSet, "[Sheet1$]");
                Dt = DtSet.Tables[0];
                myConnection.Close();

                if (Dt.Rows.Count == 0) { goto rerun; }

                string studentname, gender, occupation, address, lga, parentname, parentphone, parentemail, debt;

                for (int i = 0; i < (Dt.Rows.Count); i = i + 1)
                {
                    if (string.IsNullOrEmpty(Dt.Rows[i][0].ToString()) == true) { studentname = ""; } else { studentname = Dt.Rows[i][0].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][1].ToString()) == true) { gender = ""; } else { gender = Dt.Rows[i][1].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][2].ToString()) == true) { debt = ""; } else { debt = Dt.Rows[i][2].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][3].ToString()) == true) { parentname = ""; } else { parentname = Dt.Rows[i][3].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][4].ToString()) == true) { parentphone = ""; } else { parentphone = Dt.Rows[i][4].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][5].ToString()) == true) { parentemail = ""; } else { parentemail = Dt.Rows[i][5].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][6].ToString()) == true) { occupation = ""; } else { occupation = Dt.Rows[i][6].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][7].ToString()) == true) { address = ""; } else { address = Dt.Rows[i][7].ToString(); }
                    if (string.IsNullOrEmpty(Dt.Rows[i][8].ToString()) == true) { lga = ""; } else { lga = Dt.Rows[i][8].ToString(); }
                    rj.AnySQLQuery("INSERT INTO d_student (schoolid,[fullname],[pname],[pphone],[pemail],[poccupation],[debt],[activ],[gender],[homeaddres],[blacklist],[lga],[confirmed]) VALUES (" + schoolid.Text + ",'" + studentname.Replace("'", "") + "','" + parentname.Replace("'", "") + "','" + parentphone.Replace("'", "") + "','" + parentemail.Replace("'", "") + "','" + occupation.Replace("'", "") + "'," + rj.vall(debt).ToString() + ",0,'" + gender.Replace("'", "") + "','" + address.Replace("'", "") + "',1,'" + lga.Replace("'", "") + "',0)");
                }
                rj.AnySQLQuery("update d_schools set migrated = 1 where id = " + schoolid.Text);
                Button3.Visible = false;
                ClientScript.RegisterStartupScript(this.GetType(), "load", "swal('Uploaded!','','success')", true);
            rerun:
                Button3.Text = "Upload List";
            }

        }



    }
}