using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisService;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00112
{
    public class Mrs00112Processor : AbstractProcessor
    {
        Mrs00112Filter castFilter = null;
        List<Mrs00112RDO> ListRdo = new List<Mrs00112RDO>();
        List<Mrs00112RDO> listTreatmentService = new List<Mrs00112RDO>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();

        public Mrs00112Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00112Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                castFilter = ((Mrs00112Filter)this.reportFilter);

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listService = new HisServiceManager().Get(serviceFilter);
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, MRS00112 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                ListRdo = new ManagerSql().GetTreatment(castFilter);

               
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
                this.listService = this.listService.Where(o => ListRdo.Exists(p => p.SERVICE_ID == o.ID)).OrderBy(p=>p.SERVICE_TYPE_ID).ToList();
                var groupByTreatment = ListRdo.GroupBy(o => o.TDL_TREATMENT_ID).ToList();
                foreach (var item in groupByTreatment)
                {
                    Mrs00112RDO rdo = new Mrs00112RDO();
                    Mrs00112RDO ss = item.First();
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00112RDO>(rdo, ss);
                   
                    for (int i = 0; i < listService.Count; i++)
                    {
                        if (i > 49)
                        {
                            continue;
                        }
                        System.Reflection.PropertyInfo pi = typeof(Mrs00112RDO).GetProperty(string.Format("VIR_TOTAL_PRICE_{0}", i + 1));
                        pi.SetValue(rdo, item.ToList().Where(o => o.SERVICE_ID == listService[i].ID).Sum(s => s.VIR_TOTAL_PRICE));
                    }
                    rdo.DOB_YEAR = item.First().TDL_PATIENT_DOB > 1000 ? item.First().TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                    rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.OrderBy(o => o.TDL_INTRUCTION_TIME).First().TDL_INTRUCTION_TIME);
                    rdo.VIR_TOTAL_PRICE = item.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    rdo.AMOUNT = item.Sum(s => s.AMOUNT);

                    this.listTreatmentService.Add(rdo);
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
                if (((Mrs00112Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00112Filter)reportFilter).TIME_FROM));
                }
                if (((Mrs00112Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00112Filter)reportFilter).TIME_TO));
                }
                for (int i = 0; i < listService.Count; i++)
                {
                    if (i > 49)
                    {
                        continue;
                    }
                    dicSingleTag.Add(string.Format("SERVICE_NAME_{0}", i + 1), listService[i].SERVICE_NAME);
                }
                objectTag.AddObjectData(store, "TreatmentService", listTreatmentService);
                objectTag.AddObjectData(store, "Patients", listTreatmentService);
                objectTag.AddObjectData(store, "KskContracts", listTreatmentService.GroupBy(o=>o.KSK_CONTRACT_ID).Select(p=>p.First()).ToList());
                objectTag.AddRelationship(store, "KskContracts", "Patients", "KSK_CONTRACT_ID", "KSK_CONTRACT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
