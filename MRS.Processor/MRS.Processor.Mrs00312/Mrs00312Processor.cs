using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using Inventec.Common.Logging;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisExpMestReason;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisBed;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisMestRoom;
using MOS.MANAGER.HisRoom;

namespace MRS.Processor.Mrs00312
{
    class Mrs00312Processor : AbstractProcessor
    {
        Mrs00312Filter castFilter = null;
        Dictionary<string, Mrs00312RDO> dicRDO = new Dictionary<string, Mrs00312RDO>();
        List<Mrs00312RDO> listRdo = new List<Mrs00312RDO>();
        List<Mrs00312RDO> listParentRdo = new List<Mrs00312RDO>();
        V_HIS_MEDI_STOCK mediStock = null;
        List<V_HIS_MEDI_STOCK> mediStocks = new List<V_HIS_MEDI_STOCK>();
        Dictionary<string, PLACE_USE> dicPlaceUse = new Dictionary<string, PLACE_USE>();

        List<V_HIS_MEDICINE_TYPE> listMedicineType = null;
        List<V_HIS_MATERIAL_TYPE> listMaterialType = null;

        //List<HIS_IMP_MEST> listImp = new List<HIS_IMP_MEST>();
        //List<V_HIS_IMP_MEST_MEDICINE> listImpMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        //List<V_HIS_IMP_MEST_MATERIAL> listImpMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();

        List<DataGet> listGet = new List<DataGet>();

        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        //DS nha cung cap
        List<HIS_SUPPLIER> Suppliers = new List<HIS_SUPPLIER>();
        //DS nguon nhap
        List<HIS_IMP_SOURCE> ImpSources = new List<HIS_IMP_SOURCE>();
        //DS loai xuat
        List<HIS_EXP_MEST_TYPE> ExpMestTypes = new List<HIS_EXP_MEST_TYPE>();
        //DS loai nhap
        List<HIS_IMP_MEST_TYPE> ImpMestTypes = new List<HIS_IMP_MEST_TYPE>();
        //DS li do xuat
        List<HIS_EXP_MEST_REASON> ExpMestReasons = new List<HIS_EXP_MEST_REASON>();
        //nhom bao cao
        Dictionary<string, object> dicDepartment = new Dictionary<string, object>();
        Dictionary<string, object> dicRoom = new Dictionary<string, object>();

        Dictionary<string, TotalAmountStore> dicTotalAmount = new Dictionary<string, TotalAmountStore>();

        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();

