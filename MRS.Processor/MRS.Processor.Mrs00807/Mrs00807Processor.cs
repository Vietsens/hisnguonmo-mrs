
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00807
{
    public class Mrs00807Processor : AbstractProcessor
    {
        List<Mrs00807RDO> ListRdo = new List<Mrs00807RDO>();
        List<Mrs00807RDOGroup> ListRdoGroup = new List<Mrs00807RDOGroup>();
        List<HIS_PATIENT> ListPatient = new List<HIS_PATIENT>();
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        Mrs00807Filter Filter;
        public Mrs00807Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00807Filter);
        }
        List<long> SERVICE_TYPE_IDs = new List<long>(){
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN
        
        };
        protected override bool GetData()
        {
            bool result = false;
            Filter = (Mrs00807Filter)this.reportFilter;
            try
            {
                listSereServ = new ManagerSql().GetSereServDO(Filter);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex.Message);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                foreach (var item in listSereServ)
                {
                    Mrs00807RDO rdo = new Mrs00807RDO();
                    rdo.DOCTOR_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                    rdo.DOCTOR_USERNAME = item.TDL_REQUEST_USERNAME;
                    rdo.SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                    rdo.AMOUNT = item.AMOUNT;
                    rdo.PRICE = item.PRICE;
                    rdo.SERVICE_ID = item.SERVICE_ID;
                    rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                    rdo.TREATMENT_ID = item.TDL_TREATMENT_ID ?? 0;
                    ListRdo.Add(rdo);
                }
                var group = ListRdo.GroupBy(x => new { x.DOCTOR_LOGINNAME, x.DOCTOR_USERNAME }).ToList();
                int num = ListRdo.Where(p => p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).GroupBy(x => x.DOCTOR_LOGINNAME).Count();
                foreach (var item in group)
                {
                    Mrs00807RDOGroup rdoGroup = new Mrs00807RDOGroup();
                    rdoGroup.DOCTOR_LOGINNAME = item.First().DOCTOR_LOGINNAME;
                    rdoGroup.DOCTOR_USERNAME = item.First().DOCTOR_USERNAME;
                    var check = item.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Count();
                    var CDHA = item.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(x => x.TOTAL_PRICE);
                    var XN = item.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(x => x.TOTAL_PRICE);
                    var countTreatmentCDHA = item.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Select(x => x.TREATMENT_ID).Distinct().Count();
                    var countTreatmentXN = item.Where(x => x.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Select(x => x.TREATMENT_ID).Distinct().Count();
                    rdoGroup.AMOUNT_KH = check;
                    rdoGroup.TOTAL_SERVICE = check;
                    rdoGroup.TOTAL_PRICE_CDHA = CDHA;
                    rdoGroup.TOTAL_PRICE_XN = XN;
                    if (countTreatmentCDHA == 0)
                    {
                        rdoGroup.TOTAL_PRICE_CDHA_TB = 0;
                    }
                    else
                    {
                        rdoGroup.TOTAL_PRICE_CDHA_TB = Math.Round(CDHA / countTreatmentCDHA);
                    }
                    if (countTreatmentXN == 0)
                    {
                        rdoGroup.TOTAL_PRICE_XN_TB = 0;
                    }
                    else
                    {
                        rdoGroup.TOTAL_PRICE_XN_TB = Math.Round(XN / countTreatmentXN);
                    }
                    rdoGroup.TOTAL_PRICE_ALL = Math.Round(item.Sum(x => x.TOTAL_PRICE));
                    if (rdoGroup.AMOUNT_KH>0)
                    {
                        ListRdoGroup.Add(rdoGroup);
                    }
                   
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex.Message);
                throw;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(Filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(Filter.TIME_TO));
            if (Filter.REQUEST_LOGINNAME_DOCTORs != null)
            {
                ListRdoGroup = ListRdoGroup.Where(x => Filter.REQUEST_LOGINNAME_DOCTORs.Contains(x.DOCTOR_LOGINNAME)).ToList();
            }
            objectTag.AddObjectData(store, "Report", ListRdoGroup);
        }
    }
}
