using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceUnit;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00026
{
    public class Mrs00026Processor : AbstractProcessor
    {
        Mrs00026Filter castFilter = null;
        List<HIS_PATIENT_TYPE> Parent = new List<HIS_PATIENT_TYPE>();
        List<Mrs00026RDO> ListRdo = new List<Mrs00026RDO>();
        List<long> CurentTreatmendId = new List<long>();
        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERVICE_UNIT> listHisServiceUnit = new List<HIS_SERVICE_UNIT>();
        CommonParam paramGet = new CommonParam();

        public Mrs00026Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00026Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00026Filter)this.reportFilter);
                CommonParam paramGet = new CommonParam();

                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID;
                ListCurrentSereServ = new HisSereServManager().Get(filter);
                if (this.ListCurrentSereServ != null)
                {
                    this.ListCurrentSereServ = this.ListCurrentSereServ.Where(o => o.IS_NO_EXECUTE != 1).ToList();
                }
                HisServiceUnitFilterQuery HisServiceUnitfilter = new HisServiceUnitFilterQuery();
                listHisServiceUnit = new HisServiceUnitManager(paramGet).Get(HisServiceUnitfilter);
                var listTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                        filterSereServBill.TDL_TREATMENT_IDs = listIDs;
                        filterSereServBill.IS_NOT_CANCEL = true;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                        ListSereServBill.AddRange(listSereServBillSub);
                    }
                }

                if (paramGet.HasException)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListCurrentSereServ))
                {
                    ProcessBeforeGeneralData(paramGet, ListCurrentSereServ);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessBeforeGeneralData(CommonParam paramGet, List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ)
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => (ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) 
                        || o.VIR_TOTAL_PATIENT_PRICE == 0) && o.AMOUNT > 0).ToList();
                    if (castFilter.TREATMENT_TYPE_ID.HasValue)
                    {
                        ProcessListCurrentSereServ(paramGet, ListCurrentSereServ, false);
                    }
                    else if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                    {
                        ProcessListCurrentSereServ(paramGet, ListCurrentSereServ, true);
                    }
                    else
                    {
                        ProcessListCurrentSereServ(ListCurrentSereServ);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        #region No Treatment Type
        private void ProcessListCurrentSereServ(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ)
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => (ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0) && o.AMOUNT > 0).ToList();
                    if (ListCurrentSereServ.Count > 0)
                    {
                        var Groups = ListCurrentSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                        foreach (var group in Groups)
                        {
                            List<HIS_SERE_SERV> listSub = group.ToList<HIS_SERE_SERV>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                ProcessDetailListSereServ(listSub);
                            }
                        }
                        ListRdo = ListRdo.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0 || o.VIR_TOTAL_PATIENT_PRICE > 0).ToList();
                        ListRdo = ListRdo.OrderBy(o => o.SERVICE_TYPE_CODE).ThenBy(t => t.SERVICE_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailListSereServ(List<HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (ListSereServ.Count > 0)
                {
                    var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == ListSereServ[0].TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                    var serviceUnit = listHisServiceUnit.FirstOrDefault(o => o.ID == ListSereServ[0].TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT();
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ListSereServ[0].PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    Mrs00026RDO rdo = new Mrs00026RDO();
                    rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;
                    rdo.SERVICE_ID = ListSereServ[0].SERVICE_ID;
                    rdo.SERVICE_CODE = ListSereServ[0].TDL_SERVICE_CODE;
                    rdo.SERVICE_NAME = ListSereServ[0].TDL_SERVICE_NAME;
                    foreach (var sereServ in ListSereServ)
                    {
                        rdo.AMOUNT += sereServ.AMOUNT;
                        rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                        rdo.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                    }
                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        #endregion

        #region Treatment Type
        private void ProcessListCurrentSereServ(CommonParam paramGet, List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ, bool isListTreatmentId)
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    if (ListCurrentSereServ.Count > 0)
                    {
                        List<long> ListTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                        int start = 0;
                        int count = ListTreatmentId.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            List<long> TreatmentIds = ListTreatmentId.Skip(start).Take(limit).ToList();
                            HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                            filter.TREATMENT_IDs = TreatmentIds;
                            var PatientTypeAlters = new HisPatientTypeAlterManager(paramGet).GetView(filter);
                            if (PatientTypeAlters != null && PatientTypeAlters.Count > 0)
                            {
                                CheckTreatmentType(TreatmentIds, PatientTypeAlters, isListTreatmentId);
                            }
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                        }
                        else
                        {
                            ProcessDetailSereServAndCurrentTreatmentId(ListCurrentSereServ);
                        }
                        ListRdo = ListRdo.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0 || o.VIR_TOTAL_PATIENT_PRICE > 0).ToList();
                        ListRdo = ListRdo.OrderBy(o => o.SERVICE_TYPE_CODE).ThenBy(t => t.SERVICE_ID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailSereServAndCurrentTreatmentId(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListCurrentSereServ) && IsNotNullOrEmpty(CurentTreatmendId))
                {
                    var listSereServ = ListCurrentSereServ.Where(o => CurentTreatmendId.Contains(o.TDL_TREATMENT_ID ?? 0)).ToList();
                    if (IsNotNullOrEmpty(listSereServ))
                    {
                        var Groups = listSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                        foreach (var group in Groups)
                        {
                            List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> hisSereServ = group.ToList<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                            Mrs00026RDO rdo = new Mrs00026RDO();
                            var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == hisSereServ[0].TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                            var serviceUnit = listHisServiceUnit.FirstOrDefault(o => o.ID == hisSereServ[0].TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT();
                            var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == hisSereServ[0].PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();

                            rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                            rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                            rdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;
                            rdo.SERVICE_ID = hisSereServ.First().SERVICE_ID;
                            rdo.SERVICE_CODE = hisSereServ.First().TDL_SERVICE_CODE;
                            rdo.SERVICE_NAME = hisSereServ.First().TDL_SERVICE_NAME;
                            rdo.AMOUNT = hisSereServ.Sum(s => s.AMOUNT);
                            rdo.VIR_TOTAL_PATIENT_PRICE = hisSereServ.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                            rdo.VIR_TOTAL_HEIN_PRICE = hisSereServ.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
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

        private void ProcessDetailListSereServ(List<HIS_SERE_SERV> ListSereServ, List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter, bool isListTreatmentId)
        {
            try
            {
                if (ListSereServ.Count > 0)
                {
                    Mrs00026RDO rdo = new Mrs00026RDO();
                    var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == ListSereServ[0].TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE();
                    var serviceUnit = listHisServiceUnit.FirstOrDefault(o => o.ID == ListSereServ[0].TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT();
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ListSereServ[0].PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();

                    rdo.SERVICE_TYPE_CODE = serviceType.SERVICE_TYPE_CODE;
                    rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                    rdo.SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = serviceUnit.SERVICE_UNIT_NAME;
                    rdo.SERVICE_ID = ListSereServ[0].SERVICE_ID;
                    rdo.SERVICE_CODE = ListSereServ[0].TDL_SERVICE_CODE;
                    rdo.SERVICE_NAME = ListSereServ[0].TDL_SERVICE_NAME;
                    foreach (var sereServ in ListSereServ)
                    {
                        var PatientTypeAlter = ListPatientTypeAlter.Where(s => s.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).ToList();
                        if (PatientTypeAlter != null && PatientTypeAlter.Count > 0)
                        {
                            PatientTypeAlter = PatientTypeAlter.OrderByDescending(d => d.LOG_TIME).ToList();
                            if (isListTreatmentId && castFilter.TREATMENT_TYPE_IDs.Contains(PatientTypeAlter[0].TREATMENT_TYPE_ID))
                            {
                                rdo.AMOUNT += sereServ.AMOUNT;
                                rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                rdo.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                ListRdo.Add(rdo);
                            }
                            else if (!isListTreatmentId && PatientTypeAlter[0].TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)
                            {
                                rdo.AMOUNT += sereServ.AMOUNT;
                                rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                rdo.VIR_TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                                ListRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CheckTreatmentType(List<long> TreatmentIds, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, bool isListTreatmentId)
        {
            try
            {
                foreach (var id in TreatmentIds)
                {
                    var PatientTypeAlter = PatientTypeAlters.Where(s => s.TREATMENT_ID == id).ToList();
                    if (PatientTypeAlter != null && PatientTypeAlter.Count > 0)
                    {
                        PatientTypeAlter = PatientTypeAlter.OrderByDescending(d => d.LOG_TIME).ToList();
                        if (isListTreatmentId && castFilter.TREATMENT_TYPE_IDs.Contains(PatientTypeAlter[0].TREATMENT_TYPE_ID))
                        {
                            if (!CurentTreatmendId.Contains(id))
                            {
                                CurentTreatmendId.Add(id);
                            }
                        }
                        else if (!isListTreatmentId && PatientTypeAlter[0].TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)
                        {
                            if (!CurentTreatmendId.Contains(id))
                            {
                                CurentTreatmendId.Add(id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        #endregion

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                Parent = HisPatientTypeCFG.PATIENT_TYPEs;
                objectTag.AddObjectData(store, "Parent", Parent);
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddRelationship(store, "Parent", "Report", "PATIENT_TYPE_NAME", "PATIENT_TYPE_NAME");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
