using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMaterialType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMediStock;

namespace MRS.Processor.Mrs00257
{
    public class Mrs00257Processor : AbstractProcessor
    {
        Mrs00257Filter filter = null;
        private List<Mrs00257RDO> ListRDO = new List<Mrs00257RDO>();
        private List<V_HIS_BID_MATERIAL_TYPE> ListBidMaterialType = new List<V_HIS_BID_MATERIAL_TYPE>();
        private List<V_HIS_MATERIAL_BEAN> ListMaterialBean = new List<V_HIS_MATERIAL_BEAN>();
        private List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();

        private List<HIS_MEDI_STOCK> ListHisMediStock = new List<HIS_MEDI_STOCK>();
        CommonParam paramGet = new CommonParam();
        public Mrs00257Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00257Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00257Filter)reportFilter);
            var result = true;
            try
            {
                HisMediStockFilterQuery FilterHisMediStock = new HisMediStockFilterQuery()
                {
                    ID = filter.MEDI_STOCK_ID
                };
                ListHisMediStock = new HisMediStockManager(paramGet).Get(FilterHisMediStock);


                HisBidMaterialTypeViewFilterQuery Filter = new HisBidMaterialTypeViewFilterQuery()
                {
                    BID_IDs = filter.BID_IDs
                };
                ListBidMaterialType = new HisBidMaterialTypeManager(paramGet).GetView(Filter);

                var materialTypeIds = ListBidMaterialType.Select(o => o.MATERIAL_TYPE_ID ?? 0).Distinct().ToList();

                HisMaterialBeanViewFilterQuery FilterMaterialBeanView = new HisMaterialBeanViewFilterQuery()
                {
                    MATERIAL_TYPE_IDs = materialTypeIds,
                    IN_STOCK = MOS.Filter.HisMaterialBeanViewFilter.InStockEnum.YES
                    //MEDI_STOCK_ID = filter.MEDI_STOCK_ID
                };
                ListMaterialBean = new HisMaterialBeanManager(paramGet).GetView(FilterMaterialBeanView);

                HisImpMestMaterialViewFilterQuery FilterImpMestMaterialView = new HisImpMestMaterialViewFilterQuery()
                {
                    MATERIAL_TYPE_IDs = materialTypeIds,
                    IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                };

                ListImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(FilterImpMestMaterialView);
                if (filter.BID_IDs != null)
                {
                    ListImpMestMaterial = ListImpMestMaterial.Where(o => filter.BID_IDs.Contains(o.BID_ID ?? 0)).ToList();
                }
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
                ListRDO.Clear();
                var GroupByBidId = ListBidMaterialType.GroupBy(o => o.BID_ID).ToList();
                foreach (var group in GroupByBidId)
                {
                    List<V_HIS_BID_MATERIAL_TYPE> ListSub = group.ToList<V_HIS_BID_MATERIAL_TYPE>();
                    foreach (var Sub in ListSub)
                    {
                        Mrs00257RDO rdo = new Mrs00257RDO()
                        {
                            BID_NUMBER = Sub.BID_NUMBER,
                            MATERIAL_TYPE_NAME = Sub.MATERIAL_TYPE_NAME,
                            BID_AMOUNT = Sub.AMOUNT,
                            AMOUNT_COMPLETE = ListImpMestMaterial.Where(o => o.MATERIAL_TYPE_ID == Sub.MATERIAL_TYPE_ID && o.IMP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT).Sum(p => p.AMOUNT),
                            AMOUNT_WAITING = Sub.AMOUNT - ListImpMestMaterial.Where(o => o.MATERIAL_TYPE_ID == Sub.MATERIAL_TYPE_ID && o.IMP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT).Sum(p => p.AMOUNT),
                            END_AMOUNT = ListMaterialBean.Where(o => o.MATERIAL_TYPE_ID == Sub.MATERIAL_TYPE_ID).Sum(p => p.AMOUNT),
                            END_MEDI_AMOUNT = ListMaterialBean.Where(o => o.MEDI_STOCK_ID == this.filter.MEDI_STOCK_ID && o.MATERIAL_TYPE_ID == Sub.MATERIAL_TYPE_ID).Sum(p => p.AMOUNT),
                            BID_END_AMOUNT = Sub.AMOUNT - ListImpMestMaterial.Where(o => o.MATERIAL_TYPE_ID == Sub.MATERIAL_TYPE_ID).Sum(p => p.AMOUNT)
                        };
                        ListRDO.Add(rdo);

                    }
                }
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
            if (IsNotNullOrEmpty(ListHisMediStock)) dicSingleTag.Add("MEDI_STOCK_NAME", ListHisMediStock.First().MEDI_STOCK_NAME);
            objectTag.AddObjectData(store, "Report", ListRDO);
        }
    }
}
