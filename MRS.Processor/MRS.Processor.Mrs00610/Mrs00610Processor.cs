using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisSereServFile;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;


namespace MRS.Processor.Mrs00610
{

    class Mrs00610Processor : AbstractProcessor
    {
        enum TimeData
        {
            FeeLock,
            Bill
        }
        Mrs00610Filter castFilter = null;
        List<Mrs00610RDO> listRdo = new List<Mrs00610RDO>();
        List<Mrs00610RDO> listRdoCateGory = new List<Mrs00610RDO>();

        List<Mrs00610RDO> listHisSereServ = new List<Mrs00610RDO>();

        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();

        public Mrs00610Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00610Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00610Filter)this.reportFilter;

                TimeData OptionTakeData = TimeData.FeeLock;
                if (this.castFilter.FEE_LOCK_TIME_FROM != null && this.castFilter.FEE_LOCK_TIME_TO != null)
                {
                    OptionTakeData = TimeData.FeeLock;
                }
                else if (this.castFilter.TRANSACTION_TIME_FROM != null && this.castFilter.TRANSACTION_TIME_TO != null)
                {
                    OptionTakeData = TimeData.Bill;
                }
                if (OptionTakeData == TimeData.FeeLock)
                {
                    listHisSereServ = new ManagerSql().GetByFeeLockTime(this.castFilter);
                    listHisTreatment = new ManagerSql().GetTreatmentFeeLock(this.castFilter);
                }
                else if (OptionTakeData == TimeData.Bill)
                {
                    listHisSereServ = new ManagerSql().GetByTransactionTime(this.castFilter);
                    listHisTreatment = new ManagerSql().GetTreatmentBill(this.castFilter);
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
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                //if (IsNotNullOrEmpty(listHisSereServ))
                //{
                //    var group = listHisSereServ.GroupBy(o => o.SERVICE_ID).ToList();
                //    foreach (var item in group)
                //    {
                //        List<Mrs00610RDO> listSub = item.ToList<Mrs00610RDO>();
                //        Mrs00610RDO ss = listSub.First();
                //        var rdo = new Mrs00610RDO();
                //        //Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00610RDO>(rdo, ss);
                //        rdo.SERVICE_ID = ss.SERVICE_ID;
                //        rdo.TDL_SERVICE_CODE = ss.TDL_SERVICE_CODE;
                //        rdo.TDL_SERVICE_NAME = ss.TDL_SERVICE_NAME;
                //        rdo.IN_AMOUNT = listSub.Where(o=>listHisTreatment.Exists(p=>p.ID==o.TDL_TREATMENT_ID&&p.TDL_TREATMENT_TYPE_ID ==IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.AMOUNT);
                //        rdo.OUT_AMOUNT = listSub.Where(o => listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID && p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.AMOUNT);
                //        rdo.ALL_AMOUNT = listSub.Sum(s => s.AMOUNT);

                //        rdo.IN_TOTAL_PRICE = listSub.Where(o => listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID && p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.ALL_TOTAL_PRICE);
                //        rdo.OUT_TOTAL_PRICE = listSub.Where(o => listHisTreatment.Exists(p => p.ID == o.TDL_TREATMENT_ID && p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Sum(s => s.ALL_TOTAL_PRICE);

                //        rdo.ALL_TOTAL_PRICE = listSub.Sum(s => s.ALL_TOTAL_PRICE);

                //        rdo.CATEGORY_CODE = ss.CATEGORY_CODE;
                //        rdo.CATEGORY_NAME = ss.CATEGORY_NAME;
                //        //for (int i = 0; i < 12; i++)
                //        //{
                //        //    PropertyInfo amount = (typeof(Mrs00610RDO)).GetProperty(string.Format("ALL_AMOUNT_{0}", i + 1));
                //        //    PropertyInfo totalPrice = (typeof(Mrs00610RDO)).GetProperty(string.Format("ALL_TOTAL_PRICE_{0}", i + 1));
                //        //    if (amount != null)
                //        //    {
                //        //        amount.SetValue(rdo, listSub.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TIME.Value).Value.Month == i + 1).Sum(s => s.AMOUNT));
                //        //    }
                //        //    if (totalPrice != null)
                //        //    {
                //        //        totalPrice.SetValue(rdo, listSub.Where(o => Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(o.TIME.Value).Value.Month == i + 1).Sum(s => s.VIR_TOTAL_PRICE));
                //        //    }
                //        //}
                //        listRdo.Add(rdo);
                //    }

                //    //
                //}
                listRdoCateGory = MakeGroupCategory(listHisSereServ);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<Mrs00610RDO> MakeGroupCategory(List<Mrs00610RDO> list)
        {
            List<Mrs00610RDO> result = new List<Mrs00610RDO>();
            try
            {
                var group = list.Where(p => p.CATEGORY_CODE != null).GroupBy(o => o.CATEGORY_CODE).ToList();
                if (group != null)
                {
                    foreach (var item in group)
                    {
                        Mrs00610RDO rdo = new Mrs00610RDO();
                        rdo.CATEGORY_CODE = item.Key;
                        rdo.CATEGORY_NAME = item.ToList().First().CATEGORY_NAME;
                        rdo.ALL_TOTAL_PRICE = item.ToList().Sum(s => s.ALL_TOTAL_PRICE);
                        rdo.ALL_AMOUNT = item.ToList().Sum(s => s.ALL_AMOUNT);
                        rdo.IN_TOTAL_PRICE = item.ToList().Sum(s => s.IN_TOTAL_PRICE);
                        rdo.IN_AMOUNT = item.ToList().Sum(s => s.IN_AMOUNT);
                        rdo.OUT_TOTAL_PRICE = item.ToList().Sum(s => s.OUT_TOTAL_PRICE);
                        rdo.OUT_AMOUNT = item.ToList().Sum(s => s.OUT_AMOUNT);

                        //for (int i = 0; i < 12; i++)
                        //{
                        //    PropertyInfo totalPrice = null;
                        //    PropertyInfo amount = null;

                        //    totalPrice = (typeof(Mrs00610RDO)).GetProperty(string.Format("ALL_TOTAL_PRICE_{0}", i + 1));
                        //    amount = (typeof(Mrs00610RDO)).GetProperty(string.Format("ALL_AMOUNT_{0}", i + 1));
                        //    amount.SetValue(rdo, (decimal)amount.GetValue(rdo) + item.ToList<Mrs00610RDO>().Sum(s => (decimal)amount.GetValue(s)));
                        //    totalPrice.SetValue(rdo, (decimal)totalPrice.GetValue(rdo) + item.ToList<Mrs00610RDO>().Sum(s => (decimal)totalPrice.GetValue(s)));
                        //}
                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

                result = new List<Mrs00610RDO>();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            if (this.castFilter.FEE_LOCK_TIME_FROM != null)
            {
                dicSingleTag.Add("TIME_FROM", castFilter.FEE_LOCK_TIME_FROM);
            }
            if (this.castFilter.FEE_LOCK_TIME_TO != null)
            {
                dicSingleTag.Add("TIME_TO", castFilter.FEE_LOCK_TIME_TO);
            }
            if (this.castFilter.TRANSACTION_TIME_FROM != null)
            {
                dicSingleTag.Add("TIME_FROM", castFilter.TRANSACTION_TIME_FROM);
            }
            if (this.castFilter.TRANSACTION_TIME_TO != null)
            {
                dicSingleTag.Add("TIME_TO", castFilter.TRANSACTION_TIME_TO);
            }
            dicSingleTag.Add("TITLE_REPORT", string.Join(" - ", listRdoCateGory.Select(o=>o.CATEGORY_NAME).ToList()));

            objectTag.AddObjectData(store, "RdoDetail", listHisSereServ.OrderBy(s => s.TDL_SERVICE_NAME).ToList());
            objectTag.AddObjectData(store, "Rdo", listHisSereServ.OrderBy(s => s.TDL_SERVICE_NAME).ToList());
            objectTag.AddObjectData(store, "RdoCategory", listRdoCateGory);
            objectTag.AddRelationship(store, "RdoCategory", "Rdo", "CATEGORY_CODE", "CATEGORY_CODE");

        }
    }
}
