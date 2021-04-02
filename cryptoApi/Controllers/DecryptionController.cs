using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace cryptoApi.Controllers
{
    public class DecryptionController : ApiController
    {

        public class IpData
        {
            public string key { get; set; }
            public string data { get; set; }
        }
        //[HttpGet]
        public HttpResponseMessage Post([FromBody] IpData ipData)
        {
            string key = ipData.key;
            string data = ipData.data;
            byte[] iv = new byte[16];
            byte[] decrypted_text = Convert.FromBase64String(data);

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream mem_stream = new MemoryStream(decrypted_text))
                    {
                        using (CryptoStream crypt_stream = new CryptoStream((Stream)mem_stream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader stream_reader = new StreamReader((Stream)crypt_stream))
                            {
                                string res = stream_reader.ReadToEnd();
                                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                                HttpContent con = new StringContent(res);
                                response.Content = con;
                                return response;
                            }
                        }
                    }
                }
            }
            catch (CryptographicException err)
            {
                string res = "Wrong Key entered!!!";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest);
                HttpContent con = new StringContent(res);
                response.Content = con;
                return response;
            }
        }
    }
}