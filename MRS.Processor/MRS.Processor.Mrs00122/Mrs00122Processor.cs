using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisExpMestReason;

namespace MRS.Processor.Mrs00122
{
    public class Mrs00122Processor : AbstractProcessor
    {
        List<Mrs00122RDO> expData = null;
        HIS_MEDI_STOCK mediStock = null;
        List<long> listExpMestIds = new List<long>();
        Dictionary<long, HIS_EXP_MEST> dicExpMest = new Dictionary<long, HIS_EXP_MEST>();
        List<V_HIS_IMP_MEST> listMobaImpMest = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST> listMobaImpMestInTime = new List<V_HIS_IMP_MEST>();

        List<MedicineTypeRdo> listMedicineTypeRdo = new List<MedicineTypeRdo>();
        List<Mrs00122RDO> ListRdo = new List<Mrs00122RDO>();
        List<Mrs00122RDO> ListParent = new List<Mrs00122RDO>();
        CommonParam paramGet = new CommonParam();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, List<HIS_IMP_MEST_MEDICINE>> dicImpMestMedicine = new Dictionary<long, List<HIS_IMP_MEST_MEDICINE>>();
        Dictionary<long, List<HIS_IMP_MEST_MATERIAL>> dicImpMestMaterial = new Dictionary<long, List<HIS_IMP_MEST_MATERIAL>>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineView = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialView = new List<V_HIS_IMP_MEST_MATERIAL>();

        List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<long> ExpMestForTreatmentIds = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
        };
        const long CHMS_TYPE_BSCS = 1;
        const long CHMS_TYPE_HTCS = 2;

        HIS_MEDI_STOCK hisMediStock = null;
        List<V_HIS_MEDI_STOCK> hisMediStocks = new List<V_HIS_MEDI_STOCK>();
        V_HIS_MEDI_STOCK hisImpMediStock = null;
        List<V_HIS_MEDI_STOCK> hisImpMediStocks = new List<V_HIS_MEDI_STOCK>();
        HIS_DEPARTMENT hisDepartment = new HIS_DEPARTMENT();


        Dictionary<long, HIS_EXP_MEST_TYPE> dicMestTypes = new Dictionary<long, HIS_EXP_MEST_TYPE>();
        Dictionary<long, HIS_EXP_MEST_REASON> dicMestReasons = new Dictionary<long, HIS_EXP_MEST_REASON>();
        Dictionary<long, HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, HIS_MEDICINE_TYPE>();
        Dictionary<long, HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_MEDI_STOCK> dicMediStock = new Dictionary<long, V_HIS_MEDI_STOCK>();
        Dictionary<long, HIS_DEPARTMENT> dicDepartment = new Dictionary<long, HIS_DEPARTMENT>();
        Dictionary<long, ParentMedicine> dicParentMedicine = new Dictionary<long, ParentMedicine>();
        Dictionary<long, ParentMaterial> dicParentMaterial = new Dictionary<long, ParentMaterial>();

