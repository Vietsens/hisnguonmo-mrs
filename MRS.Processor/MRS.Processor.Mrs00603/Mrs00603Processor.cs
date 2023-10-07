using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTranPatiForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
//using MOS.MANAGER.HisTranPati; 
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTransaction;


namespace MRS.Processor.Mrs00603
{
    public class Mrs00603Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00603RDO> ListRdo = new List<Mrs00603RDO>();
        List<V_HIS_TREATMENT> listHisTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Mrs00603Filter filter = null;
        public List<HIS_TREATMENT_STT> listTreatmentSTT = new List<HIS_TREATMENT_STT>();
        private List<Mrs00603RDO_TotalDepartment> ListRdoDepartment = new List<Mrs00603RDO_TotalDepartment>();

        Dictionary<long, List<HIS_DEPARTMENT_TRAN>> dicDepartmentTran = new Dictionary<long, List<HIS_DEPARTMENT_TRAN>>();
        Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>>();

        long PatientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;

        public Mrs00603Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00603Filter);
        }

        protected override bool GetData()///
        {
            filter = ((Mrs00603Filter)reportFilter);
            var result = true;
            try
            {

                if (filter.IS_END == true)
                {
                    filter.TIME_FROM = filter.TIME_TO;
                }
                HisTreatmentViewFilterQuery listHisTreatmentfilter = new HisTreatmentViewFilterQuery();
                listHisTreatmentfilter.IS_OUT = false;
                listHisTreatmentfilter.IN_TIME_TO = filter.TIME_TO;
                listHisTreatmentfilter.CLINICAL_IN_TIME_FROM = 1;
                var listTreatment = new HisTreatmentManager().GetView(listHisTreatmentfilter);
                if (listTreatment != null)
                {
                    listHisTreatment.AddRange(listTreatment);
                }
               
                listHisTreatmentfilter = new HisTreatmentViewFilterQuery();
                listHisTreatmentfilter.IN_TIME_TO = filter.TIME_TO;
                listHisTreatmentfilter.IS_OUT = true;
                listHisTreatmentfilter.OUT_TIME_FROM = filter.TIME_FROM;
                listHisTreatmentfilter.CLINICAL_IN_TIME_FROM = 1;
                var listTreatmentOut = new HisTreatmentManager().GetView(listHisTreatmentfilter);
                if (listTreatmentOut != null)
                {
                    listHisTreatment.AddRange(listTreatmentOut);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(o => filter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }

                if (filter.INPUT_DATA_ID_PATIENT_STT == 1) //chưa ra viện
                {
                    listHisTreatment = listHisTreatment.Where(p => p.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (filter.INPUT_DATA_ID_PATIENT_STT == 2 || filter.INPUT_DATA_ID_PATIENT_STT == 3)
                {
                    var listBillTreatmentId = new List<long>();
                    var skip = 0;
                    var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTransactionFilterQuery transacFilter = new HisTransactionFilterQuery();
                        transacFilter.TREATMENT_IDs = listIds;
                        transacFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                        var listTransSub = new HisTransactionManager().Get(transacFilter);
                        if (listTransSub != null)
                        {
                            listBillTreatmentId.AddRange(listTransSub.Select(o => o.TREATMENT_ID ?? 0).ToList());
                        }
                    }
                    if (filter.INPUT_DATA_ID_PATIENT_STT == 2) //đã thanh toán
                    {
                        listHisTreatment = listHisTreatment.Where(p => listBillTreatmentId.Contains(p.ID)).ToList();
                    }

                    if (filter.INPUT_DATA_ID_PATIENT_STT == 3) //đã ra viện nhưng chưa thanh toán
                    {
                        listHisTreatment = listHisTreatment.Where(p => p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && !listBillTreatmentId.Contains(p.ID)).ToList();
                    }
                }
                //var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();

                var treatmentTreatIds = listHisTreatment.Where(p => p.CLINICAL_IN_TIME != null).Select(o => o.ID).ToList();
                if (treatmentTreatIds != null && treatmentTreatIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentTreatIds.Count - skip > 0)
                    {
                        var Ids = treatmentTreatIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        HisDepartmentTranfilter.DEPARTMENT_IN_TIME_FROM = 0;
                        //HisDepartmentTranfilter.DEPARTMENT_ID = filter.DEPARTMENT_ID;
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);

                        HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery
                        {
                            TREATMENT_IDs = Ids
                        };
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager().Get(patientTypeAlterFilter);
                        listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }

                dicDepartmentTran = listHisDepartmentTran.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());

                dicPatientTypeAlter = listHisPatientTypeAlter.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                listHisTreatment = listHisTreatment.Where(o => dicPatientTypeAlter.ContainsKey(o.ID) && dicPatientTypeAlter[o.ID].Exists(p => p.TREATMENT_ID == o.ID && (p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))).ToList();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                ListRdo.Clear();
               
                foreach (var item in listHisTreatment)
                {
                   
                    List<HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                    if (dicDepartmentTran.ContainsKey(item.ID))
                    {
                        listDepartmentTran.AddRange(dicDepartmentTran[item.ID]);
                    }
                    var transDepartment = listDepartmentTran.Where(x => x.TREATMENT_ID == item.ID).ToList();

                    if (filter.DEPARTMENT_ID != null)
                    {
                        transDepartment = transDepartment.Where(o => o.DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                    }

                    if (filter.DEPARTMENT_IDs != null)
                    {
                        transDepartment = transDepartment.Where(o => filter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                    }

                    var patientTypeAlterSub = dicPatientTypeAlter.ContainsKey(item.ID) ? dicPatientTypeAlter[item.ID].Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList() : new List<HIS_PATIENT_TYPE_ALTER>();
                    if (filter.IS_TREAT_IN.HasValue)
                    {
                        if (filter.IS_TREAT_IN.Value == true)
                        {
                            patientTypeAlterSub = patientTypeAlterSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                        }
                        else
                        {
                            patientTypeAlterSub = patientTypeAlterSub.Where(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                        }
                    }
                    foreach (var tran in transDepartment)
                    {
                        bool isTreatIn = this.HasTreatIn(tran, patientTypeAlterSub);

                        bool isTreatOut = this.HasTreatOut(tran, patientTypeAlterSub);

                        //BC hiển thị BN nội trú, ngoại trú
                        if (!isTreatIn && !isTreatOut) continue;

                        if (filter.IS_TREAT_IN.HasValue)
                        {
                            if (filter.IS_TREAT_IN.Value == true)
                            {
                                if (!isTreatIn) continue;
                            }
                            else
                            {
                                if (!isTreatOut) continue;
                            }
                        }

                        HIS_DEPARTMENT_TRAN previousDepartmentTran = PreviousDepartment(tran, listDepartmentTran);

                        bool isPrevTreatIn = this.HasTreatIn(previousDepartmentTran, patientTypeAlterSub);

                        bool isPrevTreatOut = this.HasTreatOut(previousDepartmentTran, patientTypeAlterSub);

                        HIS_DEPARTMENT_TRAN nextDepartmentTran = NextDepartment(tran, listDepartmentTran);
                        //chi lay cac vao khoa co thoi gian ra khoa sau time_from
                        long? departmentOutTime = nextDepartmentTran != null ? nextDepartmentTran.DEPARTMENT_IN_TIME : item.OUT_TIME;
                        if (departmentOutTime <= filter.TIME_FROM)
                            continue;
                        //chi lay các vao khoa truoc time_to
                        if (tran.DEPARTMENT_IN_TIME > filter.TIME_TO)
                            continue;
                        Mrs00603RDO rdo = new Mrs00603RDO(item, listDepartmentTran, tran, filter, isPrevTreatIn, isPrevTreatOut, previousDepartmentTran, nextDepartmentTran, departmentOutTime);

                        if (item.TDL_PATIENT_TYPE_ID == PatientTypeIdBhyt)
                        {
                            rdo.IS_BHYT = "x";
                        }
                        rdo.PATIENT_DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB);
                        rdo.IN_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IN_DATE);
                        if (item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.SEX = "1";
                        }
                        if (item.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.SEX = "0";
                        }

                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.OUT_DATE ?? 0);
                        ListRdo.Add(rdo);

                    }
                   
                    if (IsNotNullOrEmpty(listDepartmentTran) && item.OUT_TIME.HasValue)
                    {
                        //xep theo thoi gian vao khoa giam dan
                        listDepartmentTran = listDepartmentTran.OrderByDescending(o => o.DEPARTMENT_IN_TIME ?? 99999999999999).ThenByDescending(o => o.ID).ToList();

                        for (int i = 0; i < listDepartmentTran.Count; i++)
                        {
                            Mrs00603RDO_TotalDepartment de = new Mrs00603RDO_TotalDepartment();
                            de.DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listDepartmentTran[i].DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                            de.DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listDepartmentTran[i].DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                           
                            decimal dayCount = 0;

                            if (listDepartmentTran[i].DEPARTMENT_IN_TIME.HasValue)
                            {
                                DateTime depaTranInTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(listDepartmentTran[i].DEPARTMENT_IN_TIME.Value) ?? DateTime.MinValue;
                                var tranPrevious = listDepartmentTran.FirstOrDefault(o => o.PREVIOUS_ID == listDepartmentTran[i].ID);
                                if (tranPrevious != null)
                                {
                                    if (tranPrevious.DEPARTMENT_IN_TIME.HasValue)
                                    {
                                        DateTime tranPreviousInTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(tranPrevious.DEPARTMENT_IN_TIME.Value) ?? DateTime.MinValue;
                                        dayCount = ProcessDayCount(tranPreviousInTime, depaTranInTime, true);
                                    }
                                    else //ban tin chuyen khoa sau khong co thoi gian vao khoa thi lay thoi gian ra vien
                                    {
                                        if (item.OUT_TIME.HasValue)
                                        {
                                            DateTime outTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.OUT_TIME.Value) ?? DateTime.MinValue;
                                            dayCount = ProcessDayCount(outTime, depaTranInTime, false);
                                        }
                                        else //khong co thoi gian ra vien bo qua
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            else //chua vao khoa khong tinh
                            {
                                continue;
                            }

                            de.TOTAL_DAY_IN = dayCount;
                            if (listDepartmentTran[i].DEPARTMENT_ID == item.END_DEPARTMENT_ID)
                            {
                                de.TOTAL_DAY_END = dayCount;
                            }

                            ListRdoDepartment.Add(de);
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListRdoDepartment))
                {
                    ListRdoDepartment = ListRdoDepartment.GroupBy(o => o.DEPARTMENT_CODE).Select(s => new Mrs00603RDO_TotalDepartment()
                    {
                        DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                        TOTAL_DAY_END = s.Sum(t => t.TOTAL_DAY_END),
                        TOTAL_DAY_IN = s.Sum(t => t.TOTAL_DAY_IN)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        //khoa truoc
        private HIS_DEPARTMENT_TRAN PreviousDepartment(HIS_DEPARTMENT_TRAN o, List<HIS_DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.ID == o.PREVIOUS_ID);

        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o, List<HIS_DEPARTMENT_TRAN> listHisDepartmentTranAll)
        {

            return listHisDepartmentTranAll.FirstOrDefault(p => p.PREVIOUS_ID == o.ID);

        }
        private decimal ProcessDayCount(DateTime end, DateTime begin, bool isTran)
        {
            decimal result = 0;
            try
            {
                var hoursBetween = end - begin;
                if (hoursBetween.TotalHours > 8)//Lay ngay ra - ngay vao + 1
                {
                    DateTime dayEnd = new DateTime(end.Year, end.Month, end.Day);
                    DateTime dayBegin = new DateTime(begin.Year, begin.Month, begin.Day);

                    if (isTran)//chuyen khoa se tinh 0,5 ngay
                    {
                        result = Convert.ToInt64((dayEnd - dayBegin).TotalDays) + 0.5m;
                    }
                    else
                    {
                        result = Convert.ToInt64((dayEnd - dayBegin).TotalDays) + 1;
                    }
                }
                else if (hoursBetween.TotalHours <= 8 && hoursBetween.TotalHours >= 4)//Neu BN nam tu 4 - 8h --> tinh 1 ngay
                {
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                result = 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);

                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "Department", ListRdoDepartment.OrderBy(o => o.DEPARTMENT_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {
            return listHisDepartmentTran.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();
        }

        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool HasTreatOut(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                if (departmentTran == null)
                    return false;
                if (listPatientTypeAlter == null)
                    return false;
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID).ToList();

                if (patientTypeAlterSub.Exists(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                {
                    return false;
                }
                return patientTypeAlterSub.Exists(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID || o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
