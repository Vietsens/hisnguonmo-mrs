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
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00128
{
    public class Mrs00128Processor : AbstractProcessor
    {
        List<Mrs00128RDO> _lisMrs00128RDO = new List<Mrs00128RDO>();
        Mrs00128Filter CastFilter;
        List<V_HIS_EXP_MEST_MEDICINE> listExpMertMedicineView = new List<V_HIS_EXP_MEST_MEDICINE>();

        public Mrs00128Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00128Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00128Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Debug("Bat dau lay du lieu filter: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                //////////////////////////////////////////////////////////////////////////////////-V_HIS_SEREVICE
                var metyFilterExpMertMedicineView = new HisExpMestMedicineViewFilterQuery
                {
                    MEDICINE_TYPE_ID = CastFilter.MEDICINE_TYPE_ID,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN,//HisExpMestTypeCFG.EXP_MEST_TYPE_ID__SALE,
                    EXP_TIME_FROM = CastFilter.DATE_FROM,
                    EXP_TIME_TO = CastFilter.DATE_TO,
                    IS_EXPORT = true
                };
                listExpMertMedicineView = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMertMedicineView);


                if (CastFilter.BRANCH_IDs != null)
                {
                 


                    var departmentIds = (new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { BRANCH_IDs = this.CastFilter.BRANCH_IDs }) ?? new List<HIS_DEPARTMENT>()).Select(o => o.ID).ToList();

                    listExpMertMedicineView = listExpMertMedicineView.Where(o => departmentIds.Contains(o.REQ_DEPARTMENT_ID)).ToList();


                }

                if (CastFilter.BRANCH_ID != null)
                {



                    var departmentIds = (new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { BRANCH_ID = this.CastFilter.BRANCH_ID }) ?? new List<HIS_DEPARTMENT>()).Select(o => o.ID).ToList();

                    listExpMertMedicineView = listExpMertMedicineView.Where(o => departmentIds.Contains(o.REQ_DEPARTMENT_ID)).ToList();


                }

                //////////////////////////////////////////////////////////////////////////////////
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00128." +
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
                if (IsNotNullOrEmpty(listExpMertMedicineView))
                {
                    ProcessFilterData(listExpMertMedicineView);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines)
        {
            var listRdos = listExpMestMedicines.Select(listExpMestMedicine => new Mrs00128RDO
            {
                EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listExpMestMedicine.EXP_TIME.ToString()),
                MEDICINE_TYPE_NAME = listExpMestMedicine.MEDICINE_TYPE_NAME,
                TOTAL_AMOUNT = listExpMestMedicine.AMOUNT,
                TOTAL_DISCOUNT = listExpMestMedicine.DISCOUNT,
                TOTAL_PRICE = listExpMestMedicine.PRICE
            }).OrderBy(s => s.EXP_TIME).GroupBy(s => s.EXP_TIME).ToList();

            foreach (var listRdo in listRdos)
            {
                var groupByTotalPrices = listRdo.GroupBy(s => s.TOTAL_PRICE).ToList();
                var listTotalPrice = new List<decimal>();
                foreach (var groupByTotalPrice in groupByTotalPrices)
                {
                    var amount = groupByTotalPrice.Select(s => s.TOTAL_AMOUNT).Sum();
                    var price = groupByTotalPrice.Key;
                    listTotalPrice.Add(amount * (decimal)price);
                }

                var totalAmount = listRdo.Select(s => s.TOTAL_AMOUNT).Sum();
                var totalDiscount = listRdo.Select(s => s.TOTAL_DISCOUNT).Sum();
                var rdo = new Mrs00128RDO
                {
                    EXP_TIME = listRdo.Key,
                    MEDICINE_TYPE_NAME = listRdo.Select(s => s.MEDICINE_TYPE_NAME).First(),
                    TOTAL_AMOUNT = totalAmount,
                    TOTAL_DISCOUNT = totalDiscount,
                    TOTAL_PRICE = listTotalPrice.Sum() - totalDiscount
                };
                _lisMrs00128RDO.Add(rdo);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                objectTag.AddObjectData(store, "Report", _lisMrs00128RDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
