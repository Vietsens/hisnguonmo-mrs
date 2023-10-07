using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00618
{
    public class Mrs00618Processor : AbstractProcessor
    {
        Mrs00618Filter castFilter = null;
        private List<Mrs00618RDO> ListRdo = new List<Mrs00618RDO>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        //List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();

        private decimal AMOUNT { get; set; }
        private long DAY { get; set; }
        private decimal VIR_TOTAL_PRICE { get; set; }
        private decimal BED { get; set; }
        private decimal BED_AMOUNT { get; set; }
        private decimal MEDI { get; set; }
        private decimal TEST { get; set; }
        private decimal CDHA { get; set; }
        private decimal SERV { get; set; }
        private decimal TOTAL_PATIENT_PRICE { get; set; }
        private decimal TOTAL_HEIN_PRICE { get; set; }

        CommonParam paramGet = new CommonParam();
        public Mrs00618Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }
        List<long> SERVICE_TYPE_IDs = new List<long>()
        { 
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
        };
        public override Type FilterType()
        {
            return typeof(Mrs00618Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00618Filter)reportFilter);
            var result = true;
            try
            {
                //YC - DV
                HisTreatmentFilterQuery filterTreatment = new HisTreatmentFilterQuery();
                if (castFilter.INPUT_DATA_ID_TIME_TYPE == 2)//loại thời gian ra viện
                {
                    filterTreatment.OUT_TIME_FROM = castFilter.TIME_FROM;
                    filterTreatment.OUT_TIME_TO = castFilter.TIME_TO;
                    filterTreatment.IS_PAUSE = true;
                }
                else if (castFilter.INPUT_DATA_ID_TIME_TYPE == 3)//loại thời gian vào viện
                {
                    filterTreatment.IN_TIME_FROM = castFilter.TIME_FROM;
                    filterTreatment.IN_TIME_TO = castFilter.TIME_TO;
                }
                else // loại thời gian khóa viện phí
                {
                    filterTreatment.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                    filterTreatment.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    filterTreatment.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                }
                ListTreatment = new HisTreatmentManager(paramGet).Get(filterTreatment);

                if (IsNotNullOrEmpty(ListTreatment))
                {

                    if (this.castFilter.IS_TREAT != null)
                    {
                        if (this.castFilter.IS_TREAT == true)
                        {
                            ListTreatment = ListTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                        }
                        else
                        {
                            ListTreatment = ListTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                        }
                    }


                    if (this.castFilter.TREATMENT_TYPE_IDs != null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                    }

                    if (this.castFilter.BRANCH_IDs != null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.BRANCH_IDs.Contains(o.BRANCH_ID )).ToList();
                    }

                    if (this.castFilter.BRANCH_ID != null)
                    {
                        ListTreatment = ListTreatment.Where(o =>o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                    }

                    if (this.castFilter.LOGINNAME_DOCTORs != null)
                    {
                        ListTreatment = ListTreatment.Where(o => castFilter.LOGINNAME_DOCTORs.Contains(o.DOCTOR_LOGINNAME )).ToList();
                    }
                    var listTreatmentId = ListTreatment.Select(p => p.ID).ToList();
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery();
                        filterSereServ.TREATMENT_IDs = listIDs;
                        filterSereServ.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSereServ);
                        if (IsNotNullOrEmpty(listSereServSub))
                            ListSereServ.AddRange(listSereServSub);
                        //HisPatientTypeAlterFilterQuery filterPatientTypeAlter = new HisPatientTypeAlterFilterQuery();
                        //filterPatientTypeAlter.TREATMENT_IDs = listIDs;
                        //var listPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).Get(filterPatientTypeAlter);
                        //if (IsNotNullOrEmpty(listPatientTypeAlterSub))
                        //    ListPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                List<Mrs00618RDO> listRdo = new List<Mrs00618RDO>();
                if (ListSereServ != null)
                {
                    foreach (var item in ListTreatment)
                    {
                        //var patientTypeAlter = ListPatientTypeAlter.FirstOrDefault(o => item.ID == o.TREATMENT_ID) ?? new HIS_PATIENT_TYPE_ALTER();
                        var sereServSub = ListSereServ.Where(o => item.ID == o.TDL_TREATMENT_ID).ToList();
                        Mrs00618RDO rdo = new Mrs00618RDO(item, sereServSub, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.branch_id) ?? new HIS_BRANCH());
                        listRdo.Add(rdo);
                    }
                    var groupByDepartentRouteType = listRdo.GroupBy(o => new { o.DEPARTMENT_CODE, o.ROUTE_TYPE_CODE }).ToList();
                    foreach (var group in groupByDepartentRouteType)
                    {
                        Mrs00618RDO rdo = new Mrs00618RDO();
                        List<Mrs00618RDO> listSub = group.ToList<Mrs00618RDO>();
                        rdo.ROUTE_TYPE_CODE = listSub.First().ROUTE_TYPE_CODE;
                        rdo.ROUTE_TYPE_NAME = listSub.First().ROUTE_TYPE_NAME;
                        rdo.DEPARTMENT_CODE = listSub.First().DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                        foreach (var item in listSub)
                        {
                            rdo.AMOUNT += item.AMOUNT;
                            rdo.DAY += item.DAY;
                            rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                            rdo.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                            rdo.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                            rdo.BED += item.BED;
                            rdo.BED_AMOUNT += item.BED_AMOUNT;
                            rdo.MEDI += item.MEDI;
                            rdo.TEST += item.TEST;
                            rdo.CDHA += item.CDHA;
                            rdo.SERV += item.SERV;
                            rdo.TOTAL_PATIENT_PRICE_BHYT += item.TOTAL_PATIENT_PRICE_BHYT;
                            rdo.TOTAL_PATIENT_PRICE_SELF += item.TOTAL_PATIENT_PRICE_SELF;
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00618Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00618Filter)this.reportFilter).TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.ROUTE_TYPE_CODE).ToList());
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.DEPARTMENT_CODE).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "DEPARTMENT_CODE", "DEPARTMENT_CODE");

            decimal TOTAL_PATIENT_PRICE_BHYT = 0;
            decimal TOTAL_PATIENT_PRICE_SELF = 0;
            foreach (var item in ListRdo)
            {
                this.AMOUNT += item.AMOUNT;
                this.DAY += item.DAY;
                this.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                this.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                this.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                this.BED += item.BED;
                this.BED_AMOUNT += item.BED_AMOUNT;
                this.MEDI += item.MEDI;
                this.TEST += item.TEST;
                this.CDHA += item.CDHA;
                this.SERV += item.SERV;
                TOTAL_PATIENT_PRICE_BHYT += item.TOTAL_PATIENT_PRICE_BHYT;
                TOTAL_PATIENT_PRICE_SELF += item.TOTAL_PATIENT_PRICE_SELF;
            }

            dicSingleTag.Add("AMOUNT", this.AMOUNT);
            dicSingleTag.Add("DAY", this.DAY);
            dicSingleTag.Add("VIR_TOTAL_PRICE", this.VIR_TOTAL_PRICE);
            dicSingleTag.Add("TOTAL_HEIN_PRICE", this.TOTAL_HEIN_PRICE);
            dicSingleTag.Add("TOTAL_PATIENT_PRICE", this.TOTAL_PATIENT_PRICE);
            dicSingleTag.Add("BED", this.BED);
            dicSingleTag.Add("BED_AMOUNT", this.BED_AMOUNT);
            dicSingleTag.Add("MEDI", this.MEDI);
            dicSingleTag.Add("TEST", this.TEST);
            dicSingleTag.Add("CDHA", this.CDHA);
            dicSingleTag.Add("SERV", this.SERV);
            dicSingleTag.Add("TOTAL_PATIENT_PRICE_BHYT", TOTAL_PATIENT_PRICE_BHYT);
            dicSingleTag.Add("TOTAL_PATIENT_PRICE_SELF", TOTAL_PATIENT_PRICE_SELF);
        }

    }
}
