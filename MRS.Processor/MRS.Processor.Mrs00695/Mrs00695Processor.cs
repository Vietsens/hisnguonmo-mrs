using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockMaty;
using MOS.MANAGER.HisMediStockMety;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00695
{
    class Mrs00695Processor : AbstractProcessor
    {
        Mrs00695Filter castFilter = new Mrs00695Filter();

        private List<Mrs00695RDO> ListRdo = new List<Mrs00695RDO>();

        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();
        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod = new Dictionary<long, HIS_MEDI_STOCK_PERIOD>();
        //Cac list dau ki
        List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = new List<V_HIS_MEST_PERIOD_MEDI>(); // DS thuoc chot ki
        List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = new List<V_HIS_MEST_PERIOD_MATE>(); // DS vat tu chot ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBefore = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap truoc ki
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialBefore = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap truoc ki
        List<Mrs00695RDO> listExpMestMedicineBefore = new List<Mrs00695RDO>(); // DS thuoc xuat truoc ki
        List<Mrs00695RDO> listExpMestMaterialBefore = new List<Mrs00695RDO>(); // DS vat tu xuat truoc ki
        //Cac list nhap xuat trong ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineOn = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap trong ki
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialOn = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap trong ki
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOn = new List<V_HIS_EXP_MEST_MEDICINE>(); // DS thuoc xuat trong ki
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOn = new List<V_HIS_EXP_MEST_MATERIAL>(); // DS vat tu xuat trong ki
        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<V_HIS_IMP_MEST_MEDICINE> InputMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc dau vao kiem ke
        List<V_HIS_IMP_MEST_MATERIAL> InputMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu dau vao kiem ke
        List<DETAIL> ListDetail = new List<DETAIL>(); // Chi tiet
        List<HIS_IMP_MEST> ListImpMest = new List<HIS_IMP_MEST>();
        Dictionary<long, HIS_MEDI_STOCK_MATY> DicMediStockMaty = new Dictionary<long, HIS_MEDI_STOCK_MATY>();
        Dictionary<long, HIS_MEDI_STOCK_METY> DicMediStockMety = new Dictionary<long, HIS_MEDI_STOCK_METY>();

        List<long> medicineId = new List<long>();
        List<long> materialId = new List<long>();

        List<HIS_MEDI_STOCK> MestRoom;

        HIS_MEDI_STOCK_PERIOD PreviousStock = new HIS_MEDI_STOCK_PERIOD();

        public Mrs00695Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00695Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00695Filter)this.reportFilter;
            try
            {
                //gán lại thời gian để không get dữ liệu khác ngoài dữ liệu của kỳ
                if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    castFilter.TIME_FROM = 0;
                    castFilter.TIME_TO = 0;
                }

                //Tao loai nhap xuat
                makeRdo();

                //Loai thuoc, vat tu
                GetMedicineTypeMaterialType();

                //Danh sach chot ki gan nhat cua kho
                GetMestMediPeriod();

                //Lấy danh sách kho đưa ra sau để nếu chọn kỳ không chọn kho thì vẫn gán lại kho theo kỳ được chọn
                ///Danh sách kho
                GetMediStock();

                //Cac phieu khi chot ki
                GetMestPeriodMediMate(dicMediStockPeriod);

                //Cac phieu tu sau chot ki den TIME_FROM
                GetAfterPeriod(dicMediStockPeriod);
                //Nhap thuoc, vat tu trong ki
                GetImpMestMediMate(ListMediStock.Select(o => o.ID).ToList(), castFilter.TIME_FROM, castFilter.TIME_TO, ref listImpMestMedicineOn, ref listImpMestMaterialOn);
                //Xuat thuoc, vat tu trong ki
                GetExpMestMediMate(ListMediStock.Select(o => o.ID).ToList(), castFilter.TIME_FROM, castFilter.TIME_TO, ref listExpMestMedicineOn, ref listExpMestMaterialOn);
                var listMediStockNoPeriod = ListMediStock.Where(o => !dicMediStockPeriod.ContainsKey(o.ID)).Select(o => o.ID).ToList();
                //Cac phieu tu dau den TIME_FORM voi truong hop khong co chot ki
                if (listMediStockNoPeriod.Count > 0)
                {
                    GetMestMediMate(listMediStockNoPeriod, null, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listImpMestMaterialBefore, ref listExpMestMedicineBefore, ref listExpMestMaterialBefore);
                }

                //Lay thong tin medicine, material cua cac thuoc vat tu
                GetMediMate();

                //Lay cac thuoc vat tu dau vao kiem ke
                GetInputEndAmount();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void makeRdo()
        {
            //Danh sach loai nhap, loai xuat
            Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
            Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00695RDO>();

            foreach (var item in piAmount)
            {
                if (dicImpMestType.ContainsKey(item.Name))
                {
                    if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item;
                }
                else if (dicExpMestType.ContainsKey(item.Name))
                {
                    if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item;
                }
            }
        }

        private void GetMediStock()
        {
            if (this.castFilter.MEDI_STOCK_CABINET_ID.HasValue || this.castFilter.MEDI_STOCK_CABINET_IDs != null)
            {
                HisMediStockViewFilterQuery HisMediStockfilter = new HisMediStockViewFilterQuery();
                if (this.castFilter.MEDI_STOCK_CABINET_ID.HasValue) HisMediStockfilter.ID = this.castFilter.MEDI_STOCK_CABINET_ID.Value;
                if (this.castFilter.MEDI_STOCK_CABINET_IDs != null) HisMediStockfilter.IDs = this.castFilter.MEDI_STOCK_CABINET_IDs;
                ListMediStock = new HisMediStockManager(param).GetView(HisMediStockfilter);
            }
            else
            {
                throw new Exception("Mrs00695. Nguoi dung chua chon kho");
            }
            HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
            
            if (castFilter.MEST_ROOM_STOCK_ID.HasValue)
            {
               mediStockFilter.ID = castFilter.MEST_ROOM_STOCK_ID.Value;
            }
            if (castFilter.MEDI_STOCK_IDs!=null)
            {
                mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
            }
            MestRoom = new HisMediStockManager(param).Get(mediStockFilter);
        }

        private void GetMedicineTypeMaterialType()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            if (IsNotNullOrEmpty(ListMedicineType))
            {
                foreach (var item in ListMedicineType)
                {
                    dicMedicineType[item.ID] = item;
                }
            }

            var ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            if (IsNotNullOrEmpty(ListMaterialType))
            {
                foreach (var item in ListMaterialType)
                {
                    dicMaterialType[item.ID] = item;
                }
            }

            var listMediStockMaty = new HisMediStockMatyManager().GetByMediStockId(castFilter.MEDI_STOCK_CABINET_ID ?? 0);
            if (IsNotNullOrEmpty(listMediStockMaty))
            {
                foreach (var item in listMediStockMaty)
                {
                    DicMediStockMaty[item.MATERIAL_TYPE_ID] = item;
                }
            }

            var listMediStockMety = new HisMediStockMetyManager().GetByMediStockId(castFilter.MEDI_STOCK_CABINET_ID ?? 0);
            if (IsNotNullOrEmpty(listMediStockMety))
            {
                foreach (var item in listMediStockMety)
                {
                    DicMediStockMety[item.MEDICINE_TYPE_ID] = item;
                }
            }
        }

        private void GetMestMediPeriod()
        {
            CommonParam paramGet = new CommonParam();
            HisMediStockPeriodFilterQuery periodFilter = new HisMediStockPeriodFilterQuery();
            if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
            {
                periodFilter.ID = castFilter.MEDI_STOCK_PERIOD_ID;
            }
            else
            {
                periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                periodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_CABINET_ID;
            }

            periodFilter.ORDER_DIRECTION = "DESC";
            periodFilter.ORDER_FIELD = "CREATE_TIME";
            var listPeriod = new HisMediStockPeriodManager(paramGet).Get(periodFilter);
            if (IsNotNullOrEmpty(listPeriod))
            {
                listPeriod = listPeriod.Where(o => o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE).ToList();

                if (listPeriod.Count == 1)
                {
                    //khi có 1 kỳ đã chốt thì 
                    //gán lại điều kiện lọc theo kho tránh gửi lên thông tin kỳ của kho khác
                    castFilter.MEDI_STOCK_CABINET_ID = listPeriod.First().MEDI_STOCK_ID;

                    HisMediStockPeriodFilterQuery perFilter = new HisMediStockPeriodFilterQuery();
                    perFilter.PREVIOUS_ID = listPeriod.First().ID;
                    var listPer = new HisMediStockPeriodManager(paramGet).Get(perFilter);
                    if (IsNotNullOrEmpty(listPer))
                    {
                        PreviousStock = listPer.First();
                    }
                }

                foreach (var item in listPeriod)
                {
                    if (dicMediStockPeriod.ContainsKey(item.MEDI_STOCK_ID)) continue;
                    dicMediStockPeriod[item.MEDI_STOCK_ID] = item;
                }
            }
        }

        private void GetMestPeriodMediMate(Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod)
        {
            CommonParam paramGet = new CommonParam();
            if (!IsNotNullOrEmpty(dicMediStockPeriod)) return;
            foreach (var item in dicMediStockPeriod)
            {
                HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery();
                mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = item.Value.ID;
                listMestPeriodMedi.AddRange(new HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter) ?? new List<V_HIS_MEST_PERIOD_MEDI>());
                HisMestPeriodMateViewFilterQuery mestPeriodMateFilter = new HisMestPeriodMateViewFilterQuery();
                mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = item.Value.ID;
                listMestPeriodMate.AddRange(new HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateFilter) ?? new List<V_HIS_MEST_PERIOD_MATE>());
            }
        }

        private void GetAfterPeriod(Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod)
        {
            if (!IsNotNullOrEmpty(dicMediStockPeriod)) return;
            foreach (var item in dicMediStockPeriod)
            {
                GetMestMediMate(new List<long> { item.Key }, item.Value.CREATE_TIME, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listImpMestMaterialBefore, ref listExpMestMedicineBefore, ref listExpMestMaterialBefore);
            }
        }

        private void GetMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials, ref List<Mrs00695RDO> expMestMedicines, ref List<Mrs00695RDO> expMestMaterials)
        {
            //Nhap thuoc, vat tu
            GetImpMestMediMate(listMediStockId, timeFrom, timeTo, ref impMestMedicines, ref impMestMaterials);
            //Xuat thuoc, vat tu
            GetExpMestMediMate(listMediStockId, timeFrom, timeTo, ref expMestMedicines, ref expMestMaterials);
        }

        private void GetImpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
            impMestFilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            impMestFilter.IMP_TIME_FROM = timeFrom;
            impMestFilter.IMP_TIME_TO = timeTo;
            impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            var listImpMest = new HisImpMestManager(paramGet).Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
            ListImpMest.AddRange(listImpMest);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            List<long> listImpMestId = new List<long>();
            //chọn kho bổ sung thuốc cho tủ trực
            //lọc theo các phiếu xuất tương ứng phiếu nhập của kho được chọn
            if (castFilter.MEST_ROOM_STOCK_ID.HasValue && MestRoom != null)
            {
                var mestRoom = MestRoom.Where(x => x.ID == castFilter.MEST_ROOM_STOCK_ID.Value).FirstOrDefault();
                listImpMestId = GetImpMestIdByMestRoom(listImpMest, mestRoom,MestRoom);
            }
            else
            {
                listImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
            }

            if (listImpMestId != null && listImpMestId.Count > 0)
            {
                var skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var limit = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery ImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery();
                    ImpMestMedicinefilter.IMP_MEST_IDs = limit;
                    var ImpMestMedicineSub = new HisImpMestMedicineManager(paramGet).GetView(ImpMestMedicinefilter);
                    if (paramGet.HasException)
                        throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
                    impMestMedicines.AddRange(ImpMestMedicineSub);
                }

                medicineId.AddRange(impMestMedicines.Select(s => s.MEDICINE_ID).Distinct().ToList());
                skip = 0;
                while (listImpMestId.Count - skip > 0)
                {
                    var limit = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery ImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery();
                    ImpMestMaterialfilter.IMP_MEST_IDs = limit;
                    var ImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(ImpMestMaterialfilter);
                    if (paramGet.HasException)
                        throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
                    impMestMaterials.AddRange(ImpMestMaterialSub);
                }

                materialId.AddRange(impMestMaterials.Select(s => s.MATERIAL_ID).Distinct().ToList());
            }
        }

        private List<long> GetImpMestIdByMestRoom(List<HIS_IMP_MEST> listImpMest, HIS_MEDI_STOCK MestRoom,List<HIS_MEDI_STOCK> listMestRoom)
        {
            List<long> result = null;
            try
            {
                if (IsNotNullOrEmpty(listImpMest) && IsNotNull(MestRoom))
                {
                    List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
                    var expId = listImpMest.Select(o => o.CHMS_EXP_MEST_ID ?? 0).Distinct().ToList();
                    var skip = 0;
                    while (expId.Count - skip > 0)
                    {
                        var limit = expId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestFilterQuery expFilter = new HisExpMestFilterQuery();
                        expFilter.MEDI_STOCK_ID = MestRoom.ID;
                        if (castFilter.MEDI_STOCK_IDs!=null)
                        {
                            expFilter.MEDI_STOCK_IDs = listMestRoom.Select(x => x.ID).Distinct().ToList();
                        }
                        expFilter.IDs = limit;
                        var expMest = new HisExpMestManager().Get(expFilter);
                        if (IsNotNullOrEmpty(expMest))
                        {
                            listExpMest.AddRange(expMest);
                        }
                    }

                    var impMest = listImpMest.Where(o => listExpMest.Select(s => s.ID).Contains(o.CHMS_EXP_MEST_ID ?? 0)).ToList();

                    if (IsNotNullOrEmpty(impMest))
                    {
                        result = impMest.Select(s => s.ID).Distinct().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GetExpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<Mrs00695RDO> expMestMedicines, ref List<Mrs00695RDO> expMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMedicinefilter.EXP_TIME_FROM = timeFrom;
            ExpMestMedicinefilter.EXP_TIME_TO = timeTo;
            ExpMestMedicinefilter.IS_EXPORT = true;
            var ExpMestMedicineSub = new ManagerSql().Get(ExpMestMedicinefilter);
            if (ExpMestMedicineSub != null)
            {
                expMestMedicines.AddRange(ExpMestMedicineSub);
            }

            HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
            ExpMestMaterialfilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMaterialfilter.EXP_TIME_FROM = timeFrom;
            ExpMestMaterialfilter.EXP_TIME_TO = timeTo;
            ExpMestMaterialfilter.IS_EXPORT = true;
            var ExpMestMaterialSub = new ManagerSql().Get(ExpMestMaterialfilter);
            if (ExpMestMaterialSub != null)
            {
                expMestMaterials.AddRange(ExpMestMaterialSub);
            }
        }

        private void GetExpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines, ref List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMedicinefilter.EXP_TIME_FROM = timeFrom;
            ExpMestMedicinefilter.EXP_TIME_TO = timeTo;
            ExpMestMedicinefilter.IS_EXPORT = true;
            var ExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicinefilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            if (ExpMestMedicineSub != null)
            {
                expMestMedicines.AddRange(ExpMestMedicineSub);
            }
            HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
            ExpMestMaterialfilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMaterialfilter.EXP_TIME_FROM = timeFrom;
            ExpMestMaterialfilter.EXP_TIME_TO = timeTo;
            ExpMestMaterialfilter.IS_EXPORT = true;
            var ExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialfilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
            if (ExpMestMaterialSub != null)
            {
                expMestMaterials.AddRange(ExpMestMaterialSub);
            }
        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();
                if (listMestPeriodMedi != null)
                {
                    medicineIds.AddRange(listMestPeriodMedi.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listImpMestMedicineBefore != null)
                {
                    medicineIds.AddRange(listImpMestMedicineBefore.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listExpMestMedicineBefore != null)
                {
                    medicineIds.AddRange(listExpMestMedicineBefore.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listImpMestMedicineOn != null)
                {
                    medicineIds.AddRange(listImpMestMedicineOn.Select(o => o.MEDICINE_ID).ToList());
                }
                if (listExpMestMedicineOn != null)
                {
                    medicineIds.AddRange(listExpMestMedicineOn.Select(o => o.MEDICINE_ID ?? 0).ToList());
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
                if (listMestPeriodMate != null)
                {
                    materialIds.AddRange(listMestPeriodMate.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listImpMestMaterialBefore != null)
                {
                    materialIds.AddRange(listImpMestMaterialBefore.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listExpMestMaterialBefore != null)
                {
                    materialIds.AddRange(listExpMestMaterialBefore.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listImpMestMaterialOn != null)
                {
                    materialIds.AddRange(listImpMestMaterialOn.Select(o => o.MATERIAL_ID).ToList());
                }
                if (listExpMestMaterialOn != null)
                {
                    materialIds.AddRange(listExpMestMaterialOn.Select(o => o.MATERIAL_ID ?? 0).ToList());
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
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetInputEndAmount()
        {
            try
            {
                InputMedicines = new ManagerSql().GetMediInput(this.castFilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                InputMaterials = new ManagerSql().GetMateInput(this.castFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                var listSub = (from r in listMestPeriodMedi select new Mrs00695RDO(r, dicMediStockPeriod, dicMedicineType, Medicines, DicMediStockMety)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listMestPeriodMate select new Mrs00695RDO(r, dicMediStockPeriod, dicMaterialType, Materials, DicMediStockMaty)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineBefore select new Mrs00695RDO(r, dicMedicineType, Medicines, DicMediStockMety)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialBefore select new Mrs00695RDO(r, dicMaterialType, Materials, DicMediStockMaty)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineBefore select new Mrs00695RDO(r, dicMedicineType, Medicines, DicMediStockMety)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialBefore select new Mrs00695RDO(r, dicMaterialType, Materials, DicMediStockMaty)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineOn select new Mrs00695RDO(r, dicMedicineType, dicImpAmountType, Medicines, DicMediStockMety)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialOn select new Mrs00695RDO(r, dicMaterialType, dicImpAmountType, Materials, DicMediStockMaty)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineOn select new Mrs00695RDO(r, dicMedicineType, dicExpAmountType, Medicines, DicMediStockMety)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialOn select new Mrs00695RDO(r, dicMaterialType, dicExpAmountType, Materials, DicMediStockMaty)).ToList();
                ListRdo.AddRange(listSub);

                AddImpMestMedicineDetail();
                AddImpMestMaterialDetail();
                AddExpMestMedicineDetail();
                AddExpMestMaterialDetail();

                //lọc bỏ các dòng không nhập
                if (IsNotNullOrEmpty(medicineId))
                {
                    medicineId = medicineId.Distinct().ToList();
                    ListRdo = ListRdo.Where(o => o.TYPE != Mrs00695RDO.THUOC || (o.TYPE == Mrs00695RDO.THUOC && medicineId.Contains(o.MEDI_MATE_ID))).ToList();
                }

                if (IsNotNullOrEmpty(materialId))
                {
                    materialId = materialId.Distinct().ToList();
                    ListRdo = ListRdo.Where(o => o.TYPE != Mrs00695RDO.VATTU || (o.TYPE == Mrs00695RDO.VATTU && materialId.Contains(o.MEDI_MATE_ID))).ToList();
                }

                if (castFilter.IS_GROUP == true)
                {
                    ProcessGroupMediMate();
                }
                else
                {
                    //Gộp theo thuốc, theo giá
                    ProcessGroup();
                }
                
                

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo = new List<Mrs00695RDO>();
                result = false;
            }
            return result;
        }

        private void AddImpMestMedicineDetail()
        {
            try
            {
                foreach (var item in listImpMestMedicineOn)
                {
                    DETAIL rdo = new DETAIL();
                    rdo.IMP_MEST_CODE = item.IMP_MEST_CODE;
                    rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    var expMestMedicine = listExpMestMedicineOn.FirstOrDefault(o => o.CK_IMP_MEST_MEDICINE_ID == item.ID);
                    if (expMestMedicine != null)
                    {
                        rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMestMedicine.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    }
                    var impMest = ListImpMest.Where(x => x.ID == item.AGGR_IMP_MEST_ID).FirstOrDefault();
                    if (impMest != null)
                    {
                        rdo.AGGR_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                    }
                    rdo.MATE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                    rdo.MATE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                    rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                    rdo.MANUFACTURER_NAME = item.MANUFACTURER_NAME;
                    rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                    rdo.EXPIRED_DATE = item.EXPIRED_DATE ?? 0;
                    rdo.CONCENTRA = item.CONCENTRA;
                    rdo.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
                    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    rdo.AMOUNT = item.AMOUNT;
                    rdo.PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                    ListDetail.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddImpMestMaterialDetail()
        {
            try
            {
                foreach (var item in listImpMestMaterialOn)
                {
                    DETAIL rdo = new DETAIL();
                    rdo.IMP_MEST_CODE = item.IMP_MEST_CODE;
                    rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    var expMestMaterial = listExpMestMaterialOn.FirstOrDefault(o => o.CK_IMP_MEST_MATERIAL_ID == item.ID);
                    if (expMestMaterial != null)
                    {
                        rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMestMaterial.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    }
                    var impMest = ListImpMest.Where(x => x.ID == item.AGGR_IMP_MEST_ID).FirstOrDefault();
                    if (impMest != null)
                    {
                        rdo.AGGR_IMP_MEST_CODE = impMest.IMP_MEST_CODE;
                    }
                    rdo.MATE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                    rdo.MATE_TYPE_NAME =  item.MATERIAL_TYPE_NAME;
                    rdo.EXPIRED_DATE = item.EXPIRED_DATE ?? 0;
                    rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                    rdo.MANUFACTURER_NAME = item.MANUFACTURER_NAME;
                    rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    rdo.AMOUNT = item.AMOUNT;
                    rdo.PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                    ListDetail.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddExpMestMedicineDetail()
        {
            try
            {
                foreach (var item in listExpMestMedicineOn)
                {
                    DETAIL rdo = new DETAIL();
                    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                    rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    if (item.CK_IMP_MEST_MEDICINE_ID != null)
                    {
                        var impMestMedicine = listImpMestMedicineOn.FirstOrDefault(o => o.ID == item.CK_IMP_MEST_MEDICINE_ID);
                        if (impMestMedicine != null)
                        {
                            rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMestMedicine.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        }
                    }

                    rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                    rdo.CONCENTRA = item.CONCENTRA;
                    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    rdo.AMOUNT = item.AMOUNT;
                    ListDetail.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddExpMestMaterialDetail()
        {
            try
            {
                foreach (var item in listExpMestMaterialOn)
                {
                    DETAIL rdo = new DETAIL();
                    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                    rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                    if (item.CK_IMP_MEST_MATERIAL_ID != null)
                    {
                        var impMestMaterial = listImpMestMaterialOn.FirstOrDefault(o => o.ID == item.CK_IMP_MEST_MATERIAL_ID);
                        if (impMestMaterial != null)
                        {
                            rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMestMaterial.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                        }
                    }
                    
                    rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                    rdo.AMOUNT = item.AMOUNT;
                    ListDetail.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroup()
        {
            try
            {
                GroupByID();
                GroupByPrice();
                AddInfo();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessGroupMediMate()
        {
            try
            {
                GroupAll();
                AddInfoAll();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GroupAll()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.SERVICE_CODE, o.SERVICE_NAME, o.IMP_PRICE }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00695RDO rdo;
                List<Mrs00695RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00695RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00695RDO();
                    listSub = item.ToList<Mrs00695RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                Inventec.Common.Logging.LogSystem.Info(errorField);
            }
        }

        private void AddInfoAll()
        {
            foreach (var item in ListRdo)
            {
                item.SERVICE_TYPE_NAME = item.TYPE == 1 ? "THUỐC" : "VẬT TƯ";
                item.VAT_RATIO_STR = string.Format("'{0}%", (long)(100 * item.VAT_RATIO ?? 0));
                item.INPUT_END_AMOUNT = item.TYPE == 1 ? InputMedicines.Where(o => o.MEDICINE_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE).Sum(s => s.AMOUNT) : InputMaterials.Where(o => o.MATERIAL_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE).Sum(s => s.AMOUNT);
            }
        }

        private void GroupByID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_ID, o.TYPE, o.MEDI_STOCK_ID }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00695RDO rdo;
                List<Mrs00695RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00695RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00695RDO();
                    listSub = item.ToList<Mrs00695RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                Inventec.Common.Logging.LogSystem.Info(errorField);
            }
        }

        private Mrs00695RDO IsMeaningful(List<Mrs00695RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00695RDO();
        }

        private void GroupByPrice()
        {
            var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_TYPE_ID, o.IMP_PRICE, o.TYPE, o.MEDI_STOCK_ID, o.SUPPLIER_ID, o.EXPIRED_DATE }).ToList();
            ListRdo.Clear();
            Decimal sum = 0;
            Mrs00695RDO rdo;
            List<Mrs00695RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00695RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00695RDO();
                listSub = item.ToList<Mrs00695RDO>();

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }

                if (!hide) ListRdo.Add(rdo);
            }
        }

        private void AddInfo()
        {
            foreach (var item in ListRdo)
            {
                item.SERVICE_TYPE_NAME = item.TYPE == 1 ? "THUỐC" : "VẬT TƯ";
                item.VAT_RATIO_STR = string.Format("'{0}%", (long)(100 * item.VAT_RATIO ?? 0));
                item.INPUT_END_AMOUNT = item.TYPE == 1 ? InputMedicines.Where(o => o.MEDICINE_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && (o.EXPIRED_DATE ?? 0) == (item.EXPIRED_DATE ?? 0)).Sum(s => s.AMOUNT) : InputMaterials.Where(o => o.MATERIAL_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && (o.EXPIRED_DATE ?? 0) == (item.EXPIRED_DATE ?? 0)).Sum(s => s.AMOUNT);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                long timeFrom = 0;
                long timeTo = 0;
                //có chọn kỳ thì sẽ ko có thời gian
                //Lấy thời gian từ đến là thời gian của kỳ được chọn và kỳ trước đó
                if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    if (PreviousStock != null)
                    {
                        if (PreviousStock.TO_TIME.HasValue)
                            timeFrom = PreviousStock.TO_TIME.Value;
                        else
                            timeFrom = PreviousStock.CREATE_TIME ?? 0;
                    }

                    if (dicMediStockPeriod.Count > 0 && dicMediStockPeriod.First().Value != null)
                    {
                        if (dicMediStockPeriod.First().Value.TO_TIME.HasValue)
                            timeTo = dicMediStockPeriod.First().Value.TO_TIME.Value;
                        else
                            timeTo = dicMediStockPeriod.First().Value.CREATE_TIME ?? 0;
                    }
                }
                else
                {
                    if (castFilter.TIME_FROM.HasValue)
                        timeFrom = castFilter.TIME_FROM.Value;

                    if (castFilter.TIME_TO.HasValue)
                        timeTo = castFilter.TIME_TO.Value;
                }

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(timeFrom));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(timeTo));

                AddObjectKeyIntoListkey(ListMediStock.FirstOrDefault(), dicSingleTag, true);

                if (MestRoom != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_EXP_NAME", string.Join(",", MestRoom.Select(x=>x.MEDI_STOCK_NAME).ToList()));
                }

                if (castFilter.IS_MEDICINE == false && castFilter.IS_MATERIAL == true)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE == 2).ToList();
                }

                if (castFilter.IS_MEDICINE == true && castFilter.IS_MATERIAL == false)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE == 1).ToList();
                }

                List<Mrs00695RDO> ListRdoGroup = new List<Mrs00695RDO>();
                List<Mrs00695RDO> ListRdoParent = new List<Mrs00695RDO>();
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var grGroup = ListRdo.GroupBy(o => new { o.TYPE, o.MEDICINE_GROUP_CODE }).ToList();
                    foreach (var group in grGroup)
                    {
                        ListRdoGroup.Add(group.First());
                        var grParent = group.GroupBy(o => o.MEDI_MATE_PARENT_CODE).ToList();
                        foreach (var pa in grParent)
                        {
                            ListRdoParent.Add(pa.First());
                        }
                    }
                }

                objectTag.AddObjectData(store, "Services", ListRdo.OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "MetyMateGroup", ListRdoGroup.OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "MetyMateParent", ListRdoParent.OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());

                objectTag.AddRelationship(store, "MetyMateGroup", "MetyMateParent", new string[] { "TYPE", "MEDICINE_GROUP_CODE" }, new string[] { "TYPE", "MEDICINE_GROUP_CODE" });
                objectTag.AddRelationship(store, "MetyMateGroup", "Services", new string[] { "TYPE", "MEDICINE_GROUP_CODE", "MEDI_MATE_PARENT_CODE" }, new string[] { "TYPE", "MEDICINE_GROUP_CODE", "MEDI_MATE_PARENT_CODE" });
                objectTag.AddRelationship(store, "MetyMateParent", "Services", "MEDI_MATE_PARENT_CODE", "MEDI_MATE_PARENT_CODE");
                objectTag.AddObjectData(store, "ReportMediMate", ListDetail);
                objectTag.AddObjectData(store, "Detail", ListDetail);
                objectTag.SetUserFunction(store, "Element", new RDOElement());
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
                objectTag.AddObjectData(store, "SumMediStock", SumOfMediStock().OrderBy(o => o.TYPE).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected void AddObjectKeyIntoListkey<T>(T data, Dictionary<string, object> dicSingleTag, bool isOveride)
        {
            try
            {
                PropertyInfo[] pis = typeof(T).GetProperties();
                if (pis != null && pis.Length > 0)
                {
                    foreach (var pi in pis)
                    {
                        if (pi.GetGetMethod().IsVirtual) continue;

                        if (!dicSingleTag.ContainsKey(pi.Name))
                        {
                            dicSingleTag.Add(pi.Name, (data != null ? pi.GetValue(data) : null));
                        }
                        else
                        {
                            if (isOveride)
                                dicSingleTag[pi.Name] = (data != null ? pi.GetValue(data) : null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private List<Mrs00695RDO> SumOfMediStock()
        {
            List<Mrs00695RDO> SumMediStock = new List<Mrs00695RDO>();
            try
            {
                foreach (var item in ListMediStock)
                {
                    Mrs00695RDO rdo = new Mrs00695RDO();
                    rdo.SERVICE_TYPE_NAME = "THUỐC";
                    rdo.TYPE = 1;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);

                    rdo = new Mrs00695RDO();
                    rdo.SERVICE_TYPE_NAME = "VTYT";
                    rdo.TYPE = 2;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00695RDO>();
            }
            return SumMediStock;
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long MediStockId;
        int SameType;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (SameType == ServiceId && MediStockId == mediId)
                    {
                        return true;
                    }
                    else
                    {
                        MediStockId = mediId;
                        SameType = ServiceId;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
