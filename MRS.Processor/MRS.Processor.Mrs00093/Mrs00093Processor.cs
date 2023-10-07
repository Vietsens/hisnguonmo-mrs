using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisIcd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00093
{
    public class Mrs00093Processor : AbstractProcessor
    {
        Mrs00093Filter castFilter = null;
        List<Mrs00093RDO> listRdo = new List<Mrs00093RDO>();
        List<DateTimeString> listDate = new List<DateTimeString>();
        List<COUNTTOTAL> listCount = new List<COUNTTOTAL>();
        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();
        Dictionary<long, List<V_HIS_EXP_MEST>> dicUserTime = new Dictionary<long, List<V_HIS_EXP_MEST>>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_EXP_MEST> ListPrescription;
        HIS_TREATMENT currentTreatment;

        public Mrs00093Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00093Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00093Filter)this.reportFilter);

                Inventec.Common.Logging.LogSystem.Debug("Ba dau lay du lieu V_HIS_PRESCRIPTION MRS00093." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                if (!String.IsNullOrEmpty(castFilter.TREATMENT_CODE.Trim()))
                {
                    string code = castFilter.TREATMENT_CODE.Trim();
                    if (code.Length < 12 && checkDigit(code))
                    {
                        code = string.Format("{0:000000000000}", Convert.ToInt64(code));
                    }
                    treatFilter.TREATMENT_CODE__EXACT = code;
                }
                var treatment = new HisTreatmentManager().Get(treatFilter);

                if (treatment != null && treatment.Count == 1)
                {
                    currentTreatment = treatment.FirstOrDefault();
                    HisExpMestViewFilterQuery presFilter = new HisExpMestViewFilterQuery();
                    presFilter.TDL_TREATMENT_ID = currentTreatment.ID;
                    presFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                    presFilter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                };
                    ListPrescription = new HisExpMestManager(paramGet).GetView(presFilter);
                    dicIcd = LoadDicHisIcd();
                    if (paramGet.HasException)
                    {
                        Inventec.Common.Logging.LogSystem.Error("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_PRESCRIPTION. MRS00093" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
                        throw new DataMisalignedException();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool checkDigit(string s)
        {
            bool result = false;
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsDigit(s[i]) == true) result = true;
                    else result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return result;
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListPrescription))
                {
                    ProcessListPrescription(paramGet, ListPrescription);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessDicSingleData(Dictionary<string, object> dicSingleTag)
        {
            try
            {
                var treatment = currentTreatment;// new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetViewById(castFilter.TREATMENT_ID);
                if (IsNotNull(treatment))
                {
                    dicSingleTag.Add("TREATMENT_CODE", treatment.TREATMENT_CODE);
                    dicSingleTag.Add("PATIENT_CODE", treatment.TDL_PATIENT_CODE);
                    dicSingleTag.Add("VIR_PATIENT_NAME", treatment.TDL_PATIENT_NAME);
                    dicSingleTag.Add("YEAR_AGE", GetYearAndAge(treatment.TDL_PATIENT_DOB));
                    dicSingleTag.Add("GENDER_NAME", treatment.TDL_PATIENT_GENDER_NAME);
                    dicSingleTag.Add("VIR_ADDRESS", treatment.TDL_PATIENT_ADDRESS);
                    dicSingleTag.Add("ICD_OUT_NAME", treatment.ICD_NAME ?? treatment.ICD_TEXT);

                    //Lấy dữ liệu thẻ bảo hiểm y tế nếu có của hồ sơ điều trị
                    ProcsssHeinCardToDicSingleData(dicSingleTag);

                    //Lấy thời gian vào viện và ra viện của hồ sơ điều trị
                    // ProcessInOutToDicSingleData(dicSingleTag); 
                    if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                    {
                        dicSingleTag.Add("ICD_IN_NAME",treatment.TRANSFER_IN_ICD_NAME);
                    }
                    else
                    {
                        dicSingleTag.Add("ICD_IN_NAME", (treatment.TRANSFER_IN_ICD_CODE != null && dicIcd.ContainsKey(treatment.TRANSFER_IN_ICD_CODE)) ? dicIcd[treatment.TRANSFER_IN_ICD_CODE].ICD_NAME : "");
                    }
                    dicSingleTag.Add("ICD_IN_CODE", treatment.TRANSFER_IN_ICD_CODE);
                   
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Dictionary<string, HIS_ICD> LoadDicHisIcd()
        {
            Dictionary<string, HIS_ICD> result = new Dictionary<string, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new HisIcdManager(param).Get(filter);
                foreach (var item in listIcd)
                {
                    if (String.IsNullOrEmpty(item.ICD_CODE)) continue;
                    if (!result.ContainsKey(item.ICD_CODE)) result[item.ICD_CODE] = item;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        private void ProcsssHeinCardToDicSingleData(Dictionary<string, object> dicSingleTag)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery ptaFilter = new HisPatientTypeAlterViewFilterQuery();
                ptaFilter.TREATMENT_ID = currentTreatment.ID;
                var PatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager().GetView(ptaFilter);
                if (IsNotNullOrEmpty(PatientTypeAlters))
                {
                    PatientTypeAlters = PatientTypeAlters.OrderByDescending(o => o.LOG_TIME).ToList();
                    if (PatientTypeAlters.First().PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {


                        dicSingleTag.Add("HEIN_CARD_NUMBER", RDOCommon.GenerateHeinCardSeparate(PatientTypeAlters.First().HEIN_CARD_NUMBER));
                        dicSingleTag.Add("HEIN_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(PatientTypeAlters.First().HEIN_CARD_FROM_TIME ?? 0));
                        dicSingleTag.Add("HEIN_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(PatientTypeAlters.First().HEIN_CARD_TO_TIME ?? 0));
                    }
                    else
                    {
                        dicSingleTag.Add("HEIN_CARD_NUMBER", null);
                        dicSingleTag.Add("HEIN_DATE_FROM_STR", null);
                        dicSingleTag.Add("HEIN_DATE_TO_STR", null);
                    }
                }
                else
                {
                    dicSingleTag.Add("HEIN_CARD_NUMBER", null);
                    dicSingleTag.Add("HEIN_DATE_FROM_STR", null);
                    dicSingleTag.Add("HEIN_DATE_TO_STR", null);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //private void ProcessInOutToDicSingleData(Dictionary<string, object> dicSingleTag)
        //{
        //    try
        //    {
        //        var departmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager().GetHospitalInOut(castFilter.TREATMENT_ID); 
        //        if (IsNotNullOrEmpty(departmentTrans))
        //        {
        //            if (IsNotNull(departmentTrans[0]))
        //            {
        //                dicSingleTag.Add("DATE_IN_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTrans[0].DEPARTMENT_IN_TIME ?? 0)); 
        //                if (departmentTrans.Count > 1 && IsNotNull(departmentTrans[1]))
        //                {
        //                    dicSingleTag.Add("DATE_OUT_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(departmentTrans[1].DEPARTMENT_IN_TIME ?? 0)); 
        //                    dicSingleTag.Add("DEPARTMENT_NAME", departmentTrans[1].DEPARTMENT_NAME); 
        //                }
        //                else
        //                {
        //                    dicSingleTag.Add("DATE_OUT_STR", null); 
        //                    dicSingleTag.Add("DEPARTMENT_NAME", departmentTrans[0].DEPARTMENT_NAME); 
        //                }
        //            }
        //            else
        //            {
        //                dicSingleTag.Add("DATE_IN_STR", null); 
        //                dicSingleTag.Add("DATE_OUT_STR", null); 
        //            }

        //        }
        //        else
        //        {
        //            dicSingleTag.Add("DATE_IN_STR", null); 
        //            dicSingleTag.Add("DATE_OUT_STR", null); 
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex); 
        //    }
        //}

        private void ProcessListPrescription(CommonParam paramGet, List<V_HIS_EXP_MEST> ListPrescription)
        {
            try
            {
                if (IsNotNullOrEmpty(ListPrescription))
                {
                    List<V_HIS_SERE_SERV> hisSereServs = new List<V_HIS_SERE_SERV>();
                    int start = 0;
                    int count = ListPrescription.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_EXP_MEST> hisPres = ListPrescription.Skip(start).Take(limit).ToList();
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_ID = currentTreatment.ID;
                        ssFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC; // Config.IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM; 
                        ssFilter.SERVICE_REQ_IDs = hisPres.Select(s => s.SERVICE_REQ_ID ?? 0).ToList();
                        var sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                        if (IsNotNullOrEmpty(sereServs))
                        {
                            hisSereServs.AddRange(sereServs);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xa ra tai DAOGET trong qua trinh tong hop du lieu MRS00093.");
                    }
                    else if (IsNotNullOrEmpty(hisSereServs))
                    {
                        ProcessDetailListPrescription(ListPrescription);
                        ProcessDetailListSereServ(hisSereServs);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdo.Clear();
            }
        }

        private void ProcessDetailListPrescription(List<V_HIS_EXP_MEST> ListPrescription)
        {
            try
            {
                if (IsNotNullOrEmpty(ListPrescription))
                {
                    ListPrescription = ListPrescription.OrderBy(o => o.AGGR_USE_TIME).ToList();
                    foreach (var pres in ListPrescription)
                    {
                        long time = GetDateLongByUserTime(pres.AGGR_USE_TIME ?? 0);
                        if (time > 0)
                        {
                            if (dicUserTime.ContainsKey(time))
                            {
                                dicUserTime[time].Add(pres);
                            }
                            else
                            {
                                List<V_HIS_EXP_MEST> listData = new List<V_HIS_EXP_MEST>();
                                listData.Add(pres);
                                dicUserTime.Add(time, listData);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ProcessDetailListSereServ(List<V_HIS_SERE_SERV> hisSereServs)
        {
            try
            {
                Dictionary<long, Mrs00093RDO> dicIdService = new Dictionary<long, Mrs00093RDO>();
                int count = 1;
                DateTimeString dateTime = new DateTimeString();
                foreach (var dic in dicUserTime)
                {
                    if (count > 26)
                    {
                        Inventec.Common.Logging.LogSystem.Info("So ngay vuot qua Max 25. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dicUserTime), dicUserTime));
                        break;
                    }
                    System.Reflection.PropertyInfo pi = typeof(DateTimeString).GetProperty("DATE_STR" + count);
                    pi.SetValue(dateTime, GetDateString(dic.Key));
                    var sereServ = hisSereServs.Where(o => dic.Value.Select(s => s.SERVICE_REQ_ID).Contains(o.SERVICE_REQ_ID)).ToList();
                    foreach (var sere in sereServ)
                    {
                        if (dicIdService.ContainsKey(sere.SERVICE_ID))
                        {
                            System.Reflection.PropertyInfo pi2 = typeof(Mrs00093RDO).GetProperty("AMOUNT" + count);
                            pi2.SetValue(dicIdService[sere.SERVICE_ID], sere.AMOUNT);
                        }
                        else
                        {
                            Mrs00093RDO rdo = new Mrs00093RDO();
                            rdo.SERVICE_NAME = sere.TDL_SERVICE_NAME;
                            rdo.SERVICE_UNIT_NAME = sere.SERVICE_UNIT_NAME;
                            System.Reflection.PropertyInfo pi2 = typeof(Mrs00093RDO).GetProperty("AMOUNT" + count);
                            pi2.SetValue(rdo, sere.AMOUNT);
                            dicIdService.Add(sere.SERVICE_ID, rdo);
                        }
                    }
                    count++;
                }
                listDate.Add(dateTime);
                listRdo.AddRange(dicIdService.Values.ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdo.Clear();
                listDate.Clear();
            }
        }

        private long GetDateLongByUserTime(long UserTime)
        {
            long time = 0;
            try
            {
                if (UserTime > 0)
                {
                    time = long.Parse(UserTime.ToString().Substring(0, 8));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                time = 0;
            }
            return time;
        }

        private string GetDateString(long time)
        {
            string result;
            try
            {
                result = time.ToString().Substring(6, 2) + "/" + time.ToString().Substring(4, 2);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public string GetYearAndAge(long dob)
        {
            string result = null;
            try
            {
                int tuoi = DateTime.Now.Year - int.Parse(dob.ToString().Substring(0, 4));
                if (tuoi >= 0)
                {
                    result = dob.ToString().Substring(0, 4) + "(" + tuoi + ")";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private void ProcssListCount()
        {
            try
            {
                COUNTTOTAL data = new COUNTTOTAL();
                int? count = 0;
                if (IsNotNullOrEmpty(listRdo))
                {
                    data.COUNT1 = (count = listRdo.Where(o => o.AMOUNT1 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT2 = (count = listRdo.Where(o => o.AMOUNT2 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT3 = (count = listRdo.Where(o => o.AMOUNT3 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT4 = (count = listRdo.Where(o => o.AMOUNT4 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT5 = (count = listRdo.Where(o => o.AMOUNT5 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT6 = (count = listRdo.Where(o => o.AMOUNT6 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT7 = (count = listRdo.Where(o => o.AMOUNT7 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT8 = (count = listRdo.Where(o => o.AMOUNT8 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT9 = (count = listRdo.Where(o => o.AMOUNT9 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT10 = (count = listRdo.Where(o => o.AMOUNT10 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT11 = (count = listRdo.Where(o => o.AMOUNT11 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT12 = (count = listRdo.Where(o => o.AMOUNT12 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT13 = (count = listRdo.Where(o => o.AMOUNT13 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT14 = (count = listRdo.Where(o => o.AMOUNT14 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT15 = (count = listRdo.Where(o => o.AMOUNT15 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT16 = (count = listRdo.Where(o => o.AMOUNT16 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT17 = (count = listRdo.Where(o => o.AMOUNT17 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT18 = (count = listRdo.Where(o => o.AMOUNT18 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT19 = (count = listRdo.Where(o => o.AMOUNT19 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT20 = (count = listRdo.Where(o => o.AMOUNT20 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT21 = (count = listRdo.Where(o => o.AMOUNT21 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT22 = (count = listRdo.Where(o => o.AMOUNT22 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT23 = (count = listRdo.Where(o => o.AMOUNT23 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT24 = (count = listRdo.Where(o => o.AMOUNT24 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT25 = (count = listRdo.Where(o => o.AMOUNT25 > 0).ToList().Count) > 0 ? count : null;
                    data.COUNT26 = (count = listRdo.Where(o => o.AMOUNT26 > 0).ToList().Count) > 0 ? count : null;
                }
                listCount.Add(data);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("USE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("USE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ProcessDicSingleData(dicSingleTag);

                ProcssListCount();
                if (listDate.Count == 0)
                    listDate.Add(new DateTimeString());

                objectTag.AddObjectData(store, "DateString", listDate);
                objectTag.AddObjectData(store, "Medicines", listRdo);
                objectTag.AddObjectData(store, "CountTotals", listCount);
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class RDOCustomerFuncRownumberData : TFlexCelUserFunction
    {
        public RDOCustomerFuncRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            long result = 0;
            try
            {
                long rownumber = Convert.ToInt64(parameters[0]);
                result = (rownumber + 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return result;
        }
    }

    public class DateTimeString
    {
        public string DATE_STR1 { get; set; }
        public string DATE_STR2 { get; set; }
        public string DATE_STR3 { get; set; }
        public string DATE_STR4 { get; set; }
        public string DATE_STR5 { get; set; }
        public string DATE_STR6 { get; set; }
        public string DATE_STR7 { get; set; }
        public string DATE_STR8 { get; set; }
        public string DATE_STR9 { get; set; }
        public string DATE_STR10 { get; set; }
        public string DATE_STR11 { get; set; }
        public string DATE_STR12 { get; set; }
        public string DATE_STR13 { get; set; }
        public string DATE_STR14 { get; set; }
        public string DATE_STR15 { get; set; }
        public string DATE_STR16 { get; set; }
        public string DATE_STR17 { get; set; }
        public string DATE_STR18 { get; set; }
        public string DATE_STR19 { get; set; }
        public string DATE_STR20 { get; set; }
        public string DATE_STR21 { get; set; }
        public string DATE_STR22 { get; set; }
        public string DATE_STR23 { get; set; }
        public string DATE_STR24 { get; set; }
        public string DATE_STR25 { get; set; }
        public string DATE_STR26 { get; set; }
    }

    public class COUNTTOTAL
    {
        public decimal? COUNT1 { get; set; }
        public decimal? COUNT2 { get; set; }
        public decimal? COUNT3 { get; set; }
        public decimal? COUNT4 { get; set; }
        public decimal? COUNT5 { get; set; }
        public decimal? COUNT6 { get; set; }
        public decimal? COUNT7 { get; set; }
        public decimal? COUNT8 { get; set; }
        public decimal? COUNT9 { get; set; }
        public decimal? COUNT10 { get; set; }
        public decimal? COUNT11 { get; set; }
        public decimal? COUNT12 { get; set; }
        public decimal? COUNT13 { get; set; }
        public decimal? COUNT14 { get; set; }
        public decimal? COUNT15 { get; set; }
        public decimal? COUNT16 { get; set; }
        public decimal? COUNT17 { get; set; }
        public decimal? COUNT18 { get; set; }
        public decimal? COUNT19 { get; set; }
        public decimal? COUNT20 { get; set; }
        public decimal? COUNT21 { get; set; }
        public decimal? COUNT22 { get; set; }
        public decimal? COUNT23 { get; set; }
        public decimal? COUNT24 { get; set; }
        public decimal? COUNT25 { get; set; }
        public decimal? COUNT26 { get; set; }
    }
}
