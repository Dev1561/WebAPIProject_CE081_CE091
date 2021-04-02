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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cryptoApi.Controllers
{
    public class EncryptionController : ApiController
    {
        public class IpData
        {
            public string key { get; set; }
            public string data { get; set; }
        }

        public HttpResponseMessage Post( [FromBody] IpData inputData)
        {
            string key = inputData.key;
            string data = inputData.data;

            byte[] iv = new byte[16];
            byte[] encrypted_data;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream mem_stream = new MemoryStream())
                {
                    using (CryptoStream crypt_stream = new CryptoStream((Stream)mem_stream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter stream_writer = new StreamWriter((Stream)crypt_stream))
                        {
                            stream_writer.Write(data);
                        }
                        encrypted_data = mem_stream.ToArray();
                    }
                }
            }
            string res = Convert.ToBase64String(encrypted_data);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            StringContent con = new StringContent(res);
            response.Content = con;
            return response;
        }


    }
}