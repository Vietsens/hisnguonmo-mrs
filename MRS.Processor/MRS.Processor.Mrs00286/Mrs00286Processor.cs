using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using ACS.Filter;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00286
{
    class Mrs00286Processor : AbstractProcessor
    {
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();

        CommonParam paramGet = new CommonParam();
        List<Mrs00286RDO> ListRdo = new List<Mrs00286RDO>();
        public Mrs00286Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00286Filter);
        }


        protected override bool GetData()
        {
            var filter = ((Mrs00286Filter)reportFilter);
            bool result = true;
            try
            {
                // HSDT vao vien
                HisTreatmentViewFilterQuery filterTreatment = new HisTreatmentViewFilterQuery();
                filterTreatment.IN_TIME_FROM = filter.TIME_FROM;
                filterTreatment.IN_TIME_TO = filter.TIME_TO;
                listTreatment = new HisTreatmentManager(paramGet).GetView(filterTreatment);
                var listTreatmentId = listTreatment.Select(o => o.ID).ToList();

                // yeu cau
                List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceReqViewFilterQuery filterSs = new HisServiceReqViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            SERVICE_REQ_TYPE_ID = filter.SERVICE_REQ_TYPE_ID,

                        };
                        var serviceReqSub = new HisServiceReqManager(paramGet).GetView(filterSs);
                        listServiceReq.AddRange(serviceReqSub);
                    }

                    // loc YC - DV theo BS chi dinh
                    if (filter.LOGINNAME != null)
                    {
                        listServiceReq = listServiceReq.Where(o => o.REQUEST_LOGINNAME == filter.LOGINNAME).ToList();

                    }
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(o => o.Key, o => o.First());
                //Dich vu

                if (IsNotNullOrEmpty(dicServiceReq.Keys))
                {
                    int skip = 0;
                    while (dicServiceReq.Keys.Count - skip > 0)
                    {
                        var limit = dicServiceReq.Keys.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.SERVICE_REQ_IDs = limit;
                        ssFilter.ORDER_FIELD = "TREATMENT_ID";
                        ssFilter.ORDER_DIRECTION = "ASC";

                        var listSereServSub = new HisSereServManager(paramGet).Get(ssFilter);
                        listSereServ.AddRange(listSereServSub);
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


        protected override bool ProcessData()
        {
            var result = true;
            try
            {

                ListRdo.Clear();

                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var sese in listSereServ)
                    {
                        if (!dicServiceReq.ContainsKey(sese.SERVICE_REQ_ID ?? 0)) continue;
                        var treatment = listTreatment.FirstOrDefault(o => o.ID == sese.TDL_TREATMENT_ID)??new V_HIS_TREATMENT();
                        var req = dicServiceReq[sese.SERVICE_REQ_ID ?? 0];
                        Mrs00286RDO rdo = new Mrs00286RDO();
                        rdo.DOB = req.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.ICD_NAME = req.ICD_NAME;
                        rdo.ICD_TEXT = req.ICD_TEXT;
                        rdo.INSTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(req.INTRUCTION_TIME);
                        rdo.PATIENT_CODE = req.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = req.TDL_PATIENT_NAME;
                        rdo.REQ_USERNAME = req.REQUEST_USERNAME;
                        rdo.IN_CODE = treatment.IN_CODE;
                        rdo.SERVICE_REQ_CODE = req.SERVICE_REQ_CODE;
                        rdo.BARCODE = req.BARCODE;
                        rdo.REQUEST_ROOM_NAME = req.REQUEST_ROOM_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = req.REQUEST_DEPARTMENT_NAME;
                        rdo.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
                        rdo.HEIN_SERVICE_CODE = sese.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_NAME = sese.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.SERVICE_CODE = sese.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = sese.TDL_SERVICE_NAME;
                        rdo.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o=>o.ID==sese.TDL_SERVICE_UNIT_ID)??new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                        rdo.AMOUNT = sese.AMOUNT;
                        rdo.PRICE = sese.VIR_PRICE??0;
                        rdo.TOTAL_PRICE = sese.VIR_TOTAL_PRICE??0;

                        ListRdo.Add(rdo);

                    }
                }

                result = true;
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


            if (((Mrs00286Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00286Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00286Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00286Filter)reportFilter).TIME_TO));
            }

            objectTag.AddObjectData(store, "Report", ListRdo);
        }


    }

}
