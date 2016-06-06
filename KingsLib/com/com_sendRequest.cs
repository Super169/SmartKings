using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using MyUtil;
using System.Net;

namespace KingsLib
{
    public static partial class com
    {

        public static RequestReturnObject SendRequest(ConnectionInfo ci, string requestText)
        {
            RequestReturnObject rro = new RequestReturnObject();
            rro.success = false;
            rro.ok = -1;
            rro.msg = "";
            rro.requestText = requestText;

            try
            {
                string result = "";
                try
                {

                    HttpContent _Body = new StringContent(requestText);
                    _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();

                    if (ci.cookies.Count > 0)
                    {
                        HttpClientHandler handler = new HttpClientHandler();
                        handler.CookieContainer = new CookieContainer();
                        foreach (KeyValuePair<string, string> o in ci.cookies)
                        {
                            handler.CookieContainer.Add(new Uri(ci.uri), new Cookie(o.Key, o.Value));
                        }
                        client = new HttpClient(handler);
                    } else
                    {
                        client = new HttpClient();
                    }
                    foreach (KeyValuePair<string, string> o in ci.headers)
                    {
                        // NEVER set the Content-Length & Content-Type for using HttpClient
                        if ((o.Key != "Content-Length") && (o.Key != "Content-Type"))
                        {
                            client.DefaultRequestHeaders.Add(o.Key, o.Value);
                        }
                    }

                    HttpResponseMessage response = client.PostAsync(ci.fullPath, _Body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        HttpContent content = response.Content;
                        result = content.ReadAsStringAsync().Result;
                        rro.success = true;
                        rro.responseText = result;
                        rro.responseJson = com.getJsonFromResponse(rro.responseText, true);
                        if (rro.responseJson != null)
                        {
                            if (rro.responseJson["ok"] != null) rro.ok = (int)rro.responseJson["ok"];
                            if (rro.responseJson["style"] != null) rro.style = rro.responseJson["style"];
                            if (rro.responseJson["prompt"] != null) rro.prompt = rro.responseJson["prompt"];
                        }

                    }

                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }

            }
            catch (Exception ex)
            {
                rro.success = false;
                rro.msg = ex.Message;
            }

            return rro;
        }

        public static RequestReturnObject SendGenericRequest(ConnectionInfo ci, string sid, string act, bool addSId = true, string body = null)
        {
            dynamic json;
            string requestText = "";
            RequestReturnObject rro;

            try
            {
                json = Json.Decode("{}");
                json.act = act;
                if (addSId) json.sid = sid;
                if (body != null) json.body = body;
                requestText = Json.Encode(json);
                rro = SendRequest(ci, requestText);
            }
            catch (Exception ex)
            {
                rro = new RequestReturnObject();
                rro.success = false;
                rro.ok = -1;
                rro.msg = ex.Message;
            }
            return rro;
        }
    }
}
