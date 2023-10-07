using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00651
{
    class Mrs00651Processor : AbstractProcessor
    {
        Mrs00651Filter castFilter = null;
        List<Mrs00651RDO> ListParent = new List<Mrs00651RDO>();
        List<Mrs00651RDO> ListRdo = new List<Mrs00651RDO>();

        List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>();
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();

        public Mrs00651Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00651Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00651Filter)this.reportFilter;

                HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                tranFilter.IS_CANCEL = false;
                tranFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                tranFilter.HAS_SALL_TYPE = true;
                tranFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                var totalTranssaction = new MOS.MANAGER.HisTransaction.HisTransactionManager().GetView(tranFilter);

                if (IsNotNullOrEmpty(totalTranssaction) && IsNotNullOrEmpty(castFilter.CASHIER_LOGINNAMEs))
                {
                    totalTranssaction = totalTranssaction.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }

                if (IsNotNullOrEmpty(totalTranssaction))
                {
                    List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();

                    var skip = 0;
                    while (totalTranssaction.Count - skip > 0)
                    {
                        var listIDs = totalTranssaction.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var expMestFilter = new HisExpMestFilterQuery()
                        {
                            BILL_IDs = listIDs.Select(s => s.ID).ToList()
                        };
                        var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager().Get(expMestFilter);
                        if (expMest != null && expMest.Count > 0) listExpMest.AddRange(expMest);
                    }

                    if (listExpMest!=null)
                    {
                        var lstTransactionIds = listExpMest.Select(s => s.BILL_ID ?? 0).Distinct().ToList();
                        totalTranssaction = totalTranssaction.Where(o => lstTransactionIds.Contains(o.ID)).ToList();
                    }

                    this.ListExpMest.AddRange(listExpMest);
                    this.ListTransaction.AddRange(totalTranssaction);
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
                ListRdo.Clear();
                ListParent.Clear();
                if (IsNotNullOrEmpty(this.ListTransaction))
                {
                    var listSub = (from r in ListTransaction select new Mrs00651RDO(r)).ToList();
                    if (IsNotNullOrEmpty(listSub))
                        ListRdo.AddRange(listSub);

                    foreach (var item in ListRdo)
                    {
                        if (!IsNotNull(item.TDL_PATIENT_NAME) && !IsNotNull(item.TDL_PATIENT_DOB) && !IsNotNull(item.BUYER_NAME))
                        {
                            var expMest = ListExpMest.FirstOrDefault(o => o.BILL_ID == item.ID);
                            if (expMest != null)
                            {
                                var room = HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == expMest.REQ_ROOM_ID);
                                item.TDL_PATIENT_ADDRESS = expMest.TDL_PATIENT_ADDRESS;
                                item.TDL_PATIENT_CODE = expMest.TDL_PATIENT_CODE;
                                item.TDL_PATIENT_DOB = expMest.TDL_PATIENT_DOB;
                                item.TDL_PATIENT_GENDER_NAME = expMest.TDL_PATIENT_GENDER_NAME;
                                item.TDL_PATIENT_NAME = expMest.TDL_PATIENT_NAME;
                                item.TDL_TREATMENT_CODE = expMest.TDL_TREATMENT_CODE;
                                if (room != null)
                                {
                                    if (room.IS_EXAM == 1)
                                    {
                                        item.CHECK_EXAM_ROOM = "X";
                                    }
                                    else if (room.ROOM_TYPE_CODE == "GI")
                                    {
                                        item.CHECK_BED_ROOM = "X";
                                    }
                                    item.ROOM_CODE = room.ROOM_CODE;
                                    item.ROOM_NAME = room.ROOM_NAME;
                                    item.DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                                }
                                else
                                {
                                    item.CHECK_EXAM_ROOM = item.CHECK_BED_ROOM = "";
                                }
                            }

                            
                        }
                    }

                    var groups = ListRdo.GroupBy(g => g.TRANSACTION_DATE).ToList();
                    var lstGroup = (from r in groups
                                    select new Mrs00651RDO()
                                    {
                                        TRANSACTION_DAY_STR = r.First().TRANSACTION_DAY_STR,
                                        TRANSACTION_DATE = r.First().TRANSACTION_DATE
                                    }).ToList();
                    if (IsNotNullOrEmpty(lstGroup))
                        ListParent.AddRange(lstGroup);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                if (!IsNotNullOrEmpty(ListParent))
                {
                    ListParent.Add(new Mrs00651RDO());
                }

                ListParent = ListParent.OrderBy(s => s.TRANSACTION_DAY_STR).ToList();
                ListRdo = ListRdo.OrderBy(s => s.TRANSACTION_CODE).ToList();

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Parent", ListParent);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Parent", "Report", "TRANSACTION_DATE", "TRANSACTION_DATE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
