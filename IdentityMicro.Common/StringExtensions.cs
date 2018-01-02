using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Tcig.Platform.Common
    {
        /// <summary>
        /// Summary description for Extensions
        /// </summary>
        public static class StringExtensions
        {
            /// <summary>
            /// Converts to string.
            /// </summary>
            /// <param name="e">The e.</param>
            /// <returns></returns>
            public static string ConvertToString(Enum e)
            {
                // Get the Type of the enum
                var t = e.GetType();

                // Get the FieldInfo for the member field with the enums name
                var info = t.GetField(e.ToString("G"));

                // Check to see if the XmlEnumAttribute is defined on this field
                if (!info.IsDefined(typeof(XmlEnumAttribute), false))
                {
                    // If no XmlEnumAttribute then return the string version of the enum.
                    return e.ToString("G");
                }

                // Get the XmlEnumAttribute
                var o = info.GetCustomAttributes(typeof(XmlEnumAttribute), false);
                var att = (XmlEnumAttribute)o[0];
                return att.Name;
            }


            /// <summary>
            /// Retrieve the description on the enum, e.g.
            /// [Description("Bright Pink")]
            /// BrightPink = 2,
            /// Then when you pass in the enum, it will retrieve the description
            /// </summary>
            /// <param name="en">The Enumeration</param>
            /// <returns>A string representing the friendly name</returns>
            public static string GetDescription(Enum en)
            {
                var type = en.GetType();

                var memInfo = type.GetMember(en.ToString());

                if (memInfo != null && memInfo.Length > 0)
                {
                    var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }

                return en.ToString();
            }
            /// <summary>
            /// Convert String Initial Case
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static string ToInitCase(this string input)
            {
                var pattern = @"(\b)(\w)(\w*\b)";
                var results = Regex.Replace(input, pattern, delegate (Match m)
                { return m.Groups[1].Value + m.Groups[2].Value.ToUpper() + m.Groups[3].Value; });

                return results;
            }

            /// <summary>
            /// Returns the given string truncated to the specified length, suffixed with an elipses (...)
            /// </summary>
            /// <param name="input"></param>
            /// <param name="length">Maximum length of return string</param>
            /// <returns></returns>
            public static string Truncate(this string input, int length)
            {
                return Truncate(input, length, "...");
            }

            /// <summary>
            /// Returns the given string truncated to the specified length, suffixed with the given value
            /// </summary>
            /// <param name="input"></param>
            /// <param name="length">Maximum length of return string</param>
            /// <param name="suffix">The value to suffix the return value with (if truncation is performed)</param>
            /// <returns></returns>
            public static string Truncate(this string input, int length, string suffix)
            {
                if (input == null) return "";
                if (input.Length <= length) return input;

                if (suffix == null) suffix = "...";

                return input.Substring(0, length - suffix.Length) + suffix;
            }

            /// <summary>
            /// Splits a given string into an array based on character line breaks
            /// </summary>
            /// <param name="input"></param>
            /// <returns>String array, each containing one line</returns>
            public static string[] ToLineArray(this string input)
            {
                if (input == null) return new string[] { };
                return System.Text.RegularExpressions.Regex.Split(input, "\r\n");
            }

            /// <summary>
            /// Splits a given string into a strongly-typed list based on character line breaks
            /// </summary>
            /// <param name="input"></param>
            /// <returns>Strongly-typed string list, each containing one line</returns>
            public static List<string> ToLineList(this string input)
            {
                var output = new List<string>();
                output.AddRange(input.ToLineArray());
                return output;
            }

            /// <summary>
            /// Replaces line breaks with self-closing HTML 'br' tags
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static string ReplaceBreaksWithBR(this string input)
            {
                return string.Join("<br/>", input.ToLineArray());
            }

            /// <summary>
            /// Replaces any single apostrophes with two of the same
            /// </summary>
            /// <param name="input"></param>
            /// <returns>String</returns>
            public static string DoubleApostrophes(this string input)
            {
                return Regex.Replace(input, "'", "''");
            }

            /// <summary>
            /// Encodes the input string as HTML (converts special characters to entities)
            /// </summary>
            /// <param name="input"></param>
            /// <returns>HTML-encoded string</returns>
            public static string ToHTMLEncoded(this string input)
            {
                //return HttpContext.Current.Server.HtmlEncode(input);
                return HttpUtility.HtmlEncode(input);
            }

            /// <summary>
            /// Encodes the input string as a URL (converts special characters to % codes)
            /// </summary>
            /// <param name="input"></param>
            /// <returns>URL-encoded string</returns>
            public static string ToURLEncoded(this string input)
            {
                //return HttpContext.Current.Server.UrlEncode(input);
                return HttpUtility.UrlEncode(input);
            }

            /// <summary>
            /// Decodes any HTML entities in the input string
            /// </summary>
            /// <param name="input"></param>
            /// <returns>String</returns>
            public static string FromHTMLEncoded(this string input)
            {
                return HttpUtility.HtmlDecode(input);
            }

            /// <summary>
            /// Decodes any URL codes (% codes) in the input string
            /// </summary>
            /// <param name="input"></param>
            /// <returns>String</returns>
            public static string FromURLEncoded(this string input)
            {
                return HttpUtility.UrlDecode(input);
            }

            /// <summary>
            /// Converts the object to save string to avoid exception.
            /// </summary>
            /// <param name="CreatedBy">  </param> 
            /// <param name="CreatedDate"> </param>
            /// <param name="ModifiedDate"> </param>
            /// <param name="input">string value </param>
            /// <returns>Retruns object value</returns>
            public static object ToDbSafeString(this string input)
            {
                if (!string.IsNullOrEmpty(input))
                    return input;
                else
                    return DBNull.Value;
            }

            /// <summary>
            /// Sets the default enum.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <param name="defaultEnumValue">The default enum value.</param>
            /// <returns></returns>
            public static string SetDefaultEnum(this string input, string defaultEnumValue)
            {
                if (String.IsNullOrEmpty(input) == true)
                {
                    input = defaultEnumValue;
                }

                return input;
            }

            /// <summary>
            /// Manuplate String to prevent SQL Injection
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public static string PreventSQLInjection(this string input)
            {
                var results = input.Replace("'", "''");
                return results;
            }

            /// <summary>
            /// Appends the specified string.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <param name="str">The string.</param>
            /// <returns></returns>
            public static string Append(this string input, string str)
            {
                if (input == null || str == null) return "";
                return input + str;
            }

            /// <summary>
            /// Removes the last comma.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns></returns>
            public static string RemoveLastComma(this string input)
            {
                if (input == null || input.Length < 3) return "";

                input = input.TrimEnd(' ');
                if (input.LastIndexOf(",") == input.Length - 1)
                {
                    input = input.Substring(0, input.Length - 1);
                }
                return input;
                //return input.Substring(0, input.Length - 3);
            }

            /// <summary>
            /// Convert new line to <br /> tags
            /// </summary>
            /// <param name="html"></param>
            /// <returns>html string</returns>
            public static string ConvertHtmlLineBreak(this string html)
            {
                return html.Replace("\n", "<br />").Replace("\r\n", "<br />");
            }

            /// <summary>
            /// Encodes to Json Format
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static string EncodeJSON(this string s)
            {
                if (s == null || s.Length == 0)
                {
                    return "\"\"";
                }
                char c;
                int i;
                var len = s.Length;
                var sb = new StringBuilder(len + 4);
                string t;

                sb.Append('"');
                for (i = 0; i < len; i += 1)
                {
                    c = s[i];
                    //if (c == '>')
                    //{
                    //    sb.Append('\\');
                    //    sb.Append('\\');
                    //    sb.Append(c);
                    //}
                    //else 
                    if ((c == '\\') || (c == '"') || (c == '>'))
                    {
                        sb.Append('\\');
                        sb.Append(c);
                    }
                    else if (c == '\b')
                        sb.Append("\\b");
                    else if (c == '\t')
                        sb.Append("\\t");
                    else if (c == '\n')
                        sb.Append("\\n");
                    else if (c == '\f')
                        sb.Append("\\f");
                    else if (c == '\r')
                        sb.Append("\\r");
                    else
                    {
                        if (c < ' ')
                        {
                            var tmp = new string(c, 1);

                            try
                            {

                                t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);

                                sb.Append("\\u" + t.Substring(t.Length - 4));
                            }
                            catch (Exception)
                            {

                                //throw;
                            }
                        }
                        else
                        {
                            sb.Append(c);
                        }
                    }
                }
                sb.Append('"');
                return sb.ToString();
            }

            public static string EscapeXml(this string s)
            {
                var objXmlDoc = new XmlDocument();
                objXmlDoc.XmlResolver = null;
                var objXmlElement = objXmlDoc.CreateElement("temp");
                objXmlElement.InnerText = s;
                return objXmlElement.InnerXml;
            }

            /// <summary>
            /// Makes the tiny url from the tinyurl service.
            /// </summary>
            /// <param name="Url"></param>
            /// <returns></returns>
            public static string MakeTinyUrl(this string Url)
            {
                try
                {
                    if (Url.Length <= 100)
                    {
                        return Url;
                    }
                    if (!Url.ToLower().StartsWith("http") && !Url.ToLower().StartsWith("ftp"))
                    {
                        Url = "http://" + Url;
                    }
                    var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + Url);
                    var res = request.GetResponse();
                    string text;
                    using (var reader = new StreamReader(res.GetResponseStream()))
                    {
                        text = reader.ReadToEnd();
                    }
                    return text;
                }
                catch (Exception)
                {
                    return Url;
                }
            }

            /// <summary>
            /// Limits string to Specific value
            /// </summary>
            /// <param name="valueString"></param>
            /// <param name="numberOfChars"></param>
            /// <param name="trailingString"></param>
            /// <returns></returns>
            public static string LimitString(this string valueString, int numberOfChars, string trailingString)
            {
                var retrunString = "";
                if (String.IsNullOrEmpty(valueString) == false)
                {
                    var TotalChars = numberOfChars;
                    if (valueString.Length > TotalChars)
                    {
                        valueString = valueString.Substring(0, TotalChars).Trim();
                        //valueString = valueString + "...";
                        valueString = valueString + trailingString;
                        retrunString = valueString;
                    }
                    else
                    {
                        retrunString = valueString;
                    }
                }
                return retrunString;

            }

            /// <summary>
            /// Limits String to breakingchar by the nearest of numberof Chars.
            /// Like u always want to break string on space.
            /// author:Vakas
            /// </summary>
            /// <param name="valueString"></param>
            /// <param name="numberOfChars"></param>
            /// <param name="breakingChar"></param>
            /// <param name="trailingString"></param>
            /// <returns></returns>
            public static string LimitStringByChar(this string valueString, int numberOfChars, char breakingChar, string trailingString)
            {
                var retrunString = "";
                if (String.IsNullOrEmpty(valueString) == false)
                {
                    var TotalChars = numberOfChars;
                    if (valueString.Length > TotalChars)
                    {
                        var character = Convert.ToChar(valueString.Substring(numberOfChars, 1));
                        valueString = valueString.Substring(0, TotalChars).Trim();
                        if (character.Equals(breakingChar) == false)
                        {
                            var lastSpacePos = valueString.LastIndexOf(breakingChar);
                            if (lastSpacePos > 0)
                            {
                                valueString = valueString.Substring(0, lastSpacePos);
                            }
                        }

                        valueString = valueString + trailingString;
                        retrunString = valueString;
                    }
                    else
                    {
                        retrunString = valueString;
                    }
                }
                return retrunString;

            }

            /// <summary>
            /// This method return that match word exist in complete string or not
            /// </summary>
            /// <param name="matchWord">Word or string To check</param>
            /// <param name="CompleteString">String in which we check that match word exist or not</param>
            /// <returns></returns>
            public static bool IsWordExistInString(this string CompleteString, string matchWord)
            {
                var regex = new Regex(@"\b" + matchWord + @"\b", RegexOptions.IgnoreCase);
                if (regex.IsMatch(CompleteString))
                {
                    return true;
                }
                else
                    return false;
            }


            /// <summary>
            /// Remove Invalid charactes from file name
            /// </summary>
            /// <param name="filename"></param>
            /// <returns></returns>
            public static string RemoveInvalidCharactersFromFileName(this string filename)
            {
                if (string.IsNullOrWhiteSpace(filename))
                    return filename;

                var reg = "~!@#$%^&*()_+,'|?:;<>/\\ ";
                var r2 = new Regex(string.Format("[{0}]", Regex.Escape(reg)));
                return r2.Replace(filename.Trim(), "");
            }

            /// <summary>
            /// Remove spaces from string
            /// </summary>
            /// <param name="stringtext"></param>
            /// <returns></returns>
            public static string RemoveSpacesFromString(this string stringtext)
            {
                stringtext = stringtext.Trim().Trim(",; ".ToCharArray());
                return stringtext;
            }

            public static bool CheckIsNullOrWhiteSpace(this string value)
            {

                // this is needed because .net 3.5 and older don't have 
                // string.IsNullOrWhiteSpace
                if (value == null) return true;

                for (var i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i])) return false;
                }

                return true;
            }

            public static bool IsNumeric(this object Expression)
            {
                double retNum;

                var isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                return isNum;
            }

            #region HtmlHelper

            //private static Regex RegExBadTags = new Regex(@"<(?:script|object|meta|embed|frameset|i?frame|style|link)[\s>]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            private static Regex RegExBadTags = new Regex(@"<(?:script|meta|frameset|i?frame|style|link|applet|body|bgsound|base|basefont|frame|frameset|head|html|id|i?layer|name|title|xml|javascript|form|input|select|textarea|noscript)[\s>]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            //private static Regex RegExAnyTags = new Regex("</?[^<>]+>", RegexOptions.Compiled); 
            private static Regex RegExAnyTags = new Regex("<[\\w/]+[^<>]*>", RegexOptions.Compiled);
            /// <summary>
            /// The reg ex any entity
            /// </summary>
            private static Regex RegExAnyEntity = new Regex("&[#a-zA-z0-9]+;", RegexOptions.Compiled);
            /// <summary>
            /// The reg ex find title
            /// </summary>
            private static Regex RegExFindTitle = new Regex(@"<head\s*>.*<title\s*>(?<title>[^<]+)</title>.*</head>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            /// <summary>
            /// Converts a relative url to an absolute one. baseUrl is used as the base to fix the other.
            /// </summary>
            /// <param name="url">Url to fix</param>
            /// <param name="baseUrl">base Url to be used</param>
            /// <returns></returns>
            public static string ConvertToAbsoluteUrl(this string url, string baseUrl)
            {
                return ConvertToAbsoluteUrl(url, baseUrl, false);
            }

            /// <summary>
            /// Converts a relative url to an absolute one. baseUrl is used as the base to fix the other.
            /// </summary>
            /// <param name="url">Url to fix</param>
            /// <param name="baseUrl">base Url to be used</param>
            /// <param name="onlyValid">Provide true, if the url should only be handled if no UriFormatException happens.</param>
            /// <returns></returns>
            public static string ConvertToAbsoluteUrl(this string url, string baseUrl, bool onlyValid)
            {

                // we try to prevent the exception caused in the case the url is relative
                // (no scheme info) just for speed
                if (url.IndexOf(Uri.SchemeDelimiter) < 0 && baseUrl != null)
                {
                    try
                    {
                        var baseUri = new Uri(baseUrl);
                        return (new Uri(baseUri, url).ToString());
                    }
                    catch
                    {
                        if (onlyValid)
                            return null;
                    }
                }

                try
                {
                    var uri = new Uri(url);
                    return uri.ToString();
                }
                catch (Exception)
                {

                    if (baseUrl != null)
                    {
                        try
                        {
                            var baseUri = new Uri(baseUrl);
                            return (new Uri(baseUri, url).ToString());
                        }
                        catch (Exception)
                        {
                            if (onlyValid)
                                return null;
                            return url;
                        }
                    }
                    else
                    {
                        if (onlyValid)
                            return null;
                        return url;
                    }
                }
            }




            /// <summary>
            /// Removes any tag of the form &lt;xxx> or &lt;/xxx>.
            /// </summary>
            /// <param name="html">string to work on</param>
            /// <returns>A Text-Only version of the provided input.</returns>
            public static string StripAnyTags(this string html)
            {
                if (html == null)
                    return String.Empty;
                return RegExAnyTags.Replace(html, String.Empty);
            }

            /// <summary>
            /// Replace any bad tag of the form &lt;object> by &lt;bject> to make
            /// it does not work on rendering later
            /// </summary>
            /// <param name="html">string to work on</param>
            /// <returns>A cleaner version of the provided input.</returns>
            public static string StripBadTags(this string html)
            {
                if (html == null)
                    return String.Empty;
                if (RegExBadTags.IsMatch(html))
                {
                    html = RegExBadTags.Replace(html, new MatchEvaluator(RegexTagEvaluate));
                }
                return html;
            }

            /// <summary>
            /// Regexes the tag evaluate.
            /// </summary>
            /// <param name="m">The m.</param>
            /// <returns></returns>
            private static string RegexTagEvaluate(Match m)
            {
                return string.Concat("<", m.ToString().Substring(2));
            }

            /// <summary>
            /// Removes a specific tag
            /// </summary>
            /// <param name="html">string to work on</param>
            /// <param name="html">tag to remove</param>
            /// <returns>A cleaner version of the provided input.</returns>
            public static string StripTag(this string html, string tag)
            {
                var open = string.Format("<{0} [\\w/]+[^<>]*>", tag);
                var close = string.Format(@"<((\s)*)?/((\s)*)?{0}((\s)*)?>", tag);
                //Regex RegExBadTag = new Regex(reqRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                var RegExBadTag = new Regex(open, RegexOptions.Compiled | RegexOptions.Compiled);
                var RegExCloseTag = new Regex(close, RegexOptions.IgnoreCase | RegexOptions.Compiled);


                if (html == null)
                    return String.Empty;

                html = RegExBadTag.Replace(html, String.Empty);
                return RegExCloseTag.Replace(html, String.Empty);

            }


            /// <summary>
            /// Decodes Basic allowed Tags
            /// </summary>
            /// <param name="html"></param>
            /// <returns></returns>
            public static string DecodeAllowedTags(this string html)
            {
                html = html.Replace("&amp;", "&");

                html = html.Replace("&lt;strong&gt;", "<strong>");
                html = html.Replace("&lt;/strong&gt;", "</strong>");
                html = html.Replace("&lt;em&gt;", "<em>");
                html = html.Replace("&lt;/em&gt;", "</em>");

                html = html.Replace("&lt;u&gt;", "<u>");
                html = html.Replace("&lt;/u&gt;", "</u>");

                html = html.Replace("&lt;ol&gt;", "<ol>");
                html = html.Replace("&lt;/ol&gt;", "</ol>");
                html = html.Replace("&lt;li&gt;", "<li>");
                html = html.Replace("&lt;/li&gt;", "</li>");
                html = html.Replace("&lt;ul&gt;", "<ul>");
                html = html.Replace("&lt;/ul&gt;", "</ul>");

                html = html.Replace("&lt;p&gt;", "<p>");
                html = html.Replace("&lt;/p&gt;", "</p>");


                html = html.Replace("&lt;blockquote&gt;", "<blockquote>");
                html = html.Replace("&lt;/blockquote&gt;", "</blockquote>");



                return html;
            }

            /// <summary>
            /// Create Links in HTML for text like http://www.google.com and www.google.com
            /// </summary>
            /// <param name="html"></param>
            /// <returns></returns>
            public static string CreateLinks(this string html)
            {
                Regex regex = null;
                regex = new Regex("(http(s)?://([\\w-]+\\.)[\\w-./?%&amp;=#]*)", RegexOptions.IgnoreCase);
                html = regex.Replace(html, "<a href=\"$1\" rel=\"nofollow\" target=\"_blank\">$1</a>");
                html = html.Replace(">www", "> www");
                regex = new Regex("([^http(s)?://]www.([\\w-]+\\.)[\\w-./?%&amp;=#]*)", RegexOptions.IgnoreCase);
                html = regex.Replace(html, "<a href=\"http://$1\" rel=\"nofollow\" target=\"_blank\">$1</a>");

                return html;
            }

            #endregion

            

            /// <summary>
            /// Make the first charter to Upper
            /// </summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public static string FirstCharToUpper(this string input)
            {
                if (string.IsNullOrEmpty(input))
                    return input;
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            }
        }

    }


