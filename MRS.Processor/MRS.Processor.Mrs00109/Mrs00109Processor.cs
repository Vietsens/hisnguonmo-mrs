using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00109
{
    public class Mrs00109Processor : AbstractProcessor
    {
        List<V_HIS_SERE_SERV_BILL> ListSereServBill = new List<V_HIS_SERE_SERV_BILL>(); 
        CommonParam paramGet = new CommonParam(); 
        Mrs00109Filter castFilter = null; 
        List<Mrs00109RDO> ListRdo = new List<Mrs00109RDO>(); 
        List<Mrs00109RDO> ListPatientType = new List<Mrs00109RDO>(); 
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>(); 
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>(); 
        Dictionary<long, V_HIS_SERVICE> dicService = new Dictionary<long, V_HIS_SERVICE>(); 

        public Mrs00109Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00109Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00109Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("bat dau lay du lieu V_HIS_SERE_SERV, Mrs00109 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                //yeu cau
                GetServiceReq(); 
                //yc - dv 
                var listServiceReqId = dicServiceReq.Keys.Distinct().ToList(); 
                if (IsNotNullOrEmpty(listServiceReqId)) ListSereServ = GetSereServ(listServiceReqId); 
                //dich vu
                var listServiceId = ListSereServ.Select(o => o.SERVICE_ID).Distinct().ToList(); 
                if (IsNotNullOrEmpty(listServiceId)) dicService = GetService(listServiceId).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 

                Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListSereServ.Count.ToString()); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00109."); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private List<V_HIS_SERVICE> GetService(List<long> listServiceId)
        {
            List<V_HIS_SERVICE> result = new List<V_HIS_SERVICE>(); 
            try
            {
                var skip = 0; 
                while (listServiceId.Count - skip > 0)
                {
                    var listIDs = listServiceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisServiceViewFilterQuery filterService = new HisServiceViewFilterQuery(); 
                    filterService.IDs = listIDs; 
                    var listServiceSub = new MOS.MANAGER.HisService.HisServiceManager(paramGet).GetView(filterService); 
                    result.AddRange(listServiceSub); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new List<V_HIS_SERVICE>(); 
            }
            return result; 
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
                    var listSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filterSereServ); 
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
                srFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT; 
                //Config.IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT; 
                dicServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager().Get(srFilter).ToDictionary(o => o.ID); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                ProcessListSereServ(ListSereServ); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListSereServ(List<V_HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {

                    var Groups = ListSereServ.GroupBy(g => g.PATIENT_TYPE_ID).ToList(); 
                    foreach (var group in Groups)
                    {
                        List<Mrs00109RDO> listRdo = new List<Mrs00109RDO>(); 
                        List<V_HIS_SERE_SERV> hisSereServs = group.ToList<V_HIS_SERE_SERV>(); 
                        Mrs00109RDO rdo = new Mrs00109RDO(); 
                        rdo.PATIENT_TYPE_ID = hisSereServs.First().PATIENT_TYPE_ID; 
                        rdo.PATIENT_TYPE_NAME = hisSereServs.First().PATIENT_TYPE_NAME; 
                        var Group2s = hisSereServs.GroupBy(g => new { g.SERVICE_ID, sv(g).COGS, g.VIR_PRICE }).ToList(); 
                        foreach (var group2 in Group2s)
                        {
                            List<V_HIS_SERE_SERV> listSub = group2.ToList<V_HIS_SERE_SERV>(); 
                            listRdo.Add(new Mrs00109RDO(listSub, sv(listSub.First()))); 
                        }
                        listRdo = listRdo.Where(o => o.AMOUNT > 0).ToList(); 

                        if (IsNotNullOrEmpty(listRdo))
                        {
                            rdo.TOTAL_COST_PRICE = listRdo.Sum(s => s.TOTAL_COST_PRICE); 
                            rdo.TOTAL_FEE_PRICE = listRdo.Sum(s => s.TOTAL_FEE_PRICE); 
                            rdo.AMOUNT = listRdo.Sum(s => s.AMOUNT); 
                            ListPatientType.Add(rdo); 
                            ListRdo.AddRange(listRdo); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private V_HIS_SERVICE sv(V_HIS_SERE_SERV g)
        {
            V_HIS_SERVICE result = new V_HIS_SERVICE(); 
            try
            {
                if (dicService.ContainsKey(g.SERVICE_ID))
                {
                    result = dicService[g.SERVICE_ID]; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return new V_HIS_SERVICE(); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                #region Cac the Single
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                #endregion

                ListRdo = ListRdo.OrderBy(o => o.PATIENT_TYPE_ID).ThenBy(t => t.SERVICE_TYPE_ID).ThenBy(b => b.SERVICE_ID).ToList(); 
                ListPatientType = ListPatientType.OrderBy(o => o.PATIENT_TYPE_ID).ToList(); 

                objectTag.AddObjectData(store, "PatientTypes", ListPatientType); 
                objectTag.AddObjectData(store, "SereServs", ListRdo); 
                objectTag.AddRelationship(store, "PatientTypes", "SereServs", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID"); 
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData()); 
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData()); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

    class CustomerFuncMergeSameData : FlexCel.Report.TFlexCelUserFunction
    {
        long PatientTypeId; 
        int SameType; 
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            bool result = false; 
            try
            {
                long patientTypeId = Convert.ToInt64(parameters[0]); 
                int ServiceTypeId = Convert.ToInt32(parameters[1]); 

                if (patientTypeId > 0 && ServiceTypeId > 0)
                {
                    if (SameType == ServiceTypeId && PatientTypeId == patientTypeId)
                    {
                        return true; 
                    }
                    else
                    {
                        PatientTypeId = patientTypeId; 
                        SameType = ServiceTypeId; 
                        return false; 
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result; 
        }
    }

    class RDOCustomerFuncManyRownumberData : FlexCel.Report.TFlexCelUserFunction
    {
        long PatientTypeId; 
        long num_order = 0; 
        public RDOCustomerFuncManyRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            try
            {
                long patientTypeId = Convert.ToInt64(parameters[0]); 

                if (patientTypeId > 0)
                {
                    if (PatientTypeId == patientTypeId)
                    {
                        num_order = num_order + 1; 
                    }
                    else
                    {
                        PatientTypeId = patientTypeId; 
                        num_order = 1; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex); 
            }

            return num_order; 
        }
    }
}
