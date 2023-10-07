using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.Proccessor.Mrs00105;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisBid;
using MOS.MANAGER.HisAnticipateMety;
using MOS.MANAGER.HisAnticipateMaty;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisExpMestReason;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisBloodType;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisBidBloodType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisExpMestBlood;
using MOS.MANAGER.HisImpMestBlood;
using MOS.DAO.Sql;

namespace MRS.Processor.Mrs00105
{
    public class Mrs00105Processor : AbstractProcessor
    {

        const long THUOC = 1;
        const long VATTU = 2;
        const long HOACHAT = 3;
        const long MAU = 4;
        private Dictionary<string, Mrs00105RDO> dicRdo = new Dictionary<string, Mrs00105RDO>();
        private List<Mrs00105RDO> ListRdo = new List<Mrs00105RDO>();
        private List<Mrs00105RDO> ListRdoA = new List<Mrs00105RDO>();
        private List<Mrs00105RDO> ListRdoB = new List<Mrs00105RDO>();
        Mrs00105Filter castFilter = new Mrs00105Filter();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_BLOOD_TYPE> dicBloodType = new Dictionary<long, V_HIS_BLOOD_TYPE>();
        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();
        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<V_HIS_MEDI_STOCK> ListExactMediStock = new List<V_HIS_MEDI_STOCK>();

        List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
        List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
        List<HIS_BLOOD> Bloods = new List<HIS_BLOOD>();
        //Cac list dau ki
        List<Mrs00105RDO> listMestPeriodMedi = new List<Mrs00105RDO>(); // DS thuoc chot ki
        List<Mrs00105RDO> listMestPeriodMate = new List<Mrs00105RDO>(); // DS vat tu chot ki
        List<Mrs00105RDO> listMestPeriodBlood = new List<Mrs00105RDO>(); // DS mau chot ki

        List<Mrs00105RDO> listImpMestMedicine = new List<Mrs00105RDO>(); // DS thuoc nhap tu sau chot ky den timeto
        List<Mrs00105RDO> listImpMestBlood = new List<Mrs00105RDO>(); // DS mau nhap tu sau chot ky den timeto
        List<Mrs00105RDO> listImpMestMaterial = new List<Mrs00105RDO>(); // DS vat tu nhap tu sau chot ky den timeto

        List<Mrs00105RDO> listExpMestMedicine = new List<Mrs00105RDO>(); // DS thuoc xuat tu sau chot ky den timeto
        List<Mrs00105RDO> listExpMestMaterial = new List<Mrs00105RDO>(); // DS vat tu xuat tu sau chot ky den timeto
        List<Mrs00105RDO> listExpMestBlood = new List<Mrs00105RDO>(); // DS Mau xuat tu sau chot ky den timeto

        List<DETAIL> ListDetail = new List<DETAIL>(); // Chi tiet

        List<V_HIS_IMP_MEST_MEDICINE> InputMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); // DS thuoc dau vao kiem ke

