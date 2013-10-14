using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace System
{
    public class cURL
    {
        private HttpWebRequest request;
        public static CookieContainer cookieJar;
        private CookieContainer privateCookieJar;
        private bool usePrivateCookieJar = false;
        private string referer;
        public int timeout = 100000;

        public cURL()
        {
        }

        public cURL(string r, bool usePrivateCookieJar)
        {
            this.referer = r;
            if (usePrivateCookieJar)
            {
                this.usePrivateCookieJar = true;
                this.privateCookieJar = new CookieContainer();
            }
            else
            {
                if (cookieJar == null)
                    cookieJar = new CookieContainer();
            }
        }

        public cURL(string r, int timeout)
        {
            this.referer = r;
            this.timeout = timeout;
            if (cookieJar == null)
                cookieJar = new CookieContainer();
        }

        public cURL(string r)
        {
            this.referer = r;
            if (cookieJar == null)
                cookieJar = new CookieContainer();
        }

        public cURL(CookieContainer cookies)
        {
            cookieJar = cookies;
        }

        public string post(string url, string data)
        {
            this.request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = this.timeout;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:8.0) Gecko/20100101 Firefox/8.0";
            request.Referer = this.referer;
            if (!usePrivateCookieJar)
                request.CookieContainer = cookieJar;
            else
                request.CookieContainer = privateCookieJar;

            if (!data.Equals(""))
            {
                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream;
                try { dataStream = request.GetRequestStream(); }
                catch (Exception e) { return null; }
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            this.referer = url;
            string responseFromServer;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStreamR = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStreamR);
                responseFromServer = reader.ReadToEnd();

                reader.Close();
                dataStreamR.Close();
                response.Close();
            }
            catch (WebException e)
            {
                return null;
            }
           
            return responseFromServer;

        }

        public string post(string url, string[][] data)
        {
            string sData = "";
            for (int i = 0; i < data[0].Length; i++)
            {
                sData += data[0][i] + "=" + data[1][i];
                if (i != data[0].Length - 1)
                    sData += "&";
            }

            return post(url, sData);
        }

        public string post(string url)
        {
            return post(url, "");
        }

        public string get(string url)
        {
            return post(url, "");
        }


        public string getReferer()
        {
            return this.referer;
        }
        public void setReferer(string r)
        {
            this.referer = r;
        }





        public static string textBetween(string str, string startPart, string endPart, int cropStrStart)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(startPart) || String.IsNullOrEmpty(endPart) || cropStrStart < 0)
                return null;

            if (cropStrStart != 0)
                str = str.Substring(cropStrStart, str.Length-cropStrStart);

            int startIndex = str.IndexOf(startPart);

            string temp = str.Substring(startIndex + startPart.Length, str.Length - startIndex - startPart.Length);

            int endIndex = temp.IndexOf(endPart);
            return temp.Substring(0, endIndex);
        }
        public static string textBetween(string str, string startPart, string endPart)
        {
            return textBetween(str, startPart, endPart, 0);
        }
        public static string textBetween(string str, string endPart)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(endPart))
                return null;

            int endIndex = str.IndexOf(endPart);
            return str.Substring(0, endIndex);
        }
    }
}