        Dictionary<long, TREATMENT> dicTreatment = new Dictionary<long, TREATMENT>();
        Dictionary<string, long> dicMobaIdPatientType = new Dictionary<string, long>();
        List<long> listChmsImpMestId = new List<long>();
        Mrs00122Filter filter;
        public Mrs00122Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00122Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = ((Mrs00122Filter)reportFilter);
                if (filter.IS_MEDICINE != true && filter.IS_MATERIAL != true && filter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    filter.IS_MEDICINE = true;
                    filter.IS_MATERIAL = true;
                    filter.IS_CHEMICAL_SUBSTANCE = true;
                }
                //get dữ liệu:
                if (filter.EXP_MEST_TYPE_IDs != null && filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL) && !filter.EXP_MEST_TYPE_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT))
                {
                    filter.EXP_MEST_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT);
                }


                HisExpMestMedicineViewFilterQuery expMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineFilter.EXP_TIME_FROM = filter.EXP_TIME_FROM;
                expMestMedicineFilter.EXP_TIME_TO = filter.EXP_TIME_TO;
                expMestMedicineFilter.EXP_MEST_TYPE_IDs = filter.EXP_MEST_TYPE_IDs;
                expMestMedicineFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMestMedicineFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                expMestMedicineFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                expMestMedicineFilter.IS_EXPORT = true;
                var expMestMedicinesSub = new HisExpMestMedicineManager(paramGet).GetView(expMestMedicineFilter);

                var listExpMestIdsSub = new List<long>();
                if (expMestMedicinesSub != null)
                {
                    expMestMedicines.AddRange(expMestMedicinesSub);
                }
                listExpMestIdsSub = expMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                listExpMestIds.AddRange(listExpMestIdsSub);

                HisExpMestMaterialViewFilterQuery expMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialFilter.EXP_TIME_FROM = filter.EXP_TIME_FROM;
                expMestMaterialFilter.EXP_TIME_TO = filter.EXP_TIME_TO;
                expMestMaterialFilter.EXP_MEST_TYPE_IDs = filter.EXP_MEST_TYPE_IDs;
                expMestMaterialFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                expMestMaterialFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                expMestMaterialFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                expMestMaterialFilter.IS_EXPORT = true;
                var expMestMaterialsSub = new HisExpMestMaterialManager(paramGet).GetView(expMestMaterialFilter);
                if (expMestMaterialsSub != null)
                {
                    expMestMaterials.AddRange(expMestMaterialsSub);
                }
                listExpMestIdsSub = expMestMaterials.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                listExpMestIds.AddRange(listExpMestIdsSub);


                Inventec.Common.Logging.LogSystem.Info("expMestMedicines" + expMestMedicines.Count);
                if ((filter.PATIENT_TYPE_ID ?? 0) != 0)
                {
                    expMestMedicines = expMestMedicines.Where(o => o.PATIENT_TYPE_ID == filter.PATIENT_TYPE_ID).ToList();
                    expMestMaterials = expMestMaterials.Where(o => o.PATIENT_TYPE_ID == filter.PATIENT_TYPE_ID).ToList();
                }
                Inventec.Common.Logging.LogSystem.Info("expMestMedicines" + expMestMedicines.Count);
                var listExpMest = new List<HIS_EXP_MEST>();
                var skip = 0;
                listExpMestIds = listExpMestIds.GroupBy(o => o).Select(p => p.First()).ToList();
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIDs = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestFilterQuery ExpMestFilter = new HisExpMestFilterQuery()
                    {
                        IDs = listIDs
                    };
                    var listExpMestSub = new HisExpMestManager(paramGet).Get(ExpMestFilter);
                    listExpMest.AddRange(listExpMestSub);
                }
                //Lấy các phiếu nhập thu hồi từ các phiếu đã xuất
                skip = 0;
                var mobaExpMestIds = new List<long>();
                mobaExpMestIds.AddRange(expMestMedicines.Where(p => p.TH_AMOUNT > 0).Select(o => o.EXP_MEST_ID ?? 0).ToList());
                mobaExpMestIds.AddRange(expMestMaterials.Where(p => p.TH_AMOUNT > 0).Select(o => o.EXP_MEST_ID ?? 0).ToList());

                mobaExpMestIds = mobaExpMestIds.GroupBy(o => o).Select(p => p.First()).ToList();
                while (mobaExpMestIds.Count - skip > 0)
                {
                    var listIDs = mobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery MobaImpMestFilter = new HisImpMestViewFilterQuery()
                    {
                        MOBA_EXP_MEST_IDs = listIDs,
                        IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                    };
                    var listMobaImpMestSub = new HisImpMestManager(paramGet).GetView(MobaImpMestFilter) ?? new List<V_HIS_IMP_MEST>();
                    listMobaImpMest.AddRange(listMobaImpMestSub);

                    HisImpMestMedicineFilterQuery impMeFilter = new HisImpMestMedicineFilterQuery();
                    impMeFilter.IMP_MEST_IDs = listMobaImpMestSub.Select(o => o.ID).Distinct().ToList();
                    var impMestMedicines = new HisImpMestMedicineManager().Get(impMeFilter) ?? new List<HIS_IMP_MEST_MEDICINE>();
                    foreach (var item in impMestMedicines)
                    {
                        if (item.TH_EXP_MEST_MEDICINE_ID.HasValue)
                        {
                            if (dicImpMestMedicine.ContainsKey(item.TH_EXP_MEST_MEDICINE_ID.Value))
                            {
                                dicImpMestMedicine[item.TH_EXP_MEST_MEDICINE_ID.Value].Add(item);
                            }
                            else
                            {
                                dicImpMestMedicine[item.TH_EXP_MEST_MEDICINE_ID.Value] = new List<HIS_IMP_MEST_MEDICINE>();
                                dicImpMestMedicine[item.TH_EXP_MEST_MEDICINE_ID.Value].Add(item);
                            }
                        }
                    }
                    HisImpMestMaterialFilterQuery impMaFilter = new HisImpMestMaterialFilterQuery();
                    impMaFilter.IMP_MEST_IDs = listMobaImpMestSub.Select(o => o.ID).Distinct().ToList();
                    var impMestMaterials = new HisImpMestMaterialManager().Get(impMaFilter) ?? new List<HIS_IMP_MEST_MATERIAL>();
                    foreach (var item in impMestMaterials)
                    {
                        if (item.TH_EXP_MEST_MATERIAL_ID.HasValue)
                        {
                            if (dicImpMestMaterial.ContainsKey(item.TH_EXP_MEST_MATERIAL_ID.Value))
                            {
                                dicImpMestMaterial[item.TH_EXP_MEST_MATERIAL_ID.Value].Add(item);
                            }
                            else
                            {
                                dicImpMestMaterial[item.TH_EXP_MEST_MATERIAL_ID.Value] = new List<HIS_IMP_MEST_MATERIAL>();
                                dicImpMestMaterial[item.TH_EXP_MEST_MATERIAL_ID.Value].Add(item);
                            }
                        }
                    }
                }
                HisImpMestViewFilterQuery MobaImpMestFilterInTime = new HisImpMestViewFilterQuery();

                MobaImpMestFilterInTime.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                MobaImpMestFilterInTime.IMP_MEST_TYPE_IDs = new List<long>() {
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                    IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL
                };
                MobaImpMestFilterInTime.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                MobaImpMestFilterInTime.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                MobaImpMestFilterInTime.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                MobaImpMestFilterInTime.IMP_TIME_FROM = filter.EXP_TIME_FROM;
                MobaImpMestFilterInTime.IMP_TIME_TO = filter.EXP_TIME_TO;
                listMobaImpMestInTime = new HisImpMestManager(paramGet).GetView(MobaImpMestFilterInTime) ?? new List<V_HIS_IMP_MEST>();

                //Lấy danh sách bệnh nhân trả thuốc trong thời gian đó
                GetTreatmentBhytMobaInTime();
                //Move back from cabinet
                HisImpMestViewFilterQuery MobackCabinetFilter = new HisImpMestViewFilterQuery();
                MobackCabinetFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                MobackCabinetFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK;
                MobackCabinetFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                MobackCabinetFilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_IDs;
                MobackCabinetFilter.REQ_DEPARTMENT_ID = filter.DEPARTMENT_ID;
                MobackCabinetFilter.IMP_TIME_FROM = filter.EXP_TIME_FROM;
                MobackCabinetFilter.IMP_TIME_TO = filter.EXP_TIME_TO;
                var MobackCabinets = new HisImpMestManager(paramGet).GetView(MobackCabinetFilter) ?? new List<V_HIS_IMP_MEST>();
                listMobaImpMestInTime.AddRange(MobackCabinets/*.Where(o => o.IS_CABINET != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE &&o.CHMS_MEDI_STOCK_ID.HasValue && HisMediStockCFG.HisMediStocks.Exists(p => o.CHMS_MEDI_STOCK_ID.Value == p.ID && p.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)).ToList()*/ );
                listChmsImpMestId = MobackCabinets.Where(o => !(o.IS_CABINET != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.CHMS_MEDI_STOCK_ID.HasValue && HisMediStockCFG.HisMediStocks.Exists(p => o.CHMS_MEDI_STOCK_ID.Value == p.ID && p.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE))).Select(o => o.ID).ToList();
                if (filter.IMP_MEDI_STOCK_ID != null)
                {
                    listExpMest = listExpMest.Where(o => o.IMP_MEDI_STOCK_ID == filter.IMP_MEDI_STOCK_ID).ToList();
                    var expMestIds = listExpMest.Select(o => o.ID).ToList();
                    expMestMedicines = expMestMedicines.Where(o => expMestIds.Contains(o.EXP_MEST_ID ?? 0)).ToList();
                    expMestMaterials = expMestMaterials.Where(o => expMestIds.Contains(o.EXP_MEST_ID ?? 0)).ToList();

                }

                if (filter.IMP_MEDI_STOCK_IDs != null)
                {
                    listExpMest = listExpMest.Where(o => filter.IMP_MEDI_STOCK_IDs.Contains(o.IMP_MEDI_STOCK_ID ?? -1)).ToList();
                    var expMestIds = listExpMest.Select(o => o.ID).ToList();
                    expMestMedicines = expMestMedicines.Where(o => expMestIds.Contains(o.EXP_MEST_ID ?? 0)).ToList();
                    expMestMaterials = expMestMaterials.Where(o => expMestIds.Contains(o.EXP_MEST_ID ?? 0)).ToList();
                }

                if (filter.EXP_MEST_REASON_ID != null)
                {
                    listExpMest = listExpMest.Where(o => o.EXP_MEST_REASON_ID == filter.EXP_MEST_REASON_ID).ToList();
                }

                if (filter.EXP_MEST_REASON_IDs != null)
                {
                    listExpMest = listExpMest.Where(o => filter.EXP_MEST_REASON_IDs.Contains(o.EXP_MEST_REASON_ID ?? -1)).ToList();
                }

                if (filter.EXP_LOGINNAME != null)
                {
                    listExpMest = listExpMest.Where(o => filter.EXP_LOGINNAME == o.LAST_EXP_LOGINNAME).ToList();
                }


                skip = 0;
                var MobaImpMestInTimeIds = listMobaImpMestInTime.Select(o => o.ID).Distinct().ToList();

                while (MobaImpMestInTimeIds.Count - skip > 0)
                {
                    var listIDs = MobaImpMestInTimeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery impMeFilterView = new HisImpMestMedicineViewFilterQuery();
                    impMeFilterView.IMP_MEST_IDs = listIDs;
                    var listImpMestMedicineViewSub = new HisImpMestMedicineManager().GetView(impMeFilterView) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                    listImpMestMedicineView.AddRange(listImpMestMedicineViewSub);

                    HisImpMestMaterialViewFilterQuery impMaFilterView = new HisImpMestMaterialViewFilterQuery();
                    impMaFilterView.IMP_MEST_IDs = listIDs;
                    var listImpMestMaterialViewSub = new HisImpMestMaterialManager().GetView(impMaFilterView) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                    listImpMestMaterialView.AddRange(listImpMestMaterialViewSub);
                }


                if (filter.MEDI_STOCK_ID != null) hisMediStock = new HisMediStockManager(paramGet).GetById(filter.MEDI_STOCK_ID ?? 0);
                if (filter.MEDI_STOCK_IDs != null) hisMediStocks = HisMediStockCFG.HisMediStocks.Where(o => filter.MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                if (filter.IMP_MEDI_STOCK_ID != null) hisImpMediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => filter.IMP_MEDI_STOCK_ID == o.ID);
                if (filter.IMP_MEDI_STOCK_IDs != null) hisImpMediStocks = HisMediStockCFG.HisMediStocks.Where(o => filter.IMP_MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                hisDepartment = new HisDepartmentManager(paramGet).GetById(filter.DEPARTMENT_ID ?? 0);
                dicTreatment = (new ManagerSql().GetTreatment(filter) ?? new List<TREATMENT>()).GroupBy(g => g.ID).ToDictionary(p => p.Key, q => q.First());
                result = true;

                dicExpMest = listExpMest.GroupBy(g => g.ID).ToDictionary(p => p.Key, q => q.First());

                //Get du lieu cuc bo
                GetLocalData();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void GetLocalData()
        {


            dicMestTypes = (new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery()) ?? new List<HIS_EXP_MEST_TYPE>()).ToDictionary(o => o.ID);
            dicMestReasons = (new HisExpMestReasonManager().Get(new HisExpMestReasonFilterQuery()) ?? new List<HIS_EXP_MEST_REASON>()).ToDictionary(o => o.ID);

            HisMedicineTypeFilterQuery HisMedicineTypeFilter = new HisMedicineTypeFilterQuery();
            var listMedicineType = new HisMedicineTypeManager(paramGet).Get(HisMedicineTypeFilter);
            foreach (var item in listMedicineType)
            {
                ParentMedicine rdo = new ParentMedicine();
                if (item.PARENT_ID != null)
                {
                    rdo.MEDICINE_TYPE_ID = item.ID;
                    rdo.PARENT_MEDICINE_TYPE_NAME = (listMedicineType.FirstOrDefault(o => o.ID == item.PARENT_ID) ?? new HIS_MEDICINE_TYPE()).MEDICINE_TYPE_NAME;
                    rdo.PARENT_MEDICINE_TYPE_CODE = (listMedicineType.FirstOrDefault(o => o.ID == item.PARENT_ID) ?? new HIS_MEDICINE_TYPE()).MEDICINE_TYPE_CODE;
                }
                dicParentMedicine.Add(item.ID, rdo);
            }

            HisMaterialTypeFilterQuery HisMaterialTypeFilter = new HisMaterialTypeFilterQuery();
            var listMaterialType = new HisMaterialTypeManager(paramGet).Get(HisMaterialTypeFilter);
            foreach (var item in listMaterialType)
            {
                ParentMaterial rdo = new ParentMaterial();
                if (item.PARENT_ID != null)
                {
                    rdo.MATERIAL_TYPE_ID = item.ID;
                    rdo.PARENT_MATERIAL_TYPE_NAME = (listMaterialType.FirstOrDefault(o => o.ID == item.PARENT_ID) ?? new HIS_MATERIAL_TYPE()).MATERIAL_TYPE_NAME;
                    rdo.PARENT_MATERIAL_TYPE_CODE = (listMaterialType.FirstOrDefault(o => o.ID == item.PARENT_ID) ?? new HIS_MATERIAL_TYPE()).MATERIAL_TYPE_CODE;
                }
                dicParentMaterial.Add(item.ID, rdo);
            }

            dicMedicineType = listMedicineType.ToDictionary(p => p.ID);

            dicMaterialType = listMaterialType.ToDictionary(p => p.ID);

            dicMediStock = HisMediStockCFG.HisMediStocks.ToDictionary(p => p.ID);

            dicDepartment = HisDepartmentCFG.DEPARTMENTs.ToDictionary(p => p.ID);
        }

        private void GetTreatmentBhytMobaInTime()
        {
            try
            {
                string query = @"select 
'THUOC' TYPE,
immm.ID,
exmm.patient_type_id
from v_his_imp_mest_medicine immm
join v_his_exp_mest_medicine exmm on exmm.id=immm.th_exp_mest_medicine_id
where immm.imp_mest_stt_id=5
and immm.imp_time between {0} and {1}
and {2}
and exmm.patient_type_id is not null
union all
select 
'VATTU' TYPE,
immm.ID,
exmm.patient_type_id
from v_his_imp_mest_material immm
join v_his_exp_mest_material exmm on exmm.id=immm.th_exp_mest_material_id
where immm.imp_mest_stt_id=5
and immm.imp_time between {0} and {1}
and {2}
and exmm.patient_type_id is not null";
                string filterMediStock = filter.MEDI_STOCK_IDs != null? string.Format("exmm.medi_stock_id in ({0})\n", string.Join(",", filter.MEDI_STOCK_IDs)):"1=1\n";
                query = string.Format(query, filter.EXP_TIME_FROM, filter.EXP_TIME_TO, filterMediStock);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var listMobaInTime = new MOS.DAO.Sql.SqlDAO().GetSql<MOBA_IMP_MEST_DETAIL_ID>(query);
                dicMobaIdPatientType = listMobaInTime.GroupBy(o => string.Format("{0}_{1}",o.TYPE,o.ID)).ToDictionary(p=>p.Key,q=>q.First().PATIENT_TYPE_ID??0);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                this.mediStock = hisMediStock;
                string KeyGroupExp = "{0}_{1}_{2}_{3}_{4}_{5}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null)
                {
                    KeyGroupExp = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                Dictionary<string, Mrs00122RDO> data = new Dictionary<string, Mrs00122RDO>();
                //Them du lieu thuoc vao danh sach xuat bao cao
                this.AddMedicine(expMestMedicines, KeyGroupExp, ref data);
                //Them du lieu vat tu vao danh sach xuat bao cao
                this.AddMaterial(expMestMaterials, KeyGroupExp, ref data);
                //Gom nhom theo lo thuoc/vat tu
                this.ListRdo = data.Values.ToList();

                var groupMedicineType = ListRdo.GroupBy(o => new { o.Type, o.TypeId, o.ImpPrice, o.ImpVatRatio, o.InternalPrice }).ToList();
                foreach (var gr in groupMedicineType)
                {
                    MedicineTypeRdo rdo = new MedicineTypeRdo();
                    List<Mrs00122RDO> listSub = gr.ToList<Mrs00122RDO>();
                    rdo.MEDICINE_TYPE_NAME = listSub.First().TypeName;
                    rdo.PRICE = listSub.First().ImpPrice;
                    rdo.SERVICE_UNIT_NAME = listSub.First().ServiceUnitName;
                    rdo.AMOUNT = listSub.Sum(s => s.Amount);
                    rdo.TOTAL_PRICE = listSub.Sum(s => s.Amount * s.ImpPrice);
                    rdo.DIC_AMOUNT = listSub.Where(r => r.ImpMediStockId.HasValue).GroupBy(o => o.ImpMediStockId).ToDictionary(p => MediStockCode(p.Key), q => q.Sum(s => (s.Amount)));
                    rdo.DIC_EXP_TYPE_AMOUNT = listSub.Where(r => r.ExpMestTypeId > 0).GroupBy(o => o.ExpMestTypeId).ToDictionary(p => p.Key.ToString(), q => q.Sum(s => (s.Amount)));
                    rdo.DIC_IMP_TYPE_AMOUNT = listSub.Where(r => r.ImpMestTypeId > 0).GroupBy(o => o.ImpMestTypeId).ToDictionary(p => p.Key.ToString(), q => q.Sum(s => (s.AmountMobaInTime)));
                    rdo.DIC_EXP_REASON_AMOUNT = listSub.Where(r => r.ExpMestReasonId > 0).GroupBy(o => o.ExpMestReasonId).ToDictionary(p => ExpMestReasonCode(p.Key), q => q.Sum(s => (s.Amount)));
                    rdo.DIC_TOTAL_PRICE = listSub.Where(r => r.ImpMediStockId.HasValue).GroupBy(o => o.ImpMediStockId).ToDictionary(p => MediStockCode(p.Key), q => q.Sum(s => (s.Amount * s.ImpPrice)));
                    listMedicineTypeRdo.Add(rdo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        private string MediStockCode(long? mediStockId)
        {
            string result = "";
            try
            {
                result = dicMediStock.ContainsKey(mediStockId ?? 0) ? dicMediStock[mediStockId ?? 0].MEDI_STOCK_CODE : "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string ExpMestReasonCode(long? ExpMestReasonId)
        {
            string result = "";
            try
            {
                result = dicMestReasons.ContainsKey(ExpMestReasonId??0) ?dicMestReasons[ExpMestReasonId??0].EXP_MEST_REASON_CODE:"";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private void AddMedicine(List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines, string KeyGroup, ref Dictionary<string, Mrs00122RDO> dicRdo)
        {
            if (IsNotNullOrEmpty(expMestMedicines))
            {

                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;

                foreach (var item in expMestMedicines)
                {
                    //thu hồi
                    decimal amountMoba = dicImpMestMedicine.ContainsKey(item.ID) ? dicImpMestMedicine[item.ID].Sum(s => s.AMOUNT) : 0;

                    //kho nhập
                    var impMediStockId = ImpMediStockId(item.EXP_MEST_ID ?? 0);

                    //lý do xuất
                    var expMestReasonId = HisExpMestReason(item.EXP_MEST_ID ?? 0) ?? 0;
                    var expMestReasonCode = ExpMestReasonCode(expMestReasonId);

                    string groupKey = string.Format(KeyGroup, Mrs00122RDO.MEDICINE, item.MEDICINE_TYPE_ID, item.IMP_PRICE, item.IMP_VAT_RATIO, item.INTERNAL_PRICE, item.MEDI_STOCK_ID, item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL ? ImpDepartmentId(item.EXP_MEST_ID ?? 0) ?? 0 : item.REQ_DEPARTMENT_ID, item.EXP_MEST_TYPE_ID, ImpMediStockId(item.EXP_MEST_ID ?? 0) ?? item.EXP_MEST_TYPE_ID, HisExpMestReason(item.EXP_MEST_ID ?? 0) ?? 0, item.PACKAGE_NUMBER);

                    if (dicRdo.ContainsKey(groupKey))
                    {
                        dicRdo[groupKey].Amount += item.AMOUNT;
                        dicRdo[groupKey].AmountBhyt += (item.TDL_TREATMENT_ID != null && item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountVp += (item.TDL_TREATMENT_ID != null && !(item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null)) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountFree += (item.VIR_PRICE == 0 && item.IS_EXPEND == null) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountExpend += (item.TDL_TREATMENT_ID != null && item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountHpKp += (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) ? item.AMOUNT : 0;
                        //dicRdo[groupKey].AmountNuocNgoai += (o.TDL_TREATMENT_ID != null && dicTreatment.ContainsKey(o.TDL_TREATMENT_ID ?? 0)) ? o.AMOUNT : 0;
                        dicRdo[groupKey].AmountTreEm += (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "TE") ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountNguoiNgheo += (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && (dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "HN" || dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "CN")) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountMoba += amountMoba;
                        dicRdo[groupKey].AmountTruct += item.AMOUNT - amountMoba;
                        
                        //tong xuat cho cac kho
                        if(!dicRdo[groupKey].DIC_AMOUNT.ContainsKey(MediStockCode(impMediStockId)))
                        {
                            dicRdo[groupKey].DIC_AMOUNT[MediStockCode(impMediStockId)] = 0;
                        }
                        dicRdo[groupKey].DIC_AMOUNT[MediStockCode(impMediStockId)] += (item.AMOUNT - amountMoba);

                        //tong xuat theo loai xuat
                        if (!dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT.ContainsKey(item.EXP_MEST_TYPE_ID.ToString()))
                        {
                            dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT[item.EXP_MEST_TYPE_ID.ToString()] = 0;
                        }
                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT[item.EXP_MEST_TYPE_ID.ToString()] += item.AMOUNT;

                        //tong xuat theo ly do
                        if (!dicRdo[groupKey].DIC_EXP_REASON_AMOUNT.ContainsKey(expMestReasonCode))
                        {
                            dicRdo[groupKey].DIC_EXP_REASON_AMOUNT[expMestReasonCode] = 0;
                        }
                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT[expMestReasonCode] += item.AMOUNT;

                        //tong tien xuat cho cac kho
                        if (!dicRdo[groupKey].DIC_TOTAL_PRICE.ContainsKey(MediStockCode(impMediStockId)))
                        {
                            dicRdo[groupKey].DIC_TOTAL_PRICE[MediStockCode(impMediStockId)] = 0;
                        }

                        dicRdo[groupKey].DIC_TOTAL_PRICE[MediStockCode(impMediStockId)] += (item.AMOUNT - amountMoba) * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {

                        var rdo = new Mrs00122RDO()
                        {
                            Id = item.MEDICINE_ID ?? 0,
                            Type = Mrs00122RDO.MEDICINE,
                            Amount = item.AMOUNT,
                            AmountBhyt = (item.TDL_TREATMENT_ID != null && item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null) ? item.AMOUNT : 0,
                            AmountVp = (item.TDL_TREATMENT_ID != null && !(item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null)) ? item.AMOUNT : 0,
                            AmountFree = (item.VIR_PRICE == 0 && item.IS_EXPEND == null) ? item.AMOUNT : 0,
                            AmountExpend = (item.TDL_TREATMENT_ID != null && item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) ? item.AMOUNT : 0,
                            AmountHpKp = (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) ? item.AMOUNT : 0,
                            //AmountNuocNgoai = (o.TDL_TREATMENT_ID != null && dicTreatment.ContainsKey(o.TDL_TREATMENT_ID ?? 0)) ? o.AMOUNT : 0,
                            AmountTreEm = (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "TE") ? item.AMOUNT : 0,
                            AmountNguoiNgheo = (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && (dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "HN" || dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "CN")) ? item.AMOUNT : 0,
                            AmountMoba = amountMoba,
                            BidNumber = item.BID_NUMBER,
                            ExpiredDateStr = item.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXPIRED_DATE.Value) : null,
                            ImpPrice = item.IMP_PRICE,
                            ExpTimeStr = item.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXP_TIME.Value) : null,
                            InternalPrice = item.INTERNAL_PRICE,
                            PackageNumber = item.PACKAGE_NUMBER,
                            RegisterNumber = item.REGISTER_NUMBER,
                            ServiceUnitCode = item.SERVICE_UNIT_CODE,
                            ServiceUnitName = item.SERVICE_UNIT_NAME,
                            SupplierCode = item.SUPPLIER_CODE,
                            SupplierName = item.SUPPLIER_NAME,
                            TypeName = item.MEDICINE_TYPE_NAME,
                            TypeCode = item.MEDICINE_TYPE_CODE,
                            ParentName = dicParentMedicine.ContainsKey(item.MEDICINE_TYPE_ID) ? dicParentMedicine[item.MEDICINE_TYPE_ID].PARENT_MEDICINE_TYPE_NAME : "",
                            ParentCode = dicParentMedicine.ContainsKey(item.MEDICINE_TYPE_ID) ? dicParentMedicine[item.MEDICINE_TYPE_ID].PARENT_MEDICINE_TYPE_CODE : "",
                            ManuFacturerName = item.MANUFACTURER_NAME,
                            Concentra = item.CONCENTRA,
                            TypeId = item.MEDICINE_TYPE_ID,
                            ImpVatRatio = item.IMP_VAT_RATIO,
                            ExpMestId = item.EXP_MEST_ID ?? 0,
                            ExpMestCodeStr = item.EXP_MEST_CODE,
                            ExpMestTypeId = item.EXP_MEST_TYPE_ID,
                            ExpMestReasonId = expMestReasonId,
                            MediStockId = item.MEDI_STOCK_ID,
                            MediStockName = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_NAME : "",
                            ReqDepartmentId = item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL ? ImpDepartmentId(item.EXP_MEST_ID ?? 0) ?? 0 : item.REQ_DEPARTMENT_ID,
                            MaHoatChat = item.ACTIVE_INGR_BHYT_CODE,
                            BietDuoc = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME : "",
                            NationalName = item.NATIONAL_NAME,
                            IsChemicalSubstance = 0,
                            AmountTruct = item.AMOUNT - amountMoba,
                            ReqDepartmentName = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_NAME : "",
                            ReqDepartmentCode = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_CODE : "",
                            ImpStockThenExpTypeName = dicMediStock.ContainsKey(impMediStockId ?? 0) ? dicMediStock[impMediStockId ?? 0].MEDI_STOCK_NAME 
                            : (dicMestTypes.ContainsKey(item.EXP_MEST_TYPE_ID ) ? dicMestTypes[item.EXP_MEST_TYPE_ID].EXP_MEST_TYPE_NAME : ""),
                        };
                        dicRdo.Add(groupKey, rdo);
                        dicRdo[groupKey].DIC_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_AMOUNT.Add(MediStockCode(impMediStockId), (item.AMOUNT - amountMoba));

                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT.Add(item.EXP_MEST_TYPE_ID.ToString(), item.AMOUNT);

                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT.Add(expMestReasonCode, item.AMOUNT);

                        dicRdo[groupKey].DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_TOTAL_PRICE.Add(MediStockCode(impMediStockId), (item.AMOUNT - amountMoba) * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO));
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                    }
                }

                foreach (var item in listImpMestMedicineView)
                {

                    string groupKey = string.Format(KeyGroup, Mrs00122RDO.MEDICINE, item.MEDICINE_TYPE_ID, item.IMP_PRICE, item.IMP_VAT_RATIO, item.INTERNAL_PRICE, item.MEDI_STOCK_ID, item.REQ_DEPARTMENT_ID, 0, 0, 0, item.PACKAGE_NUMBER);

                    if (dicRdo.ContainsKey(groupKey))
                    {
                        dicRdo[groupKey].AmountMobaInTime += listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : item.AMOUNT;
                        dicRdo[groupKey].AmountMobaInTimeBhyt += listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "THUOC", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "THUOC", item.ID)] == patientTypeIdBhyt ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountMobaInTimeVp += listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "THUOC", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "THUOC", item.ID)] != patientTypeIdBhyt ? item.AMOUNT : 0;

                        //tong thu hoi cac loai
                        if (!dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT.ContainsKey(item.IMP_MEST_TYPE_ID.ToString()))
                        {
                            dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT[item.IMP_MEST_TYPE_ID.ToString()] = item.AMOUNT;
                        }
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT[item.IMP_MEST_TYPE_ID.ToString()] += item.AMOUNT;
                    }
                    else
                    {

                        var rdo = new Mrs00122RDO()
                        {
                            IsChms = listChmsImpMestId.Contains(item.IMP_MEST_ID),
                            Id = item.MEDICINE_ID,
                            Type = Mrs00122RDO.MEDICINE,
                            AmountMobaInTime = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : item.AMOUNT,
                            AmountMobaInTimeBhyt = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "THUOC", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "THUOC", item.ID)] == patientTypeIdBhyt ? item.AMOUNT : 0,
                            AmountMobaInTimeVp = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : (dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "THUOC", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "THUOC", item.ID)] != patientTypeIdBhyt ? item.AMOUNT : 0),
                            AmountChms = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? item.AMOUNT : 0,
                            BidNumber = item.BID_NUMBER,
                            ExpiredDateStr = item.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXPIRED_DATE.Value) : null,
                            ImpPrice = item.IMP_PRICE,
                            ImpTimeStr = item.IMP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IMP_TIME.Value) : null,
                            InternalPrice = item.INTERNAL_PRICE,
                            RegisterNumber = item.REGISTER_NUMBER,
                            PackageNumber = item.PACKAGE_NUMBER,
                            ServiceUnitCode = item.SERVICE_UNIT_CODE,
                            ServiceUnitName = item.SERVICE_UNIT_NAME,
                            SupplierCode = item.SUPPLIER_CODE,
                            SupplierName = item.SUPPLIER_NAME,
                            TypeName = item.MEDICINE_TYPE_NAME,
                            TypeCode = item.MEDICINE_TYPE_CODE,
                            ParentName = dicParentMedicine.ContainsKey(item.MEDICINE_TYPE_ID) ? dicParentMedicine[item.MEDICINE_TYPE_ID].PARENT_MEDICINE_TYPE_NAME : "",
                            ParentCode = dicParentMedicine.ContainsKey(item.MEDICINE_TYPE_ID) ? dicParentMedicine[item.MEDICINE_TYPE_ID].PARENT_MEDICINE_TYPE_CODE : "",
                            ManuFacturerName = item.MANUFACTURER_NAME,
                            Concentra = item.CONCENTRA,
                            TypeId = item.MEDICINE_TYPE_ID,
                            ImpVatRatio = item.IMP_VAT_RATIO,
                            MediStockId = item.MEDI_STOCK_ID,
                            MediStockName = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_NAME : "",
                            ReqDepartmentId = item.REQ_DEPARTMENT_ID ?? 0,
                            MaHoatChat = item.ACTIVE_INGR_BHYT_CODE,
                            BietDuoc = dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID) ? dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME : "",
                            NationalName = item.NATIONAL_NAME,
                            IsChemicalSubstance = 0,
                            ImpMestTypeId = item.IMP_MEST_TYPE_ID,
                        };
                        dicRdo.Add(groupKey, rdo);
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT.Add(item.IMP_MEST_TYPE_ID.ToString(), item.AMOUNT);
                        dicRdo[groupKey].DIC_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
                    }

                }
            }
        }

        private long? HisExpMestReason(long expMestId)
        {
            long? result = null;
            try
            {
                if (dicExpMest.ContainsKey(expMestId))
                {
                    result = (dicExpMest[expMestId] ?? new HIS_EXP_MEST()).EXP_MEST_REASON_ID;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        private void AddMaterial(List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials, string KeyGroup, ref Dictionary<string, Mrs00122RDO> dicRdo)
        {
            if (IsNotNullOrEmpty(expMestMaterials))
            {

                long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;

                foreach (var item in expMestMaterials)
                {
                    //thu hồi
                    decimal amountMoba = dicImpMestMaterial.ContainsKey(item.ID) ? dicImpMestMaterial[item.ID].Sum(s => s.AMOUNT) : 0;

                    //kho nhập
                    var impMediStockId = ImpMediStockId(item.EXP_MEST_ID ?? 0);

                    //lý do xuất
                    var expMestReasonId = HisExpMestReason(item.EXP_MEST_ID ?? 0) ?? 0;
                    var expMestReasonCode = ExpMestReasonCode(expMestReasonId);

                    string groupKey = string.Format(KeyGroup, Mrs00122RDO.MATERIAL, item.MATERIAL_TYPE_ID, item.IMP_PRICE, item.IMP_VAT_RATIO, item.INTERNAL_PRICE, item.MEDI_STOCK_ID, item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL ? ImpDepartmentId(item.EXP_MEST_ID ?? 0) ?? 0 : item.REQ_DEPARTMENT_ID, item.EXP_MEST_TYPE_ID, ImpMediStockId(item.EXP_MEST_ID ?? 0) ?? item.EXP_MEST_TYPE_ID, HisExpMestReason(item.EXP_MEST_ID ?? 0) ?? 0, item.PACKAGE_NUMBER);

                    if (dicRdo.ContainsKey(groupKey))
                    {
                        dicRdo[groupKey].Amount += item.AMOUNT;
                        dicRdo[groupKey].AmountBhyt += (item.TDL_TREATMENT_ID != null && item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountVp += (item.TDL_TREATMENT_ID != null && !(item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null)) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountFree += (item.VIR_PRICE == 0 && item.IS_EXPEND == null) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountExpend += (item.TDL_TREATMENT_ID != null && item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountHpKp += (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) ? item.AMOUNT : 0;
                        //dicRdo[groupKey].AmountNuocNgoai += (o.TDL_TREATMENT_ID != null && dicTreatment.ContainsKey(o.TDL_TREATMENT_ID ?? 0)) ? o.AMOUNT : 0;
                        dicRdo[groupKey].AmountTreEm += (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "TE") ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountNguoiNgheo += (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && (dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "HN" || dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "CN")) ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountMoba += amountMoba;
                        dicRdo[groupKey].AmountTruct += item.AMOUNT - amountMoba;

                        //tong xuat cho cac kho
                        if (!dicRdo[groupKey].DIC_AMOUNT.ContainsKey(MediStockCode(impMediStockId)))
                        {
                            dicRdo[groupKey].DIC_AMOUNT[MediStockCode(impMediStockId)] = 0;
                        }
                        dicRdo[groupKey].DIC_AMOUNT[MediStockCode(impMediStockId)] += (item.AMOUNT - amountMoba);

                        //tong xuat theo loai xuat
                        if (!dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT.ContainsKey(item.EXP_MEST_TYPE_ID.ToString()))
                        {
                            dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT[item.EXP_MEST_TYPE_ID.ToString()] = 0;
                        }
                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT[item.EXP_MEST_TYPE_ID.ToString()] += item.AMOUNT;

                        //tong xuat theo ly do
                        if (!dicRdo[groupKey].DIC_EXP_REASON_AMOUNT.ContainsKey(expMestReasonCode))
                        {
                            dicRdo[groupKey].DIC_EXP_REASON_AMOUNT[expMestReasonCode] = 0;
                        }
                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT[expMestReasonCode] += item.AMOUNT;

                        //tong tien xuat cho cac kho
                        if (!dicRdo[groupKey].DIC_TOTAL_PRICE.ContainsKey(MediStockCode(impMediStockId)))
                        {
                            dicRdo[groupKey].DIC_TOTAL_PRICE[MediStockCode(impMediStockId)] = 0;
                        }
                        dicRdo[groupKey].DIC_TOTAL_PRICE[MediStockCode(impMediStockId)] += (item.AMOUNT - amountMoba) * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    }
                    else
                    {

                        var rdo = new Mrs00122RDO()
                        {
                            Id = item.MATERIAL_ID ?? 0,
                            Type = item.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? Mrs00122RDO.CHEMICAL_SUBSTANCE : Mrs00122RDO.MATERIAL,
                            Amount = item.AMOUNT,
                            AmountBhyt = (item.TDL_TREATMENT_ID != null && item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null) ? item.AMOUNT : 0,
                            AmountVp = (item.TDL_TREATMENT_ID != null && !(item.PATIENT_TYPE_ID == patientTypeIdBhyt && item.IS_EXPEND == null)) ? item.AMOUNT : 0,
                            AmountFree = (item.VIR_PRICE == 0 && item.IS_EXPEND == null) ? item.AMOUNT : 0,
                            AmountExpend = (item.TDL_TREATMENT_ID != null && item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE) ? item.AMOUNT : 0,
                            AmountHpKp = (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) ? item.AMOUNT : 0,
                            //AmountNuocNgoai = (o.TDL_TREATMENT_ID != null && dicTreatment.ContainsKey(o.TDL_TREATMENT_ID ?? 0)) ? o.AMOUNT : 0,
                            AmountTreEm = (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "TE") ? item.AMOUNT : 0,
                            AmountNguoiNgheo = (dicTreatment.ContainsKey(item.TDL_TREATMENT_ID ?? 0) && (dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "HN" || dicTreatment[item.TDL_TREATMENT_ID ?? 0].HEAD_CARD == "CN")) ? item.AMOUNT : 0,
                            AmountMoba = amountMoba,
                            BidNumber = item.BID_NUMBER,
                            ExpiredDateStr = item.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXPIRED_DATE.Value) : null,
                            ImpPrice = item.IMP_PRICE,
                            ExpTimeStr = item.EXP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXP_TIME.Value) : null,
                            InternalPrice = item.INTERNAL_PRICE,
                            PackageNumber = item.PACKAGE_NUMBER,
                            //RegisterNumber = item.REGISTER_NUMBER,
                            ServiceUnitCode = item.SERVICE_UNIT_CODE,
                            ServiceUnitName = item.SERVICE_UNIT_NAME,
                            SupplierCode = item.SUPPLIER_CODE,
                            SupplierName = item.SUPPLIER_NAME,
                            TypeName = item.MATERIAL_TYPE_NAME,
                            TypeCode = item.MATERIAL_TYPE_CODE,
                            ParentName = dicParentMaterial.ContainsKey(item.MATERIAL_TYPE_ID) ? dicParentMaterial[item.MATERIAL_TYPE_ID].PARENT_MATERIAL_TYPE_NAME : "",
                            ParentCode = dicParentMaterial.ContainsKey(item.MATERIAL_TYPE_ID) ? dicParentMaterial[item.MATERIAL_TYPE_ID].PARENT_MATERIAL_TYPE_CODE : "",
                            ManuFacturerName = item.MANUFACTURER_NAME,
                            //Concentra = item.CONCENTRA,
                            TypeId = item.MATERIAL_TYPE_ID,
                            ImpVatRatio = item.IMP_VAT_RATIO,
                            ExpMestId = item.EXP_MEST_ID ?? 0,
                            ExpMestCodeStr = item.EXP_MEST_CODE,
                            ExpMestTypeId = item.EXP_MEST_TYPE_ID,
                            ExpMestReasonId = expMestReasonId,
                            MediStockId = item.MEDI_STOCK_ID,
                            MediStockName = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_NAME : "",
                            ReqDepartmentId = item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL ? ImpDepartmentId(item.EXP_MEST_ID ?? 0) ?? 0 : item.REQ_DEPARTMENT_ID,
                            //MaHoatChat = item.ACTIVE_INGR_BHYT_CODE,
                            //BietDuoc = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_PROPRIETARY_NAME : "",
                            NationalName = item.NATIONAL_NAME,
                            IsChemicalSubstance = 0,
                            AmountTruct = item.AMOUNT - amountMoba,
                            ReqDepartmentName = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_NAME : "",
                            ReqDepartmentCode = dicDepartment.ContainsKey(item.REQ_DEPARTMENT_ID) ? dicDepartment[item.REQ_DEPARTMENT_ID].DEPARTMENT_CODE : "",
                            ImpStockThenExpTypeName = dicMediStock.ContainsKey(impMediStockId ?? 0) ? dicMediStock[impMediStockId ?? 0].MEDI_STOCK_NAME
                            : (dicMestTypes.ContainsKey(item.EXP_MEST_TYPE_ID) ? dicMestTypes[item.EXP_MEST_TYPE_ID].EXP_MEST_TYPE_NAME : ""),
                        };
                        dicRdo.Add(groupKey, rdo);
                        dicRdo[groupKey].DIC_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_AMOUNT.Add(MediStockCode(impMediStockId), (item.AMOUNT - amountMoba));

                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_TYPE_AMOUNT.Add(item.EXP_MEST_TYPE_ID.ToString(), item.AMOUNT);

                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_EXP_REASON_AMOUNT.Add(expMestReasonCode, item.AMOUNT);

                        dicRdo[groupKey].DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_TOTAL_PRICE.Add(MediStockCode(impMediStockId), (item.AMOUNT - amountMoba) * item.IMP_PRICE * (1 + item.IMP_VAT_RATIO));
                    }
                }

                foreach (var item in listImpMestMaterialView)
                {

                    string groupKey = string.Format(KeyGroup, Mrs00122RDO.MATERIAL, item.MATERIAL_TYPE_ID, item.IMP_PRICE, item.IMP_VAT_RATIO, item.INTERNAL_PRICE, item.MEDI_STOCK_ID, item.REQ_DEPARTMENT_ID, 0, 0, 0, item.PACKAGE_NUMBER);

                    if (dicRdo.ContainsKey(groupKey))
                    {
                        dicRdo[groupKey].AmountMobaInTime += item.AMOUNT;
                        dicRdo[groupKey].AmountMobaInTimeBhyt += listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "VATTU", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "VATTU", item.ID)] == patientTypeIdBhyt ? item.AMOUNT : 0;
                        dicRdo[groupKey].AmountMobaInTimeVp += listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "VATTU", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "VATTU", item.ID)] != patientTypeIdBhyt ? item.AMOUNT : 0;

                        //tong thu hoi cac loai
                        if (!dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT.ContainsKey(item.IMP_MEST_TYPE_ID.ToString()))
                        {
                            dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT[item.IMP_MEST_TYPE_ID.ToString()] = item.AMOUNT;
                        }
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT[item.IMP_MEST_TYPE_ID.ToString()] += item.AMOUNT;
                    }
                    else
                    {

                       var rdo = new Mrs00122RDO()
                        {
                            IsChms = listChmsImpMestId.Contains(item.IMP_MEST_ID),
                            Id = item.MATERIAL_ID,
                            Type = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) && dicMaterialType[item.MATERIAL_TYPE_ID].IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? Mrs00122RDO.CHEMICAL_SUBSTANCE : Mrs00122RDO.MATERIAL,
                            AmountMobaInTime = listChmsImpMestId.Contains(item.IMP_MEST_ID)?0:item.AMOUNT,
                            AmountMobaInTimeBhyt = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "VATTU", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "VATTU", item.ID)] == patientTypeIdBhyt ? item.AMOUNT : 0,
                            AmountMobaInTimeVp = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? 0 : (dicMobaIdPatientType.ContainsKey(string.Format("{0}_{1}", "VATTU", item.ID)) && dicMobaIdPatientType[string.Format("{0}_{1}", "VATTU", item.ID)] != patientTypeIdBhyt ? item.AMOUNT : 0),
                            AmountChms = listChmsImpMestId.Contains(item.IMP_MEST_ID) ? item.AMOUNT:0,
                            BidNumber = item.BID_NUMBER,
                            ExpiredDateStr = item.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXPIRED_DATE.Value) : null,
                            ImpPrice = item.IMP_PRICE,
                            ImpTimeStr = item.IMP_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IMP_TIME.Value) : null,
                            InternalPrice = item.INTERNAL_PRICE,
                            //RegisterNumber = item.REGISTER_NUMBER,
                            PackageNumber = item.PACKAGE_NUMBER,
                            ServiceUnitCode = item.SERVICE_UNIT_CODE,
                            ServiceUnitName = item.SERVICE_UNIT_NAME,
                            SupplierCode = item.SUPPLIER_CODE,
                            SupplierName = item.SUPPLIER_NAME,
                            TypeName = item.MATERIAL_TYPE_NAME,
                            TypeCode = item.MATERIAL_TYPE_CODE,
                            ParentName = dicParentMaterial.ContainsKey(item.MATERIAL_TYPE_ID) ? dicParentMaterial[item.MATERIAL_TYPE_ID].PARENT_MATERIAL_TYPE_NAME : "",
                            ParentCode = dicParentMaterial.ContainsKey(item.MATERIAL_TYPE_ID) ? dicParentMaterial[item.MATERIAL_TYPE_ID].PARENT_MATERIAL_TYPE_CODE : "",
                            ManuFacturerName = item.MANUFACTURER_NAME,
                            //Concentra = item.CONCENTRA,
                            TypeId = item.MATERIAL_TYPE_ID,
                            ImpVatRatio = item.IMP_VAT_RATIO,
                            MediStockId = item.MEDI_STOCK_ID,
                            MediStockName = dicMediStock.ContainsKey(item.MEDI_STOCK_ID) ? dicMediStock[item.MEDI_STOCK_ID].MEDI_STOCK_NAME : "",
                            ReqDepartmentId = item.REQ_DEPARTMENT_ID ?? 0,
                            //MaHoatChat = item.ACTIVE_INGR_BHYT_CODE,
                            //BietDuoc = dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID) ? dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_PROPRIETARY_NAME : "",
                            NationalName = item.NATIONAL_NAME,
                            IsChemicalSubstance = 0,
                            ImpMestTypeId = item.IMP_MEST_TYPE_ID,
                        };
                        dicRdo.Add(groupKey, rdo);
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT = new Dictionary<string, decimal>();
                        dicRdo[groupKey].DIC_IMP_TYPE_AMOUNT.Add(item.IMP_MEST_TYPE_ID.ToString(), item.AMOUNT);
                    }

                }
            }
        }

        private long? ImpMediStockId(long expMestId)
        {
            long? result = null;
            try
            {
                if (dicExpMest.ContainsKey(expMestId))
                {
                    result = (dicExpMest[expMestId] ?? new HIS_EXP_MEST()).IMP_MEDI_STOCK_ID;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private long? ImpDepartmentId(long expMestId)
        {
            long? result = null;
            try
            {
                if (dicExpMest.ContainsKey(expMestId) && dicExpMest[expMestId] != null)
                {
                    var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == dicExpMest[expMestId].IMP_MEDI_STOCK_ID);
                    if (mediStock != null)
                    {
                        result = mediStock.DEPARTMENT_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var filter = ((Mrs00122Filter)reportFilter);
            if (filter.EXP_TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.EXP_TIME_FROM));
            }
            if (filter.EXP_TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.EXP_TIME_TO));
            }

            if (filter.EXP_LOGINNAME != null)
            {
                var x = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery()).Where(o => ((Mrs00122Filter)reportFilter).EXP_LOGINNAME == o.LOGINNAME).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("EXP_USERNAME", x.First().USERNAME);
            }
            if (this.mediStock != null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", this.mediStock.MEDI_STOCK_NAME);
                dicSingleTag.Add("MEDI_STOCK_CODE", this.mediStock.MEDI_STOCK_CODE);
            }
            if (this.hisMediStocks != null)
            {
                dicSingleTag.Add("MEDI_STOCK_NAMEs", string.Join(",", this.hisMediStocks.Select(o => o.MEDI_STOCK_NAME).ToList()));
                dicSingleTag.Add("MEDI_STOCK_CODEs", string.Join(",", this.hisMediStocks.Select(o => o.MEDI_STOCK_CODE).ToList()));
            }


            if (this.hisImpMediStock != null)
            {
                dicSingleTag.Add("IMP_MEDI_STOCK_NAME", this.hisImpMediStock.MEDI_STOCK_NAME);
                dicSingleTag.Add("IMP_MEDI_STOCK_CODE", this.hisImpMediStock.MEDI_STOCK_CODE);
            }
            if (this.hisImpMediStocks != null)
            {
                dicSingleTag.Add("IMP_MEDI_STOCK_NAMEs", string.Join(",", this.hisImpMediStocks.Select(o => o.MEDI_STOCK_NAME).ToList()));
                dicSingleTag.Add("IMP_MEDI_STOCK_CODEs", string.Join(",", this.hisImpMediStocks.Select(o => o.MEDI_STOCK_CODE).ToList()));
            }

            if (this.hisDepartment != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", this.hisDepartment.DEPARTMENT_NAME);
                dicSingleTag.Add("DEPARTMENT_CODE", this.hisDepartment.DEPARTMENT_CODE);
            }
            if (filter.IS_MEDICINE == false)
            {
                ListRdo = ListRdo.Where(o => o.Type != 1).ToList();
            }
            if (filter.IS_MATERIAL == false)
            {
                ListRdo = ListRdo.Where(o => o.Type != 2).ToList();
            }
            if (filter.IS_CHEMICAL_SUBSTANCE == false)
            {
                ListRdo = ListRdo.Where(o => o.Type != 3).ToList();
            }
            ListParent = ListRdo.GroupBy(o => o.MediStockId).Select(p => p.First()).ToList();
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListParent);
            objectTag.AddRelationship(store, "Parent", "Report", "MediStockId", "MediStockId");
            objectTag.AddObjectData(store, "MedicineType", listMedicineTypeRdo);
            objectTag.AddObjectData(store, "ExpMest", ListRdo.GroupBy(o => o.ExpMestId).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ExpMest", "Report", "ExpMestId", "ExpMestId");
            objectTag.AddObjectData(store, "ParentService", ListRdo.OrderBy(p => p.ParentName).GroupBy(o => o.ParentCode).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ParentService", "Report", "ParentCode", "ParentCode");
            objectTag.AddObjectData(store, "Chms", new ManagerSql().GetChms(filter));

            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

    }
}
/*select ss.exp_loginname,ss.exp_time,ss.exp_mest_type_id,(select medi_stock_code from his_medi_stock where id = ss.medi_stock_id) as kho_xuat,(select medi_stock_code from his_medi_stock where id = sss.imp_medi_stock_id) as kho_nhap,ss.amount,ss.medicine_type_code 
from v_his_exp_mest_medicine ss join v_his_exp_mest sss on sss.id = ss.exp_mest_id 
where ss.is_export =1 and ss.exp_time between 20181001000000 and 20181006235959
and ss.exp_mest_type_id in(3,5,13);*/