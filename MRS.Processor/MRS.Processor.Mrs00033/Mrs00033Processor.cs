using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00033
{
    public class Mrs00033Processor : AbstractProcessor
    {
        Mrs00033Filter castFilter = null;
        List<Mrs00033RDO> ListRdo = new List<Mrs00033RDO>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, HIS_SERE_SERV> dicRdoService = new Dictionary<long, HIS_SERE_SERV>();
        CommonParam paramGet = new CommonParam();
        List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ> ListCurrentServiceReq = new List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();

        private const string EXAM = "EXAM";
        private const string TEST = "TEST";
        private const string DIIM = "DIIM";
        private const string MISU = "MISU";
        private const string FUEX = "FUEX";
        private const string SURG = "SURG";
        private const string SUIM = "SUIM";
        private const string ENDO = "ENDO";

        public Mrs00033Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00033Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00033Filter)this.reportFilter);
                LoadDataToRam();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void LoadDataToRam()
        {
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                ListCurrentServiceReq = new HisServiceReqManager().Get(filter);
                HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                HisSereServfilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                HisSereServfilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                HisSereServfilter.TDL_EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                HisSereServfilter.TDL_EXECUTE_DEPARTMENT_IDs = castFilter.EXECUTE_DEPARTMENT_IDs;
                ListSereServ = new HisSereServManager(param).Get(HisSereServfilter);
                ListCurrentServiceReq = ListCurrentServiceReq.Where(o => ListSereServ.Exists(p => p.SERVICE_REQ_ID == o.ID)).ToList();
                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    //DV - thanh toan
                    var listTreatmentId = ListSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).ToList();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                            filterSereServBill.TDL_TREATMENT_IDs = listIDs;
                            var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                            if (IsNotNullOrEmpty(listSereServBillSub))
                                ListSereServBill.AddRange(listSereServBillSub);
                        }
                    }
                    ListSereServBill = ListSereServBill.Where(o => ListSereServ.Exists(p => p.ID == o.SERE_SERV_ID)).ToList();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentServiceReq.Clear();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListCurrentServiceReq();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentServiceReq()
        {
            try
            {
                if (ListCurrentServiceReq != null && ListCurrentServiceReq.Count > 0)
                {
                    ListCurrentServiceReq = ListCurrentServiceReq.Where(o => castFilter.HAS_NOT_DONE==true||(o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && o.FINISH_TIME != null)).ToList();
                    if (ListCurrentServiceReq.Count > 0)
                    {
                        //CommonParam param = new CommonParam(); 
                        var Groups = ListCurrentServiceReq.GroupBy(g => g.EXECUTE_DEPARTMENT_ID).ToList();
                        foreach (var group in Groups)
                        {
                            List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ> listDepartment = group.ToList<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();
                            if (listDepartment != null && listDepartment.Count > 0)
                            {
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listDepartment[0].EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                                Mrs00033RDO rdo = new Mrs00033RDO();
                                rdo.EXECUTE_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                rdo.EXECUTE_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                
                                ProcessListByDepartment(param, listDepartment, rdo);
                                ListRdo.Add(rdo);
                            }
                        }
                        if (param.HasException)
                        {
                            throw new DataMisalignedException("co exception xay ra tai DAOGET trong qua trinh tong hop du lieu");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListByDepartment(CommonParam param, List<HIS_SERVICE_REQ> listDeparment, Mrs00033RDO rdo)
        {
            try
            {
                var listExam = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_EXAM = listExam.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listTest = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_TEST = listTest.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listDiim = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_DIIM = listDiim.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listMisu = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_MISU = listMisu.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listFuex = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_FUEX = listExam.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listSurg = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_SURG = listSurg.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listSuim = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA && p.ID == o.SERVICE_REQ_ID)).ToList();

                rdo.AMOUNT_SUIM = listSuim.Where(o => ListSereServBill.Exists(p => p.SERE_SERV_ID == o.ID)).Sum(s => s.AMOUNT);

                var listEndo = ListSereServ.Where(o => listDeparment.Exists(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS && p.ID == o.SERVICE_REQ_ID)).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportDetail", dicRdoService.Values.OrderBy(o=>o.TDL_SERVICE_TYPE_ID).ThenBy(p=>p.TDL_SERVICE_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
