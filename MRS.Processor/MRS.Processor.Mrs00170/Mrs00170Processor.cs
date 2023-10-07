using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisBidMedicineType;
using System;
using System.Collections.Generic;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using System.Linq;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00170
{
    internal class Mrs00170Processor : AbstractProcessor
    {
        List<VSarReportMrs00170RDO> _listSarReportMrs00170Rdos = new List<VSarReportMrs00170RDO>();
        Mrs00170Filter CastFilter;

        public Mrs00170Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00170Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00170Filter)this.reportFilter;

                // V_HIS_BID_MEDICINE_TYPE
                var metyFilterBidMedicineType = new HisBidMedicineTypeViewFilterQuery
                {
                    BID_ID = CastFilter.BID_ID
                };
                var listBidMedicineTypes = new MOS.MANAGER.HisBidMedicineType.HisBidMedicineTypeManager(paramGet).GetView(metyFilterBidMedicineType);
                // V_HIS_MEDICINE

                var listMedicines = new List<V_HIS_MEDICINE>();
                var listBidNumbers = listBidMedicineTypes.Select(s => s.BID_ID).ToList();
                var skip = 0;
                while (listBidNumbers.Count() - skip > 0)
                {
                    var listIds = listBidNumbers.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterMedicine = new HisMedicineViewFilterQuery
                    {
                        BID_IDs = listIds,
                    };
                    var listMedicine = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(metyFilterMedicine).Where(o => o.IMP_TIME != null).ToList();
                    listMedicines.AddRange(listMedicine);
                }
                // V_HIS_IMP_MEDICINE
                var listMedicineIDs = listMedicines.Select(s => s.ID).ToList();
                var ggg = string.Join(", ", listMedicineIDs);
                var listIMP_MEDIs = new List<V_HIS_IMP_MEST_MEDICINE>();
                skip = 0;
                while (listMedicineIDs.Count() - skip > 0)
                {
                    var listIds = listMedicineIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        MEDICINE_IDs = listIds,
                    };
                    var listImpMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine).Where(o => o.IMP_TIME != null).ToList();
                    listIMP_MEDIs.AddRange(listImpMestMedicineViews);
                }
                // V_HIS_EXP_MEST_MEDICINE
                var listEXP_MEDIs = new List<V_HIS_EXP_MEST_MEDICINE>();
                skip = 0;
                while (listMedicineIDs.Count() - skip > 0)
                {
                    var listIds = listMedicineIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                    var metyFilterExpMestMedicineViews = new HisExpMestMedicineViewFilterQuery
                    {
                        MEDICINE_IDs = listIds,
                        IS_EXPORT = true
                    };
                    var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicineViews).Where(o => o.EXP_TIME != null).ToList();

                    listEXP_MEDIs.AddRange(listExpMestMedicines);

                }

                ProcessFilterData(listBidMedicineTypes, listMedicines, listIMP_MEDIs, listEXP_MEDIs);

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
            dicSingleTag.Add("BID", CastFilter.BID_ID);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00170Rdos);
        }


        private void ProcessFilterData(List<V_HIS_BID_MEDICINE_TYPE> listBidMedicineTypes, List<V_HIS_MEDICINE> listMedicines, List<V_HIS_IMP_MEST_MEDICINE> listIMP_MEDIs, List<V_HIS_EXP_MEST_MEDICINE> listEXP_MEDIs)
        {
            try
            {
                listMedicines = listMedicines.OrderBy(o => o.MEDICINE_TYPE_ID).ToList();

                foreach (var medicine in listMedicines)
                {

                    var rdo = new VSarReportMrs00170RDO
                        {

                            MEDICINE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME,
                            BID_AMOUNT = listBidMedicineTypes.Where(o => o.MEDICINE_TYPE_ID == medicine.MEDICINE_TYPE_ID).First().AMOUNT,
                            IMP_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString((long)medicine.IMP_TIME),
                            IMP_MEST_CODE = listIMP_MEDIs.Where(o => o.MEDICINE_ID == medicine.ID).OrderBy(p => p.IMP_TIME).First().IMP_MEST_CODE,
                            IMP_AMOUNT = medicine.AMOUNT,
                            BID_INVENTORY = listBidMedicineTypes.Where(o => o.MEDICINE_TYPE_ID == medicine.MEDICINE_TYPE_ID).First().AMOUNT - listMedicines.Where(o => o.MEDICINE_TYPE_ID == medicine.MEDICINE_TYPE_ID).Select(p => p.AMOUNT).Sum(),
                            EXP_AMOUNT = listEXP_MEDIs.Where(o => o.MEDICINE_ID == medicine.ID && o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).Select(p => p.AMOUNT).Sum() - listIMP_MEDIs.Where(o => o.MEDICINE_ID == medicine.ID && o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH).Select(p => p.AMOUNT).Sum(),
                            BID_END_AMOUNT = medicine.AMOUNT - (listEXP_MEDIs.Where(o => o.MEDICINE_ID == medicine.ID && o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).Select(p => p.AMOUNT).Sum() - listIMP_MEDIs.Where(o => o.MEDICINE_ID == medicine.ID && o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH).Select(p => p.AMOUNT).Sum())
                        };
                    _listSarReportMrs00170Rdos.Add(rdo);

                }


            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }
}
