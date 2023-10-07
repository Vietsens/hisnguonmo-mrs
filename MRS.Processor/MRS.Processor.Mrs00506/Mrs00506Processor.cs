using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisPatientTypeAlter;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisHeinServiceType;

namespace MRS.Processor.Mrs00506
{
    class Mrs00506Processor : AbstractProcessor
    {
        Mrs00506Filter castFilter = null;

        public Mrs00506Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<Mrs00506RDO> listRdoDetail = new List<Mrs00506RDO>();
        List<Mrs00506RDO> listRdo = new List<Mrs00506RDO>();
        List<Mrs00506RDO> listRdoParent = new List<Mrs00506RDO>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_HEIN_SERVICE_TYPE> listHisHeinServiceType = new List<HIS_HEIN_SERVICE_TYPE>();
        List<Mrs00506RDO> listSereServs = new List<Mrs00506RDO>();

        HIS_BRANCH branch = new HIS_BRANCH();

        public override Type FilterType()
        {
            return typeof(Mrs00506Filter);
        }

        protected override bool GetData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00506Filter)this.reportFilter;

                listHisService = new HisServiceManager(paramGet).Get(new HisServiceFilterQuery());
                listHisHeinServiceType = new HisHeinServiceTypeManager(paramGet).Get(new HisHeinServiceTypeFilterQuery());

                // HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                // departmentFilter.BRANCH_ID = this.castFilter.BRANCH_ID; 
                // listDepartments = new HisDepartmentManager(paramGet).Get(departmentFilter); 

                // var listRequestDepartmentId = listDepartments.Select(s => s.ID).ToList(); 
                // if (IsNotNullOrEmpty(listRequestDepartmentId))
                // {
                //     foreach(var departmentId in listRequestDepartmentId)
                //     {
                //         HisSereServFilterQuery serServFilter = new HisSereServFilterQuery(); 
                //         serServFilter.CREATE_TIME_FROM = this.castFilter.TIME_FROM;
                //         serServFilter.CREATE_TIME_TO = this.castFilter.TIME_TO;
                //         serServFilter.HAS_EXECUTE = true;
                //         serServFilter.IS_EXPEND = false;
                //         serServFilter.TDL_REQUEST_DEPARTMENT_ID = departmentId; 
                //         listSereServs.AddRange(new HisSereServManager(paramGet).Get(serServFilter)??new List<HIS_SERE_SERV>()); 
                //     }
                branch = new HisBranchManager(paramGet).GetById(this.castFilter.BRANCH_ID ?? 0) ?? new HIS_BRANCH();
                // }
                //var listTreatmentId = listSereServs.Select(o => o.TDL_TREATMENT_ID??0).Distinct().ToList(); 
                // List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>(); 
                // if (IsNotNullOrEmpty(listTreatmentId))
                // {
                //     var skip = 0; 
                //     while (listTreatmentId.Count - skip> 0)
                //     {
                //         var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //         skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //         var treatmentFilter = new HisTreatmentFilterQuery
                //         {
                //             IDs = limit,
                //         }; 
                //         var listHisTreatmentSub = new HisTreatmentManager(paramGet).Get(treatmentFilter); 
                //         listHisTreatment.AddRange(listHisTreatmentSub ?? new List<HIS_TREATMENT>()); 
                //     }
                // }
                listSereServs = new ManagerSql().GetSereServDO(castFilter);
                if (IsNotNullOrEmpty(listSereServs))
                {
                    if (castFilter.IS_TREAT == true)
                    {
                        listSereServs = listSereServs.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH || o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList();
                    }
                    else if (castFilter.IS_TREAT == false)
                    {
                        listSereServs = listSereServs.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || o.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();
                    };

                }
                Inventec.Common.Logging.LogSystem.Info("3:" + listSereServs.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {
                listRdo.Clear();

                if (IsNotNullOrEmpty(listSereServs))
                {
                    List<long> sereServIdThrow = listSereServs.Where(o => o.PACKAGE_ID != null).Select(p => p.SERE_SERV_ID).ToList();

                    foreach (var item in listSereServs)
                    {
                        Mrs00506RDO rdo = new Mrs00506RDO();
                        if (item.IS_OUT_PARENT_FEE == null && item.PARENT_ID != null && sereServIdThrow.Contains(item.PARENT_ID ?? 0))
                        {
                            continue;
                        }
                        ;
                        rdo.SERVICE_TYPE_ID = item.SERVICE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = item.SERVICE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME;
                        rdo.SERE_SERV_ID = item.SERE_SERV_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                        rdo.SERVICE_CODE = item.SERVICE_CODE;
                        rdo.SERVICE_NAME = item.SERVICE_NAME;
                        rdo.AMOUNT = item.AMOUNT;
                        if (item.PACKAGE_ID != null)
                        {
                            var servicePatyPrpo = listHisService.FirstOrDefault
                                (o => o.ID == item.SERVICE_ID
                                    && o.PACKAGE_ID == item.PACKAGE_ID) ?? new HIS_SERVICE();
                            rdo.PRICE = servicePatyPrpo.PACKAGE_PRICE ?? 0;
                        }
                        else
                            rdo.PRICE = item.VIR_PRICE ?? 0;
                        rdo.VIR_TOTAL_PRICE = item.VIR_TOTAL_PRICE ?? 0;
                        var heinServiceType = listHisHeinServiceType.FirstOrDefault(o => o.ID == item.TDL_HEIN_SERVICE_TYPE_ID) ?? new HIS_HEIN_SERVICE_TYPE();
                        if (heinServiceType != null)
                        {
                            rdo.HEIN_SERVICE_TYPE_NUM_ORDER = heinServiceType.NUM_ORDER ?? 100;
                        }

                        listRdoDetail.Add(rdo);
                    }
                    //Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdoDetail.Count);
                    groupByService();
                    //Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                    listRdo = listRdo.OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).Where(o => o.VIR_TOTAL_PRICE > 0).ToList();
                    listRdoParent = listRdo.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).OrderBy(q => q.HEIN_SERVICE_TYPE_NUM_ORDER).ToList();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }
        private void groupByService()
        {
            string errorField = "";
            try
            {
                var group = listRdoDetail.GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList();
                listRdo.Clear();
                decimal sum = 0;
                Mrs00506RDO rdo;
                List<Mrs00506RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00506RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00506RDO();
                    listSub = item.ToList<Mrs00506RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("AMOUNT") || field.Name.Contains("TOTAL"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) listRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private Mrs00506RDO IsMeaningful(List<Mrs00506RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00506RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

            dicSingleTag.Add("BRANCH_NAME", branch.BRANCH_NAME);
            objectTag.AddObjectData(store, "ReportDetail", listRdoDetail);
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Parent", listRdoParent);
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");

        }


    }

}
