using MRS.Processor.Mrs00826.HoSoProcessor;
using MRS.Processor.Mrs00826.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    class Xml1Processor
    {
        internal Xml1ADO GenerateXml1Data(InputADO data, List<Xml2ADO> listXmlThuocAdo, List<Xml3ADO> listXmlDvktVt)
        {
            Xml1ADO result = null;
            try
            {
                string tenBenhOption = "";
                string IsTreatmentDayCount6556 = "";
                string transferOption = "";
                string addressOptionCFG = "";

                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    tenBenhOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TEN_BENH_OPTION);
                    IsTreatmentDayCount6556 = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.IS_TREATMENT_DAY_COUNT_6556);
                    transferOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.TransferOptionCFG);
                    addressOptionCFG = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.AddressOptionCFG);
                }
                else
                {
                    tenBenhOption = data.TenBenhOption;
                    IsTreatmentDayCount6556 = data.IsTreatmentDayCount6556;
                }


                Xml1ADO xml1 = new Xml1ADO();

                if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                {
                    string province = !String.IsNullOrWhiteSpace(data.HeinApproval.HEIN_MEDI_ORG_CODE) ? data.HeinApproval.HEIN_MEDI_ORG_CODE.Substring(0, 2) : "";
                    var mediOrg = GlobalConfigStore.HisHeinMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == data.HeinApproval.HEIN_MEDI_ORG_CODE);

                    if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        xml1.LyDoVaoVien = 2;
                    else if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.OVER)
                        xml1.LyDoVaoVien = 4;
                    else if (data.HeinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.PRESENT)
                        xml1.LyDoVaoVien = 1;
                    else if (!String.IsNullOrWhiteSpace(data.HeinApproval.HEIN_MEDI_ORG_CODE) &&
                        (data.HeinApproval.HEIN_MEDI_ORG_CODE == data.Branch.HEIN_MEDI_ORG_CODE
                        || (!String.IsNullOrWhiteSpace(data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && data.Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(data.HeinApproval.HEIN_MEDI_ORG_CODE))
                        ))
                        xml1.LyDoVaoVien = 1;
                    else if (data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || data.Branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE
                        )
                    {
                        xml1.LyDoVaoVien = 3;
                        if (province == data.Branch.HEIN_PROVINCE_CODE && mediOrg != null && (mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.DISTRICT || mediOrg.LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE))
                        {
                            xml1.LyDoVaoVien = 4;
                        }
                    }
                    else
                        xml1.LyDoVaoVien = 1;
                }
                else if (data.HeinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    xml1.LyDoVaoVien = 3;

                if (data.Treatment.ACCIDENT_HURT_TYPE_ID.HasValue && !String.IsNullOrEmpty(data.Treatment.ACCIDENT_HURT_TYPE_BHYT_CODE))
                {
                    xml1.MaTaiNan = data.Treatment.ACCIDENT_HURT_TYPE_BHYT_CODE;
                }
                else
                {
                    xml1.MaTaiNan = "0";
                }

                if (IsTreatmentDayCount6556 == "1")
                {
                    xml1.SoNgayDieuTri = (HIS.Common.Treatment.Calculation.DayOfTreatment6556(data.Treatment.IN_TIME, data.Treatment.CLINICAL_IN_TIME, data.Treatment.OUT_TIME, data.Treatment.TDL_TREATMENT_TYPE_ID ?? 0) ?? 0);
                }
                else
                {
                    if (data.Treatment.TREATMENT_DAY_COUNT.HasValue)
                    {
                        xml1.SoNgayDieuTri = Convert.ToInt64(data.Treatment.TREATMENT_DAY_COUNT.Value);
                    }
                    else
                    {
                        if (data.HeinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT ||
                            data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)//them so ngay dt voi bn ngoai tru
                        {
                            if (data.Treatment.CLINICAL_IN_TIME.HasValue && data.Treatment.OUT_TIME.HasValue)
                            {
                                xml1.SoNgayDieuTri = HIS.Common.Treatment.Calculation.DayOfTreatment(data.Treatment.CLINICAL_IN_TIME,
                                    data.Treatment.OUT_TIME, data.Treatment.TREATMENT_END_TYPE_ID,
                                    data.Treatment.TREATMENT_RESULT_ID, HIS.Common.Treatment.PatientTypeEnum.TYPE.BHYT);
                            }
                            else
                            {
                                xml1.SoNgayDieuTri = 0;
                            }
                        }
                        else
                        {
                            xml1.SoNgayDieuTri = 0;
                        }
                    }
                }

                if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                    xml1.KetQuaDieuTri = TreatmentResultBhytCFG.Khoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                    xml1.KetQuaDieuTri = TreatmentResultBhytCFG.Do;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                    xml1.KetQuaDieuTri = TreatmentResultBhytCFG.KhongThayDoi;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                    xml1.KetQuaDieuTri = TreatmentResultBhytCFG.NangHon;
                else if (data.Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                    xml1.KetQuaDieuTri = TreatmentResultBhytCFG.TuVong;

                if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    xml1.TinhTranRaVien = TreatmentEndTypeBhytCFG.ChuyenVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                    xml1.TinhTranRaVien = TreatmentEndTypeBhytCFG.TronVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                    xml1.TinhTranRaVien = TreatmentEndTypeBhytCFG.XinRaVien;
                else if (data.Treatment.TREATMENT_END_TYPE_ID.HasValue)
                    xml1.TinhTranRaVien = TreatmentEndTypeBhytCFG.RaVien;

                decimal tongChi = 0;
                decimal tongBNTT = 0;
                decimal tongBNCCT = 0;
                decimal tongBHTT = 0;
                decimal tongNguonKhac = 0;
                decimal tongNgoaiDs = 0;
                decimal tienThuoc = 0;
                if (listXmlThuocAdo != null && listXmlThuocAdo.Count > 0)
                {
                    foreach (var xml2 in listXmlThuocAdo)
                    {
                        tongChi += Math.Round(xml2.ThanhTien, 2);
                        tienThuoc += Math.Round(xml2.ThanhTien, 2);
                        tongBNTT += Math.Round(xml2.TongBNTT, 2);
                        tongBNCCT += Math.Round(xml2.TongBNCCT, 2);
                        tongBHTT += Math.Round(xml2.TongBHTT, 2);
                        tongNguonKhac += Math.Round(xml2.TongNguonKhac, 2);
                        tongNgoaiDs += Math.Round(xml2.TongNgoaiDS, 2);
                    }
                }

                decimal tientVTYT = 0;
                if (listXmlDvktVt != null && listXmlDvktVt.Count > 0)
                {
                    foreach (var xml3 in listXmlDvktVt)
                    {
                        tongChi += Math.Round(xml3.ThanhTien, 2);
                        if (xml3.IsMaterial)
                            tientVTYT += Math.Round(xml3.ThanhTien, 2);
                        tongBNTT += Math.Round(xml3.TongBNTT, 2);
                        tongBNCCT += Math.Round(xml3.TongBNCCT, 2);
                        tongBHTT += Math.Round(xml3.TongBHTT, 2);
                        tongNguonKhac += Math.Round(xml3.TongNguonKhac, 2);
                        tongNgoaiDs += Math.Round(xml3.TongNgoaiDS, 2);
                    }
                }

                xml1.TongThuoc = Math.Round(tienThuoc, 2);
                xml1.TongVTYT = Math.Round(tientVTYT, 2);
                xml1.TongChi = Math.Round(tongChi, 2);
                xml1.TongBNTT = tongBNTT;
                xml1.TongBHTT = tongBHTT;
                xml1.TongBNCCT = tongBNCCT;
                xml1.TongNguocKhac = tongNguonKhac;
                xml1.TongNgoaiDs = tongNgoaiDs;

                if (data.Treatment.FEE_LOCK_TIME.HasValue)
                {
                    xml1.NgayThanhToan = data.Treatment.FEE_LOCK_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NgayThanhToan = "";
                }

                if (data.HeinApproval.EXECUTE_TIME.HasValue)
                {
                    xml1.NamQT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(0, 4));
                    xml1.ThangQT = Convert.ToInt32(data.HeinApproval.EXECUTE_TIME.Value.ToString().Substring(4, 2));
                }
                else
                {
                    xml1.NamQT = DateTime.Now.Year;
                    xml1.ThangQT = DateTime.Now.Month;
                }

                if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    xml1.MaLoaiKCB = TreatmentTypeBhytCFG.Kham;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    xml1.MaLoaiKCB = TreatmentTypeBhytCFG.NgoaiTru;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                    xml1.MaLoaiKCB = TreatmentTypeBhytCFG.NoiTru;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__NHANTHUOC)
                    xml1.MaLoaiKCB = TreatmentTypeBhytCFG.NhanThuoc;
                else if (data.HeinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__TYTXA)
                    xml1.MaLoaiKCB = TreatmentTypeBhytCFG.Tyt;

                var listIcdCode = data.ListSereServ.Where(s => s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT).Select(s => s.ICD_CODE).Distinct().ToList();
                if (listIcdCode != null && listIcdCode.Count > 0)
                {
                    xml1.MaPTTTQT = string.Join(";", listIcdCode);
                }
                else
                {
                    xml1.MaPTTTQT = "";
                }

                //lấy cân nặng với trường hợp là trẻ em dưới 1 tuổi
                decimal? cannang = null;
                var tinhTuoi = Inventec.Common.DateTime.Calculation.DifferenceDate(data.Treatment.TDL_PATIENT_DOB, data.Treatment.IN_TIME);
                if (tinhTuoi <= 365 && data.Dhst != null)
                {
                    if (data.Dhst.WEIGHT.HasValue)
                    {
                        cannang = Math.Round(data.Dhst.WEIGHT.Value, 2);
                    }
                }

                xml1.CanNang = cannang;
                xml1.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";
                xml1.Stt = 1;
                xml1.MaBenhNhan = data.Treatment.TDL_PATIENT_CODE ?? "";
                xml1.TenBenhNhan = data.Treatment.TDL_PATIENT_NAME.ToLower();
                //if (data.HeinApproval.HAS_NOT_DAY_DOB == IMSys.DbConfig.HIS_RS.HIS_PATIENT.HAS_NOT_DAY_DOB__TRUE)
                //{
                //    xml1.NGAY_SINH = data.HeinApproval.DOB.ToString().Substring(0, 4);
                //}
                //else
                //{
                xml1.NgaySinh = data.Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                //}
                xml1.GioiTinh = data.Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE ? 2 : 1;
                if (addressOptionCFG == "1")
                {
                    xml1.DiaChi = data.Treatment.TDL_PATIENT_ADDRESS;
                }
                else
                {
                    xml1.DiaChi = data.HeinApproval.ADDRESS;
                }
                xml1.MaThe = data.HeinApproval.HEIN_CARD_NUMBER ?? "";
                xml1.MaDKBD = data.HeinApproval.HEIN_MEDI_ORG_CODE ?? "";
                xml1.HanTheTu = data.HeinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                xml1.HanTheDen = data.HeinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                if (data.HeinApprovals != null && data.HeinApprovals.Count > 1)
                {
                    //sap xep the theo thu tu han den cua the de tranh viec day nhieu lan bi tao ra nhieu dong tren cong bhyt.
                    List<V_HIS_HEIN_APPROVAL> heinApprovals = new List<V_HIS_HEIN_APPROVAL>();
                    heinApprovals = data.HeinApprovals.OrderBy(d => d.HEIN_CARD_TO_TIME).ToList();
                    //the dau tien lay theo the luu trong treatment                    

                    List<Base.BhytCardADO> listCard = new List<BhytCardADO>();
                    foreach (var item in heinApprovals)
                    {
                        Base.BhytCardADO ado = new BhytCardADO();
                        ado.HeinCard = item.HEIN_CARD_NUMBER;
                        ado.MediOrg = item.HEIN_MEDI_ORG_CODE;
                        ado.TimeFrom = item.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                        ado.TimeTo = item.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                        listCard.Add(ado);
                    }

                    listCard = listCard.Distinct().ToList();

                    var listHeinCard = listCard.Select(s => s.HeinCard).ToList();
                    var listMediOrg = listCard.Select(s => s.MediOrg).ToList();
                    var listTimeFrom = listCard.Select(s => s.TimeFrom).ToList();
                    var listTimeTo = listCard.Select(s => s.TimeTo).ToList();

                    if (listHeinCard != null && listHeinCard.Count > 0)
                    {
                        xml1.MaThe = String.Join(";", listHeinCard);
                    }

                    if (listMediOrg != null && listMediOrg.Count > 0)
                    {
                        xml1.MaDKBD = String.Join(";", listMediOrg);
                    }

                    if (listTimeFrom != null && listTimeFrom.Count > 0)
                    {
                        xml1.HanTheTu = String.Join(";", listTimeFrom);
                    }

                    if (listTimeTo != null && listTimeTo.Count > 0)
                    {
                        xml1.HanTheDen = String.Join(";", listTimeTo);
                    }
                }

                xml1.MienCungCT = "";
                if (data.HeinApproval.FREE_CO_PAID_TIME.HasValue)
                {
                    xml1.MienCungCT = data.HeinApproval.FREE_CO_PAID_TIME.Value.ToString().Substring(0, 8);
                }

                string tenBenh = data.Treatment.ICD_NAME ?? "";
                if (tenBenhOption == "2")
                {
                    List<string> lstTenBenh = new List<string>();
                    if (!String.IsNullOrWhiteSpace(data.Treatment.ICD_NAME))
                    {
                        lstTenBenh.Add(data.Treatment.ICD_NAME);
                    }

                    if (!string.IsNullOrEmpty(data.Treatment.ICD_TEXT))
                    {
                        var icdname = data.Treatment.ICD_TEXT.Split(';');
                        icdname = icdname.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
                        if (icdname != null && icdname.Count() > 0)
                        {
                            lstTenBenh.AddRange(icdname);
                        }
                    }

                    var reqIcd = data.ListSereServ.GroupBy(g => g.SERVICE_REQ_ID ?? 0).ToList();
                    foreach (var item in reqIcd)
                    {
                        if (!String.IsNullOrWhiteSpace(item.First().ICD_NAME))
                        {
                            lstTenBenh.Add(item.First().ICD_NAME);
                        }

                        if (!String.IsNullOrWhiteSpace(item.First().ICD_TEXT))
                        {
                            var icdname = item.First().ICD_TEXT.Split(';');
                            icdname = icdname.Where(o => !String.IsNullOrWhiteSpace(o)).ToArray();
                            if (icdname != null && icdname.Count() > 0)
                            {
                                lstTenBenh.AddRange(icdname);
                            }
                        }
                    }

                    if (lstTenBenh != null && lstTenBenh.Count > 0)
                    {
                        tenBenh = string.Join(";", lstTenBenh.Distinct().ToList());
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.Treatment.ICD_TEXT))
                    {
                        if (tenBenh == "")
                            tenBenh += data.Treatment.ICD_TEXT.Trim(';');
                        else
                            tenBenh += ";" + data.Treatment.ICD_TEXT.Trim(';');
                    }
                }

                xml1.TenBenh = tenBenh;
                xml1.MaBenh = data.Treatment.ICD_CODE ?? "";
                string mabenhKhac = "";
                if (!String.IsNullOrEmpty(data.Treatment.ICD_SUB_CODE))
                {
                    mabenhKhac = data.Treatment.ICD_SUB_CODE.Trim(';');
                }

                xml1.MaBenhKhac = mabenhKhac;
                xml1.MaNoiChuyen = data.Treatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";

                xml1.NgayVao = data.Treatment.IN_TIME.ToString().Substring(0, 12);
                //}

                if (data.Treatment.OUT_TIME.HasValue)
                {
                    xml1.NgayRa = data.Treatment.OUT_TIME.Value.ToString().Substring(0, 12);
                }
                else
                {
                    xml1.NgayRa = "";
                }

                if (!string.IsNullOrWhiteSpace(data.Treatment.END_ROOM_BHYT_CODE))
                {
                    xml1.MaKhoa = data.Treatment.END_ROOM_BHYT_CODE;
                }
                else if (!string.IsNullOrWhiteSpace(data.Treatment.EXIT_BHYT_CODE))
                {
                    xml1.MaKhoa = data.Treatment.EXIT_BHYT_CODE;
                }
                else
                {
                    xml1.MaKhoa = data.Treatment.END_BHYT_CODE ?? "";
                }
                if (!String.IsNullOrWhiteSpace(xml1.MaKhoa))
                {
                    var dataDepartment = GlobalConfigStore.ListDepartments.FirstOrDefault(p => p.DEPARTMENT_CODE == xml1.MaKhoa);
                    if (dataDepartment != null)
                    {
                        xml1.TenKhoa = dataDepartment.DEPARTMENT_NAME;
                    }
                    else
                    {
                        xml1.TenKhoa = "";
                    }
                }

                string maKcbdb = "";
                if (data.Branch != null && !String.IsNullOrEmpty(data.Branch.HEIN_MEDI_ORG_CODE))
                {
                    maKcbdb = data.Branch.HEIN_MEDI_ORG_CODE;
                }
                else if (GlobalConfigStore.Branch != null && !String.IsNullOrEmpty(GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE))
                {
                    maKcbdb = GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                }

                xml1.MaCSKCB = maKcbdb;
                xml1.MaKhuVuc = data.HeinApproval.LIVE_AREA_CODE ?? "";

                if (transferOption == "1")
                {
                    xml1.MaNoiChuyen = data.Treatment.TRANSFER_IN_MEDI_ORG_CODE ?? "";
                }

                result = xml1;

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
