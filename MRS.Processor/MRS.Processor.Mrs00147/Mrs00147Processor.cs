using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisExpMestStt;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineTypeAcin;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMedicineUseForm;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisExpMestReason;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00147
{
    public class Mrs00147Processor : AbstractProcessor
    {
        List<Mrs00147RDO> _listSereServRdo = new List<Mrs00147RDO>();
        List<Mrs00147RDO> _listDetailRdo = new List<Mrs00147RDO>();
        Mrs00147Filter CastFilter;
        private string MEDI_STOCK_NAME;
        private string EXP_MEST_STT_NAME;
        private string EXP_MEST_TYPE_NAME;
        private int TOTAL_ALL_ACTIVE_INGREDIENT;

        List<V_HIS_MEDICINE_TYPE_ACIN> listMedicineTypeAcinViews = new List<V_HIS_MEDICINE_TYPE_ACIN>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST> listMobaImpMestViews = new List<V_HIS_IMP_MEST>();
        List<V_HIS_MEDICINE_TYPE> listMedicineTypeViews = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineViews = new List<V_HIS_EXP_MEST_MEDICINE>();

        private Dictionary<string, int> dicActiveIngreBhytCode = new Dictionary<string, int>();
        //thêm
        List<HIS_MEDICINE> listMedicineViews = new List<HIS_MEDICINE>();
        List<HIS_IMP_SOURCE> listimpsourceViews = new List<HIS_IMP_SOURCE>();
        //
        List<HIS_EXP_MEST_REASON> listExpMestReason = new List<HIS_EXP_MEST_REASON>();
        List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
        List<HIS_EXP_MEST> listExpMestTNCC = new List<HIS_EXP_MEST>();
        //
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();

        List<PL5> listPL5 = new List<PL5>();

        public Mrs00147Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00147Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00147Filter)this.reportFilter;
                var paramGet = new CommonParam();
                LogSystem.Info("Bat dau lay du lieu filter MRS00147: " +
                    LogUtil.TraceData(LogUtil.GetMemberName(() => CastFilter), CastFilter));
                //--------------------------------------------------------------------------------------------------
                //Tên kho xuất
                if (CastFilter.MEDI_STOCK_ID != null)
                {
                    MEDI_STOCK_NAME = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetById(CastFilter.MEDI_STOCK_ID ?? 0).MEDI_STOCK_NAME;
                }
                //Trạng thái phiếu xuất
                if (CastFilter.EXP_MEST_STT_ID != null)
                {
                    EXP_MEST_STT_NAME = new MOS.MANAGER.HisExpMestStt.HisExpMestSttManager(paramGet).GetById(CastFilter.EXP_MEST_STT_ID ?? 0).EXP_MEST_STT_NAME;
                }
                //Loại xuất
                if (CastFilter.EXP_MEST_TYPE_ID != null)
                {
                    EXP_MEST_TYPE_NAME = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager(paramGet).GetById(CastFilter.EXP_MEST_TYPE_ID ?? 0).EXP_MEST_TYPE_NAME;
                }
                //--------------------------------------------------------------------------------------------------
                //Tên kho xuất
                if (CastFilter.MEDI_STOCK_NOT_BUSINESS_IDs != null)
                {
                    HisMediStockFilterQuery msfilter = new HisMediStockFilterQuery();
                    msfilter.IDs = CastFilter.MEDI_STOCK_NOT_BUSINESS_IDs;
                    var ms = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).Get(msfilter);
                    if (ms != null)
                    {
                        MEDI_STOCK_NAME = string.Join(" - ", ms.Select(o => o.MEDI_STOCK_NAME).ToList());
                    }
                }
                if (CastFilter.MEDI_STOCK_IDs != null)
                {
                    HisMediStockFilterQuery msfilter = new HisMediStockFilterQuery();
                    msfilter.IDs = CastFilter.MEDI_STOCK_IDs;
                    var ms = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).Get(msfilter);
                    if (ms != null)
                    {
                        MEDI_STOCK_NAME = string.Join(" - ", ms.Select(o => o.MEDI_STOCK_NAME).ToList());
                    }
                }

                //Loại xuất
                if (CastFilter.EXP_MEST_TYPE_IDs != null)
                {
                    HisExpMestTypeFilterQuery emtfilter = new HisExpMestTypeFilterQuery();
                    emtfilter.IDs = CastFilter.EXP_MEST_TYPE_IDs;
                    var emt = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager(paramGet).Get(emtfilter);
                    if (emt != null)
                    {
                        EXP_MEST_TYPE_NAME = string.Join(" - ", emt.Select(o => o.EXP_MEST_TYPE_NAME).ToList());
                    }
                }
                //-------------------------------------------------------------------------------------------------- EXP_MEST
                var metyFilterExpMest = new HisExpMestViewFilterQuery
                {
                    MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID,
                    MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_NOT_BUSINESS_IDs ?? CastFilter.MEDI_STOCK_IDs,
                    EXP_MEST_STT_ID = CastFilter.EXP_MEST_STT_ID,
                    EXP_MEST_TYPE_ID = CastFilter.EXP_MEST_TYPE_ID,
                    EXP_MEST_TYPE_IDs = CastFilter.EXP_MEST_TYPE_IDs
                };
                if (CastFilter.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                {
                    metyFilterExpMest.FINISH_TIME_FROM = CastFilter.DATE_FROM;
                    metyFilterExpMest.FINISH_TIME_TO = CastFilter.DATE_TO;
                }
                else
                {
                    metyFilterExpMest.CREATE_TIME_FROM = CastFilter.DATE_FROM;
                    metyFilterExpMest.CREATE_TIME_TO = CastFilter.DATE_TO;
                }
                if (CastFilter.EXP_MEST_TYPE_IDs == null)
                {
                    if (!string.IsNullOrWhiteSpace(CastFilter.EMT_LIMIT_CODE))
                    {
                        List<long> ExpMestTypeIds = (new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager(paramGet).Get(new HisExpMestTypeFilterQuery()) ?? new List<HIS_EXP_MEST_TYPE>()).Where(o => CastFilter.EMT_LIMIT_CODE.Split(',').Contains(o.EXP_MEST_TYPE_CODE)).Select(p => p.ID).ToList();
                        if (ExpMestTypeIds.Count > 0)
                        {
                            metyFilterExpMest.EXP_MEST_TYPE_IDs = ExpMestTypeIds;
                        }
                    }
                }
                var listExpMestViews = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(metyFilterExpMest);
                LogSystem.Info("listExpMestViews" + listExpMestViews.Count);
                //-------------------------------------------------------------------------------------------------- V_HIS_EXP_MEST_MEDICINE
                var listExpMestIds = listExpMestViews.Select(s => s.ID).ToList();
                var skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        SERVICE_UNIT_ID = CastFilter.SERVICE_UNIT_ID,
                        SERVICE_UNIT_IDs = CastFilter.SERVICE_UNIT_IDs
                    };
                    if (CastFilter.EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                    {
                        metyFilterExpMestMedicine.IS_EXPORT = true;
                    }
                    var expMestMedicineViews = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine);
                    listExpMestMedicineViews.AddRange(expMestMedicineViews);
                }
                if (CastFilter.MEDICINE_USE_FORM_ID != null)
                {
                    listExpMestMedicineViews = listExpMestMedicineViews.Where(o => o.MEDICINE_USE_FORM_ID == CastFilter.MEDICINE_USE_FORM_ID).ToList();
                }
                if (CastFilter.MEDICINE_USE_FORM_IDs != null)
                {
                    listExpMestMedicineViews = listExpMestMedicineViews.Where(o => CastFilter.MEDICINE_USE_FORM_IDs.Contains(o.MEDICINE_USE_FORM_ID ?? 0)).ToList();
                }
                if (CastFilter.MEDICINE_USE_FORM_ID == null && CastFilter.MEDICINE_USE_FORM_IDs == null)
                {
                    if (!string.IsNullOrWhiteSpace(CastFilter.MUF_LIMIT_CODE))
                    {
                        List<long> MedicineUseFormIds = (new MOS.MANAGER.HisMedicineUseForm.HisMedicineUseFormManager(paramGet).Get(new HisMedicineUseFormFilterQuery()) ?? new List<HIS_MEDICINE_USE_FORM>()).Where(o => CastFilter.MUF_LIMIT_CODE.Split(',').Contains(o.MEDICINE_USE_FORM_CODE)).Select(p => p.ID).ToList();
                        if (MedicineUseFormIds.Count > 0)
                        {
                            listExpMestMedicineViews = listExpMestMedicineViews.Where(o => MedicineUseFormIds.Contains(o.MEDICINE_USE_FORM_ID ?? 0)).ToList();
                        }
                    }
                }
                LogSystem.Info("listExpMestMedicineViews" + listExpMestMedicineViews.Count);

                //-------------------------------------------------------------------------------------------------- MEDICINE_TYPE
                var listMedicineTypeIds = listExpMestMedicineViews.Select(s => s.MEDICINE_TYPE_ID).Distinct().ToList();
                var listServiceReqIds = listExpMestMedicineViews.Select(s => s.TDL_SERVICE_REQ_ID ?? 0).Distinct().ToList();
                skip = 0;
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    var listReqIds = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                    var req = new HisServiceReqFilterQuery
                    {
                        IDs = listReqIds,

                    };
                    listServiceReq = new HisServiceReqManager(paramGet).Get(req);
                    listServiceReq = listServiceReq.Where(p => p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                }
                var metyFilterMedicineType = new HisMedicineTypeViewFilterQuery();
                var medicineTypeViews = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(metyFilterMedicineType);
                listMedicineTypeViews.AddRange(medicineTypeViews.Where(o => o.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS).ToList());

                foreach (V_HIS_MEDICINE_TYPE item in (from o in listMedicineTypeViews
                                                      where !string.IsNullOrEmpty(o.ACTIVE_INGR_BHYT_CODE)
                                                      select o into p
                                                      orderby p.CREATE_TIME
                                                      select p).ToList())
                {
                    if (!dicActiveIngreBhytCode.ContainsKey(item.ACTIVE_INGR_BHYT_CODE))
                    {
                        dicActiveIngreBhytCode[item.ACTIVE_INGR_BHYT_CODE] = 0;
                    }
                    dicActiveIngreBhytCode[item.ACTIVE_INGR_BHYT_CODE]++;
                }
                LogSystem.Info("listMedicineTypeViews" + listMedicineTypeViews.Count);
                //xu li lai danh sach phieu xuat va danh sach phieu xuat co thu hoi
                var expMestId = listExpMestMedicineViews.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList();
                var expMestIdMoba = listExpMestMedicineViews.Where(p => p.TH_AMOUNT > 0).Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList();
                listExpMestViews = listExpMestViews.Where(o => expMestId.Contains(o.ID)).ToList();
                var listMobaExpMestId = listExpMestViews.Where(o => expMestIdMoba.Contains(o.ID)).Select(q => q.ID).ToList();

                //dữ liệu thu hồi
                if (CastFilter.IS_MOBA_ON_TIME == true)
                {
                    GetMoba(CastFilter);
                }
                else
                {
                    GetMoba(listMobaExpMestId);
                }
                skip = 0;
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterMedicineTypeAcin = new HisMedicineTypeAcinViewFilterQuery
                    {
                        MEDICINE_TYPE_IDs = listIds
                    };
                    var medicineTypeAcinViews = new MOS.MANAGER.HisMedicineTypeAcin.HisMedicineTypeAcinManager(paramGet).GetView(metyFilterMedicineTypeAcin);
                    listMedicineTypeAcinViews.AddRange(medicineTypeAcinViews);
                }
                List<long> medicineIds = listExpMestMedicineViews.Select(o => o.MEDICINE_ID ?? 0).Distinct().ToList();
                skip = 0;
                while (medicineIds.Count - skip > 0)
                {
                    var listIds = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterMedicine = new HisMedicineFilterQuery
                    {
                        IDs = listIds,

                    };
                    var MedicineViews = new HisMedicineManager(paramGet).Get(metyFilterMedicine);
                    listMedicineViews.AddRange(MedicineViews);
                }
                HisImpSourceFilterQuery Impfilter = new HisImpSourceFilterQuery();
                listimpsourceViews = new HisImpSourceManager(paramGet).Get(Impfilter);
                //danh sách thuốc thanh lý

                listExpMestReason = new HisExpMestReasonManager(paramGet).Get(new HisExpMestReasonFilterQuery());
                var idExpMestTl = listExpMestReason.Where(x => x.EXP_MEST_REASON_NAME.ToLower().Contains("thanh lý")).Select(x => x.ID).ToList();
                var idExpMestTNCC = listExpMestReason.Where(x => x.EXP_MEST_REASON_CODE == "09").Select(x => x.ID).ToList();

                HisExpMestFilterQuery expMesstFilter = new HisExpMestFilterQuery();
                expMesstFilter.EXP_MEST_REASON_IDs = idExpMestTl;
                listExpMest = new HisExpMestManager(paramGet).Get(expMesstFilter);
                var expMestID = listExpMest.Select(o => o.ID).Distinct().ToList();

                HisExpMestFilterQuery expMesstTNCCFilter = new HisExpMestFilterQuery();
                expMesstTNCCFilter.EXP_MEST_REASON_IDs = idExpMestTNCC;
                listExpMestTNCC = new HisExpMestManager(paramGet).Get(expMesstTNCCFilter);
                var expMestTNCCID = listExpMest.Select(o => o.ID).Distinct().ToList();
                //--------------------------------------------------------------------------------------------------
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00147." +
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

        private void GetMoba(Mrs00147Filter CastFilter)
        {
            try
            {
                var metyfilterMobaImpMest = new HisImpMestViewFilterQuery
                {
                    IMP_TIME_FROM = CastFilter.DATE_FROM,
                    IMP_TIME_TO = CastFilter.DATE_TO,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT

                };
                listMobaImpMestViews = new HisImpMestManager().GetView(metyfilterMobaImpMest);
                listMobaImpMestViews = listMobaImpMestViews.Where(o => o.MOBA_EXP_MEST_ID != null).ToList();
                LogSystem.Info("listMobaImpMestViews" + listMobaImpMestViews.Count);
                var metyFilterimpMestMedicine = new HisImpMestMedicineViewFilterQuery
                {
                    IMP_TIME_FROM = CastFilter.DATE_FROM,
                    IMP_TIME_TO = CastFilter.DATE_TO,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                };
                listImpMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(metyFilterimpMestMedicine);
                listImpMestMedicineViews = listImpMestMedicineViews.Where(o => o.TH_EXP_MEST_MEDICINE_ID != null).ToList();
                LogSystem.Info("listImpMestMedicineViews" + listImpMestMedicineViews.Count);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void GetMoba(List<long> listMobaExpMestId)
        {
            try
            {
                var skip = 0;
                while (listMobaExpMestId.Count - skip > 0)
                {
                    var listIds = listMobaExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyfilterMobaImpMest = new HisImpMestViewFilterQuery
                    {
                        MOBA_EXP_MEST_IDs = listIds
                    };
                    var mobaImpMestViews = new HisImpMestManager().GetView(metyfilterMobaImpMest);
                    listMobaImpMestViews.AddRange(mobaImpMestViews);
                }
                LogSystem.Info("listMobaImpMestViews" + listMobaImpMestViews.Count);
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds = listMobaImpMestViews.Select(s => s.ID).ToList();
                skip = 0;
                while (listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterimpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds
                    };
                    var impMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(metyFilterimpMestMedicine);
                    listImpMestMedicineViews.AddRange(impMestMedicineViews);
                }
                LogSystem.Info("listImpMestMedicineViews" + listImpMestMedicineViews.Count);
                //-------------------------------------------------------------------------------------------------- V_HIS_MEDICINE_TYPE_ACIN
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        protected override bool ProcessData()
        {
            var result = false;
            try
            {
                ProcessFilterData(listExpMestMedicineViews, listMedicineTypeViews, listImpMestMedicineViews, listMedicineTypeAcinViews);
                //ProcessDetailData(listExpMest, listExpMestMedicineViews, listImpMestMedicineViews);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines, List<V_HIS_MEDICINE_TYPE> listMedicineTypes,
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines, List<V_HIS_MEDICINE_TYPE_ACIN> listMedicineTypeAcins)
        {
            try
            {
                var listExpMestMedicineViews = listExpMestMedicines.Where(s => s.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS /*s.IS_ANTIBIOTIC==1*/).OrderBy(s => s.MEDICINE_TYPE_NAME).ToList();
                var listMedicineTypeIds = listExpMestMedicineViews.Select(s => s.MEDICINE_TYPE_ID).ToList();
                TOTAL_ALL_ACTIVE_INGREDIENT = listMedicineTypeAcins.Where(s => listMedicineTypeIds.Contains(s.MEDICINE_TYPE_ID)).Distinct().Count();

                var listNewExpMestMedicineGroupByMedicineTypeIds = (from listExpMestMedicineView in listExpMestMedicineViews
                                                                    let medicineType = listMedicineTypes.First(s => s.ID == listExpMestMedicineView.MEDICINE_TYPE_ID)
                                                                    select new NewExpMestMedicineViews
                                                                    {
                                                                        V_HIS_EXP_MEST_MEDICINE = listExpMestMedicineView,
                                                                        NATIONAL_NAME = medicineType.NATIONAL_NAME,
                                                                        MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME,
                                                                        MANUFACTURER_ID = medicineType.MANUFACTURER_ID,
                                                                        MEDICINE_TYPE_ID = listExpMestMedicineView.MEDICINE_TYPE_ID,
                                                                        ACTIVE_INGR_BHYT_CODE = medicineType.ACTIVE_INGR_BHYT_CODE,
                                                                        ACTIVE_INGR_BHYT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME,
                                                                        HEIN_SERVICE_BHYT_CODE = medicineType.HEIN_SERVICE_BHYT_CODE,
                                                                        HEIN_SERVICE_BHYT_NAME = medicineType.HEIN_SERVICE_BHYT_NAME,
                                                                        ATC_CODES = medicineType.ATC_CODES,
                                                                        MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.MEDICINE_TYPE_PROPRIETARY_NAME,
                                                                        CONCENTRA = medicineType.CONCENTRA,
                                                                        SERVICE_REQ_ID = listExpMestMedicineView.TDL_SERVICE_REQ_ID,
                                                                        EXP_MEST_TYPE = listExpMestMedicineView.EXP_MEST_TYPE_ID
                                                                    }).GroupBy(s => s.MEDICINE_TYPE_ID).ToList();
                LogSystem.Info("listNewExpMestMedicineGroupByMedicineTypeIds" + listNewExpMestMedicineGroupByMedicineTypeIds.Count);
                foreach (var listNewExpMestMedicineGroupByMedicineTypeId in listNewExpMestMedicineGroupByMedicineTypeIds)
                {
                    var listNewExpMestMedicineGroupByManufacturers = listNewExpMestMedicineGroupByMedicineTypeId.GroupBy(s => s.MANUFACTURER_ID).ToList();
                    foreach (var listNewExpMestMedicineGroupByManufacturer in listNewExpMestMedicineGroupByManufacturers)
                    {
                        var listNewExpMestMedicineGroupByNationals = listNewExpMestMedicineGroupByManufacturer.GroupBy(s => s.NATIONAL_NAME).ToList();
                        foreach (var listNewExpMestMedicineGroupByNational in listNewExpMestMedicineGroupByNationals)
                        {
                            var listNewExpMestMedicineGroupByImpPrices = listNewExpMestMedicineGroupByNational.GroupBy(s => s.V_HIS_EXP_MEST_MEDICINE.IMP_PRICE * (1 + s.V_HIS_EXP_MEST_MEDICINE.IMP_VAT_RATIO)).ToList();
                            foreach (var listNewExpMestMedicineGroupByImpPrice in listNewExpMestMedicineGroupByImpPrices)
                            {
                                //đơn giá
                                var price = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.IMP_PRICE * (1 + listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.IMP_VAT_RATIO);
                                //số lượng thuốc đã thu hồi
                                var listMedicineTypeIdFromExpMestMedicines = listNewExpMestMedicineGroupByImpPrice.Select(s => s.V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_ID);
                                var impMestMedicine = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID).Sum(s => s.AMOUNT);
                                //số lượng thuốc đã xuất
                                var expMestMedicine = listNewExpMestMedicineGroupByImpPrice.Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);

                                //số lượng thuốc đã xuất theo loại xuất và theo lý do xuất 

                                var pl = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                var khac = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                var hpkp = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                var dtt = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                var dpk = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                var ddt = listNewExpMestMedicineGroupByImpPrice.Where(p => p.EXP_MEST_TYPE == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);

                                var dnttl = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID && s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL).Sum(s => s.AMOUNT);
                                var dtttl = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID && s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL).Sum(s => s.AMOUNT);
                                var hptl = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID && s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL).Sum(s => s.AMOUNT);
                                var tht = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID && s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT).Sum(s => s.AMOUNT);
                                var donktl = listImpMestMedicines.Where(s => listMedicineTypeIdFromExpMestMedicines.Contains(s.MEDICINE_TYPE_ID) &&
                                   Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                    s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                    s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID && s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL).Sum(s => s.AMOUNT);

                                var expMestMedicineByExpReason = listExpMestTNCC.Where(x => listNewExpMestMedicineGroupByImpPrice.Select(y => y.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_ID).ToList().Contains(x.ID)).Select(x => x.ID).ToList();
                                var amountTNCC = listExpMestMedicines.Where(x => expMestMedicineByExpReason.Contains(x.EXP_MEST_ID ?? 0)).Sum(x => x.AMOUNT);

                                //số lượng thuốc đã sử dụng
                                var amountUser = expMestMedicine - impMestMedicine;
                                //số lượng thanh lý
                                var expMestIds = listExpMest.Where(x => listNewExpMestMedicineGroupByImpPrice.Select(y => y.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_ID).ToList().Contains(x.ID)).Select(x => x.ID).ToList();
                                var amountTl = listExpMestMedicines.Where(x => expMestIds.Contains(x.EXP_MEST_ID ?? 0)).Sum(x => x.AMOUNT);

                                var dicExpTypeAmount = listNewExpMestMedicineGroupByImpPrice.GroupBy(g => g.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_TYPE_ID).ToDictionary(p => p.Key, q => q.Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT));

                                //số lượng xuất theo công thức 105_NhapXuatTonCongThuc = số lượng xuất (không lấy bù cơ số + bù lẻ + trả ncc) - số lượng nhập trả lại - số lượng thanh lý
                                var exp = pl + khac + hpkp + dtt + dpk + ddt;
                                var imp = dnttl + dtttl + hptl + tht + donktl;
                                var rdo = new Mrs00147RDO
                                {
                                    MEDICINE_TYPE_CODE = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_CODE,//Mã thuốc
                                    MEDICINE_TYPE_NAME = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_NAME,//Tên thuốc
                                    SERVICE_UNIT_CODE = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.SERVICE_UNIT_CODE,//Mã đơn vị tính
                                    SERVICE_UNIT_NAME = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.SERVICE_UNIT_NAME,//Tên đơn vị tính
                                    MEDICINE_USE_FORM_CODE = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_USE_FORM_CODE,//Mã đường dùng
                                    MEDICINE_USE_FORM_NAME = listNewExpMestMedicineGroupByImpPrice.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_USE_FORM_NAME,//Tên đường dùng
                                    ACTIVE_INGR_BHYT_CODE = listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE,//Mã hoạt chất
                                    ACTIVE_INGR_BHYT_NAME = listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_NAME,//Tên hoạt chất
                                    HEIN_SERVICE_BHYT_CODE = listNewExpMestMedicineGroupByImpPrice.First().HEIN_SERVICE_BHYT_CODE,
                                    HEIN_SERVICE_BHYT_NAME = listNewExpMestMedicineGroupByImpPrice.First().HEIN_SERVICE_BHYT_NAME,
                                    MANUFACTURER_NAME = listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_NAME,//Tên hãng sản xuất
                                    NATIONAL_NAME = listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME,//Nước sản xuất
                                    ATC_CODES = listNewExpMestMedicineGroupByImpPrice.First().ATC_CODES,//Nước sản xuất
                                    TT_BIET_DUOC = (dicActiveIngreBhytCode.ContainsKey(listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE ?? "") ? ((listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE ?? "") + "." + dicActiveIngreBhytCode[listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE ?? ""]) : null),
                                    MEDICINE_TYPE_PROPRIETARY_NAME = listNewExpMestMedicineGroupByImpPrice.First().MEDICINE_TYPE_PROPRIETARY_NAME,//Nước sản xuất
                                    CONCENTRA = listNewExpMestMedicineGroupByImpPrice.First().CONCENTRA,//Nước sản xuất
                                    //Thêm nguồn nhập
                                    IMP_SOURCE_NAME = ProcessImpSource(listNewExpMestMedicineGroupByImpPrice.First()),//Nguồn nhập
                                    PRICE = price,//Đơn giá
                                    AMOUNT_USED = amountUser,//Số lượng đã sử dụng
                                    DIC_EXP_TYPE_AMOUNT = dicExpTypeAmount != null ? dicExpTypeAmount : new Dictionary<long, decimal>(),
                                    TOTAL_PRICE = price * amountUser,//Thành tiền
                                    AMOUNT_TL = amountTl,
                                    AMOUNT_EXP_NEW = exp - imp - amountTl - amountTNCC,
                                    AMOUNT_EXP = exp,
                                    AMOUNT_KHAC_TNCC = amountTNCC,
                                    AMOUNT_IMP = imp,
                                    TOTAL_PRICE_NEW = price * (exp - imp - amountTl - amountTNCC)
                                };
                                _listSereServRdo.Add(rdo);

                                var listNewExpMestMedicineGroupByReqDepartments = listNewExpMestMedicineGroupByImpPrice.GroupBy(s => s.SERVICE_REQ_ID).ToList();
                                foreach (var listNewExpMestMedicineGroupByReqDepartment in listNewExpMestMedicineGroupByReqDepartments)
                                {
                                    var price1 = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.IMP_PRICE * (1 + listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.IMP_VAT_RATIO);
                                    //số lượng thuốc đã thu hồi
                                    var listMedicineTypeIdFromReqDepartment = listNewExpMestMedicineGroupByReqDepartment.Select(s => s.V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_ID);
                                    var impMestMedicine1 = listImpMestMedicines.Where(s => listMedicineTypeIdFromReqDepartment.Contains(s.MEDICINE_TYPE_ID) &&
                                       Math.Round(s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) == (Math.Round(price)) &&
                                        s.NATIONAL_NAME == listNewExpMestMedicineGroupByImpPrice.First().NATIONAL_NAME &&
                                        s.MANUFACTURER_ID == listNewExpMestMedicineGroupByImpPrice.First().MANUFACTURER_ID &&
                                        (s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL || s.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL)).Sum(s => s.AMOUNT);
                                    //số lượng thuốc đã xuất
                                    var expMestMedicine1 = listNewExpMestMedicineGroupByReqDepartment.Where(p => p.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT || p.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT).Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT);
                                    var dicExpTypeAmount1 = listNewExpMestMedicineGroupByReqDepartment.GroupBy(g => g.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_TYPE_ID).ToDictionary(p => p.Key, q => q.Sum(s => s.V_HIS_EXP_MEST_MEDICINE.AMOUNT));

                                    //số lượng thuốc đã sử dụng
                                    var amountUser1 = expMestMedicine1 - impMestMedicine1;
                                    //số lượng thanh lý
                                    var expMestIds1 = listExpMest.Where(x => listNewExpMestMedicineGroupByReqDepartment.Select(y => y.V_HIS_EXP_MEST_MEDICINE.EXP_MEST_ID).ToList().Contains(x.ID)).Select(x => x.ID).ToList();
                                    var amountTl1 = listExpMestMedicines.Where(x => expMestIds1.Contains(x.EXP_MEST_ID ?? 0)).Sum(x => x.AMOUNT);

                                    var serviceReq = listServiceReq.FirstOrDefault(p => p.ID == listNewExpMestMedicineGroupByReqDepartment.First().SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();

                                    var rdo1 = new Mrs00147RDO
                                    {
                                        EXP_MEST_TYPE_ID = listNewExpMestMedicineGroupByReqDepartment.First().EXP_MEST_TYPE,
                                        TREATMENT_TYPE_ID = serviceReq.TDL_TREATMENT_TYPE_ID,
                                        REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(P => P.ID == serviceReq.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME,
                                        MEDICINE_TYPE_CODE = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_CODE,//Mã thuốc
                                        MEDICINE_TYPE_NAME = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_TYPE_NAME,//Tên thuốc
                                        SERVICE_UNIT_CODE = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.SERVICE_UNIT_CODE,//Mã đơn vị tính
                                        SERVICE_UNIT_NAME = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.SERVICE_UNIT_NAME,//Tên đơn vị tính
                                        MEDICINE_USE_FORM_CODE = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_USE_FORM_CODE,//Mã đường dùng
                                        MEDICINE_USE_FORM_NAME = listNewExpMestMedicineGroupByReqDepartment.First().V_HIS_EXP_MEST_MEDICINE.MEDICINE_USE_FORM_NAME,//Tên đường dùng
                                        ACTIVE_INGR_BHYT_CODE = listNewExpMestMedicineGroupByReqDepartment.First().ACTIVE_INGR_BHYT_CODE,//Mã hoạt chất
                                        ACTIVE_INGR_BHYT_NAME = listNewExpMestMedicineGroupByReqDepartment.First().ACTIVE_INGR_BHYT_NAME,//Tên hoạt chất
                                        HEIN_SERVICE_BHYT_CODE = listNewExpMestMedicineGroupByReqDepartment.First().HEIN_SERVICE_BHYT_CODE,
                                        HEIN_SERVICE_BHYT_NAME = listNewExpMestMedicineGroupByReqDepartment.First().HEIN_SERVICE_BHYT_NAME,
                                        MANUFACTURER_NAME = listNewExpMestMedicineGroupByReqDepartment.First().MANUFACTURER_NAME,//Tên hãng sản xuất
                                        NATIONAL_NAME = listNewExpMestMedicineGroupByReqDepartment.First().NATIONAL_NAME,//Nước sản xuất
                                        ATC_CODES = listNewExpMestMedicineGroupByReqDepartment.First().ATC_CODES,//Nước sản xuất
                                        TT_BIET_DUOC = (dicActiveIngreBhytCode.ContainsKey(listNewExpMestMedicineGroupByReqDepartment.First().ACTIVE_INGR_BHYT_CODE ?? "") ? ((listNewExpMestMedicineGroupByImpPrice.First().ACTIVE_INGR_BHYT_CODE ?? "") + "." + dicActiveIngreBhytCode[listNewExpMestMedicineGroupByReqDepartment.First().ACTIVE_INGR_BHYT_CODE ?? ""]) : null),
                                        MEDICINE_TYPE_PROPRIETARY_NAME = listNewExpMestMedicineGroupByReqDepartment.First().MEDICINE_TYPE_PROPRIETARY_NAME,//Nước sản xuất
                                        CONCENTRA = listNewExpMestMedicineGroupByReqDepartment.First().CONCENTRA,//Nước sản xuất
                                        //Thêm nguồn nhập
                                        IMP_SOURCE_NAME = ProcessImpSource(listNewExpMestMedicineGroupByReqDepartment.First()),//Nguồn nhập
                                        PRICE = price1,//Đơn giá
                                        AMOUNT_USED = amountUser1,//Số lượng đã sử dụng
                                        DIC_EXP_TYPE_AMOUNT = dicExpTypeAmount1 != null ? dicExpTypeAmount1 : new Dictionary<long, decimal>(),
                                        TOTAL_PRICE = price1 * amountUser1,//Thành tiền
                                        AMOUNT_EXP = expMestMedicine1,
                                        AMOUNT_IMP = impMestMedicine1,
                                        AMOUNT_TL = amountTl1
                                    };
                                    _listDetailRdo.Add(rdo1);
                                }


                            }
                        }
                    }
                    LogSystem.Info("_listSereServRdo" + _listSereServRdo.Count);
                    LogSystem.Info("_listDetailRdo" + _listDetailRdo.Count);
                }
                if (CastFilter.IS_ALL_ACTIVE_INGR_BHYT == true)
                {
                    foreach (var item in listMedicineTypeViews.Where(o => o.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__KS).ToList())
                    {
                        var rdo = new Mrs00147RDO
                        {
                            MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE,//Mã thuốc
                            MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME,//Tên thuốc
                            SERVICE_UNIT_CODE = item.SERVICE_UNIT_CODE,//Mã đơn vị tính
                            SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME,//Tên đơn vị tính
                            MEDICINE_USE_FORM_CODE = item.MEDICINE_USE_FORM_CODE,//Mã đường dùng
                            MEDICINE_USE_FORM_NAME = item.MEDICINE_USE_FORM_NAME,//Tên đường dùng
                            ACTIVE_INGR_BHYT_CODE = item.ACTIVE_INGR_BHYT_CODE,//Mã hoạt chất
                            ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME,//Tên hoạt chất
                            HEIN_SERVICE_BHYT_CODE = item.HEIN_SERVICE_BHYT_CODE,
                            HEIN_SERVICE_BHYT_NAME = item.HEIN_SERVICE_BHYT_NAME,
                            MANUFACTURER_NAME = item.MANUFACTURER_NAME,//Tên hãng sản xuất
                            NATIONAL_NAME = item.NATIONAL_NAME,//Nước sản xuất
                            ATC_CODES = item.ATC_CODES,//Nước sản xuất
                            TT_BIET_DUOC = (dicActiveIngreBhytCode.ContainsKey(item.ACTIVE_INGR_BHYT_CODE ?? "") ? ((item.ACTIVE_INGR_BHYT_CODE ?? "") + "." + dicActiveIngreBhytCode[item.ACTIVE_INGR_BHYT_CODE ?? ""]) : null),
                            MEDICINE_TYPE_PROPRIETARY_NAME = item.MEDICINE_TYPE_PROPRIETARY_NAME,//Nước sản xuất
                            CONCENTRA = item.CONCENTRA,//Nước sản xuất
                            //Thêm nguồn nhập
                            //IMP_SOURCE_NAME = ProcessImpSource(item),//Nguồn nhập
                            //PRICE = price,//Đơn giá
                            //AMOUNT_USED = amountUser,//Số lượng đã sử dụng
                            //TOTAL_PRICE = price * amountUser//Thành tiền
                        };
                        if (!_listSereServRdo.Exists(o => o.MEDICINE_TYPE_CODE == item.MEDICINE_TYPE_CODE))
                        {
                            _listSereServRdo.Add(rdo);
                        }
                    }
                }
                if (CastFilter.IS_THROW_ATC_NULL == true)
                {
                    _listSereServRdo = _listSereServRdo.Where(o => !string.IsNullOrWhiteSpace(o.ATC_CODES)).ToList();
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00147 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        //private void ProcessDetailData(List<HIS_EXP_MEST> listExpMest, List<V_HIS_EXP_MEST_MEDICINE> listExpMedicine, List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine)
        //{

        //    if (listExpMedicine != null)
        //    {
        //        var group = listExpMedicine.Where(p => p.).GroupBy(p => new { p.REQ_DEPARTMENT_ID, p.MEDICINE_TYPE_ID, p.PRICE }).ToList();
        //        foreach (var item in group)
        //        {
        //            List<V_HIS_EXP_MEST_MEDICINE> listSub = item.ToList<V_HIS_EXP_MEST_MEDICINE>();
        //            var expMest = listExpMest.Where(p => p.ID == listSub[0].EXP_MEST_ID);
        //            var impMest = listImpMestMedicine.Where(p => p.REQ_DEPARTMENT_ID == listSub[0].REQ_DEPARTMENT_ID && p.MEDICINE_TYPE_ID == listSub[0].MEDICINE_TYPE_ID);
        //            Mrs00147RDO rdo = new Mrs00147RDO();

        //        }
        //    }
        //}

        private string ProcessImpSource(NewExpMestMedicineViews expMestMedicine)
        {
            if (expMestMedicine == null)
                return null;
            if (expMestMedicine.V_HIS_EXP_MEST_MEDICINE == null)
                return null;
            long medicine_id = expMestMedicine.V_HIS_EXP_MEST_MEDICINE.MEDICINE_ID ?? 0;
            var medicine = listMedicineViews.FirstOrDefault(o => o.ID == medicine_id);
            if (medicine == null)
                return null;
            return (listimpsourceViews.FirstOrDefault(o => o.ID == medicine.IMP_SOURCE_ID) ?? new HIS_IMP_SOURCE()).IMP_SOURCE_NAME;
            return null;
        }

        private class NewExpMestMedicineViews
        {
            public V_HIS_EXP_MEST_MEDICINE V_HIS_EXP_MEST_MEDICINE { get; set; }

            public string MANUFACTURER_NAME { get; set; }

            public long? MANUFACTURER_ID { get; set; }

            public string NATIONAL_NAME { get; set; }

            public long MEDICINE_TYPE_ID { get; set; }

            public string ACTIVE_INGR_BHYT_NAME { get; set; }

            public string ACTIVE_INGR_BHYT_CODE { get; set; }
            public string ATC_CODES { get; set; }
            public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
            public string CONCENTRA { get; set; }//hàm lượng nồng độ


            public long? SERVICE_REQ_ID { get; set; }

            public long EXP_MEST_TYPE { get; set; }

            public string HEIN_SERVICE_BHYT_NAME { get; set; }

            public string HEIN_SERVICE_BHYT_CODE { get; set; }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
                dicSingleTag.Add("EXP_MEST_STT_NAME", EXP_MEST_STT_NAME);
                dicSingleTag.Add("EXP_MEST_TYPE_NAME", EXP_MEST_TYPE_NAME);
                dicSingleTag.Add("TOTAL_ALL_PRICE", _listSereServRdo.Sum(s => s.TOTAL_PRICE).ToString());
                dicSingleTag.Add("TOTAL_ALL_PRICE_NEW", _listSereServRdo.Sum(s => s.TOTAL_PRICE_NEW).ToString());
                dicSingleTag.Add("TOTAL_ACTIVE_INGREDIENT", _listSereServRdo.Where(s => !string.IsNullOrEmpty(s.ACTIVE_INGR_BHYT_CODE)).Select(s => s.ACTIVE_INGR_BHYT_CODE).Distinct().Count());
                dicSingleTag.Add("TOTAL_ALL_ACTIVE_INGREDIENT", TOTAL_ALL_ACTIVE_INGREDIENT);
                objectTag.AddObjectData(store, "Report", _listSereServRdo);
                objectTag.AddObjectData(store, "Report1", _listDetailRdo.Where(p => !string.IsNullOrEmpty(p.REQ_DEPARTMENT_NAME) && p.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).OrderBy(p => p.MEDICINE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "MedicineTypes", listMedicineTypeViews);
                CreateObjectPL5();
                AddDataToObjectPL5();
                objectTag.AddObjectData(store, "PL5s", listPL5);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddDataToObjectPL5()
        {
            foreach (var item in _listSereServRdo)
            {
                if (item.AMOUNT_USED <= 0) continue;
                PL5 rdo = listPL5.FirstOrDefault(o => o.ACTIVE_INGR_BHYT_NAME == item.ACTIVE_INGR_BHYT_NAME && o.ATC_CODES == item.ATC_CODES && o.CONCENTRA == item.CONCENTRA && o.MEDICINE_USE_FORM_NAME == item.MEDICINE_USE_FORM_NAME && o.SERVICE_UNIT_NAME == item.SERVICE_UNIT_NAME);
                if (rdo == null)
                {
                    rdo = new PL5();
                    listPL5.Add(rdo);
                }
                rdo.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
                rdo.HEIN_SERVICE_BHYT_NAME = item.HEIN_SERVICE_BHYT_NAME;
                rdo.ATC_CODES = item.ATC_CODES;
                rdo.NATIONAL_NAME = item.NATIONAL_NAME;
                rdo.CONCENTRA = item.CONCENTRA;
                rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                rdo.MEDICINE_USE_FORM_NAME = item.MEDICINE_USE_FORM_NAME;
                rdo.AMOUNT_USED = item.AMOUNT_USED;
                rdo.PRICE = item.PRICE;
                rdo.AT_NUM_ORDER = rdo.AT_NUM_ORDER == 0 ? 86 : rdo.AT_NUM_ORDER;
            }
        }
        private void CreateObjectPL5()
        {
            string strPL5 = "1|Amikacin|J01GB06|1.1|250mg|Lọ|Tiêm_|||1.2|500mg|Lọ|Tiêm_2|Amoxicilin|J01CA04|2.1|250mg|Viên|Uống_|||2.2|500mg|Viên|Uống_|||2.3|125mg|Gói|Uống_|||2.4|250mg|Gói|Uống_|||2.5|125mg/5ml x 60ml|Lọ|Uống_|||2.6|250mg/5ml x 60ml|Lọ|Uống_|||2.7|125mg/5ml x 100ml|Lọ|Uống_|||2.8|250mg/5ml x 100ml|Lọ|Uống_|||2.9|1000mg|Lọ|Tiêm_3|Amoxicilin/clavulanat|J01CR02|3.1|100mg/25mg|Viên|Uống_|||3.2|200mg/50mg|Viên|Uống_|||3.3|250mg/125mg|Viên|Uống_|||3.4|500mg/125mg|Viên|Uống_|||3.5|875mg/125mg|Viên|Uống_|||3.6|250mg/31,25mg|Gói|Uống_|||3.7|250mg/62,5mg|Gói|Uống_|||3.8|500mg/62,5mg|Gói|Uống_|||3.9|875mg/125mg|Gói|Uống_|||3.10|1000mg/125mg|Gói|Uống_|||3.11|125mg/31,25mg/5ml x 70ml|Lọ|Uống_|||3.12|200mg/28,5mg/5ml x 70ml|Lọ|Uống_|||3.13|250mg/62,5mg/5ml x 100ml|Lọ|Uống_|||3.14|301,35mg/157,05mg/5ml x  100ml|Lọ|Uống_|||3.15|400mg/57mg/5ml x 70ml|Lọ|Uống_|||3.16|500mg/100mg|Lọ|Tiêm_|||3.17|1000mg/200mg|Lọ|Tiêm_4|Amoxicilin/sulbactam|J01CR02|4.1|250mg/250mg|Viên|Uống_|||4.2|500mg/500mg|Viên|Uống_|||4.3|125mg/125mg/5ml x 60ml|Lọ|Uống_|||4.4|250mg/250mg/5ml x 60ml|Lọ|Uống_|||4.5|500mg/250mg|Lọ|Tiêm_|||4.6|1000mg/500mg|Lọ|Tiêm_5|Amoxicilin/cloxaciln|J01CR50|5.1|250mg/250mg|Viên|Uống_6|Ampicilin|J01CA01|6.1|250mg|Viên|Uống_|||6.2|500mg|Viên|Uống_|||6.3|1000mg|Lọ|Tiêm_7|Ampicilin/sulbactam|J01CR04|7.1|375mg (sultamicilin)|Viên|Uống_|||7.2|250mg (sultamicilin)/5ml x 30ml|Lọ|Uống_||J01CR01|7.3|500mg/250mg|Lọ|Tiêm_|||7.4|1000mg/500mg|Lọ|Tiêm_|||7.5|2000mg/1000mg|Lọ|Tiêm_8|Ampicilin/cloxacilin|J01CR50|8.1|250mg/250mg|Viên|Uống_9|Azithromycin|J01FA10|9.1|100mg|Viên|Uống_|||9.2|250mg|Viên|Uống_|||9.3|500mg|Viên|Uống_|||9.4|100mg|Gói|Uống_|||9.5|200mg|Gói|Uống_|||9.6|200mg/5ml x 15ml|Lọ|Uống_|||9.7|200mg|Lọ|Tiêm_|||9.8|500mg|Lọ|Tiêm_10|Benzathin benzylpenicilin|J01CE08|10.1|1.200.000UI|Lọ|Tiêm_11|Benzylpenicilin|J01CE01|11.1|1.000.000UI|Lọ|Tiêm_12|Cefaclor|J01DC04|12.1|250mg|Viên|Uống_|||12.2|375mg|Viên|Uống_|||12.3|500mg|Viên|Uống_|||12.4|125mg|Gói|Uống_| ||12.5|250mg|Gói|Uống_|||12.6|125mg/5ml x 30ml|Lo|Uống_|||12.7|125mg/5ml x 60ml|Lọ|Uống_13|Cefadroxil|J01DB05|13.1|125mg|Viên|Uống_|||13.2|250mg|Viên|Uống_|||13.3|500mg|Viên|Uống_|||13.4|1000mg|Viên|Uống_|||13.5|250mg|Gói|Uống_|||13.6|125mg/5ml x 30ml|Lọ|Uống_|||13.7|125mg/5ml x 60ml|Lọ|Uống_ |||13.8|250mg/5ml x 30ml|Lọ|Uống_|||13.9|250mg/5ml x 60ml|Lọ|Uống_14|Cefalexin|J01DB01|14.1|250mg|Viên|Uống_|||14.2|500mg|Viên|Uống_|||14.3|125mg|Gói|Uống_|||14.4|250mg|Gói|Uống_|||14.5|250mg/5ml x 60ml|Lọ|Uống_15|Cefalotin|J01DB03|15.1|500mg|Lọ|Tiêm_|||15.2|1000mg|Lọ|Tiêm_16|Cefamandol|J01DC03|16.1|1000mg|Lọ|Tiêm_17|Cefatrizin|J01DB07|17.1|250mg|Viên|Uống_18|Cefazedon|J01DB06|18.1|1000mg|Lọ|Tiêm_19|Cefazolin|J01DB04|19.1|1000mg|Lọ|Tiêm_20|Cefdinir|J01DD15|20.1|100mg|Viên|Uống_|||20.2|125mg|Viên|Uống_|||20.3|300mg|Viên|Uống_|||20.4|125mg/5ml x 30ml|Lọ|Uống_21|Cefepim|J01DE01|21.1|500mg|Lọ|Tiêm_|||21.2|1000mg|Lọ|Tiêm_|||21.3|2000mg|Lọ|Tiêm_22|Cefetamet|J01DD10|22.1|250mg|Viên|Uống_|||22.2|500mg|Viên|Uống_23|Cefixim|J01DD08|23.1|100mg|Viên|Uống_|||23.2|200mg|Viên|Uống_|||23.3|50mg|Gói|Uống_|||23.4|100mg|Gói|Uống_|||23.5|50mg/5ml x 30ml|Lọ|Uống_|||23.6|50mg/5ml x 60ml|Lọ|Uống_|||23.7|100mg/5ml x 30ml|Lọ|Uống_|||23.8|100mg/5ml x 60ml|Lọ|Uống_24|Cefmetazol|J01DC09|24.1|1000mg|Lọ|Tiêm_25|Cefoperazon|J01DD12|25.1|500mg|Lọ|Tiêm_|||25.2|1000mg|Lọ|Tiêm_|||25.3|2000mg|Lọ|Tiêm_26|Cefoperazon/sulbactam|J01DD62|26.1|500mg/500mg|Lọ|Tiêm_|||26.2|1000mg/500mg|Lọ|Tiêm_|||26.3|1000mg/1000mg|Lọ|Tiêm_27|Cefotaxim|J01DD01|27.1|250mg|Lọ|Tiêm_|||27.2|500mg|Lọ|Tiêm_|||27.3|1000mg|Lọ|Tiêm_28|Cefotetan|J01DC05|28.1|1000mg|Lọ|Tiêm_29|Cefotiam|J01DC07|29.1|1000mg|Lọ|Tiêm_30|Cefozidim|J01DD09|30.1|1000mg|Lọ|Tiêm_31|Cefpirom|J01DE02|31.1|1000mg|Lọ|Tiêm_32|Cefpodoxim|J01DD13|32.1|100mg|Viên|Uống_|||32.2|200mg|Viên|Uống_|||32.3|100mg|Gói|Uống_|||32.4|40mg/5ml x 50ml|Lọ|Uống_|||32.5|50mg/5ml x 30ml |Lọ|Uống_|||32.6|50mg/5ml x 60ml|Lọ|Uống_|||32.7|100mg/5ml x 30ml|Lọ|Uống_|||32.8|100mg/5ml x 60ml|Lọ|Uống_33|Cefradin|J01DB09|33.1|250mg|Viên|Uống_|||33.2|500mg|Viên|Uống_|||33.3|125mg/5ml x 100ml|Lọ|Uống_|||33.4|500mg|Lọ|Tiêm_|||33.5|1000mg|Lọ|Tiêm_34|Ceftazidim|J01DD02|34.1|500mg|Lọ|Tiêm_|||34.2|1000mg|Lọ|Tiêm_35|Ceftezol|J01DB12|35.1|1000mg|Lọ|Tiêm_36|Ceftizoxim|J01DD07|36.1|1000mg|Lọ|Tiêm_37|Ceftriaxon|J01DD04|37.1|250mg|Lọ|Tiêm_|||37.2|500mg|Lọ|Tiêm_|||37.3|1000mg|Lọ|Tiêm_38|Ceftriaxon/sulbactam|J01DD54|38.1|1000mg/500mg|Lọ|Tiêm_39|Ceftriaxon/tazobactam|J01DD54|39.1|500mg/62,5mg|Lọ|Tiêm_|||39.2|1000mg/125mg|Lọ|Tiêm_40|Cefuroxim|J01DC02|40.1|125mg|Viên|Uống_|||40.2|250mg|Viên|Uống_|||40.3|500mg|Viên|Uống_|||40.4|125mg|Gói|Uống_|||40.5|125mg/5ml x 50ml|Lọ|Uống_|||40.6|250mg|Lọ|Tiêm_|||40.7|750mg|Lọ|Tiêm_|||40.8|1500mg|Lọ|Tiêm_41|Ciprofloxacin|J01MA02|41.1|500mg|Viên|Uống_|||41.2|200mg|Lọ/Túi|Tiêm_42|Clarithromycin|J01FA09|42.1|250mg|Viên|Uống_|||42.2|500mg|Viên|Uống_|||42.3|125mg|Gói|Uống_|||42.4|125mg/5ml x 30ml|Lọ|Uống_|||42.5|125mg/5ml x 50ml|Lọ|Uống_|||42.6|125mg/5ml x 60ml|Lọ|Uống_|||42.7|125mg/5ml x 100ml|Lọ|Uống_43|Clindamycin|J01FF01|43.1|150mg|Viên|Uống_|||43.2|300mg|Viên|Uống_|||43.3|300mg|Lọ|Tiêm_|||43.4|600mg|Lọ|Tiêm_44|Cloramphenicol|J01BA01|44.1|250mg|Viên|Uống_|||44.2|1000mg|Lọ|Tiêm_45|Cloxacilin|J01CF02|45.1|250mg|Viên|Uống_|||45.2|500mg|Viên|Uống_|||45.3|500mg|Lọ|Tiêm_46|Colistin|J01XB01|46.1|1.000.000UI|Lọ|Tiêm_47|Doxycyclin|J01AA02|47.1|100mg|Viên|Uống_48|Ertapenem|J01DH03|48.1|1000mg|Lọ|Tiêm_49|Erythromycin|J01FA01|49.1|250mg|Viên|Uống_|||49.2|500mg|Viên|Uống_|||49.3|250mg|Gói|Uống_|||49.4|100mg/2,5ml x 30ml|Lọ|Uống_50|Fosfomycin|J01XX01|50.1|2000mg|Gói|Uống_|||50.2|3000mg|Gói|Uống_|||50.3|1000mg|Lọ|Tiêm_|||50.4|2000mg|Lọ|Tiêm_51|Gentamicin|J01GB03|51.1|40mg|Lọ|Tiêm_|||51.2|80mg|Lọ|Tiêm_52|Imipenem/cilastatin|J01DH51|52.1|250mg/250mg|Lọ|Tiêm_|||52.2|500mg/500mg|Lọ|Tiêm_53|Kanamycin|J01GB04|53.1|1000mg|Lọ|Tiêm_54|Levofloxacin|J01MA12|54.1|100mg|Viên|Uống_|||54.2|250mg|Viên|Uống_|||54.3|500mg|Viên|Uống_|||54.4|750mg|Viên|Uống_|||54.5|250mg|Lọ|Tiêm_|||54.6|500mg|Lọ/Túi|Tiêm_55|Lincomycin|J01FF02|55.1|500mg|Viên|Uống_|||55.2|300mg|Lọ|Tiêm_|||55.3|600mg|Lọ|Tiêm_56|Lomefloxacin|J01MA07|56.1|400mg|Viên|Uống_57|Meropenem|J01DH02|57.1|500mg|Lọ|Tiêm_|||57.2|1000mg|Lọ|Tiêm_58|Metronidazol|P01AB01|58.1|200mg|Viên|Uống_|||58.2|250mg|Viên|Uống_|||58.3|400mg|Viên|Uống_|||58.4|500mg|Viên|Uống_||J01XD01|58.5|500mg|Lọ/Túi|Tiêm_59|Minocyclin|J01AA08|59.1|50mg|Viên|Uống_|||59.2|100mg|Viên|Uống_60|Moxifloxacin|J01MA14|60.1|400mg|Viên|Uống_|||60.2|400mg|Lọ|Tiêm_61|Nalidixic acid|J01MB02|61.1|500mg|Viên|Uống_|||61.2|300mg/5ml x 100ml|Lọ|Uống_62|Netilmicin|J01GB07|62.1|50mg|Lọ|Tiêm_|||62.2|100mg|Lọ|Tiêm_|||62.3|150mg|Lọ|Tiêm_|||62.4|200mg|Lọ |Tiêm_63|Nitrofurantoin|J01XE01|63.1|100mg|Viên|Uống_64|Nitoxolin|J01XX07|64.1|100mg|Viên|Uống_65|Norfloxacin|J01MA06|65.1|400mg|Viên|Uống_66|Ofloxacin|J01MA01|66.1|100mg|Viên|Uống_|||66.2|200mg|Viên|Uống_|||66.3|400mg|Viên|Uống_|||66.4|200mg|Lọ|Tiêm_67|Oxacilin|J01CF04|67.1|500mg|Viên|Uống_|||67.2|1000mg|Lọ|Tiêm_68|Pefloxacin|J01MA03|68.1|400mg|Viên|Uống_|||68.2|400mg|Lọ|Tiêm_69|Phenoxymethyl penicilin|J01CE02|69.1|400.000UI|Viên|Uống_|||69.2|1.000.000UI|Viên|Uống_70|Piperacilin|J01CA12|70.1|2000mg|Lọ|Tiêm_|||70.2|4000mg|Lọ|Tiêm_71|Piperacilin/tazobactam|J01CR05|71.1|4000mg/500mg|Lọ|Tiêm_72|Roxithromycin|J01FA06|72.1|50mg|Viên|Uống_|||72.2|150mg|Viên|Uống_|||72.3|50mg/5ml x 30ml|Lọ|Uống_|||72.4|50mg/5ml x 60ml|Lọ|Uống_73|Secnidazol|P01AB07|73.1|500mg|Viên|Uống_|||73.2|1000mg|Viên|Uống_|||73.3|2000mg|Gói|Uống_74|Sparfloxacin|J01MA09|74.1|200mg|Viên|Uống_75|Spiramycin|J01FA02|75.1|750.000UI|Gói|Uống_|||75.2|1.500.000UI|Viên|Uống_|||75.3|3.000.000UI|Viên|Uống_76|Spiramycin/metronidazol|J01RA04|76.1|750.000UI/125mg|Viên|Uống_77|Sulfasalazin|A07EC01|77.1|500mg|Viên|Uống_78|Sulfaguanidin|A07AB03|78.1|500mg|Viên|Uống_79|Sulfamethoxazol/ trimethoprim|J01EE01|79.1|400mg/80mg|Viên|Uống_|||79.2|800mg/160mg|Viên|Uống_|||79.3|200mg/40mg/5ml x 50ml|Lọ|Uống_80|Telithromycin|J01FA15|80.1|400mg|Viên|Uống_81|Tetracyclin|J01AA01|81.1|250mg|Viên|Uống_|||81.2|500mg|Viên|Uống_82|Ticarcilin/clavulanat|J01CR03|82.1|1600mg/100mg|Lọ|Tiêm_|||82.2|3200mg/200mg|Lọ|Tiêm_83|Tinidazol|P01AB02|83.1|500mg|Viên|Uống_||J01XD02|83.2|400mg|Lọ|Tiêm_|||83.3|500mg|Lọ|Tiêm_84|Tobramycin|J01GB01|84.1|40mg|Lọ|Tiêm_|||84.2|80mg|Lọ|Tiêm_|||84.3|100mg|Lọ|Tiêm_85|Vancomycin|J01XA01|85.1|500mg|Lọ|Tiêm_|||85.2|1000mg|Lọ|Tiêm";

            //tạo danh sách ban đầu PL5 từ string PL5
            string[] PL5s = strPL5.Split('_');
            PL5 previous = new PL5();
            foreach (var item in PL5s)
            {
                PL5 rdo = new PL5();
                string[] PL5Details = item.Split('|');
                if (PL5Details.Length == 7)
                {
                    try
                    {
                        rdo.AT_NUM_ORDER = !string.IsNullOrWhiteSpace(PL5Details[0]) ? System.Int64.Parse(PL5Details[0]) : previous.AT_NUM_ORDER;
                    }
                    catch (Exception)
                    {
                        rdo.AT_NUM_ORDER = previous.AT_NUM_ORDER;
                    }
                    rdo.ACTIVE_INGR_BHYT_NAME = !string.IsNullOrWhiteSpace(PL5Details[1]) ? PL5Details[1] : previous.ACTIVE_INGR_BHYT_NAME;
                    rdo.ATC_CODES = !string.IsNullOrWhiteSpace(PL5Details[2]) ? PL5Details[2] : previous.ATC_CODES;
                    rdo.METY_NUM_ORDER = !string.IsNullOrWhiteSpace(PL5Details[3]) ? PL5Details[3] : previous.METY_NUM_ORDER;
                    rdo.CONCENTRA = !string.IsNullOrWhiteSpace(PL5Details[4]) ? PL5Details[4] : previous.CONCENTRA;
                    rdo.SERVICE_UNIT_NAME = !string.IsNullOrWhiteSpace(PL5Details[5]) ? PL5Details[5] : previous.SERVICE_UNIT_NAME;
                    rdo.MEDICINE_USE_FORM_NAME = !string.IsNullOrWhiteSpace(PL5Details[6]) ? PL5Details[6] : previous.MEDICINE_USE_FORM_NAME;
                }
                listPL5.Add(rdo);
                previous = rdo;
            }
        }
    }
}
