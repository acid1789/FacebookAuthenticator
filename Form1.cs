using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Facebook;

namespace FacebookAuthenticator
{
    public partial class Form1 : Form
    {
        string _appId;
        public Form1(string appId)
        {
            _appId = appId;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string url = string.Format("https://www.facebook.com/v2.9/dialog/oauth?client_id={0}&redirect_uri={1}&scope=email&response_type=token", _appId, "https://www.facebook.com/connect/login_success.html");
            webBrowser1.Url = new Uri(url);
        }

        private void webBrowser1_LocationChanged(object sender, EventArgs e)
        {
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            System.Diagnostics.Debug.Print("Navigated: " + e.Url);

            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            string accessToken = null;

            FacebookClient fb = new FacebookClient();
            FacebookOAuthResult oauthResult;
            if (fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                if (oauthResult.IsSuccess)
                {
                    accessToken = oauthResult.AccessToken;
                }
                else
                {
                    string errorDescription = oauthResult.ErrorDescription;
                    string errorReason = oauthResult.ErrorReason;
                }
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                return;
            }

            string output = "Credentials=Error";
            if (accessToken != null)
            {
                fb = new FacebookClient(accessToken);
                Dictionary<string, object> result = new Dictionary<string, object>((IDictionary<string, object>)fb.Get("me?fields=name,id,email"));
                output = string.Format("Credentials={0}$?${1}$?${2}", result["name"], result["id"], result["email"]);                
            }

            Console.WriteLine(output);
            Application.Exit();
        }
    }
}
