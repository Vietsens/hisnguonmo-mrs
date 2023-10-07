using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMedicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00127
{
    public class Mrs00127Processor : AbstractProcessor
    {
        List<Mrs00127RDO> Mrs00127RDO = new List<Mrs00127RDO>();
        Mrs00127Filter CastFilter;
        private string SERVICE_REPORT_NAME;
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();

        public Mrs00127Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00127Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00127Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                ////////////////////////////////////////////////////////////////////////////////// -V_HIS_EXP_MEST_MEDICINE
                var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                {
                    MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_ID,

                    EXP_TIME_FROM = CastFilter.DATE_FROM,
                    EXP_TIME_TO = CastFilter.DATE_TO,
                    IS_EXPORT = true
                };
                if (this.CastFilter.EXP_MEST_TYPE_IDs != null)
                {
                    metyFilterExpMestMedicine.EXP_MEST_TYPE_IDs = this.CastFilter.EXP_MEST_TYPE_IDs;
                }
                else
                {
                    metyFilterExpMestMedicine.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;//HisExpMestTypeCFG.EXP_MEST_TYPE_ID__SALE,
                }
                listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                //////////////////////////////////////////////////////////////////////////////////
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00127." +
                        LogUtil.TraceData(
                            LogUtil.GetMemberName(() => paramGet), paramGet));
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
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    ProcessFillterData(listExpMestMedicine);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFillterData(List<V_HIS_EXP_MEST_MEDICINE> listeExpMestMedicines)
        {
            if (listeExpMestMedicines != null && listeExpMestMedicines.Count > 0)
            {
                var listRdos = listeExpMestMedicines.Select(o => new Mrs00127RDO(o)).OrderBy(s => s.EXP_TIME).GroupBy(s => s.EXP_DATE).ToList();
                Inventec.Common.Logging.LogSystem.Error("listRdos" + listRdos.Count);
                foreach (var listRdo in listRdos)
                {
                    var groupByMedicineTypeNames = listRdo.GroupBy(s => s.MEDICINE_TYPE_ID).ToList();
                    foreach (var groupByMedicineTypeName in groupByMedicineTypeNames)
                    {
                        //var groupByPrices = groupByMedicineTypeName.GroupBy(s => s.PRICE).ToList();
                        //var listTotalPrice = new List<decimal>();
                        //foreach (var group in groupByPrices)
                        //{
                        //    if (group.Key != null)
                        //    {
                        //        var amount = group.Sum(s => s.AMOUNT);
                        //        var price = group.Key;
                        //        listTotalPrice.Add(amount * (decimal)price);
                        //    }
                        //}
                        var totalAmount = groupByMedicineTypeName.Sum(s => s.AMOUNT);
                        var totalDiscount = groupByMedicineTypeName.Sum(s => s.TOTAL_DISCOUNT);

                        var rdo = new Mrs00127RDO(groupByMedicineTypeName.ToList().First());
                        rdo.TOTAL_AMOUNT = totalAmount;
                        rdo.TOTAL_DISCOUNT = totalDiscount;
                        rdo.TOTAL_PRICE = groupByMedicineTypeName.Sum(s => s.AMOUNT * s.PRICE) - totalDiscount;
                        Mrs00127RDO.Add(rdo);
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("SERVICE_REPORT_NAME", SERVICE_REPORT_NAME);
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

                this.Mrs00127RDO = Mrs00127RDO.OrderBy(o => o.EXP_TIME).ToList();
                objectTag.AddObjectData(store, "Report", Mrs00127RDO);
                objectTag.SetUserFunction(store, "FuncSameTitleRow", new CustomerFuncMergeSameData(Mrs00127RDO));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        class CustomerFuncMergeSameData : FlexCel.Report.TFlexCelUserFunction
        {
            List<Mrs00127RDO> sereServRdos;
            int SameType;
            public CustomerFuncMergeSameData(List<Mrs00127RDO> sereServRdos)
            {
                this.sereServRdos = sereServRdos;
            }
            public override object Evaluate(object[] parameters)
            {
                bool result = false;
                try
                {
                    if (parameters == null || parameters.Length < 1 || sereServRdos == null || sereServRdos.Count == 0)
                        throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                    long? currentValue = null;
                    long? nextValue = null;

                    int currentIdx = (int)parameters[0];

                    currentValue = sereServRdos[currentIdx].EXP_DATE;
                    if (currentIdx + 1 < sereServRdos.Count)
                    {
                        nextValue = sereServRdos[currentIdx + 1].EXP_DATE;

                        if (currentValue != null
                        && nextValue != null
                        && currentValue.Equals((nextValue)))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                }

                return result;
            }
        }
    }
}
