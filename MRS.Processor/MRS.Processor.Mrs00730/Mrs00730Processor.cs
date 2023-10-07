using Inventec.Common.Logging;
using Inventec.Core;
using MOS.MANAGER;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMedicine;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00730
{
    class Mrs00730Processor : AbstractProcessor
    {
        List<Mrs00730RDO> list1 = new List<Mrs00730RDO>();
        List<Mrs00730RDO> ListRdo = new List<Mrs00730RDO>();
        List<Mrs00730RDO> ListRdoTotal = new List<Mrs00730RDO>();
        List<V_HIS_EXP_MEST_MEDICINE> ListExpMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<Mrs00730ServiceRDO> ListService = new List<Mrs00730ServiceRDO>();
        Mrs00730Filter filter = null;
        CommonParam paramGet = new CommonParam();
        
        
        public Mrs00730Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00730Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00730Filter)reportFilter;
                ListService = new ManagerSql().GetService();

                HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                mediFilter.EXP_TIME_FROM = filter.TIME_FROM;
                mediFilter.EXP_TIME_TO = filter.TIME_TO;
                mediFilter.IS_EXPORT = true;
                ListExpMedicine = new HisExpMestMedicineManager(paramGet).GetView(mediFilter);
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
            bool result = true;
            try
            {
                
                List<V_HIS_SERVICE_RETY_CAT> listRety = new HisServiceRetyCatManager().GetView(new HisServiceRetyCatViewFilterQuery());
                if (IsNotNullOrEmpty(ListExpMedicine))
                {
                    string typeGroup = "{0}_{1}";//mặc định gộp theo lô và giá bán
                    if (filter.INPUT_DATA_ID_GROUP_TYPE == 2)
                    {
                        typeGroup = "{2}_{3}";//gộp theo loại và giá nhập
                    }
                    if (filter.INPUT_DATA_ID_GROUP_TYPE == 3)
                    {
                        typeGroup = "{1}_{2}";//gộp theo loại và giá bán
                    }
                    if (filter.INPUT_DATA_ID_GROUP_TYPE == 4)
                    {
                        typeGroup = "{0}";//gộp theo lô
                    }
                    if (filter.INPUT_DATA_ID_GROUP_TYPE == 5)
                    {
                        typeGroup = "{2}";//gộp theo loại
                    }
                    var group = ListExpMedicine.GroupBy(p => string.Format(typeGroup, p.MEDICINE_ID, (p.PRICE??0)*(1+(p.VAT_RATIO??0)),p.MEDICINE_TYPE_ID,(p.IMP_PRICE)*(1+(p.IMP_VAT_RATIO)))).ToList();
                    foreach (var item in group)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listSub = item.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        var parent_service = ListService.FirstOrDefault(p => p.PARENT_ID == listSub[0].SERVICE_ID) ?? new Mrs00730ServiceRDO();
                        var category = listRety.FirstOrDefault(p => p.SERVICE_ID == listSub[0].SERVICE_ID) ?? new V_HIS_SERVICE_RETY_CAT();
                        Mrs00730RDO rdo = new Mrs00730RDO();
                        
                        rdo.MEDICINE_CODE = listSub[0].MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_NAME = listSub[0].MEDICINE_TYPE_NAME;
                        if (parent_service != null)
                        {
                            rdo.PARENT_MEDICINE_CODE = parent_service.PARENT_CODE;
                            rdo.PARENT_MEDICINE_NAME = parent_service.PARENT_NAME;
                        }
                        rdo.EXP_AMOUNT = listSub.Sum(o => o.AMOUNT);
                        rdo.PRICE = (listSub[0].PRICE ?? 0) * (1 + (listSub[0].VAT_RATIO ?? 0));
                        rdo.TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0)));
                        if (filter.INPUT_DATA_ID_GROUP_TYPE == 2)
                        {
                            rdo.TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.IMP_PRICE) * (1 + (o.IMP_VAT_RATIO)));
                            rdo.PRICE = (listSub[0].IMP_PRICE) * (1 + listSub[0].IMP_VAT_RATIO);//gộp theo loại và giá nhập
                        }
                        if (filter.INPUT_DATA_ID_GROUP_TYPE ==3)
                        {
                            rdo.TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.PRICE ?? 0) * (1 + (o.VAT_RATIO ?? 0)));
                            rdo.PRICE = (listSub[0].PRICE ?? 0) * (1 + (listSub[0].VAT_RATIO ?? 0));//gộp theo loại và giá bán
                        }
                        if (filter.INPUT_DATA_ID_GROUP_TYPE == 4)
                        {
                            rdo.TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.IMP_PRICE) * (1 + (o.IMP_VAT_RATIO)));
                            rdo.PRICE = (listSub[0].IMP_PRICE) * (1 + listSub[0].IMP_VAT_RATIO);//gộp theo lô
                        }
                        if (filter.INPUT_DATA_ID_GROUP_TYPE ==5)
                        {
                            rdo.TOTAL_PRICE = listSub.Sum(o => o.AMOUNT * (o.IMP_PRICE) * (1 + (o.IMP_VAT_RATIO)));
                            rdo.PRICE = (listSub[0].IMP_PRICE) * (1 + listSub[0].IMP_VAT_RATIO);//gộp theo loại và giá nhập
                        }
                        rdo.CONCENTRA = listSub[0].CONCENTRA;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        rdo.NATIONAL_NAME = !string.IsNullOrEmpty(listSub[0].NATIONAL_NAME) ? listSub[0].NATIONAL_NAME.ToLower() : "";
                        
                        rdo.CATEGORY_CODE = category != null ? category.CATEGORY_CODE : "";
                        ListRdo.Add(rdo);
                    }
                    if (ListRdo != null)
                    {
                        var total = ListRdo.Sum(p => p.TOTAL_PRICE);
                        foreach (var item1 in ListRdo)
                        {
                            item1.PERCENT_PRICE = item1.TOTAL_PRICE / total * 100 ?? 0;
                            
                        }


                        ListRdo = ListRdo.OrderByDescending(p => p.PERCENT_PRICE).ToList();
                        var max = ListRdo.Max(p => p.PERCENT_PRICE) ?? 0;
                        for (int i = 0; i < ListRdo.Count(); i++)
                        {
                            ListRdo[0].LUY_KE = max;
                            if (i > 0)
                            {
                                ListRdo[i].LUY_KE = ListRdo[i - 1].LUY_KE + ListRdo[i].PERCENT_PRICE;
                            }
                        }
                        
                    }
                    list1 = ListRdoTotal = ListRdo;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            dicSingleTag.Add("MAX_PERCENT_PRICE", ListRdo.Max(p => p.PERCENT_PRICE));
            
            objectTag.AddObjectData(store, "Report", ListRdoTotal);

            objectTag.AddObjectData(store, "Report1", list1);
        }
    }
}
