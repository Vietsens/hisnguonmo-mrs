using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00453
{
    class Mrs00453Processor : AbstractProcessor
    {
        Mrs00453Filter castFilter = null;
        List<Mrs00453RDO> listRdo = new List<Mrs00453RDO>();
        List<Mrs00453RDO> Service = new List<Mrs00453RDO>();
        List<Mrs00453RDO> GroupDepartment = new List<Mrs00453RDO>();
        List<Mrs00453RDO> GroupServiceType = new List<Mrs00453RDO>();
        List<Mrs00453RDO> GroupTreatment = new List<Mrs00453RDO>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();
        List<HIS_ICD> listICDs = new List<HIS_ICD>();
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();

        public Mrs00453Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00453Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00453Filter)this.reportFilter;
                var skip = 0;

                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                sereServFilter.INTRUCTION_TIME_FROM = this.castFilter.TIME_FROM;
                sereServFilter.INTRUCTION_TIME_TO = this.castFilter.TIME_TO;
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter);

                var listTreatmentIds = listSereServs.Where(w => w.TDL_TREATMENT_ID != null).Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();
                dicServiceReq = getServiceReq(listTreatmentIds).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                    filter.TREATMENT_IDs = listIds;
                    var departmentTrans = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(filter);
                    listDepartmentTrans.AddRange(departmentTrans);
                }
                var listICDCodes = listDepartmentTrans.Where(w => !String.IsNullOrEmpty(w.ICD_CODE)).Select(s => s.ICD_CODE).Distinct().ToList();
                skip = 0;
                while (listICDCodes.Count - skip > 0)
                {
                    var listCodes = listICDCodes.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisIcdFilterQuery filter = new HisIcdFilterQuery();
                    filter.ICD_CODEs = listCodes;
                    var Icds = new MOS.MANAGER.HisIcd.HisIcdManager(paramGet).Get(filter);
                    listICDs.AddRange(Icds);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<V_HIS_SERVICE_REQ> getServiceReq(List<long> listTreatmentIds)
        {
            List<V_HIS_SERVICE_REQ> result = new List<V_HIS_SERVICE_REQ>();
            try
            {
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var limit = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var ssFilter = new HisServiceReqViewFilterQuery();
                    ssFilter.TREATMENT_IDs = limit;
                    result.AddRange(new HisServiceReqManager().GetView(ssFilter));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<V_HIS_SERVICE_REQ>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue;
                        var serviceReq = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0];
                        var departmentTrans = listDepartmentTrans.Where(w => w.DEPARTMENT_CODE == sereServ.REQUEST_DEPARTMENT_CODE && w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).OrderBy(o => o.DEPARTMENT_IN_TIME).ToList();
                        if (IsNotNullOrEmpty(departmentTrans))
                        {
                            var departmentTranIn = departmentTrans.Where(w => w.DEPARTMENT_IN_TIME <= sereServ.TDL_INTRUCTION_TIME).ToList();  // lay Last()

                            var departmentTranOut = departmentTrans.Where(w => w.DEPARTMENT_IN_TIME >= sereServ.TDL_INTRUCTION_TIME).ToList();  //lay First()

                            Mrs00453RDO rdo = new Mrs00453RDO();
                            if (sereServ.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            {
                                rdo.AMOUT_NO_EXECUTE = sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUT_EXECUTE = sereServ.AMOUNT;
                            }
                            rdo.AMOUT_INTRUCTION = sereServ.AMOUNT;
                            if (serviceReq.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                            {
                                rdo.AMOUT_RESULT = sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUT_NO_RESULT = sereServ.AMOUNT;
                            }
                            rdo.BILL_TYPE_NAME = sereServ.PATIENT_TYPE_NAME;
                            rdo.DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME;
                            rdo.DOB = serviceReq.TDL_PATIENT_DOB;
                            rdo.GENDER_NAME = serviceReq.TDL_PATIENT_GENDER_NAME;
                            if (IsNotNullOrEmpty(departmentTranIn))
                            {
                                var icd = listICDs.Where(w => w.ICD_CODE == departmentTranIn.Last().ICD_CODE).ToList();
                                rdo.IN_ICD10 = IsNotNullOrEmpty(icd) ? icd.First().ICD_CODE : "";
                                rdo.IN_TIME = departmentTranIn.Last().DEPARTMENT_IN_TIME ?? 0;
                            }
                            if (IsNotNullOrEmpty(departmentTranOut))
                            {
                                var icd = listICDs.Where(w => w.ICD_CODE == departmentTranOut.First().ICD_CODE).ToList();
                                rdo.OUT_ICD10 = IsNotNullOrEmpty(icd) ? icd.First().ICD_CODE : "";
                                rdo.OUT_TIME = departmentTranOut.First().DEPARTMENT_IN_TIME ?? 0;
                            }
                            rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                            rdo.PRICE = sereServ.PRICE;
                            rdo.SERVICE_TYPE_NAME = sereServ.SERVICE_TYPE_NAME;
                            rdo.TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE ?? 0;
                            rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;
                            if (IsNotNullOrEmpty(departmentTranIn))
                            {
                                if (IsNotNullOrEmpty(departmentTranOut))
                                {
                                    rdo.TREATMENT_TIME = DateDiff.diffDate(departmentTranIn.Last().DEPARTMENT_IN_TIME ?? 0, departmentTranOut.First().DEPARTMENT_IN_TIME ?? 0);
                                }
                                else
                                {
                                    rdo.TREATMENT_TIME = DateDiff.diffDate(departmentTranIn.Last().DEPARTMENT_IN_TIME ?? 0, this.castFilter.TIME_TO);
                                }
                            }

                            rdo.SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID;
                            rdo.SERVICE_ID = sereServ.SERVICE_ID;
                            rdo.DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID;
                            rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID.Value;
                            rdo.SERVICE_TYPE_CODE = sereServ.SERVICE_TYPE_CODE;
                            rdo.SERVICE_CODE = sereServ.TDL_SERVICE_CODE;
                            rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                            listRdo.Add(rdo);

                        }

                    }
                    if (IsNotNullOrEmpty(listRdo))
                    {
                        GroupTreatment = listRdo.GroupBy(gr => new
                        {
                            gr.SERVICE_TYPE_ID,
                            gr.DEPARTMENT_ID,
                            gr.SERVICE_ID,
                            gr.TREATMENT_ID,
                            gr.PRICE,
                        }).Select(s => new Mrs00453RDO
                        {
                            AMOUT_EXECUTE = s.Sum(su => su.AMOUT_EXECUTE),
                            AMOUT_INTRUCTION = s.Sum(su => su.AMOUT_INTRUCTION),
                            AMOUT_NO_EXECUTE = s.Sum(su => su.AMOUT_NO_EXECUTE),
                            AMOUT_NO_RESULT = s.Sum(su => su.AMOUT_NO_RESULT),
                            AMOUT_RESULT = s.Sum(su => su.AMOUT_RESULT),
                            BILL_TYPE_NAME = s.First().BILL_TYPE_NAME,
                            DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                            DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                            DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                            DOB = s.First().DOB,
                            GENDER_NAME = s.First().GENDER_NAME,
                            IN_ICD10 = s.First().IN_ICD10,
                            IN_TIME = s.First().IN_TIME,
                            OUT_ICD10 = s.First().OUT_ICD10,
                            OUT_TIME = s.First().OUT_TIME,
                            PATIENT_CODE = s.First().PATIENT_CODE,
                            PATIENT_NAME = s.First().PATIENT_NAME,
                            PRICE = s.First().PRICE,
                            SERVICE_CODE = s.First().SERVICE_CODE,
                            SERVICE_ID = s.First().SERVICE_ID,
                            SERVICE_NAME = s.First().SERVICE_NAME,
                            SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                            SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                            TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE),
                            TREATMENT_CODE = s.First().TREATMENT_CODE,
                            TREATMENT_ID = s.First().TREATMENT_ID,
                            TREATMENT_TIME = s.First().TREATMENT_TIME

                        }).ToList();

                        Service = listRdo.GroupBy(gr => new
                        {
                            gr.SERVICE_TYPE_ID,
                            gr.DEPARTMENT_ID,
                            gr.SERVICE_ID
                        }).Select(s => new Mrs00453RDO
                        {
                            SERVICE_CODE = s.First().SERVICE_CODE,
                            SERVICE_NAME = s.First().SERVICE_NAME,
                            SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                            DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                            SERVICE_ID = s.First().SERVICE_ID
                        }).ToList();

                        GroupDepartment = listRdo.GroupBy(gr => new
                        {
                            gr.SERVICE_TYPE_ID,
                            gr.DEPARTMENT_ID
                        }).Select(s => new Mrs00453RDO
                        {
                            DEPARTMENT_CODE = s.First().DEPARTMENT_CODE,
                            DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                            SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                            DEPARTMENT_ID = s.First().DEPARTMENT_ID
                        }).ToList();

                        GroupServiceType = listRdo.GroupBy(gr => gr.SERVICE_TYPE_ID).Select(s => new Mrs00453RDO
                        {
                            SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                            SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                            SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID
                        }).ToList();


                        GroupServiceType = GroupServiceType.OrderBy(s => s.SERVICE_TYPE_ID).ToList();
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", GroupTreatment);
                objectTag.AddObjectData(store, "Service", Service);
                objectTag.AddObjectData(store, "GroupDepartment", GroupDepartment);
                objectTag.AddObjectData(store, "GroupServiceType", GroupServiceType);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupServiceType", "GroupDepartment", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "GroupDepartment", "Service", new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID" }, new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID" });
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Service", "Report", new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID", "SERVICE_ID" }, new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID", "SERVICE_ID" });

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
