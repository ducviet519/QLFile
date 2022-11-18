using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebTools.Router
{
    public class CustomRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            string sURL_Path = request.Path.Value;

            if (sURL_Path.Contains("."))   //*check for IsFile
            {
                //if file like *.js or *.css do nothing
            }
            else if (Regex.IsMatch(sURL_Path, @"=|🏠|📚|📄|📂|📝|🔎", RegexOptions.IgnoreCase))   //*if /📚/📂=
            {
                //if redirect with parameters
            }
            //*default pattern: "📄/{IDArticle}",
            else if (request.QueryString.HasValue)
            {
                //----< Check for numeric ?123 >----
                string sQuerystring = request.QueryString.Value;    //?1234
                string sNumericTest = sQuerystring.Substring(1);//1234
                if (float.TryParse(sNumericTest, out _))
                {
                    //-< isNumeric >-
                    string sNewURL = "/📄/" + sNumericTest;
                    //context.Request.Path = sNewURL;   //*simple rewrite does not work
                    response.Redirect(sNewURL, permanent: true);    //guide to new url
                    return;   // short circuit
                    //-</ isNumeric >-
                }
                //----</ Check for numeric ?123 >----
            }
            else if (request.QueryString.HasValue == false)  //*check for old URL-Folder Paths
            {
                //----< QueryParameters is empty not ?123 >----
                string sPath = request.Path;
                if (sPath.StartsWith("/")) sPath = sPath.Substring(1);
                if (sPath != "")
                {
                    //-< change path to parameters >-
                    string sNewURL = "";
                    if (sPath.Contains("Search/", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string sSearchWords = sPath.Replace("Search/", "", StringComparison.InvariantCultureIgnoreCase);
                        //sSearchWords = Correct_old.decode_SemiCharactoers_in_Path(sSearchWords);
                        //*old folders in path
                        sNewURL = "/📚?🔎=" + sSearchWords;
                    }
                    else
                    {
                        //*old folders in path
                        //string sNewFolderPath = Correct_old.decode_SemiCharactoers_in_Path(sPath);
                        //sNewURL = "/📚?📂=" + sNewFolderPath;
                    }
                    //option1: rewrite without changing visible URL
                    //context.Request.Path = sNewURL;
                    //note: .redirect dos not solve ~/
                    response.Redirect(sNewURL, permanent: true);
                    return;   // short circuit
                    //-</ change path to parameters >-
                }
                //----</ QueryParameters is empty not ?123 >----
            }

            response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = RuleResult.EndResponse;
        }
    }
}
