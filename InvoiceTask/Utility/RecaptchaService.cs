using System.Text.Json.Nodes;

namespace InvoiceTask.Utility
{
    public class RecaptchaService
    {
        public static async Task<bool> verifyReCaptcha(string response, string secret)
        {
            using (var client = new HttpClient())
            {
                string Url = "https://www.google.com/recaptcha/api/siteverify";
                MultipartFormDataContent content = new();
                content.Add(new StringContent(response), "response");  
                content.Add(new StringContent(secret), "secret");  
                var result= await client.PostAsync(Url, content);
                if (result.IsSuccessStatusCode) 
                {
                    var strResponse= await result.Content.ReadAsStringAsync();
                    var jsonResponse=JsonNode.Parse(strResponse);
                    if (jsonResponse !=null)
                    {
                        var success = ((bool?)jsonResponse["success"]);
                        if (success != null&& success==true)return true;
                    }
                }
            }
            return false;
        }
    }
}
