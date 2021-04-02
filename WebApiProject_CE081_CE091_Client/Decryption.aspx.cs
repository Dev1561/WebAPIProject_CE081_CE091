using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.IO;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

namespace WCFProject_CE091_CE081_Client
{
    public partial class WebForm2 : System.Web.UI.Page
    {

        public static string dec_data;
        public static string file_name;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/NotAuthenticated.aspx");
            }
            Panel1.Visible = false;
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (conn)
                {
                    string query = "select FilePath from UserFiles where UserName=@username";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", user);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    GridView1.DataSource = reader;
                    GridView1.DataBind();
                    conn.Close();
                }

                if(GridView1.Rows.Count == 0)
                {
                    TextBox1.Visible = false;
                    Label1.Visible = false;
                    Response.Redirect("~/EmptyView.aspx");
                }
            }
            catch (SqlException err)
            {
                Response.Write(err);
            }
        }

        public class IpData
        {
            public string key { get; set; }
            public string data { get; set; }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Uri baseAddress = new Uri("https://localhost:44390/api/");
            HttpClient client = new HttpClient();
            client.BaseAddress = baseAddress;

            Panel1.Visible = true;
            if (e.CommandName == "Decrypt")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string encrypted_text, decrypted_text, key;

                key = TextBox1.Text.ToString();
                if (key == "")
                {
                    Response.Write("<script>window.alert('Please enter a key!!');</script>");
                    return;
                }

                EncDecServiceReference.EncDecServiceClient ed_service = new EncDecServiceReference.EncDecServiceClient();
                GridViewRow row = GridView1.Rows[rowIndex];

                file_name = row.Cells[2].Text;

                string user = System.Web.HttpContext.Current.User.Identity.Name;
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                try
                {
                    using (conn)
                    {
                        string query = "select TextToByte from UserFiles where UserName=@username and FilePath=@file_name";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@username", user);
                        cmd.Parameters.AddWithValue("@file_name", file_name);
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            encrypted_text = reader.GetValue(0).ToString();

                            IpData ipData = new IpData();
                            ipData.key = key;
                            ipData.data = encrypted_text;

                            string data = JsonConvert.SerializeObject(ipData);
                            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/Json");

                            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Decryption", stringContent).Result;

                            dec_data = response.Content.ReadAsStringAsync().Result;
                            
                        }
                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex);
                }
            }
        }

        protected void download_Click(object sender, EventArgs e)
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string folder = "D:\\temp\\" + user;
            if (file_name == null)
            {
                Response.Write("<script> alert('please decrypt a file to download!')</script>");
                return;
            }
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment; filename="+file_name);
            FileStream fs;
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                file_name = folder + "\\" + file_name;  
                if (File.Exists(file_name))
                {
                    File.Delete(file_name);
                }
 
                using (fs = File.Create(file_name))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(dec_data);
                    fs.Write(title, 0, title.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            Response.TransmitFile(file_name);
            Response.End();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Panel1.Visible = false;
            //int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GridView1.Rows[e.RowIndex];
            file_name = row.Cells[2].Text.ToString();

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (conn)
                {
                    string user = System.Web.HttpContext.Current.User.Identity.Name;
                    string query = "Delete from UserFiles where UserName=@username and FilePath=@file_name";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", user);
                    cmd.Parameters.AddWithValue("@file_name", file_name);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (SqlException er)
            {
                Response.Write(er);
            }
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
    }
}