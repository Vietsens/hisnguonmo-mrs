using MOS.MANAGER.HisSereServ;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
 

namespace MRS.Processor.Mrs00406
{
    class Mrs00406Processor : AbstractProcessor
    {
        private List<Mrs00406RDO> listMrs00406Rdos = new List<Mrs00406RDO>(); 
        Mrs00406Filter castFilter = null; 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        //List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 

        public Mrs00406Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00406Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00406Filter)this.reportFilter; 

                var sereServFillter = new HisSereServViewFilterQuery()
                {
                    INTRUCTION_DATE_FROM = castFilter.TIME_FROM,
                    INTRUCTION_DATE_TO = castFilter.TIME_TO,
                }; 
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServFillter); 

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
                var listSereServ = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM && s.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM && s.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU && s.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList(); 
                List<Mrs00406RDO> listRs = new List<Mrs00406RDO>(); 
                var listSereServGroupByServiceId = listSereServ.GroupBy(s => s.SERVICE_ID).ToList(); 
                foreach (var sereServ in listSereServGroupByServiceId)
                {
                    Mrs00406RDO rdo = new Mrs00406RDO(); 

                    rdo.HEIN_SERVICE_BHYT_CODE = sereServ.First().TDL_HEIN_SERVICE_BHYT_CODE; 
                    rdo.SERVICE_CODE = sereServ.First().TDL_SERVICE_CODE; 
                    rdo.SERVICE_BHYT_NAME = sereServ.First().TDL_HEIN_SERVICE_BHYT_NAME; 
                    rdo.AMOUNT = sereServ.Sum(s => s.AMOUNT); 
                    rdo.TOTAL_PRICE = sereServ.Sum(s => s.VIR_TOTAL_PRICE); 
                    listRs.Add(rdo); 
                }
                listRs = listRs.OrderByDescending(s => s.AMOUNT).ToList(); 
                listMrs00406Rdos = listRs.Take(10).ToList(); 

                //if (listRs != null && listRs.Count > 10)
                //{
                //    for (int i = 0;  i < 10;  i++)
                //    {
                //        listMrs00406Rdos.Add(listRs[i]); 
                //    }
                //}
                //else if (listRs != null && listRs.Count <= 10)
                //{
                //    for (int i = 0;  i < listRs.Count;  i++)
                //    {
                //        listMrs00406Rdos.Add(listRs[i]); 
                //    }
                //}
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                //dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", listSereServs.Select(s => s.REQUEST_DEPARTMENT_NAME).First()); 
                //dicSingleTag.Add("SERVICE_NAME", listServices.Select(s => s.SERVICE_NAME).First()); 

                objectTag.AddObjectData(store, "Report", listMrs00406Rdos); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Report", "Report1", "EXP_MEST_CODE", "EXP_MEST_CODE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
