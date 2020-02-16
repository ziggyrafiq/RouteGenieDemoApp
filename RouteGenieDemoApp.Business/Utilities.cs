using RouteGenieDemoApp.Business.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RouteGenieDemoApp.Business
{
    public static class Utilities
    {
        public static string Slugify(string phrase, int maxLength = 200)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return "";

            string str = phrase.ToLower();


            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = Regex.Replace(str, @"[\s-]+", " ").Trim();

            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();

            str = Regex.Replace(str, @"\s", "-");

            return str;
        }

        /// <summary>
        /// Calculate the File Data to create ETAG
        /// 
        /// </summary>
        /// <param name="fileData">byte[] FileData</param>
        /// <returns>joined string</returns>
        public static string CalculateHash(byte[] fileData)
        {
            var cryptoServiceProvider = new MD5CryptoServiceProvider();
            cryptoServiceProvider.ComputeHash(fileData);
            return string.Join("", cryptoServiceProvider.Hash.Select(c => c.ToString("x2")));
        }


        public static string UploadFile(HttpPostedFileBase file)
        {

            if (file != null)
            {
                string url = "";

                switch (file.ContentType)
                {
                    case "image/jpeg":
                    case "image/png":
                    case "image/bmp":
                    case "image/gif":
                        url = "/Content/images/";
                        break;

                    default:
                        url = "/Content/files/";
                        break;
                }

                string directory = System.Web.HttpContext.Current.Server.MapPath("~" + url);

                string path;
                var fileName = GetUniqueFileName(directory, file.FileName, out path);
                fileName = Path.Combine(directory, fileName);

                file.SaveAs(fileName);
                return fileName;
            }
            return string.Empty;
        }

        public static string GetUniqueFileName(string directory, string filename, out string filepath)
        {
            using (var crypto = new Cryptographer())
            {
                bool unique = false;
                string origFilename = filename;
                do
                {
                    filename = string.Format("{0}-{1}", crypto.CreateUniqueKey(4), filename);
                    filepath = Path.Combine(directory, filename);

                    if (!File.Exists(filepath))
                        unique = true;

                } while (!unique);
                return filename;
            }
        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str) || str.Length < 1)
                {
                    return str = ".";
                }
                else
                {
                    return str.Substring(0, Math.Min(str.Length, maxLength));

                }

            }
            catch
            {
                return str = ".";

            }

        }


        /// <summary>
        /// Get a substring of the first N characters.
        /// </summary>
        public static string TruncateString(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }

            return source;
        }


        /// <summary>
        /// Format Result formats any string. Best used for Search Results.
        /// </summary>
        /// <param name="SearchWord">Pass a String Search Word or SearchString</param>
        /// <param name="SearchResultContent">Pass throw Search Result string. such as Search Result Content as a String</param>
        /// <param name="SearchResultContentLength">Set a  limt to a Return format Search Result String Length</param>
        /// <returns>Format Search Result String</returns>
        public static string FormatSearchResult(string SearchWord, string SearchResultContent, int SearchResultContentLength, bool IsForHTML)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int count = 0;
            int i = 0;
            int searchWordLength = !string.IsNullOrWhiteSpace(SearchWord) ? SearchWord.Length : 0;
            int temp = 0;
            int tempLegth = 0;
            int contentWordsAfter = 5;  
            if (string.IsNullOrWhiteSpace(SearchResultContent))
                SearchResultContent = " ";

            
            if (!string.IsNullOrWhiteSpace(SearchWord))
            {

                while ((i = SearchResultContent.IndexOf(SearchWord, i, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    temp = i - searchWordLength;
                    tempLegth = i += SearchWord.Length;
                    int z = SearchResultContent.Length - i;
                    sb.Append(temp < 0 ? SearchResultContent.Substring(0, SearchWord.Length + contentWordsAfter) : SearchResultContent.Substring(i - searchWordLength, z)).AppendLine();
                    count++;
                }
            }
            return !string.IsNullOrWhiteSpace(sb.ToString())
                ? Regex.Replace(sb.ToString().Length > SearchResultContentLength - 1
                ? sb.ToString().Substring(0, SearchResultContentLength) : sb.ToString(), SearchWord, IsForHTML ? "<b>" + SearchWord + "</b>" : SearchWord, RegexOptions.IgnoreCase) :
                 !string.IsNullOrWhiteSpace(SearchResultContent.Substring(0, SearchResultContent.Length >= SearchResultContentLength
                 ? SearchResultContentLength : SearchResultContent.Length))
                 ? SearchResultContent.Substring(0, SearchResultContent.Length >= SearchResultContentLength
                 ? SearchResultContentLength : SearchResultContent.Length) :
                 "Apologies there is no Abstract or Any type of  Content available for this file/data.";
        }
    }
}
