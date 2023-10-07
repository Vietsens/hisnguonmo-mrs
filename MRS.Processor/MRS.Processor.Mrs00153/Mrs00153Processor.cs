using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestStt;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisDepartment;
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
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisBedRoom;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00153
{
    class Mrs00153Processor : AbstractProcessor
    {
        List<VSarReportMrs00153RDO> listRdo = new List<VSarReportMrs00153RDO>();
        List<MedicineTypeRdo> listMedicineTypeRdo = new List<MedicineTypeRdo>();
        Mrs00153Filter CastFilter;
        private List<HIS_DEPARTMENT> listHisDEpartments = new List<HIS_DEPARTMENT>();
        private List<TotalPrice> listTotalPrices = new List<TotalPrice>(); //tính tổng tiền của mối khoa
        private string MEDI_STOCK_NAME;
        private string IMP_MEST_STT_NAME;
        List<V_HIS_IMP_MEST> listMobaImpMestViews = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialViews = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_MEDICINE_TYPE> MedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> MaterialTypes = new List<V_HIS_MATERIAL_TYPE>();
        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<V_HIS_IMP_MEST> listAggrMobaImpMestViews = new List<V_HIS_IMP_MEST>();
        List<MedicineTypeRdo> listMediMateTypeRdo = new List<MedicineTypeRdo>();
        List<HIS_IMP_MEST_TYPE> listImpMestType = new List<HIS_IMP_MEST_TYPE>();
        List<HIS_EXP_MEST> listMobaExpMest = new List<HIS_EXP_MEST>();
        const long CHMS_TYPE_BSCS = 1;
        const long CHMS_TYPE_HTCS = 2;
        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();
        public Mrs00153Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00153Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                CastFilter = (Mrs00153Filter)this.reportFilter;
                //HisExpMestFilterQuery expMestCabinetFilter = new HisExpMestFilterQuery();
                //expMestCabinetFilter.MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_CABINET_IDs;// tủ trực xuất
                //ListExpMest = new HisExpMestManager().Get(expMestCabinetFilter);
                var paramGet = new CommonParam();
                var metyMediStockId = new HisMediStockViewFilterQuery
                {
                    ID = CastFilter.MEDI_STOCK_ID,
                    IDs = CastFilter.MEDI_STOCK_IDs,
                };
                var mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(metyMediStockId);
                if (mediStock != null)
                {
                    MEDI_STOCK_NAME = string.Join(", ", mediStock.Select(s => s.MEDI_STOCK_NAME).ToList());
                }
                //--------------------------------------------------------------------------------------------------

                var metyImpMestTypeIds = new HisImpMestSttFilterQuery
                {
                    ID = CastFilter.IMP_MEST_STT_ID,
                    IDs = CastFilter.IMP_MEST_STT_IDs,
                };
                var impMestType = new MOS.MANAGER.HisImpMestStt.HisImpMestSttManager(paramGet).Get(metyImpMestTypeIds);

                if (impMestType != null)
                {
                    IMP_MEST_STT_NAME = string.Join(", ", impMestType.Select(s => s.IMP_MEST_STT_NAME).ToList());
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST
                var listImpMestSttId = new List<long> { 
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__REQUEST,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__APPROVAL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__DRAFT,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__REJECT,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                    };

                if (CastFilter.IMP_MEST_STT_ID != null)
                {
                    listImpMestSttId = listImpMestSttId.Where(o => o == CastFilter.IMP_MEST_STT_ID).ToList();
                }

                if (CastFilter.IMP_MEST_STT_IDs != null)
                {
                    listImpMestSttId = listImpMestSttId.Where(o => CastFilter.IMP_MEST_STT_IDs.Contains(o)).ToList();
                }
                var listImpMestTypeId = new List<long> { 
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK

                    };//loại nhập "thu hồi"
                if (CastFilter.IMP_MEST_TYPE_IDs != null)
                {
                    listImpMestTypeId = listImpMestTypeId.Where(o => CastFilter.IMP_MEST_TYPE_IDs.Contains(o)).ToList();
                }
                var listImpMestViews = new List<V_HIS_IMP_MEST>();
                var bedRoom = new HisBedRoomManager().Get(new HisBedRoomFilterQuery() { IDs = CastFilter.EXACT_BED_ROOM_IDs ?? new List<long>() });
                List<long> roomIds = null;
                if (bedRoom != null && bedRoom.Count > 0)
                {
                    roomIds = bedRoom.Select(o => o.ROOM_ID).Distinct().ToList();
                }
                if (listImpMestSttId.Exists(o => o == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT))
                {
                    var metyFilterImpMest = new HisImpMestViewFilterQuery
                    {
                        IMP_MEST_TYPE_IDs = listImpMestTypeId,
                        //REQ_DEPARTMENT_IDs = CastFilter.REQ_DEPARTMENT_IDs,
                        //REQ_ROOM_IDs = roomIds ?? CastFilter.REQ_ROOM_IDs,
                        MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID, //Kho nhập vào
                        MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs //Kho nhập vào
                    };

                    metyFilterImpMest.IMP_MEST_STT_IDs = listImpMestSttId.Where(o => o == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT).ToList();
                    metyFilterImpMest.IMP_TIME_FROM = CastFilter.DATE_FROM;
                    metyFilterImpMest.IMP_TIME_TO = CastFilter.DATE_TO;
                    listImpMestViews.AddRange(new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterImpMest));
                   
                    if (CastFilter.MEDI_STOCK_CABINET_IDs != null)
                    {

                        listImpMestViews = listImpMestViews.Where(x =>CastFilter.MEDI_STOCK_CABINET_IDs.Contains( x.CHMS_MEDI_STOCK_ID??0)).ToList();
                    }
                }

                if (listImpMestSttId.Exists(o => o != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT))
                {
                    var metyFilterImpMest = new HisImpMestViewFilterQuery
                    {

                        IMP_MEST_TYPE_IDs = listImpMestTypeId,
                        //REQ_DEPARTMENT_IDs = CastFilter.REQ_DEPARTMENT_IDs,
                        //REQ_ROOM_IDs = roomIds ?? CastFilter.REQ_ROOM_IDs,
                        MEDI_STOCK_ID = CastFilter.MEDI_STOCK_ID, //Kho nhập vào
                        MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs //Kho nhập vào
                    };

                    metyFilterImpMest.IMP_MEST_STT_IDs = listImpMestSttId.Where(o => o != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT).ToList();
                    metyFilterImpMest.CREATE_TIME_FROM = CastFilter.DATE_FROM;
                    metyFilterImpMest.CREATE_TIME_TO = CastFilter.DATE_TO;
                    listImpMestViews.AddRange(new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterImpMest) ?? new List<V_HIS_IMP_MEST>());

                    if (CastFilter.MEDI_STOCK_CABINET_IDs != null)
                    {

                        listImpMestViews = listImpMestViews.Where(x => CastFilter.MEDI_STOCK_CABINET_IDs.Contains(x.CHMS_MEDI_STOCK_ID ?? 0)).ToList();
                    }
                }
                FixReq(listImpMestViews);
                if (CastFilter.REQ_DEPARTMENT_IDs != null)
                {
                    listImpMestViews = listImpMestViews.Where(o => CastFilter.REQ_DEPARTMENT_IDs.Contains(o.REQ_DEPARTMENT_ID ?? 0)).ToList();
                }
                if (CastFilter.REQ_ROOM_IDs != null)
                {
                    listImpMestViews = listImpMestViews.Where(o => CastFilter.REQ_ROOM_IDs.Contains(o.REQ_ROOM_ID ?? 0)).ToList();
                }
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ V_HIS_MOBA_IMP_MEST
                if (CastFilter.MEDI_STOCK_CABINET_IDs != null)
                {
                    listMobaImpMestViews = listImpMestViews.ToList();
                }
                else
                {
                    listMobaImpMestViews = listImpMestViews.Where(o => o.MOBA_EXP_MEST_ID > 0 || o.CHMS_TYPE_ID == CHMS_TYPE_HTCS).ToList();
                }

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ HIS_DEPARTMENT
                var listDepartments = listMobaImpMestViews.Where(s => s.REQ_DEPARTMENT_ID.HasValue).Select(s => s.REQ_DEPARTMENT_ID.Value).Distinct().ToList();
                //var listDerpartmentIds = new List<HIS_DEPARTMENT>(); 
                var skip = 0;
                while (listDepartments.Count - skip > 0)
                {
                    var listIds = listDepartments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterDerpartment = new HisDepartmentFilterQuery
                    {
                        IDs = listIds,
                    };
                    var departmentSubs = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilterDerpartment);
                    listHisDEpartments.AddRange(departmentSubs);
                }

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds2 = listMobaImpMestViews.Select(s => s.ID).ToList();
                skip = 0;
                while (listImpMestIds2.Count - skip > 0)
                {
                    var listIds = listImpMestIds2.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var impMestMedicineView = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine);
                    listImpMestMedicineViews.AddRange(impMestMedicineView);

                    var mateFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var impMestMaterialView = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(mateFilterImpMestMaterial);
                    listImpMestMaterialViews.AddRange(impMestMaterialView);
                }
                FixReq(listImpMestMedicineViews, listImpMestViews);
                FixReq(listImpMestMaterialViews, listImpMestViews);
                GetMediMate();
                GetMetyMaty();
                var aggrImpMestId = new List<long>();
                if (IsNotNullOrEmpty(listImpMestMedicineViews))
                {
                    aggrImpMestId.AddRange(listImpMestMedicineViews.Select(o => o.AGGR_IMP_MEST_ID ?? 0).ToList());
                }
                if (IsNotNullOrEmpty(listImpMestMaterialViews))
                {
                    aggrImpMestId.AddRange(listImpMestMaterialViews.Select(o => o.AGGR_IMP_MEST_ID ?? 0).ToList());
                }

                aggrImpMestId = aggrImpMestId.Distinct().ToList();

                if (IsNotNullOrEmpty(aggrImpMestId))
                {
                    skip = 0;
                    while (aggrImpMestId.Count - skip > 0)
                    {
                        var listIds = aggrImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var aggrFilterImpMest = new HisImpMestViewFilterQuery();
                        aggrFilterImpMest.IDs = listIds;
                        var aggrImpMestViews = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(aggrFilterImpMest);
                        if (IsNotNullOrEmpty(aggrImpMestViews))
                        {
                            listAggrMobaImpMestViews.AddRange(aggrImpMestViews);
                        }
                    }
                }

                var impMestTypeFilter = new HisImpMestTypeFilterQuery
                {
                };
                listImpMestType = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager(paramGet).Get(impMestTypeFilter);

                var listMobaExpMestId = listMobaImpMestViews.Where(o => o.MOBA_EXP_MEST_ID.HasValue).Select(p => p.MOBA_EXP_MEST_ID.Value).Distinct().ToList();
                if (listMobaExpMestId != null)
                {
                    skip = 0;
                    while (listMobaExpMestId.Count - skip > 0)
                    {
                        var listIds = listMobaExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var expMestFilter = new HisExpMestFilterQuery
                        {
                            IDs = listIds,
                        };
                        var mobaExpMestSubs = new HisExpMestManager(paramGet).Get(expMestFilter);
                        listMobaExpMest.AddRange(mobaExpMestSubs);
                    }
                }

                var listChmsExpMestId = listMobaImpMestViews.Where(o => o.CHMS_EXP_MEST_ID.HasValue).Select(p => p.CHMS_EXP_MEST_ID.Value).Distinct().ToList();
                if (listChmsExpMestId != null)
                {
                    skip = 0;
                    while (listChmsExpMestId.Count - skip > 0)
                    {
                        var listIds = listChmsExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var expMestFilter = new HisExpMestFilterQuery
                        {
                            IDs = listIds,
                        };
                        var chmsExpMestSubs = new HisExpMestManager(paramGet).Get(expMestFilter);
                        ListExpMest.AddRange(chmsExpMestSubs);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FixReq(List<V_HIS_IMP_MEST> listImpMestViews)
        {
            foreach (var item in listImpMestViews)
            {
                if (item.CHMS_MEDI_STOCK_ID != null)
                {
                    var room = HisMediStockCFG.HisMediStocks.FirstOrDefault(o=>o.ID == item.CHMS_MEDI_STOCK_ID);
                    if (room != null)
                    {
                        item.REQ_ROOM_ID = room.ROOM_ID;
                        item.REQ_DEPARTMENT_ID = room.DEPARTMENT_ID;
                        item.REQ_DEPARTMENT_CODE = room.DEPARTMENT_CODE;
                        item.REQ_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                    }
                }
            }
        }

        private void FixReq(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine, List<V_HIS_IMP_MEST> listImpMestViews)
        {
            foreach (var item in listImpMestMedicine)
            {
                if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS || item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL)
                {
                    var ImpMest = listImpMestViews.FirstOrDefault(o => o.ID == item.IMP_MEST_ID);
                    if (ImpMest != null && ImpMest.CHMS_MEDI_STOCK_ID != null)
                    {
                        var room = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == ImpMest.CHMS_MEDI_STOCK_ID);
                        if (room != null)
                        {
                            item.REQ_ROOM_ID = room.ROOM_ID;
                            item.REQ_DEPARTMENT_ID = room.DEPARTMENT_ID;
                        }
                    }
                }
            }
        }

        private void FixReq(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial, List<V_HIS_IMP_MEST> listImpMestViews)
        {
            foreach (var item in listImpMestMaterial)
            {
                if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS || item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL)
                {
                    var ImpMest = listImpMestViews.FirstOrDefault(o => o.ID == item.IMP_MEST_ID);
                    if (ImpMest != null && ImpMest.CHMS_MEDI_STOCK_ID != null)
                    {
                        var room = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == ImpMest.CHMS_MEDI_STOCK_ID);
                        if (room != null)
                        {
                            item.REQ_ROOM_ID = room.ROOM_ID;
                            item.REQ_DEPARTMENT_ID = room.DEPARTMENT_ID;
                        }
                    }
                }
            }
        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();
                if (listImpMestMedicineViews != null)
                {
                    medicineIds.AddRange(listImpMestMedicineViews.Select(o => o.MEDICINE_ID).ToList());
                }

                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                        Medicinefilter.IDs = limit;
                        var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                        Medicines.AddRange(MedicineSub);
                    }
                }

                List<long> materialIds = new List<long>();
                if (listImpMestMaterialViews != null)
                {
                    materialIds.AddRange(listImpMestMaterialViews.Select(o => o.MATERIAL_ID).ToList());
                }

                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        Materials.AddRange(MaterialSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }
        private void GetMetyMaty()
        {
            try
            {
                List<long> medicineTypeIds = new List<long>();
                if (listImpMestMedicineViews != null)
                {
                    medicineTypeIds.AddRange(listImpMestMedicineViews.Select(o => o.MEDICINE_TYPE_ID).ToList());
                }

                medicineTypeIds = medicineTypeIds.Distinct().ToList();

                if (medicineTypeIds != null && medicineTypeIds.Count > 0)
                {
                    HisMedicineTypeViewFilterQuery MedicineTypefilter = new HisMedicineTypeViewFilterQuery();
                    var MedicineTypeSub = new HisMedicineTypeManager().GetView(MedicineTypefilter);
                    MedicineTypes = MedicineTypeSub.Where(o => medicineTypeIds.Contains(o.ID)).ToList();
                }

                List<long> materialTypeIds = new List<long>();
                if (listImpMestMaterialViews != null)
                {
                    materialTypeIds.AddRange(listImpMestMaterialViews.Select(o => o.MATERIAL_TYPE_ID).ToList());
                }

                materialTypeIds = materialTypeIds.Distinct().ToList();

                if (materialTypeIds != null && materialTypeIds.Count > 0)
                {

                    HisMaterialTypeViewFilterQuery MaterialTypefilter = new HisMaterialTypeViewFilterQuery();
                    var MaterialTypeSub = new HisMaterialTypeManager().GetView(MaterialTypefilter);
                    MaterialTypes = MaterialTypeSub.Where(o => materialTypeIds.Contains(o.ID)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listMobaImpMestViews))
                {
                    ProcessFilterData(listMobaImpMestViews, listImpMestMedicineViews);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            // Đẩy ngày tháng với khoa phòng sang excel nà :D
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.DATE_FROM ?? 0));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.DATE_TO ?? 0));
            dicSingleTag.Add("DEPARTMENT_NAME", MEDI_STOCK_NAME);
            dicSingleTag.Add("IMP_MEST_STT_NAME", IMP_MEST_STT_NAME);
            dicSingleTag.Add("MEDI_STOCK_NAMEs", MEDI_STOCK_NAME);
            dicSingleTag.Add("IMP_MEST_STT_NAMEs", IMP_MEST_STT_NAME);
            dicSingleTag.Add("TOTAL_ALL_PRICE", listRdo.Sum(s => (s.TOTAL_PRICE ?? 0)));
            dicSingleTag.Add("TOTAL_ALL_PRICE_TO_STRING", Inventec.Common.String.Convert.CurrencyToVneseString(listRdo.Sum(s => (s.TOTAL_PRICE ?? 0)).ToString()));

            objectTag.AddObjectData(store, "Department", listHisDEpartments);
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddRelationship(store, "Department", "Report", "ID", "DEPARTMENT_ID");
            objectTag.AddObjectData(store, "TotalPrice", listTotalPrices);
            objectTag.AddObjectData(store, "MedicineType", listMedicineTypeRdo.OrderBy(o => o.IMP_MEST_TYPE_ID).ThenBy(p => p.IMP_MEST_CODE).ThenBy(p => p.AGGR_IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "ReportMediMate", listMediMateTypeRdo.OrderBy(o => o.IMP_MEST_TYPE_ID).ThenBy(p => p.IMP_MEST_CODE).ThenBy(p => p.AGGR_IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "ReportMediMateNoChms", listMediMateTypeRdo.Where(o=>string.IsNullOrWhiteSpace(o.CHMS_MEDI_STOCK_CODE)).OrderBy(o => o.IMP_MEST_TYPE_ID).ThenBy(p => p.IMP_MEST_CODE).ThenBy(p => p.AGGR_IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "ReportMediMateYesChms", listMediMateTypeRdo.Where(o => !string.IsNullOrWhiteSpace(o.CHMS_MEDI_STOCK_CODE)).OrderBy(o => o.IMP_MEST_TYPE_ID).ThenBy(p => p.IMP_MEST_CODE).ThenBy(p => p.AGGR_IMP_MEST_CODE).ToList());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        private void ProcessFilterData(List<V_HIS_IMP_MEST> _listMobaImpMestViews, List<V_HIS_IMP_MEST_MEDICINE> _listImpMestMedicineViews)
        {
            try
            {
                //var number = 1; 
                var numberOrder = 0;
                // lấy ra các phiếu thuốc trong từng khoa
                var listMobaImpMestGroupByDepartmentIds = _listMobaImpMestViews.GroupBy(s => s.REQ_DEPARTMENT_ID).ToList();
                // mỗi lần lặp là lặp từng khoa, trong mỗi khoa có nhiều phiếu thuốc.
                foreach (var listMobaImpMestGroupByDepartmentId in listMobaImpMestGroupByDepartmentIds)
                {
                    var listImpMestInMobaImpMestIds = listMobaImpMestGroupByDepartmentId.Select(s => s.ID).ToList();
                    var listMobaImpMestMedicine = _listImpMestMedicineViews.Where(s => listImpMestInMobaImpMestIds.Contains(s.IMP_MEST_ID)).ToList();
                    var listMedicineGroupByNames = listMobaImpMestMedicine.GroupBy(s => s.MEDICINE_TYPE_NAME).ToList();
                    foreach (var listMedicineGroupByName in listMedicineGroupByNames)
                    {
                        var listMedicineGroupByPrices = listMedicineGroupByName.GroupBy(s => new { s.IMP_PRICE, s.IMP_VAT_RATIO }).ToList();
                        foreach (var listMedicineGroupByPrice in listMedicineGroupByPrices)
                        {
                            var listMedicineGroupByNationals = listMedicineGroupByPrice.GroupBy(s => s.NATIONAL_NAME).ToList();
                            foreach (var listMedicineGroupByNational in listMedicineGroupByNationals)
                            {
                                numberOrder = numberOrder + 1;
                                var price = listMedicineGroupByPrice.First().IMP_PRICE * (1 + listMedicineGroupByPrice.First().IMP_VAT_RATIO);
                                var amount = listMedicineGroupByNational.Sum(s => s.AMOUNT);
                                var departmentId = listMobaImpMestGroupByDepartmentId.Where(s => s.REQ_DEPARTMENT_ID.HasValue).First().REQ_DEPARTMENT_ID.Value;
                                var rdo = new VSarReportMrs00153RDO
                                {
                                    DEPARTMENT_ID = departmentId,
                                    NUMBER_ORDER = numberOrder,
                                    MEDICINE_TYPE_NAME = listMedicineGroupByNational.First().MEDICINE_TYPE_NAME,
                                    SERVICE_UNIT_NAME = listMedicineGroupByNational.First().SERVICE_UNIT_NAME,
                                    NATIONAL_NAME = listMedicineGroupByNational.First().NATIONAL_NAME,
                                    PRICE = price,
                                    AMOUNT = amount,
                                    TOTAL_PRICE = price * amount
                                };
                                listRdo.Add(rdo);
                            };
                        };
                    };
                }

                var listSarReportMrs00153RdoGroupbyDepartmentIds = listRdo.GroupBy(s => s.DEPARTMENT_ID).ToList();
                foreach (var listSarReportMrs00153RdoGroupbyDepartmentId in listSarReportMrs00153RdoGroupbyDepartmentIds)
                {
                    var totalPrice = new TotalPrice
                    {
                        DEPARTMENT_ID = listSarReportMrs00153RdoGroupbyDepartmentId.Key,
                        TOTAL_PRICE = listSarReportMrs00153RdoGroupbyDepartmentId.Sum(s => s.TOTAL_PRICE)
                    };
                    listTotalPrices.Add(totalPrice);
                }

                var groupMedicineType = _listImpMestMedicineViews.GroupBy(s => new { s.MEDICINE_TYPE_NAME, s.PACKAGE_NUMBER, s.EXPIRED_DATE, s.NATIONAL_NAME, s.IMP_PRICE, s.IMP_VAT_RATIO, s.SERVICE_UNIT_NAME }).ToList();
                foreach (var gr in groupMedicineType)
                {
                    List<V_HIS_IMP_MEST_MEDICINE> listSub = gr.ToList<V_HIS_IMP_MEST_MEDICINE>();
                    MedicineTypeRdo rdo = new MedicineTypeRdo(listSub.First(), Medicines);
                    var medicineType = MedicineTypes.FirstOrDefault(o => gr.First().MEDICINE_TYPE_CODE == o.MEDICINE_TYPE_CODE);
                    if (medicineType != null)
                    {
                        rdo.MEDICINE_GROUP_ID = gr.First().MEDICINE_GROUP_ID ?? 0;
                        rdo.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                        rdo.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                    }
                    rdo.PRICE = gr.Key.IMP_PRICE;
                    rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                    rdo.TOTAL_PRICE = listSub.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT);
                    rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_TOTAL_PRICE = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));
                    rdo.DIC_ROOM_AMOUNT = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    rdo.DIC_ROOM_TOTAL_PRICE = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));
                    listMedicineTypeRdo.Add(rdo);
                }

                if (IsNotNullOrEmpty(_listImpMestMedicineViews))
                {
                    var groupMedicineTypeHasAggr = _listImpMestMedicineViews.Where(o => o.AGGR_IMP_MEST_ID.HasValue).GroupBy(s => new { s.MEDICINE_TYPE_CODE, s.PACKAGE_NUMBER, s.EXPIRED_DATE, s.NATIONAL_NAME, s.IMP_PRICE, s.IMP_VAT_RATIO, s.AGGR_IMP_MEST_ID, s.REQ_DEPARTMENT_ID }).ToList();
                    foreach (var gr in groupMedicineTypeHasAggr)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listSub = gr.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        MedicineTypeRdo rdo = new MedicineTypeRdo(listSub.First(), Medicines);
                        rdo.PRICE = gr.Key.IMP_PRICE;
                        rdo.VAT_RATIO = gr.Key.IMP_VAT_RATIO;
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                        rdo.TOTAL_PRICE = listSub.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT);
                        rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_ROOM_AMOUNT = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_ROOM_TOTAL_PRICE = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));
                        if (gr.Key.AGGR_IMP_MEST_ID.HasValue)
                        {
                            var impMest = listAggrMobaImpMestViews.FirstOrDefault(o => o.ID == gr.Key.AGGR_IMP_MEST_ID.Value);
                            if (impMest != null)
                            {
                                var expMest = ListExpMest.Where(x => x.ID == impMest.CHMS_EXP_MEST_ID).FirstOrDefault();
                                if (expMest != null)
                                {
                                    rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                                }

                                rdo.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                                rdo.AGGR_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                                rdo.START_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                                rdo.IMP_MEST_TYPE_NAME = impMest.IMP_MEST_TYPE_NAME;
                                rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                                rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;

                                rdo.REQ_TIME = impMest.CREATE_TIME ?? 0;
                                rdo.APPROVAL_TIME = impMest.APPROVAL_TIME ?? 0;
                            }

                        }
                        var listImpMestSub = listMobaImpMestViews.Where(o => listSub.Exists(p => p.IMP_MEST_ID == o.ID)).ToList();
                        //if (listImpMestSub != null && listMobaExpMest!=null)
                        //{
                        //    var listMobaExpMestSub = listMobaExpMest.Where(o => listImpMestSub.Exists(p => p.MOBA_EXP_MEST_ID == o.ID)).ToList();
                        //    rdo.MOBA_EXP_MEDI_STOCK_NAME = string.Join("-", HisMediStockCFG.HisMediStocks.Where(o => listMobaExpMestSub.Exists(p => p.MEDI_STOCK_ID == o.ID)).Select(q => q.MEDI_STOCK_NAME).ToList());
                        //    rdo.MOBA_EXP_MEDI_STOCK_CODE = string.Join("-", HisMediStockCFG.HisMediStocks.Where(o => listMobaExpMestSub.Exists(p => p.MEDI_STOCK_ID == o.ID)).Select(q => q.MEDI_STOCK_CODE).ToList());
                        //}
                        var medicineType = MedicineTypes.FirstOrDefault(o => gr.First().MEDICINE_TYPE_CODE == o.MEDICINE_TYPE_CODE);
                        if (medicineType != null)
                        {
                            rdo.MEDICINE_GROUP_ID = gr.First().MEDICINE_GROUP_ID ?? 0;
                            rdo.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                            rdo.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        }

                        listMediMateTypeRdo.Add(rdo);
                    }
                    var groupMedicineTypeNoAggr = _listImpMestMedicineViews.Where(o => !o.AGGR_IMP_MEST_ID.HasValue).GroupBy(s => new { s.MEDICINE_TYPE_CODE, s.PACKAGE_NUMBER, s.EXPIRED_DATE, s.NATIONAL_NAME, s.IMP_PRICE, s.IMP_VAT_RATIO, s.IMP_MEST_ID, s.REQ_DEPARTMENT_ID }).ToList();
                    foreach (var gr in groupMedicineTypeNoAggr)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listSub = gr.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        MedicineTypeRdo rdo = new MedicineTypeRdo(listSub.First(), Medicines);
                        rdo.PRICE = gr.Key.IMP_PRICE;
                        rdo.VAT_RATIO = gr.Key.IMP_VAT_RATIO;
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                        rdo.TOTAL_PRICE = listSub.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT);
                        rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID.Value).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_TOTAL_PRICE = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID ?? 0).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.IMP_PRICE * s.AMOUNT));
                        rdo.DIC_ROOM_AMOUNT = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_ROOM_TOTAL_PRICE = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));
                        //rdo.AGGR_IMP_MEST_CODE = rdo.IMP_MEST_CODE;
                        if (this.listImpMestType != null)
                        {
                            var impMestType = this.listImpMestType.FirstOrDefault(o => o.ID == rdo.IMP_MEST_TYPE_ID);
                            if (impMestType != null)
                            {
                                rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;

                            }
                        }
                        var impMest = _listMobaImpMestViews.FirstOrDefault(o => o.ID == gr.Key.IMP_MEST_ID);
                        if (impMest != null)
                        {
                            var expMest = ListExpMest.Where(x => x.ID == impMest.CHMS_EXP_MEST_ID).FirstOrDefault();
                            if (expMest != null)
                            {
                                rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                            }

                            rdo.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                            rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                            rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;
                            rdo.REQ_TIME = impMest.CREATE_TIME ?? 0;
                            rdo.APPROVAL_TIME = impMest.APPROVAL_TIME ?? 0;
                            rdo.CHMS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                            rdo.CHMS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                            rdo.CHMS_DEPARTMENT_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_CODE;
                            rdo.CHMS_DEPARTMENT_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_NAME;

                            if (listMobaExpMest != null)
                            {
                                var mobaExpMest = listMobaExpMest.FirstOrDefault(o => impMest.MOBA_EXP_MEST_ID == o.ID && impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
                                if (mobaExpMest != null)
                                {
                                    rdo.MOBA_EXP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => mobaExpMest.MEDI_STOCK_ID == o.ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                                    rdo.MOBA_EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => mobaExpMest.MEDI_STOCK_ID == o.ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                                    rdo.CONTAIN_DEPARTMENT_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == mobaExpMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_CODE;
                                    rdo.CONTAIN_DEPARTMENT_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == mobaExpMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_NAME;
                                }
                            }
                        }
                        var medicineType = MedicineTypes.FirstOrDefault(o => gr.Key.MEDICINE_TYPE_CODE == o.MEDICINE_TYPE_CODE);
                        if (medicineType != null)
                        {
                            rdo.MEDICINE_GROUP_ID = gr.First().MEDICINE_GROUP_ID ?? 0;
                            rdo.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                            rdo.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        }
                        listMediMateTypeRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listImpMestMaterialViews))
                {
                    var groupMaterialTypeHasAggr = listImpMestMaterialViews.Where(o => o.AGGR_IMP_MEST_ID.HasValue).GroupBy(s => new { s.MATERIAL_TYPE_CODE, s.PACKAGE_NUMBER, s.EXPIRED_DATE, s.NATIONAL_NAME, s.IMP_PRICE, s.IMP_VAT_RATIO, s.AGGR_IMP_MEST_ID, s.REQ_DEPARTMENT_ID }).ToList();
                    foreach (var gr in groupMaterialTypeHasAggr)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listSub = gr.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        MedicineTypeRdo rdo = new MedicineTypeRdo(listSub.First(), Materials);
                        rdo.PRICE = gr.Key.IMP_PRICE;
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                        rdo.TOTAL_PRICE = listSub.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT);
                        rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID.Value).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_TOTAL_PRICE = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID ?? 0).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.IMP_PRICE * s.AMOUNT));
                        rdo.DIC_ROOM_AMOUNT = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_ROOM_TOTAL_PRICE = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));

                        if (gr.Key.AGGR_IMP_MEST_ID.HasValue)
                        {
                            var impMest = listAggrMobaImpMestViews.FirstOrDefault(o => o.ID == gr.Key.AGGR_IMP_MEST_ID.Value);
                            if (impMest != null)
                            {
                                var expMest = ListExpMest.Where(x => x.ID == impMest.CHMS_EXP_MEST_ID).FirstOrDefault();
                                if (expMest != null)
                                {
                                    rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                                }
                                rdo.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                                rdo.AGGR_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                                rdo.START_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                                rdo.IMP_MEST_TYPE_NAME = impMest.IMP_MEST_TYPE_NAME;
                                rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                                rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;
                                rdo.REQ_TIME = impMest.CREATE_TIME ?? 0;
                                rdo.APPROVAL_TIME = impMest.APPROVAL_TIME ?? 0;
                            }
                        }
                        //var listImpMestSub = listMobaImpMestViews.Where(o => listSub.Exists(p => p.IMP_MEST_ID == o.ID)).ToList();
                        //if (listImpMestSub != null && listMobaExpMest != null)
                        //{
                        //    var listMobaExpMestSub = listMobaExpMest.Where(o => listImpMestSub.Exists(p => p.MOBA_EXP_MEST_ID == o.ID)).ToList();
                        //    rdo.MOBA_EXP_MEDI_STOCK_NAME = string.Join("-", HisMediStockCFG.HisMediStocks.Where(o => listMobaExpMestSub.Exists(p => p.MEDI_STOCK_ID == o.ID)).Select(q => q.MEDI_STOCK_NAME).ToList());
                        //    rdo.MOBA_EXP_MEDI_STOCK_CODE = string.Join("-", HisMediStockCFG.HisMediStocks.Where(o => listMobaExpMestSub.Exists(p => p.MEDI_STOCK_ID == o.ID)).Select(q => q.MEDI_STOCK_CODE).ToList());
                        //}

                        rdo.MEDICINE_GROUP_ID = 0;
                        rdo.MEDICINE_GROUP_NAME = "Vật tư";
                        rdo.MEDICINE_GROUP_CODE = "VTU";
                        listMediMateTypeRdo.Add(rdo);
                    }
                    var groupMaterialTypeNoAggr = listImpMestMaterialViews.Where(o => !o.AGGR_IMP_MEST_ID.HasValue).GroupBy(s => new { s.MATERIAL_TYPE_CODE, s.PACKAGE_NUMBER, s.EXPIRED_DATE, s.NATIONAL_NAME, s.IMP_PRICE, s.IMP_VAT_RATIO, s.IMP_MEST_ID, s.REQ_DEPARTMENT_ID }).ToList();
                    foreach (var gr in groupMaterialTypeNoAggr)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listSub = gr.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        MedicineTypeRdo rdo = new MedicineTypeRdo(listSub.First(), Materials);
                        rdo.PRICE = gr.Key.IMP_PRICE;
                        rdo.AMOUNT = listSub.Sum(s => s.AMOUNT);
                        rdo.TOTAL_PRICE = listSub.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT);
                        rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => { var r = DepartmentCode(o.REQ_DEPARTMENT_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_AMOUNT = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID.Value).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.AMOUNT));
                        //rdo.DIC_TOTAL_PRICE = listSub.Where(r => r.REQ_DEPARTMENT_ID.HasValue).GroupBy(o => o.REQ_DEPARTMENT_ID ?? 0).ToDictionary(p => DepartmentCode(p.Key), q => q.Sum(s => s.IMP_PRICE * s.AMOUNT));
                        rdo.DIC_ROOM_AMOUNT = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        rdo.DIC_ROOM_TOTAL_PRICE = listSub.Where(r => r.REQ_ROOM_ID.HasValue).GroupBy(o => { var r = RoomCode(o.REQ_ROOM_ID.Value); return r; }).ToDictionary(p => p.Key, q => q.Sum(s => s.IMP_PRICE * (1 + s.IMP_VAT_RATIO) * s.AMOUNT));
                        //rdo.AGGR_IMP_MEST_CODE = rdo.IMP_MEST_CODE;
                        if (this.listImpMestType != null)
                        {
                            var impMestType = this.listImpMestType.FirstOrDefault(o => o.ID == rdo.IMP_MEST_TYPE_ID);
                            if (impMestType != null)
                            {
                                rdo.IMP_MEST_TYPE_NAME = impMestType.IMP_MEST_TYPE_NAME;

                            }
                        }
                        var impMest = _listMobaImpMestViews.FirstOrDefault(o => o.ID == gr.Key.IMP_MEST_ID);
                        if (impMest != null)
                        {
                            var expMest = ListExpMest.Where(x => x.ID == impMest.CHMS_EXP_MEST_ID).FirstOrDefault();
                            if (expMest != null)
                            {
                                rdo.EXP_MEST_CODE = expMest.EXP_MEST_CODE;
                            }
                            rdo.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                            rdo.MEDI_STOCK_CODE = impMest.MEDI_STOCK_CODE;
                            rdo.MEDI_STOCK_NAME = impMest.MEDI_STOCK_NAME;
                            rdo.REQ_TIME = impMest.CREATE_TIME ?? 0;
                            rdo.APPROVAL_TIME = impMest.APPROVAL_TIME ?? 0;
                            rdo.CHMS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                            rdo.CHMS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                            rdo.CHMS_DEPARTMENT_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_CODE;
                            rdo.CHMS_DEPARTMENT_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_NAME;

                            if (listMobaExpMest != null)
                            {
                                var mobaExpMest = listMobaExpMest.FirstOrDefault(o => impMest.MOBA_EXP_MEST_ID == o.ID && impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL);
                                if (mobaExpMest != null)
                                {
                                    rdo.MOBA_EXP_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => mobaExpMest.MEDI_STOCK_ID == o.ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                                    rdo.MOBA_EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => mobaExpMest.MEDI_STOCK_ID == o.ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                                    rdo.CONTAIN_DEPARTMENT_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == mobaExpMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_CODE;
                                    rdo.CONTAIN_DEPARTMENT_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == mobaExpMest.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).DEPARTMENT_NAME;
                                }
                            }
                        }
                        rdo.MEDICINE_GROUP_ID = 0;
                        rdo.MEDICINE_GROUP_NAME = "Vật tư";
                        rdo.MEDICINE_GROUP_CODE = "VTU";
                        listMediMateTypeRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private string DepartmentCode(long departmentId)
        {
            string result = "";
            try
            {
                var dp = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentId);
                if (dp != null)
                    result = dp.DEPARTMENT_CODE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string RoomCode(long roomId)
        {
            string result = "";
            try
            {
                var ro = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == roomId && o.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__BUONG);
                if (ro != null)
                    result = ro.ROOM_CODE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }
    }
}