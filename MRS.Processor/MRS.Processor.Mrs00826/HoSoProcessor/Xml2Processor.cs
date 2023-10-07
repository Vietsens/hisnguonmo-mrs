using MRS.Processor.Mrs00826.HoSoProcessor;
using MRS.Processor.Mrs00826.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    class Xml2Processor 
    {
        internal List<Xml2ADO> GenerateXml2ADO(InputADO data)
        {
            List<Xml2ADO> rs = null;
            try
            {
                string TutorialFormat = "";
                string gtOption = "";

                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    TutorialFormat = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TutorialFormatCFG);
                    gtOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.GayTeOptionCFG);
                }

                List<Xml2ADO> ListXml2Ado = new List<Xml2ADO>();
                var listHeinServiceType = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM
                };

                var listLiveArea = new List<string>
                {
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K1,
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K2,
                    MOS.LibraryHein.Bhyt.HeinLiveArea.HeinLiveAreaCode.K3
                };

                var hisSereServs = data.ListSereServ.Where(o => listHeinServiceType.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value)).OrderBy(t => t.INTRUCTION_TIME).ToList();//lấy các dịch vụ là thuốc, vật tư và không phải hao phí
                int count = 1;
                foreach (var hisSereServ in hisSereServs)
                {
                    string maThuoc = "";
                    string maNhomThuoc = "";
                    string loaiDichVu = "TH";
                    if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM || !hisSereServ.MEDICINE_ID.HasValue)
                    {
                        loaiDichVu = "MAU";
                        maThuoc = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        //maNhomThuoc = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                    }
                    else
                    {
                        maThuoc = hisSereServ.ACTIVE_INGR_BHYT_CODE ?? "";
                        //maNhomThuoc = hisSereServ.TDL_HST_BHYT_CODE ?? "";
                    }

                    maNhomThuoc = hisSereServ.HST_BHYT_CODE;
                    if (hisSereServ.HST_BHYT_CODE_IN_TIME.HasValue && data.Treatment.IN_TIME < hisSereServ.HST_BHYT_CODE_IN_TIME.Value)
                    {
                        maNhomThuoc = hisSereServ.OLD_HST_BHYT_CODE ?? (hisSereServ.HST_BHYT_CODE ?? "");
                    }

                    var xml2 = new Xml2ADO();
                    xml2.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml2.Stt = count;
                    xml2.LoaiDichVu = loaiDichVu;
                    xml2.MaThuoc = maThuoc;
                    xml2.MaNhom = maNhomThuoc;
                    xml2.TenThuoc = hisSereServ.TDL_HEIN_SERVICE_BHYT_NAME ?? "";
                    xml2.DonViTinh = hisSereServ.SERVICE_UNIT_NAME ?? "";
                    xml2.HamLuong = hisSereServ.CONCENTRA ?? "";
                    xml2.DuongDung = hisSereServ.MEDICINE_USE_FORM_CODE ?? "";
                    if (String.IsNullOrWhiteSpace(xml2.DuongDung) && (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM || !hisSereServ.MEDICINE_ID.HasValue))
                    {
                        if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            xml2.DuongDung = "2.14";
                        }
                        else if (!hisSereServ.MEDICINE_ID.HasValue)
                        {
                            xml2.DuongDung = "9.99";
                        }
                    }

                    string lieudung = "Uống";

                    if (!String.IsNullOrWhiteSpace(hisSereServ.TUTORIAL))
                    {
                        if (hisSereServ.TUTORIAL != "\n")
                        {
                            lieudung = ProcessDataTutorial(hisSereServ.TUTORIAL, TutorialFormat);
                        }
                    }
                    else if (hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || hisSereServ.TDL_HEIN_SERVICE_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM || !hisSereServ.MEDICINE_ID.HasValue)
                    {
                        lieudung = "Truyền tĩnh mạch";
                    }
                    else if (!hisSereServ.MEDICINE_ID.HasValue)
                    {
                        lieudung = "Đường hô hấp, dạng khí lỏng hoặc nén";
                    }

                    if (lieudung.Length > 255)
                    {
                        lieudung = lieudung.Substring(0, 250) + " ...";
                    }

                    xml2.LieuDung = lieudung;
                    xml2.SoDangKy = hisSereServ.MEDICINE_REGISTER_NUMBER ?? "";

                    List<string> ttThau = new List<string>();
                    if (!string.IsNullOrEmpty(hisSereServ.MEDICINE_BID_NUMBER)) ttThau.Add(hisSereServ.MEDICINE_BID_NUMBER);

                    if (!string.IsNullOrEmpty(hisSereServ.MEDICINE_BID_PACKAGE_CODE)) ttThau.Add(hisSereServ.MEDICINE_BID_PACKAGE_CODE);

                    if (!string.IsNullOrEmpty(hisSereServ.MEDICINE_BID_GROUP_CODE)) ttThau.Add(hisSereServ.MEDICINE_BID_GROUP_CODE);

                    if (ttThau.Count > 0)
                    {
                        var thau = String.Join(";", ttThau);
                        if (thau.EndsWith(";"))
                        {
                            thau = thau.Substring(0, thau.Length - 1);
                        }
                        xml2.TTThau = thau;
                    }
                    else
                        xml2.TTThau = "";

                    xml2.SoLuong = Math.Round(hisSereServ.AMOUNT, 3, MidpointRounding.AwayFromZero);
                    xml2.DonGia = Math.Round(hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO), 3, MidpointRounding.AwayFromZero);

                    //đơn vị quy đổi #16449
                    if (hisSereServ.CONVERT_RATIO.HasValue && hisSereServ.USE_ORIGINAL_UNIT_FOR_PRES != 1)
                    {
                        xml2.DonViTinh = hisSereServ.CONVERT_UNIT_NAME ?? "";
                        xml2.SoLuong = Math.Round(hisSereServ.AMOUNT * hisSereServ.CONVERT_RATIO.Value, 2, MidpointRounding.AwayFromZero);
                        xml2.DonGia = Math.Round((hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO)) / hisSereServ.CONVERT_RATIO.Value, 3, MidpointRounding.AwayFromZero);
                    }

                    xml2.TyLeTT = Math.Round(hisSereServ.ORIGINAL_PRICE > 0 ? (hisSereServ.HEIN_LIMIT_PRICE.HasValue ? (hisSereServ.HEIN_LIMIT_PRICE.Value / (hisSereServ.ORIGINAL_PRICE * (1 + hisSereServ.VAT_RATIO))) * 100 : (hisSereServ.PRICE / hisSereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                    xml2.ThanhTien = Math.Round(xml2.SoLuong * xml2.DonGia, 2, MidpointRounding.AwayFromZero);
                    xml2.MucHuong = hisSereServ.HEIN_RATIO.HasValue ? (int)(hisSereServ.HEIN_RATIO.Value * 100) : 0;
                    xml2.TongNguonKhac = Math.Round(xml2.SoLuong * (hisSereServ.OTHER_SOURCE_PRICE ?? 0), 2, MidpointRounding.AwayFromZero);

                    xml2.TongBHTT = Math.Round(xml2.ThanhTien * (hisSereServ.HEIN_RATIO ?? 0) * (xml2.TyLeTT / 100), 2, MidpointRounding.AwayFromZero);
                    xml2.TongBNCCT = Math.Round(xml2.ThanhTien * (xml2.TyLeTT / 100), 2, MidpointRounding.AwayFromZero) - xml2.TongBHTT;

                    if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    {
                        //cong thuc tinh tien cv 1/6/2018
                        //cong thuc khong de cap den noi tru ngoai tru ma chi co trai tuyen
                        decimal TRAITUYEN = 0;
                        if (!String.IsNullOrWhiteSpace(data.HeinApproval.LIVE_AREA_CODE) && listLiveArea.Contains(data.HeinApproval.LIVE_AREA_CODE))
                        {
                            TRAITUYEN = 100;
                        }
                        else if (data.HeinApproval.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.PROVINCE)
                        {
                            if ((hisSereServ.HEIN_RATIO ?? 0) > 0.6m)
                                TRAITUYEN = 100;
                            else
                                TRAITUYEN = 60;
                        }
                        else if (data.HeinApproval.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.NATIONAL)
                        {
                            TRAITUYEN = 40;
                        }
                        if (TRAITUYEN != 0)
                        {
                            xml2.TongBNCCT = Math.Round(xml2.ThanhTien * (xml2.TyLeTT / 100) * TRAITUYEN / 100, 2, MidpointRounding.AwayFromZero) - xml2.TongBHTT;
                        }
                        //else
                        //{
                        //    xml2.TongBNCCT = 0;
                        //}
                    }

                    xml2.TongBNTT = xml2.ThanhTien - xml2.TongBHTT - xml2.TongBNCCT - xml2.TongNguonKhac;

                    //làm tròn TongBHTT và TongBNCCT dẫn đến âm
                    // dồn lại tiền TongBNCCT
                    if (xml2.TongBNTT < 0)
                    {
                        xml2.TongBNTT = 0;
                        xml2.TongBNCCT = xml2.ThanhTien - xml2.TongBHTT - xml2.TongNguonKhac;
                        if (xml2.TongBNCCT < 0)//nguồn khác làm tròn lên bị âm cả TongBNCCT
                        {
                            xml2.TongBNCCT = 0;
                            xml2.TongNguonKhac = xml2.ThanhTien - xml2.TongBHTT;
                        }
                    }

                    xml2.PhamVi = 1;
                    if (xml2.TongBHTT == 0)
                    {
                        xml2.PhamVi = 2;
                        xml2.TyLeTT = 0;
                    }

                    MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval = null;
                    if (data.HeinApprovals != null && data.HeinApprovals.Count > 0)
                    {
                        heinApproval = data.HeinApprovals.FirstOrDefault(o => o.ID == hisSereServ.HEIN_APPROVAL_ID);
                    }

                    if (heinApproval == null)
                    {
                        heinApproval = data.HeinApproval;
                    }
                    //Ngoai dinh suat
                    if (XmlProcessorBase.CheckBhytNsd(GlobalConfigStore.ListIcdCode_Nds, GlobalConfigStore.ListIcdCode_Nds_Te,
                    hisSereServ.ICD_CODE, heinApproval, hisSereServ.SERVICE_ID, data.TotalSericeData, data.TotalIcdData))
                    {
                        xml2.TongNgoaiDS = xml2.TongBHTT;
                    }
                    else if (XmlProcessorBase.CheckBhytNsd(GlobalConfigStore.ListIcdCode_Nds, GlobalConfigStore.ListIcdCode_Nds_Te, data.Treatment, heinApproval))
                    {
                        xml2.TongNgoaiDS = xml2.TongBHTT;
                    }
                    else
                    {
                        xml2.TongNgoaiDS = 0;
                    }

                    xml2.MaKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                    if (!String.IsNullOrWhiteSpace(xml2.MaKhoa))
                    {
                        var dataDepartment = GlobalConfigStore.ListDepartments.FirstOrDefault(p => p.DEPARTMENT_CODE == xml2.MaKhoa);
                        if (dataDepartment != null)
                        {
                            xml2.TenKhoa = dataDepartment.DEPARTMENT_NAME;
                        }
                        else
                        {
                            xml2.TenKhoa = "";
                        }
                    }
                    if (GlobalConfigStore.ListEmployees != null && GlobalConfigStore.ListEmployees.Count > 0)
                    {
                        var dataEmployee = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == hisSereServ.REQUEST_LOGINNAME);
                        if (dataEmployee != null)
                        {
                            xml2.MaBacSi = dataEmployee.DIPLOMA;
                        }
                        else
                        {
                            xml2.MaBacSi = "";
                        }
                    }
                    else
                    {
                        xml2.MaBacSi = "";//TO DO - phầm mềm chưa quản lý
                    }

                    HIS_EMPLOYEE bacSiChiDinh = GetBacSi(hisSereServ.REQUEST_LOGINNAME);
                    if (bacSiChiDinh != null)
                    {
                        xml2.MaBacSiChiDinh = bacSiChiDinh.DIPLOMA;
                        xml2.TenBacSiChiDinh = bacSiChiDinh.TDL_USERNAME;
                    }
                    List<HIS_EMPLOYEE> lstBacSi = ProcessorGetTatCaBacSiLienQuan(hisSereServ, data.EkipUsers).Distinct().ToList();
                    if (lstBacSi != null && lstBacSi.Count > 0)
                    {
                        xml2.MaTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.DIPLOMA ?? ""));
                        xml2.TenTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.TDL_USERNAME ?? ""));
                    }

                    List<string> mabenh = new List<string>();
                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_CODE))
                    {
                        mabenh.Add(hisSereServ.ICD_CODE);
                    }

                    if (!String.IsNullOrWhiteSpace(hisSereServ.ICD_SUB_CODE))
                    {
                        var benh = hisSereServ.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        mabenh.AddRange(benh);
                    }

                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_CODE))
                    {
                        mabenh.Add(data.Treatment.ICD_CODE);
                    }

                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_SUB_CODE))
                    {
                        var benh = data.Treatment.ICD_SUB_CODE.Trim(';').Split(';').ToList();
                        mabenh.AddRange(benh);
                    }

                    mabenh = mabenh.Distinct().ToList();
                    xml2.MaBenh = string.Join(";", mabenh);
                    xml2.NgayYLenh = hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                    if (xml2.TongNgoaiDS > 0)
                    {
                        xml2.MaPTTT = 2;
                    }
                    else
                    {
                        xml2.MaPTTT = 1;
                    }

                    if (!String.IsNullOrWhiteSpace(gtOption) && hisSereServ.PARENT_ID.HasValue && hisSereServ.MEDICINE_IS_ANAESTHESIA == 1 && data.SereServPttts != null && data.SereServPttts.Count > 0)
                    {
                        var ssParent = data.ListSereServ.FirstOrDefault(o => o.ID == hisSereServ.PARENT_ID);
                        if (ssParent != null)
                        {
                            var ssPttt = data.SereServPttts.FirstOrDefault(o => o.SERE_SERV_ID == ssParent.ID);
                            if (ssPttt != null && ssPttt.IS_ANAESTHESIA == 1)
                            {
                                //mã dịch vụ cha đã có _GT
                                if (gtOption == "2")
                                {
                                    xml2.MaThuoc += string.Format("_{0}", ssParent.TDL_HEIN_SERVICE_BHYT_CODE);
                                }
                                else
                                {
                                    xml2.MaThuoc += string.Format("_{0}_GT", ssParent.TDL_HEIN_SERVICE_BHYT_CODE);
                                }
                            }
                        }
                    }

                    if (data.TotalSericeData != null && data.TotalSericeData.Count > 0)
                    {
                        V_HIS_SERVICE service = data.TotalSericeData.FirstOrDefault(o => o.ID == hisSereServ.SERVICE_ID);
                        if (service!= null && service.PARENT_ID > 0)
                        {
                            V_HIS_SERVICE parentService = data.TotalSericeData.FirstOrDefault(o => o.ID == service.PARENT_ID);
                            xml2.MaCha = parentService.SERVICE_CODE;
                        }
                    }

                    ListXml2Ado.Add(xml2);
                    count++;
                }

                rs = ListXml2Ado;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                rs = null;
            }
            return rs;
        }

        /// <summary>
        /// "1 viên/lần * 2 lần/ngày (Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên)" lên xml là "1 viên/lần * 2 lần/ngày"
        /// "(Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên)" lên xml là "Ngày uống 3 viên chia 2 lần, sáng 01 viên, chiều 02 viên"
        /// </summary>
        /// <param name="tutorial"></param>
        /// <param name="configValue"></param>
        /// <returns></returns>
        private string ProcessDataTutorial(string tutorial, string configValue)
        {
            string result = tutorial;
            try
            {
                if (configValue == "3")
                {
                    List<string> DataInParentheses = new List<string>();

                    while (result.Contains("(") && result.Contains(")"))
                    {
                        List<int> indexStart = new List<int>();
                        List<int> indexEnd = new List<int>();

                        for (int i = 0; i < result.Length; i++)
                        {
                            if (result[i] == '(')
                            {
                                indexStart.Add(i);
                            }
                            else if (result[i] == ')')
                            {
                                indexEnd.Add(i);
                            }
                        }

                        if (indexStart.Count > 0 && indexEnd.Count > 0)
                        {
                            for (int i = indexStart.Count - 1; i >= 0; i--)
                            {
                                if (indexEnd.Count == 0)
                                {
                                    break;
                                }

                                int end = indexEnd.FirstOrDefault(o => o - indexStart[i] > 0);
                                if (end > 0)
                                {
                                    DataInParentheses.Add(result.Substring(indexStart[i], end - indexStart[i] + 1));
                                    indexEnd.Remove(end);
                                }
                            }
                        }

                        foreach (var item in DataInParentheses)
                        {
                            result = result.Replace(item, "");
                        }
                    }

                    if (String.IsNullOrWhiteSpace(result) && DataInParentheses.Count > 0)
                    {
                        result = DataInParentheses.OrderByDescending(o => o.Length).FirstOrDefault();
                        result = result.Substring(1, result.Length - 2);
                    }
                }
            }
            catch (Exception ex)
            {
                result = tutorial;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool CheckSamePrice(decimal p1, decimal p2)
        {
            bool result = false;
            p1 = Math.Round(p1, 0);
            p2 = Math.Round(p2, 0);
            if (p1 == p2 || (p1 - 1) == p2 || (p1 + 1) == p2) result = true;
            return result;
        }

        //internal void MapADOToXml(List<Xml2ADO> listAdo, ref List<XML2DetailData> datas)
        //{
        //    try
        //    {
        //        if (datas == null)
        //            datas = new List<XML2DetailData>();
        //        if (listAdo != null && listAdo.Count >= 0)
        //        {
        //            foreach (var ado in listAdo)
        //            {
        //                XML2DetailData detail = new XML2DetailData();
        //                detail.DON_GIA = ado.DonGia.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.DON_VI_TINH = ado.DonViTinh;
        //                detail.DUONG_DUNG = ado.DuongDung;
        //                detail.HAM_LUONG = this.ConvertStringToXmlDocument(ado.HamLuong);
        //                detail.LIEU_DUNG = this.ConvertStringToXmlDocument(ado.LieuDung);
        //                detail.MA_BAC_SI = ado.MaBacSi;
        //                detail.MA_BENH = ado.MaBenh;
        //                detail.MA_KHOA = ado.MaKhoa;
        //                detail.MA_LK = ado.MaLienKet;
        //                detail.MA_NHOM = ado.MaNhom;
        //                detail.MA_PTTT = ado.MaPTTT;
        //                detail.MA_THUOC = ado.MaThuoc;
        //                detail.MUC_HUONG = ado.MucHuong;
        //                detail.NGAY_YL = ado.NgayYLenh;
        //                detail.PHAM_VI = ado.PhamVi;
        //                detail.SO_DANG_KY = ado.SoDangKy;
        //                detail.SO_LUONG = ado.SoLuong.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.STT = ado.Stt;
        //                detail.TEN_THUOC = this.ConvertStringToXmlDocument(ado.TenThuoc);
        //                detail.T_BHTT = ado.TongBHTT.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.T_BNCCT = ado.TongBNCCT.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.T_BNTT = ado.TongBNTT.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.T_NGOAIDS = ado.TongNgoaiDS.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.T_NGUONKHAC = ado.TongNguonKhac.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.THANH_TIEN = ado.ThanhTien.ToString("G27", CultureInfo.InvariantCulture);
        //                detail.TT_THAU = ado.TTThau;
        //                detail.TYLE_TT = ado.TyLeTT.ToString("G27", CultureInfo.InvariantCulture);
        //                datas.Add(detail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        datas = null;
        //    }
        //}

        private List<HIS_EMPLOYEE> ProcessorGetTatCaBacSiLienQuan(V_HIS_SERE_SERV_2 hisSereServ, List<HIS_EKIP_USER> ekipUsers)
        {
            List<HIS_EMPLOYEE> lstMaBacSi = new List<HIS_EMPLOYEE>();
            try
            {
                HIS_EMPLOYEE executeName = GetBacSi(hisSereServ.EXECUTE_LOGINNAME);
                HIS_EMPLOYEE reqName = GetBacSi(hisSereServ.REQUEST_LOGINNAME);
                List<HIS_EMPLOYEE> resultName = new List<HIS_EMPLOYEE>();
                if (!String.IsNullOrWhiteSpace(hisSereServ.SUBCLINICAL_RESULT_LOGINNAME))
                {
                    string[] loginname = hisSereServ.SUBCLINICAL_RESULT_LOGINNAME.Split(';');
                    foreach (var item in loginname)
                    {
                        HIS_EMPLOYEE cchn = GetBacSi(item);
                        if (cchn != null)
                        {
                            resultName.Add(cchn);
                        }
                    }
                }

                if (reqName != null)
                {
                    lstMaBacSi.Add(reqName);
                }
                if (executeName != null)
                {
                    lstMaBacSi.Add(executeName);
                }

                if (resultName.Count > 0)
                {
                    lstMaBacSi.AddRange(resultName);
                }

                if (hisSereServ.EKIP_ID.HasValue && ekipUsers != null && ekipUsers.Count > 0)
                {
                    //có kíp add theo kíp
                    var dataEkip = ekipUsers.Where(o => o.EKIP_ID == hisSereServ.EKIP_ID.Value).Distinct().ToList();
                    if (dataEkip != null && dataEkip.Count > 0)
                    {
                        foreach (var item in dataEkip)
                        {
                            HIS_EMPLOYEE cchn = GetBacSi(item.LOGINNAME);
                            if (cchn != null)
                            {
                                lstMaBacSi.Add(cchn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return lstMaBacSi;
        }
        
        private HIS_EMPLOYEE GetBacSi(string loginName)
        {
            HIS_EMPLOYEE result = null;
            try
            {
                if (GlobalConfigStore.ListEmployees != null)
                {
                    result = GlobalConfigStore.ListEmployees.FirstOrDefault(p => p.LOGINNAME == loginName);
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("ListEmployees null");
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

    }
}
