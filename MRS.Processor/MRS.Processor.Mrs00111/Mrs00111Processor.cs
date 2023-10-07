using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;

namespace MRS.Processor.Mrs00111
{
    public class Mrs00111Processor : AbstractProcessor
    {
        Mrs00111Filter castFilter = null;
        List<Mrs00111RDO> ListRdo = new List<Mrs00111RDO>();
        List<Mrs00111RDO> ListTreatmentRdo = new List<Mrs00111RDO>();
        string DEPARTMENT_NAME;
        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();
        List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran;
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> DicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
        List<V_HIS_SERE_SERV> listSS = new List<V_HIS_SERE_SERV>();

        public Mrs00111Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00111Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00111Filter)this.reportFilter;

                CommonParam paramGet = new CommonParam();
                if (!String.IsNullOrEmpty(castFilter.HEIN_NUMBER_CODE))
                {
                    Inventec.Common.Logging.LogSystem.Debug("bat dau lay du lieu V_HIS_DEPARTMENT_TRAN Mrs00111, Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                    HisDepartmentTranViewFilterQuery depaTranFilter = new HisDepartmentTranViewFilterQuery();
                    depaTranFilter.DEPARTMENT_IN_TIME_FROM = castFilter.TIME_FROM;
                    depaTranFilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO;
                    //depaTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID; 
                    //depaTranFilter.IN_OUT = IMSys.DbConfig.HIS_RS.HIS_DEPARTMENT_TRAN.IN_OUT__IN; 
                    ListDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(depaTranFilter);
                    dicIcd = LoadDicHisIcd();

                    if (IsNotNullOrEmpty(ListDepartmentTran))
                    {
                        var treatmentIds = ListDepartmentTran.Select(o => o.TREATMENT_ID).Distinct().ToList();
                        var skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                            appFilter.TREATMENT_IDs = Ids;
                            var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter);
                            if (IsNotNullOrEmpty(currentPatientTypeAlter))
                            {
                                foreach (var item in currentPatientTypeAlter)
                                {
                                    if (!DicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                        DicPatientTypeAlter[item.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                                    DicPatientTypeAlter[item.TREATMENT_ID].Add(item);
                                }
                            }
                        }
                    }
                    if (!paramGet.HasException)
                    {
                        result = true;
                    }
                    else
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_DEPARTMENT_TRAN MRS00111.");
                    }
                }
                else if(castFilter.IS_BHYT_100 != false)
                {
                    HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                    filterTreatment.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                    filterTreatment.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    
                    listTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                    if (castFilter.TREATMENT_TYPE_IDs != null)
                    {
                        listTreatment = listTreatment.Where(p => castFilter.TREATMENT_TYPE_IDs.Contains(p.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }
                    if (IsNotNullOrEmpty(listTreatment))
                    {
                        var treatmentIDs = listTreatment.Select(p => p.ID).Distinct().ToList();
                        var skip = 0;
                        while (treatmentIDs.Count - skip > 0)
                        {
                            var IDs = treatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisSereServViewFilterQuery filterSS = new HisSereServViewFilterQuery();
                            filterSS.TREATMENT_IDs = IDs;

                            var ss = new HisSereServManager(paramGet).GetView(filterSS);
                            listSS.AddRange(ss);
                            HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                            appFilter.TREATMENT_IDs = IDs;
                            var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter);
                            if (IsNotNullOrEmpty(currentPatientTypeAlter))
                            {
                                foreach (var item in currentPatientTypeAlter)
                                {
                                    if (!DicPatientTypeAlter.ContainsKey(item.TREATMENT_ID))
                                        DicPatientTypeAlter[item.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                                    DicPatientTypeAlter[item.TREATMENT_ID].Add(item);
                                }
                            }
                        }
                    }
                    
                        result = true;
                    
                    
                }
                else
                {
                    throw new DataMisalignedException("Filter khong truyen vao HeinNumberCode hoac BHYT 100% MRS00111.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private Dictionary<string, HIS_ICD> LoadDicHisIcd()
        {
            Dictionary<string, HIS_ICD> result = new Dictionary<string, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(filter);
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

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessListDepartmentTran(ListDepartmentTran);
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    GetDepartmentById();
                }
                if (castFilter.IS_BHYT_100 == true)
                {
                    ProcessListSereServ(listTreatment.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList());
                }
                
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListSereServ(List<V_HIS_TREATMENT> listTreatment)
        {
            try
            {
                
                if (IsNotNullOrEmpty(listTreatment))
                {
                    
                    foreach (var item in listTreatment)
                    {
                        var sereServ = listSS != null ? listSS.Where(o => o.TDL_TREATMENT_ID == item.ID) : null;
                        Mrs00111RDO rdo = new Mrs00111RDO();

                        rdo.CASHIER_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.FEE_LOCK_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.CASHIER_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.FEE_LOCK_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        
                        rdo.CASHIER_USERNAME = item.FEE_LOCK_USERNAME;
                        rdo.CASHIER_LOGINNAME = item.FEE_LOCK_LOGINNAME;
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.FEE_LOCK_TIME ?? 0);
                        rdo.VIR_PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        if (sereServ != null)
                        {
                            rdo.TOTAL_HEIN_PRICE = sereServ.Sum(p => p.VIR_TOTAL_PRICE);
                            rdo.TOTAL_PATIENT_PRICE_BHYT = sereServ.Sum(p => p.VIR_TOTAL_PATIENT_PRICE_BHYT);
                            if (sereServ.Sum(p => p.VIR_TOTAL_PATIENT_PRICE_BHYT) == 0)
                            {
                                rdo.COUNT = 1;
                            }
                        }
                        ListTreatmentRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            try
            {
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    ListDepartmentTran = CheckDepartmentTran(ListDepartmentTran);
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListDepartmentTran = ListDepartmentTran.Where(o => o.DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                    }
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListDepartmentTran.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_DEPARTMENT_TRAN> hisDepartmentTrans = ListDepartmentTran.Skip(start).Take(limit).ToList();
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisDepartmentTrans.Select(s => s.TREATMENT_ID).ToList();
                        List<V_HIS_TREATMENT> ListTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        ProcessListTreatment(paramGet, ListTreatment, hisDepartmentTrans);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00111.");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> ListTreatment, List<V_HIS_DEPARTMENT_TRAN> hisDepartmentTrans)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    foreach (var treatment in ListTreatment)
                    {
                        Mrs00111RDO rdo = new Mrs00111RDO();
                        if (ProcessIsBhytType(paramGet, treatment, rdo))
                        {
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                            rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                            CalcuatorAge(rdo, treatment);
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool ProcessIsBhytType(CommonParam paramGet, V_HIS_TREATMENT treatment, Mrs00111RDO rdo)
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(DicPatientTypeAlter) && DicPatientTypeAlter.ContainsKey(treatment.ID))
                {
                    var currentPatientTypeAlter = DicPatientTypeAlter[treatment.ID].OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last();
                    if (currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        if (currentPatientTypeAlter.HEIN_CARD_NUMBER != null && currentPatientTypeAlter.HEIN_CARD_NUMBER.Contains(castFilter.HEIN_NUMBER_CODE))
                        {
                            rdo.HEIN_CARD_NUMBER = currentPatientTypeAlter.HEIN_CARD_NUMBER;
                            result = true;
                            if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                            {
                                rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                            }
                            else
                            {
                                rdo.DIAGNOSE_TUYENDUOI = (treatment.TRANSFER_IN_ICD_CODE != null && dicIcd.ContainsKey(treatment.TRANSFER_IN_ICD_CODE)) ? dicIcd[treatment.TRANSFER_IN_ICD_CODE].ICD_NAME : "";
                            }

                            rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;
                            rdo.GIOITHIEU = treatment.MEDI_ORG_NAME;
                        if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.DIAGNOSE_KKB = treatment.ICD_NAME;
                            rdo.ICD_CODE_KKB = treatment.ICD_CODE;
                        }
                        else
                        {
                            rdo.DIAGNOSE_KDT = treatment.ICD_NAME;
                            rdo.ICD_CODE_KDT = treatment.ICD_CODE;
                        }
                           
                           
                        }
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

        private List<V_HIS_DEPARTMENT_TRAN> CheckDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            List<V_HIS_DEPARTMENT_TRAN> currentDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
            try
            {
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    List<long> ListTreatmentId = ListDepartmentTran.Select(s => s.TREATMENT_ID).Distinct().ToList();
                    foreach (var treatId in ListTreatmentId)
                    {
                        var listData = ListDepartmentTran.Where(o => o.TREATMENT_ID == treatId).ToList();
                        if (listData != null && listData.Count > 0)
                        {
                            if (listData.Count == 1)
                            {
                                currentDepartmentTran.Add(listData.First());
                            }
                            else
                            {
                                currentDepartmentTran.Add(listData.OrderBy(o => o.DEPARTMENT_IN_TIME).First());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                currentDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
            }
            return currentDepartmentTran;
        }

        private void GetDepartmentById()
        {
            try
            {
                var department = new MOS.MANAGER.HisDepartment.HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value);
                if (IsNotNull(department))
                {
                    DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00111RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportTreatment", ListTreatmentRdo.Where(p => p.TOTAL_PATIENT_PRICE_BHYT == 0).ToList());
                dicSingleTag.Add("TREATMENT_COUNT", ListTreatmentRdo.Where(p => p.TOTAL_PATIENT_PRICE_BHYT == 0).Select(p => p.TREATMENT_CODE).Distinct().Count());
                dicSingleTag.Add("TOTAL_HEIN_PRICE_ALL", ListTreatmentRdo.Where(p => p.TOTAL_PATIENT_PRICE_BHYT == 0).Sum(p => p.TOTAL_HEIN_PRICE ?? 0));
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
