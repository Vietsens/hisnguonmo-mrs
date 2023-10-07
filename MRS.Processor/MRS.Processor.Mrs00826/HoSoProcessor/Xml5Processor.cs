using MRS.Processor.Mrs00826.HoSoProcessor;
using MRS.Processor.Mrs00826.Base;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.HoSoProcessor
{
    class Xml5Processor 
    {
        internal List<Xml5ADO> GenerateXml5ADO(InputADO data)
        {
            List<Xml5ADO> rs = null;
            try
            {
                List<Xml5ADO> listXml5Ado = new List<Xml5ADO>();
                V_HIS_SERE_SERV_2 sere_serv_main_exam = data.ListSereServ.FirstOrDefault(o => o.TDL_IS_MAIN_EXAM == 1);
                if (sere_serv_main_exam == null)
                {
                    sere_serv_main_exam = data.ListSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).OrderBy(o => o.TDL_INTRUCTION_TIME).FirstOrDefault();
                }

                HIS_DHST dataDhst = null;

                if (sere_serv_main_exam != null && sere_serv_main_exam.DHST_ID.HasValue && data.ListDhsts != null && data.ListDhsts.Count > 0)
                {
                    dataDhst = data.ListDhsts.FirstOrDefault(o => o.ID == sere_serv_main_exam.DHST_ID);
                }

                int count = 1;
                if (data.Trackings != null && data.Trackings.Count > 0)
                {
                    List<HIS_TRACKING> hisTrackings = data.Trackings.Where(o => !String.IsNullOrEmpty(o.CONTENT)).OrderBy(t => t.TRACKING_TIME).ToList();
                    foreach (HIS_TRACKING tracking in hisTrackings)
                    {
                        Xml5ADO xml5 = new Xml5ADO();
                        xml5.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                        xml5.Stt = count;
                        xml5.DienBien = this.SubMaxLength(tracking.CONTENT ?? ProcessDataExam(sere_serv_main_exam, dataDhst));
                        List<V_HIS_SERE_SERV_2> sereServs = data.ListSereServ.Where(o => o.TRACKING_ID == tracking.ID && (o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)).ToList();
                        string pttt = "";
                        if (sereServs != null && sereServs.Count > 0)
                        {
                            if (data.SereServPttts != null) data.SereServPttts = data.SereServPttts.OrderByDescending(o => o.ID).ToList();

                            foreach (V_HIS_SERE_SERV_2 ss in sereServs)
                            {
                                V_HIS_SERE_SERV_PTTT ssPttt = data.SereServPttts != null ? data.SereServPttts.FirstOrDefault(o => o.SERE_SERV_ID == ss.ID) : null;
                                if (ssPttt == null)// || String.IsNullOrEmpty(ssPttt.PTTT_METHOD_NAME) || pttt.Contains(ssPttt.PTTT_METHOD_NAME))
                                {
                                    continue;
                                }

                                //if (pttt == "")
                                //{
                                //    pttt = ssPttt.PTTT_METHOD_NAME;
                                //}
                                //else
                                //{
                                //    pttt = pttt + ";" + ssPttt.PTTT_METHOD_NAME;
                                //}

                                //Sửa lại để lấy theo trường mô tả trong màn hình xử lý phẫu thuật thủ thuật
                                if (pttt == "")
                                {
                                    pttt = ss.DESCRIPTION;
                                }
                                else
                                {
                                    pttt = pttt + ";" + ss.DESCRIPTION;
                                }
                            }
                        }

                        xml5.PhauThuat = this.SubMaxLength(pttt);

                        if (data.ListDebate != null && data.ListDebate.Count > 0)
                        {
                            var debates = data.ListDebate.Where(o => o.TRACKING_ID == tracking.ID).ToList();
                            if (debates != null && debates.Count > 0)
                            {
                                xml5.HoiChan = this.SubMaxLength(string.Join("; ", debates.Select(s => s.CONCLUSION).Distinct()));
                            }
                        }

                        if (String.IsNullOrWhiteSpace(xml5.HoiChan))
                        {
                            xml5.HoiChan = ".";
                        }

                        xml5.NgayYLenh = tracking.TRACKING_TIME.ToString().Substring(0, 12);
                        listXml5Ado.Add(xml5);
                        count++;
                    }
                }
                else if (sere_serv_main_exam != null)
                {
                    Xml5ADO xml5 = new Xml5ADO();
                    xml5.MaLienKet = data.Treatment.TREATMENT_CODE ?? "";//lấy mã BHYT làm mã liên kết trong toàn bộ file XML
                    xml5.Stt = 1;
                    xml5.DienBien = this.SubMaxLength(ProcessDataExam(sere_serv_main_exam, dataDhst));
                    xml5.PhauThuat = "";
                    xml5.HoiChan = ".";
                    xml5.NgayYLenh = sere_serv_main_exam.TDL_INTRUCTION_TIME.ToString().Substring(0, 12);
                    listXml5Ado.Add(xml5);
                }

                rs = listXml5Ado;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return rs;
        }

        private string ProcessDataExam(V_HIS_SERE_SERV_2 examData, HIS_DHST dhst)
        {
            string result = "";
            try
            {
                List<string> dienBien = new List<string>();
                if (examData != null)
                {
                    //A1
                    if (!String.IsNullOrWhiteSpace(examData.HOSPITALIZATION_REASON))
                    {
                        dienBien.Add("Lý do khám: " + examData.HOSPITALIZATION_REASON);
                    }

                    //A2
                    if (!String.IsNullOrWhiteSpace(examData.PATHOLOGICAL_PROCESS))
                    {
                        dienBien.Add("Quá trình bệnh lý: " + examData.PATHOLOGICAL_PROCESS);
                    }

                    //A3
                    if (!String.IsNullOrWhiteSpace(examData.PATHOLOGICAL_HISTORY))
                    {
                        dienBien.Add("Tiền sử bệnh nhân: " + examData.PATHOLOGICAL_HISTORY);
                    }

                    //A4
                    if (!String.IsNullOrWhiteSpace(examData.PATHOLOGICAL_HISTORY_FAMILY))
                    {
                        dienBien.Add("Tiền sử của gia đình: " + examData.PATHOLOGICAL_HISTORY_FAMILY);
                    }

                    //A5
                    if (!String.IsNullOrWhiteSpace(examData.FULL_EXAM))
                    {
                        dienBien.Add("Khám toàn thân: " + examData.FULL_EXAM);
                    }

                    //A6
                    if (!String.IsNullOrWhiteSpace(examData.PART_EXAM))
                    {
                        dienBien.Add("Khám bộ phận: " + examData.PART_EXAM);
                    }

                    if (dhst != null)
                    {
                        List<string> dataDhst = new List<string>();

                        //A7
                        if (dhst.PULSE.HasValue)
                        {
                            dataDhst.Add("Mạch: " + dhst.PULSE);
                        }

                        //A8
                        if (dhst.BLOOD_PRESSURE_MAX.HasValue || dhst.BLOOD_PRESSURE_MIN.HasValue)
                        {
                            dataDhst.Add(string.Format("Huyết áp: {0}/{1}", dhst.BLOOD_PRESSURE_MAX, dhst.BLOOD_PRESSURE_MIN));
                        }

                        //A9
                        if (dhst.WEIGHT.HasValue)
                        {
                            dataDhst.Add("Cân nặng: " + dhst.WEIGHT);
                        }

                        //A10
                        if (dhst.HEIGHT.HasValue)
                        {
                            dataDhst.Add("Chiều cao: " + dhst.HEIGHT);
                        }

                        //A11
                        if (dhst.TEMPERATURE.HasValue)
                        {
                            dataDhst.Add("Nhiệt độ: " + dhst.TEMPERATURE);
                        }

                        //A12
                        if (dhst.SPO2.HasValue)
                        {
                            dataDhst.Add("SPO2: " + dhst.SPO2);
                        }

                        //A13
                        if (dhst.BREATH_RATE.HasValue)
                        {
                            dataDhst.Add("Nhịp thở: " + dhst.BREATH_RATE);
                        }

                        if (dataDhst.Count > 0)
                        {
                            dienBien.Add(string.Format("Dấu hiệu sinh tồn: {0}", string.Join(". ", dataDhst)));
                        }
                    }

                    //A14
                    if (!String.IsNullOrWhiteSpace(examData.SUBCLINICAL))
                    {
                        dienBien.Add("Tóm tắt kết quả cls: " + examData.SUBCLINICAL);
                    }

                    //A15
                    if (!String.IsNullOrWhiteSpace(examData.TREATMENT_INSTRUCTION))
                    {
                        dienBien.Add("Phương pháp điều trị: " + examData.TREATMENT_INSTRUCTION);
                    }
                }

                if (dienBien.Count > 0)
                {
                    result = string.Join("; ", dienBien);
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //internal void MapADOToXml(List<Xml5ADO> listAdo, ref List<XML5DetailData> datas)
        //{
        //    try
        //    {
        //        if (datas == null)
        //            datas = new List<XML5DetailData>();
        //        if (listAdo != null || listAdo.Count > 0)
        //        {
        //            foreach (var ado in listAdo)
        //            {
        //                XML5DetailData detail = new XML5DetailData();
        //                detail.DIEN_BIEN = this.ConvertStringToXmlDocument(ado.DienBien);
        //                detail.HOI_CHAN = this.ConvertStringToXmlDocument(ado.HoiChan);
        //                detail.MA_LK = ado.MaLienKet;
        //                detail.NGAY_YL = ado.NgayYLenh;
        //                detail.PHAU_THUAT = this.ConvertStringToXmlDocument(ado.PhauThuat);
        //                detail.STT = ado.Stt;
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
    }
}
