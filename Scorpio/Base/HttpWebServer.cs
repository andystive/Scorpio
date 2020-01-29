using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Scorpio.Base
{
    public class HttpWebServer
    {
        private HttpListener _listener;
        private Func<string, string> _responderMethod;

        public HttpWebServer(string[] prefixes, Func<string, string> method)
        {
            try
            {
                _listener = new HttpListener();

                if (!HttpListener.IsSupported)
                    throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

                // URL prefixes are required, for example
                // "http://localhost:8080/index"
                if (prefixes == null || prefixes.Length == 0)
                    throw new ArgumentException("prefixes");

                // A reponder method is required
                if (method == null)
                    throw new ArgumentException("method");

                foreach (string s in prefixes)

            }
        }
    }
