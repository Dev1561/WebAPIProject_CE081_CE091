using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WCFProject_CE091_CE081_Client
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static string generated_key = null;
        private static Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        protected void Page_Load(object sender, EventArgs e)
        {
            if(! User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/NotAuthenticated.aspx");
            }
            Label2.Text = null;
            Label1.Text = null;
            Label3.Text = null;

        }

        protected bool isAlphanumeric(string str)
        {
            for(int i = 0; i < str.Length; i++)
            {
                if( (str[i] <= 'z' && str[i] >= 'a') || (str[i] <= '9' && str[i] >= '0') || (str[i] >= 'A' && str[i] <= 'Z'))
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        public class IpData
        {
            public string key { get; set; }
            public string data { get; set; }
        }

        protected void EncryptData(object sender, EventArgs e)
        {
            Uri baseAddress = new Uri("https://localhost:44390/api/");
            HttpClient client = new HttpClient();
            client.BaseAddress = baseAddress;


            if (FileUpload1.HasFile == false)
            {
                Label2.Text = "please select a file";
                Label2.ForeColor = System.Drawing.Color.Red;
                return;
            }

            string key;
            if (TextBox1.Text.Length != 32 || isAlphanumeric(TextBox1.Text.ToString() ) == false)
            {
                Label1.Text = "key must be of 32 char long AlphaNumberic value";
                Label1.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            {
                key = TextBox1.Text.ToString();
            }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try
            {
                using (conn)
                {
                    string query = "insert into UserFiles(UserName, FilePath, TextToByte) values (@username, @filepath, @texttobyte)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    EncDecServiceReference.EncDecServiceClient service = new EncDecServiceReference.EncDecServiceClient();
                    

                    string file_content = null;
                    using (StreamReader reader = new StreamReader(FileUpload1.PostedFile.InputStream))
                    { 
                        file_content = reader.ReadToEnd();
                    }
                    string file_path = FileUpload1.FileName;


                    IpData ipData = new IpData();
                    ipData.key = key;
                    ipData.data = file_content;

                    string data = JsonConvert.SerializeObject(ipData);
                    StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/Json");

                    HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Encryption", stringContent).Result;

                    string encrypted_text = response.Content.ReadAsStringAsync().Result;

                    cmd.Parameters.AddWithValue("@username", System.Web.HttpContext.Current.User.Identity.Name);
                    cmd.Parameters.AddWithValue("@filepath", file_path);
                    cmd.Parameters.AddWithValue("@texttobyte", encrypted_text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Label3.Text = "<h3> Your File has been successfully added to the database !!! </h3>";
                    Label3.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch(SqlException err)
            {
                Label3.Text = "You cannot have multiple files with same name!!! ";
                Label3.ForeColor = System.Drawing.Color.Red;
                //Label3.Text += err.Message;
            }
            catch(System.ServiceModel.ProtocolException ex)
            {
                Label3.Text = "File Format not supported!";
                Label3.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GenKey_Click(object sender, EventArgs e)
        {
            string user = System.Web.HttpContext.Current.User.Identity.Name;
            string folder = "D:\\temp\\" + user;
            string generated_key = getRandomKey();
            //var gen_key = new StringBuilder();
            string key_file_name = null;
            //for(int i=0; i<32; i++)
            //{
            //    var temp = chars[random.Next(0, chars.Length)];
            //    gen_key.Append(temp);
            //}
            //TextBox1.Text = gen_key.ToString();
            TextBox1.Text = generated_key;

            //Response.Write(generated_key);

            string download_filename = "";
            FileStream fs;
            try
            {
                if(! Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (FileUpload1.HasFile)
                {
                    key_file_name = folder + "\\key_"+ FileUpload1.FileName;
                    download_filename = "key_"+ FileUpload1.FileName ;
                }
                else
                {
                    key_file_name = folder + "\\generated_key.txt";
                    download_filename = "generated_key.txt";
                }

                if (File.Exists(key_file_name))
                {
                    File.Delete(key_file_name);
                }

                using (fs = File.Create(key_file_name))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes(generated_key);
                    fs.Write(title, 0, title.Length);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
            Response.ContentType = "text/plain";
            Response.AppendHeader("Content-Disposition", "attachment; filename="+download_filename);
            Response.TransmitFile(key_file_name);
            Response.End();
        }

        protected string getRandomKey()
        {
            var gen_key = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                var temp = chars[random.Next(0, chars.Length)];
                gen_key.Append(temp);
            }
            return gen_key.ToString();
        }
    }
}