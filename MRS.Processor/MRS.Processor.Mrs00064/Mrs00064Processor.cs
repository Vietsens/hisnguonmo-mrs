using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMestStt;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisImpMestMaterial;

namespace MRS.Processor.Mrs00064
{
    public class Mrs00064Processor : AbstractProcessor
    {
        Mrs00064Filter castFilter = null;
        List<Mrs00064RDO> ListRdoMedicine = new List<Mrs00064RDO>();
        List<Mrs00064RDO> ListRdoMaterial = new List<Mrs00064RDO>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_EXP_MEST> hisDepaExpMests;

        string deparment_name;
        string room_name;

        public Mrs00064Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00064Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00064Filter)this.reportFilter);

                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPA_EXP_MEST MRS00064." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisExpMestViewFilterQuery depaExpFilter = new HisExpMestViewFilterQuery();
                depaExpFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                depaExpFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                depaExpFilter.EXP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT, IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT };
                depaExpFilter.REQ_ROOM_ID = castFilter.ROOM_ID;
                depaExpFilter.REQ_ROOM_IDs = castFilter.ROOM_IDs;
                depaExpFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                depaExpFilter.REQ_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                depaExpFilter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                depaExpFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; // IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                hisDepaExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(depaExpFilter);

                ListMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());

                ListMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00064.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(hisDepaExpMests))
                {
                    ProcessListDepaExpMest(paramGet, hisDepaExpMests);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetRoom()
        {
            try
            {
                if (castFilter.ROOM_ID.HasValue)
                {
                    var room = new HisRoomManager().GetView(new HisRoomViewFilterQuery() { ID = castFilter.ROOM_ID.Value }).FirstOrDefault();
                    if (IsNotNull(room))
                    {
                        deparment_name = room.DEPARTMENT_NAME;
                        room_name = room.ROOM_NAME;
                    }
                }
                else
                {
                    var department = new HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID??0);
                    if (IsNotNull(department))
                    {
                        deparment_name = department.DEPARTMENT_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListDepaExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> hisDepaExpMests)
        {
            try
            {
                if (IsNotNullOrEmpty(hisDepaExpMests))
                {
                    int start = 0;
                    int count = hisDepaExpMests.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<long> listExpMestId = hisDepaExpMests.Skip(start).Take(limit).Select(s => s.ID).ToList();
                        HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                        expMediFilter.EXP_MEST_IDs = listExpMestId;
                        expMediFilter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID;
                        expMediFilter.IS_EXPORT = true;

                        List<V_HIS_EXP_MEST_MEDICINE> hisExpMedicines = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                        ProcessDetailExpMestMedicine(paramGet, listExpMestId, hisExpMedicines);
                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.EXP_MEST_IDs = listExpMestId;
                        expMateFilter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID;
                        expMateFilter.IS_EXPORT = true;

                        List<V_HIS_EXP_MEST_MATERIAL> hisExpMaterials = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                        ProcessDetailExpMestMaterial(paramGet, listExpMestId, hisExpMaterials);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tổng hợp số lượng các loại thuốc
        // Số lượng xuất sử dụng
        private void ProcessDetailExpMestMedicine(CommonParam paramGet, List<long> listExpMestId, List<V_HIS_EXP_MEST_MEDICINE> hisExpMedicines)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMedicines))
                {
                    var Groups = hisExpMedicines.GroupBy(g => g.MEDICINE_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listMediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00064RDO rdo = new Mrs00064RDO();
                        rdo.MEDICINE_TYPE_ID = listMediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listMediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listMediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listMediSub.First().SERVICE_UNIT_NAME;
                        rdo.AMOUNT = listMediSub.Sum(s => s.AMOUNT);
                        rdo.PRICE = listMediSub.First().IMP_PRICE;
                        rdo.VIR_TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                        GetConcentraByMedicineTypeId(paramGet, rdo, listMediSub.First().MEDICINE_TYPE_ID);
                        ListRdoMedicine.Add(rdo);
                    }
                    ProcessImpMoveBackMedicine(paramGet, listExpMestId);
                    ListRdoMedicine = ListRdoMedicine.GroupBy(g => g.MEDICINE_TYPE_ID).Select(s => new Mrs00064RDO { MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, CONCENTRA = s.First().CONCENTRA, AMOUNT = s.Sum(s1 => s1.AMOUNT), PRICE = s.First().PRICE, VIR_TOTAL_PRICE = s.Sum(s1 => s1.VIR_TOTAL_PRICE) }).ToList();
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu thuoc MRS00064");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoMedicine.Clear();
            }
        }

        //Tổng hợp số lượng các loại thuốc
        //Số lượng nhập thu hồi theo phiếu xuất
        private void ProcessImpMoveBackMedicine(CommonParam paramGet, List<long> listExpMestId)
        {
            try
            {
                HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                mobaFilter.MOBA_EXP_MEST_IDs = listExpMestId;
                List<V_HIS_IMP_MEST> hisMobaImpMests = new HisImpMestManager(paramGet).GetView(mobaFilter);
                if (IsNotNullOrEmpty(hisMobaImpMests))
                {
                    int start = 0;
                    int count = hisMobaImpMests.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_IMP_MEST> mobaImpMests = hisMobaImpMests.Skip(start).Take(limit).ToList();
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; // Config.HisImpMestSttCFG.IMP_MEST_STT_ID__IMPORTED; 
                        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                        ProcessDetailImpMobaMedicine(paramGet, hisImpMestMedicines);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDetailImpMobaMedicine(CommonParam paramGet, List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicines)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicines))
                {
                    var Groups = hisImpMestMedicines.GroupBy(g => g.MEDICINE_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listMediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00064RDO rdo = new Mrs00064RDO();
                        rdo.MEDICINE_TYPE_ID = listMediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listMediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listMediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listMediSub.First().SERVICE_UNIT_NAME;
                        rdo.AMOUNT = listMediSub.Sum(s => -(s.AMOUNT));
                        rdo.PRICE = listMediSub.First().IMP_PRICE;
                        rdo.VIR_TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                        GetConcentraByMedicineTypeId(paramGet, rdo, listMediSub.First().MEDICINE_TYPE_ID);
                        ListRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetConcentraByMedicineTypeId(CommonParam paramGet, Mrs00064RDO rdo, long MedicineTypeId)
        {
            try
            {
                var medicineType = ListMedicineType.SingleOrDefault(s => s.ID == MedicineTypeId);
                if (medicineType != null)
                {
                    rdo.CONCENTRA = medicineType.CONCENTRA;
                }
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tổng hợp số lượng các loại vật tư
        //Số lượng xuất sử dụng
        private void ProcessDetailExpMestMaterial(CommonParam paramGet, List<long> listExpMestId, List<V_HIS_EXP_MEST_MATERIAL> hisExpMaterials)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMaterials))
                {
                    var Groups = hisExpMaterials.GroupBy(g => g.MATERIAL_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listMateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00064RDO rdo = new Mrs00064RDO();
                        rdo.MATERIAL_TYPE_ID = listMateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listMateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listMateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listMateSub.First().SERVICE_UNIT_NAME;
                        rdo.AMOUNT = listMateSub.Sum(s => s.AMOUNT);
                        rdo.PRICE = listMateSub.First().IMP_PRICE;
                        rdo.VIR_TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                        GetConcentraByMaterialTypeId(paramGet, rdo, listMateSub.First().MATERIAL_TYPE_ID);
                        ListRdoMaterial.Add(rdo);
                    }
                    ProcessImpMoveBackMaterial(paramGet, listExpMestId);
                    ListRdoMaterial = ListRdoMaterial.GroupBy(g => g.MATERIAL_TYPE_ID).Select(s => new Mrs00064RDO { MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, CONCENTRA = s.First().CONCENTRA, AMOUNT = s.Sum(s1 => s1.AMOUNT), PRICE = s.First().PRICE, VIR_TOTAL_PRICE = s.Sum(s1 => s1.VIR_TOTAL_PRICE) }).ToList();
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu vat tu MRS00064");
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListMaterialType.Clear();
            }
        }

        //Tổng hợp số lượng các loại vật tư
        //Số lượng nhập thu hồi theo phiếu xuất
        private void ProcessImpMoveBackMaterial(CommonParam paramGet, List<long> listExpMestId)
        {
            try
            {
                HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                mobaFilter.MOBA_EXP_MEST_IDs = listExpMestId;
                List<V_HIS_IMP_MEST> hisMobaImpMests = new HisImpMestManager(paramGet).GetView(mobaFilter);
                if (IsNotNullOrEmpty(hisMobaImpMests))
                {
                    int start = 0;
                    int count = hisMobaImpMests.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_IMP_MEST> mobaImpMests = hisMobaImpMests.Skip(start).Take(limit).ToList();
                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; // Config.HisImpMestSttCFG.IMP_MEST_STT_ID__IMPORTED; 
                        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                        ProcessDetailImpMobaMaterial(paramGet, hisImpMestMaterials);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDetailImpMobaMaterial(CommonParam paramGet, List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterials)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMaterials))
                {
                    var Groups = hisImpMestMaterials.GroupBy(g => g.MATERIAL_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listMediSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00064RDO rdo = new Mrs00064RDO();
                        rdo.MATERIAL_TYPE_ID = listMediSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listMediSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listMediSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listMediSub.First().SERVICE_UNIT_NAME;
                        rdo.AMOUNT = listMediSub.Sum(s => -(s.AMOUNT));
                        rdo.PRICE = listMediSub.First().IMP_PRICE;
                        rdo.VIR_TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                        GetConcentraByMedicineTypeId(paramGet, rdo, listMediSub.First().MATERIAL_TYPE_ID);
                        ListRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetConcentraByMaterialTypeId(CommonParam paramGet, Mrs00064RDO rdo, long MaterialTypeId)
        {
            try
            {
                var materialType = ListMaterialType.SingleOrDefault(s => s.ID == MaterialTypeId);
                if (materialType != null)
                {
                    rdo.CONCENTRA = materialType.CONCENTRA;
                }
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                GetRoom();
                dicSingleTag.Add("DEPARTMENT_NAME", deparment_name);
                if (castFilter.ROOM_ID > 0)
                {
                    dicSingleTag.Add("ROOM_NAME", room_name);
                }
                else
                {
                    dicSingleTag.Add("ROOM_NAME", null);
                }

                objectTag.AddObjectData(store, "ReportMedicine", ListRdoMedicine);
                objectTag.AddObjectData(store, "ReportMaterial", ListRdoMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
