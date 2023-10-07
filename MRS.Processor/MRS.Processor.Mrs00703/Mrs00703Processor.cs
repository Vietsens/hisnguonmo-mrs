using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00703
{
    class Mrs00703Processor : AbstractProcessor
    {
        private List<Mrs00703RDO> ListRdo = new List<Mrs00703RDO>();
        Mrs00703Filter castFilter = new Mrs00703Filter();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, HIS_MEDI_STOCK_PERIOD> dicMediStockPeriod = new Dictionary<long, HIS_MEDI_STOCK_PERIOD>();
        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();

        //Cac list dau ki
        List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = new List<V_HIS_MEST_PERIOD_MEDI>(); // DS thuoc chot ki
        List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = new List<V_HIS_MEST_PERIOD_MATE>(); // DS vat tu chot ki

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBefore = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap truoc ki
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialBefore = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap truoc ki

        List<Mrs00703RDO> listExpMestMedicineBefore = new List<Mrs00703RDO>(); // DS thuoc xuat truoc ki
        List<Mrs00703RDO> listExpMestMaterialBefore = new List<Mrs00703RDO>(); // DS vat tu xuat truoc ki

        List<HIS_IMP_MEST> listImpMestAll = new List<HIS_IMP_MEST>(); //Phieu nhap

        //Cac list nhap xuat trong ki
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineOn = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc nhap trong ki
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialOn = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu nhap trong ki

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOn = new List<V_HIS_EXP_MEST_MEDICINE>(); // DS thuoc xuat trong ki
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOn = new List<V_HIS_EXP_MEST_MATERIAL>(); // DS vat tu xuat trong ki

        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<ImpMestIdChmsMediStockId> listImpMestIdChmsMediStockId = new List<ImpMestIdChmsMediStockId>();//kho xuat chuyen kho cho phieu nhap

        List<TotalPriceInStock> ListTotalPriceInStock = new List<TotalPriceInStock>();//chi tiết giá gom theo tài khoản
        List<TotalPriceImpExpStock> ListTotalPriceImpStock = new List<TotalPriceImpExpStock>();//chi tiết nhập chuyển kho gom theo tài khoản, kho
        List<TotalPriceImpExpStock> ListTotalPriceExpStock = new List<TotalPriceImpExpStock>();//chi tiết xuất chuyển kho gom theo tài khoản, kho

        List<HIS_EXP_MEST> ListExpMest = new List<HIS_EXP_MEST>();

        public Mrs00703Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00703Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00703Filter)this.reportFilter;
            try
            {
                //Tao loai nhap xuat
                //makeRdo();

                ///Danh sách kho
                GetMediStock();

                //Neu ket qua loc khong co kho phu hop thi bao loi ve may tram
                if (ListMediStock.Count == 0)
                {
                    return true;
                }
                //Loai thuoc, vat tu
                GetMedicineTypeMaterialType();

                //Danh sach chot ki gan nhat cua kho
                GetMestMediPeriod();

                //Cac phieu khi chot ki
                GetMestPeriodMediMate();

                //Cac phieu tu sau chot ki den TIME_FROM
                GetAfterPeriod();
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

                ////Lay cac kho xuat chuyen kho cho cac phieu nhap chuyen kho
                GetChmsMediStockForImpMest();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMediStock()
        {
            ListMediStock = HisMediStockCFG.HisMediStocks.Where(o => this.castFilter.MEDI_STOCK_ID == o.ID).ToList();

            if (this.castFilter.MEDI_STOCK_CABINET_IDs != null && this.castFilter.MEDI_STOCK_CABINET_IDs.Count > 0)
            {
                var ListMediStockCabinet = HisMediStockCFG.HisMediStocks.Where(o => this.castFilter.MEDI_STOCK_CABINET_IDs.Contains(o.ID)).ToList();
                ListMediStock.AddRange(ListMediStockCabinet);
            }
        }

        private void GetMedicineTypeMaterialType()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
            var ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
            if (IsNotNullOrEmpty(ListMedicineType))
            {
                foreach (var item in ListMedicineType)
                {
                    dicMedicineType[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListMaterialType))
            {
                foreach (var item in ListMaterialType)
                {
                    dicMaterialType[item.ID] = item;
                }
            }
        }

        private void GetMestMediPeriod()
        {
            CommonParam paramGet = new CommonParam();
            HisMediStockPeriodFilterQuery periodFilter = new HisMediStockPeriodFilterQuery();
            periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
            if (this.ListMediStock != null)
            {
                periodFilter.MEDI_STOCK_IDs = this.ListMediStock.Select(o => o.ID).ToList();
            }
            var listPeriod = new HisMediStockPeriodManager(paramGet).Get(periodFilter);
            if (IsNotNullOrEmpty(listPeriod))
            {
                listPeriod = listPeriod.Where(o => o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE).OrderByDescending(o => o.CREATE_TIME).ToList();
                foreach (var item in listPeriod)
                {
                    if (dicMediStockPeriod.ContainsKey(item.MEDI_STOCK_ID)) continue;
                    dicMediStockPeriod[item.MEDI_STOCK_ID] = item;
                }
            }
        }

        private void GetMestPeriodMediMate()
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

        private void GetAfterPeriod()
        {
            if (!IsNotNullOrEmpty(dicMediStockPeriod)) return;
            foreach (var item in dicMediStockPeriod)
            {
                GetMestMediMate(new List<long> { item.Key }, item.Value.CREATE_TIME, castFilter.TIME_FROM, ref listImpMestMedicineBefore, ref listImpMestMaterialBefore, ref listExpMestMedicineBefore, ref listExpMestMaterialBefore);
            }
        }

        private void GetMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials, ref List<Mrs00703RDO> expMestMedicines, ref List<Mrs00703RDO> expMestMaterials)
        {
            //Nhap thuoc, vat tu
            GetImpMestMediMate(listMediStockId, timeFrom, timeTo, ref impMestMedicines, ref impMestMaterials);
            //Xuat thuoc, vat tu
            GetExpMestMediMate(listMediStockId, timeFrom, timeTo, ref expMestMedicines, ref expMestMaterials);
        }

        private void GetExpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<Mrs00703RDO> expMestMedicines, ref List<Mrs00703RDO> expMestMaterials)
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

        private void GetImpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines, ref List<V_HIS_IMP_MEST_MATERIAL> impMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
            impMestFilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            impMestFilter.IMP_TIME_FROM = timeFrom;
            impMestFilter.IMP_TIME_TO = timeTo;
            impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            var listImpMest = new HisImpMestManager(paramGet).Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");

            if (listImpMest != null && listImpMest.Count > 0)
            {
                listImpMestAll.AddRange(listImpMest);
                var listImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
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
                            throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");
                        impMestMedicines.AddRange(ImpMestMedicineSub);
                    }
                    skip = 0;
                    while (listImpMestId.Count - skip > 0)
                    {
                        var limit = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestMaterialViewFilterQuery ImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery();
                        ImpMestMaterialfilter.IMP_MEST_IDs = limit;
                        var ImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(ImpMestMaterialfilter);
                        if (paramGet.HasException)
                            throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");
                        impMestMaterials.AddRange(ImpMestMaterialSub);
                    }
                }
            }
        }

        private void GetExpMestMediMate(List<long> listMediStockId, long? timeFrom, long? timeTo, ref List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines, ref List<V_HIS_EXP_MEST_MATERIAL> expMestMaterials)
        {
            CommonParam paramGet = new CommonParam();
            List<long> expMestIds = new List<long>();
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMedicinefilter.EXP_TIME_FROM = timeFrom;
            ExpMestMedicinefilter.EXP_TIME_TO = timeTo;
            ExpMestMedicinefilter.IS_EXPORT = true;
            var ExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicinefilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");
            if (ExpMestMedicineSub != null)
            {
                expMestMedicines.AddRange(ExpMestMedicineSub);
                expMestIds.AddRange(ExpMestMedicineSub.Select(s => s.EXP_MEST_ID ?? 0).ToList());
            }
            HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
            ExpMestMaterialfilter.MEDI_STOCK_IDs = listMediStockId.Count > 0 ? listMediStockId : null;
            ExpMestMaterialfilter.EXP_TIME_FROM = timeFrom;
            ExpMestMaterialfilter.EXP_TIME_TO = timeTo;
            ExpMestMaterialfilter.IS_EXPORT = true;
            var ExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialfilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00703");
            if (ExpMestMaterialSub != null)
            {
                expMestMaterials.AddRange(ExpMestMaterialSub);
                expMestIds.AddRange(ExpMestMaterialSub.Select(s => s.EXP_MEST_ID ?? 0).ToList());
            }

            if (expMestIds != null && expMestIds.Count > 0)
            {
                expMestIds = expMestIds.Distinct().ToList();
                int skip = 0;
                while (expMestIds.Count - skip > 0)
                {
                    var listIDs = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisExpMestFilterQuery expFilter = new HisExpMestFilterQuery();
                    expFilter.IDs = listIDs;
                    var ExpMestSub = new HisExpMestManager(paramGet).Get(expFilter);
                    if (IsNotNullOrEmpty(ExpMestSub))
                    {
                        ListExpMest.AddRange(ExpMestSub);
                    }
                }
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
                LogSystem.Error(ex);

            }
        }

        private void GetChmsMediStockForImpMest()
        {
            try
            {
                if (listImpMestAll != null)
                {
                    var listImpMestChmsNoChmsMediStock = listImpMestAll.Where(o => (o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK || o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS) && o.CHMS_MEDI_STOCK_ID == null).ToList();
                    if (listImpMestChmsNoChmsMediStock.Count > 0)
                    {
                        listImpMestIdChmsMediStockId = new ManagerSql().GetChmsMediStockId(listImpMestChmsNoChmsMediStock.Min(o => o.ID), listImpMestChmsNoChmsMediStock.Max(o => o.ID));
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
            bool result = false;
            try
            {
                var DicMediStock = ListMediStock.ToDictionary(o => o.ID, o => o);
                ListRdo.Clear();
                var listSub = (from r in listMestPeriodMedi select new Mrs00703RDO(r, dicMediStockPeriod, dicMedicineType, Medicines, DicMediStock)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listMestPeriodMate select new Mrs00703RDO(r, dicMediStockPeriod, dicMaterialType, Materials, DicMediStock)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineBefore select new Mrs00703RDO(r, dicMedicineType, Medicines, DicMediStock, listImpMestAll, listImpMestIdChmsMediStockId, true)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialBefore select new Mrs00703RDO(r, dicMaterialType, Materials, DicMediStock, listImpMestAll, listImpMestIdChmsMediStockId, true)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineBefore select new Mrs00703RDO(r, dicMedicineType, Medicines, DicMediStock)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialBefore select new Mrs00703RDO(r, dicMaterialType, Materials, DicMediStock)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMedicineOn select new Mrs00703RDO(r, dicMedicineType, Medicines, DicMediStock, listImpMestAll, listImpMestIdChmsMediStockId, false)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listImpMestMaterialOn select new Mrs00703RDO(r, dicMaterialType, Materials, DicMediStock, listImpMestAll, listImpMestIdChmsMediStockId, false)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMedicineOn select new Mrs00703RDO(r, dicMedicineType, Medicines, DicMediStock, ListExpMest)).ToList();
                ListRdo.AddRange(listSub);
                listSub = (from r in listExpMestMaterialOn select new Mrs00703RDO(r, dicMaterialType, Materials, DicMediStock, ListExpMest)).ToList();
                ListRdo.AddRange(listSub);

                //không có RECORDING_TRANSACTION sẽ không lấy
                ListRdo = ListRdo.Where(o => !String.IsNullOrWhiteSpace(o.RECORDING_TRANSACTION)).ToList();

                var listImpMestTypeId = new List<long> { 
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK
                    };

                var groupbyRetr = ListRdo.GroupBy(g => g.RECORDING_TRANSACTION).ToList();
                foreach (var retr in groupbyRetr)
                {
                    decimal beginCabinPrice = 0;
                    decimal endCabinPrice = 0;
                    if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_CABINET_IDs))
                    {
                        var cabinets = retr.Where(o => castFilter.MEDI_STOCK_CABINET_IDs.Contains(o.MEDI_STOCK_ID)).ToList();
                        if (IsNotNullOrEmpty(cabinets))
                        {
                            TotalPriceInStock sdo = new TotalPriceInStock();
                            sdo.RECORDING_TRANSACTION = retr.First().RECORDING_TRANSACTION;
                            sdo.MEDI_STOCK_ID = -1;
                            beginCabinPrice = cabinets.Sum(s => s.BEGIN_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            sdo.BEGIN_PRICE = beginCabinPrice;
                            endCabinPrice = cabinets.Sum(s => s.END_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            sdo.END_PRICE = endCabinPrice;
                            sdo.EXP_TOTAL_PRICE = cabinets.Sum(s => s.EXP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            sdo.IMP_TOTAL_PRICE = cabinets.Sum(s => s.IMP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            ListTotalPriceInStock.Add(sdo);
                        }
                    }

                    var rdoStock = retr.Where(o => o.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList();
                    if (IsNotNullOrEmpty(rdoStock))
                    {
                        TotalPriceInStock sdo = new TotalPriceInStock();
                        sdo.RECORDING_TRANSACTION = retr.First().RECORDING_TRANSACTION;
                        sdo.MEDI_STOCK_ID = rdoStock.First().MEDI_STOCK_ID;

                        sdo.BEGIN_PRICE = rdoStock.Sum(s => s.BEGIN_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                        sdo.END_PRICE = rdoStock.Sum(s => s.END_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                        sdo.EXP_TOTAL_PRICE = rdoStock.Sum(s => s.EXP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                        sdo.IMP_TOTAL_PRICE = rdoStock.Sum(s => s.IMP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));

                        var impNcc = rdoStock.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList();
                        if (IsNotNullOrEmpty(impNcc))
                        {
                            sdo.IMP_NCC_TOTAL_PRICE = impNcc.Sum(s => s.IMP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                        }

                        var expOther = rdoStock.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC).ToList();
                        if (IsNotNullOrEmpty(expOther))
                        {
                            sdo.EXP_OTHER_TOTAL_PRICE = expOther.Sum(s => s.EXP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                        }

                        //xuất quyết toán: xuất của báo cáo tkb30 - hoàn báo cáo 153 + tồn đầu các tủ trực - tồn cuối các tủ trực.
                        var tongxuat = rdoStock.Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC && o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).ToList();
                        var nhapTraLai = rdoStock.Where(o => listImpMestTypeId.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                        decimal tienXuat = IsNotNullOrEmpty(tongxuat) ? tongxuat.Sum(s => s.EXP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO)) : 0;
                        decimal tienHoan = IsNotNullOrEmpty(nhapTraLai) ? nhapTraLai.Sum(s => s.IMP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO)) : 0;

                        sdo.EXP_QT_TOTAL_PRICE = tienXuat - tienHoan + beginCabinPrice - endCabinPrice;

                        ListTotalPriceInStock.Add(sdo);
                    }
                    var listCabinet = HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET == 1).ToList();

                    var impChms = retr.Where(o => o.PREVIOUS_MEDI_STOCK_ID.HasValue && !listCabinet.Select(s => s.ID).Contains(o.PREVIOUS_MEDI_STOCK_ID ?? 0) && o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                    if (IsNotNullOrEmpty(impChms))
                    {
                        var grStock = impChms.GroupBy(o => o.PREVIOUS_MEDI_STOCK_ID).ToList();
                        foreach (var st in grStock)
                        {
                            TotalPriceImpExpStock ado = new TotalPriceImpExpStock();
                            ado.RECORDING_TRANSACTION = retr.First().RECORDING_TRANSACTION;
                            ado.MEDI_STOCK_ID = st.First().PREVIOUS_MEDI_STOCK_ID ?? 0;
                            ado.MEDI_STOCK_CODE = st.First().PREVIOUS_MEDI_STOCK_CODE;
                            ado.MEDI_STOCK_NAME = st.First().PREVIOUS_MEDI_STOCK_NAME;
                            ado.IMP_TOTAL_PRICE = st.Sum(s => s.IMP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            ListTotalPriceImpStock.Add(ado);
                        }
                    }

                    var expChms = retr.Where(o => o.PREVIOUS_MEDI_STOCK_ID.HasValue && !listCabinet.Select(s => s.ID).Contains(o.PREVIOUS_MEDI_STOCK_ID ?? 0) && o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).ToList();
                    if (IsNotNullOrEmpty(expChms))
                    {
                        var grStock = expChms.GroupBy(o => o.PREVIOUS_MEDI_STOCK_ID).ToList();
                        foreach (var st in grStock)
                        {
                            TotalPriceImpExpStock ado = new TotalPriceImpExpStock();
                            ado.RECORDING_TRANSACTION = retr.First().RECORDING_TRANSACTION;
                            ado.MEDI_STOCK_ID = st.First().PREVIOUS_MEDI_STOCK_ID ?? 0;
                            ado.MEDI_STOCK_CODE = st.First().PREVIOUS_MEDI_STOCK_CODE;
                            ado.MEDI_STOCK_NAME = st.First().PREVIOUS_MEDI_STOCK_NAME;
                            ado.EXP_TOTAL_PRICE = st.Sum(s => s.EXP_TOTAL_AMOUNT * s.IMP_PRICE * (1 + s.VAT_RATIO));
                            ListTotalPriceExpStock.Add(ado);
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00703RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_ID, o.TYPE, o.MEDI_STOCK_ID }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00703RDO rdo;
                List<Mrs00703RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00703RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00703RDO();
                    listSub = item.ToList<Mrs00703RDO>();
                    //lọc theo kho xuất thuốc đên kho báo cáo

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
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByPrice()
        {
            //try
            //{
            var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_TYPE_ID, o.IMP_PRICE, o.TYPE, o.MEDI_STOCK_ID, o.SUPPLIER_ID, o.EXPIRED_DATE }).ToList();
            ListRdo.Clear();
            Decimal sum = 0;
            Mrs00703RDO rdo;
            List<Mrs00703RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00703RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00703RDO();
                listSub = item.ToList<Mrs00703RDO>();

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

        private Mrs00703RDO IsMeaningful(List<Mrs00703RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00703RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (ListMediStock.Count == 0)
                {
                    throw new Exception("Không tạo được báo cáo theo điều kiện lọc đã chọn.");
                }

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                var stock = ListMediStock.FirstOrDefault(o => o.ID == castFilter.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK();
                dicSingleTag.Add("MEDI_STOCK_NAME", stock.MEDI_STOCK_NAME);
                dicSingleTag.Add("MEDI_STOCK_ID", stock.ID);
                string thang = "";
                if (castFilter.TIME_TO.HasValue)
                {
                    thang = (castFilter.TIME_TO ?? 0).ToString().Substring(4, 2);
                }
                dicSingleTag.Add("THANG", thang);

                decimal endPrice = ListTotalPriceInStock.Sum(s => s.END_PRICE);
                dicSingleTag.Add("END_PRICE", endPrice);

                List<TotalPriceImpExpStock> ListTotalImpStock = new List<TotalPriceImpExpStock>();
                List<TotalPriceImpExpStock> ListTotalExpStock = new List<TotalPriceImpExpStock>();

                ListTotalImpStock = ListTotalPriceImpStock.GroupBy(o => o.MEDI_STOCK_ID).Select(s => s.First()).ToList();
                ListTotalExpStock = ListTotalPriceExpStock.GroupBy(o => o.MEDI_STOCK_ID).Select(s => s.First()).ToList();

                var records = ListTotalPriceInStock.OrderBy(o => o.RECORDING_TRANSACTION).GroupBy(g => g.RECORDING_TRANSACTION).Select(s => s.First()).ToList();
                for (int i = 0; i < records.Count; i++)
                {
                    dicSingleTag.Add("RECORDING_TRANSACTION" + i, records[i].RECORDING_TRANSACTION);
                }

                objectTag.AddObjectData(store, "ReportPrice", ListTotalPriceInStock);

                objectTag.AddObjectData(store, "ReportImpDetail", ListTotalPriceImpStock);
                objectTag.AddObjectData(store, "ReportExpDetail", ListTotalPriceExpStock);

                objectTag.AddObjectData(store, "ReportImpStock", ListTotalImpStock);
                objectTag.AddObjectData(store, "ReportExpStock", ListTotalExpStock);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
//phần 1 là tồn kho của tất cả các kho được chọn. ở 2 điều kiện lọc
//1.1 là tồn kho của kho được chọn 
//1.2 tồn kho của tủ trực nếu ko chọn tủ trực thì để trống

//phần 2 là nhập của kho lẻ được chọn
//2.1: nhập từ nhà cung cấp
//2.2 ... n : nhập chuyển kho. Không nhập từ tủ trực

//phần 3 là xuất của kho lẻ được chọn
//3.1: xuất quyết toán
//bằng xuất kho lẻ - hoàn kho lẻ + tồn đầu các tủ trực - tồn cuối các tủ trự.
//+ xuất: là xuất của báo cáo tkb30 ( xuất bù, bổ sung,đơn nội trú, hao phí)
//+ hoàn: hoàn thì lấy theo thằng 153 ( đơn tủ trực trả lại, đơn điều trị trả lại, phiếu tổng hợp trả lại)
//3.2: xuất khác lấy theo thằng 362
//3.3 ... n : xuất chuyển kho tkb23. Không xuất cho tủ trực

//phần 4: tồn cuỗi kỳ
//4.1 là tồn kho của kho được chọn 
//4.2 tồn kho của tủ trực nếu ko chọn tủ trực thì để trống