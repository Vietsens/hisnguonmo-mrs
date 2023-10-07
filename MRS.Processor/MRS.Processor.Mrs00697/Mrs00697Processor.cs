using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00697
{
    class Mrs00697Processor : AbstractProcessor
    {
        Mrs00697Filter castFilter = null;
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, HIS_TREATMENT> DicTreatment = new Dictionary<long, HIS_TREATMENT>();
        List<Mrs00697RDO> ListRdo = new List<Mrs00697RDO>();
        Dictionary<long, V_HIS_SERVICE> DicService = new Dictionary<long, V_HIS_SERVICE>();

        public Mrs00697Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00697Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00697Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();

                if (castFilter.TIME_TYPE.HasValue)
                {
                    if (castFilter.TIME_TYPE.Value == 2)
                    {
                        HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                        approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                        approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                        approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                        approvalFilter.ORDER_DIRECTION = "ASC";
                        var ListHeinApprovalBhyt = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                        if (IsNotNullOrEmpty(ListHeinApprovalBhyt))
                        {
                            var treatmentIds = ListHeinApprovalBhyt.Select(s => s.TREATMENT_ID).Distinct().ToList();
                            var heinApprovalIds = ListHeinApprovalBhyt.Select(s => s.ID).ToList();

                            int skipt = 0;
                            while (treatmentIds.Count - skipt > 0)
                            {
                                var listId = treatmentIds.Skip(skipt).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skipt += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                                HisTreatmentFilterQuery TreatmentFilter = new HisTreatmentFilterQuery();
                                TreatmentFilter.IDs = listId;
                                var treatment = new HisTreatmentManager().Get(TreatmentFilter);
                                if (IsNotNullOrEmpty(treatment))
                                {
                                    listTreatment.AddRange(treatment);
                                }
                            }

                            skipt = 0;
                            while (heinApprovalIds.Count - skipt > 0)
                            {
                                var listIds = heinApprovalIds.Skip(skipt).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skipt += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                                var sereServ = ManagerSql.GetSSByHeinApprovalId(listIds, castFilter.PATIENT_TYPE_ID);
                                if (IsNotNullOrEmpty(sereServ))
                                {
                                    ListSereServ.AddRange(sereServ);
                                }
                            }
                        }
                    }
                    else
                    {
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        if (castFilter.TIME_TYPE.Value == 1)
                        {
                            treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                            treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        }
                        else
                        {
                            treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                            treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                        }

                        treatmentFilter.IS_PAUSE = true;
                        listTreatment = new HisTreatmentManager(paramGet).Get(treatmentFilter);

                        int skip = 0;
                        while (listTreatment.Count - skip > 0)
                        {
                            var listIds = listTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            var sereServ = ManagerSql.GetSSByTreatmentId(listIds.Select(s => s.ID).ToList(), castFilter.PATIENT_TYPE_ID);
                            if (IsNotNullOrEmpty(sereServ))
                            {
                                ListSereServ.AddRange(sereServ);
                            }
                        }
                    }
                }
                else
                {
                    HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                    tranFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                    tranFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                    var listTransaction = new HisTransactionManager(paramGet).Get(tranFilter);

                    int skip = 0;
                    while (listTransaction.Count - skip > 0)
                    {
                        var listIds = listTransaction.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var sereServ = ManagerSql.GetSSByTransactionId(listIds.Select(s => s.ID).ToList(), castFilter.PATIENT_TYPE_ID);
                        if (IsNotNullOrEmpty(sereServ))
                        {
                            ListSereServ.AddRange(sereServ);
                        }
                    }

                    var treatmentIds = ListSereServ.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                        treatFilter.IDs = listIds;
                        var treatment = new HisTreatmentManager(paramGet).Get(treatFilter);
                        if (IsNotNullOrEmpty(treatment))
                        {
                            listTreatment.AddRange(treatment);
                        }
                    }
                }

                if (IsNotNullOrEmpty(listTreatment))
                {
                    DicTreatment = listTreatment.ToDictionary(o => o.ID, o => o);
                }

                HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                serviceFilter.IS_ACTIVE = 1;
                var service = new HisServiceManager(paramGet).GetView(serviceFilter);
                if (IsNotNullOrEmpty(service))
                {
                    DicService = service.ToDictionary(o => o.ID, o => o);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var sereServ in ListSereServ)
                    {
                        if (!DicTreatment.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                        {
                            continue;
                        }

                        if (sereServ.VIR_PRICE <= 0)
                        {
                            continue;
                        }

                        Mrs00697RDO rdo = new Mrs00697RDO(sereServ);
                        List<V_HIS_SERVICE_PATY> patys = new List<V_HIS_SERVICE_PATY>();
                        if (MANAGER.Config.HisServicePatyCFG.DicServicePaty != null && MANAGER.Config.HisServicePatyCFG.DicServicePaty.ContainsKey(sereServ.SERVICE_ID))
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DicServicePaty[sereServ.SERVICE_ID].Where(o => o.PATIENT_TYPE_ID == sereServ.PATIENT_TYPE_ID).ToList();
                        }
                        else
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DATAs.Where(o => sereServ.SERVICE_ID == o.SERVICE_ID && o.PATIENT_TYPE_ID == sereServ.PATIENT_TYPE_ID).ToList();
                        }

                        //giá mới là giá hiện tại của dịch vụ được chỉ định
                        //giá cũ là giá có thời gian áp dụng đến nhỏ hơn thời gian áp dụng từ của chính sách giá tại thời điểm chỉ định dịch vụ
                        if (DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            rdo.NT_AMOUNT = 1;
                        }
                        else
                        {
                            rdo.NGT_AMOUNT = 1;
                        }

                        bool hein_price = false;
                        if (sereServ.PRIMARY_PATIENT_TYPE_ID.HasValue)
                        {
                            rdo.NEW_PRICE = (sereServ.PRIMARY_PRICE ?? 0) * (1 + sereServ.VAT_RATIO);
                        }
                        else
                        {
                            rdo.NEW_PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                            if (rdo.NEW_PRICE < (sereServ.VIR_PRICE ?? 0))
                            {
                                hein_price = true;
                            }
                        }

                        if (IsNotNullOrEmpty(patys))
                        {
                            //lấy giá hiện tại
                            //lấy giá cũ là giá khác
                            var currentPaty = MOS.ServicePaty.ServicePatyUtil.GetApplied(patys, this.branch_id, null, sereServ.TDL_REQUEST_ROOM_ID, sereServ.TDL_REQUEST_DEPARTMENT_ID, sereServ.TDL_INTRUCTION_TIME, DicTreatment[sereServ.TDL_TREATMENT_ID ?? 0].IN_TIME, sereServ.SERVICE_ID, sereServ.PATIENT_TYPE_ID, null);
                            if (currentPaty != null)
                            {
                                var oldpaty = patys.Where(o => o.ID != currentPaty.ID).OrderByDescending(o => o.PRIORITY).ThenByDescending(o => o.ID).FirstOrDefault();
                                if (oldpaty != null)
                                {
                                    rdo.OLD_PRICE = oldpaty.PRICE * (1 + oldpaty.VAT_RATIO);
                                }
                            }
                        }

                        //nếu dùng giá trần bhyt thì giá cũ sẽ là giá trần bhyt cũ trong loại dịch vụ
                        if (hein_price && DicService.ContainsKey(sereServ.SERVICE_ID) && DicService[sereServ.SERVICE_ID].HEIN_LIMIT_PRICE_OLD.HasValue)
                        {
                            rdo.OLD_PRICE = DicService[sereServ.SERVICE_ID].HEIN_LIMIT_PRICE_OLD.Value * (1 + (DicService[sereServ.SERVICE_ID].HEIN_LIMIT_RATIO_OLD ?? 0));
                        }

                        ListRdo.Add(rdo);
                    }

                    ListRdo = ListRdo.GroupBy(s => new { s.SERVICE_ID, s.NEW_PRICE }).Select(s => new Mrs00697RDO(s.ToList())).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                ListRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                List<Mrs00697RDO> ListTypeRdo = new List<Mrs00697RDO>();
                var groupType = ListRdo.GroupBy(s => s.TDL_SERVICE_TYPE_ID).ToList();
                foreach (var item in groupType)
                {
                    ListTypeRdo.Add(new Mrs00697RDO(item.ToList()));
                }
                ListTypeRdo = ListTypeRdo.OrderBy(s => s.TDL_SERVICE_TYPE_ID).ToList();
                ListRdo = ListRdo.OrderBy(s => s.TDL_HEIN_ORDER).ThenBy(s => s.TDL_SERVICE_CODE).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportType", ListTypeRdo);
                objectTag.AddRelationship(store, "ReportType", "Report", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
