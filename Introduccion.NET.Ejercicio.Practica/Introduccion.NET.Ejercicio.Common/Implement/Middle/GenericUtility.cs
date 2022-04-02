using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public static class GenericUtility
    {
        private static readonly string pdfMimeType = "application/pdf";
        private static readonly string contentTypeJson = "application/json";

        #region Comparar listas
        public static bool AnyMatch(List<string> list1, List<string> list2)
        {
            var container = list1.Count >= list2.Count ? 1 : 2;
            var compare = container == 1 ? list1.Where(x => list2.Contains(x)).ToList() : list2.Where(x => list1.Contains(x)).ToList();

            if (!compare.IsEmpty())
                return true;
            else
                return false;
        }

        #endregion

        #region A Dictionary

        public static IDictionary<string, object> ToDictionary(this object source, string rootName = null)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var property in source.GetType().GetProperties())
            {
                string key = (string.IsNullOrWhiteSpace(rootName)) ? property.Name : $"{rootName}.{property.Name}";
                object value = property.GetValue(source);
                result.Add(key, value);
            }
            return result;
        }

        public static IDictionary<string, Int32> ConvertEnumToDictionary<K>()
        {
            if (typeof(K).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(K)).Cast<Int32>().ToDictionary(currentItem => Enum.GetName(typeof(K), currentItem));
        }
        public static IDictionary<string, int> ConvertEnumToDictionaryDescription<K>()
        {
            if (typeof(K).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            Dictionary<string, int> _dic = new Dictionary<string, int>();

            foreach (var item in Enum.GetNames(typeof(K)))
            {
                var types = Enum.Parse(typeof(K), item.ToString());
                _dic.Add(((K)types).DescriptionAttr(), (int)types);
            }

            return _dic;
        }

        public static IActionResult ToResponse<T>(this ResponseBase<T> data)
        {
            try
            {
                HttpResponseMessage res = new HttpResponseMessage();
                foreach (PropertyInfo prop in data.GetType().GetProperties())
                {
                    if (prop.Name.ToUpper() != "DATA" && prop.Name.ToUpper() != "CODE")
                    {
                        var value = prop.GetValue(data, null);
                        if (!value.IsEmpty())
                            res.Headers.Add(prop.Name, value.ToString());
                    }
                }
                res.StatusCode = (HttpStatusCode)data.Code;
                var serializerSetting = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    FloatParseHandling = FloatParseHandling.Decimal,
                };
                string content = JsonConvert.SerializeObject(data.Data, Formatting.None, serializerSetting);
                res.Content = new StringContent(content, System.Text.Encoding.UTF8, contentTypeJson);
                return new HttpResponseMessageResult(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public static IDictionary<string, string> ConvertEnumToDictionaryString<K>()
        {
            if (typeof(K).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            Dictionary<string, string> _dic = new Dictionary<string, string>();

            foreach (var item in Enum.GetNames(typeof(K)))
            {
                var types = Enum.Parse(typeof(K), item);
                _dic.Add(((K)types).DescriptionAttr(), item);
            }

            return _dic;
        }
        #endregion

        #region Converts

        public static HttpRequestHeaders ConvertToHttpHeaders(this Dictionary<string, string> queryParameters, HttpRequestMessage message)
        {
            queryParameters.ToList().ForEach(a => message.Headers.Add(a.Key, a.Value));
            return message.Headers;
        }

        public static string ConvertToQueryString(this Dictionary<string, string> queryParameters)
        {
            return string.Join("&", queryParameters.
                Select(a => $"{ System.Web.HttpUtility.UrlEncode(a.Key)}={ System.Web.HttpUtility.UrlEncode(a.Value)}"));
        }

        public static string ConvertToQueryString(this Dictionary<string, string> queryParameters, string url)
        {
            foreach (KeyValuePair<string, string> item in queryParameters)
            {
                url = url.Replace("{" + item.Key + "}", item.Value);
            }
            return url.ToString();
        }

        #endregion

        #region Get Description

        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static T GetValueFromDescription<T>(string description, bool isUpper = false) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (ValidateDescription(isUpper, description, attribute.Description))
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (ValidateDescription(isUpper, description, null, field.Name))
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }

        internal static bool ValidateDescription(bool isUpper, string description, string attributeDescription = null, string fieldName = null)
        {
            if (attributeDescription != null)
            {
                if (((!isUpper) && (attributeDescription == description)) || ((isUpper) && (attributeDescription.ToUpper() == description.ToUpper())))
                    return true;
            }
            else
            {
                if (((!isUpper) && (fieldName == description)) || ((isUpper) && (fieldName.ToUpper() == description.ToUpper())))
                    return true;
            }

            return false;
        }

        #endregion

        #region Comunes

        public static IEnumerable<double> DoubleRange(double min, double max, double step)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                double value = min + step * i;
                if (value > max)
                {
                    break;
                }
                yield return value;
            }
        }

        public static bool IsEmpty<T>(this List<T> data)
        {
            return data == null || data.Count == 0;
        }
        public static bool IsEmpty(this string me)
        {
            return string.IsNullOrWhiteSpace(me);
        }

        public static bool IsEmpty(this object me)
        {
            return me == null;
        }
        public static string ToTitleCase(this string s)
        {
            // make the first letter of each word uppercase
            var titlecase = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
            // match any letter after an apostrophe and make uppercase
            titlecase = Regex.Replace(titlecase, "[^A-Za-z0-9 ](?:.)", m => m.Value.ToUpper());
            // look for 'S at the end of a word and make lower
            titlecase = Regex.Replace(titlecase, @"('S)\b", m => m.Value.ToLower());
            return titlecase;
        }

        public static DateTime ToDateTime(this string data)
        {
            if (!string.IsNullOrWhiteSpace(data) && DateTime.TryParse(data, out _))
                return Convert.ToDateTime(data);

            return DateTime.MinValue;
        }
        public static decimal ToDecimal(this string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var data1 = decimal.TryParse(data.Replace(",", "."), out _);
                var data2 = decimal.TryParse(data.Replace(".", ","), out _);

                if (data1 || data2)
                {
                    return Convert.ToDecimal(data.Replace(".", ","));
                }
                return -1;
            }

            return -1;
        }
        public static decimal? ToDecimalNull(this string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                var data1 = decimal.TryParse(data.Replace(",", "."), out _);
                var data2 = decimal.TryParse(data.Replace(".", ","), out _);

                if (data1 || data2)
                {
                    return Convert.ToDecimal(data.Replace(".", ","));
                }
                return null;
            }

            return null;
        }
        public static Int32 ToInt(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return 0;
            }
            else
            {
                var data1 = int.TryParse(data.Replace(",", "."), out _);
                var data2 = int.TryParse(data.Replace(".", ","), out _);

                if (data1 || data2)
                {
                    return Convert.ToInt32(data);
                }
            }

            return -1;
        }
        public static Int32? ToIntNull(this string data)
        {
            int value;
            bool isValid = int.TryParse(data, out value);
            if (isValid)
            {
                return value;
            }
            return null;
        }

        public static bool IsValidFormat(this string data, string regularExpression)
        {
            return Regex.IsMatch(data, regularExpression);
        }

        public static string GetSerializedEntity<T>(this T entity)
        {
            try
            {
                return JsonConvert.SerializeObject(entity);
            }
            catch (Exception) { return null; }
        }

        #endregion

        #region Response

        public static ResponseBase<T> ResponseBaseCatch<T>(bool validation, string message, HttpStatusCode status, TelemetryClient log)
        {
            ResponseBase<T> retval = new ResponseBase<T>();
            if (validation)
            {
                retval.Code = (int)status;
                retval.Message = message;
                if (log != null)
                    log.TrackAuditException(new Exception(message));
            }
            return retval;
        }

        public static void ResponseBaseCatch<T>(this ResponseBase<T> data, bool validation, string message, HttpStatusCode status, TelemetryClient log)
        {
            ResponseBase<T> retval = data;
            if (validation)
            {
                retval.Code = (int)status;
                retval.Message = message;
                if (log != null)
                    log.TrackAuditException(new Exception(message));
            }
        }

        public static void ResponseBaseCatch<T>(this ResponseBase<T> data, bool validation, Exception ex, HttpStatusCode status, TelemetryClient log)
        {
            ResponseBase<T> retval = data;
            if (validation)
            {
                retval.Code = (int)status;
                if (retval.Code == 500)
                    retval.Message = "Se ha generado un error procesando la solicitud";
                else
                    retval.Message = ex.Message;
            }
            if (log != null)
                log.TrackAuditException(ex);
        }

        public static ResponseBase<T> ResponseBaseCatch<T>(bool validation, Exception ex, HttpStatusCode status, TelemetryClient log)
        {
            ResponseBase<T> retval = new ResponseBase<T>();
            if (validation)
            {
                retval.Code = (int)status;
                if (retval.Code == 500)
                    retval.Message = "Se ha generado un error procesando la solicitud";
                else
                    retval.Message = ex.Message;
            }
            if (log != null)
                log.TrackAuditException(ex);
            return retval;
        }

        public static void CloseRequest<T>(ResponseBase<T> response, T data, string message, int count, int httpStatusCode)
        {
            response.Data = data;
            response.Message = message;
            response.Count = count;
            response.Code = httpStatusCode;
            response.StopTimer();
        }


        #endregion

        #region Otras

        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (endDate.Year - startDate.Year) + endDate.Month - startDate.Month;
            return Math.Abs(monthsApart);
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static DateTime GetProcessDate()
        {
            return new DateTime(DateTime.UtcNow.AddHours(-5).Year, DateTime.UtcNow.AddHours(-5).Month, 1).AddMonths(1);
        }

        public static bool IsValidJson<T>(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    _ = JsonConvert.DeserializeObject<T>(strInput);
                    return true;
                }
                catch // not valid
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static T BoolValidate<T>(bool validate, T valueIf, T valueElse)
        {
            return validate ? valueIf : valueElse;
        }

        #endregion



        public static bool IsPdf(this object AttachmentBase64)
        {
            byte[] data = Convert.FromBase64String(AttachmentBase64.ToString());
            var mimeType = GetMimeFromBytes(data);
            if (mimeType != pdfMimeType) return false;

            return true;
        }

        /// <summary>
        /// Get mime type from data
        /// </summary>
        /// <param name="pBC"></param>
        /// <param name="pwzUrl"></param>
        /// <param name="pBuffer"></param>
        /// <param name="cbSize"></param>
        /// <param name="pwzMimeProposed"></param>
        /// <param name="dwMimeFlags"></param>
        /// <param name="ppwzMimeOut"></param>
        /// <param name="dwReserved"></param>
        /// <returns></returns>
        [DllImport("urlmon.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
        static extern int FindMimeFromData(IntPtr pBC,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.I1, SizeParamIndex=3)]
            byte[] pBuffer,
            int cbSize,
            [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
            int dwMimeFlags,
            out IntPtr ppwzMimeOut,
            int dwReserved);

        /// <summary>
        /// Get bytes mime type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMimeFromBytes(byte[] data)
        {
            try
            {
                string mime = string.Empty;
                int ret = FindMimeFromData(IntPtr.Zero, null, data, data.Length, null, 0, out IntPtr outPtr, 0);
                if (ret == 0 && outPtr != IntPtr.Zero)
                {
                    return Marshal.PtrToStringUni(outPtr);
                }
                return mime;
            }
            catch
            {
                return "";
            }
        }

        public static string ToBase64(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            byte[] bytes = Encoding.Default.GetBytes(json);

            return Convert.ToBase64String(bytes);
        }


        public static T FromBase64<T>(this string base64Text)
        {
            byte[] bytes = Convert.FromBase64String(base64Text);

            string json = Encoding.Default.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(json);
        }


        public static string FromBase64(this string base64Text)
        {
            byte[] bytes = Convert.FromBase64String(base64Text);

            string text = Encoding.Default.GetString(bytes);

            return text;
        }

    }
}
