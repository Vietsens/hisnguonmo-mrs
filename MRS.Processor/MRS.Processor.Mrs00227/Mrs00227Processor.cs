using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisBidMedicineType;
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
using MOS.MANAGER.HisBidBloodType;

namespace MRS.Processor.Mrs00227
{
    public class Mrs00227Processor : AbstractProcessor
    {
        private List<Mrs00227RDO> _listMrs00227Rdos = new List<Mrs00227RDO>();
        private Mrs00227Filter filter;

        private List<HIS_BID> listBids = new List<HIS_BID>();
        private Dictionary<long, List<V_HIS_BID_MEDICINE_TYPE>> dicBidMedicineTypes = new Dictionary<long, List<V_HIS_BID_MEDICINE_TYPE>>();
        private Dictionary<long, List<V_HIS_BID_MATERIAL_TYPE>> dicBidMaterialTypes = new Dictionary<long, List<V_HIS_BID_MATERIAL_TYPE>>();
        private Dictionary<long, List<V_HIS_BID_BLOOD_TYPE>> dicBidBloodTypes = new Dictionary<long, List<V_HIS_BID_BLOOD_TYPE>>();

        public Mrs00227Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00227Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00227Filter)reportFilter;
            var result = true;
            try
            {
                //-------------------------------------------------------------------------------------- V_HIS_BID
                var bidFilter = new HisBidFilterQuery
                {
                    CREATE_TIME_FROM = filter.DATE_TIME_FROM,
                    CREATE_TIME_TO = filter.DATE_TIME_TO
                };
                listBids = new HisBidManager(param).Get(bidFilter);
                //-------------------------------------------------------------------------------------- V_HIS_BID_MEDICINE_TYPE
                var listBidIds = listBids.Select(s => s.ID).ToList();
                var skip = 0;
                while (listBidIds.Count - skip > 0)
                {
                    var listIds = listBidIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var bisMedicineTypeFilter = new HisBidMedicineTypeViewFilterQuery
                    {
                        BID_IDs = listIds
                    };
                    var bidMedicineTypeViews = new HisBidMedicineTypeManager(param).GetView(bisMedicineTypeFilter);
                    if (IsNotNullOrEmpty(bidMedicineTypeViews))
                    {
                        foreach (var item in bidMedicineTypeViews)
                        {
                            if (!dicBidMedicineTypes.ContainsKey(item.BID_ID))
                                dicBidMedicineTypes[item.BID_ID] = new List<V_HIS_BID_MEDICINE_TYPE>();
                            dicBidMedicineTypes[item.BID_ID].Add(item);
                        }
                    }

                    var bidMaterialTyeFilter = new HisBidMaterialTypeViewFilterQuery
                    {
                        BID_IDs = listIds
                    };
                    var bidMaterialTypeViews = new HisBidMaterialTypeManager(param).GetView(bidMaterialTyeFilter);
                    if (IsNotNullOrEmpty(bidMaterialTypeViews))
                    {
                        foreach (var item in bidMaterialTypeViews)
                        {
                            if (!dicBidMaterialTypes.ContainsKey(item.BID_ID))
                                dicBidMaterialTypes[item.BID_ID] = new List<V_HIS_BID_MATERIAL_TYPE>();
                            dicBidMaterialTypes[item.BID_ID].Add(item);
                        }
                    }

                    var bidBloodTypeFilter = new HisBidBloodTypeViewFilterQuery
                    {
                        BID_IDs = listIds
                    };
                    var bidBloodTypeViews = new HisBidBloodTypeManager(param).GetView(bidBloodTypeFilter);
                    if (IsNotNullOrEmpty(bidBloodTypeViews))
                    {
                        foreach (var item in bidBloodTypeViews)
                        {
                            if (!dicBidBloodTypes.ContainsKey(item.BID_ID))
                                dicBidBloodTypes[item.BID_ID] = new List<V_HIS_BID_BLOOD_TYPE>();
                            dicBidBloodTypes[item.BID_ID].Add(item);
                        }
                    }
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
                foreach (var listBid in listBids)
                {
                    decimal totalBidsMedicine = 0;
                    decimal totalBidsMaterial = 0;
                    decimal totalBidBlood = 0;
                    var createTimeToString = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listBid.CREATE_TIME.Value);
                    if (dicBidMaterialTypes.ContainsKey(listBid.ID))
                    {
                        totalBidsMaterial = dicBidMaterialTypes[listBid.ID].Select(o => o.AMOUNT * (o.IMP_PRICE ?? 0) * (1 + (o.IMP_VAT_RATIO ?? 0))).Sum();
                    }
                    if (dicBidMedicineTypes.ContainsKey(listBid.ID))
                    {
                        totalBidsMedicine = dicBidMedicineTypes[listBid.ID].Select(o => o.AMOUNT * (o.IMP_PRICE ?? 0) * (1 + (o.IMP_VAT_RATIO ?? 0))).Sum();
                    }
                    if (dicBidBloodTypes.ContainsKey(listBid.ID))
                    {
                        totalBidBlood = dicBidBloodTypes[listBid.ID].Select(o => o.AMOUNT * (o.IMP_PRICE ?? 0) * (1 + (o.IMP_VAT_RATIO ?? 0))).Sum();
                    }
                    //totalBidsMedicine = listBidMedicineTypes.Where(s => s.BID_ID == listBid.ID && s.IMP_PRICE.HasValue).Select(s => s.IMP_PRICE.Value).Sum();
                    //totalBidsMaterial = listBidMaterialTypes.Where(s => s.BID_ID == listBid.ID && s.IMP_PRICE.HasValue).Select(s => s.IMP_PRICE.Value).Sum();
                    var totalBids = totalBidsMedicine + totalBidsMaterial + totalBidBlood;
                    var rdo = new Mrs00227RDO
                    {
                        CREATE_TIME_STRING = createTimeToString,
                        V_HIS_BID = listBid,
                        TOTAL_BIDS = totalBids
                    };
                    _listMrs00227Rdos.Add(rdo);
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
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_TIME_TO));
            objectTag.AddObjectData(store, "Report", _listMrs00227Rdos);
        }
    }
}
