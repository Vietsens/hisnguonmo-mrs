using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisImpMest;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00165
{
    internal class Mrs00165Processor : AbstractProcessor
    {
        List<V_HIS_MEDICINE_TYPE> _listParentMedicine = new List<V_HIS_MEDICINE_TYPE>();
        List<VSarReportMrs00165RDO> _listSarReportMrs00165Rdos = new List<VSarReportMrs00165RDO>();
        Mrs00165Filter CastFilter;

        public Mrs00165Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00165Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00165Filter)this.reportFilter;

                var metyFilterManuImpMest = new HisImpMestViewFilterQuery
                {
                    IMP_TIME_FROM = CastFilter.DATE_FROM,
                    IMP_TIME_TO = CastFilter.DATE_TO,
                    MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID,
                    IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                };
                var listManuImpMestViews = new HisImpMestManager(paramGet).GetView(metyFilterManuImpMest);
                var listManuImpMestIds = listManuImpMestViews.Select(s => s.ID).ToList();
                //--------------------------------------------------------------------------------------------------V_HIS_IMP_MEST_MEDICINE
                var listImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>();
                var skip = 0;
                while (listManuImpMestIds.Count - skip > 0)
                {
                    var listIds = listManuImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var impMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine);
                    listImpMestMedicineViews.AddRange(impMestMedicineViews);
                }
                //--------------------------------------------------------------------------------------------------MEDICINE_TYPE               
           
                ProcessFilterData(listImpMestMedicineViews, listManuImpMestViews);
                result = true;

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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
            dicSingleTag.Add("MEDI_STOCK_ID", CastFilter.MEDI_STOCK_ID);
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00165Rdos.OrderBy(o=>o.MEDICINE_TYPE_NAME).ToList());
        }

        private void ProcessFilterData(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews, List<V_HIS_IMP_MEST> listManuImpMestViews)
        {
            try
            {

              
                    foreach (var listImpMestMedicineView in listImpMestMedicineViews)
                    {
                        var ManuImpMest = listManuImpMestViews.FirstOrDefault(o => o.ID == listImpMestMedicineView.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                            var amount = listImpMestMedicineView.AMOUNT;
                            var discount = ManuImpMest.DISCOUNT_RATIO;
                            var price = listImpMestMedicineView.PRICE;
                            var total = amount * price * (1 - discount);
                            var date = Inventec.Common.DateTime.Convert.TimeNumberToDateString(ManuImpMest.IMP_TIME.Value);
                            var number = ManuImpMest.DOCUMENT_NUMBER;
                            VSarReportMrs00165RDO rdo = new VSarReportMrs00165RDO
                            {
                                MEDICINE_TYPE_NAME = listImpMestMedicineView.MEDICINE_TYPE_NAME,
                                SERVICE_UNIT_NAME = listImpMestMedicineView.SERVICE_UNIT_NAME,
                                AMOUNT = amount,
                                DISCOUNT_RATIO = discount,
                                PRICE = price,
                                TOTAL_PRICE = total,
                                DOCUMENT_DATE = date,
                                DOCUMENT_NUMBER = number
                            };
                            _listSarReportMrs00165Rdos.Add(rdo);
                    }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
