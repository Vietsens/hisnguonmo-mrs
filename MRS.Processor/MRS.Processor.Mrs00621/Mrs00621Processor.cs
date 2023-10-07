using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00621
{
    class Mrs00621Processor : AbstractProcessor
    {
        Mrs00621Filter castFilter = null;
        List<V_HIS_TREATMENT> ListTreatmentGet = new List<V_HIS_TREATMENT>();
        List<HIS_SERE_SERV> ListSereServGet = new List<HIS_SERE_SERV>();

        List<Mrs00621RDO> ListRdo = new List<Mrs00621RDO>();
        List<HIS_SERE_SERV> ListSereServReport = new List<HIS_SERE_SERV>();

        HIS_DEPARTMENT Department;

        List<long> SERVICE_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
        };

        public Mrs00621Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00621Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00621Filter)this.reportFilter;

                CommonParam paramGet = new CommonParam();
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                filterTreatment.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                filterTreatment.IS_ACTIVE = 0;
                ListTreatmentGet = new HisTreatmentManager(paramGet).GetView(filterTreatment);

                if (ListTreatmentGet != null && ListTreatmentGet.Count > 0)
                {
                    //ListTreatmentGet = ListTreatmentGet.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatmentGet = ListTreatmentGet.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
                        Department = new HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value);
                    }

                    if (ListTreatmentGet != null && ListTreatmentGet.Count > 0)
                    {
                        List<HIS_PATIENT_TYPE_ALTER> lstPaty = new List<HIS_PATIENT_TYPE_ALTER>();
                        var listTreatmentId = ListTreatmentGet.Select(p => p.ID).ToList();
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisPatientTypeAlterFilterQuery filterPaty = new HisPatientTypeAlterFilterQuery();
                            filterPaty.TREATMENT_IDs = listIDs;
                            var listPatySub = new HisPatientTypeAlterManager(paramGet).Get(filterPaty);
                            if (IsNotNullOrEmpty(listPatySub))
                                lstPaty.AddRange(listPatySub);
                        }

                        List<long> treatId = new List<long>();
                        if (lstPaty != null && lstPaty.Count > 0)
                        {
                            var treatGroup = lstPaty.GroupBy(o => o.TREATMENT_ID).ToList();
                            foreach (var group in treatGroup)
                            {
                                var treatType = group.OrderByDescending(o => o.LOG_TIME).FirstOrDefault();
                                if (treatType.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                                {
                                    treatId.Add(treatType.TREATMENT_ID);
                                }
                            }
                        }

                        ListTreatmentGet = ListTreatmentGet.Where(o => treatId.Contains(o.ID)).ToList();
                    }

                    if (ListTreatmentGet != null && ListTreatmentGet.Count > 0)
                    {
                        var listTreatmentId = ListTreatmentGet.Select(p => p.ID).ToList();
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery();
                            filterSereServ.TREATMENT_IDs = listIDs;
                            filterSereServ.HAS_EXECUTE = true;
                            filterSereServ.TDL_SERVICE_TYPE_IDs = SERVICE_TYPE_IDs;//loai pttt
                            var listSereServSub = new HisSereServManager(paramGet).Get(filterSereServ);
                            if (IsNotNullOrEmpty(listSereServSub))
                                ListSereServGet.AddRange(listSereServSub);
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
                if (ListTreatmentGet != null && ListTreatmentGet.Count > 0)
                {
                    ListRdo.Clear();
                    ListSereServReport.Clear();

                    Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<HIS_SERE_SERV>>();
                    if (ListSereServGet != null && ListSereServGet.Count > 0)
                    {
                        foreach (var item in ListSereServGet)
                        {
                            if (!item.TDL_TREATMENT_ID.HasValue) continue;
                            if (!dicSereServ.ContainsKey(item.TDL_TREATMENT_ID.Value))
                                dicSereServ[item.TDL_TREATMENT_ID.Value] = new List<HIS_SERE_SERV>();
                            dicSereServ[item.TDL_TREATMENT_ID.Value].Add(item);
                        }
                    }

                    foreach (var item in ListTreatmentGet)
                    {
                        Mrs00621RDO rdo = new Mrs00621RDO(item);
                        if (dicSereServ.ContainsKey(item.ID))
                        {
                            var grSereServ = dicSereServ[item.ID].GroupBy(o => o.SERVICE_ID).ToList();
                            foreach (var gr in grSereServ)
                            {
                                HIS_SERE_SERV ss = new HIS_SERE_SERV();
                                Inventec.Common.Mapper.DataObjectMapper.Map<HIS_SERE_SERV>(ss, gr.First());
                                ss.AMOUNT = gr.Sum(s => s.AMOUNT);
                                ss.TDL_TREATMENT_ID = item.ID;
                                ListSereServReport.Add(ss);
                            }
                        }
                        else
                        {
                            ListSereServReport.Add(new HIS_SERE_SERV() { TDL_TREATMENT_ID = item.ID });
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                ListRdo.Clear();
                ListSereServReport.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));

                if (Department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", Department.DEPARTMENT_NAME.ToUpper());
                }

                objectTag.AddObjectData(store, "Report", ListSereServReport);
                objectTag.AddObjectData(store, "Parent", ListRdo);
                objectTag.AddRelationship(store, "Parent", "Report", "ID", "TDL_TREATMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