        List<long> ListRoomId = new List<long>();
        List<PLACE_USE> filterPlaceUse = new List<PLACE_USE>();
        int THUOC = 1;
        int VAT_TU = 2;
        int HOA_CHAT = 3;
        string KeyGroup = "_{2}_{3}_{4}";
        public Mrs00312Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00312Filter);
        }

        protected override bool GetData()
        {
            bool valid = true;
            try
            {
                this.castFilter = (Mrs00312Filter)this.reportFilter;
                //Revert dieu kien loc theo kho
                if (castFilter.REVERT_MEDI_STOCK_IDs != null)
                {
                    castFilter.MEDI_STOCK_IDs = HisMediStockCFG.HisMediStocks.Where(o => !castFilter.REVERT_MEDI_STOCK_IDs.Contains(o.ID)).Select(p => p.ID).ToList();
                }
                if (this.castFilter.PLACE_DEPARTMENT_IDs != null)
                {
                    foreach (var item in this.castFilter.PLACE_DEPARTMENT_IDs)
                    {
                        var depa = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item);
                        if (depa != null)
                        {
                            filterPlaceUse.Add(new PLACE_USE() { PLACK_NUMBER = 2, PLACK_USE_CODE = depa.DEPARTMENT_CODE });
                        }
                    }
                }
                if (this.castFilter.PLACE_ROOM_IDs != null)
                {
                    foreach (var item in this.castFilter.PLACE_ROOM_IDs)
                    {
                        var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item);
                        if (room != null)
                        {
                            filterPlaceUse.Add(new PLACE_USE() { PLACK_NUMBER = 1, PLACK_USE_CODE = room.ROOM_CODE });
                        }
                    }
                }
                CommonParam paramGet = new CommonParam();
                if (!(castFilter.IS_MEDICINE ?? false) && !(castFilter.IS_MATERIAL ?? false) && !(castFilter.IS_CHEMICAL_SUBSTANCE ?? false))
                {
                    castFilter.IS_MEDICINE = true;
                    castFilter.IS_MATERIAL = true;
                    castFilter.IS_CHEMICAL_SUBSTANCE = true;
                }
                if (castFilter.OMS_LIMIT_CODE != null && castFilter.OUTSIDE_MEDI_STOCK_IDs == null)
                {
                    var ms = HisMediStockCFG.HisMediStocks.Where(o => string.Format(",{0},", castFilter.OMS_LIMIT_CODE).Contains(string.Format(",{0},", o.MEDI_STOCK_CODE))).Select(p => p.ID).ToList();
                    if (ms != null && ms.Count > 0)
                    {
                        castFilter.OUTSIDE_MEDI_STOCK_IDs = ms;
                    }
                }
                if (castFilter.IMS_LIMIT_CODE != null && castFilter.OUTSIDE_MEDI_STOCK_IDs == null)
                {
                    var ms = HisMediStockCFG.HisMediStocks.Where(o => !string.Format(",{0},", castFilter.IMS_LIMIT_CODE).Contains(string.Format(",{0},", o.MEDI_STOCK_CODE))).Select(p => p.ID).ToList();
                    if (ms != null && ms.Count > 0)
                    {
                        castFilter.OUTSIDE_MEDI_STOCK_IDs = ms;
                    }
                }
                if (castFilter.MERGE_PRICE == true)
                {
                    KeyGroup = KeyGroup.Replace("_{2}", "");
                }
                if (castFilter.MERGE_PACKAGE == true)
                {
                    KeyGroup = KeyGroup.Replace("_{3}", "");
                }
                if (castFilter.MERGE_EXPIRED == true)
                {
                    KeyGroup = KeyGroup.Replace("_{4}", "");
                }
                mediStock = MANAGER.Config.HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == castFilter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK();
                if (castFilter.MEDI_STOCK_IDs != null)
                {
                    mediStocks = MANAGER.Config.HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                }

                if (castFilter.IS_MEDICINE == true)
                {
                    listGet.AddRange(new ManagerSql().GetExpMestMe(castFilter));
                    if (castFilter.TRUE_FALSE != true)
                    {
                        listGet.AddRange(new ManagerSql().GetMobaMe(castFilter));
                    }
                }

                if (castFilter.IS_MATERIAL == true)
                {
                    listGet.AddRange(new ManagerSql().GetExpMestMa(castFilter));
                    if (castFilter.TRUE_FALSE != true)
                    {
                        listGet.AddRange(new ManagerSql().GetMobaMa(castFilter));
                    }
                }

                if (castFilter.IS_CHEMICAL_SUBSTANCE == true)
                {
                    listGet.AddRange(new ManagerSql().GetExpMestChem(castFilter));
                    if (castFilter.TRUE_FALSE != true)
                    {
                        listGet.AddRange(new ManagerSql().GetMobaChem(castFilter));
                    }
                }

                //lọc theo kho nhập của phiếu xuất chuyển kho
                if ((castFilter.IMP_MEDI_STOCK_ID ?? 0) != 0)
                {

                    listGet = listGet.Where(o => o.CHMS_MEDI_STOCK_ID == castFilter.IMP_MEDI_STOCK_ID && o.AMOUNT > 0).ToList();
                }

                //bỏ các lô nhập thử nhưng không xóa được.
                if (castFilter.TEST_PACKAGE_NUMBERs != null)
                {
                    listGet = listGet.Where(o => !string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", o.MEMA_ID))).ToList();
                }

                GetMedicineTypeMaterialType();
                GetMediMate();
                GetSupplier();
                GetImpSource();
                GetExpMestType();
                GetImpMestType();
                GetExpMestReason();
                GetDicRoomDicDepartment();
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00312");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void GetExpMestType()
        {
            CommonParam paramGet = new CommonParam();
            ExpMestTypes = new HisExpMestTypeManager(paramGet).Get(new HisExpMestTypeFilterQuery());
        }

        private void GetImpMestType()
        {
            CommonParam paramGet = new CommonParam();
            ImpMestTypes = new HisImpMestTypeManager(paramGet).Get(new HisImpMestTypeFilterQuery());
        }

        private void GetExpMestReason()
        {
            CommonParam paramGet = new CommonParam();
            ExpMestReasons = new HisExpMestReasonManager(paramGet).Get(new HisExpMestReasonFilterQuery());
        }

        private void GetMedicineTypeMaterialType()
        {
            CommonParam paramGet = new CommonParam();
            listMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            listMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();

                if (listGet != null)
                {
                    medicineIds.AddRange(listGet.Where(o => o.TYPE == this.THUOC).Select(o => o.MEMA_ID ?? 0).ToList());
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

                if (listGet != null)
                {
                    materialIds.AddRange(listGet.Where(o => o.TYPE == VAT_TU || o.TYPE == HOA_CHAT).Select(o => o.MEMA_ID ?? 0).ToList());
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

        private void GetSupplier()
        {
            try
            {
                HisSupplierFilterQuery Supplierfilter = new HisSupplierFilterQuery();
                Suppliers = new HisSupplierManager().Get(Supplierfilter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetImpSource()
        {
            try
            {
                HisImpSourceFilterQuery ImpSourcefilter = new HisImpSourceFilterQuery();
                ImpSources = new HisImpSourceManager().Get(ImpSourcefilter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetDicRoomDicDepartment()
        {
            try
            {
                int count = 1;
                int countR = 1;
                List<HIS_DEPARTMENT> listDepa = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs;

                ListRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());

                //if (castFilter.REQ_DEPARTMENT_IDs != null)
                //{
                //    listDepa = listDepa.Where(p => castFilter.REQ_DEPARTMENT_IDs.Contains(p.ID)).ToList();
                //    ListRoom = ListRoom.Where(p => castFilter.REQ_DEPARTMENT_IDs.Contains(p.DEPARTMENT_ID)).ToList();
                //}
                LogSystem.Info("depa: " + listDepa.Count());

                if (IsNotNullOrEmpty(listDepa))
                {
                    foreach (var department in listDepa)
                    {
                        if (count > 200)
                        {
                            Inventec.Common.Logging.LogSystem.Info("So Khoa Lon Ho Max 200 cua bao cao: " + count);
                            break;
                        }
                        System.Reflection.PropertyInfo piExp = typeof(Mrs00312RDO).GetProperty("EXP_AMOUNT_" + count);
                        System.Reflection.PropertyInfo piTh = typeof(Mrs00312RDO).GetProperty("TH_AMOUNT_" + count);
                        System.Reflection.PropertyInfo pi = typeof(Mrs00312RDO).GetProperty("AMOUNT_" + count);
                        System.Reflection.PropertyInfo piMoba = typeof(Mrs00312RDO).GetProperty("MOBA_AMOUNT_" + count);
                        if (!string.IsNullOrWhiteSpace(castFilter.DEPARTMENT_CODE__EXAMs) && ("," + castFilter.DEPARTMENT_CODE__EXAMs + ",").Contains(("," + department.DEPARTMENT_CODE + ",") ?? ""))
                        {
                            dicDepartment["DEPARTMENT_NAME_0"] = department.DEPARTMENT_NAME;
                            dicDepartment["DEPARTMENT_CODE_0"] = department.DEPARTMENT_CODE;
                        }
                        else
                        {
                            if (!dicDepartment.ContainsKey("DEPARTMENT_NAME_0"))
                            { dicDepartment["DEPARTMENT_NAME_0"] = "";/*mất đoạn*/dicDepartment["DEPARTMENT_CODE_0"] = ""; }
                            dicDepartment["DEPARTMENT_NAME_" + count] = department.DEPARTMENT_NAME;/*mất đoạn*/dicDepartment["DEPARTMENT_CODE_" + count] = department.DEPARTMENT_CODE;
                        }

                        department.NUM_ORDER = count;
                        ListDepartment.Add(department);

                        count++;
                    }
                    int more = count;
                    if (more - listDepa.Count() > 0 && more < 200)
                    {

                        for (int i = more; i < 200; i++)
                        {

                            dicDepartment["DEPARTMENT_NAME_" + i] = "";
                        }
                    }
                    dicDepartment["DEPARTMENT_NAME_200"] = "Tổng";

                }

                if (IsNotNullOrEmpty(ListRoom))
                {
                    if (!string.IsNullOrEmpty(dicDepartment["DEPARTMENT_CODE_0"].ToString()))
                    {
                        //chỉ nạp vào danh sách phòng của khoa khám bệnh
                        countR = 1;
                        foreach (var room in ListRoom.Where(o => o.DEPARTMENT_CODE == dicDepartment["DEPARTMENT_CODE_0"].ToString()))
                        {
                            if (countR > 200)
                            {
                                Inventec.Common.Logging.LogSystem.Info("So Phong Lon Ho Max 200 cua bao cao: " + countR);
                                break;
                            }
                            dicRoom["ROOM_CODE_" + countR] = room.ROOM_CODE;
                            dicRoom["ROOM_NAME_" + countR] = room.ROOM_NAME;
                            countR++;
                        }
                    }
                    else
                    {
                        //nạp vào danh sách phòng của tất cả các khoa
                        countR = 1;
                        foreach (var room in ListRoom)
                        {
                            if (countR > 200)
                            {
                                Inventec.Common.Logging.LogSystem.Info("So Phong Lon Ho Max 200 cua bao cao: " + countR);
                                break;
                            }
                            dicRoom["ROOM_CODE_" + countR] = room.ROOM_CODE;
                            dicRoom["ROOM_NAME_" + countR] = room.ROOM_NAME;
                            countR++;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {


                if (IsNotNullOrEmpty(listGet))
                {
                    this.ProcessDataDetail(listGet, ref dicRDO, ref dicTotalAmount, ref ListDepartment, ref ListRoom);

                    this.ProcessListParrent();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void ProcessDataDetail(List<DataGet> listGet, ref Dictionary<string, Mrs00312RDO> _dicRDO, ref Dictionary<string, TotalAmountStore> _dicTotalAmount, ref List<HIS_DEPARTMENT> _ListDepartment, ref List<V_HIS_ROOM> _ListRoom)
        {
            //var listReqRoomId = listGet.Select(o => o.REQ_ROOM_ID).Distinct().ToList();
            //foreach (var rooms in MANAGER.Config.HisRoomCFG.HisRooms)
            //{
            //    if (listReqRoomId.Contains(rooms.ID))
            //        _ListRoom.Add(rooms);
            //}

            this.ProcessExpMestData(listGet, ref _dicRDO, ref _dicTotalAmount);

            if (_dicRDO.Count > 0)
            {
                System.Reflection.PropertyInfo piExp = typeof(Mrs00312RDO).GetProperty("EXP_AMOUNT_200");//Max column
                System.Reflection.PropertyInfo pi = typeof(Mrs00312RDO).GetProperty("AMOUNT_200");
                System.Reflection.PropertyInfo piMoba = typeof(Mrs00312RDO).GetProperty("MOBA_AMOUNT_200");
                foreach (var rdo in _dicRDO)
                {
                    if (_dicTotalAmount.ContainsKey(rdo.Key))
                    {
                        var total = _dicTotalAmount[rdo.Key];
                        pi.SetValue(rdo.Value, total.TotalAmount);
                        piExp.SetValue(rdo.Value, total.TotalExpAmount);
                        piMoba.SetValue(rdo.Value, total.TotalMobaAmount);
                    }
                }
                dicDepartment["DEPARTMENT_NAME_200"] = "Tổng";
            }

        }

        private void ProcessExpMestData(List<DataGet> listGet, ref Dictionary<string, Mrs00312RDO> _dicRDO, ref Dictionary<string, TotalAmountStore> _dicTotalAmount)
        {
            if (listGet.Count > 0)
            {
                //gộp theo thông tin lô thuốc
                var groupByMedicine = listGet.GroupBy(g => string.Format("{0}_{1}" + KeyGroup, g.TYPE, g.MEMA_TYPE_ID, g.IMP_PRICE * (1 + g.IMP_VAT_RATIO), g.PACKAGE_NUMBER, g.EXPIRED_DATE)).ToList();
                var dicMe = this.Medicines.GroupBy(o => o.MEDICINE_TYPE_ID).ToDictionary(p => p.Key, q => q.ToList());
                var dicMa = this.Materials.GroupBy(o => o.MATERIAL_TYPE_ID).ToDictionary(p => p.Key, q => q.ToList());
                foreach (var group in groupByMedicine)
                {
                    var item = group.First();
                    var medicineSub = dicMe.ContainsKey(item.MEMA_TYPE_ID) ? dicMe[item.MEMA_TYPE_ID] : new List<HIS_MEDICINE>();
                    var materialSub = dicMa.ContainsKey(item.MEMA_TYPE_ID) ? dicMa[item.MEMA_TYPE_ID] : new List<HIS_MATERIAL>();
                    Mrs00312RDO rdo = new Mrs00312RDO();
                    rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "NONE";
                    rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME ?? "NONE";
                    if (item.TYPE == THUOC)
                    {
                        var medicine = medicineSub.FirstOrDefault(o => o.ID == item.MEMA_ID && item.TYPE == THUOC) ?? new HIS_MEDICINE();
                        //bổ sung nhà cung cấp
                        if (Suppliers != null)
                        {
                            var supplier = Suppliers.FirstOrDefault(o => o.ID == (medicine.SUPPLIER_ID));
                            if (supplier != null)
                            {
                                rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                            }
                        }
                        //bổ sung nguồn nhập
                        if (ImpSources != null)
                        {
                            var ImpSource = ImpSources.FirstOrDefault(o => o.ID == (medicine.IMP_SOURCE_ID));
                            if (ImpSource != null)
                            {
                                rdo.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                rdo.IMP_SOURCE_ID = ImpSource.ID;
                            }
                            else
                            {
                                ImpSource = ImpSources.FirstOrDefault(o => medicineSub.Exists(p => p.IMP_SOURCE_ID == o.ID && p.MEDICINE_TYPE_ID == item.MEMA_TYPE_ID && p.IMP_PRICE * (1 + p.IMP_VAT_RATIO) == item.IMP_PRICE * (1 + item.IMP_VAT_RATIO) && p.EXPIRED_DATE == item.EXPIRED_DATE && (p.SUPPLIER_ID ?? 0) == (item.SUPPLIER_ID ?? 0)));
                                if (ImpSource != null)
                                {
                                    rdo.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                    rdo.IMP_SOURCE_ID = ImpSource.ID;
                                }
                            }

                        }
                        //var impMedi = listImpMedicine.FirstOrDefault(p => p.MEDICINE_TYPE_ID == item.MEMA_TYPE_ID) ?? new V_HIS_IMP_MEST_MEDICINE();
                        //var imp = listImp.FirstOrDefault(p => p.ID == impMedi.IMP_MEST_ID) ?? new HIS_IMP_MEST();
                        rdo.BID_NUMBER = medicine.TDL_BID_NUMBER;
                        rdo.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;

                        rdo.BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;
                        rdo.BID_YEAR = medicine.TDL_BID_YEAR;
                        rdo.BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;
                        rdo.APPROVAL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medicine.IMP_TIME??0);
                        var medicineType = listMedicineType.FirstOrDefault(o => o.ID == item.MEMA_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID) ?? new V_HIS_MEDICINE_TYPE();
                        rdo.PARENT_NUM_ORDER = parent.NUM_ORDER??9999;
                        rdo.PARENT_MEDICINE_TYPE_CODE = parent.MEDICINE_TYPE_CODE;
                        rdo.PARENT_MEDICINE_TYPE_NAME = parent.MEDICINE_TYPE_NAME;
                        //Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00312RDO>(rdo, medicineType);
                        rdo.MEDICINE_TYPE_ID = medicineType.ID;
                        rdo.SERVICE_ID = medicineType.SERVICE_ID;
                        rdo.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medicineType.CONCENTRA;
                        rdo.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        rdo.ACTIVE_INGR_CODE = medicineType.ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                        rdo.MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = medicineType.NATIONAL_NAME;
                        rdo.REGISTER_NUMBER = medicineType.REGISTER_NUMBER;
                        rdo.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                        
                        //thông tin nhóm thuốc
                        if (item.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                        {
                            rdo.PARENT_TYPE_ID = 1;
                            rdo.PARENT_NAME = "Thuốc Gây Nghiện";
                        }
                        else if (item.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                        {
                            rdo.PARENT_TYPE_ID = 2;
                            rdo.PARENT_NAME = "Thuốc Hướng Thần";
                        }
                        else
                        {
                            rdo.PARENT_TYPE_ID = 3;
                            rdo.PARENT_NAME = "Thuốc Thường";
                        }

                    }
                    else
                    {
                        var material = materialSub.FirstOrDefault(o => o.ID == item.MEMA_ID && item.TYPE != THUOC) ?? new HIS_MATERIAL();
                        //bổ sung nhà cung cấp
                        if (Suppliers != null)
                        {
                            var supplier = Suppliers.FirstOrDefault(o => o.ID == (material.SUPPLIER_ID));
                            if (supplier != null)
                            {
                                rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                            }
                        }
                        //bổ sung nguồn nhập
                        if (ImpSources != null)
                        {
                            var ImpSource = ImpSources.FirstOrDefault(o => o.ID == (material.IMP_SOURCE_ID));
                            if (ImpSource != null)
                            {
                                rdo.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                rdo.IMP_SOURCE_ID = ImpSource.ID;
                            }
                            else
                            {
                                ImpSource = ImpSources.FirstOrDefault(o => materialSub.Exists(p => p.IMP_SOURCE_ID == o.ID && p.MATERIAL_TYPE_ID == item.MEMA_TYPE_ID && p.IMP_PRICE * (1 + p.IMP_VAT_RATIO) == item.IMP_PRICE * (1 + item.IMP_VAT_RATIO) && p.EXPIRED_DATE == item.EXPIRED_DATE && (p.SUPPLIER_ID ?? 0) == (item.SUPPLIER_ID ?? 0)));
                                if (ImpSource != null)
                                {
                                    rdo.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                    rdo.IMP_SOURCE_ID = ImpSource.ID;
                                }
                            }

                        }
                        //var impMate = listImpMaterial.FirstOrDefault(p => p.MATERIAL_TYPE_ID == item.MEMA_TYPE_ID) ?? new V_HIS_IMP_MEST_MATERIAL();
                        //var imp = listImp.FirstOrDefault(p => p.ID == impMate.IMP_MEST_ID) ?? new HIS_IMP_MEST();
                        rdo.BID_NUMBER = material.TDL_BID_NUMBER;
                        rdo.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;
                        rdo.BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;
                        rdo.BID_YEAR = material.TDL_BID_YEAR;
                        rdo.BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;
                        rdo.APPROVAL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.DOCUMENT_DATE ?? 0);
                        var materialType = listMaterialType.FirstOrDefault(o => o.ID == item.MEMA_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
                        var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE();
                        rdo.PARENT_NUM_ORDER = parent.NUM_ORDER??9999;
                        rdo.PARENT_MEDICINE_TYPE_CODE = parent.MATERIAL_TYPE_CODE;
                        rdo.PARENT_MEDICINE_TYPE_NAME = parent.MATERIAL_TYPE_NAME;
                        //Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00312RDO>(rdo, materialType);
                        rdo.MEDICINE_TYPE_ID = materialType.ID;
                        rdo.SERVICE_ID = materialType.SERVICE_ID;
                        rdo.MEDICINE_TYPE_CODE = materialType.MATERIAL_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = materialType.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = materialType.CONCENTRA;
                        rdo.MANUFACTURER_NAME = materialType.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = materialType.NATIONAL_NAME;
                        rdo.REGISTER_NUMBER = materialType.REGISTER_NUMBER;
                        rdo.PACKING_TYPE_NAME = materialType.PACKING_TYPE_NAME;
                    }


                    rdo.TYPE = item.TYPE;
                    rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE = item.EXPIRED_DATE;
                    rdo.IMP_VAT_RATIO = item.IMP_VAT_RATIO;
                    rdo.IMP_PRICE = item.IMP_PRICE;

                    //so luong theo tung phong
                    rdo.DIC_ROOM_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_ROOM_EXP_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_ROOM_MOBA_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_ROOM_TH_AMOUNT = new Dictionary<string, decimal>();

                    //so luong theo tung khoa
                    rdo.DIC_DEPARTMENT_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_DEPARTMENT_EXP_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_DEPARTMENT_MOBA_AMOUNT = new Dictionary<string, decimal>();
                    rdo.DIC_DEPARTMENT_TH_AMOUNT = new Dictionary<string, decimal>();

                    _dicRDO[group.Key] = rdo;

                    int count = 1;
                    foreach (var department in MANAGER.Config.HisDepartmentCFG.DEPARTMENTs)
                    {
                        if (count > 200)
                        {
                            Inventec.Common.Logging.LogSystem.Info("So Khoa Lon Ho Max 200 cua bao cao: " + count);
                            break;
                        }

                        System.Reflection.PropertyInfo piExp = typeof(Mrs00312RDO).GetProperty("EXP_AMOUNT_" + count);
                        System.Reflection.PropertyInfo piTh = typeof(Mrs00312RDO).GetProperty("TH_AMOUNT_" + count);
                        System.Reflection.PropertyInfo pi = typeof(Mrs00312RDO).GetProperty("AMOUNT_" + count);
                        System.Reflection.PropertyInfo piMoba = typeof(Mrs00312RDO).GetProperty("MOBA_AMOUNT_" + count);

                        piExp.SetValue(rdo, group.Where(o => o.REQ_DEPARTMENT_ID == department.ID).Sum(s => s.AMOUNT));
                        piTh.SetValue(rdo, group.Where(o => o.REQ_DEPARTMENT_ID == department.ID).Sum(s => s.TH_AMOUNT));
                        piMoba.SetValue(rdo, group.Where(o => o.REQ_DEPARTMENT_ID == department.ID).Sum(s => s.MOBA_AMOUNT));
                        pi.SetValue(rdo, group.Where(o => o.REQ_DEPARTMENT_ID == department.ID).Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT));
                        count++;
                    }
                    
                    rdo.AMOUNT = group.Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT);

                    rdo.DIC_ROOM_AMOUNT = group.GroupBy(g => (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == g.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT));

                    rdo.EXP_AMOUNT = group.Sum(s => s.AMOUNT);

                    rdo.DIC_ROOM_EXP_AMOUNT = group.GroupBy(g => (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == g.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.MOBA_AMOUNT = group.Sum(s => s.MOBA_AMOUNT);

                    rdo.DIC_ROOM_MOBA_AMOUNT = group.GroupBy(g => (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == g.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.MOBA_AMOUNT));

                    rdo.DIC_ROOM_TH_AMOUNT = group.GroupBy(g => (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == g.REQ_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TH_AMOUNT));

                    rdo.DIC_DEPARTMENT_AMOUNT = group.GroupBy(g => (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == g.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT));

                    rdo.DIC_DEPARTMENT_EXP_AMOUNT = group.GroupBy(g => (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == g.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    rdo.DIC_DEPARTMENT_MOBA_AMOUNT = group.GroupBy(g => (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == g.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.MOBA_AMOUNT));

                    rdo.TH_AMOUNT = group.Sum(s => s.TH_AMOUNT);

                    rdo.DIC_DEPARTMENT_TH_AMOUNT = group.GroupBy(g => (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == g.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TH_AMOUNT));

                    if (rdo.DIC_AMOUNT == null) rdo.DIC_AMOUNT = new Dictionary<string, decimal>();

                    rdo.DIC_AMOUNT = group.GroupBy(g => (ExpMestTypes.FirstOrDefault(o => o.ID == g.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE()).EXP_MEST_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT));

                    if (rdo.DIC_EXP_AMOUNT == null) rdo.DIC_EXP_AMOUNT = new Dictionary<string, decimal>();

                    rdo.DIC_EXP_AMOUNT = group.GroupBy(g => (ExpMestTypes.FirstOrDefault(o => o.ID == g.EXP_MEST_TYPE_ID) ?? new HIS_EXP_MEST_TYPE()).EXP_MEST_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    if (rdo.DIC_MOBA_AMOUNT == null) rdo.DIC_MOBA_AMOUNT = new Dictionary<string, decimal>();

                    rdo.DIC_MOBA_AMOUNT = group.GroupBy(g => (ImpMestTypes.FirstOrDefault(o => o.ID == g.IMP_MEST_TYPE_ID) ?? new HIS_IMP_MEST_TYPE()).IMP_MEST_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.MOBA_AMOUNT));

                    if (rdo.DIC_TH_AMOUNT == null) rdo.DIC_TH_AMOUNT = new Dictionary<string, decimal>();

                    rdo.DIC_TH_AMOUNT = group.GroupBy(g => (ImpMestTypes.FirstOrDefault(o => o.ID == g.IMP_MEST_TYPE_ID) ?? new HIS_IMP_MEST_TYPE()).IMP_MEST_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TH_AMOUNT));

                    if (rdo.DIC_REASON_AMOUNT == null) rdo.DIC_REASON_AMOUNT = new Dictionary<string, decimal>();

                    rdo.DIC_REASON_AMOUNT = group.GroupBy(g => (ExpMestReasons.FirstOrDefault(o => o.ID == g.EXP_MEST_REASON_ID) ?? new HIS_EXP_MEST_REASON()).EXP_MEST_REASON_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));

                    TotalAmountStore total = null;
                    if (_dicTotalAmount.ContainsKey(group.Key))
                    {
                        total = _dicTotalAmount[group.Key];
                    }
                    else
                    {
                        total = new TotalAmountStore();
                        _dicTotalAmount[group.Key] = total;
                    }
                    total.TotalAmount += group.Sum(s => s.AMOUNT - s.MOBA_AMOUNT - s.TH_AMOUNT);
                    total.TotalMobaAmount += group.Sum(s => s.MOBA_AMOUNT);
                    total.TotalThAmount += group.Sum(s => s.TH_AMOUNT);
                    total.TotalExpAmount += group.Sum(s => s.AMOUNT);

                }

                //gộp theo nơi sử dụng
                foreach (var item in listGet)
                {
                    PLACE_USE dt = new PLACE_USE();

                    //thêm vào danh sách các id phòng có yêu cầu xuất
                    if (!ListRoomId.Contains(item.REQ_ROOM_ID))
                    {
                        ListRoomId.Add(item.REQ_ROOM_ID);
                    }
                    var medicineSub = dicMe.ContainsKey(item.MEMA_TYPE_ID) ? dicMe[item.MEMA_TYPE_ID] : new List<HIS_MEDICINE>();
                    var materialSub = dicMa.ContainsKey(item.MEMA_TYPE_ID) ? dicMa[item.MEMA_TYPE_ID] : new List<HIS_MATERIAL>();
                    if (item.TYPE == THUOC)
                    {
                        var medicine = medicineSub.FirstOrDefault(o => o.ID == item.MEMA_ID && item.TYPE == THUOC) ?? new HIS_MEDICINE();

                        //bổ sung nguồn nhập
                        if (ImpSources != null)
                        {
                            var ImpSource = ImpSources.FirstOrDefault(o => o.ID == (medicine.IMP_SOURCE_ID));
                            if (ImpSource != null)
                            {
                                dt.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                dt.IMP_SOURCE_CODE = ImpSource.IMP_SOURCE_CODE;
                                dt.IMP_SOURCE_ID = ImpSource.ID;
                            }
                            else
                            {
                                ImpSource = ImpSources.FirstOrDefault(o => medicineSub.Exists(p => p.IMP_SOURCE_ID == o.ID && p.MEDICINE_TYPE_ID == item.MEMA_TYPE_ID && p.IMP_PRICE * (1 + p.IMP_VAT_RATIO) == item.IMP_PRICE * (1 + item.IMP_VAT_RATIO) && p.EXPIRED_DATE == item.EXPIRED_DATE && (p.SUPPLIER_ID ?? 0) == (item.SUPPLIER_ID ?? 0)));
                                if (ImpSource != null)
                                {
                                    dt.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                    dt.IMP_SOURCE_CODE = ImpSource.IMP_SOURCE_CODE;
                                    dt.IMP_SOURCE_ID = ImpSource.ID;
                                }
                            }

                        }

                        var medicineType = listMedicineType.FirstOrDefault(o => o.ID == item.MEMA_TYPE_ID) ?? new V_HIS_MEDICINE_TYPE();
                        var parent = listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID) ?? new V_HIS_MEDICINE_TYPE();
                        dt.MEDICINE_TYPE_CODE = medicineType.MEDICINE_TYPE_CODE;
                        dt.MEDICINE_TYPE_NAME = medicineType.MEDICINE_TYPE_NAME;
                        dt.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                        dt.CATEGORY_CODE = parent.MEDICINE_TYPE_CODE;
                        dt.CATEGORY_NAME = parent.MEDICINE_TYPE_NAME;

                    }
                    else
                    {
                        var material = materialSub.FirstOrDefault(o => o.ID == item.MEMA_ID && item.TYPE != THUOC) ?? new HIS_MATERIAL();

                        //bổ sung nguồn nhập
                        if (ImpSources != null)
                        {
                            var ImpSource = ImpSources.FirstOrDefault(o => o.ID == (material.IMP_SOURCE_ID));
                            if (ImpSource != null)
                            {
                                dt.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                dt.IMP_SOURCE_CODE = ImpSource.IMP_SOURCE_CODE;
                                dt.IMP_SOURCE_ID = ImpSource.ID;
                            }
                            else
                            {
                                ImpSource = ImpSources.FirstOrDefault(o => materialSub.Exists(p => p.IMP_SOURCE_ID == o.ID && p.MATERIAL_TYPE_ID == item.MEMA_TYPE_ID && p.IMP_PRICE * (1 + p.IMP_VAT_RATIO) == item.IMP_PRICE * (1 + item.IMP_VAT_RATIO) && p.EXPIRED_DATE == item.EXPIRED_DATE && (p.SUPPLIER_ID ?? 0) == (item.SUPPLIER_ID ?? 0)));
                                if (ImpSource != null)
                                {
                                    dt.IMP_SOURCE_NAME = ImpSource.IMP_SOURCE_NAME;
                                    dt.IMP_SOURCE_CODE = ImpSource.IMP_SOURCE_CODE;
                                    dt.IMP_SOURCE_ID = ImpSource.ID;
                                }
                            }

                        }
                        var materialType = listMaterialType.FirstOrDefault(o => o.ID == item.MEMA_TYPE_ID) ?? new V_HIS_MATERIAL_TYPE();
                        var parent = listMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID) ?? new V_HIS_MATERIAL_TYPE();
                        dt.MEDICINE_TYPE_CODE = materialType.MATERIAL_TYPE_CODE;
                        dt.MEDICINE_TYPE_NAME = materialType.MATERIAL_TYPE_NAME;
                        dt.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
                        dt.CATEGORY_CODE = parent.MATERIAL_TYPE_CODE;
                        dt.CATEGORY_NAME = parent.MATERIAL_TYPE_NAME;
                    }
                    var Reason = ExpMestReasons.FirstOrDefault(o => o.ID == item.EXP_MEST_REASON_ID);
                    if (Reason != null)
                    {
                        dt.EXP_MEST_REASON_CODE = Reason.EXP_MEST_REASON_CODE;
                        if (Reason.EXP_MEST_REASON_NAME != null && Reason.EXP_MEST_REASON_NAME.ToLower().Contains("trả phần mềm cũ"))
                            continue;
                    }
                    var ExpMestType = ExpMestTypes.FirstOrDefault(o => o.ID == item.EXP_MEST_TYPE_ID);
                    if (ExpMestType != null)
                    {
                        dt.EXP_MEST_TYPE_CODE = ExpMestType.EXP_MEST_TYPE_CODE;
                    }


                    var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                    var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK();
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.REQ_ROOM_ID) ?? new V_HIS_ROOM();
                    //var mestRoomSub = this.listMestRoom.Where(o => o.MEDI_STOCK_ID == item.REQ_ROOM_ID).ToList();

                    if (item.TYPE == THUOC)
                    {
                        //dược:
                        MedicinePlace(item, dt, mediStock, reqDepartment, room, Reason);
                    }

                    else
                    {
                        //vật tư:
                        MaterialPlace(item, dt, mediStock, reqDepartment, room, Reason);
                    }
                    if (filterPlaceUse != null && filterPlaceUse.Count > 0)
                    {
                        if (!filterPlaceUse.Exists(o => o.PLACK_NUMBER == dt.PLACK_NUMBER && o.PLACK_USE_CODE == dt.PLACK_USE_CODE))
                        {
                            continue;
                        }
                    }

                    string KeyPlace = string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}", dt.IS_NOI_TRU, dt.PLACK_USE_CODE, item.TYPE, item.MEMA_TYPE_ID, item.EXP_MEST_REASON_ID, item.EXP_MEST_TYPE_ID, (1 + item.IMP_VAT_RATIO) * item.IMP_PRICE, dt.IMP_SOURCE_ID, dt.PLACK_NUMBER);
                    item.KEY_PLACE = KeyPlace;
                    if (!dicPlaceUse.ContainsKey(KeyPlace))
                    {
                        dicPlaceUse[KeyPlace] = dt;
                        dt.PRICE = (1 + item.IMP_VAT_RATIO) * item.IMP_PRICE;
                        dt.VAT_RATIO = item.IMP_VAT_RATIO;
                    }
                    dicPlaceUse[KeyPlace].KEY_PLACE = KeyPlace;
                    dicPlaceUse[KeyPlace].AMOUNT += item.AMOUNT - item.MOBA_AMOUNT - item.TH_AMOUNT;
                    dicPlaceUse[KeyPlace].EXP_AMOUNT += item.AMOUNT;
                    dicPlaceUse[KeyPlace].MOBA_AMOUNT += item.MOBA_AMOUNT;
                    dicPlaceUse[KeyPlace].TH_AMOUNT += item.MOBA_AMOUNT;
                    dicPlaceUse[KeyPlace].TOTAL_PRICE += (item.AMOUNT - item.MOBA_AMOUNT - item.TH_AMOUNT) * (1 + item.IMP_VAT_RATIO) * item.IMP_PRICE;
                    dicPlaceUse[KeyPlace].PRICE = Math.Round((1 + item.IMP_VAT_RATIO) * item.IMP_PRICE);
                }
            }

        }


        private void MedicinePlace(DataGet item, PLACE_USE dt, V_HIS_MEDI_STOCK mediStock, HIS_DEPARTMENT department, V_HIS_ROOM room, HIS_EXP_MEST_REASON reason)
        {
            //ngoại trú:
            //(phiếu xuất cho bệnh nhân khám, bệnh nhân ngoại trú hoặc đơn phòng khám của bệnh nhân nội trú hoặc do khoa khám bệnh yêu cầu) 
            if ((this.castFilter.DEPARTMENT_CODE__KKBs != null && department.DEPARTMENT_CODE !=null && this.castFilter.DEPARTMENT_CODE__KKBs.Contains(department.DEPARTMENT_CODE)
                || item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM || item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                //và không phải do kho lẻ xuất
                && !string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__KHOLEs ?? "").Contains(string.Format(",{0},", mediStock.MEDI_STOCK_CODE ?? ""))
                )
            {
                dt.IS_NOI_TRU = false;
                dt.PLACK_NUMBER = 1;
                dt.PLACK_USE_CODE = mediStock.MEDI_STOCK_CODE;
                dt.PLACK_USE_NAME = mediStock.MEDI_STOCK_NAME;
            }
            else//khoa khám bệnh yêu cầu kho lẻ xuất
                if (this.castFilter.DEPARTMENT_CODE__KKBs != null && department.DEPARTMENT_CODE != null && this.castFilter.DEPARTMENT_CODE__KKBs.Contains(department.DEPARTMENT_CODE)
                    && string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__KHOLEs ?? "").Contains(string.Format(",{0},", mediStock.MEDI_STOCK_CODE ?? "")))
                {
                    dt.IS_NOI_TRU = false;
                    dt.PLACK_NUMBER = 1;
                    dt.PLACK_USE_CODE = room.ROOM_CODE;
                    dt.PLACK_USE_NAME = room.ROOM_NAME;
                }
                else//đơn phòng khám yêu cầu kho lẻ xuất
                    if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                        && string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__KHOLEs ?? "").Contains(string.Format(",{0},", mediStock.MEDI_STOCK_CODE ?? "")))
                    {
                        dt.IS_NOI_TRU = false;
                        dt.PLACK_NUMBER = 2;
                        dt.PLACK_USE_CODE = department.DEPARTMENT_CODE;
                        dt.PLACK_USE_NAME = department.DEPARTMENT_NAME;
                    }
                else
                {
                    dt.IS_NOI_TRU = true;
                    if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC && room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__KHO)
                    {
                        dt.PLACK_NUMBER = 1;
                        dt.PLACK_USE_CODE = room.ROOM_CODE;
                        dt.PLACK_USE_NAME = room.ROOM_NAME;
                        //xuất ban giám đốc xuất kiểm nghiệm thì lấy lý do xuất
                        if (reason != null && !string.IsNullOrEmpty(reason.EXP_MEST_REASON_CODE))
                        {
                            if (reason.EXP_MEST_REASON_CODE == this.castFilter.EXP_MEST_REASON_CODE__KN)
                            {
                                dt.PLACK_NUMBER = 0;
                                dt.PLACK_USE_CODE = reason.EXP_MEST_REASON_CODE;
                                dt.PLACK_USE_NAME = reason.EXP_MEST_REASON_NAME;
                            }
                            else if (reason.EXP_MEST_REASON_CODE == this.castFilter.EXP_MEST_REASON_CODE__BGD)
                            {
                                dt.PLACK_NUMBER = 0;
                                dt.PLACK_USE_CODE = reason.EXP_MEST_REASON_CODE;
                                dt.PLACK_USE_NAME = reason.EXP_MEST_REASON_NAME;
                            }
                            else if (reason.EXP_MEST_REASON_CODE == "07")
                            {
                                dt.PLACK_NUMBER = 2;
                                dt.PLACK_USE_CODE = department.DEPARTMENT_CODE;
                                dt.PLACK_USE_NAME = department.DEPARTMENT_NAME;
                            }

                        }
                    }
                    else
                    {
                        dt.PLACK_NUMBER = 2;
                        dt.PLACK_USE_CODE = department.DEPARTMENT_CODE;
                        dt.PLACK_USE_NAME = department.DEPARTMENT_NAME;
                    }


                }
        }

        private void MaterialPlace(DataGet item, PLACE_USE dt, V_HIS_MEDI_STOCK mediStock, HIS_DEPARTMENT department, V_HIS_ROOM room, HIS_EXP_MEST_REASON reason)
        {
            //nếu là tủ trực tron cấu hình TTPK thì cho vào nội trú và tách riêng ra
            if (room.ROOM_CODE != null && castFilter.MEDI_STOCK_CODE__TTPKs != null && castFilter.MEDI_STOCK_CODE__TTPKs.Contains(room.ROOM_CODE ?? ""))
            {
                dt.IS_NOI_TRU = true;
                dt.PLACK_NUMBER = 1;
                dt.PLACK_USE_CODE = room.ROOM_CODE;
                dt.PLACK_USE_NAME = room.ROOM_NAME;
            }
            else
                //ngoại trú: các tủ trực phòng khám hoặc khoa khám bệnh yêu cầu
                if ((room.ROOM_NAME != null && (room.ROOM_NAME.ToLower().Contains("pk") || room.ROOM_NAME.ToLower().Contains("phòng") || room.ROOM_NAME.ToLower().Contains("phòng khám")) && room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__KHO
                    || this.castFilter.DEPARTMENT_CODE__KKBs != null && department.DEPARTMENT_CODE != null && this.castFilter.DEPARTMENT_CODE__KKBs.Contains(department.DEPARTMENT_CODE))
                    && (castFilter.MEDI_STOCK_CODE__TTKHs == null || !castFilter.MEDI_STOCK_CODE__TTKHs.Contains(room.ROOM_CODE ?? ""))
                    )
                {
                    dt.IS_NOI_TRU = false;
                    dt.PLACK_NUMBER = 1;
                    dt.PLACK_USE_CODE = room.ROOM_CODE;
                    dt.PLACK_USE_NAME = room.ROOM_NAME;
                }
                else
                {
                    dt.IS_NOI_TRU = true;
                    dt.PLACK_NUMBER = 2;
                    dt.PLACK_USE_CODE = department.DEPARTMENT_CODE;
                    dt.PLACK_USE_NAME = department.DEPARTMENT_NAME;
                    if (item.CHMS_MEDI_STOCK_ID != null)
                    {
                        var chmsMediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.CHMS_MEDI_STOCK_ID);
                        if (chmsMediStock != null && mediStock.DEPARTMENT_ID == chmsMediStock.DEPARTMENT_ID)
                        {
                            dt.PLACK_NUMBER = 1;
                            dt.PLACK_USE_CODE = chmsMediStock.MEDI_STOCK_CODE;
                            dt.PLACK_USE_NAME = chmsMediStock.MEDI_STOCK_NAME;
                        }
                        else if (chmsMediStock != null && mediStock.DEPARTMENT_ID != chmsMediStock.DEPARTMENT_ID)
                        {
                            dt.PLACK_NUMBER = 2;
                            dt.PLACK_USE_CODE = chmsMediStock.DEPARTMENT_CODE;
                            dt.PLACK_USE_NAME = chmsMediStock.DEPARTMENT_NAME;
                        }
                    }
                    else if (mediStock.DEPARTMENT_ID == department.ID)
                    {
                        if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP && room.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__KHO)
                        {
                            dt.PLACK_NUMBER = 1;
                            dt.PLACK_USE_CODE = room.ROOM_CODE;
                            dt.PLACK_USE_NAME = room.ROOM_NAME;
                        }
                    }
                }

        }

        void ProcessListParrent()
        {
            try
            {
                if (dicRDO.Count > 0)
                {
                    listRdo = dicRDO.Select(s => s.Value).OrderBy(o => o.PARENT_TYPE_ID).ThenBy(o => o.MEDICINE_TYPE_NAME).ToList();

                    listParentRdo = listRdo.GroupBy(g => g.PARENT_TYPE_ID).Select(s => new Mrs00312RDO() { PARENT_TYPE_ID = s.First().PARENT_TYPE_ID, PARENT_NAME = s.First().PARENT_NAME }).ToList();

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
                foreach (var dic in dicDepartment)
                {
                    dicSingleTag.Add(dic.Key, dic.Value);
                }

                foreach (var dics in dicRoom)
                {
                    dicSingleTag.Add(dics.Key, dics.Value);
                }

                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (mediStock != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME);
                }
                else if (mediStocks != null && mediStocks.Count > 0)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(",", mediStocks.Select(s => s.MEDI_STOCK_NAME).ToList()));
                }

                if (castFilter.REQ_DEPARTMENT_IDs != null)
                {
                    dicSingleTag.Add("REQ_DEPARTMENT_NAME", string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.REQ_DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
                }

                //if (castFilter.PLACE_DEPARTMENT_ID != null)
                //{
                //    dicSingleTag.Add("PLACE_DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => castFilter.PLACE_DEPARTMENT_ID == o.ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
                //}

                if (castFilter.PLACE_DEPARTMENT_IDs != null)
                {
                    dicSingleTag.Add("PLACE_DEPARTMENT_NAMEs", string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.PLACE_DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
                    dicSingleTag.Add("PLACE_DEPARTMENT_NAME", string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.PLACE_DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
                }


                if (castFilter.IMP_SOURCE_IDs != null)
                {
                    dicSingleTag.Add("IMP_SOURCE_NAME", string.Join(" - ", this.ImpSources.Where(o => castFilter.IMP_SOURCE_IDs.Contains(o.ID)).Select(p => p.IMP_SOURCE_NAME).ToList()));
                }

                objectTag.AddObjectData(store, "Parent", listParentRdo.OrderBy(p=>p.PARENT_NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddRelationship(store, "Parent", "Report", "PARENT_TYPE_ID", "PARENT_TYPE_ID");
                objectTag.SetUserFunction(store, "SumKeys", new RDOSumKeys());
                objectTag.SetUserFunction(store, "Element", new RDOElement());

                objectTag.AddObjectData(store, "Departments", ListDepartment.Where(o => castFilter.REQ_DEPARTMENT_IDs == null || castFilter.REQ_DEPARTMENT_IDs.Contains(o.ID)).ToList());

                objectTag.AddObjectData(store, "Rooms", ListRoom.Where(o => (castFilter.REQ_DEPARTMENT_IDs == null || castFilter.REQ_DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)) && (castFilter.REQ_ROOM_IDs == null || castFilter.REQ_ROOM_IDs.Contains(o.ID))).ToList());

                //danh sách phòng khám được xuất:
                objectTag.AddObjectData(store, "RoomData", ListRoom.Where(o => castFilter.REQ_DEPARTMENT_IDs == null || castFilter.REQ_DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).Where(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && ListRoomId.Contains(o.ID)).ToList());

                //danh sách Khoa còn lại được xuất:
                objectTag.AddObjectData(store, "DepartmentData", ListDepartment.Where(o => castFilter.REQ_DEPARTMENT_IDs == null || castFilter.REQ_DEPARTMENT_IDs.Contains(o.ID)).Where(o => ListRoom.Exists(p => p.DEPARTMENT_ID == o.ID && p.IS_EXAM != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && ListRoomId.Contains(p.ID))).ToList());

                objectTag.AddObjectData(store, "mediStocks", mediStocks);

                objectTag.AddObjectData(store, "listMedicineType", listMedicineType);

                objectTag.AddObjectData(store, "listMaterialType", listMaterialType);

                objectTag.AddObjectData(store, "listGet", listGet);

                objectTag.AddObjectData(store, "Medicines", Medicines);

                objectTag.AddObjectData(store, "Materials", Materials);

                objectTag.AddObjectData(store, "Suppliers", Suppliers);

                objectTag.AddObjectData(store, "ExpMestTypes", ExpMestTypes);

                objectTag.AddObjectData(store, "ExpMestReasons", ExpMestReasons);

                objectTag.AddObjectData(store, "PlaceUse", dicPlaceUse.Values.Where(o => castFilter.IMP_SOURCE_IDs == null || castFilter.IMP_SOURCE_IDs.Contains(o.IMP_SOURCE_ID)).ToList());



            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Dictionary<string, object> medicineType { get; set; }
    }

    class TotalAmountStore
    {
        internal decimal TotalAmount;
        internal decimal TotalExpAmount;
        internal decimal TotalMobaAmount;
        internal decimal TotalThAmount;
    }
}
