using MOS.MANAGER.HisService;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00046
{
    public class Mrs00046Processor : AbstractProcessor
    {
        Mrs00046Filter castFilter = null;
        List<Mrs00046RDO> ListRdoExam = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoTest = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoDiim = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoEndo = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoSuim = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoFuex = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoSurg = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoMisu = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoMedicine = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoMaterial = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoBed = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdoOther = new List<Mrs00046RDO>();
        List<Mrs00046RDO> ListRdo = new List<Mrs00046RDO>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();

        CommonParam paramGet = new CommonParam();
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV> listAttach = new List<V_HIS_SERE_SERV>();
        public Mrs00046Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00046Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00046Filter)this.reportFilter);
                GetServiceReq();
                var listServiceReqId = dicServiceReq.Keys.Distinct().ToList();
                if (IsNotNullOrEmpty(listServiceReqId))
                {
                    ListSereServ = GetSereServ(listServiceReqId);
                    if(ListSereServ !=null && ListSereServ.Count>0)
                    {
                        listAttach = GetSereServAttach(ListSereServ.Select(o=>o.ID).ToList());
                    }    
                    
                } 
                
                result = true;
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
                ProcessListSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListSereServ()
        {
            try
            {
                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    CommonParam paramGet = new CommonParam();

                    if (ListSereServ != null && ListSereServ.Count > 0)
                    {
                        ProcessListSereServ(ListSereServ);
                        var listExam = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listExam != null && listExam.Count > 0)
                        {
                            ProcessListSubSereServ(listExam, "EXAM");
                        }

                        var listTest = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listTest != null && listTest.Count > 0)
                        {
                            ProcessListSubSereServ(listTest, "TEST");
                        }

                        var listDiim = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listDiim != null && listDiim.Count > 0)
                        {
                            ProcessListSubSereServ(listDiim, "DIIM");
                        }

                        var listEndo = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listEndo != null && listEndo.Count > 0)
                        {
                            ProcessListSubSereServ(listEndo, "ENDO");
                        }

                        var listSuim = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listSuim != null && listSuim.Count > 0)
                        {
                            ProcessListSubSereServ(listSuim, "SUIM");
                        }

                        var listFuex = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listFuex != null && listFuex.Count > 0)
                        {
                            ProcessListSubSereServ(listFuex, "FUEX");
                        }

                        var listSurg = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listSurg != null && listSurg.Count > 0)
                        {
                            ProcessListSubSereServ(listSurg, "SURG");
                        }

                        var listMisu = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT && req(o).SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
                        if (listMisu != null && listMisu.Count > 0)
                        {
                            ProcessListSubSereServ(listMisu, "MISU");
                        }

                        var listMedi = ListSereServ.Where(o => (req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK || req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT || req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT) && o.AMOUNT > 0 && o.MEDICINE_ID.HasValue).ToList();
                        if (listMedi != null && listMedi.Count > 0)
                        {
                            ProcessListSubSereServMediMate(paramGet, listMedi, "MEDI");
                        }

                        var listMate = ListSereServ.Where(o => (req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK || req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT || req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT) && o.AMOUNT > 0 && o.MATERIAL_ID.HasValue).ToList();
                        if (listMate != null && listMate.Count > 0)
                        {
                            ProcessListSubSereServMediMate(paramGet, listMate, "MATE");
                        }

                        var listBed = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__G).ToList();
                        if (listBed != null && listBed.Count > 0)
                        {
                            ProcessListSubSereServ(listBed, "BED");
                        }

                        var listOther = ListSereServ.Where(o => req(o).SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KHAC).ToList();
                        if (listOther != null && listOther.Count > 0)
                        {
                            ProcessListSubSereServ(listOther, "OTHER");
                        }
                    }
                    ListRdoMedicine = ListRdoMedicine.Where(o => o.AMOUNT > 0).ToList();
                    ListRdoMaterial = ListRdoMaterial.Where(o => o.AMOUNT > 0).ToList();
                    if (param.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoExam.Clear();
                ListRdoTest.Clear();
                ListRdoDiim.Clear();
                ListRdoEndo.Clear();
                ListRdoSuim.Clear();
                ListRdoFuex.Clear();
                ListRdoSurg.Clear();
                ListRdoMisu.Clear();
                ListRdoMedicine.Clear();
                ListRdoMaterial.Clear();
                ListRdoBed.Clear();
                ListRdoOther.Clear();
            }
        }

        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ();
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new HIS_SERVICE_REQ();
            }
            return result;
        }

        private void ProcessListSereServ(List<V_HIS_SERE_SERV> listSereServ)
        {
            try
            {
                var Groups = listSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                
                foreach (var group in Groups)
                {
                    List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00046RDO rdo = new Mrs00046RDO();
                        rdo.SERVICE_ID = listSub[0].SERVICE_ID;
                        rdo.SERVICE_CODE = listSub[0].TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = listSub[0].TDL_SERVICE_NAME;
                        rdo.PRICE = listSub[0].PRICE;
                        rdo.SERVICE_TYPE_CODE = listSub[0].SERVICE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = listSub[0].SERVICE_TYPE_NAME;
                        //if (listSub[0].SERVICE_ID != null)
                        //{
                        //    listAttach = listAttach.Where(p => p.PARENT_ID == listSub[0].SERVICE_ID && p.IS_EXPEND == 1).ToList();
                        //    if (listAttach != null)
                        //    {
                        //        rdo.EXPEND_PRICE = listAttach.Sum(p => p.PRICE);
                        //    }
                        //}
                        foreach (var sereServ in listSub)
                        {
                            rdo.AMOUNT += sereServ.AMOUNT;
                             var listAttachSub = listAttach.Where(p => p.PARENT_ID == sereServ.ID && p.IS_EXPEND == 1).ToList();
                            if (listAttachSub != null)
                            {
                                rdo.EXPEND_PRICE += listAttachSub.Sum(p => (p.VIR_PRICE_NO_EXPEND??0)*p.AMOUNT);
                            }
                        }
                        if(rdo.AMOUNT>0)
                        {
                            rdo.EXPEND_PRICE = Math.Round(rdo.EXPEND_PRICE / rdo.AMOUNT, 0);
                        }    
                        else
                        {
                            rdo.EXPEND_PRICE = 0;
                        }    
                        
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoExam.Clear();
                ListRdoTest.Clear();
                ListRdoDiim.Clear();
                ListRdoEndo.Clear();
                ListRdoSuim.Clear();
                ListRdoFuex.Clear();
                ListRdoSurg.Clear();
                ListRdoMisu.Clear();
                ListRdoBed.Clear();
                ListRdoOther.Clear();
            }
        }

        private void ProcessListSubSereServ(List<V_HIS_SERE_SERV> listSereServ, string Code)
        {
            try
            {
                var Groups = listSereServ.GroupBy(g => g.SERVICE_ID).ToList();
                foreach (var group in Groups)
                {
                    List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00046RDO rdo = new Mrs00046RDO();
                        
                        rdo.SERVICE_CODE = listSub[0].TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = listSub[0].TDL_SERVICE_NAME;
                        
                        foreach (var sereServ in listSub)
                        {
                            
                            rdo.AMOUNT += sereServ.AMOUNT;
                            
                        }
                        

                        switch (Code)
                        {
                            case "EXAM": ListRdoExam.Add(rdo); break;
                            case "TEST": ListRdoTest.Add(rdo); break;
                            case "DIIM": ListRdoDiim.Add(rdo); break;
                            case "ENDO": ListRdoEndo.Add(rdo); break;
                            case "SUIM": ListRdoSuim.Add(rdo); break;
                            case "FUEX": ListRdoFuex.Add(rdo); break;
                            case "SURG": ListRdoSurg.Add(rdo); break;
                            case "MISU": ListRdoMisu.Add(rdo); break;
                            case "BED": ListRdoBed.Add(rdo); break;
                            case "OTHER": ListRdoOther.Add(rdo); break;
                            default:
                                return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoExam.Clear();
                ListRdoTest.Clear();
                ListRdoDiim.Clear();
                ListRdoEndo.Clear();
                ListRdoSuim.Clear();
                ListRdoFuex.Clear();
                ListRdoSurg.Clear();
                ListRdoMisu.Clear();
                ListRdoBed.Clear();
                ListRdoOther.Clear();
            }
        }

        private void ProcessListSubSereServMediMate(CommonParam param, List<V_HIS_SERE_SERV> listSereServ, string Code)
        {
            try
            {
                var Groups = listSereServ.Where(o => o.AMOUNT > 0).GroupBy(g => g.SERVICE_ID).ToList();
                foreach (var group in Groups)
                {
                    List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00046RDO rdo = new Mrs00046RDO();
                        rdo.SERVICE_CODE = listSub[0].TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = listSub[0].TDL_SERVICE_NAME;
                        foreach (var sereServ in listSub)
                        {
                            HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                            filter.SERVICE_REQ_ID = sereServ.SERVICE_REQ_ID;
                            filter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                            var expMests = new HisExpMestManager(param).Get(filter);
                            if (expMests != null && expMests.Count > 0)
                            {
                                rdo.AMOUNT += sereServ.AMOUNT;
                            }
                        }
                        if (Code == "MEDI")
                        {
                            ListRdoMedicine.Add(rdo);
                        }
                        else if (Code == "MATE")
                        {
                            ListRdoMaterial.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoMaterial.Clear();
                ListRdoMedicine.Clear();
            }
        }

        private List<V_HIS_SERE_SERV> GetSereServ(List<long> listServiceReqId)
        {
            List<V_HIS_SERE_SERV> result = new List<V_HIS_SERE_SERV>();
            try
            {
                var skip = 0;
                while (listServiceReqId.Count - skip > 0)
                {
                    var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery();
                    filterSereServ.SERVICE_REQ_IDs = listIDs;
                    if (castFilter.SERVICE_TYPE_IDs != null)
                    {
                        filterSereServ.SERVICE_TYPE_IDs = castFilter.SERVICE_TYPE_IDs;
                    }
                    if (castFilter.SERVICE_IDs != null)
                    {
                        filterSereServ.SERVICE_IDs = castFilter.SERVICE_IDs;
                    }
                    var listSereServSub = new HisSereServManager(paramGet).GetView(filterSereServ);
                    if (IsNotNullOrEmpty(listSereServSub))
                        result.AddRange(listSereServSub);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<V_HIS_SERE_SERV>();
            }
            return result;
        }

        private List<V_HIS_SERE_SERV> GetSereServAttach(List<long> listSereServId)
        {
            List<V_HIS_SERE_SERV> result = new List<V_HIS_SERE_SERV>();
            try
            {
                var skip = 0;
                while (listSereServId.Count - skip > 0)
                {
                    var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery();
                    filterSereServ.PARENT_IDs = listIDs;
                    filterSereServ.SERVICE_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC,IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT};

                    var listSereServSub = new HisSereServManager(paramGet).GetView(filterSereServ);
                    if (IsNotNullOrEmpty(listSereServSub))
                        result.AddRange(listSereServSub);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<V_HIS_SERE_SERV>();
            }
            return result;
        }

        private void GetServiceReq()
        {
            try
            {
                HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                srFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                srFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                
                if (castFilter.DEPARTMENT_ID != null)
                    srFilter.EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                var resultServiceReq = new HisServiceReqManager().Get(srFilter);
                dicServiceReq = IsNotNullOrEmpty(resultServiceReq) ? resultServiceReq.ToDictionary(o => o.ID) : new Dictionary<long, HIS_SERVICE_REQ>();
                
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
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                objectTag.AddObjectData(store, "Report", ListRdo);
               
                objectTag.AddObjectData(store, "ReportTest", ListRdoTest);
                objectTag.AddObjectData(store, "ReportDiim", ListRdoDiim);
                objectTag.AddObjectData(store, "ReportEndo", ListRdoEndo);
                objectTag.AddObjectData(store, "ReportSuim", ListRdoSuim);
                objectTag.AddObjectData(store, "ReportFuex", ListRdoFuex);
                objectTag.AddObjectData(store, "ReportSurg", ListRdoSurg);
                objectTag.AddObjectData(store, "ReportMisu", ListRdoMisu);
                objectTag.AddObjectData(store, "ReportMedicine", ListRdoMedicine);
                objectTag.AddObjectData(store, "ReportMaterial", ListRdoMaterial);
                objectTag.AddObjectData(store, "ReportBed", ListRdoBed);
                objectTag.AddObjectData(store, "ReportOther", ListRdoOther);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
