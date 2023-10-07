using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00508
{
    class Mrs00508Processor : AbstractProcessor
    {
        Mrs00508Filter castFilter = null;

        public Mrs00508Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        Dictionary<long, Decimal> dicExpendService = new Dictionary<long, Decimal>();
        List<Mrs00508RDO> listRdo = new List<Mrs00508RDO>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

        public override Type FilterType()
        {
            return typeof(Mrs00508Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00508Filter)this.reportFilter;
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery()
                {
                    IN_TIME_FROM = this.castFilter.TIME_FROM,
                    IN_TIME_TO = this.castFilter.TIME_TO
                };
                listTreatment = new HisTreatmentManager().GetView(treatmentFilter);
                //Doi tuong dieu tri
                dicCurrentPatyAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatment.Select(o => o.ID).ToList()).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).GroupBy(q => q.TREATMENT_ID).ToDictionary(r => r.Key, r => r.Last());
                //Loc theo dien dieu tri da chon
                if (listTreatment != null)
                    listTreatment = listTreatment.Where(o => (!IsNotNullOrEmpty(this.castFilter.TREATMENT_TYPE_IDs)) || this.castFilter.TREATMENT_TYPE_IDs.Contains(treatmentType(o.ID))).ToList();
                HisDepartmentTranViewFilterQuery departmentTranFiler = new HisDepartmentTranViewFilterQuery()
                {
                    DEPARTMENT_IN_TIME_FROM = this.castFilter.TIME_FROM,
                    DEPARTMENT_IN_TIME_TO = this.castFilter.TIME_TO,
                };
                listDepartmentTran = new HisDepartmentTranManager().GetView(departmentTranFiler)??new List<V_HIS_DEPARTMENT_TRAN>();
                listDepartmentTran = listDepartmentTran.OrderBy(o => o.DEPARTMENT_IN_TIME).ThenBy(p => p.ID).ToList();
                if (IsNotNullOrEmpty(listTreatment))
                {
                    var treatmentIds = listTreatment.Select(o => o.ID).ToList();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery()
                        {
                            TREATMENT_IDs = limit,
                            HAS_EXECUTE = true
                        };
                        listSereServ.AddRange(new HisSereServManager().GetView(sereServFilter)??new List<V_HIS_SERE_SERV>());

                    }

                }

                //neu la kham thi lay phong chi dinh la phong thuc hien
                foreach (var item in listSereServ)
                {
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                    }
                }

                listSereServ = listSereServ.Where(o => (this.castFilter.DEPARTMENT_ID ?? 0) == 0 || o.TDL_REQUEST_DEPARTMENT_ID == this.castFilter.DEPARTMENT_ID).ToList();

                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery()
                {
                };
                var listServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);
                Dictionary<long, Decimal> dicServiceMety = listServiceMety.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, p => p.Sum(s => s.EXPEND_AMOUNT * (s.EXPEND_PRICE??0)));
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery()
                {
                };
                var listServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);
                Dictionary<long, Decimal> dicServiceMaty = listServiceMaty.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, p => p.Sum(s => s.EXPEND_AMOUNT * (s.EXPEND_PRICE ?? 0)));
                foreach (var item in dicServiceMety)
                {
                    dicExpendService.Add(item.Key, item.Value);
                }
                foreach (var item in dicServiceMaty)
                {
                    if (dicExpendService.ContainsKey(item.Key))
                    {
                        dicExpendService[item.Key] += item.Value;
                        continue;
                    }
                    dicExpendService.Add(item.Key, item.Value);
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
                listRdo.Clear();

                if (IsNotNullOrEmpty(listTreatment))
                {
                    foreach (var item in listTreatment)
                    {
                        var sereServ = listSereServ.Where(o=>o.TDL_TREATMENT_ID==item.ID).ToList();
                        Mrs00508RDO rdo = new Mrs00508RDO(item);
                        rdo.DEPARTMENT_NAME = listDepartmentTran.LastOrDefault(o => o.TREATMENT_ID == item.ID).DEPARTMENT_NAME;
                        rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                        rdo.TEIN_PRICE = sereServ.Where(o=>o.TDL_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(p=>p.VIR_TOTAL_PRICE??0);
                        rdo.EXPEND_TEIN_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(p => dicExpendService.ContainsKey(p.SERVICE_ID) ? dicExpendService[p.SERVICE_ID] : 0);
                        rdo.LEFT_TEIN_PRICE = rdo.TEIN_PRICE - rdo.EXPEND_TEIN_PRICE;
                        rdo.FUEX_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.SUIM_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.ENDO_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.DIIM_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.EXPEND_DIIM_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(p => dicExpendService.ContainsKey(p.SERVICE_ID) ? dicExpendService[p.SERVICE_ID] : 0);
                        rdo.MISU_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.SURG_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.EXAM_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.EXPEND_TICK = sereServ.Where(o => o.IS_EXPEND==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.BED_PRICE = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.TRAN_PRICE = sereServ.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(p => p.VIR_TOTAL_PRICE ?? 0);
                        rdo.VIR_TOTAL_PRICE = sereServ.Sum(p=>p.VIR_TOTAL_PRICE??0);
                        listRdo.Add(rdo);
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
        private long treatmentType(long treatmentId)
        {
            return dicCurrentPatyAlter.ContainsKey(treatmentId) ? dicCurrentPatyAlter[treatmentId].TREATMENT_TYPE_ID : 0;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.DEPARTMENT_NAME).ToList());

        }

    }

}
