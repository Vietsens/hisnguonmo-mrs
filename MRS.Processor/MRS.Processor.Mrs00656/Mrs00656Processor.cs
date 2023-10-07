using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00656
{
    class Mrs00656Processor : AbstractProcessor
    {
        List<Mrs00656RDO> ListRdo = new List<Mrs00656RDO>();
        List<Mrs00656RDO> ListRdoDepartment = new List<Mrs00656RDO>();
        List<Mrs00656RDO> ListRdoTotal = new List<Mrs00656RDO>();
        Mrs00656Filter castFilter;
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV_PTTT> listHisSereServPttt = new List<HIS_SERE_SERV_PTTT>();

        public Mrs00656Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00656Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00656Filter)reportFilter);

                HisSereServExtFilterQuery sereServExt = new HisSereServExtFilterQuery();
                sereServExt.END_TIME_FROM = castFilter.TIME_FROM;
                sereServExt.END_TIME_TO = castFilter.TIME_TO;
                if (castFilter.IS_NOT_FEE.HasValue) sereServExt.IS_NOT_FEE = castFilter.IS_NOT_FEE == 1;
                if (castFilter.IS_NOT_GATHER_DATA.HasValue) sereServExt.IS_NOT_GATHER_DATA = castFilter.IS_NOT_GATHER_DATA == 1;
                var listSsExt = new HisSereServExtManager().Get(sereServExt);

                List<HIS_DEPARTMENT> departments = new List<HIS_DEPARTMENT>();
                if (castFilter.IS_NT_NGT.HasValue)
                {
                    if (castFilter.IS_NT_NGT.Value == 0)
                    {
                        departments = HisDepartmentCFG.DEPARTMENTs.Where(o => o.REQ_SURG_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    }
                    else if (castFilter.IS_NT_NGT.Value == 1)
                    {
                        departments = HisDepartmentCFG.DEPARTMENTs.Where(o => o.REQ_SURG_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }

                var listSereServIds = listSsExt.Select(s => s.SERE_SERV_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listSereServIds))
                {
                    var skip = 0;
                    while (listSereServIds.Count - skip > 0)
                    {
                        var listIDs = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.IDs = listIDs;
                        filterSs.TDL_SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT };
                        filterSs.HAS_EXECUTE = true;
                        if (IsNotNullOrEmpty(departments))
                        {
                            filterSs.TDL_REQUEST_DEPARTMENT_IDs = departments.Select(s => s.ID).ToList();
                        }

                        if (castFilter.IS_PT_TT.HasValue)
                        {
                            if (castFilter.IS_PT_TT.Value == 0)
                            {
                                filterSs.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT;
                            }
                            else if (castFilter.IS_PT_TT.Value == 1)
                            {
                                filterSs.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT;
                            }
                        }

                        var listSereServSub = new HisSereServManager().Get(filterSs);
                        if (listSereServSub != null)
                            ListSereServ.AddRange(listSereServSub);
                    }

                    if (IsNotNullOrEmpty(ListSereServ)) ListSereServ = ListSereServ.Where(o => o.SERVICE_REQ_ID.HasValue && o.TDL_TREATMENT_ID.HasValue).ToList();
                }

                if (IsNotNullOrEmpty(castFilter.EXECUTE_DEPARTMENT_IDs) && IsNotNullOrEmpty(ListSereServ))
                {
                    ListSereServ = ListSereServ.Where(o => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                }

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    var listTreatmentId = ListSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                            HisPatientTypeAlterfilter.TREATMENT_IDs = listIDs;
                            var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager().Get(HisPatientTypeAlterfilter);
                            if (listHisPatientTypeAlterSub != null)
                                listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                        }

                        lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                    }

                    if (castFilter.TREATMENT_TYPE_ID.HasValue)
                    {
                        ListSereServ = ListSereServ.Where(o => lastHisPatientTypeAlter.Exists(
                            p => o.TDL_TREATMENT_ID == p.TREATMENT_ID && p.TREATMENT_TYPE_ID == castFilter.TREATMENT_TYPE_ID)).ToList();
                    }

                    var sereServIds = ListSereServ.Select(o => o.ID).ToList();
                    if (IsNotNullOrEmpty(sereServIds))
                    {
                        var skip = 0;
                        while (sereServIds.Count - skip > 0)
                        {
                            var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServPtttFilterQuery HisSereServPtttfilter = new HisSereServPtttFilterQuery();
                            HisSereServPtttfilter.SERE_SERV_IDs = listIDs;
                            var listHisSereServPtttSub = new HisSereServPtttManager().Get(HisSereServPtttfilter);
                            if (listHisSereServPtttSub != null)
                                listHisSereServPttt.AddRange(listHisSereServPtttSub);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                Dictionary<long, HIS_SERE_SERV_PTTT> dicPttt = new Dictionary<long, HIS_SERE_SERV_PTTT>();

                if (IsNotNullOrEmpty(listHisSereServPttt))
                {
                    foreach (var item in listHisSereServPttt)
                    {
                        if (!dicPttt.ContainsKey(item.SERE_SERV_ID))
                        {
                            dicPttt[item.SERE_SERV_ID] = item;
                        }
                    }
                }

                List<Mrs00656RDO> lstDataSereServ = new List<Mrs00656RDO>();

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var item in ListSereServ)
                    {
                        HIS_SERE_SERV_PTTT pttt = new HIS_SERE_SERV_PTTT();
                        if (dicPttt.ContainsKey(item.ID)) pttt = dicPttt[item.ID];

                        lstDataSereServ.Add(new Mrs00656RDO(item, pttt));
                    }
                }

                if (IsNotNullOrEmpty(lstDataSereServ))
                {
                    //sửa theo 21589
                    //Tổng số = kế hoạch + cấp cứu
                    lstDataSereServ = lstDataSereServ.OrderBy(o => o.PTTT_GROUP_CODE).ToList();
                    var groupDepartment = lstDataSereServ.GroupBy(o => new { o.TDL_EXECUTE_DEPARTMENT_ID, o.PTTT_GROUP_ID }).ToList();
                    foreach (var depa in groupDepartment)
                    {
                        Mrs00656RDO rdo = new Mrs00656RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00656RDO>(rdo, depa.First());
                        rdo.COUNT_CC = depa.Sum(s => s.COUNT_CC);
                        rdo.COUNT_DV = depa.Sum(s => s.COUNT_DV);
                        rdo.COUNT_KH = depa.Sum(s => s.COUNT_KH);
                        rdo.COUNT_YC = depa.Sum(s => s.COUNT_YC);
                        rdo.COUNT_OTHER = depa.Sum(s => s.COUNT_OTHER);
                        //rdo.COUNT_TOTAL = depa.Sum(s => s.COUNT_TOTAL);
                        rdo.COUNT_TOTAL = rdo.COUNT_CC + rdo.COUNT_KH + rdo.COUNT_YC;
                        ListRdo.Add(rdo);
                    }

                    //if (IsNotNullOrEmpty(ListRdo)) ListRdo = ListRdo.Where(o => o.COUNT_TOTAL > 0).ToList();
                    if (IsNotNullOrEmpty(ListRdo)) ListRdo = ListRdo.Where(o => o.COUNT_CC > 0 || o.COUNT_DV > 0 || o.COUNT_KH > 0).ToList();

                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        var department = ListRdo.GroupBy(o => o.TDL_EXECUTE_DEPARTMENT_ID).ToList();
                        foreach (var de in department)
                        {
                            Mrs00656RDO rdo = new Mrs00656RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00656RDO>(rdo, de.First());
                            rdo.COUNT_CC = de.Sum(s => s.COUNT_CC);
                            rdo.COUNT_DV = de.Sum(s => s.COUNT_DV);
                            rdo.COUNT_KH = de.Sum(s => s.COUNT_KH);
                            rdo.COUNT_YC = de.Sum(s => s.COUNT_YC);
                            rdo.COUNT_OTHER = de.Sum(s => s.COUNT_OTHER);
                            //rdo.COUNT_TOTAL = de.Sum(s => s.COUNT_TOTAL);
                            rdo.COUNT_TOTAL = rdo.COUNT_CC + rdo.COUNT_KH;
                            rdo.COUNT_TOTAL = rdo.COUNT_CC + rdo.COUNT_KH + rdo.COUNT_YC;
                            ListRdoDepartment.Add(rdo);
                        }

                        var groupTotal = ListRdo.GroupBy(o => o.PTTT_GROUP_ID).ToList();
                        foreach (var total in groupTotal)
                        {
                            Mrs00656RDO rdo = new Mrs00656RDO();
                            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00656RDO>(rdo, total.First());
                            rdo.COUNT_CC = total.Sum(s => s.COUNT_CC);
                            rdo.COUNT_DV = total.Sum(s => s.COUNT_DV);
                            rdo.COUNT_KH = total.Sum(s => s.COUNT_KH);
                            rdo.COUNT_YC = total.Sum(s => s.COUNT_YC);
                            rdo.COUNT_OTHER = total.Sum(s => s.COUNT_OTHER);
                            //rdo.COUNT_TOTAL = total.Sum(s => s.COUNT_TOTAL);
                            rdo.COUNT_TOTAL = rdo.COUNT_CC + rdo.COUNT_KH;
                            rdo.COUNT_TOTAL = rdo.COUNT_CC + rdo.COUNT_KH + rdo.COUNT_YC;
                            ListRdoTotal.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                if (castFilter.IS_PT_TT.HasValue)
                {
                    if (castFilter.IS_PT_TT.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_PT_TT", "THỦ THUẬT");
                    }
                    else if (castFilter.IS_PT_TT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_PT_TT", "PHẪU THUẬT");
                    }
                }

                if (castFilter.IS_NT_NGT.HasValue)
                {
                    if (castFilter.IS_NT_NGT.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NGOẠI TRÚ");
                    }
                    else if (castFilter.IS_NT_NGT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NỘI TRÚ");
                    }
                }

                Mrs00656RDO total = new Mrs00656RDO();
                total.COUNT_CC = ListRdoTotal.Sum(s => s.COUNT_CC);
                total.COUNT_DV = ListRdoTotal.Sum(s => s.COUNT_DV);
                total.COUNT_KH = ListRdoTotal.Sum(s => s.COUNT_KH);
                total.COUNT_YC = ListRdoTotal.Sum(s => s.COUNT_YC);
                total.COUNT_TOTAL = ListRdoTotal.Sum(s => s.COUNT_TOTAL);
                dicSingleTag.Add("COUNT_CC", total.COUNT_CC);
                dicSingleTag.Add("COUNT_DV", total.COUNT_DV);
                dicSingleTag.Add("COUNT_KH", total.COUNT_KH);
                dicSingleTag.Add("COUNT_YC", total.COUNT_YC);
                dicSingleTag.Add("COUNT_TOTAL", total.COUNT_TOTAL);

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportDepartment", ListRdoDepartment);
                objectTag.AddRelationship(store, "ReportDepartment", "Report", "TDL_EXECUTE_DEPARTMENT_ID", "TDL_EXECUTE_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "ReportTotal", ListRdoTotal);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