        List<V_HIS_IMP_MEST_MATERIAL> InputMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); // DS vat tu dau vao kiem ke

        List<MediMateIdChmsMediStockId> MediMateIdChmsMediStockIds = new List<MediMateIdChmsMediStockId>(); //DS kho cha da chuyen bo sung va bu cho tu truc

        //DS du tru thuoc vat tu
        List<AnticipateMety> AnticipateMetys = new List<AnticipateMety>();
        List<AnticipateMaty> AnticipateMatys = new List<AnticipateMaty>();
        //DS nha cung cap
        List<HIS_SUPPLIER> Suppliers = new List<HIS_SUPPLIER>();
        //DS nguồn nhập
        List<HIS_IMP_SOURCE> ImpSources = new List<HIS_IMP_SOURCE>();
        //li do xuat khac
        List<HIS_EXP_MEST_REASON> ExpMestReasons = new List<HIS_EXP_MEST_REASON>();
        List<HIS_EXP_MEST> listExpMestOn = new List<HIS_EXP_MEST>();
        List<HIS_DEPARTMENT> listExpReqDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_IMP_MEST> listImpMestOn = new List<HIS_IMP_MEST>(); //Phieu nhap
        List<HIS_BID> listBid = new List<HIS_BID>(); //danh sach goi thau
        List<HIS_BID_MEDICINE_TYPE> ListBidMety = new List<HIS_BID_MEDICINE_TYPE>(); //danh sach thuoc trong thau
        List<HIS_BID_MATERIAL_TYPE> ListBidMaty = new List<HIS_BID_MATERIAL_TYPE>(); //danh sach vat tu trong thau
        List<HIS_BID_BLOOD_TYPE> ListBidBlty = new List<HIS_BID_BLOOD_TYPE>(); //danh sach mau trong thau
        List<HIS_MEDICINE_PATY> MedicinePatys = new List<HIS_MEDICINE_PATY>();
        List<HIS_MATERIAL_PATY> MaterialPatys = new List<HIS_MATERIAL_PATY>();
        List<HIS_SALE_PROFIT_CFG> SaleProfitCfgs = new List<HIS_SALE_PROFIT_CFG>();
        List<HIS_BLOOD_ABO> listBloodAbo = new List<HIS_BLOOD_ABO>();

        Dictionary<string, BID_IMP> dicBidImpMedi = new Dictionary<string, BID_IMP>(); //danh sach thông tin nhập từ thầu
        Dictionary<string, BID_IMP> dicBidImpMate = new Dictionary<string, BID_IMP>(); //danh sach thông tin nhập từ thầu
        Dictionary<string, BID_IMP> dicBidImpBlood = new Dictionary<string, BID_IMP>(); //danh sach thông tin nhập từ thầu

        List<string> CodeSourceChms = new List<string>();
        List<string> CodeDestChms = new List<string>();
        List<string> CodeDestDps = new List<string>();
        List<string> CodeSourceDps = new List<string>();

        //Lấy các chốt kỳ ở cuối kỳ để kiểm tra chốt kỳ
        List<Mrs00105RDO> listMestPeriodMediEnd = new List<Mrs00105RDO>(); // DS thuoc chot ki
        List<Mrs00105RDO> listMestPeriodMateEnd = new List<Mrs00105RDO>(); // DS vat tu chot ki
        List<Mrs00105RDO> listMestPeriodBloodEnd = new List<Mrs00105RDO>(); // DS mau chot ki

        //danh sách  id loai thuốc theo kho cha
        List<MediStockMetyMaty> listMediStockMety = new List<MediStockMetyMaty>();

        //danh sách  id loai vật tư theo kho cha
        List<MediStockMetyMaty> listMediStockMaty = new List<MediStockMetyMaty>();

        //thông tin định mức hao phí
        List<ServiceMetyMaty> listServMetyMaty = new List<ServiceMetyMaty>();

        Dictionary<long, IMP_MEST> dicMedicineDocumentInfo = new Dictionary<long, IMP_MEST>();
        Dictionary<long, IMP_MEST> dicMaterialDocumentInfo = new Dictionary<long, IMP_MEST>();

        Dictionary<string, decimal> dicDiffDocumentPrice = new Dictionary<string, decimal>();

        List<MATERIAL_REUSABLING> listIsReusabling = new List<MATERIAL_REUSABLING>();

        List<V_HIS_MEDI_STOCK> listMediStockHasData = new List<V_HIS_MEDI_STOCK>();

        string KeyGroupBidDetail = "{0}_{1}_{2}_{3}";

        public Mrs00105Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00105Filter);
        }

        protected override bool GetData()
        {

            var result = true;
            castFilter = (Mrs00105Filter)this.reportFilter;
            MapDataFilterToFilter();
            if (castFilter.IS_MEDICINE != true && castFilter.IS_MATERIAL != true && castFilter.IS_CHEMICAL_SUBSTANCE != true && castFilter.IS_BLOOD != true)
            {
                castFilter.IS_MEDICINE = true;
                castFilter.IS_MATERIAL = true;
                castFilter.IS_CHEMICAL_SUBSTANCE = true;
                castFilter.IS_BLOOD = true;
            }
            try
            {

                ListMediStock = HisMediStockCFG.HisMediStocks;

                //kiem tra loc theo thang
                checkFilterMonth();
                //kiem tra loc theo khoa
                checkFilterDepartment();

                //kiem tra loc theo cơ sở
                checkFilterBranchs();

                //kiem tra loc theo nhieu khoa
                checkFilterDepartments();

                //kiem tra loc chi lay tu truc
                checkFilterCabinet();

                //kiem tra loc thoi gian tao thau
                checkFilterBidCreateTime();

                //Tao loai nhap xuat
                makeRdo();

                ///Danh sách kho
                GetMediStock();

                //Neu ket qua loc khong co kho phu hop thi bao loi ve may tram
                if (ListMediStock.Count == 0)
                {
                    return true;
                }
                //Loai thuoc, vat tu, mau
                GetMedicineTypeMaterialTypeBloodType();

                //---------------------------------

                //lay thong tin cac kho cha da tung nhap thuoc vat tu cho tu truc
                GetChmsMediStockForImpMest();

                //lay thong tin kho cha
                GetMediStockMetyMaty();

                //Danh sách kho được chuyển
                GetImpMediStockOnTime();

                //Danh sách kho chuyển
                GetChmsMediStockOnTime();

                //Du lieu ton thuoc, vat tu, mau
                GetMediMateBloodPeriod();

                //Xuat thuoc, vat tu, mau
                GetExpMestMediMateBlood();

                //Nhap thuoc, vat tu, mau
                GetImpMestMediMateBlood();

                //Lay thong tin medicine, material, blood cua cac thuoc vat tu mau
                GetMediMateBlood();

                //Lay thong tin goi thau
                GetBid();

                //Lay cac thuoc vat tu dau vao kiem ke
                GetInputEndAmount();



                ///Danh sách khoa phòng yêu cầu xuất
                GetExpReqDepartment();

                ///Danh sách phiếu xuất trong kỳ
                GetExpMest();

                ///Danh sách đang dự trù
                GetAnticipate();

                ///Danh sách nhà cung cấp
                GetSupplier();

                ///Danh sách lí do xuất khác
                GetExpMestReason();

                ///Danh sách nguồn nhập
                GetImpSource();

                ///Danh sách chinh sach gia
                GetMediMatePaty();

                //Danh sách cấu hình giá theo giá nhập HIS_SALE_PROFIT_CFG
                GetSaleProfitCfg();

                ///Danh sách so luong thuoc vat tu trong thau
                GetBidMetyBidMatyAmount();

                //dịch vụ định mức
                GetServiceMetyMaty();

                //nhóm máu
                GetBloodAbo();

                //thông tin chứng từ
                GetDocumentInfo();

                //thông tin gộp chi tiết thầu
                GetGroupBidDetailInfo();

                //thông tin nhập từ thầu
                GetBidImpInfo();

                //thông tin lệch tiền hóa đơn
                GetDiffDocumentPrice();

                //thông tin tái sử dụng
                GetIsReusabling();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetGroupBidDetailInfo()
        {
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_BID_DETAIL") && this.dicDataFilter["KEY_GROUP_BID_DETAIL"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_BID_DETAIL"].ToString()))
            {
                this.KeyGroupBidDetail = this.dicDataFilter["KEY_GROUP_BID_DETAIL"].ToString();
            }
        }

        private void GetIsReusabling()
        {
            string sql = @"select
MATERIAL_ID,
(case when min(imp_time)>{0} then 2 else 1 end) IS_REUSABLING
from v_his_imp_mest_material 
where 1=1
and imp_mest_stt_id=5
and imp_mest_type_id in (17)
and imp_time <{1}
and material_type_id in (select id from his_material_type where is_reusable=1)
group by
material_id";
            CommonParam paramGet = new CommonParam();
            listIsReusabling = new SqlDAO().GetSql<MATERIAL_REUSABLING>(paramGet, string.Format(sql, castFilter.TIME_FROM, castFilter.TIME_TO));
        }

        private void GetImpMediStockOnTime()
        {
            castFilter.MEDI_STOCK_CODE__IMPs = string.Join(",", (new ManagerSql().GetMediStockCodeImps(ListMediStock.Select(o => o.ID).ToList(), castFilter) ?? new List<MEDI_STOCK_CODE_IMPs>()).Select(o => o.MEDI_STOCK_CODE).ToList());
        }

        private void GetChmsMediStockOnTime()
        {
            castFilter.MEDI_STOCK_CODE__CHMSs = string.Join(",", (new ManagerSql().GetMediStockCodeChmss(ListMediStock.Select(o => o.ID).ToList(), castFilter) ?? new List<MEDI_STOCK_CODE_IMPs>()).Select(o => o.MEDI_STOCK_CODE).ToList());
        }

        private void GetBidImpInfo()
        {
            if (castFilter.IS_ADD_BID_IMP_INFO == true)
            {
                var listBidImpMedi = new ManagerSql().GetBidImpMediInfo(ListMediStock.Select(o => o.ID).ToList(), castFilter, this.KeyGroupBidDetail) ?? new List<BID_IMP>();
                dicBidImpMedi = listBidImpMedi.GroupBy(o => o.KEY_BID_IMP).ToDictionary(p => p.Key, q => new BID_IMP() { KEY_BID_IMP = q.First().KEY_BID_IMP, BID_AMOUNT = q.First().BID_AMOUNT, BID_ADJUST_AMOUNT = q.First().BID_ADJUST_AMOUNT, BID_IMP_AMOUNT = q.Sum(s => s.BID_IMP_AMOUNT) });
                var listBidImpMate = new ManagerSql().GetBidImpMateInfo(ListMediStock.Select(o => o.ID).ToList(), castFilter, this.KeyGroupBidDetail) ?? new List<BID_IMP>();
                dicBidImpMate = listBidImpMate.GroupBy(o => o.KEY_BID_IMP).ToDictionary(p => p.Key, q => new BID_IMP() { KEY_BID_IMP = q.First().KEY_BID_IMP, BID_AMOUNT = q.First().BID_AMOUNT, BID_ADJUST_AMOUNT = q.First().BID_ADJUST_AMOUNT, BID_IMP_AMOUNT = q.Sum(s => s.BID_IMP_AMOUNT) });
                var listBidImpBlood = new ManagerSql().GetBidImpBloodInfo(ListMediStock.Select(o => o.ID).ToList(), castFilter, this.KeyGroupBidDetail) ?? new List<BID_IMP>();
                dicBidImpBlood = listBidImpBlood.GroupBy(o => o.KEY_BID_IMP).ToDictionary(p => p.Key, q => new BID_IMP() { KEY_BID_IMP = q.First().KEY_BID_IMP, BID_AMOUNT = q.First().BID_AMOUNT, BID_ADJUST_AMOUNT = q.First().BID_ADJUST_AMOUNT, BID_IMP_AMOUNT = q.Sum(s => s.BID_IMP_AMOUNT) });
            }
        }

        private void GetDocumentInfo()
        {
            var documentInfo = new ManagerSql().GetDocumentInfo() ?? new List<IMP_MEST>();
            if (documentInfo != null)
            {
                dicMedicineDocumentInfo = Medicines.ToDictionary(o => o.ID, p => documentInfo.Where(o => o.IMP_MEST_CODE == p.TDL_IMP_MEST_CODE).FirstOrDefault() ?? new IMP_MEST());
                dicMaterialDocumentInfo = Materials.ToDictionary(o => o.ID, p => documentInfo.Where(o => o.IMP_MEST_CODE == p.TDL_IMP_MEST_CODE).FirstOrDefault() ?? new IMP_MEST());
                //var bloodDocumentInfo = Bloods.ToDictionary(o => o.ID, p => documentInfo.Where(o => o.IMP_MEST_CODE == p.TDL_IMP_MEST_CODE).FirstOrDefault() ?? new HIS_IMP_MEST());
            }
        }

        private void GetDiffDocumentPrice()
        {
            var DiffDocumentPrice = new ManagerSql().GetDiffDocumentPrice(ListMediStock.Select(o => o.ID).ToList(), castFilter) ?? new List<Mrs00105RDO>();
            if (DiffDocumentPrice != null)
            {
                dicDiffDocumentPrice = DiffDocumentPrice.GroupBy(o => string.Format("{0}_{1}_{2}", o.MEDI_MATE_ID, o.MEDI_STOCK_ID, o.TYPE)).ToDictionary(p => p.Key, q => q.Sum(s => s.DIFF_DOCUMENT_TOTAL_PRICE));
            }
        }

        private void MapDataFilterToFilter()
        {
            if (this.dicDataFilter.ContainsKey("IS_BLOOD") && this.dicDataFilter["IS_BLOOD"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["IS_BLOOD"].ToString()))
            {
                if (this.dicDataFilter["IS_BLOOD"].ToString().ToLower() == "true")
                {
                    castFilter.IS_BLOOD = true;
                }
                if (this.dicDataFilter["IS_BLOOD"].ToString().ToLower() == "false")
                {
                    castFilter.IS_BLOOD = false;
                }
            }
            if (this.dicDataFilter.ContainsKey("PARENT_SERVICE_CODEs") && this.dicDataFilter["PARENT_SERVICE_CODEs"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["PARENT_SERVICE_CODEs"].ToString()))
            {
                var services = new HisServiceManager().Get(new HisServiceFilterQuery());
                if (services != null && services.Count > 0)
                {
                    castFilter.EXACT_PARENT_SERVICE_IDs = services.Where(o => string.Format(",{0},", this.dicDataFilter["PARENT_SERVICE_CODEs"].ToString()).Contains(string.Format(",{0},", o.SERVICE_CODE))).Select(p => p.ID).ToList();
                }
            }
        }

        private void GetServiceMetyMaty()
        {
            HisServiceMetyViewFilterQuery serviceMetyFilter = new HisServiceMetyViewFilterQuery();
            //serviceMetyFilter.SERVICE_TYPE_IDs = castFilter.QUOTA_SERVICE_TYPE_IDs;
            List<V_HIS_SERVICE_METY> listServiceMety = new HisServiceMetyManager().GetView(serviceMetyFilter);
            if (listServiceMety != null)
            {
                foreach (var item in listServiceMety)
                {
                    if (castFilter.QUOTA_SERVICE_TYPE_IDs != null && !castFilter.QUOTA_SERVICE_TYPE_IDs.Contains(item.SERVICE_TYPE_ID))
                        continue;
                    ServiceMetyMaty rdo = new ServiceMetyMaty();
                    rdo.TYPE = THUOC;
                    rdo.MEDI_MATE_TYPE_ID = item.MEDICINE_TYPE_ID;
                    rdo.QUOTA_AMOUNT = item.EXPEND_AMOUNT;
                    rdo.SERVICE_NAME = item.SERVICE_NAME;
                    listServMetyMaty.Add(rdo);
                }
            }

            HisServiceMatyViewFilterQuery serviceMatyFilter = new HisServiceMatyViewFilterQuery();
            //serviceMatyFilter.SERVICE_TYPE_IDs = castFilter.QUOTA_SERVICE_TYPE_IDs;
            List<V_HIS_SERVICE_MATY> listServiceMaty = new HisServiceMatyManager().GetView(serviceMatyFilter);
            if (listServiceMaty != null)
            {
                foreach (var item in listServiceMaty)
                {
                    if (castFilter.QUOTA_SERVICE_TYPE_IDs != null && !castFilter.QUOTA_SERVICE_TYPE_IDs.Contains(item.SERVICE_TYPE_ID))
                        continue;
                    ServiceMetyMaty rdo = new ServiceMetyMaty();
                    rdo.TYPE = VATTU;
                    rdo.MEDI_MATE_TYPE_ID = item.MATERIAL_TYPE_ID;
                    rdo.QUOTA_AMOUNT = item.EXPEND_AMOUNT;
                    rdo.SERVICE_NAME = item.SERVICE_NAME;
                    listServMetyMaty.Add(rdo);
                }
            }
        }


        private void GetBloodAbo()
        {
            this.listBloodAbo = new ManagerSql().GetBloodAbo() ?? new List<HIS_BLOOD_ABO>();
        }


        private void GetSaleProfitCfg()
        {
            SaleProfitCfgs = new ManagerSql().GetSaleProfitCfg();
        }

        private void GetMediMatePaty()
        {

            HisMedicinePatyFilterQuery MedicinePatyfilter = new HisMedicinePatyFilterQuery();
            MedicinePatys = new HisMedicinePatyManager().Get(MedicinePatyfilter);
            HisMaterialPatyFilterQuery MaterialPatyfilter = new HisMaterialPatyFilterQuery();
            MaterialPatys = new HisMaterialPatyManager().Get(MaterialPatyfilter);
        }

        private void GetBidMetyBidMatyAmount()
        {
            try
            {
                HisBidMedicineTypeFilterQuery BidMedicineTypefilter = new HisBidMedicineTypeFilterQuery();
                BidMedicineTypefilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                BidMedicineTypefilter.BID_IDs = castFilter.BID_IDs;
                BidMedicineTypefilter.BID_ID = castFilter.BID_ID;
                var bidMedicineTypes = new HisBidMedicineTypeManager().Get(BidMedicineTypefilter);
                if (bidMedicineTypes != null && bidMedicineTypes.Count > 0)
                {
                    ListBidMety = bidMedicineTypes;
                }

                HisBidMaterialTypeFilterQuery BidMaterialTypefilter = new HisBidMaterialTypeFilterQuery();
                BidMaterialTypefilter.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                BidMaterialTypefilter.BID_IDs = castFilter.BID_IDs;
                BidMaterialTypefilter.BID_ID = castFilter.BID_ID;
                var bidMaterialTypes = new HisBidMaterialTypeManager().Get(BidMaterialTypefilter);
                if (bidMaterialTypes != null && bidMaterialTypes.Count > 0)
                {
                    ListBidMaty = bidMaterialTypes;
                }

                HisBidBloodTypeFilterQuery BidBloodTypefilter = new HisBidBloodTypeFilterQuery();
                BidBloodTypefilter.BLOOD_TYPE_IDs = castFilter.BLOOD_TYPE_IDs;
                BidBloodTypefilter.BID_IDs = castFilter.BID_IDs;
                BidBloodTypefilter.BID_ID = castFilter.BID_ID;
                var bidBloodTypes = new HisBidBloodTypeManager().Get(BidBloodTypefilter);
                if (bidBloodTypes != null && bidBloodTypes.Count > 0)
                {
                    ListBidBlty = bidBloodTypes;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void GetBid()
        {
            HisBidFilterQuery bidFilter = new HisBidFilterQuery();
            this.listBid = new HisBidManager().Get(bidFilter);
        }

        private void checkFilterMonth()
        {
            try
            {
                if (castFilter.MONTH != null)
                {
                    castFilter.TIME_FROM = Inventec.Common.DateTime.Get.StartMonth(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.MONTH ?? 0) ?? new DateTime());
                    castFilter.TIME_TO = Inventec.Common.DateTime.Get.EndMonth(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.MONTH ?? 0) ?? new DateTime());
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void GetExpMest()
        {
            if (castFilter.TAKE_IMP_EXP_MEST == true)
            {
                CommonParam paramGet = new CommonParam();
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.MEDI_STOCK_IDs = ListMediStock.Select(o => o.ID).ToList();
                expMestFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expMestFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                var listExpMest = new HisExpMestManager(paramGet).Get(expMestFilter) ?? new List<HIS_EXP_MEST>();
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
                listExpMestOn.AddRange(listExpMest);
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listExpMestOn = listExpMestOn.Where(p => castFilter.TREATMENT_TYPE_IDs.Contains(1) && p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK || (castFilter.TREATMENT_TYPE_IDs.Contains(3) || castFilter.TREATMENT_TYPE_IDs.Contains(4) || castFilter.TREATMENT_TYPE_IDs.Contains(2)) && p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT).ToList();
                }
                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.MEDI_STOCK_IDs = ListMediStock.Select(o => o.ID).ToList();
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listImpMest = new HisImpMestManager(paramGet).Get(impMestFilter) ?? new List<HIS_IMP_MEST>();
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00105");
                listImpMestOn.AddRange(listImpMest);
            }

        }

        private void GetExpReqDepartment()
        {
            try
            {
                listExpReqDepartment = HisDepartmentCFG.DEPARTMENTs.Where(o => listExpMestMedicine.Exists(p => p.EXP_TOTAL_AMOUNT > 0 && p.REQ_DEPARTMENT_ID == o.ID) || listExpMestMaterial.Exists(p => p.EXP_TOTAL_AMOUNT > 0 && p.REQ_DEPARTMENT_ID == o.ID)).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetAnticipate()
        {
            try
            {
                if (castFilter.TAKE_ANTICIPATE_INFO == true)
                {
                    List<long> mediStockIds = ListMediStock.Select(o => o.ID).ToList();

                    if (mediStockIds != null && mediStockIds.Count > 0)
                    {
                        var AnticipateMetySub = new ManagerSql().GetAnticipateMety(this.castFilter, mediStockIds);
                        if (AnticipateMetySub != null)
                        {
                            AnticipateMetys.AddRange(AnticipateMetySub);
                        }
                        var AnticipateMatySub = new ManagerSql().GetAnticipateMaty(this.castFilter, mediStockIds);
                        if (AnticipateMatySub != null)
                        {
                            AnticipateMatys.AddRange(AnticipateMatySub);
                        }
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

        private void GetExpMestReason()
        {
            try
            {
                HisExpMestReasonFilterQuery ExpMestReasonfilter = new HisExpMestReasonFilterQuery();
                ExpMestReasons = new HisExpMestReasonManager().Get(ExpMestReasonfilter);
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

        private void checkFilterDepartments()
        {
            try
            {
                if (this.castFilter.DEPARTMENT_IDs != null)
                {
                    ListMediStock = ListMediStock.Where(o => this.castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void checkFilterBranchs()
        {
            try
            {
                if (this.castFilter.BRANCH_ID != null)
                {
                    var departmentIds = (new HisDepartmentManager().Get(new HisDepartmentFilterQuery() { BRANCH_ID = this.castFilter.BRANCH_ID }) ?? new List<HIS_DEPARTMENT>()).Select(o => o.ID).ToList();

                    ListMediStock = ListMediStock.Where(o => departmentIds.Contains(o.DEPARTMENT_ID)).ToList();
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
                //if (castFilter.PREVIOUS_MEDI_STOCK_IDs != null)
                //{
                MediMateIdChmsMediStockIds = new ManagerSql().GetChmsMediStockId(ListMediStock.Select(o => o.ID).ToList(), null, castFilter.TIME_FROM ?? 0, castFilter.TIME_TO ?? 0, castFilter.MEDICINE_TYPE_CODEs, castFilter.MATERIAL_TYPE_CODEs, castFilter.BLOOD_TYPE_CODEs, castFilter);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GetMediStockMetyMaty()
        {
            try
            {
                if (castFilter.EXP_MEDI_STOCK_IDs != null)
                {
                    listMediStockMety = new ManagerSql().GetMediStockMety(castFilter, ListMediStock.Select(o => o.ID).ToList()) ?? new List<MediStockMetyMaty>();
                    listMediStockMaty = new ManagerSql().GetMediStockMaty(castFilter, ListMediStock.Select(o => o.ID).ToList()) ?? new List<MediStockMetyMaty>();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void checkFilterDepartment()
        {
            try
            {
                if (this.castFilter.DEPARTMENT_ID != null)
                {
                    ListMediStock = ListMediStock.Where(o => o.DEPARTMENT_ID == this.castFilter.DEPARTMENT_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }
        private void checkFilterCabinet()
        {
            try
            {
                if (this.castFilter.IS_CABINET == true)
                {
                    ListMediStock = ListMediStock.Where(o => o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void checkFilterBidCreateTime()
        {
            try
            {
                if (this.castFilter.BID_CREATE_TIME_FROM > 0 || this.castFilter.BID_CREATE_TIME_TO > 0)
                {
                    HisBidFilterQuery bidFilter = new HisBidFilterQuery();
                    bidFilter.CREATE_TIME_FROM = this.castFilter.BID_CREATE_TIME_FROM;
                    bidFilter.CREATE_TIME_TO = this.castFilter.BID_CREATE_TIME_TO;
                    var bids = new HisBidManager().Get(bidFilter) ?? new List<HIS_BID>();
                    this.castFilter.BID_IDs = bids.Select(o => o.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetInputEndAmount()
        {
            try
            {
                if (castFilter.TAKE_INPUT_END_AMOUNT == true)
                {
                    InputMedicines = new ManagerSql().GetMediInput(this.castFilter, ListMediStock.Select(o => o.ID).ToList()) ?? new List<V_HIS_IMP_MEST_MEDICINE>();
                    InputMaterials = new ManagerSql().GetMateInput(this.castFilter, ListMediStock.Select(o => o.ID).ToList()) ?? new List<V_HIS_IMP_MEST_MATERIAL>();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetMediMateBlood()
        {
            try
            {
                List<long> medicineIds = new List<long>();
                if (listMestPeriodMedi != null)
                {
                    medicineIds.AddRange(listMestPeriodMedi.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listMestPeriodMediEnd != null)
                {
                    medicineIds.AddRange(listMestPeriodMediEnd.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listImpMestMedicine != null)
                {
                    medicineIds.AddRange(listImpMestMedicine.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listExpMestMedicine != null)
                {
                    medicineIds.AddRange(listExpMestMedicine.Select(o => o.MEDI_MATE_ID).ToList());
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
                if (listMestPeriodMateEnd != null)
                {
                    materialIds.AddRange(listMestPeriodMateEnd.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listMestPeriodMate != null)
                {
                    materialIds.AddRange(listMestPeriodMate.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listImpMestMaterial != null)
                {
                    materialIds.AddRange(listImpMestMaterial.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listExpMestMaterial != null)
                {
                    materialIds.AddRange(listExpMestMaterial.Select(o => o.MEDI_MATE_ID).ToList());
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

                List<long> bloodIds = new List<long>();
                if (listMestPeriodBloodEnd != null)
                {
                    bloodIds.AddRange(listMestPeriodBloodEnd.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listMestPeriodBlood != null)
                {
                    bloodIds.AddRange(listMestPeriodBlood.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listImpMestBlood != null)
                {
                    bloodIds.AddRange(listImpMestBlood.Select(o => o.MEDI_MATE_ID).ToList());
                }
                if (listExpMestBlood != null)
                {
                    bloodIds.AddRange(listExpMestBlood.Select(o => o.MEDI_MATE_ID).ToList());
                }

                bloodIds = bloodIds.Distinct().ToList();

                if (bloodIds != null && bloodIds.Count > 0)
                {
                    var skip = 0;
                    while (bloodIds.Count - skip > 0)
                    {
                        var limit = bloodIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisBloodFilterQuery Bloodfilter = new HisBloodFilterQuery();
                        Bloodfilter.IDs = limit;
                        var BloodSub = new HisBloodManager().Get(Bloodfilter);
                        Bloods.AddRange(BloodSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void makeRdo()
        {
            //Danh sach loai nhap, loai xuat

            Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
            Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00105RDO>();

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

        private void GetMediMateBloodPeriod()
        {
            if (castFilter.IS_MEDICINE == true)
            {
                listMestPeriodMedi = new ManagerSql().GetMediPeriod(ListMediStock.Select(o => o.ID).ToList(), castFilter.TIME_FROM ?? 0, castFilter.MEDICINE_TYPE_CODEs, castFilter);
                listMestPeriodMediEnd = new ManagerSql().GetMediPeriod(ListMediStock.Select(o => o.ID).ToList(), (castFilter.TIME_TO ?? 0) + 1, castFilter.MEDICINE_TYPE_CODEs, castFilter);
            }

            if (castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true)
            {
                listMestPeriodMate = new ManagerSql().GetMatePeriod(ListMediStock.Select(o => o.ID).ToList(), castFilter.TIME_FROM ?? 0, castFilter.MATERIAL_TYPE_CODEs, castFilter);
                listMestPeriodMateEnd = new ManagerSql().GetMatePeriod(ListMediStock.Select(o => o.ID).ToList(), (castFilter.TIME_TO ?? 0) + 1, castFilter.MATERIAL_TYPE_CODEs, castFilter);
            }

            if (castFilter.IS_BLOOD == true)
            {
                listMestPeriodBlood = new ManagerSql().GetBloodPeriod(ListMediStock.Select(o => o.ID).ToList(), castFilter.TIME_FROM ?? 0, castFilter.BLOOD_TYPE_CODEs, castFilter);
                listMestPeriodBloodEnd = new ManagerSql().GetBloodPeriod(ListMediStock.Select(o => o.ID).ToList(), (castFilter.TIME_TO ?? 0) + 1, castFilter.BLOOD_TYPE_CODEs, castFilter);
            }
        }

        private void GetExpMestMediMateBlood()
        {
            if (castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true)
            {
                listExpMestMaterial = new ManagerSql().GetMateExp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }

            if (castFilter.IS_MEDICINE == true)
            {
                listExpMestMedicine = new ManagerSql().GetMediExp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }

            if (castFilter.IS_BLOOD == true)
            {
                listExpMestBlood = new ManagerSql().GetBloodExp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }
        }

        private void GetImpMestMediMateBlood()
        {
            if (castFilter.IS_MATERIAL == true || castFilter.IS_CHEMICAL_SUBSTANCE == true)
            {
                listImpMestMaterial = new ManagerSql().GetMateImp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }

            if (castFilter.IS_MEDICINE == true)
            {
                listImpMestMedicine = new ManagerSql().GetMediImp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }

            if (castFilter.IS_BLOOD == true)
            {
                listImpMestBlood = new ManagerSql().GetBloodImp(ListMediStock.Select(o => o.ID).ToList(), castFilter);
            }

        }

        private void GetMedicineTypeMaterialTypeBloodType()
        {
            CommonParam paramGet = new CommonParam();
            var ListMedicineType = castFilter.IS_MEDICINE != null ? new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery()) : new List<V_HIS_MEDICINE_TYPE>();
            var ListMaterialType = castFilter.IS_MATERIAL != null || castFilter.IS_CHEMICAL_SUBSTANCE != null ? new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery()) : new List<V_HIS_MATERIAL_TYPE>();
            var ListBloodType = castFilter.IS_BLOOD != null ? new HisBloodTypeManager(paramGet).GetView(new HisBloodTypeViewFilterQuery()) : new List<V_HIS_BLOOD_TYPE>();
            if (IsNotNullOrEmpty(ListMedicineType))
            {
                foreach (var item in ListMedicineType)
                {
                    if (!string.IsNullOrWhiteSpace(this.castFilter.MEDICINE_TYPE_CODEs) && !string.Format(",{0},", this.castFilter.MEDICINE_TYPE_CODEs).Contains(string.Format(",{0},", item.MEDICINE_TYPE_CODE)))
                    {
                        continue;
                    }
                    dicMedicineType[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListMaterialType))
            {
                foreach (var item in ListMaterialType)
                {
                    if (!string.IsNullOrWhiteSpace(this.castFilter.MATERIAL_TYPE_CODEs) && !string.Format(",{0},", this.castFilter.MATERIAL_TYPE_CODEs).Contains(string.Format(",{0},", item.MATERIAL_TYPE_CODE)))
                    {
                        continue;
                    }
                    dicMaterialType[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListBloodType))
            {
                foreach (var item in ListBloodType)
                {
                    if (!string.IsNullOrWhiteSpace(this.castFilter.BLOOD_TYPE_CODEs) && !string.Format(",{0},", this.castFilter.BLOOD_TYPE_CODEs).Contains(string.Format(",{0},", item.BLOOD_TYPE_CODE)))
                    {
                        continue;
                    }
                    dicBloodType[item.ID] = item;
                }
            }

        }

        private void GetMediStock()
        {
            if (this.castFilter.MEDI_STOCK_IDs != null)
            {
                ListMediStock = ListMediStock.Where(o => this.castFilter.MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                //kiem tra loc theo cơ sở
                checkFilterBranchs();
            }
            if (this.castFilter.MEDI_STOCK_ID != null)
            {
                ListMediStock = ListMediStock.Where(o => this.castFilter.MEDI_STOCK_ID == o.ID).ToList();
                //kiem tra loc theo cơ sở
                checkFilterBranchs();
            }
            if (this.castFilter.MEDI_STOCK_CABINET_IDs != null)
            {
                ListMediStock = ListMediStock.Where(o => this.castFilter.MEDI_STOCK_CABINET_IDs.Contains(o.ID)).ToList();
                //kiem tra loc theo cơ sở
                checkFilterBranchs();
            }
            if (this.castFilter.MEDI_STOCK_CABINET_ID != null)
            {
                ListMediStock = ListMediStock.Where(o => this.castFilter.MEDI_STOCK_CABINET_ID == o.ID).ToList();
                //kiem tra loc theo cơ sở
                checkFilterBranchs();
            }
            if (!string.IsNullOrWhiteSpace(this.castFilter.MEDI_STOCK_STR_CODEs))
            {
                List<string> mss = this.castFilter.MEDI_STOCK_STR_CODEs.Split(',').ToList();
                ListMediStock = ListMediStock.Where(o => mss.Contains(o.MEDI_STOCK_CODE ?? "")).ToList();
                //kiem tra loc theo cơ sở
                checkFilterBranchs();
            }

        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                dicRdo.Clear();
                //Xử lý lọc thuốc vật tư theo kho cha
                if (castFilter.EXP_MEDI_STOCK_IDs != null)
                {
                    FilterExpMediStock();
                }
                //Xử lý lọc thuốc vật tư theo thiết lập định mức
                if (castFilter.QUOTA_SERVICE_TYPE_IDs != null)
                {
                    FilterQuota();
                }

                // gan du lieu chot ky thuoc vao so luong ton dau cua cac lo thuoc
                foreach (var item in listMestPeriodMedi)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var medicineType = dicMedicineType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMedicineType[item.MEDI_MATE_TYPE_ID] : null;
                        var requestDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID);
                        if (!checkServiceType(medicineType))
                            continue;

                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoMediToRdo(item, medicineType, medicine, mediStock, Suppliers);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                }
                listMestPeriodMedi.Clear();
                listMestPeriodMedi = null;
                // gan du lieu chot ky mau vao so luong ton dau cua cac lo mau
                foreach (var item in listMestPeriodBlood)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var blood = Bloods.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var bloodType = dicBloodType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicBloodType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(bloodType))
                            continue;

                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoBloodToRdo(item, bloodType, blood, mediStock, Suppliers);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                }
                listMestPeriodBlood.Clear();
                listMestPeriodBlood = null;
                // gan du lieu chot ky vat tu vao so luong ton dau cua cac lo vat tu
                foreach (var item in listMestPeriodMate)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var materialType = dicMaterialType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMaterialType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(materialType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoMateToRdo(item, materialType, material, mediStock, Suppliers);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listMestPeriodMate.Clear();
                listMestPeriodMate = null;
                // gan du lieu xuat vat tu vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo vat tu
                foreach (var item in listExpMestMaterial)
                {
                    if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        item.PATIENT_TYPE_NAME = "BHYT";
                    }
                    else
                    {
                        item.PATIENT_TYPE_NAME = "VP";
                    }
                    item.LAST_EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.LAST_EXP_TIME_NUM ?? 0);
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var materialType = dicMaterialType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMaterialType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(materialType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        //them thong tin lo
                        InfoMateToRdo(item, materialType, material, mediStock, Suppliers);
                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(item, item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(item, item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(item, item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(item, item);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_HTCS_AMOUNT += item.CABIN_HTCS_AMOUNT;
                        dicRdo[key].BSCS_AMOUNT += item.BSCS_AMOUNT;
                        dicRdo[key].EXP_TOTAL_AMOUNT += item.EXP_TOTAL_AMOUNT;
                        dicRdo[key].SALE_TOTAL_PRICE += item.SALE_TOTAL_PRICE;
                        dicRdo[key].SALE_PRICE = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_PRICE : item.SALE_PRICE;
                        dicRdo[key].SALE_VAT_RATIO = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_VAT_RATIO : item.SALE_VAT_RATIO;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT += item.BHYT_EXP_TOTAL_AMOUNT;
                        dicRdo[key].VP_EXP_TOTAL_AMOUNT += item.VP_EXP_TOTAL_AMOUNT;
                        dicRdo[key].EXP_NGOAI_TRU_AMOUNT += item.EXP_NGOAI_TRU_AMOUNT;
                        dicRdo[key].EXP_NOI_TRU_AMOUNT += item.EXP_NOI_TRU_AMOUNT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NGT += item.BHYT_EXP_TOTAL_AMOUNT_NGT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NT += item.BHYT_EXP_TOTAL_AMOUNT_NT;
                        dicRdo[key].LOCAL_EXP_TOTAL_AMOUNT += item.LOCAL_EXP_TOTAL_AMOUNT;

                       
 
                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].EXP_TOTAL_PRICE += item.EXP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listExpMestMaterial.Clear();
                listExpMestMaterial = null;
                // gan du lieu xuat thuoc vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo thuoc
                foreach (var item in listExpMestMedicine)
                {
                    if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        item.PATIENT_TYPE_NAME = "BHYT";
                    }
                    else
                    {
                        item.PATIENT_TYPE_NAME = "VP";
                    }
                    item.LAST_EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.LAST_EXP_TIME_NUM ?? 0);
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var medicineType = dicMedicineType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMedicineType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(medicineType))
                            continue;

                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        var reqDepa = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID);

                        //them thong tin lo
                        InfoMediToRdo(item, medicineType, medicine, mediStock, Suppliers);
                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(item, item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(item, item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(item, item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(item, item);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_HTCS_AMOUNT += item.CABIN_HTCS_AMOUNT;
                        dicRdo[key].BSCS_AMOUNT += item.BSCS_AMOUNT;
                        dicRdo[key].EXP_TOTAL_AMOUNT += item.EXP_TOTAL_AMOUNT;
                        dicRdo[key].SALE_TOTAL_PRICE += item.SALE_TOTAL_PRICE;
                        dicRdo[key].SALE_PRICE = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_PRICE : item.SALE_PRICE;
                        dicRdo[key].SALE_VAT_RATIO = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_VAT_RATIO : item.SALE_VAT_RATIO;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT += item.BHYT_EXP_TOTAL_AMOUNT;
                        dicRdo[key].VP_EXP_TOTAL_AMOUNT += item.VP_EXP_TOTAL_AMOUNT;
                        dicRdo[key].EXP_NGOAI_TRU_AMOUNT += item.EXP_NGOAI_TRU_AMOUNT;
                        dicRdo[key].EXP_NOI_TRU_AMOUNT += item.EXP_NOI_TRU_AMOUNT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NGT += item.BHYT_EXP_TOTAL_AMOUNT_NGT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NT += item.BHYT_EXP_TOTAL_AMOUNT_NT;
                        dicRdo[key].LOCAL_EXP_TOTAL_AMOUNT += item.LOCAL_EXP_TOTAL_AMOUNT;


                        dicRdo[key].VACCIN_PRICE = dicRdo[key].VACCIN_PRICE > 0 ? dicRdo[key].VACCIN_PRICE : item.VACCIN_PRICE;
                        dicRdo[key].VACCIN_VAT_RATIO = dicRdo[key].VACCIN_PRICE > 0 ? dicRdo[key].VACCIN_VAT_RATIO : item.VACCIN_VAT_RATIO;

                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].EXP_TOTAL_PRICE += item.EXP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                //Tách xuất bán nhiều giá
                SeperateMediSalePrice();
                listExpMestMedicine.Clear();
                listExpMestMedicine = null;
                // gan du lieu xuat mau vao so luong ton dau, ton cuoi, cac loai xuat, cac khoa yeu cau, cac li do xuat khac cua cac lo mau
                foreach (var item in listExpMestBlood)
                {
                    if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        item.PATIENT_TYPE_NAME = "BHYT";
                    }
                    else
                    {
                        item.PATIENT_TYPE_NAME = "VP";
                    }
                    item.LAST_EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.LAST_EXP_TIME_NUM ?? 0);
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var blood = Bloods.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var bloodType = dicBloodType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicBloodType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(bloodType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);

                        //them thong tin lo
                        InfoBloodToRdo(item, bloodType, blood, mediStock, Suppliers);
                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(item, item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(item, item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(item, item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(item, item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(item, item);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_HTCS_AMOUNT += item.CABIN_HTCS_AMOUNT;
                        dicRdo[key].BSCS_AMOUNT += item.BSCS_AMOUNT;
                        dicRdo[key].EXP_TOTAL_AMOUNT += item.EXP_TOTAL_AMOUNT;
                        dicRdo[key].SALE_TOTAL_PRICE += item.SALE_TOTAL_PRICE;
                        dicRdo[key].SALE_PRICE = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_PRICE : item.SALE_PRICE;
                        dicRdo[key].SALE_VAT_RATIO = dicRdo[key].SALE_PRICE > 0 ? dicRdo[key].SALE_VAT_RATIO : item.SALE_VAT_RATIO;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT += item.BHYT_EXP_TOTAL_AMOUNT;
                        dicRdo[key].VP_EXP_TOTAL_AMOUNT += item.VP_EXP_TOTAL_AMOUNT;
                        dicRdo[key].EXP_NGOAI_TRU_AMOUNT += item.EXP_NGOAI_TRU_AMOUNT;
                        dicRdo[key].EXP_NOI_TRU_AMOUNT += item.EXP_NOI_TRU_AMOUNT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NGT += item.BHYT_EXP_TOTAL_AMOUNT_NGT;
                        dicRdo[key].BHYT_EXP_TOTAL_AMOUNT_NT += item.BHYT_EXP_TOTAL_AMOUNT_NT;
                        //cong so luong xuat vao cac loai xuat
                        AddExpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac ly do xuat khac
                        AddDicExpMestReasonAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.EXP_MEST_REASON_ID, ExpMestReasons);
                        //cong so luong xuat vao cac khoa yeu cau xuat
                        AddDicReqDepartmentAmountToRdo(dicRdo[key], item.EXP_TOTAL_AMOUNT, item.REQ_DEPARTMENT_ID, HisDepartmentCFG.DEPARTMENTs);
                        //cong so luong xuat vao cac kho duoc xuat
                        AddImpMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat vao cac khoa duoc xuat
                        AddImpReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong xuat HTCS cho cac kho cha
                        AddPreviousMediStockExpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].EXP_TOTAL_PRICE += item.EXP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listExpMestBlood.Clear();
                listExpMestBlood = null;
                // gan du lieu nhap vat tu vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo vat tu
                foreach (var item in listImpMestMaterial)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;

                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var materialType = dicMaterialType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMaterialType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(materialType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);

                        //them thong tin lo
                        InfoMateToRdo(item, materialType, material, mediStock, Suppliers);
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(item, item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(item, item);
                        //cong so luong nhap vao cac khoa chuyen sang
                        AddChmsReqDepartmentAmountToRdo(item, item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(item, item);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);

                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].IMP_MEST_TYPE_ID = item.IMP_MEST_TYPE_ID;
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_BSCS_AMOUNT += item.CABIN_BSCS_AMOUNT;
                        dicRdo[key].HTCS_AMOUNT += item.HTCS_AMOUNT;
                        dicRdo[key].IMP_TOTAL_AMOUNT += item.IMP_TOTAL_AMOUNT;
                        dicRdo[key].BHYT_IMP_TOTAL_AMOUNT += item.BHYT_IMP_TOTAL_AMOUNT;
                        dicRdo[key].VP_IMP_TOTAL_AMOUNT += item.VP_IMP_TOTAL_AMOUNT;
                        dicRdo[key].BHYT_DTTTL_TOTAL_AMOUNT += item.BHYT_DTTTL_TOTAL_AMOUNT;
                        dicRdo[key].VP_DTTTL_TOTAL_AMOUNT += item.VP_DTTTL_TOTAL_AMOUNT;
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac khoa chuyen sang
                        AddChmsReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].IMP_TOTAL_PRICE += item.IMP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listImpMestMaterial.Clear();
                listImpMestMaterial = null;
                // gan du lieu nhap thuoc vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo thuoc
                foreach (var item in listImpMestMedicine)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var medicineType = dicMedicineType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMedicineType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(medicineType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        var reqDepa = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID);
                        //them thong tin lo
                        InfoMediToRdo(item, medicineType, medicine, mediStock, Suppliers);
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(item, item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(item, item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsReqDepartmentAmountToRdo(item, item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(item, item);
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_BSCS_AMOUNT += item.CABIN_BSCS_AMOUNT;
                        dicRdo[key].HTCS_AMOUNT += item.HTCS_AMOUNT;
                        dicRdo[key].IMP_TOTAL_AMOUNT += item.IMP_TOTAL_AMOUNT;
                        dicRdo[key].BHYT_IMP_TOTAL_AMOUNT += item.BHYT_IMP_TOTAL_AMOUNT;
                        dicRdo[key].VP_IMP_TOTAL_AMOUNT += item.VP_IMP_TOTAL_AMOUNT;
                        dicRdo[key].BHYT_DTTTL_TOTAL_AMOUNT += item.BHYT_DTTTL_TOTAL_AMOUNT;
                        dicRdo[key].VP_DTTTL_TOTAL_AMOUNT += item.VP_DTTTL_TOTAL_AMOUNT;
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac khoa chuyen sang
                        AddChmsReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].IMP_TOTAL_PRICE += item.IMP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listImpMestMedicine.Clear();
                listImpMestMedicine = null;
                // gan du lieu nhap mau vao so luong ton dau, ton cuoi, cac loai nhap cua cac lo mau
                foreach (var item in listImpMestBlood)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var blood = Bloods.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var bloodType = dicBloodType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicBloodType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(bloodType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        //them thong tin lo
                        InfoBloodToRdo(item, bloodType, blood, mediStock, Suppliers);
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(item, item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(item, item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsReqDepartmentAmountToRdo(item, item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(item, item);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].BEGIN_AMOUNT += item.BEGIN_AMOUNT;
                        dicRdo[key].END_AMOUNT += item.END_AMOUNT;
                        dicRdo[key].CABIN_BSCS_AMOUNT += item.CABIN_BSCS_AMOUNT;
                        dicRdo[key].HTCS_AMOUNT += item.HTCS_AMOUNT;
                        dicRdo[key].IMP_TOTAL_AMOUNT += item.IMP_TOTAL_AMOUNT;
                        //cong so luong nhap vao cac loai nhap
                        AddImpMestTypeAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac kho chuyen sang
                        AddChmsMediStockAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cac khoa chuyen sang
                        AddChmsReqDepartmentAmountToRdo(dicRdo[key], item);
                        //cong so luong nhap vao cua cac kho cha
                        AddPreviousMediStockImpAmountsToRdo(dicRdo[key], item);
                    }
                    dicRdo[key].BEGIN_TOTAL_PRICE += item.BEGIN_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].END_TOTAL_PRICE += item.END_AMOUNT * dicRdo[key].IMP_PRICE;
                    dicRdo[key].IMP_TOTAL_PRICE += item.IMP_TOTAL_AMOUNT * dicRdo[key].IMP_PRICE;
                }

                listImpMestBlood.Clear();
                listImpMestBlood = null;


                //xử lý tính chênh lệch giữa tồn cuối và chốt kỳ trước tồn cuối

                // gan du lieu chot ky thuoc vao so luong ton dau cua cac lo thuoc
                foreach (var item in listMestPeriodMediEnd)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var medicine = Medicines.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var medicineType = dicMedicineType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMedicineType[item.MEDI_MATE_TYPE_ID] : null;
                        var requestDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID);
                        if (!checkServiceType(medicineType))
                            continue;

                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoMediToRdo(item, medicineType, medicine, mediStock, Suppliers);
                        item.END_PERIOD_AMOUNT = item.BEGIN_AMOUNT;
                        item.BEGIN_AMOUNT = 0;
                        item.END_AMOUNT = 0;
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].END_PERIOD_AMOUNT += item.END_AMOUNT;
                    }

                }
                listMestPeriodMediEnd.Clear();
                listMestPeriodMediEnd = null;
                // gan du lieu chot ky mau vao so luong ton dau cua cac lo mau
                foreach (var item in listMestPeriodBloodEnd)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var blood = Bloods.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var bloodType = dicBloodType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicBloodType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(bloodType))
                            continue;

                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoBloodToRdo(item, bloodType, blood, mediStock, Suppliers);
                        item.END_PERIOD_AMOUNT = item.BEGIN_AMOUNT;
                        item.BEGIN_AMOUNT = 0;
                        item.END_AMOUNT = 0;
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].END_PERIOD_AMOUNT += item.END_AMOUNT;
                    }

                }
                listMestPeriodBloodEnd.Clear();
                listMestPeriodBloodEnd = null;
                // gan du lieu chot ky vat tu vao so luong ton dau cua cac lo vat tu
                foreach (var item in listMestPeriodMateEnd)
                {
                    if (castFilter.TEST_PACKAGE_NUMBERs != null && string.Format(",{0},", castFilter.TEST_PACKAGE_NUMBERs).Contains(string.Format(",{0},", item.MEDI_MATE_ID)))
                        continue;
                    string key = string.Format("{0}_{1}_{2}", item.MEDI_MATE_ID, item.MEDI_STOCK_ID, item.TYPE);
                    if (!dicRdo.ContainsKey(key))
                    {
                        var material = Materials.FirstOrDefault(o => o.ID == item.MEDI_MATE_ID);
                        var materialType = dicMaterialType.ContainsKey(item.MEDI_MATE_TYPE_ID) ? dicMaterialType[item.MEDI_MATE_TYPE_ID] : null;
                        if (!checkServiceType(materialType))
                            continue;
                        var mediStock = ListMediStock.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        InfoMateToRdo(item, materialType, material, mediStock, Suppliers);
                        item.END_PERIOD_AMOUNT = item.BEGIN_AMOUNT;
                        item.BEGIN_AMOUNT = 0;
                        item.END_AMOUNT = 0;
                        //them so tien lech hoa don
                        AddDiffDocumentPrice(item, key);
                        dicRdo.Add(key, item);
                    }
                    else
                    {
                        dicRdo[key].END_PERIOD_AMOUNT += item.END_AMOUNT;
                    }

                }

                listMestPeriodMateEnd.Clear();
                listMestPeriodMateEnd = null;

                //kết thúc xử lý số lượng lệch giữa tồn cuối và chốt kỳ trước tồn cuối
                if (castFilter.IS_DETAIL == true)
                {
                    AddImpMestMedicineDetail();
                    AddImpMestMaterialDetail();
                    AddExpMestMedicineDetail();
                    AddExpMestMaterialDetail();
                }

                //Xử lý lọc thuốc vật tư theo kho xuất đến kho báo cáo
                FilterChmsMediStock();

                //trích xuất danh sách kho có dữ liệu
                AddMediStockHasData();


                //Gộp
                ProcessGroup();

                //lọc tách xuất bảo hiểm xuất viện phí
                FilterIsBhyt();

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                dicRdo = new Dictionary<string, Mrs00105RDO>();
                result = false;
            }
            return result;
        }

        private void SeperateMediSalePrice()
        {
            Dictionary<string, List<Mrs00105RDO>> expMediGroup = listExpMestMedicine.GroupBy(o => string.Format("{0}_{1}_{2}", o.MEDI_MATE_ID, o.MEDI_STOCK_ID, o.TYPE)).ToDictionary(o => o.Key, p => p.ToList());
            foreach (var medi in expMediGroup)
            {
                string key = medi.Key;
                if (dicRdo.ContainsKey(key))
                {
                    Dictionary<string, List<Mrs00105RDO>> groupBySalePrice = medi.Value.GroupBy(s => new { s.SALE_PRICE, s.SALE_VAT_RATIO }).ToDictionary(o => string.Format("{0}_{1}", o.Key.SALE_PRICE, o.Key.SALE_VAT_RATIO), p => p.ToList());
                    if (groupBySalePrice.Keys.Count <= 1) continue;
                    LogSystem.Info("LO_KHO_LOAI: " + key);
                    foreach (var item in groupBySalePrice)
                    {
                        Mrs00105RDO rdo = new Mrs00105RDO();
                        System.Reflection.PropertyInfo[] pis = Inventec.Common.Repository.Properties.Get<Mrs00105RDO>();
                        foreach (var pi in pis)
                        {
                            if (!pi.Name.Contains("_AMOUNT") && !pi.Name.Contains("_TOTAL_PRICE"))
                            {
                                pi.SetValue(rdo, pi.GetValue(dicRdo[key]));
                            }
                        }
                        rdo.EXP_TOTAL_AMOUNT = item.Value.Sum(s => s.EXP_TOTAL_AMOUNT);
                        rdo.SALE_TOTAL_PRICE = item.Value.Sum(s => s.SALE_TOTAL_PRICE);
                        rdo.SALE_PRICE = item.Value.First().SALE_PRICE;
                        rdo.SALE_VAT_RATIO = item.Value.First().SALE_VAT_RATIO;
                        if (dicRdo[key].EXP_TOTAL_AMOUNT - rdo.EXP_TOTAL_AMOUNT > 0)
                        {
                            LogSystem.Info("SALE_PRICE: " + item.Key);
                            dicRdo[key].EXP_TOTAL_AMOUNT -= rdo.EXP_TOTAL_AMOUNT;
                            dicRdo[key].SALE_TOTAL_PRICE -= rdo.SALE_TOTAL_PRICE;
                            dicRdo.Add(key + "_" + item.Key, rdo);
                        }
                    }
                }
            }
        }

        private void AddMediStockHasData()
        {
            var mediStockIds = dicRdo.Values.Select(o => o.MEDI_STOCK_ID).Distinct().ToList();
            listMediStockHasData = ListMediStock.Where(o => mediStockIds.Contains(o.ID)).ToList();
        }

        private void AddDiffDocumentPrice(Mrs00105RDO item, string key)
        {
            if (item.DIFF_DOCUMENT_TOTAL_PRICE == 0 && dicDiffDocumentPrice.ContainsKey(key))
            {
                item.DIFF_DOCUMENT_TOTAL_PRICE = dicDiffDocumentPrice[key];
            }
        }

        private void FilterChmsMediStock()
        {
            try
            {
                var List = dicRdo.Values.ToList();
                List<string> keyRemove = new List<string>();
                foreach (var dic in dicRdo)
                {
                    var item = dic.Value;
                    //lọc theo kho xuất thuốc đên kho báo cáo
                    List<MediMateIdChmsMediStockId> previousMediStocks = new List<MediMateIdChmsMediStockId>();

                    previousMediStocks.AddRange(MediMateIdChmsMediStockIds.Where(o => o.TYPE_ID == item.TYPE && o.MEDI_MATE_ID == item.MEDI_MATE_ID && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && o.CHMS_MEDI_STOCK_ID.HasValue).ToList());
                    item.PREVIOUS_MEDI_STOCK_CODE = string.Join(",", HisMediStockCFG.HisMediStocks.Where(o => previousMediStocks.Exists(p => p.CHMS_MEDI_STOCK_ID == o.ID)).Select(p => p.MEDI_STOCK_CODE).ToList());
                    item.PREVIOUS_MEDI_STOCK_NAME = string.Join(",", HisMediStockCFG.HisMediStocks.Where(o => previousMediStocks.Exists(p => p.CHMS_MEDI_STOCK_ID == o.ID)).Select(p => p.MEDI_STOCK_NAME).ToList());

                    if (castFilter.PREVIOUS_MEDI_STOCK_IDs != null)
                    {
                        if (!previousMediStocks.Exists(o => castFilter.PREVIOUS_MEDI_STOCK_IDs.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)))
                        {
                            if (item.PreviousMediStockExpAmounts != null && item.PreviousMediStockExpAmounts.Count > 0)
                            {
                                decimal errorAmount = item.PreviousMediStockExpAmounts.Where(p => castFilter.PREVIOUS_MEDI_STOCK_IDs.Contains(p.PREVIOUS_MEDI_STOCK_ID)).Sum(ss => ss.AMOUNT);
                                if (errorAmount > 0)
                                    Inventec.Common.Logging.LogSystem.Error(string.Format("TYPE:{0},MEDI_MATE_ID:{1},MEDI_STOCK_ID:{2} kho cha nhap != kho cha xuat,amount:{3}", item.TYPE, item.MEDI_MATE_ID, item.MEDI_STOCK_ID, errorAmount));
                            }
                            keyRemove.Add(dic.Key);
                        }
                        else if (castFilter.PREVIOUS_MEDI_STOCK_IDs.Count == 1)
                        {
                            //nhap
                            if (item.PreviousMediStockImpAmounts != null && item.PreviousMediStockImpAmounts.Count > 0)
                            {
                                item.IMP_TOTAL_AMOUNT = item.PreviousMediStockImpAmounts.Where(p => p.PREVIOUS_MEDI_STOCK_ID == castFilter.PREVIOUS_MEDI_STOCK_IDs[0]).Sum(ss => ss.AMOUNT);
                            }
                            //xuat HTCS
                            if (item.PreviousMediStockExpAmounts != null && item.PreviousMediStockExpAmounts.Count > 0)
                            {
                                item.CABIN_HTCS_AMOUNT = item.PreviousMediStockExpAmounts.Where(p => p.PREVIOUS_MEDI_STOCK_ID == castFilter.PREVIOUS_MEDI_STOCK_IDs[0]).Sum(ss => ss.AMOUNT);
                            }

                            if (previousMediStocks.Where(o => o.CHMS_MEDI_STOCK_ID.HasValue).Select(o => o.CHMS_MEDI_STOCK_ID.Value).Distinct().Count() > 1)
                            {
                                item.PREVIOUS_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => castFilter.PREVIOUS_MEDI_STOCK_IDs.Contains(o.ID)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                                item.PREVIOUS_MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => castFilter.PREVIOUS_MEDI_STOCK_IDs.Contains(o.ID)) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                                item.PACKAGE_NUMBER = item.PACKAGE_NUMBER + " ";
                                if (castFilter.IS_MERGE_PACKAGE_NUMBER == true)
                                {
                                    item.PACKAGE_NUMBER_1 = item.PACKAGE_NUMBER_1 + " ";
                                }
                                else
                                {
                                    item.PACKAGE_NUMBER_1 = "";
                                }
                                //ton dau
                                var beforeMediStock = previousMediStocks.Where(o => o.IS_ON != 1).OrderBy(m => m.MAX_IMP_TIME).LastOrDefault();
                                if (beforeMediStock != null && castFilter.PREVIOUS_MEDI_STOCK_IDs[0] == (beforeMediStock.CHMS_MEDI_STOCK_ID ?? 0))
                                {
                                    //ton dau du nguyen
                                }
                                else
                                {
                                    item.BEGIN_AMOUNT = 0;
                                }

                                //ton cuoi
                                var onMediStock = previousMediStocks.OrderBy(m => m.MAX_IMP_TIME).LastOrDefault();
                                if (onMediStock != null && castFilter.PREVIOUS_MEDI_STOCK_IDs[0] == (onMediStock.CHMS_MEDI_STOCK_ID ?? 0))
                                {
                                    //ton cuoi du nguyen
                                    //xuat du nguyen
                                }
                                else
                                {
                                    //ton cuoi
                                    item.END_AMOUNT = 0;
                                    //xuat
                                    item.EXP_TOTAL_AMOUNT = 0;
                                    //item.CABIN_HTCS_AMOUNT = 0;
                                    item.ID__DPK_EXP_AMOUNT = 0;
                                    item.ID__CK_EXP_AMOUNT = 0;
                                    item.ID__DNT_EXP_AMOUNT = 0;
                                    item.ID__DM_EXP_AMOUNT = 0;
                                    item.ID__DTT_EXP_AMOUNT = 0;
                                    item.ID__HPKP_EXP_AMOUNT = 0;
                                    item.ID__KHAC_EXP_AMOUNT = 0;
                                    item.ID__PL_EXP_AMOUNT = 0;
                                    item.ID__TNCC_EXP_AMOUNT = 0;
                                }
                            }

                        }
                    }


                }
                dicRdo = dicRdo.Where(o => !keyRemove.Contains(o.Key)).ToDictionary(p => p.Key, q => q.Value);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }



        private void FilterIsBhyt()
        {
            if (castFilter.INPUT_DATA_ID_IS_BHYT == 1)
            {
                ListRdo = ListRdo.Where(o => o.BHYT_EXP_TOTAL_AMOUNT > 0 || o.BHYT_IMP_TOTAL_AMOUNT > 0).ToList();
            }
            if (castFilter.INPUT_DATA_ID_IS_BHYT == 2)
            {
                ListRdo = ListRdo.Where(o => o.VP_EXP_TOTAL_AMOUNT > 0 || o.VP_IMP_TOTAL_AMOUNT > 0).ToList();
            }
        }

        private void FilterQuota()
        {

            listMestPeriodMedi = listMestPeriodMedi.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS thuoc chot ki
            listMestPeriodMate = listMestPeriodMate.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS vat tu chot ki

            listImpMestMedicine = listImpMestMedicine.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS thuoc nhap tu sau chot ky den timeto
            listImpMestMaterial = listImpMestMaterial.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS vat tu nhap tu sau chot ky den timeto

            listExpMestMedicine = listExpMestMedicine.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS thuoc xuat tu sau chot ky den timeto
            listExpMestMaterial = listExpMestMaterial.Where(o => listServMetyMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE)).ToList(); // DS vat tu xuat tu sau chot ky den timeto

        }

        private void FilterExpMediStock()
        {

            listMestPeriodMedi = listMestPeriodMedi.Where(o => listMediStockMety.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS thuoc chot ki
            listMestPeriodMate = listMestPeriodMate.Where(o => listMediStockMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS vat tu chot ki

            listImpMestMedicine = listImpMestMedicine.Where(o => listMediStockMety.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS thuoc nhap tu sau chot ky den timeto
            listImpMestMaterial = listImpMestMaterial.Where(o => listMediStockMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS vat tu nhap tu sau chot ky den timeto

            listExpMestMedicine = listExpMestMedicine.Where(o => listMediStockMety.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS thuoc xuat tu sau chot ky den timeto
            listExpMestMaterial = listExpMestMaterial.Where(o => listMediStockMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS vat tu xuat tu sau chot ky den timeto

            listMestPeriodMediEnd = listMestPeriodMediEnd.Where(o => listMediStockMety.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS thuoc chot cuoi ky
            listMestPeriodMateEnd = listMestPeriodMateEnd.Where(o => listMediStockMaty.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && p.TYPE == o.TYPE && o.MEDI_STOCK_ID == p.MEDI_STOCK_ID)).ToList(); // DS vat tu chot cuoi ky
        }


        private bool checkServiceType(V_HIS_MEDICINE_TYPE medicineType)
        {
            if (castFilter.EXACT_PARENT_SERVICE_IDs == null)
                return true;
            if (medicineType == null)
                return false;
            if (medicineType.PARENT_ID == null)
                return false;
            if (this.dicMedicineType == null)
                return false;
            if (!dicMedicineType.ContainsKey(medicineType.PARENT_ID ?? 0))
                return false;
            var parent = this.dicMedicineType[medicineType.PARENT_ID ?? 0];
            if (parent == null)
                return false;
            if (castFilter.EXACT_PARENT_SERVICE_IDs.Contains(parent.SERVICE_ID))
                return true;
            else return false;
        }

        private bool checkServiceType(V_HIS_BLOOD_TYPE bloodType)
        {
            if (castFilter.EXACT_PARENT_SERVICE_IDs == null)
                return true;
            if (bloodType == null)
                return false;
            if (bloodType.PARENT_ID == null)
                return false;
            if (this.dicBloodType == null)
                return false;
            if (!dicBloodType.ContainsKey(bloodType.PARENT_ID ?? 0))
                return false;
            var parent = this.dicBloodType[bloodType.PARENT_ID ?? 0];
            if (parent == null)
                return false;
            if (castFilter.EXACT_PARENT_SERVICE_IDs.Contains(parent.SERVICE_ID))
                return true;
            else return false;//
        }

        private bool checkServiceType(V_HIS_MATERIAL_TYPE MaterialType)
        {

            if (castFilter.EXACT_PARENT_SERVICE_IDs == null)
                return true;
            if (MaterialType == null)
                return false;
            if (MaterialType.PARENT_ID == null)
                return false;
            if (this.dicMaterialType == null)
                return false;
            if (!dicMaterialType.ContainsKey(MaterialType.PARENT_ID ?? 0))
                return false;
            var parent = this.dicMaterialType[MaterialType.PARENT_ID ?? 0];
            if (parent == null)
                return false;
            if (castFilter.EXACT_PARENT_SERVICE_IDs.Contains(parent.SERVICE_ID))
                return true;
            else return false;
        }

        private void AddDicReqDepartmentAmountToRdo(Mrs00105RDO itemAgg, decimal expTotalAmount, long? reqDepartmentId, List<HIS_DEPARTMENT> departments)
        {
            if (reqDepartmentId.HasValue)
            {
                var department = departments.FirstOrDefault(o => o.ID == reqDepartmentId);
                if (department != null)
                {
                    string key = department.DEPARTMENT_CODE;
                    if (itemAgg.DIC_EXP_REQ_DEPARTMENT == null)
                    {
                        itemAgg.DIC_EXP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                    }
                    if (itemAgg.DIC_EXP_REQ_DEPARTMENT.ContainsKey(key))
                    {
                        itemAgg.DIC_EXP_REQ_DEPARTMENT[key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_EXP_REQ_DEPARTMENT.Add(key, expTotalAmount);
                        if (!this.CodeDestDps.Contains(key))
                        {
                            this.CodeDestDps.Add(key);
                        }
                    }
                }
            }
        }

        private void AddDicExpMestReasonAmountToRdo(Mrs00105RDO itemAgg, decimal expTotalAmount, long? expMestReasonId, List<HIS_EXP_MEST_REASON> expMestReasons)
        {
            if (expMestReasonId.HasValue)
            {
                var expMestReason = expMestReasons.FirstOrDefault(o => o.ID == expMestReasonId);
                if (expMestReason != null)
                {
                    string key = expMestReason.EXP_MEST_REASON_CODE;
                    if (itemAgg.DIC_EXP_MEST_REASON == null)
                    {
                        itemAgg.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                    }
                    if (itemAgg.DIC_EXP_MEST_REASON.ContainsKey(key))
                    {
                        itemAgg.DIC_EXP_MEST_REASON[key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_EXP_MEST_REASON.Add(key, expTotalAmount);
                    }
                }
            }
        }

        private void AddImpMediStockAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            try
            {
                if (itemAgg.DIC_IMP_MEDI_STOCK == null) itemAgg.DIC_IMP_MEDI_STOCK = new Dictionary<string, decimal>();
                var dicChms = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(item.JSON_IMP_MEDI_STOCK);
                if (dicChms == null || dicChms.Count == 0) return;
                foreach (var i in dicChms)
                {
                    var expTotalAmount = i.Value != null ? decimal.Parse(i.Value.ToString()) / 10000 : 0;
                    if (expTotalAmount == 0) continue;
                    if (itemAgg.DIC_IMP_MEDI_STOCK.ContainsKey(i.Key))
                    {
                        itemAgg.DIC_IMP_MEDI_STOCK[i.Key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_IMP_MEDI_STOCK.Add(i.Key, expTotalAmount);
                        if (!this.CodeDestChms.Contains(i.Key))
                        {
                            this.CodeDestChms.Add(i.Key);
                        }
                    }
                    if (itemAgg.DIC_IMP_MEDI_STOCK.ContainsKey(itemAgg.MEDI_STOCK_CODE + "_TO_" + i.Key))
                    {
                        itemAgg.DIC_IMP_MEDI_STOCK[itemAgg.MEDI_STOCK_CODE + "_TO_" + i.Key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_IMP_MEDI_STOCK.Add(itemAgg.MEDI_STOCK_CODE + "_TO_" + i.Key, expTotalAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void AddChmsMediStockAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            try
            {
                if (itemAgg.DIC_CHMS_MEDI_STOCK == null) itemAgg.DIC_CHMS_MEDI_STOCK = new Dictionary<string, decimal>();
                var dicChms = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(item.JSON_CHMS_MEDI_STOCK);
                if (dicChms == null || dicChms.Count == 0) return;
                foreach (var i in dicChms)
                {
                    var expTotalAmount = i.Value != null ? decimal.Parse(i.Value.ToString()) / 10000 : 0;
                    if (expTotalAmount == 0) continue;
                    if (itemAgg.DIC_CHMS_MEDI_STOCK.ContainsKey(i.Key))
                    {
                        itemAgg.DIC_CHMS_MEDI_STOCK[i.Key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_CHMS_MEDI_STOCK.Add(i.Key, expTotalAmount);
                        if (!this.CodeSourceChms.Contains(i.Key))
                        {
                            this.CodeSourceChms.Add(i.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void AddImpReqDepartmentAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            try
            {
                if (itemAgg.DIC_IMP_REQ_DEPARTMENT == null) itemAgg.DIC_IMP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                var dicChms = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(item.JSON_IMP_REQ_DEPARTMENT);
                if (dicChms == null || dicChms.Count == 0) return;
                foreach (var i in dicChms)
                {
                    var expTotalAmount = i.Value != null ? decimal.Parse(i.Value.ToString()) / 10000 : 0;
                    if (expTotalAmount == 0) continue;
                    if (itemAgg.DIC_IMP_REQ_DEPARTMENT.ContainsKey(i.Key))
                    {
                        itemAgg.DIC_IMP_REQ_DEPARTMENT[i.Key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_IMP_REQ_DEPARTMENT.Add(i.Key, expTotalAmount);
                        if (!this.CodeDestDps.Contains(i.Key))
                        {
                            this.CodeDestDps.Add(i.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void AddChmsReqDepartmentAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            try
            {
                if (itemAgg.DIC_CHMS_REQ_DEPARTMENT == null) itemAgg.DIC_CHMS_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                var dicChms = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(item.JSON_CHMS_REQ_DEPARTMENT);
                if (dicChms == null || dicChms.Count == 0) return;
                foreach (var i in dicChms)
                {
                    var expTotalAmount = i.Value != null ? decimal.Parse(i.Value.ToString()) / 10000 : 0;
                    if (expTotalAmount == 0) continue;
                    if (itemAgg.DIC_CHMS_REQ_DEPARTMENT.ContainsKey(i.Key))
                    {
                        itemAgg.DIC_CHMS_REQ_DEPARTMENT[i.Key] += expTotalAmount;
                    }
                    else
                    {
                        itemAgg.DIC_CHMS_REQ_DEPARTMENT.Add(i.Key, expTotalAmount);
                        if (!this.CodeSourceDps.Contains(i.Key))
                        {
                            this.CodeSourceDps.Add(i.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        private void AddExpMestTypeAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            if (item.EXP_MEST_TYPE_ID.HasValue)
            {
                if (dicExpAmountType.ContainsKey(item.EXP_MEST_TYPE_ID.Value))
                {
                    decimal value = (decimal)dicExpAmountType[item.EXP_MEST_TYPE_ID.Value].GetValue(itemAgg);
                    dicExpAmountType[item.EXP_MEST_TYPE_ID.Value].SetValue(itemAgg, item.EXP_TOTAL_AMOUNT + value);
                }
            }
        }

        private void AddImpMestTypeAmountToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            if (item.IMP_MEST_TYPE_ID.HasValue)
            {
                if (dicImpAmountType.ContainsKey(item.IMP_MEST_TYPE_ID.Value))
                {
                    decimal value = (decimal)dicImpAmountType[item.IMP_MEST_TYPE_ID.Value].GetValue(itemAgg);
                    dicImpAmountType[item.IMP_MEST_TYPE_ID.Value].SetValue(itemAgg, item.IMP_TOTAL_AMOUNT + value);
                }
            }
        }

        private void AddPreviousMediStockImpAmountsToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            if (item.PREVIOUS_MEDI_STOCK_ID.HasValue)
            {
                if (itemAgg.PreviousMediStockImpAmounts == null)
                {
                    itemAgg.PreviousMediStockImpAmounts = new List<PreviousMediStockAmount>();
                }
                itemAgg.PreviousMediStockImpAmounts.Add(new PreviousMediStockAmount() { PREVIOUS_MEDI_STOCK_ID = item.PREVIOUS_MEDI_STOCK_ID ?? 0, AMOUNT = item.IMP_TOTAL_AMOUNT });
            }
        }

        private void AddPreviousMediStockExpAmountsToRdo(Mrs00105RDO itemAgg, Mrs00105RDO item)
        {
            if (item.PREVIOUS_MEDI_STOCK_ID.HasValue)
            {
                if (itemAgg.PreviousMediStockExpAmounts == null)
                {
                    itemAgg.PreviousMediStockExpAmounts = new List<PreviousMediStockAmount>();
                }
                itemAgg.PreviousMediStockExpAmounts.Add(new PreviousMediStockAmount() { PREVIOUS_MEDI_STOCK_ID = item.PREVIOUS_MEDI_STOCK_ID ?? 0, AMOUNT = item.CABIN_HTCS_AMOUNT });
            }
        }

        private void InfoMediToRdo(Mrs00105RDO item, V_HIS_MEDICINE_TYPE medicineType, HIS_MEDICINE medicine, V_HIS_MEDI_STOCK mediStock, List<HIS_SUPPLIER> suppliers)
        {
            var depa = listExpReqDepartment.FirstOrDefault(p => p.ID == item.REQ_DEPARTMENT_ID);
            if (mediStock != null)
            {
                item.IS_CABINET = mediStock.IS_CABINET;
                item.MEDI_STOCK_CODE = mediStock.MEDI_STOCK_CODE;
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 1;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NGTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 2;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__CLSs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 3;
                //}
                item.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                item.DEPARTMENT_NAME = mediStock.DEPARTMENT_NAME;

            }
            if (depa != null)
            {
                item.REQ_DEPARTMENT_CODE = depa.DEPARTMENT_CODE;
                item.REQ_DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
            }
            if (medicine != null)
            {
                item.IS_SALE_EQUAL_IMP_PRICE = medicine.IS_SALE_EQUAL_IMP_PRICE;
                item.BID_GROUP_CODE = medicine.TDL_BID_GROUP_CODE;
                item.BID_NUM_ORDER = medicine.TDL_BID_NUM_ORDER;
                item.TDL_BID_NUMBER = medicine.TDL_BID_NUMBER;
                item.BID_ID = medicine.BID_ID ?? 0;
                item.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;
                item.IMP_TIME = medicine.IMP_TIME;
                if (castFilter.IS_MERGE_PACKAGE_NUMBER == true)
                {
                    item.PACKAGE_NUMBER_1 = medicine.PACKAGE_NUMBER;
                }
                else
                {
                    item.PACKAGE_NUMBER_1 = "";
                }
                item.BID_YEAR = medicine.TDL_BID_YEAR;//năm thầu
                item.BID_PACKAGE_CODE = medicine.TDL_BID_PACKAGE_CODE;//gói thầu
                item.EXPIRED_DATE = medicine.EXPIRED_DATE;
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                item.VAT_RATIO = medicine.IMP_VAT_RATIO;

                item.IMP_PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                item.IMP_PRICE_NEW = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
                var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == medicine.BID_ID);
                if (bid != null)
                {
                    if (string.IsNullOrEmpty(item.TDL_BID_NUMBER))
                    {
                        item.TDL_BID_NUMBER = bid.BID_NUMBER;
                    }

                    item.BID_NAME = bid.BID_NAME;
                    item.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                    item.VALID_TO_TIME = bid.VALID_TO_TIME;
                    item.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.MEDI_MATE_TYPE_ID, THUOC, medicine.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                    item.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                }
                //if (dicBidMetyAmount.ContainsKey(string.Format("{0}_{1}", medicine.BID_ID, medicine.MEDICINE_TYPE_ID)))
                //{
                //    item.BID_AM = dicBidMetyAmount[string.Format("{0}_{1}", medicine.BID_ID, medicine.MEDICINE_TYPE_ID)].ToString();
                //}
                if (dicMedicineDocumentInfo.ContainsKey(medicine.ID))
                {
                    item.DOCUMENT_NUMBER = dicMedicineDocumentInfo[medicine.ID].DOCUMENT_NUMBER;
                    item.DOCUMENT_DATE = dicMedicineDocumentInfo[medicine.ID].DOCUMENT_DATE;
                }
                var impSource = (this.ImpSources ?? new List<HIS_IMP_SOURCE>()).FirstOrDefault(o => o.ID == medicine.IMP_SOURCE_ID);
                if (castFilter.IMP_SOURCE_IDs != null && !castFilter.IMP_SOURCE_IDs.Contains(medicine.IMP_SOURCE_ID ?? 0))
                {
                    impSource = null;
                }
                if (impSource != null)
                {
                    item.IMP_SOURCE_ID = impSource.ID;
                    item.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                    item.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                }
                if (medicineType != null)
                {
                    item.SERVICE_CODE = medicineType.MEDICINE_TYPE_CODE;
                    item.SERVICE_NAME = medicineType.MEDICINE_TYPE_NAME;
                    item.ATC_CODES = medicineType.ATC_CODES;
                    item.SERVICE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                    item.CONCENTRA = medicineType.CONCENTRA;
                    item.NATIONAL_NAME = medicineType.NATIONAL_NAME ?? " ";
                    item.IS_VACCINE = medicineType.IS_VACCINE;
                    item.VACCINE_TYPE_ID = medicineType.VACCINE_TYPE_ID;
                    item.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.MEDICINE_TYPE_PROPRIETARY_NAME;
                    item.MANUFACTURER_NAME = medicineType.MANUFACTURER_NAME;
                    item.HEIN_SERVICE_CODE = medicineType.HEIN_SERVICE_BHYT_CODE;
                    item.HEIN_SERVICE_NAME = medicineType.HEIN_SERVICE_BHYT_NAME;
                    item.HEIN_SERVICE_TYPE_CODE = medicineType.HEIN_SERVICE_TYPE_CODE;
                    item.HEIN_SERVICE_TYPE_NAME = medicineType.HEIN_SERVICE_TYPE_NAME;
                    item.ACTIVE_INGR_BHYT_CODE = medicine.ACTIVE_INGR_BHYT_CODE ?? medicineType.ACTIVE_INGR_BHYT_CODE;
                    item.ACTIVE_INGR_BHYT_NAME = medicine.ACTIVE_INGR_BHYT_NAME ?? medicineType.ACTIVE_INGR_BHYT_NAME;
                    item.PACKING_TYPE_NAME = medicineType.PACKING_TYPE_NAME;
                    item.REGISTER_NUMBER = medicine.MEDICINE_REGISTER_NUMBER ?? medicineType.REGISTER_NUMBER;
                    item.MEDICINE_USE_FORM_CODE = medicineType.MEDICINE_USE_FORM_CODE;
                    item.MEDICINE_USE_FORM_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                    item.BYT_NUM_ORDER = medicineType.BYT_NUM_ORDER;
                    item.TCY_NUM_ORDER = medicineType.TCY_NUM_ORDER;
                    item.SCIENTIFIC_NAME = medicineType.SCIENTIFIC_NAME;
                    item.PREPROCESSING = medicineType.PREPROCESSING;
                    item.PROCESSING = medicineType.PROCESSING;
                    item.USED_PART = medicineType.USED_PART;
                    item.DOSAGE_FORM = medicineType.DOSAGE_FORM;
                    item.DISTRIBUTED_SL = medicineType.DISTRIBUTED_AMOUNT;
                    item.QUALITY_STANDARDS = medicineType.QUALITY_STANDARDS;
                    item.SOURCE_MEDICINE = medicineType.SOURCE_MEDICINE ?? 0;

                }

                if (suppliers != null)
                {
                    var supplier = suppliers.FirstOrDefault(o => o.ID == medicine.SUPPLIER_ID);
                    if (supplier != null)
                    {
                        item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                        item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                        item.SUPPLIER_ADDRESS = supplier.ADDRESS;
                        item.SUPPLIER_ID = supplier.ID;
                    }
                }
            }

        }

        private void InfoBloodToRdo(Mrs00105RDO item, V_HIS_BLOOD_TYPE bloodType, HIS_BLOOD blood, V_HIS_MEDI_STOCK mediStock, List<HIS_SUPPLIER> suppliers)
        {

            if (mediStock != null)
            {
                item.IS_CABINET = mediStock.IS_CABINET;
                item.MEDI_STOCK_CODE = mediStock.MEDI_STOCK_CODE;
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 1;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NGTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 2;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__CLSs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 3;
                //}
                item.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                item.DEPARTMENT_NAME = mediStock.DEPARTMENT_NAME;


            }
            if (blood != null)
            {
                var bloodAbo = listBloodAbo.FirstOrDefault(o => o.ID == blood.BLOOD_ABO_ID);
                if (bloodAbo != null)
                {
                    item.BLOOD_ABO_CODE = bloodAbo.BLOOD_ABO_CODE;
                }
                //item.IS_SALE_EQUAL_IMP_PRICE = blood.IS_SALE_EQUAL_IMP_PRICE;
                //item.BID_GROUP_CODE = blood.TDL_BID_GROUP_CODE;
                //item.BID_NUM_ORDER = blood.TDL_BID_NUM_ORDER;
                //item.TDL_BID_NUMBER = blood.TDL_BID_NUMBER;
                item.PACKAGE_NUMBER = blood.PACKAGE_NUMBER;
                item.IMP_TIME = blood.IMP_TIME;
                if (castFilter.IS_MERGE_PACKAGE_NUMBER == true)
                {
                    item.PACKAGE_NUMBER_1 = blood.PACKAGE_NUMBER;
                }
                else
                {
                    item.PACKAGE_NUMBER_1 = "";
                }
                //item.BID_YEAR = blood.TDL_BID_YEAR;//năm thầu
                //item.BID_PACKAGE_CODE = blood.TDL_BID_PACKAGE_CODE;//gói thầu
                item.EXPIRED_DATE = blood.EXPIRED_DATE;
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                item.VAT_RATIO = blood.IMP_VAT_RATIO;

                item.IMP_PRICE = blood.IMP_PRICE * (1 + blood.IMP_VAT_RATIO);
                item.IMP_PRICE_NEW = blood.IMP_PRICE * (1 + (blood.IMP_VAT_RATIO));
                var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == blood.BID_ID);
                if (bid != null)
                {
                    item.BID_NAME = bid.BID_NAME;
                    item.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                    item.VALID_TO_TIME = bid.VALID_TO_TIME;
                    item.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.MEDI_MATE_TYPE_ID, MAU, blood.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                    item.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                }
                //if (dicBidMetyAmount.ContainsKey(string.Format("{0}_{1}", blood.BID_ID, blood.BLOOD_TYPE_ID)))
                //{
                //    item.BID_AM = dicBidMetyAmount[string.Format("{0}_{1}", blood.BID_ID, blood.BLOOD_TYPE_ID)].ToString();
                //}
                var impSource = (this.ImpSources ?? new List<HIS_IMP_SOURCE>()).FirstOrDefault(o => o.ID == blood.IMP_SOURCE_ID);
                if (castFilter.IMP_SOURCE_IDs != null && !castFilter.IMP_SOURCE_IDs.Contains(blood.IMP_SOURCE_ID ?? 0))
                {
                    impSource = null;
                }
                if (impSource != null)
                {
                    item.IMP_SOURCE_ID = impSource.ID;
                    item.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                    item.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                }
                if (bloodType != null)
                {
                    item.SERVICE_CODE = bloodType.BLOOD_TYPE_CODE;
                    item.SERVICE_NAME = bloodType.BLOOD_TYPE_NAME;
                    //item.ATC_CODES = bloodType.ATC_CODES;
                    item.SERVICE_UNIT_NAME = bloodType.SERVICE_UNIT_NAME;
                    //item.CONCENTRA = bloodType.CONCENTRA;
                    item.NATIONAL_NAME = " ";
                    //item.MEDICINE_TYPE_PROPRIETARY_NAME = bloodType.MEDICINE_TYPE_PROPRIETARY_NAME;
                    //item.MANUFACTURER_NAME = bloodType.MANUFACTURER_NAME;
                    item.HEIN_SERVICE_CODE = bloodType.HEIN_SERVICE_BHYT_CODE;
                    item.HEIN_SERVICE_NAME = bloodType.HEIN_SERVICE_BHYT_NAME;
                    item.HEIN_SERVICE_TYPE_CODE = bloodType.HEIN_SERVICE_TYPE_CODE;
                    item.HEIN_SERVICE_TYPE_NAME = bloodType.HEIN_SERVICE_TYPE_NAME;
                    //item.ACTIVE_INGR_BHYT_CODE = blood.ACTIVE_INGR_BHYT_CODE ?? bloodType.ACTIVE_INGR_BHYT_CODE;
                    //item.ACTIVE_INGR_BHYT_NAME = blood.ACTIVE_INGR_BHYT_NAME ?? bloodType.ACTIVE_INGR_BHYT_NAME;
                    item.PACKING_TYPE_NAME = bloodType.PACKING_TYPE_NAME;
                    //item.REGISTER_NUMBER = blood.MEDICINE_REGISTER_NUMBER ?? bloodType.REGISTER_NUMBER;
                    //item.MEDICINE_USE_FORM_CODE = bloodType.MEDICINE_USE_FORM_CODE;
                    //item.MEDICINE_USE_FORM_NAME = bloodType.MEDICINE_USE_FORM_NAME;
                    //item.BYT_NUM_ORDER = bloodType.BYT_NUM_ORDER;
                    //item.TCY_NUM_ORDER = bloodType.TCY_NUM_ORDER;
                    //item.SCIENTIFIC_NAME = bloodType.SCIENTIFIC_NAME;
                    //item.PREPROCESSING = bloodType.PREPROCESSING;
                    //item.PROCESSING = bloodType.PROCESSING;
                    //item.USED_PART = bloodType.USED_PART;
                    //item.DOSAGE_FORM = bloodType.DOSAGE_FORM;
                    //item.DISTRIBUTED_SL = bloodType.DISTRIBUTED_AMOUNT;
                    item.IMP_PRICE_NEW = (bloodType.IMP_PRICE ?? 0) * (1 + (bloodType.IMP_VAT_RATIO ?? 0));
                }

                if (suppliers != null)
                {
                    var supplier = suppliers.FirstOrDefault(o => o.ID == blood.SUPPLIER_ID);
                    if (supplier != null)
                    {
                        item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                        item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                        item.SUPPLIER_ADDRESS = supplier.ADDRESS;
                        item.SUPPLIER_ID = supplier.ID;
                    }
                }
            }

        }

        private void InfoMateToRdo(Mrs00105RDO item, V_HIS_MATERIAL_TYPE materialType, HIS_MATERIAL material, V_HIS_MEDI_STOCK mediStock, List<HIS_SUPPLIER> suppliers)
        {

            if (mediStock != null)
            {
                item.IS_CABINET = mediStock.IS_CABINET;
                item.MEDI_STOCK_CODE = mediStock.MEDI_STOCK_CODE;
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 1;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NGTs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 2;
                //}
                //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__CLSs).Contains(string.Format(",{0},", item.MEDI_STOCK_CODE)))
                //{
                //    item.IS_NT_NGT_CLS = 3;
                //}
                item.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                item.DEPARTMENT_NAME = mediStock.DEPARTMENT_NAME;

            }
            if (material != null)
            {
                item.IS_SALE_EQUAL_IMP_PRICE = material.IS_SALE_EQUAL_IMP_PRICE;
                item.BID_GROUP_CODE = material.TDL_BID_GROUP_CODE;
                item.BID_NUM_ORDER = material.TDL_BID_NUM_ORDER;
                item.TDL_BID_NUMBER = material.TDL_BID_NUMBER;
                item.BID_ID = material.BID_ID ?? 0;
                item.PACKAGE_NUMBER = material.PACKAGE_NUMBER;
                item.IMP_TIME = material.IMP_TIME;
                if (castFilter.IS_MERGE_PACKAGE_NUMBER == true)
                {
                    item.PACKAGE_NUMBER_1 = material.PACKAGE_NUMBER;
                }
                else
                {
                    item.PACKAGE_NUMBER_1 = "";
                }
                item.BID_YEAR = material.TDL_BID_YEAR;//năm thầu
                item.BID_PACKAGE_CODE = material.TDL_BID_PACKAGE_CODE;//gói thầu

                item.EXPIRED_DATE = material.EXPIRED_DATE;
                item.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                item.VAT_RATIO = material.IMP_VAT_RATIO;

                item.IMP_PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                item.IMP_PRICE_NEW = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);

                //trạng thái tái sử dụng
                if (listIsReusabling != null)
                {
                    var IsReusabling = listIsReusabling.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID);
                    if (IsReusabling != null)
                    {
                        item.IS_REUSABLING = IsReusabling.IS_REUSABLING;
                    }
                }

                var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == material.BID_ID);
                if (bid != null)
                {
                    if (string.IsNullOrEmpty(item.TDL_BID_NUMBER))
                    {
                        item.TDL_BID_NUMBER = bid.BID_NUMBER;
                    }

                    item.BID_NAME = bid.BID_NAME;
                    item.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                    item.VALID_TO_TIME = bid.VALID_TO_TIME;
                    item.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.MEDI_MATE_TYPE_ID, VATTU, material.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                    item.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                }
                //if (dicBidMatyAmount.ContainsKey(string.Format("{0}_{1}", material.BID_ID, material.MATERIAL_TYPE_ID)))
                //{
                //    item.BID_AM = dicBidMatyAmount[string.Format("{0}_{1}", material.BID_ID, material.MATERIAL_TYPE_ID)].ToString();
                //}

                if (dicMaterialDocumentInfo.ContainsKey(material.ID))
                {
                    item.DOCUMENT_NUMBER = dicMaterialDocumentInfo[material.ID].DOCUMENT_NUMBER;
                    item.DOCUMENT_DATE = dicMaterialDocumentInfo[material.ID].DOCUMENT_DATE;
                }
                var impSource = (this.ImpSources ?? new List<HIS_IMP_SOURCE>()).FirstOrDefault(o => o.ID == material.IMP_SOURCE_ID);
                if (castFilter.IMP_SOURCE_IDs != null && !castFilter.IMP_SOURCE_IDs.Contains(material.IMP_SOURCE_ID ?? 0))
                {
                    impSource = null;
                }
                if (impSource != null)
                {
                    item.IMP_SOURCE_ID = impSource.ID;
                    item.IMP_SOURCE_CODE = impSource.IMP_SOURCE_CODE;
                    item.IMP_SOURCE_NAME = impSource.IMP_SOURCE_NAME;
                }
                if (materialType != null)
                {
                    item.SERVICE_CODE = materialType.MATERIAL_TYPE_CODE;
                    item.SERVICE_NAME = materialType.MATERIAL_TYPE_NAME;
                    item.SERVICE_UNIT_NAME = materialType.SERVICE_UNIT_NAME;
                    item.IS_CHEMICAL_SUBSTANCE = materialType.IS_CHEMICAL_SUBSTANCE;
                    item.CONCENTRA = materialType.CONCENTRA;
                    item.NATIONAL_NAME = materialType.NATIONAL_NAME ?? " ";
                    item.MANUFACTURER_NAME = materialType.MANUFACTURER_NAME;
                    item.HEIN_SERVICE_CODE = materialType.HEIN_SERVICE_BHYT_CODE;
                    item.HEIN_SERVICE_NAME = materialType.HEIN_SERVICE_BHYT_NAME;
                    item.HEIN_SERVICE_TYPE_CODE = materialType.HEIN_SERVICE_TYPE_CODE;
                    item.HEIN_SERVICE_TYPE_NAME = materialType.HEIN_SERVICE_TYPE_NAME;
                    item.PACKING_TYPE_NAME = materialType.PACKING_TYPE_NAME;
                    item.IMP_PRICE_NEW = (materialType.IMP_PRICE ?? 0) * (1 + (materialType.IMP_VAT_RATIO ?? 0));
                }

                if (suppliers != null)
                {
                    var supplier = suppliers.FirstOrDefault(o => o.ID == material.SUPPLIER_ID);
                    if (supplier != null)
                    {
                        item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                        item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                        item.SUPPLIER_ADDRESS = supplier.ADDRESS;
                        item.SUPPLIER_ID = supplier.ID;
                    }
                }
            }

        }

        private void AddImpMestMedicineDetail()
        {
            try
            {
                //foreach (var item in listImpMestMedicineOn)
                //{
                //    DETAIL rdo = new DETAIL();
                //    rdo.IMP_MEST_CODE = item.IMP_MEST_CODE;
                //    rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    var expMestMedicine = listExpMestMedicineOn.FirstOrDefault(o => o.CK_IMP_MEST_MEDICINE_ID == item.ID);
                //    if (expMestMedicine != null)
                //    {
                //        rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMestMedicine.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    }
                //    rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                //    rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                //    rdo.CONCENTRA = item.CONCENTRA;
                //    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                //    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                //    rdo.AMOUNT = item.AMOUNT;
                //    ListDetail.Add(rdo);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AddImpMestMaterialDetail()
        {
            try
            {
                //foreach (var item in listImpMestMaterialOn)
                //{
                //    DETAIL rdo = new DETAIL();
                //    rdo.IMP_MEST_CODE = item.IMP_MEST_CODE;
                //    rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    var expMestMaterial = listExpMestMaterialOn.FirstOrDefault(o => o.CK_IMP_MEST_MATERIAL_ID == item.ID);
                //    if (expMestMaterial != null)
                //    {
                //        rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == expMestMaterial.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    }
                //    rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                //    rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                //    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                //    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                //    rdo.AMOUNT = item.AMOUNT;
                //    ListDetail.Add(rdo);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AddExpMestMedicineDetail()
        {
            try
            {
                //foreach (var item in listExpMestMedicineOn)
                //{
                //    DETAIL rdo = new DETAIL();
                //    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                //    rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    if (item.CK_IMP_MEST_MEDICINE_ID != null)
                //    {
                //        var impMestMedicine = listImpMestMedicineOn.FirstOrDefault(o => o.ID == item.CK_IMP_MEST_MEDICINE_ID);
                //        if (impMestMedicine != null)
                //        {
                //            rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMestMedicine.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //        }
                //    }
                //    rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                //    rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                //    rdo.CONCENTRA = item.CONCENTRA;
                //    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                //    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                //    rdo.AMOUNT = item.AMOUNT;
                //    ListDetail.Add(rdo);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AddExpMestMaterialDetail()
        {
            try
            {
                //foreach (var item in listExpMestMaterialOn)
                //{
                //    DETAIL rdo = new DETAIL();
                //    rdo.EXP_MEST_CODE = item.EXP_MEST_CODE;
                //    rdo.EXP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //    if (item.CK_IMP_MEST_MATERIAL_ID != null)
                //    {
                //        var impMestMaterial = listImpMestMaterialOn.FirstOrDefault(o => o.ID == item.CK_IMP_MEST_MATERIAL_ID);
                //        if (impMestMaterial != null)
                //        {
                //            rdo.IMP_MEDI_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == impMestMaterial.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                //        }
                //    }
                //    rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                //    rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                //    rdo.IMP_PRICE_AFTER = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                //    rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                //    rdo.AMOUNT = item.AMOUNT;
                //    ListDetail.Add(rdo);
                //}
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessGroup()
        {
            try
            {
                //xử lý tạo key gộp theo điều kiện lọc
                string mainKeyGroup = ProcessMakeKeyGroup();

                //gộp thuốc vật tư theo key đã chọn ở trên
                GroupByKey(mainKeyGroup);

                //khử thuốc vật tư âm
                string IsNegativeAmountElimination = "";
                try
                {
                    IsNegativeAmountElimination = MANAGER.Config.IsNegativeLiminationCFG.IS_NEGATIVE_AMOUNT_ELIMINATION;
                }
                catch (Exception)
                {
                    LogSystem.Warn("can not load config MRS.IS_NEGATIVE_AMOUNT_ELIMINATION.");
                }
                if (IsNegativeAmountElimination == "1")
                {
                    ProcessNagetive();
                }

                AddInfo();

                AddInfoGroup(ListRdo);
                FixTypeMaterialChemical(ListRdo);
                ListRdo = FilterMedicineGroup(ListRdo);
                ListRdo = FilerMEdicineNational(ListRdo);//new
                ListRdo = FilterMedicineLine(ListRdo);

                if (castFilter.IS_MERGER_IMP_PRICE == true)
                {
                    RecoverImpPrice();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private string ProcessMakeKeyGroup()
        {
            //var group = ListRdo.GroupBy(o => string.Format(GroupKeyInv, o.MEDI_MATE_TYPE_ID, o.PACKAGE_NUMBER, castFilter.IS_NOT_ROUND_IMP_PRICE == true ? o.IMP_PRICE : Math.Round(o.IMP_PRICE,0), o.TYPE, o.MEDI_STOCK_ID, o.SUPPLIER_ID, o.EXPIRED_DATE, o.TDL_BID_NUMBER, o.MEDI_MATE_ID, o.SALE_PRICE, o.SALE_VAT_RATIO, o.IS_REUSABLING,o.KEY_GROUP_BID_DETAIL)).ToList();

            //mặc định key group là group theo đơn giá, kho, nhà cung cấp và hạn dùng
            string mainKeyGroup = "_{0}_{2}_{3}_{4}_{5}_{6}_{11}";

            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_INV") && this.dicDataFilter["KEY_GROUP_INV"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_INV"].ToString()))
            {
                mainKeyGroup = this.dicDataFilter["KEY_GROUP_INV"].ToString();
            }

            //nếu yêu cầu tách theo quyết định thầu thì thêm key tách quyết định thầu
            if (castFilter.IS_MERGE_BID_NUMBER == true)
            {
                mainKeyGroup = mainKeyGroup + "_{7}";
                //GroupByBidMumber();
            }

            //nếu có yêu cầu tổng hợp theo dịch vụ, đơn giá thì bỏ kho, nhà cung cấp hạn dùng ra khỏi key gộp
            if (castFilter.IS_MERGE_MEDI_MATE_TYPE_ID == true || castFilter.IS_GROUP == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{4}", "").Replace("_{5}", "").Replace("_{6}", "");
                //GroupByMediMateType();
            }

            //nếu có yêu cầu gộp nhà cung cấp
            if (castFilter.IS_MERGER_SUPPLIER == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{5}", "");
                //rdo.SUPPLIER_ID = 0;
            }

            //nếu có yêu cầu gộp kho
            if (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{4}", "");
                //rdo.MEDI_STOCK_ID = 0;
            }

            //nếu có yêu cầu gộp nhà cung cấp và hạn dùng
            if (castFilter.IS_MERGER == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{5}", "").Replace("_{6}", "");
                //rdo.EXPIRED_DATE = null;
                //rdo.SUPPLIER_ID = 0;
            }

            //nếu có yêu cầu tổng hợp theo dịch vụ và kho
            if (castFilter.IS_MERGE_CODE == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{2}", "").Replace("_{5}", "").Replace("_{6}", "");
                //rdo.EXPIRED_DATE = null;
                //rdo.SUPPLIER_ID = 0;
                //rdo.IMP_PRICE = 0;
            }

            //nếu có yêu cầu tổng hợp theo dịch vụ
            if (castFilter.IS_ONLLY_GROUP_SERVICE == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{2}", "").Replace("_{4}", "").Replace("_{5}", "").Replace("_{6}", "");
                //GroupByService();
            }

            //nếu có yêu cầu gộp đơn giá
            if (castFilter.IS_MERGER_IMP_PRICE == true)
            {
                mainKeyGroup = mainKeyGroup.Replace("_{2}", "");
                //rdo.IMP_PRICE = 0;
            }

            //nếu có yêu cầu tách theo số lô, hạn dùng, đơn giá, nhà cung cấp và kho
            if (this.castFilter.IS_MERGE_PACKAGE_NUMBER == true || castFilter.IS_MERGE_PRICE_EXPIRED_DATE_PACKAGE_NUMBER == true)
            {
                mainKeyGroup = mainKeyGroup + "_{0}" + "_{1}";
                //GroupByPackageNumber();
            }

            //nếu có yêu cầu đưa về mặc định
            if (castFilter.IS_MERGER_PRICE == true)
            {
                mainKeyGroup = "_{0}_{2}_{3}_{4}_{5}_{6}_{11}";
                //GroupByPrice();
            }
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_INV_FINAL") && this.dicDataFilter["KEY_GROUP_INV_FINAL"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_INV_FINAL"].ToString()))
            {
                mainKeyGroup = this.dicDataFilter["KEY_GROUP_INV_FINAL"].ToString();
            }
            return mainKeyGroup;
        }

        private void RecoverImpPrice()
        {
            foreach (var item in ListRdo)
            {
                if (item.IMP_PRICE == 0 && item.BEGIN_TOTAL_PRICE != 0 && item.BEGIN_AMOUNT > 0)
                {
                    item.IMP_PRICE = item.BEGIN_TOTAL_PRICE / item.BEGIN_AMOUNT;
                }
                else if (item.IMP_PRICE == 0 && item.END_TOTAL_PRICE != 0 && item.END_AMOUNT > 0)
                {
                    item.IMP_PRICE = item.END_TOTAL_PRICE / item.END_AMOUNT;
                }
                else if (item.IMP_PRICE == 0 && item.IMP_TOTAL_PRICE != 0 && item.IMP_TOTAL_AMOUNT > 0)
                {
                    item.IMP_PRICE = item.IMP_TOTAL_PRICE / item.IMP_TOTAL_AMOUNT;
                }
                else if (item.IMP_PRICE == 0 && item.EXP_TOTAL_PRICE != 0 && item.EXP_TOTAL_AMOUNT > 0)
                {
                    item.IMP_PRICE = item.EXP_TOTAL_PRICE / item.EXP_TOTAL_AMOUNT;
                }

            }
        }

        private List<Mrs00105RDO> FilterMedicineLine(List<Mrs00105RDO> ListRdo)
        {
            if (castFilter.INPUT_DATA_ID_MEDICINE_LINEs != null)
            {
                return ListRdo.Where(o => castFilter.INPUT_DATA_ID_MEDICINE_LINEs.Contains(o.MEDICINE_LINE_ID ?? 0)).ToList();
            }
            return ListRdo;
        }

        private List<Mrs00105RDO> FilterMedicineGroup(List<Mrs00105RDO> ListRdo)
        {
            if (castFilter.MEDICINE_GROUP_IDs != null)
            {
                return ListRdo.Where(o => castFilter.MEDICINE_GROUP_IDs.Contains(o.MEDICINE_GROUP_ID ?? 0)).ToList();
            }
            return ListRdo;
        }
        private List<Mrs00105RDO> FilerMEdicineNational(List<Mrs00105RDO> ListRdo)//New
        {
            if (castFilter.NATIONAL_NAME.HasValue)
            {
                if (castFilter.NATIONAL_NAME.Value == 0)
                {
                    ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != null).ToList();
                    ListRdo = ListRdo.Where(x => x.NATIONAL_NAME.Contains("Việt Nam")).ToList();
                }
                else if (castFilter.NATIONAL_NAME.Value == 1)
                {
                    ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != null).ToList();
                    ListRdo = ListRdo.Where(x => x.NATIONAL_NAME != "Việt Nam").ToList();
                }
            }
            else
            {
                return ListRdo;
            }
            return ListRdo;
        }
        //private List<Mrs00105RDO> FilterMedicineLine(List<Mrs00105RDO> ListRdo)
        //{
        //    if (castFilter.MEDICINE_LINE_IDs != null)
        //    {
        //        return ListRdo.Where(o => castFilter.MEDICINE_LINE_IDs.Contains(o.MEDICINE_LINE_ID ?? 0)).ToList();
        //    }
        //    return ListRdo;
        //}

        private void ProcessNagetive()
        {
            try
            {
                List<Mrs00105RDO> listNagetive = ListRdo.Where(o => o.BEGIN_AMOUNT < 0 || o.END_AMOUNT < 0).ToList();

                ListRdoB = ListRdo.Where(o => listNagetive.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && (p.MEDI_STOCK_ID == o.MEDI_STOCK_ID || (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true)) && p.TYPE == o.TYPE && p.IMP_PRICE == o.IMP_PRICE)).ToList();
                LogSystem.Info(String.Format("Negative: {0}", string.Join(";", ListRdoB.Select(o => string.Format("MEDI_STOCK_ID:{0}_MEDI_MATE_ID:{1}_TYPE:{2}_IMP_PRICE:{3}_BEGIN_AMOUNT:{4}_END_AMOUNT:{5}_MEDI_MATE_TYPE_ID:{6}", o.MEDI_STOCK_ID, o.MEDI_MATE_ID, o.TYPE, o.IMP_PRICE, o.BEGIN_AMOUNT, o.END_AMOUNT, o.MEDI_MATE_TYPE_ID)).ToList())));

                ListRdo = ListRdo.Where(o => !listNagetive.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && (p.MEDI_STOCK_ID == o.MEDI_STOCK_ID || (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true)) && p.TYPE == o.TYPE && p.IMP_PRICE == o.IMP_PRICE)).ToList();
                GroupByType();
                ListRdo.AddRange(ListRdoB);
                ProcessNagetiveTrunc();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessNagetiveTrunc()
        {
            try
            {
                List<Mrs00105RDO> listNagetive = ListRdo.Where(o => o.BEGIN_AMOUNT < 0 || o.END_AMOUNT < 0).ToList();
                ListRdoB = ListRdo.Where(o => listNagetive.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && (p.MEDI_STOCK_ID == o.MEDI_STOCK_ID || (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true)) && p.TYPE == o.TYPE && Math.Truncate(p.IMP_PRICE) == Math.Truncate(o.IMP_PRICE))).ToList();

                LogSystem.Info(String.Format("Negative: {0}", string.Join(";", ListRdoB.Select(o => string.Format("MEDI_STOCK_ID:{0}_MEDI_MATE_ID:{1}_TYPE:{2}_IMP_PRICE:{3}_BEGIN_AMOUNT:{4}_END_AMOUNT:{5}_MEDI_MATE_TYPE_ID:{6}", o.MEDI_STOCK_ID, o.MEDI_MATE_ID, o.TYPE, o.IMP_PRICE, o.BEGIN_AMOUNT, o.END_AMOUNT, o.MEDI_MATE_TYPE_ID)).ToList())));

                ListRdo = ListRdo.Where(o => !listNagetive.Exists(p => p.MEDI_MATE_TYPE_ID == o.MEDI_MATE_TYPE_ID && (p.MEDI_STOCK_ID == o.MEDI_STOCK_ID || (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true)) && p.TYPE == o.TYPE && Math.Truncate(p.IMP_PRICE) == Math.Truncate(o.IMP_PRICE))).ToList();
                GroupByTypeTrunc();
                ListRdo.AddRange(ListRdoB);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupByKey(string GroupKeyInv)
        {
            var group = dicRdo.Values.GroupBy(o => string.Format(GroupKeyInv, o.MEDI_MATE_TYPE_ID, o.PACKAGE_NUMBER, castFilter.IS_NOT_ROUND_IMP_PRICE == true ? o.IMP_PRICE : Math.Round(o.IMP_PRICE, 0), o.TYPE, o.MEDI_STOCK_ID, o.SUPPLIER_ID, o.EXPIRED_DATE, o.TDL_BID_NUMBER, o.MEDI_MATE_ID, o.SALE_PRICE, o.SALE_VAT_RATIO, o.IS_REUSABLING, o.KEY_GROUP_BID_DETAIL)).ToList();
            ListRdo.Clear();
            Decimal sum = 0;
            Mrs00105RDO rdo;
            List<Mrs00105RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00105RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00105RDO();
                listSub = item.ToList<Mrs00105RDO>();
                foreach (var i in listSub)
                {
                    if (i.DIC_EXP_MEST_REASON != null)
                    {
                        if (rdo.DIC_EXP_MEST_REASON == null)
                        {
                            rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_MEST_REASON)
                        {
                            if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_EXP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_EXP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_EXP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_EXP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_IMP_MEDI_STOCK == null)
                        {
                            rdo.DIC_IMP_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_MEDI_STOCK)
                        {
                            if (rdo.DIC_IMP_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_CHMS_MEDI_STOCK == null)
                        {
                            rdo.DIC_CHMS_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_MEDI_STOCK)
                        {
                            if (rdo.DIC_CHMS_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_IMP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_IMP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_IMP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_CHMS_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_CHMS_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_CHMS_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }
                }

                if (rdo.DIC_MEDI_STOCK == null)
                {
                    rdo.DIC_MEDI_STOCK = new Dictionary<string, decimal>();
                }

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT") || field.Name.Contains("_TOTAL_PRICE"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);

                        //tạo dic số lượng, thành tiền các kho theo mã kho khi bị gộp kho

                        if (!GroupKeyInv.Contains("_{4}"))
                        {
                            foreach (var ms in listSub.GroupBy(o => o.MEDI_STOCK_CODE))
                            {
                                if (!rdo.DIC_MEDI_STOCK.ContainsKey(string.Format("{0}_{1}", ms.Key, field.Name)))
                                {
                                    rdo.DIC_MEDI_STOCK[string.Format("{0}_{1}", ms.Key, field.Name)] = ms.Sum(s => (Decimal)field.GetValue(s));
                                }
                                else
                                {
                                    rdo.DIC_MEDI_STOCK[string.Format("{0}_{1}", ms.Key, field.Name)] += ms.Sum(s => (Decimal)field.GetValue(s));
                                }
                            }

                        }
                    }
                    else if (!field.Name.Contains("DIC_EXP_MEST_REASON") && !field.Name.Contains("DIC_EXP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_IMP_MEDI_STOCK") && !field.Name.Contains("DIC_CHMS_MEDI_STOCK") && !field.Name.Contains("DIC_IMP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_CHMS_REQ_DEPARTMENT") && !field.Name.Contains("PreviousMediStockImpAmounts") && !field.Name.Contains("PreviousMediStockExpAmounts"))
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }

                //sửa lại kho về 0
                if (!GroupKeyInv.Contains("_{4}"))
                {
                    rdo.MEDI_STOCK_ID = 0;
                }
                if (!hide) ListRdo.Add(rdo);
            }
        }

        private void GroupByType()
        {
            //try
            //{
            var group = ListRdoB.GroupBy(o => string.Format("{0}_{1}_{2}_{3}", o.MEDI_MATE_TYPE_ID, o.TYPE, (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true) ? 0 : o.MEDI_STOCK_ID, o.IMP_PRICE)).ToList();
            ListRdoB.Clear();
            Decimal sum = 0;
            Mrs00105RDO rdo;
            List<Mrs00105RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00105RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00105RDO();
                listSub = item.ToList<Mrs00105RDO>();
                foreach (var i in listSub)
                {
                    if (IsNotNullOrEmpty(i.NATIONAL_NAME) && i.NATIONAL_NAME.ToLower() == "việt nam")
                    {
                        rdo.MEDICINE_TYPE_NATIONAL = "Thuốc nội";
                    }
                    else if (IsNotNullOrEmpty(i.NATIONAL_NAME))
                    {
                        rdo.MEDICINE_TYPE_NATIONAL = "Thuốc ngoại";
                    }

                    rdo.BID_TYPE = "Trong thầu";
                    if (i.DIC_EXP_MEST_REASON != null)
                    {
                        if (rdo.DIC_EXP_MEST_REASON == null)
                        {
                            rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_MEST_REASON)
                        {
                            if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_EXP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_EXP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_EXP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_EXP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_IMP_MEDI_STOCK == null)
                        {
                            rdo.DIC_IMP_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_MEDI_STOCK)
                        {
                            if (rdo.DIC_IMP_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_CHMS_MEDI_STOCK == null)
                        {
                            rdo.DIC_CHMS_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_MEDI_STOCK)
                        {
                            if (rdo.DIC_CHMS_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_IMP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_IMP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_IMP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_CHMS_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_CHMS_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_CHMS_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }
                }

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT") || field.Name.Contains("_TOTAL_PRICE"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else if (!field.Name.Contains("DIC_EXP_MEST_REASON") && !field.Name.Contains("DIC_EXP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_IMP_MEDI_STOCK") && !field.Name.Contains("DIC_CHMS_MEDI_STOCK") && !field.Name.Contains("DIC_IMP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_CHMS_REQ_DEPARTMENT") && !field.Name.Contains("PreviousMediStockImpAmounts") && !field.Name.Contains("PreviousMediStockExpAmounts"))
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }
                if (!hide) ListRdoB.Add(rdo);
            }
        }

        private void GroupByTypeTrunc()
        {
            //try
            //{
            var group = ListRdoB.GroupBy(o => string.Format("{0}_{1}_{2}_{3}", o.MEDI_MATE_TYPE_ID, o.TYPE, (castFilter.IS_MERGE_STOCK == true || castFilter.IS_MERGER_MEDI_STOCK == true || castFilter.IS_MERGE == true) ? 0 : o.MEDI_STOCK_ID, Math.Truncate(o.IMP_PRICE))).ToList();
            ListRdoB.Clear();
            Decimal sum = 0;
            Mrs00105RDO rdo;
            List<Mrs00105RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00105RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00105RDO();
                listSub = item.ToList<Mrs00105RDO>();
                foreach (var i in listSub)
                {
                    if(IsNotNullOrEmpty(i.NATIONAL_NAME) && i.NATIONAL_NAME.ToLower() == "việt nam")
                    {
                        rdo.MEDICINE_TYPE_NATIONAL = "Thuốc nội";
                    }
                    else if(IsNotNullOrEmpty(i.NATIONAL_NAME))
                    {
                        rdo.MEDICINE_TYPE_NATIONAL = "Thuốc ngoại";
                    }

                    rdo.BID_TYPE = "Trong thầu";
                    if (i.DIC_EXP_MEST_REASON != null)
                    {
                        if (rdo.DIC_EXP_MEST_REASON == null)
                        {
                            rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_MEST_REASON)
                        {
                            if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_EXP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_EXP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_EXP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_EXP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_EXP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_EXP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_IMP_MEDI_STOCK == null)
                        {
                            rdo.DIC_IMP_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_MEDI_STOCK)
                        {
                            if (rdo.DIC_IMP_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_MEDI_STOCK != null)
                    {
                        if (rdo.DIC_CHMS_MEDI_STOCK == null)
                        {
                            rdo.DIC_CHMS_MEDI_STOCK = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_MEDI_STOCK)
                        {
                            if (rdo.DIC_CHMS_MEDI_STOCK.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_MEDI_STOCK[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_MEDI_STOCK.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_IMP_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_IMP_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_IMP_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_IMP_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_IMP_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_IMP_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }

                    if (i.DIC_CHMS_REQ_DEPARTMENT != null)
                    {
                        if (rdo.DIC_CHMS_REQ_DEPARTMENT == null)
                        {
                            rdo.DIC_CHMS_REQ_DEPARTMENT = new Dictionary<string, decimal>();
                        }
                        foreach (var dc in i.DIC_CHMS_REQ_DEPARTMENT)
                        {
                            if (rdo.DIC_CHMS_REQ_DEPARTMENT.ContainsKey(dc.Key))
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT[dc.Key] += dc.Value;
                            }
                            else
                            {
                                rdo.DIC_CHMS_REQ_DEPARTMENT.Add(dc.Key, dc.Value);
                            }
                        }
                    }
                }

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT") || field.Name.Contains("_TOTAL_PRICE"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else if (!field.Name.Contains("DIC_EXP_MEST_REASON") && !field.Name.Contains("DIC_EXP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_IMP_MEDI_STOCK") && !field.Name.Contains("DIC_CHMS_MEDI_STOCK") && !field.Name.Contains("DIC_IMP_REQ_DEPARTMENT") && !field.Name.Contains("DIC_CHMS_REQ_DEPARTMENT") && !field.Name.Contains("PreviousMediStockImpAmounts") && !field.Name.Contains("PreviousMediStockExpAmounts"))
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                    }
                }
                if (!hide) ListRdoB.Add(rdo);
            }
        }

        private Mrs00105RDO IsMeaningful(List<Mrs00105RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").OrderByDescending(p => field.GetValue(p)).FirstOrDefault() ?? new Mrs00105RDO();
        }

        private void AddInfo()
        {
            foreach (var item in ListRdo)
            {
                if (castFilter.EXP_MEDI_STOCK_IDs != null)
                {
                    var ImpMediStock = HisMediStockCFG.HisMediStocks.Where(o => castFilter.EXP_MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                    if (item.TYPE == THUOC)
                    {
                        var mediStockMety = listMediStockMety.Where(o => o.TYPE == THUOC && (o.MEDI_STOCK_ID == item.MEDI_STOCK_ID || item.MEDI_STOCK_ID == 0) && o.MEDI_MATE_TYPE_ID == item.MEDI_MATE_TYPE_ID).ToList();
                        if (mediStockMety.Count > 0)
                        {
                            //item.EXP_MEDI_STOCK_ID = mediStockMety.EXP_MEDI_STOCK_ID??0;
                            var mediStock = HisMediStockCFG.HisMediStocks.Where(o => mediStockMety.Exists(p => p.EXP_MEDI_STOCK_ID == o.ID)).ToList();
                            if (mediStock.Count > 0)
                            {
                                item.EXP_MEDI_STOCK_CODE = string.Join(";", mediStock.Select(o => o.MEDI_STOCK_CODE).ToList());
                                item.EXP_MEDI_STOCK_NAME = string.Join(";", mediStock.Select(o => o.MEDI_STOCK_NAME).ToList());
                            }
                        }
                    }
                    if (item.TYPE == VATTU || item.TYPE == 3)
                    {
                        var mediStockMaty = listMediStockMaty.Where(o => o.TYPE == VATTU && (o.MEDI_STOCK_ID == item.MEDI_STOCK_ID || item.MEDI_STOCK_ID == 0) && o.MEDI_MATE_TYPE_ID == item.MEDI_MATE_TYPE_ID).ToList();
                        if (mediStockMaty.Count > 0)
                        {
                            //item.EXP_MEDI_STOCK_ID = mediStockMaty.EXP_MEDI_STOCK_ID??0;
                            var mediStock = HisMediStockCFG.HisMediStocks.Where(o => mediStockMaty.Exists(p => p.EXP_MEDI_STOCK_ID == o.ID)).ToList();
                            if (mediStock.Count > 0)
                            {
                                item.EXP_MEDI_STOCK_CODE = string.Join(";", mediStock.Select(o => o.MEDI_STOCK_CODE).ToList());
                                item.EXP_MEDI_STOCK_NAME = string.Join(";", mediStock.Select(o => o.MEDI_STOCK_NAME).ToList());
                            }
                        }
                    }
                }

                item.SERVICE_TYPE_NAME = item.TYPE == 1 ? "THUỐC" : (item.TYPE == 2 ? "VẬT TƯ" : (item.TYPE == 3 ? "HÓA CHẤT" : "MÁU"));
                item.VAT_RATIO_STR = string.Format("'{0}%", (long)(100 * item.VAT_RATIO ?? 0));
                item.INPUT_END_AMOUNT = item.TYPE == 1 ? InputMedicines.Where(o => o.MEDICINE_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && (o.EXPIRED_DATE ?? 0) == (item.EXPIRED_DATE ?? 0)).Sum(s => s.AMOUNT) : InputMaterials.Where(o => o.MATERIAL_TYPE_ID == item.MEDI_MATE_TYPE_ID && o.IMP_PRICE * (1 + o.IMP_VAT_RATIO) == item.IMP_PRICE && o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && (o.EXPIRED_DATE ?? 0) == (item.EXPIRED_DATE ?? 0)).Sum(s => s.AMOUNT);
                if (IsNotNullOrEmpty(item.NATIONAL_NAME) && item.NATIONAL_NAME.ToLower() == "việt nam")
                {
                    item.MEDICINE_TYPE_NATIONAL = "Thuốc nội";
                }
                else if (IsNotNullOrEmpty(item.NATIONAL_NAME))
                {
                    item.MEDICINE_TYPE_NATIONAL = "Thuốc ngoại";
                }
                item.BID_TYPE = "Trong thầu";
                AddInfoSupplier(item);
                AddSalePrice(item);
                AddBidImp(item);
            }

            //thêm thông tin thầu của các thuốc không có nhập xuất tồn
            AddInfoBidMetyMateBlood();

            //thêm thông tin dự trù
            AddInfoAnticipate();
        }

        private void AddSalePrice(Mrs00105RDO item)
        {
            if (item.SALE_PRICE == 0)
            {
                if (item.IS_SALE_EQUAL_IMP_PRICE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    item.SALE_PRICE = item.IMP_PRICE;
                    item.SALE_VAT_RATIO = 0;
                }
                else
                {
                    var saleProfitCfg = (item.TYPE != 1) ? SaleProfitCfgs.FirstOrDefault(o => o.IS_MATERIAL == 1 && item.IMP_PRICE >= o.IMP_PRICE_FROM && item.IMP_PRICE < o.IMP_PRICE_TO) : SaleProfitCfgs.FirstOrDefault(o => o.IS_MEDICINE == 1 && item.IMP_PRICE >= o.IMP_PRICE_FROM && item.IMP_PRICE < o.IMP_PRICE_TO);
                    var medicinePaty = MedicinePatys.FirstOrDefault(o => o.MEDICINE_ID == item.MEDI_MATE_ID && item.TYPE == 1);
                    var materialPaty = MaterialPatys.FirstOrDefault(o => o.MATERIAL_ID == item.MEDI_MATE_ID && item.TYPE != 1);
                    if (saleProfitCfg != null && saleProfitCfg.RATIO > 0 && medicinePaty == null && materialPaty == null)
                    {
                        item.SALE_PRICE = item.IMP_PRICE * (100 + saleProfitCfg.RATIO) / 100;
                        item.SALE_VAT_RATIO = 0;
                    }
                    else
                    {
                        if (item.TYPE == 1)
                        {

                            if (medicinePaty != null && medicinePaty.EXP_PRICE > 0)
                            {
                                item.SALE_PRICE = medicinePaty.EXP_PRICE;
                                item.SALE_VAT_RATIO = medicinePaty.EXP_VAT_RATIO;
                            }
                            else
                            {
                                item.SALE_PRICE = item.IMP_PRICE;
                                item.SALE_VAT_RATIO = 0;
                            }
                        }
                        else
                        {

                            if (materialPaty != null && materialPaty.EXP_PRICE > 0)
                            {
                                item.SALE_PRICE = materialPaty.EXP_PRICE;
                                item.SALE_VAT_RATIO = materialPaty.EXP_VAT_RATIO;
                            }
                            else
                            {
                                item.SALE_PRICE = item.IMP_PRICE;
                                item.SALE_VAT_RATIO = 0;
                            }
                        }
                    }
                }
            }
            if (this.Suppliers != null)
            {
                var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                if (supplier != null)
                {
                    item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                    item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                    item.SUPPLIER_ADDRESS = supplier.ADDRESS;
                    item.SUPPLIER_PHONE = supplier.PHONE;
                }
            }
        }

        private void AddInfoBidMetyMateBlood()
        {
            try
            {
                if (castFilter.ADD_BID_DETAIL_NO_IMP == true && (castFilter.BID_ID != null || castFilter.BID_IDs != null))
                {
                    foreach (var item in ListBidMety)
                    {
                        Mrs00105RDO rdo = new Mrs00105RDO();
                        var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == item.BID_ID);
                        if (bid != null)
                        {
                            if (string.IsNullOrEmpty(rdo.TDL_BID_NUMBER))
                            {
                                rdo.TDL_BID_NUMBER = bid.BID_NUMBER;
                            }

                            rdo.BID_NAME = bid.BID_NAME;
                            rdo.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                            rdo.VALID_TO_TIME = bid.VALID_TO_TIME;
                            rdo.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.MEDICINE_TYPE_ID, THUOC, item.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                            rdo.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                        }
                        var IsExisted = ListRdo.FirstOrDefault(o => o.KEY_GROUP_BID_DETAIL == rdo.KEY_GROUP_BID_DETAIL);

                        if (IsExisted != null)
                        {
                            continue;
                        }
                        else
                        {
                            rdo.TYPE = 1;
                            rdo.MEDI_MATE_TYPE_ID = item.MEDICINE_TYPE_ID;
                            rdo.BID_INCREATE_AMOUNT = item.AMOUNT;
                            rdo.BID_ADJUST_AMOUNT = item.ADJUST_AMOUNT ?? 0;
                            rdo.BID_NUM_ORDER = item.BID_NUM_ORDER;
                            rdo.IMP_PRICE = (item.IMP_PRICE ?? 0) * (1 + (item.IMP_VAT_RATIO ?? 0));
                            if (dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                            {
                                rdo.SERVICE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                                rdo.SERVICE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].SERVICE_UNIT_NAME;
                                rdo.ATC_CODES = dicMedicineType[item.MEDICINE_TYPE_ID].ATC_CODES;
                                rdo.CONCENTRA = dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA;
                                rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                                rdo.MANUFACTURER_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                                rdo.HEIN_SERVICE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                                rdo.HEIN_SERVICE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                                rdo.HEIN_SERVICE_TYPE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_TYPE_CODE;
                                rdo.HEIN_SERVICE_TYPE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_TYPE_NAME;
                                rdo.ACTIVE_INGR_BHYT_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                                rdo.ACTIVE_INGR_BHYT_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                                rdo.PACKING_TYPE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                                rdo.REGISTER_NUMBER = dicMedicineType[item.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                                rdo.MEDICINE_USE_FORM_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                                rdo.MEDICINE_USE_FORM_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                                rdo.BYT_NUM_ORDER = dicMedicineType[item.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                                rdo.TCY_NUM_ORDER = dicMedicineType[item.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                                rdo.SCIENTIFIC_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].SCIENTIFIC_NAME;
                                rdo.PREPROCESSING = dicMedicineType[item.MEDICINE_TYPE_ID].PREPROCESSING;
                                rdo.PROCESSING = dicMedicineType[item.MEDICINE_TYPE_ID].PROCESSING;
                                rdo.USED_PART = dicMedicineType[item.MEDICINE_TYPE_ID].USED_PART;
                                rdo.DOSAGE_FORM = dicMedicineType[item.MEDICINE_TYPE_ID].DOSAGE_FORM;
                                rdo.DISTRIBUTED_SL = dicMedicineType[item.MEDICINE_TYPE_ID].DISTRIBUTED_AMOUNT;
                                rdo.QUALITY_STANDARDS = dicMedicineType[item.MEDICINE_TYPE_ID].QUALITY_STANDARDS;
                                rdo.SOURCE_MEDICINE = dicMedicineType[item.MEDICINE_TYPE_ID].SOURCE_MEDICINE ?? 0;
                                

                            }

                            rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                            if (this.Suppliers != null)
                            {
                                var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                                if (supplier != null)
                                {
                                    rdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                                    rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                                    rdo.SUPPLIER_ADDRESS = supplier.ADDRESS;
                                    rdo.SUPPLIER_PHONE = supplier.PHONE;
                                }
                            }

                            AddBidImp(rdo);
                            ListRdo.Add(rdo);
                        }
                    }

                    foreach (var item in ListBidMaty)
                    {
                        Mrs00105RDO rdo = new Mrs00105RDO();
                        var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == item.BID_ID);
                        if (bid != null)
                        {
                            if (string.IsNullOrEmpty(rdo.TDL_BID_NUMBER))
                            {
                                rdo.TDL_BID_NUMBER = bid.BID_NUMBER;
                            }

                            rdo.BID_NAME = bid.BID_NAME;
                            rdo.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                            rdo.VALID_TO_TIME = bid.VALID_TO_TIME;
                            rdo.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.MATERIAL_TYPE_ID, VATTU, item.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                            rdo.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                        }
                        var IsExisted = ListRdo.FirstOrDefault(o => o.KEY_GROUP_BID_DETAIL == rdo.KEY_GROUP_BID_DETAIL);
                        if (IsExisted != null)
                        {
                            continue;
                        }
                        else
                        {
                            rdo.TYPE = 2;
                            rdo.MEDI_MATE_TYPE_ID = item.MATERIAL_TYPE_ID ?? 0;
                            rdo.BID_INCREATE_AMOUNT = item.AMOUNT;
                            rdo.BID_ADJUST_AMOUNT = item.ADJUST_AMOUNT ?? 0;
                            rdo.BID_NUM_ORDER = item.BID_NUM_ORDER;
                            rdo.IMP_PRICE = (item.IMP_PRICE ?? 0) * (1 + (item.IMP_VAT_RATIO ?? 0));
                            if (dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID ?? 0))
                            {
                                rdo.SERVICE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].MATERIAL_TYPE_CODE;
                                rdo.SERVICE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].MATERIAL_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].SERVICE_UNIT_NAME;
                                rdo.CONCENTRA = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].CONCENTRA;
                                rdo.MANUFACTURER_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].MANUFACTURER_NAME;
                                rdo.HEIN_SERVICE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].HEIN_SERVICE_BHYT_CODE;
                                rdo.HEIN_SERVICE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].HEIN_SERVICE_BHYT_NAME;
                                rdo.HEIN_SERVICE_TYPE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].HEIN_SERVICE_TYPE_CODE;
                                rdo.HEIN_SERVICE_TYPE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].HEIN_SERVICE_TYPE_NAME;
                                rdo.PACKING_TYPE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID ?? 0].PACKING_TYPE_NAME;
                            }

                            rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                            if (this.Suppliers != null)
                            {
                                var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                                if (supplier != null)
                                {
                                    rdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                                    rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                                    rdo.SUPPLIER_ADDRESS = supplier.ADDRESS;
                                    rdo.SUPPLIER_PHONE = supplier.PHONE;
                                }
                            }
                            AddBidImp(rdo);
                            ListRdo.Add(rdo);
                        }
                    }

                    foreach (var item in ListBidBlty)
                    {
                        Mrs00105RDO rdo = new Mrs00105RDO();
                        var bid = (this.listBid ?? new List<HIS_BID>()).FirstOrDefault(o => o.ID == item.BID_ID);
                        if (bid != null)
                        {
                            if (string.IsNullOrEmpty(rdo.TDL_BID_NUMBER))
                            {
                                rdo.TDL_BID_NUMBER = bid.BID_NUMBER;
                            }

                            rdo.BID_NAME = bid.BID_NAME;
                            rdo.VALID_FROM_TIME = bid.VALID_FROM_TIME;
                            rdo.VALID_TO_TIME = bid.VALID_TO_TIME;
                            rdo.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, item.BLOOD_TYPE_ID, MAU, item.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                            rdo.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE);
                        }
                        var IsExisted = ListRdo.FirstOrDefault(o => o.KEY_GROUP_BID_DETAIL == rdo.KEY_GROUP_BID_DETAIL);
                        if (IsExisted != null)
                        {
                            continue;
                        }
                        else
                        {
                            rdo.TYPE = 2;
                            rdo.MEDI_MATE_TYPE_ID = item.BLOOD_TYPE_ID;
                            rdo.BID_INCREATE_AMOUNT = item.AMOUNT;
                            rdo.BID_ADJUST_AMOUNT = 0;
                            rdo.BID_NUM_ORDER = item.BID_NUM_ORDER;
                            rdo.IMP_PRICE = (item.IMP_PRICE ?? 0) * (1 + (item.IMP_VAT_RATIO ?? 0));
                            if (dicBloodType.ContainsKey(item.BLOOD_TYPE_ID))
                            {
                                rdo.SERVICE_CODE = dicBloodType[item.BLOOD_TYPE_ID].BLOOD_TYPE_CODE;
                                rdo.SERVICE_NAME = dicBloodType[item.BLOOD_TYPE_ID].BLOOD_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = dicBloodType[item.BLOOD_TYPE_ID].SERVICE_UNIT_NAME;
                                rdo.HEIN_SERVICE_CODE = dicBloodType[item.BLOOD_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                                rdo.HEIN_SERVICE_NAME = dicBloodType[item.BLOOD_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                                rdo.HEIN_SERVICE_TYPE_CODE = dicBloodType[item.BLOOD_TYPE_ID].HEIN_SERVICE_TYPE_CODE;
                                rdo.HEIN_SERVICE_TYPE_NAME = dicBloodType[item.BLOOD_TYPE_ID].HEIN_SERVICE_TYPE_NAME;
                                rdo.PACKING_TYPE_NAME = dicBloodType[item.BLOOD_TYPE_ID].PACKING_TYPE_NAME;
                            }

                            rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                            if (this.Suppliers != null)
                            {
                                var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                                if (supplier != null)
                                {
                                    rdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                                    rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                                    rdo.SUPPLIER_ADDRESS = supplier.ADDRESS;
                                    rdo.SUPPLIER_PHONE = supplier.PHONE;
                                }
                            }
                            AddBidImp(rdo);
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private void AddInfoAnticipate()
        {
            try
            {
                foreach (var item in AnticipateMetys)
                {
                    Mrs00105RDO rdo = ListRdo.FirstOrDefault(o => o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && item.MEDICINE_TYPE_ID == o.MEDI_MATE_TYPE_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && o.TYPE == 1);

                    if (rdo != null)
                    {
                        rdo.ANTICIPATE_AMOUNT = item.AMOUNT ?? 0;
                    }
                    else
                    {
                        rdo = new Mrs00105RDO();
                        rdo.TYPE = 1;
                        rdo.ANTICIPATE_AMOUNT = item.AMOUNT ?? 0;
                        rdo.MEDI_MATE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        if (dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID))
                        {
                            rdo.SERVICE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_CODE;
                            rdo.SERVICE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_NAME;
                            rdo.ATC_CODES = dicMedicineType[item.MEDICINE_TYPE_ID].ATC_CODES;
                            rdo.CONCENTRA = dicMedicineType[item.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                            rdo.HEIN_SERVICE_TYPE_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_TYPE_CODE;
                            rdo.HEIN_SERVICE_TYPE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].HEIN_SERVICE_TYPE_NAME;
                            rdo.ACTIVE_INGR_BHYT_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].ACTIVE_INGR_BHYT_NAME;
                            rdo.PACKING_TYPE_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].PACKING_TYPE_NAME;
                            rdo.REGISTER_NUMBER = dicMedicineType[item.MEDICINE_TYPE_ID].REGISTER_NUMBER;
                            rdo.MEDICINE_USE_FORM_CODE = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_CODE;
                            rdo.MEDICINE_USE_FORM_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].MEDICINE_USE_FORM_NAME;
                            rdo.BYT_NUM_ORDER = dicMedicineType[item.MEDICINE_TYPE_ID].BYT_NUM_ORDER;
                            rdo.TCY_NUM_ORDER = dicMedicineType[item.MEDICINE_TYPE_ID].TCY_NUM_ORDER;
                            rdo.SCIENTIFIC_NAME = dicMedicineType[item.MEDICINE_TYPE_ID].SCIENTIFIC_NAME;
                            rdo.PREPROCESSING = dicMedicineType[item.MEDICINE_TYPE_ID].PREPROCESSING;
                            rdo.PROCESSING = dicMedicineType[item.MEDICINE_TYPE_ID].PROCESSING;
                            rdo.USED_PART = dicMedicineType[item.MEDICINE_TYPE_ID].USED_PART;
                            rdo.DOSAGE_FORM = dicMedicineType[item.MEDICINE_TYPE_ID].DOSAGE_FORM;
                            rdo.DISTRIBUTED_SL = dicMedicineType[item.MEDICINE_TYPE_ID].DISTRIBUTED_AMOUNT;
                            rdo.QUALITY_STANDARDS = dicMedicineType[item.MEDICINE_TYPE_ID].QUALITY_STANDARDS;
                            rdo.SOURCE_MEDICINE = dicMedicineType[item.MEDICINE_TYPE_ID].SOURCE_MEDICINE ?? 0;
                            
                        }

                        rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID ?? 0;
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        if (mediStock != null)
                        {
                            rdo.MEDI_STOCK_CODE = mediStock.MEDI_STOCK_CODE;
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 1;
                            //}
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NGTs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 2;
                            //}
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__CLSs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 3;
                            //}
                            rdo.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                            rdo.DEPARTMENT_NAME = mediStock.DEPARTMENT_NAME;
                            rdo.IS_CABINET = mediStock.IS_CABINET;
                        }
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        if (this.Suppliers != null)
                        {
                            var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                            if (supplier != null)
                            {
                                rdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                                rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                                rdo.SUPPLIER_ADDRESS = supplier.ADDRESS;
                                rdo.SUPPLIER_PHONE = supplier.PHONE;
                            }
                        }
                        ListRdo.Add(rdo);
                    }
                }

                foreach (var item in AnticipateMatys)
                {
                    Mrs00105RDO rdo = ListRdo.FirstOrDefault(o => o.MEDI_STOCK_ID == item.MEDI_STOCK_ID && item.MATERIAL_TYPE_ID == o.MEDI_MATE_TYPE_ID && o.SUPPLIER_ID == item.SUPPLIER_ID && o.TYPE == 2);

                    if (rdo != null)
                    {
                        rdo.ANTICIPATE_AMOUNT = item.AMOUNT ?? 0;
                    }
                    else
                    {
                        rdo = new Mrs00105RDO();
                        rdo.TYPE = 2;
                        rdo.ANTICIPATE_AMOUNT = item.AMOUNT ?? 0;
                        rdo.MEDI_MATE_TYPE_ID = item.MATERIAL_TYPE_ID;
                        if (dicMaterialType.ContainsKey(item.MATERIAL_TYPE_ID))
                        {
                            rdo.SERVICE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID].MATERIAL_TYPE_NAME;
                            rdo.CONCENTRA = dicMaterialType[item.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[item.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                            rdo.HEIN_SERVICE_TYPE_CODE = dicMaterialType[item.MATERIAL_TYPE_ID].HEIN_SERVICE_TYPE_CODE;
                            rdo.HEIN_SERVICE_TYPE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID].HEIN_SERVICE_TYPE_NAME;
                            rdo.PACKING_TYPE_NAME = dicMaterialType[item.MATERIAL_TYPE_ID].PACKING_TYPE_NAME;
                        }

                        rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID ?? 0;
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == item.MEDI_STOCK_ID);
                        if (mediStock != null)
                        {
                            rdo.MEDI_STOCK_CODE = mediStock.MEDI_STOCK_CODE;
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NTs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 1;
                            //}
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__NGTs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 2;
                            //}
                            //if (string.Format(",{0},", this.castFilter.MEDI_STOCK_CODE__CLSs).Contains(string.Format(",{0},", rdo.MEDI_STOCK_CODE)))
                            //{
                            //    rdo.IS_NT_NGT_CLS = 3;
                            //}
                            rdo.MEDI_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                            rdo.DEPARTMENT_NAME = mediStock.DEPARTMENT_NAME;
                            rdo.IS_CABINET = mediStock.IS_CABINET;
                        }
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        if (this.Suppliers != null)
                        {
                            var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                            if (supplier != null)
                            {
                                rdo.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                                rdo.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                                rdo.SUPPLIER_ADDRESS = supplier.ADDRESS;
                                rdo.SUPPLIER_PHONE = supplier.PHONE;
                            }
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private void AddInfoSupplier(Mrs00105RDO item)
        {
            if (this.Suppliers != null)
            {
                var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

                if (supplier != null)
                {
                    item.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                    item.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                    item.SUPPLIER_ADDRESS = supplier.ADDRESS;
                    item.SUPPLIER_PHONE = supplier.PHONE;
                }
            }
        }

        private void AddBidImp(Mrs00105RDO item)
        {
            if (dicBidImpMedi.ContainsKey(item.KEY_GROUP_BID_DETAIL ?? ""))
            {
                var bidImp = this.dicBidImpMedi[item.KEY_GROUP_BID_DETAIL ?? ""];

                if (bidImp != null)
                {
                    item.BID_INCREATE_AMOUNT = bidImp.BID_AMOUNT ?? 0;
                    item.BID_ADJUST_AMOUNT = bidImp.BID_ADJUST_AMOUNT ?? 0;
                    item.BID_DECREATE_AMOUNT = bidImp.BID_IMP_AMOUNT ?? 0;
                }
            }
            if (dicBidImpMate.ContainsKey(item.KEY_GROUP_BID_DETAIL ?? ""))
            {
                var bidImp = this.dicBidImpMate[item.KEY_GROUP_BID_DETAIL ?? ""];

                if (bidImp != null)
                {
                    item.BID_INCREATE_AMOUNT = bidImp.BID_AMOUNT ?? 0;
                    item.BID_ADJUST_AMOUNT = bidImp.BID_ADJUST_AMOUNT ?? 0;
                    item.BID_DECREATE_AMOUNT = bidImp.BID_IMP_AMOUNT ?? 0;
                }
            }
            if (dicBidImpBlood.ContainsKey(item.KEY_GROUP_BID_DETAIL ?? ""))
            {
                var bidImp = this.dicBidImpBlood[item.KEY_GROUP_BID_DETAIL ?? ""];

                if (bidImp != null)
                {
                    item.BID_INCREATE_AMOUNT = bidImp.BID_AMOUNT ?? 0;
                    item.BID_ADJUST_AMOUNT = bidImp.BID_ADJUST_AMOUNT ?? 0;
                    item.BID_DECREATE_AMOUNT = bidImp.BID_IMP_AMOUNT ?? 0;
                }
            }
            var supplier = this.Suppliers.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);

            if (supplier != null)
            {
                item.BID_SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                item.BID_SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                item.BID_SUPPLIER_ADDRESS = supplier.ADDRESS;
                item.BID_SUPPLIER_PHONE = supplier.PHONE;
            }
            item.BID_AM = item.BID_INCREATE_AMOUNT.ToString();
        }

        private void AddInfoGroup(List<Mrs00105RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.TYPE == 1)
                {
                    var medicineType = dicMedicineType.Values.FirstOrDefault(o => o.ID == item.MEDI_MATE_TYPE_ID);

                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
                    }

                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID != null)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                    }
                    //neu gop theo nhom thuoc thi thay the SERVICE_TYPE_ID bang nhom thuoc

                    if (castFilter.IS_MEDICINE_GROUP == true)
                    {
                        if (item.MEDICINE_GROUP_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN && item.MEDICINE_GROUP_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT && item.MEDICINE_GROUP_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__DICH_TRUYEN)
                        {
                            item.MEDICINE_GROUP_ID = 0;
                            item.MEDICINE_GROUP_CODE = "NTK";
                            item.MEDICINE_GROUP_NAME = "Thuốc Thường";
                        }
                    }
                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = dicMedicineType.Values.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_NUM = parentMedicineType.NUM_ORDER ?? 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_NUM = 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_NUM = 9999;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                    }

                    if (medicineType != null && medicineType.RECORDING_TRANSACTION != null)
                    {
                        item.RECORDING_TRANSACTION = medicineType.RECORDING_TRANSACTION;
                    }
                }
                else if (item.TYPE == 2)
                {
                    var materialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == item.MEDI_MATE_TYPE_ID);

                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = dicMaterialType.Values.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_NUM = parentMaterialType.NUM_ORDER ?? 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_NUM = 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm vật tư khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_NUM = 9999;
                        item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm vật tư khác";
                    }

                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                    if (item.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.MEDICINE_LINE_CODE = "DHC";
                        item.MEDICINE_LINE_NAME = "Hóa chất";
                        item.MEDICINE_GROUP_CODE = "DHC";
                        item.MEDICINE_GROUP_NAME = "Hóa chất";
                    }
                    //item.PARENT_MEDICINE_TYPE_CODE = "DVT";
                    //item.PARENT_MEDICINE_TYPE_NAME = "Vật tư";
                    if (materialType != null && materialType.RECORDING_TRANSACTION != null)
                    {
                        item.RECORDING_TRANSACTION = materialType.RECORDING_TRANSACTION;
                    }
                }
                else if (item.TYPE == 4)
                {
                    var bloodType = dicBloodType.Values.FirstOrDefault(o => o.ID == item.MEDI_MATE_TYPE_ID);

                    if (bloodType != null && bloodType.PARENT_ID != null)
                    {
                        var parentBloodType = dicBloodType.Values.FirstOrDefault(o => o.ID == bloodType.PARENT_ID);
                        if (parentBloodType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentBloodType.ID;
                            item.PARENT_MEDICINE_TYPE_NUM = parentBloodType.NUM_ORDER ?? 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = parentBloodType.BLOOD_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentBloodType.BLOOD_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_NUM = 9999;
                            item.PARENT_MEDICINE_TYPE_CODE = "NMK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm máu khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_NUM = 9999;
                        item.PARENT_MEDICINE_TYPE_CODE = "NMK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm máu khác";
                    }

                    item.MEDICINE_LINE_CODE = "DM";
                    item.MEDICINE_LINE_NAME = "Máu";
                    item.MEDICINE_GROUP_CODE = "DM";
                    item.MEDICINE_GROUP_NAME = "Máu";
                }
            }
        }

        private void FixTypeMaterialChemical(List<Mrs00105RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                if (item.TYPE == 2)
                {
                    if (item.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.TYPE = 3;
                        item.SERVICE_TYPE_NAME = "HÓA CHẤT";
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => o.BEGIN_AMOUNT != 0 || o.END_AMOUNT != 0 || o.EXP_TOTAL_AMOUNT != 0 || o.IMP_TOTAL_AMOUNT != 0 || o.ANTICIPATE_AMOUNT != 0 || o.INPUT_END_AMOUNT != 0 || castFilter.ADD_BID_DETAIL_NO_IMP == true && o.BID_INCREATE_AMOUNT > 0).ToList();
                }
                if (castFilter.IS_MEDICINE != true)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE != 1).ToList();
                }
                if (castFilter.IS_MATERIAL != true)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE != 2).ToList();
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE != true)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE != 3).ToList();
                }
                if (castFilter.IS_BLOOD != true)
                {
                    ListRdo = ListRdo.Where(o => o.TYPE != 4).ToList();
                }

                SetKeyOrder();

                objectTag.AddObjectData(store, "MedicinePaty", MedicinePatys);
                objectTag.AddObjectData(store, "MaterialPaty", MaterialPatys);
                objectTag.AddObjectData(store, "TonToanVien", GroupByServiceTypeRdo());

                objectTag.AddObjectData(store, "ServicesNew", ListRdo.OrderBy(o => o.KEY_ORDER).ToList());

                objectTag.AddObjectData(store, "Services", ListRdo.OrderBy(o => o.KEY_ORDER).ToList());

                objectTag.AddObjectData(store, "ServMetyMaty", listServMetyMaty.OrderBy(o => o.SERVICE_NAME).ToList());
                string[] servMetyMatyRel = new string[] { "TYPE", "MEDI_MATE_TYPE_ID" };
                objectTag.AddRelationship(store, "ServMetyMaty", "Services", servMetyMatyRel, servMetyMatyRel);

                if (ListMediStock.Count == 0)
                {
                    throw new Exception("Không tạo được báo cáo theo điều kiện lọc đã chọn.");
                }

                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                dicSingleTag.Add("PROTECT", "");
                V_HIS_MEDI_STOCK mediStockDefault = new V_HIS_MEDI_STOCK();
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.Where(o => o.BEGIN_AMOUNT != 0 || o.END_AMOUNT != 0 || o.ID__BAN_EXP_AMOUNT != 0 ||
                        o.ID__BCS_EXP_AMOUNT != 0 || o.ID__BCS_IMP_AMOUNT != 0 || o.ID__BL_EXP_AMOUNT != 0 ||
                        o.ID__BL_IMP_AMOUNT != 0 || o.ID__CK_EXP_AMOUNT != 0 || o.ID__CK_IMP_AMOUNT != 0 ||
                        o.ID__DK_IMP_AMOUNT != 0 || o.ID__DM_EXP_AMOUNT != 0 || o.ID__DMTL_IMP_AMOUNT != 0 ||
                        o.ID__BCT_EXP_AMOUNT != 0 || o.ID__DNT_EXP_AMOUNT != 0 || o.ID__DNTTL_IMP_AMOUNT != 0 ||
                        o.ID__DPK_EXP_AMOUNT != 0 || o.ID__DTT_EXP_AMOUNT != 0 || o.ID__DTTTL_IMP_AMOUNT != 0 ||
                        o.ID__HPKP_EXP_AMOUNT != 0 || o.ID__HPTL_IMP_AMOUNT != 0 || o.ID__KHAC_EXP_AMOUNT != 0 ||
                        o.ID__KHAC_IMP_AMOUNT != 0 || o.ID__KK_IMP_AMOUNT != 0 || o.ID__NCC_IMP_AMOUNT != 0 ||
                        o.ID__PL_EXP_AMOUNT != 0 || o.ID__TH_IMP_AMOUNT != 0 || o.ID__THT_IMP_AMOUNT != 0 ||
                        o.ID__TNCC_EXP_AMOUNT != 0 || o.ANTICIPATE_AMOUNT != 0 || o.ID__VACC_EXP_AMOUNT != 0).ToList();
                    mediStockDefault = new V_HIS_MEDI_STOCK() { ID = ListRdo.First().MEDI_STOCK_ID, MEDI_STOCK_NAME = "" };
                }

                objectTag.AddObjectData(store, "MediStocks", (castFilter.IS_MERGE ?? false) || (castFilter.IS_MERGER_MEDI_STOCK ?? false) || (castFilter.IS_MERGE_STOCK ?? false) ? new List<V_HIS_MEDI_STOCK>() { mediStockDefault } : ListMediStock.Where(o => ListRdo.Exists(p => p.MEDI_STOCK_ID == o.ID)).ToList());
                objectTag.AddObjectData(store, "ServiceInStocks", ListRdo.GroupBy(o => new { o.SERVICE_CODE, o.TYPE, o.MEDI_STOCK_ID }).Select(p => p.First()).OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddRelationship(store, "ServiceInStocks", "Services", new string[] { "MEDI_STOCK_ID", "TYPE", "SERVICE_CODE" }, new string[] { "MEDI_STOCK_ID", "TYPE", "SERVICE_CODE" });
                objectTag.AddObjectData(store, "TypeInStocks", ListRdo.GroupBy(o => new { o.TYPE, o.MEDI_STOCK_ID }).Select(p => p.First()).OrderBy(p => p.TYPE).ToList());
                objectTag.AddRelationship(store, "TypeInStocks", "ServiceInStocks", new string[] { "MEDI_STOCK_ID", "TYPE" }, new string[] { "MEDI_STOCK_ID", "TYPE" });
                objectTag.AddRelationship(store, "MediStocks", "TypeInStocks", "ID", "MEDI_STOCK_ID");
                objectTag.AddObjectData(store, "Detail", ListDetail);
                objectTag.AddObjectData(store, "listExpMestOn", listExpMestOn);
                objectTag.AddObjectData(store, "DistinctTrea", listExpMestOn.Where(p => p.TDL_TREATMENT_ID.HasValue).GroupBy(o => new { o.TDL_TREATMENT_ID, o.EXP_MEST_TYPE_ID }).Select(q => q.First()).ToList());
                objectTag.AddObjectData(store, "listImpMestOn", listImpMestOn.Where(o => o.IMP_TIME >= castFilter.TIME_FROM && o.IMP_TIME < castFilter.TIME_TO).ToList());
                objectTag.SetUserFunction(store, "Element", new RDOElement());

                if (!(castFilter.IS_MERGE ?? false) && !(castFilter.IS_MERGER_MEDI_STOCK ?? false) && !(castFilter.IS_MERGE_STOCK ?? false))
                {
                    objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");
                }

                var lstGroupCode = ListRdo.GroupBy(o => new { o.TYPE, o.MEDI_MATE_TYPE_ID, o.IMP_PRICE }).ToList();
                List<Mrs00105RDO> ListRdoType = new List<Mrs00105RDO>();
                foreach (var item in lstGroupCode)
                {
                    Mrs00105RDO r = new Mrs00105RDO();
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00105RDO>(r, item.First());

                    r.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                    r.HEIN_SERVICE_NAME = item.First().HEIN_SERVICE_NAME;
                    r.CONCENTRA = item.First().CONCENTRA;
                    r.BID_NUM_ORDER = item.First().BID_NUM_ORDER;
                    r.TDL_BID_NUMBER = item.First().TDL_BID_NUMBER;
                    r.BID_ID = item.First().BID_ID;
                    r.BID_YEAR = item.First().BID_YEAR;
                    r.BID_PACKAGE_CODE = item.First().BID_PACKAGE_CODE;
                    r.BID_GROUP_CODE = item.First().BID_GROUP_CODE;
                    r.NATIONAL_NAME = item.First().NATIONAL_NAME ?? " ";
                    r.IMP_PRICE = item.First().IMP_PRICE;
                    r.BEGIN_AMOUNT = item.Sum(o => o.BEGIN_AMOUNT);
                    r.END_AMOUNT = item.Sum(o => o.END_AMOUNT);
                    r.EXP_TOTAL_AMOUNT = item.Sum(o => o.EXP_TOTAL_AMOUNT);
                    r.IMP_TOTAL_AMOUNT = item.Sum(o => o.IMP_TOTAL_AMOUNT);
                    r.ID__BAN_EXP_AMOUNT = item.Sum(o => o.ID__BAN_EXP_AMOUNT);
                    r.ID__BCS_EXP_AMOUNT = item.Sum(o => o.ID__BCS_EXP_AMOUNT);
                    r.ID__BCT_EXP_AMOUNT = item.Sum(o => o.ID__BCT_EXP_AMOUNT);
                    r.ID__BL_EXP_AMOUNT = item.Sum(o => o.ID__BL_EXP_AMOUNT);
                    r.ID__CK_EXP_AMOUNT = item.Sum(o => o.ID__CK_EXP_AMOUNT);
                    r.ID__DM_EXP_AMOUNT = item.Sum(o => o.ID__DM_EXP_AMOUNT);
                    r.ID__DNT_EXP_AMOUNT = item.Sum(o => o.ID__DNT_EXP_AMOUNT);
                    r.ID__DPK_EXP_AMOUNT = item.Sum(o => o.ID__DPK_EXP_AMOUNT);
                    r.ID__DTT_EXP_AMOUNT = item.Sum(o => o.ID__DTT_EXP_AMOUNT);
                    r.ID__HPKP_EXP_AMOUNT = item.Sum(o => o.ID__HPKP_EXP_AMOUNT);
                    r.ID__KHAC_EXP_AMOUNT = item.Sum(o => o.ID__KHAC_EXP_AMOUNT);
                    r.ID__PL_EXP_AMOUNT = item.Sum(o => o.ID__PL_EXP_AMOUNT);
                    r.ID__TNCC_EXP_AMOUNT = item.Sum(o => o.ID__TNCC_EXP_AMOUNT);
                    r.ID__BCS_IMP_AMOUNT = item.Sum(o => o.ID__BCS_IMP_AMOUNT);
                    r.ID__BCT_IMP_AMOUNT = item.Sum(o => o.ID__BCT_IMP_AMOUNT);
                    r.ID__BL_IMP_AMOUNT = item.Sum(o => o.ID__BL_IMP_AMOUNT);
                    r.ID__BTL_IMP_AMOUNT = item.Sum(o => o.ID__BTL_IMP_AMOUNT);
                    r.ID__CK_IMP_AMOUNT = item.Sum(o => o.ID__CK_IMP_AMOUNT);
                    r.ID__DK_IMP_AMOUNT = item.Sum(o => o.ID__DK_IMP_AMOUNT);
                    r.ID__DMTL_IMP_AMOUNT = item.Sum(o => o.ID__DMTL_IMP_AMOUNT);
                    r.ID__DNTTL_IMP_AMOUNT = item.Sum(o => o.ID__DNTTL_IMP_AMOUNT);
                    r.ID__DTTTL_IMP_AMOUNT = item.Sum(o => o.ID__DTTTL_IMP_AMOUNT);
                    r.ID__HPTL_IMP_AMOUNT = item.Sum(o => o.ID__HPTL_IMP_AMOUNT);
                    r.ID__KHAC_IMP_AMOUNT = item.Sum(o => o.ID__KHAC_IMP_AMOUNT);
                    r.ID__KK_IMP_AMOUNT = item.Sum(o => o.ID__KK_IMP_AMOUNT);
                    r.ID__NCC_IMP_AMOUNT = item.Sum(o => o.ID__NCC_IMP_AMOUNT);
                    r.ID__TH_IMP_AMOUNT = item.Sum(o => o.ID__TH_IMP_AMOUNT);
                    r.ID__THT_IMP_AMOUNT = item.Sum(o => o.ID__THT_IMP_AMOUNT);
                    r.ID__VACC_EXP_AMOUNT = item.Sum(o => o.ID__VACC_EXP_AMOUNT);

                    ListRdoType.Add(r);
                }

                var lstGroupID = ListRdo.GroupBy(o => new { o.MEDI_MATE_TYPE_ID }).ToList();
                List<Mrs00105RDO> ListRdoID = new List<Mrs00105RDO>();
                foreach (var item in lstGroupID)
                {
                    Mrs00105RDO r = new Mrs00105RDO();
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00105RDO>(r, item.First());

                    r.BEGIN_AMOUNT = item.Sum(o => o.BEGIN_AMOUNT);
                    r.END_AMOUNT = item.Sum(o => o.END_AMOUNT);

                    r.ID__BAN_EXP_AMOUNT = item.Sum(o => o.ID__BAN_EXP_AMOUNT);
                    r.ID__BCS_EXP_AMOUNT = item.Sum(o => o.ID__BCS_EXP_AMOUNT);
                    r.ID__BCT_EXP_AMOUNT = item.Sum(o => o.ID__BCT_EXP_AMOUNT);
                    r.ID__BL_EXP_AMOUNT = item.Sum(o => o.ID__BL_EXP_AMOUNT);
                    r.ID__CK_EXP_AMOUNT = item.Sum(o => o.ID__CK_EXP_AMOUNT);
                    r.ID__DM_EXP_AMOUNT = item.Sum(o => o.ID__DM_EXP_AMOUNT);
                    r.ID__DNT_EXP_AMOUNT = item.Sum(o => o.ID__DNT_EXP_AMOUNT);
                    r.ID__DPK_EXP_AMOUNT = item.Sum(o => o.ID__DPK_EXP_AMOUNT);
                    r.ID__DTT_EXP_AMOUNT = item.Sum(o => o.ID__DTT_EXP_AMOUNT);
                    r.ID__HPKP_EXP_AMOUNT = item.Sum(o => o.ID__HPKP_EXP_AMOUNT);
                    r.ID__KHAC_EXP_AMOUNT = item.Sum(o => o.ID__KHAC_EXP_AMOUNT);
                    r.ID__PL_EXP_AMOUNT = item.Sum(o => o.ID__PL_EXP_AMOUNT);
                    r.ID__TNCC_EXP_AMOUNT = item.Sum(o => o.ID__TNCC_EXP_AMOUNT);
                    r.ID__BCS_IMP_AMOUNT = item.Sum(o => o.ID__BCS_IMP_AMOUNT);
                    r.ID__BCT_IMP_AMOUNT = item.Sum(o => o.ID__BCT_IMP_AMOUNT);
                    r.ID__BL_IMP_AMOUNT = item.Sum(o => o.ID__BL_IMP_AMOUNT);
                    r.ID__BTL_IMP_AMOUNT = item.Sum(o => o.ID__BTL_IMP_AMOUNT);
                    r.ID__CK_IMP_AMOUNT = item.Sum(o => o.ID__CK_IMP_AMOUNT);
                    r.ID__DK_IMP_AMOUNT = item.Sum(o => o.ID__DK_IMP_AMOUNT);
                    r.ID__DMTL_IMP_AMOUNT = item.Sum(o => o.ID__DMTL_IMP_AMOUNT);
                    r.ID__DNTTL_IMP_AMOUNT = item.Sum(o => o.ID__DNTTL_IMP_AMOUNT);
                    r.ID__DTTTL_IMP_AMOUNT = item.Sum(o => o.ID__DTTTL_IMP_AMOUNT);
                    r.ID__HPTL_IMP_AMOUNT = item.Sum(o => o.ID__HPTL_IMP_AMOUNT);
                    r.ID__KHAC_IMP_AMOUNT = item.Sum(o => o.ID__KHAC_IMP_AMOUNT);
                    r.ID__KK_IMP_AMOUNT = item.Sum(o => o.ID__KK_IMP_AMOUNT);
                    r.ID__NCC_IMP_AMOUNT = item.Sum(o => o.ID__NCC_IMP_AMOUNT);
                    r.ID__TH_IMP_AMOUNT = item.Sum(o => o.ID__TH_IMP_AMOUNT);
                    r.ID__THT_IMP_AMOUNT = item.Sum(o => o.ID__THT_IMP_AMOUNT);
                    r.ID__VACC_EXP_AMOUNT = item.Sum(o => o.ID__VACC_EXP_AMOUNT);

                    ListRdoID.Add(r);
                }

                var lstGroupBidNumber = ListRdo.GroupBy(o => new { o.TYPE, o.MEDI_MATE_TYPE_ID, o.TDL_BID_NUMBER }).ToList();
                List<Mrs00105RDO> ListRdoBid = new List<Mrs00105RDO>();
                foreach (var item in lstGroupBidNumber)
                {
                    Mrs00105RDO r = new Mrs00105RDO();
                    Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00105RDO>(r, item.First());

                    r.BEGIN_AMOUNT = item.Sum(o => o.BEGIN_AMOUNT);
                    r.END_AMOUNT = item.Sum(o => o.END_AMOUNT);

                    r.ID__BAN_EXP_AMOUNT = item.Sum(o => o.ID__BAN_EXP_AMOUNT);
                    r.ID__BCS_EXP_AMOUNT = item.Sum(o => o.ID__BCS_EXP_AMOUNT);
                    r.ID__BCT_EXP_AMOUNT = item.Sum(o => o.ID__BCT_EXP_AMOUNT);
                    r.ID__BL_EXP_AMOUNT = item.Sum(o => o.ID__BL_EXP_AMOUNT);
                    r.ID__CK_EXP_AMOUNT = item.Sum(o => o.ID__CK_EXP_AMOUNT);
                    r.ID__DM_EXP_AMOUNT = item.Sum(o => o.ID__DM_EXP_AMOUNT);
                    r.ID__DNT_EXP_AMOUNT = item.Sum(o => o.ID__DNT_EXP_AMOUNT);
                    r.ID__DPK_EXP_AMOUNT = item.Sum(o => o.ID__DPK_EXP_AMOUNT);
                    r.ID__DTT_EXP_AMOUNT = item.Sum(o => o.ID__DTT_EXP_AMOUNT);
                    r.ID__HPKP_EXP_AMOUNT = item.Sum(o => o.ID__HPKP_EXP_AMOUNT);
                    r.ID__KHAC_EXP_AMOUNT = item.Sum(o => o.ID__KHAC_EXP_AMOUNT);
                    r.ID__PL_EXP_AMOUNT = item.Sum(o => o.ID__PL_EXP_AMOUNT);
                    r.ID__TNCC_EXP_AMOUNT = item.Sum(o => o.ID__TNCC_EXP_AMOUNT);
                    r.ID__BCS_IMP_AMOUNT = item.Sum(o => o.ID__BCS_IMP_AMOUNT);
                    r.ID__BCT_IMP_AMOUNT = item.Sum(o => o.ID__BCT_IMP_AMOUNT);
                    r.ID__BL_IMP_AMOUNT = item.Sum(o => o.ID__BL_IMP_AMOUNT);
                    r.ID__BTL_IMP_AMOUNT = item.Sum(o => o.ID__BTL_IMP_AMOUNT);
                    r.ID__CK_IMP_AMOUNT = item.Sum(o => o.ID__CK_IMP_AMOUNT);
                    r.ID__DK_IMP_AMOUNT = item.Sum(o => o.ID__DK_IMP_AMOUNT);
                    r.ID__DMTL_IMP_AMOUNT = item.Sum(o => o.ID__DMTL_IMP_AMOUNT);
                    r.ID__DNTTL_IMP_AMOUNT = item.Sum(o => o.ID__DNTTL_IMP_AMOUNT);
                    r.ID__DTTTL_IMP_AMOUNT = item.Sum(o => o.ID__DTTTL_IMP_AMOUNT);
                    r.ID__HPTL_IMP_AMOUNT = item.Sum(o => o.ID__HPTL_IMP_AMOUNT);
                    r.ID__KHAC_IMP_AMOUNT = item.Sum(o => o.ID__KHAC_IMP_AMOUNT);
                    r.ID__KK_IMP_AMOUNT = item.Sum(o => o.ID__KK_IMP_AMOUNT);
                    r.ID__NCC_IMP_AMOUNT = item.Sum(o => o.ID__NCC_IMP_AMOUNT);
                    r.ID__TH_IMP_AMOUNT = item.Sum(o => o.ID__TH_IMP_AMOUNT);
                    r.ID__THT_IMP_AMOUNT = item.Sum(o => o.ID__THT_IMP_AMOUNT);
                    r.ID__VACC_EXP_AMOUNT = item.Sum(o => o.ID__VACC_EXP_AMOUNT);

                    ListRdoBid.Add(r);
                }

                ListRdoID = ListRdoID.OrderBy(o => o.SERVICE_CODE).ToList();
                ListRdoType = ListRdoType.OrderBy(o => o.TYPE).ThenBy(o => o.SERVICE_CODE).ToList();
                ListRdoBid = ListRdoBid.OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ThenBy(p => p.SERVICE_TYPE_NAME).ToList();
                objectTag.AddObjectData(store, "HPKP", ListRdoID.Where(P => P.ID__HPKP_EXP_AMOUNT > 0).OrderBy(p => p.PARENT_MEDICINE_TYPE_CODE).ThenBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "ServicesType", ListRdoType.OrderBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "ServicesID", ListRdoID.OrderBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "Bid", ListRdoBid);
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
                objectTag.AddObjectData(store, "SumMediStock", SumOfMediStock().OrderBy(o => o.TYPE).ToList());
                objectTag.AddObjectData(store, "listExpReqDepartment", listExpReqDepartment);
                objectTag.AddObjectData(store, "listParentMedicineType", ListRdo.GroupBy(o => o.PARENT_MEDICINE_TYPE_ID).Select(p => p.First()).OrderBy(q => q.PARENT_MEDICINE_TYPE_NUM).ToList());
                objectTag.AddObjectData(store, "ExpDepas", ListRdo.Where(o => o.DIC_EXP_REQ_DEPARTMENT != null && o.DIC_EXP_REQ_DEPARTMENT.Values != null && o.DIC_EXP_REQ_DEPARTMENT.Values.Where(p => p > 0).Count() > 0).ToList());
                objectTag.AddObjectData(store, "ParentExpDepas", ListRdo.Where(o => o.DIC_EXP_REQ_DEPARTMENT != null && o.DIC_EXP_REQ_DEPARTMENT.Values != null && o.DIC_EXP_REQ_DEPARTMENT.Values.Where(p => p > 0).Count() > 0).GroupBy(g => new { g.PARENT_MEDICINE_TYPE_ID, g.MEDI_STOCK_ID }).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "MstExpDepas", ListRdo.Where(o => o.DIC_EXP_REQ_DEPARTMENT != null && o.DIC_EXP_REQ_DEPARTMENT.Values != null && o.DIC_EXP_REQ_DEPARTMENT.Values.Where(p => p > 0).Count() > 0).GroupBy(g => g.MEDI_STOCK_ID).Select(p => p.First()).ToList());

                //thêm các dữ liệu cho mẫu nhóm theo nguồn nhập
                objectTag.AddObjectData(store, "ImpSources", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => o.IMP_SOURCE_ID).Select(p => p.First()).OrderBy(q => q.IMP_SOURCE_CODE).ToList());

                objectTag.AddObjectData(store, "Groups", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => new { o.IMP_SOURCE_ID, o.MEDICINE_GROUP_ID }).Select(p => p.First()).OrderBy(q => q.MEDICINE_GROUP_CODE).ToList());
                objectTag.AddObjectData(store, "Parents", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => new { o.IMP_SOURCE_ID, o.MEDICINE_GROUP_ID, o.PARENT_MEDICINE_TYPE_ID }).Select(p => p.First()).OrderBy(q => q.PARENT_MEDICINE_TYPE_NUM).ToList());
                objectTag.AddObjectData(store, "GroupMediLines", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => new { o.IMP_SOURCE_ID, o.MEDICINE_GROUP_ID, o.PARENT_MEDICINE_TYPE_ID, o.MEDICINE_LINE_ID }).Select(p => p.First()).OrderBy(q => q.MEDICINE_GROUP_CODE).OrderBy(q => q.MEDICINE_LINE_CODE).ToList());

                objectTag.AddObjectData(store, "HasImpSources", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());
                objectTag.AddObjectData(store, "KhangSinhNoiTru", ListRdo.Where(p => p.ID__DNTTL_IMP_AMOUNT > 0 || p.ID__DTTTL_IMP_AMOUNT > 0 || p.ID__DTT_EXP_AMOUNT > 0 || p.ID__DNT_EXP_AMOUNT > 0).ToList());

                objectTag.AddRelationship(store, "ImpSources", "Groups", "IMP_SOURCE_ID", "IMP_SOURCE_ID");
                objectTag.AddRelationship(store, "Groups", "Parents", new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID" }, new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID" });

                objectTag.AddRelationship(store, "Parents", "GroupMediLines", new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID", "PARENT_MEDICINE_TYPE_ID" });

                objectTag.AddRelationship(store, "GroupMediLines", "HasImpSources", new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID", "PARENT_MEDICINE_TYPE_ID", "MEDICINE_LINE_ID" }, new string[] { "IMP_SOURCE_ID", "MEDICINE_GROUP_ID", "PARENT_MEDICINE_TYPE_ID", "MEDICINE_LINE_ID" });
                objectTag.AddObjectData(store, "Mests", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => o.MEDI_STOCK_ID).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "Mests", "HasImpSources", "MEDI_STOCK_ID", "MEDI_STOCK_ID");
                objectTag.AddObjectData(store, "IsNtNgtCls", ListRdo.Where(o => this.castFilter.IMP_SOURCE_IDs == null || o.IMP_SOURCE_ID > 0).GroupBy(o => o.IS_NT_NGT_CLS).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "IsNtNgtCls", "Mests", "IS_NT_NGT_CLS", "IS_NT_NGT_CLS");
                objectTag.AddObjectData(store, "KhoDen", HisMediStockCFG.HisMediStocks.Where(o => this.CodeSourceChms.Contains(o.MEDI_STOCK_CODE)).ToList());
                objectTag.AddObjectData(store, "KhoDi", HisMediStockCFG.HisMediStocks.Where(o => this.CodeDestChms.Contains(o.MEDI_STOCK_CODE)).ToList());
                objectTag.AddObjectData(store, "KhoaNhan", HisDepartmentCFG.DEPARTMENTs.Where(o => this.CodeDestDps.Contains(o.DEPARTMENT_CODE)).ToList());
                objectTag.AddObjectData(store, "KhoaCho", HisDepartmentCFG.DEPARTMENTs.Where(o => this.CodeSourceDps.Contains(o.DEPARTMENT_CODE)).ToList());

                objectTag.AddObjectData(store, "MediStockHasFilter", ListMediStock);
                objectTag.AddObjectData(store, "MediStockHasData", listMediStockHasData);


                objectTag.AddObjectData(store, "VACCIN", ListRdo.Where(o => o.ID__VACC_EXP_AMOUNT > 0).ToList());


                List<RoleUserADO> listRole = new List<RoleUserADO>();
                if (IsNotNullOrEmpty(castFilter.EXECUTE_ROLE_GROUP))
                {
                    listRole.AddRange(castFilter.EXECUTE_ROLE_GROUP);
                }

                objectTag.AddObjectData(store, "UserRole", listRole);
                dicSingleTag.Add("TIME", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? (Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(System.DateTime.Now) ?? 0)));

                dicSingleTag.Add("MEDI_STOCK_CODE__CABINs", string.Join(",", HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(o => o.MEDI_STOCK_CODE).ToList()));

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }



        }

        private void SetKeyOrder()
        {
            string keyOrder = "{0}_{1}_{2}";
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_ORDER") && this.dicDataFilter["KEY_ORDER"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_ORDER"].ToString()))
            {
                keyOrder = this.dicDataFilter["KEY_ORDER"].ToString();
            }

            foreach (var item in ListRdo)
            {
                item.KEY_ORDER = string.Format(keyOrder, item.MEDI_STOCK_ID, item.TYPE, item.SERVICE_NAME, string.Format("{0:0000000000}", ToNumber(item.BID_NUM_ORDER)));
            }
        }

        private int ToNumber(string stringNumber)
        {
            int result = int.MaxValue;
            int.TryParse(stringNumber, out result);
            return result;
        }

        private List<Mrs00105RDO> GroupByServiceTypeRdo()
        {
            List<Mrs00105RDO> result = new List<Mrs00105RDO>();
            try
            {
                var group = ListRdo.GroupBy(x => new { x.IMP_PRICE, x.MEDI_MATE_TYPE_ID, x.TDL_BID_NUMBER, x.EXPIRED_DATE }).ToList();
                if (IsNotNullOrEmpty(group))
                {
                    foreach (var item in group)
                    {
                        Mrs00105RDO rdo = new Mrs00105RDO();
                        rdo.MEDI_MATE_ID = item.First().MEDI_MATE_ID;
                        rdo.IMP_PRICE = item.First().IMP_PRICE;
                        rdo.EXPIRED_DATE_STR = item.First().EXPIRED_DATE_STR;
                        rdo.MEDI_MATE_TYPE_ID = item.First().MEDI_MATE_TYPE_ID;
                        rdo.MEDICINE_GROUP_CODE = item.First().MEDICINE_GROUP_CODE;
                        rdo.MEDICINE_GROUP_NAME = item.First().MEDICINE_GROUP_NAME;
                        rdo.ATC_CODES = item.First().ATC_CODES;
                        rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_NAME = item.First().SUPPLIER_NAME;
                        rdo.MANUFACTURER_NAME = item.First().MANUFACTURER_NAME;
                        rdo.REGISTER_NUMBER = item.First().REGISTER_NUMBER;
                        rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.CONCENTRA = item.First().CONCENTRA;
                        rdo.MEDICINE_USE_FORM_NAME = item.First().MEDICINE_USE_FORM_NAME;
                        rdo.NATIONAL_NAME = item.First().NATIONAL_NAME ?? " ";
                        rdo.SERVICE_NAME = item.First().SERVICE_NAME;
                        rdo.SERVICE_CODE = item.First().SERVICE_CODE;
                        rdo.SCIENTIFIC_NAME = item.First().SCIENTIFIC_NAME;
                        rdo.HEIN_SERVICE_CODE = item.First().HEIN_SERVICE_CODE;
                        rdo.HEIN_SERVICE_NAME = item.First().HEIN_SERVICE_NAME;
                        rdo.HEIN_SERVICE_TYPE_CODE = item.First().HEIN_SERVICE_TYPE_CODE;
                        rdo.HEIN_SERVICE_TYPE_NAME = item.First().HEIN_SERVICE_TYPE_NAME;
                        rdo.TDL_BID_NUMBER = item.First().TDL_BID_NUMBER;
                        rdo.BID_ID = item.First().BID_ID;
                        rdo.BID_NUM_ORDER = item.First().BID_NUM_ORDER;
                        rdo.BID_GROUP_CODE = item.First().BID_GROUP_CODE;
                        rdo.BID_NAME = item.First().BID_NAME;
                        rdo.BID_PACKAGE_CODE = item.First().BID_PACKAGE_CODE;
                        rdo.BID_YEAR = item.First().BID_YEAR;
                        rdo.DIC_MEDI_STOCK_BEGIN_AMOUNT = item.GroupBy(x => x.MEDI_STOCK_CODE).ToDictionary(x => x.Key, y => y.Sum(x => x.BEGIN_AMOUNT));
                        rdo.DIC_MEDI_STOCK_END_AMOUNT = item.GroupBy(x => x.MEDI_STOCK_CODE).ToDictionary(x => x.Key, y => y.Sum(x => x.END_AMOUNT));
                        rdo.IMP_NCC_AMOUNT = item.Sum(x => x.IMP_NCC_AMOUNT);
                        rdo.DIC_EXP_MEST_REASON = item.First().DIC_EXP_MEST_REASON;
                        rdo.BHYT_EXP_TOTAL_AMOUNT_NOI_TRU = item.Where(x => x.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(x => x.BHYT_EXP_TOTAL_AMOUNT);
                        rdo.BHYT_EXP_TOTAL_AMOUNT_NGOAI_TRU = item.Where(x => x.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(x => x.BHYT_EXP_TOTAL_AMOUNT);
                        rdo.VP_EXP_TOTAL_AMOUNT_NOI_TRU = item.Where(x => x.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(x => x.VP_EXP_TOTAL_AMOUNT);
                        rdo.VP_EXP_TOTAL_AMOUNT_NGOAI_TRU = item.Where(x => x.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(x => x.VP_EXP_TOTAL_AMOUNT);
                        rdo.END_PERIOD_AMOUNT = item.Sum(x => x.END_PERIOD_AMOUNT);
                        rdo.BEGIN_AMOUNT = item.Sum(x => x.BEGIN_AMOUNT);
                        rdo.END_AMOUNT = item.Sum(o => o.END_AMOUNT);
                        rdo.EXP_TOTAL_AMOUNT = item.Sum(o => o.EXP_TOTAL_AMOUNT);
                        rdo.IMP_TOTAL_AMOUNT = item.Sum(o => o.IMP_TOTAL_AMOUNT);
                        rdo.ID__BAN_EXP_AMOUNT = item.Sum(o => o.ID__BAN_EXP_AMOUNT);
                        rdo.ID__BCS_EXP_AMOUNT = item.Sum(o => o.ID__BCS_EXP_AMOUNT);
                        rdo.ID__BCT_EXP_AMOUNT = item.Sum(o => o.ID__BCT_EXP_AMOUNT);
                        rdo.ID__BL_EXP_AMOUNT = item.Sum(o => o.ID__BL_EXP_AMOUNT);
                        rdo.ID__CK_EXP_AMOUNT = item.Sum(o => o.ID__CK_EXP_AMOUNT);
                        rdo.ID__DM_EXP_AMOUNT = item.Sum(o => o.ID__DM_EXP_AMOUNT);
                        rdo.ID__DNT_EXP_AMOUNT = item.Sum(o => o.ID__DNT_EXP_AMOUNT);
                        rdo.ID__DPK_EXP_AMOUNT = item.Sum(o => o.ID__DPK_EXP_AMOUNT);
                        rdo.ID__DTT_EXP_AMOUNT = item.Sum(o => o.ID__DTT_EXP_AMOUNT);
                        rdo.ID__HPKP_EXP_AMOUNT = item.Sum(o => o.ID__HPKP_EXP_AMOUNT);
                        rdo.ID__KHAC_EXP_AMOUNT = item.Sum(o => o.ID__KHAC_EXP_AMOUNT);
                        rdo.ID__PL_EXP_AMOUNT = item.Sum(o => o.ID__PL_EXP_AMOUNT);
                        rdo.ID__TNCC_EXP_AMOUNT = item.Sum(o => o.ID__TNCC_EXP_AMOUNT);
                        rdo.ID__BCS_IMP_AMOUNT = item.Sum(o => o.ID__BCS_IMP_AMOUNT);
                        rdo.ID__BCT_IMP_AMOUNT = item.Sum(o => o.ID__BCT_IMP_AMOUNT);
                        rdo.ID__BL_IMP_AMOUNT = item.Sum(o => o.ID__BL_IMP_AMOUNT);
                        rdo.ID__BTL_IMP_AMOUNT = item.Sum(o => o.ID__BTL_IMP_AMOUNT);
                        rdo.ID__CK_IMP_AMOUNT = item.Sum(o => o.ID__CK_IMP_AMOUNT);
                        rdo.ID__DK_IMP_AMOUNT = item.Sum(o => o.ID__DK_IMP_AMOUNT);
                        rdo.ID__DMTL_IMP_AMOUNT = item.Sum(o => o.ID__DMTL_IMP_AMOUNT);
                        rdo.ID__DNTTL_IMP_AMOUNT = item.Sum(o => o.ID__DNTTL_IMP_AMOUNT);
                        rdo.ID__DTTTL_IMP_AMOUNT = item.Sum(o => o.ID__DTTTL_IMP_AMOUNT);
                        rdo.ID__HPTL_IMP_AMOUNT = item.Sum(o => o.ID__HPTL_IMP_AMOUNT);
                        rdo.ID__KHAC_IMP_AMOUNT = item.Sum(o => o.ID__KHAC_IMP_AMOUNT);
                        rdo.ID__KK_IMP_AMOUNT = item.Sum(o => o.ID__KK_IMP_AMOUNT);
                        rdo.ID__NCC_IMP_AMOUNT = item.Sum(o => o.ID__NCC_IMP_AMOUNT);
                        rdo.ID__TH_IMP_AMOUNT = item.Sum(o => o.ID__TH_IMP_AMOUNT);
                        rdo.ID__THT_IMP_AMOUNT = item.Sum(o => o.ID__THT_IMP_AMOUNT);
                        rdo.ID__VACC_EXP_AMOUNT = item.Sum(o => o.ID__VACC_EXP_AMOUNT);
                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }
        private List<Mrs00105RDO> SumOfMediStock()
        {
            List<Mrs00105RDO> SumMediStock = new List<Mrs00105RDO>();
            try
            {
                foreach (var item in ListMediStock)
                {

                    Mrs00105RDO rdo = new Mrs00105RDO();
                    rdo.SERVICE_TYPE_NAME = "THUỐC";
                    rdo.TYPE = 1;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;

                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 1).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);

                    rdo = new Mrs00105RDO();
                    rdo.SERVICE_TYPE_NAME = "VTYT";
                    rdo.TYPE = 2;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 2).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);

                    rdo = new Mrs00105RDO();
                    rdo.SERVICE_TYPE_NAME = "HÓA CHẤT";
                    rdo.TYPE = 3;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 3).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 3).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 3).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 3).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);

                    rdo = new Mrs00105RDO();
                    rdo.SERVICE_TYPE_NAME = "MÁU";
                    rdo.TYPE = 4;
                    rdo.MEDI_STOCK_ID = item.ID;
                    rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                    rdo.IMP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 4).Sum(s => s.IMP_PRICE * s.IMP_TOTAL_AMOUNT);
                    rdo.EXP_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 4).Sum(s => s.IMP_PRICE * s.EXP_TOTAL_AMOUNT);
                    rdo.BEGIN_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 4).Sum(s => s.IMP_PRICE * s.BEGIN_AMOUNT);
                    rdo.END_TOTAL_PRICE = ListRdo.Where(o => o.MEDI_STOCK_ID == item.ID && o.TYPE == 4).Sum(s => s.IMP_PRICE * s.END_AMOUNT);
                    SumMediStock.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00105RDO>();
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
