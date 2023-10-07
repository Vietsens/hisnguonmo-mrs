using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MRS.Processor.Mrs00826.Base
{
    class XmlProcessorBase
    {
    //    internal XmlCDataSection ConvertStringToXmlDocument(string data)
    //    {
    //        XmlCDataSection result;
    //        XmlDocument doc = new XmlDocument();
    //        doc.LoadXml("<book genre='novel' ISBN='1-861001-57-5'>" + "<title>Pride And Prejudice</title>" + "</book>");
    //        result = doc.CreateCDataSection(RemoveXmlCharError(data));
    //        return result;
    //    }

    //    public ResultADO CreatedXmlFile<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
    //    {
    //        ResultADO rs = null;
    //        string xmlFile = null;
    //        try
    //        {
    //            var enc = Encoding.UTF8;
    //            using (var ms = new MemoryStream())
    //            {
    //                var xmlNamespaces = new XmlSerializerNamespaces();
    //                if (displayNamspacess)
    //                {
    //                    xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
    //                    xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
    //                }
    //                else
    //                    xmlNamespaces.Add("", "");

    //                var xmlWriterSettings = new XmlWriterSettings
    //                {
    //                    CloseOutput = false,
    //                    Encoding = enc,
    //                    OmitXmlDeclaration = false,
    //                    Indent = true
    //                };
    //                using (var xw = XmlWriter.Create(ms, xmlWriterSettings))
    //                {
    //                    var s = new XmlSerializer(typeof(T));
    //                    s.Serialize(xw, input, xmlNamespaces);
    //                }
    //                xmlFile = enc.GetString(ms.ToArray());
    //            }

    //            if (saveFile)//kiểm tra lưu file không
    //            {
    //                using (var file = new StreamWriter(path))
    //                {
    //                    file.Write(xmlFile);
    //                }
    //                rs = new ResultADO(true, "Luu Thanh Cong", new object[] { xmlFile });
    //            }

    //            if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
    //            {
    //                var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
    //                if (!string.IsNullOrEmpty(encodeXml))
    //                    rs = new ResultADO(true, "", new object[] { encodeXml });
    //                else
    //                    rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Inventec.Common.Logging.LogSystem.Error(ex);
    //            rs = new ResultADO(false, ex.Message, null);
    //        }
    //        return rs;
    //    }

    //    public ResultADO CreatedXmlFileEncoding<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
    //    {
    //        ResultADO rs = null;
    //        string xmlFile = null;
    //        try
    //        {
    //            var enc = new UTF8Encoding(false);

    //            using (var ms = new MemoryStream())
    //            {
    //                var xmlNamespaces = new XmlSerializerNamespaces();
    //                if (displayNamspacess)
    //                {
    //                    xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
    //                    xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
    //                }
    //                else
    //                    xmlNamespaces.Add("", "");

    //                var xmlWriterSettings = new XmlWriterSettings
    //                {
    //                    CloseOutput = false,
    //                    Encoding = enc,
    //                    OmitXmlDeclaration = false,
    //                    Indent = true
    //                };
    //                using (var xw = XmlWriter.Create(ms, xmlWriterSettings))
    //                {
    //                    var s = new XmlSerializer(typeof(T));
    //                    s.Serialize(xw, input, xmlNamespaces);
    //                }
    //                xmlFile = enc.GetString(ms.ToArray());
    //            }

    //            if (saveFile)//kiểm tra lưu file không
    //            {
    //                using (var file = new StreamWriter(path))
    //                {
    //                    file.Write(xmlFile);
    //                }
    //                rs = new ResultADO(true, "Luu Thanh Cong", new object[] { xmlFile });
    //            }

    //            if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
    //            {
    //                var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
    //                if (!string.IsNullOrEmpty(encodeXml))
    //                    rs = new ResultADO(true, "", new object[] { encodeXml });
    //                else
    //                    rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Inventec.Common.Logging.LogSystem.Error(ex);
    //            rs = new ResultADO(false, ex.Message, null);
    //        }
    //        return rs;
    //    }

        //public MemoryStream CreatedXmlFilePlus<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        //{
        //    MemoryStream stream = null;
        //    string xmlFile = null;
        //    try
        //    {
        //        var enc = Encoding.UTF8;
        //        stream = new MemoryStream();
        //        var xmlNamespaces = new XmlSerializerNamespaces();
        //        if (displayNamspacess)
        //        {
        //            xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        //            xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        //        }
        //        else
        //            xmlNamespaces.Add("", "");

        //        var xmlWriterSettings = new XmlWriterSettings
        //        {
        //            CloseOutput = false,
        //            Encoding = enc,
        //            OmitXmlDeclaration = false,
        //            Indent = true
        //        };
        //        using (var xw = XmlWriter.Create(stream, xmlWriterSettings))
        //        {
        //            var s = new XmlSerializer(typeof(T));
        //            s.Serialize(xw, input, xmlNamespaces);
        //        }
        //        xmlFile = enc.GetString(stream.ToArray());

        //        //if (encode)//kiểm tra nếu cần mã hóa file thì mã hóa sau đó trả lại cho ng dùng
        //        //{
        //        //    var encodeXml = EncodeBase64(Encoding.UTF8, xmlFile);
        //        //    if (!string.IsNullOrEmpty(encodeXml))
        //        //        rs = new ResultADO(true, "", new object[] { encodeXml });
        //        //    else
        //        //        rs = new ResultADO(false, "Ma hoa bang Base 64 that bai", null);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        stream = null;
        //    }
        //    return stream;
        //}

        //public MemoryStream CreatedXmlFileEncodingPlus<T>(T input, bool encode, bool displayNamspacess, bool saveFile, string path)
        //{
        //    MemoryStream stream = null;
        //    string xmlFile = null;
        //    try
        //    {
        //        var enc = new UTF8Encoding(false);

        //        stream = new MemoryStream();
        //        var xmlNamespaces = new XmlSerializerNamespaces();
        //        if (displayNamspacess)
        //        {
        //            xmlNamespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        //            xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        //        }
        //        else
        //            xmlNamespaces.Add("", "");

        //        var xmlWriterSettings = new XmlWriterSettings
        //        {
        //            CloseOutput = false,
        //            Encoding = enc,
        //            OmitXmlDeclaration = false,
        //            Indent = true
        //        };
        //        using (var xw = XmlWriter.Create(stream, xmlWriterSettings))
        //        {
        //            var s = new XmlSerializer(typeof(T));
        //            s.Serialize(xw, input, xmlNamespaces);
        //        }
        //        xmlFile = enc.GetString(stream.ToArray());
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        stream = null;
        //    }
        //    return stream;
        //}

        //internal string EncodeBase64(Encoding encoding, string text)//Mã hóa file XML sang Base64
        //{
        //    if (text == null)
        //        return null;
        //    byte[] textAsBytes = encoding.GetBytes(text);
        //    return Convert.ToBase64String(textAsBytes);
        //}

        internal static bool CheckBhytNsd(List<string> listIcdCode, List<string> listIcdCodeTe, string icdCode, V_HIS_HEIN_APPROVAL hisHeinApprovalBhyt, long serviceId, List<V_HIS_SERVICE> totalSericeData, List<HIS_ICD> totalIcdData)
        {
            var result = false;
            try
            {
                if (hisHeinApprovalBhyt != null && !String.IsNullOrWhiteSpace(hisHeinApprovalBhyt.HEIN_CARD_NUMBER) &&
                    (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                {
                    return true;
                }

                List<HIS_ICD> icdNds = null;
                if (totalIcdData != null && totalIcdData.Count > 0)
                {
                    icdNds = totalIcdData.Where(o => o.IS_HEIN_NDS == 1).ToList();
                }

                V_HIS_SERVICE service = new V_HIS_SERVICE();
                if (totalSericeData != null && totalSericeData.Count > 0)
                {
                    service = totalSericeData.FirstOrDefault(o => o.ID == serviceId);
                }

                if ((listIcdCode == null || listIcdCode.Count == 0) && (listIcdCodeTe == null || listIcdCodeTe.Count == 0) && (icdNds == null || icdNds.Count == 0))
                {
                    return result;
                }

                if (service != null && service.IS_OUT_OF_DRG == 1 && !string.IsNullOrEmpty(icdCode))
                {
                    if (listIcdCode == null || listIcdCode.Count == 0)
                    {
                        listIcdCode = new List<string>();
                    }

                    if (listIcdCodeTe == null || listIcdCodeTe.Count == 0)
                    {
                        listIcdCodeTe = new List<string>();
                    }

                    if (icdNds == null || icdNds.Count == 0)
                    {
                        icdNds = new List<HIS_ICD>();
                    }

                    if ((listIcdCode.Contains(icdCode) || icdNds.Exists(o => o.ICD_CODE == icdCode)))
                        result = true;
                    else if (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && (listIcdCodeTe.Contains(icdCode.Substring(0, 3)) || icdNds.Exists(o => o.ICD_CODE == icdCode)))
                        result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal static bool CheckBhytNsd(List<string> listIcdCode, List<string> listIcdCodeTe, V_HIS_TREATMENT_3 hisTreatment, V_HIS_HEIN_APPROVAL hisHeinApprovalBhyt)
        {
            var result = false;
            try
            {
                if (hisHeinApprovalBhyt != null && !String.IsNullOrWhiteSpace(hisHeinApprovalBhyt.HEIN_CARD_NUMBER) &&
                    (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CA") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("CY") ||
                    hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("QN")))
                {
                    return true;
                }

                if ((listIcdCode == null || listIcdCode.Count == 0) && (listIcdCodeTe == null || listIcdCodeTe.Count == 0))
                {
                    return result;
                }

                if (listIcdCode == null || listIcdCode.Count == 0)
                {
                    listIcdCode = new List<string>();
                }

                if (listIcdCodeTe == null || listIcdCodeTe.Count == 0)
                {
                    listIcdCodeTe = new List<string>();
                }

                if (listIcdCode.Contains(hisTreatment.ICD_CODE))
                    result = true;
                else if (!string.IsNullOrEmpty(hisTreatment.ICD_CODE))
                    if (hisHeinApprovalBhyt.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && listIcdCodeTe.Contains(hisTreatment.ICD_CODE.Substring(0, 3)))
                        result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal string RemoveXmlCharError(string data)
        {
            string result = "";
            try
            {
                StringBuilder s = new StringBuilder();
                if (!String.IsNullOrWhiteSpace(data))
                {
                    foreach (char c in data)
                    {
                        if (!System.Xml.XmlConvert.IsXmlChar(c)) continue;
                        s.Append(c);
                    }
                }

                result = s.ToString();
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        internal string SubString(string input, int maxLength)
        {
            string rs = input;
            try
            {
                if (!String.IsNullOrWhiteSpace(input) && Encoding.UTF8.GetByteCount(input) > maxLength)
                {
                    for (int i = input.Length - 1; i >= 0; i--)
                    {
                        if (Encoding.UTF8.GetByteCount(input.Substring(0, i + 1)) <= maxLength)
                        {
                            rs = String.Format("{0}", input.Substring(0, i + 1));
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = input;
            }
            return rs;
        }

        internal string ToString(decimal number)
        {
            return number.ToString("G27", CultureInfo.InvariantCulture);
        }

        internal string header_xml = "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"";
    }
}
