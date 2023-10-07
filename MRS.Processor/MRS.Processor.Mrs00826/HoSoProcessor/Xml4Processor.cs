using MRS.Processor.Mrs00826.HoSoProcessor;
using MRS.Processor.Mrs00826.Base;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    class Xml4Processor 
    {
        internal List<Xml4ADO> GenerateXml4ADO(InputADO data)
        {
            List<Xml4ADO> rs = null;
            try
            {
                string Xml4DataOption = "";

                if (data.ConfigData != null && data.ConfigData.Count > 0)
                {
                    Xml4DataOption = HisConfigKey.GetConfigData(data.ConfigData, HisConfigKey.XML__4210__XML4__DATA_OPTION);
                }

                List<Xml4ADO> listXml4Ado = new List<Xml4ADO>();
                int count = 1;
                if (data.SereServTeins == null) data.SereServTeins = new List<V_HIS_SERE_SERV_TEIN>();
                bool check = true;
                if (data.ListSereServ.Exists(e => e.MACHINE_ID.HasValue))
                {
                    check = true;
                }

                if (data.SereServTeins.Exists(e => e.MACHINE_ID.HasValue))
                {
                    check = true;
                }

                if (check)
                {
                    data.SereServTeins = data.SereServTeins.OrderBy(t => t.ID).ToList();
                    foreach (var ssTein in data.SereServTeins)
                    {
                        V_HIS_SERE_SERV_2 ss = data.ListSereServ.FirstOrDefault(o => o.ID == ssTein.SERE_SERV_ID);
                        if (ss == null)
                        {
                            continue;
                        }

                        if (!ss.TDL_HEIN_SERVICE_TYPE_ID.HasValue)
                        {
                            continue;
                        }

                        if (Xml4DataOption == "1" && String.IsNullOrWhiteSpace(ssTein.VALUE))
                        {
                            continue;
                        }
                        Xml4ADO xml4 = new Xml4ADO();
                        xml4.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                        xml4.Stt = count;
                        xml4.GiaTri = ssTein.VALUE ?? "";
                        xml4.KetLuan = this.SubMaxLength(ss.CONCLUDE ?? "");
                        xml4.MaChiSo = ssTein.BHYT_CODE ?? "";
                        xml4.MaDichVu = ss.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        if (ssTein.MACHINE_ID.HasValue)
                        {
                            xml4.MaMay = ssTein.MACHINE_GROUP_CODE + "." + ssTein.SOURCE_CODE;
                            if (data.Branch != null)
                            {
                                xml4.MaMay = xml4.MaMay + "." + data.Branch.HEIN_MEDI_ORG_CODE;
                            }
                            else
                            {
                                xml4.MaMay = xml4.MaMay + "." + GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                            }

                            string serial = "";
                            if (!String.IsNullOrEmpty(ssTein.SERIAL_NUMBER) && ssTein.SERIAL_NUMBER.Length >= 6)
                            {
                                serial = ssTein.SERIAL_NUMBER.Substring(0, 6);
                            }
                            else if (!String.IsNullOrEmpty(ssTein.SERIAL_NUMBER))
                            {
                                string format = new String('0', 6).ToString();
                                serial = new StringBuilder().Append(Convert.ToInt32(ssTein.SERIAL_NUMBER ?? "0").ToString(format)).ToString();
                            }

                            xml4.MaMay = xml4.MaMay + "." + serial;
                        }
                        else
                        {
                            xml4.MaMay = "";
                        }

                        xml4.MoTa = this.SubMaxLength(ssTein.RESULT_DESCRIPTION ?? "");
                        xml4.NgayKetQua = ss.FINISH_TIME.HasValue ? ss.FINISH_TIME.ToString().Substring(0, 12) : ss.INTRUCTION_TIME.ToString().Substring(0, 12);
                        xml4.TenChiSo = ssTein.BHYT_NAME ?? "";
                        if (data.TotalSericeData != null && data.TotalSericeData.Count > 0)
                        {
                            V_HIS_SERVICE service = data.TotalSericeData.FirstOrDefault(o => o.ID == ss.SERVICE_ID);
                            if (service != null && service.PARENT_ID > 0)
                            {
                                V_HIS_SERVICE parentService = data.TotalSericeData.FirstOrDefault(o => o.ID == service.PARENT_ID);
                                xml4.MaCha = parentService.SERVICE_CODE;
                            }
                        }
                        xml4.MaKhoa = ss.REQUEST_BHYT_CODE ?? "";
                        if (!String.IsNullOrWhiteSpace(xml4.MaKhoa))
                        {
                            var dataDepartment = GlobalConfigStore.ListDepartments.FirstOrDefault(p => p.DEPARTMENT_CODE == xml4.MaKhoa);
                            if (dataDepartment != null)
                            {
                                xml4.TenKhoa = dataDepartment.DEPARTMENT_NAME;
                            }
                            else
                            {
                                xml4.TenKhoa = "";
                            }
                        }
                        HIS_EMPLOYEE bacSiChiDinh = GetBacSi(ss.REQUEST_LOGINNAME);
                        if (bacSiChiDinh != null)
                        {
                            xml4.MaBacSiChiDinh = bacSiChiDinh.DIPLOMA;
                            xml4.TenBacSiChiDinh = bacSiChiDinh.TDL_USERNAME;
                        }
                        List<HIS_EMPLOYEE> lstBacSi = ProcessorGetTatCaBacSiLienQuan(ss, data.EkipUsers).Distinct().ToList();
                        if (lstBacSi != null && lstBacSi.Count > 0)
                        {
                            xml4.MaTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.DIPLOMA ?? ""));
                            xml4.TenTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.TDL_USERNAME ?? ""));
                        }
                        listXml4Ado.Add(xml4);
                        count++;
                    }

                    List<long> listHeinServiceTypeCLS = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN,
                        IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN
                    };

                    List<long> sereServHasTein = new List<long>();
                    sereServHasTein = data.SereServTeins.Select(s => s.SERE_SERV_ID).Distinct().ToList();

                    //lấy các dịch vụ là CDHA, TDCN, XN không có chỉ số
                    var hisSereServs = data.ListSereServ.Where(o => listHeinServiceTypeCLS.Contains(o.TDL_HEIN_SERVICE_TYPE_ID.Value) && !sereServHasTein.Contains(o.ID)).OrderBy(t => t.INTRUCTION_TIME).ToList();
                    if (hisSereServs == null) hisSereServs = new List<V_HIS_SERE_SERV_2>();
                    foreach (var hisSereServ in hisSereServs)
                    {
                        Xml4ADO xml4 = new Xml4ADO();
                        xml4.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                        xml4.Stt = count;
                        xml4.KetLuan = this.SubMaxLength(hisSereServ.CONCLUDE ?? "");
                        xml4.MaDichVu = hisSereServ.TDL_HEIN_SERVICE_BHYT_CODE ?? "";
                        if (hisSereServ.MACHINE_ID.HasValue)
                        {
                            xml4.MaMay = hisSereServ.MACHINE_GROUP_CODE + "." + hisSereServ.SOURCE_CODE;
                            if (data.Branch != null)
                            {
                                xml4.MaMay = xml4.MaMay + "." + data.Branch.HEIN_MEDI_ORG_CODE;
                            }
                            else
                            {
                                xml4.MaMay = xml4.MaMay + "." + GlobalConfigStore.Branch.HEIN_MEDI_ORG_CODE;
                            }

                            string serial = "";
                            if (!String.IsNullOrEmpty(hisSereServ.SERIAL_NUMBER) && hisSereServ.SERIAL_NUMBER.Length >= 6)
                            {
                                serial = hisSereServ.SERIAL_NUMBER.Substring(0, 6);
                            }
                            else if (!String.IsNullOrEmpty(hisSereServ.SERIAL_NUMBER))
                            {
                                string format = new String('0', 6).ToString();
                                serial = new StringBuilder().Append(Convert.ToInt32(hisSereServ.SERIAL_NUMBER ?? "0").ToString(format)).ToString();
                            }

                            xml4.MaMay = xml4.MaMay + "." + serial;
                        }
                        else
                        {
                            xml4.MaMay = "";
                        }
                        V_HIS_SERVICE service = null;
                        if (data.TotalSericeData != null && data.TotalSericeData.Count > 0)
                        {
                            service = data.TotalSericeData.FirstOrDefault(o => o.ID == hisSereServ.SERVICE_ID);
                            if (service != null && service.PARENT_ID > 0)
                            {
                                V_HIS_SERVICE parentService = data.TotalSericeData.FirstOrDefault(o => o.ID == service.PARENT_ID);
                                xml4.MaCha = parentService.SERVICE_CODE;
                            }
                        }

                        //gán rỗng để không mất thẻ
                        if (service != null)
                        {
                            xml4.MaChiSo = service.SUIM_INDEX_CODE ?? "";
                            xml4.TenChiSo = service.SUIM_INDEX_NAME ?? "";
                        }
                        else
                        {
                            xml4.MaChiSo = "";
                            xml4.TenChiSo = "";
                        }

                        xml4.MoTa = this.SubMaxLength(hisSereServ.DESCRIPTION ?? "");

                        string time_kq = hisSereServ.FINISH_TIME.HasValue ? hisSereServ.FINISH_TIME.ToString().Substring(0, 12) : hisSereServ.INTRUCTION_TIME.ToString().Substring(0, 12);
                        xml4.NgayKetQua = hisSereServ.END_TIME.HasValue ? hisSereServ.END_TIME.ToString().Substring(0, 12) : time_kq;

                        xml4.MaKhoa = hisSereServ.REQUEST_BHYT_CODE ?? "";
                        if (!String.IsNullOrWhiteSpace(xml4.MaKhoa))
                        {
                            var dataDepartment = GlobalConfigStore.ListDepartments.FirstOrDefault(p => p.DEPARTMENT_CODE == xml4.MaKhoa);
                            if (dataDepartment != null)
                            {
                                xml4.TenKhoa = dataDepartment.DEPARTMENT_NAME;
                            }
                            else
                            {
                                xml4.TenKhoa = "";
                            }
                        }
                        HIS_EMPLOYEE bacSiChiDinh = GetBacSi(hisSereServ.REQUEST_LOGINNAME);
                        if (bacSiChiDinh != null)
                        {
                            xml4.MaBacSiChiDinh = bacSiChiDinh.DIPLOMA;
                            xml4.TenBacSiChiDinh = bacSiChiDinh.TDL_USERNAME;
                        }
                        List<HIS_EMPLOYEE> lstBacSi = ProcessorGetTatCaBacSiLienQuan(hisSereServ, data.EkipUsers).Distinct().ToList();
                        if (lstBacSi != null && lstBacSi.Count > 0)
                        {
                            xml4.MaTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.DIPLOMA ?? ""));
                            xml4.TenTatCaBacSi = string.Join(";", lstBacSi.Select(o => o.TDL_USERNAME ?? ""));
                        }

                        listXml4Ado.Add(xml4);
                        count++;
                    }
                }

                rs = listXml4Ado;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }

        //internal void MapADOToXml(List<Xml4ADO> listAdo, ref List<XML4DetailData> datas)
        //{
        //    try
        //    {
        //        if (datas == null)
        //            datas = new List<XML4DetailData>();
        //        if (listAdo != null || listAdo.Count > 0)
        //        {
        //            foreach (var ado in listAdo)
        //            {
        //                XML4DetailData detail = new XML4DetailData();
        //                detail.GIA_TRI = this.ConvertStringToXmlDocument(ado.GiaTri);
        //                detail.KET_LUAN = this.ConvertStringToXmlDocument(ado.KetLuan);
        //                detail.MA_CHI_SO = ado.MaChiSo;
        //                detail.MA_DICH_VU = ado.MaDichVu;
        //                detail.MA_LK = ado.MaLienKet;
        //                detail.MA_MAY = ado.MaMay;
        //                detail.MO_TA = this.ConvertStringToXmlDocument(ado.MoTa);
        //                detail.NGAY_KQ = ado.NgayKetQua;
        //                detail.STT = ado.Stt;
        //                detail.TEN_CHI_SO = this.ConvertStringToXmlDocument(ado.TenChiSo);
        //                datas.Add(detail);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //    }
        //}

        private string SubMaxLength(string input)
        {
            string result = input;
            if (!String.IsNullOrEmpty(input) && input.Length > GlobalConfigStore.MAX_LENGTH)
            {
                result = input.Substring(0, GlobalConfigStore.MAX_LENGTH);
            }
            return result;
        }

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
