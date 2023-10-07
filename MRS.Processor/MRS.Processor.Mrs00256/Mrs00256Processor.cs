using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisAnticipate;
using MOS.MANAGER.HisBidMedicineType;
using MOS.MANAGER.HisAnticipateMety;

using System;
using MOS.MANAGER.HisBid;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisSupplier;
using System.Reflection;
using Inventec.Common.Repository;
//using MOS.MANAGER.HisMobaImpMest; 

namespace MRS.Processor.Mrs00256
{
    public class Mrs00256Processor : AbstractProcessor
    {
        Mrs00256Filter filter = null;
        private List<Mrs00256RDO> listRdo = new List<Mrs00256RDO>();
        private Dictionary<string, Mrs00256RDO> dicRdo = new Dictionary<string, Mrs00256RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_MEDICINE> ListMedicine = new List<HIS_MEDICINE>();

        List<V_HIS_BID> ListHisBid = new List<V_HIS_BID>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_BID_MEDICINE_TYPE> ListBidMedicineType = new List<V_HIS_BID_MEDICINE_TYPE>();
        List<HIS_SUPPLIER> ListSupplier = new List<HIS_SUPPLIER>();

       
        string KeyGroupBidDetail = "{0}_{1}_{2}_{3}";

        CommonParam paramGet = new CommonParam();
        public Mrs00256Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00256Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00256Filter)reportFilter);
            var result = true;
            try
            {
                // nếu không nhập bid_id thì không cho thực hiện báo cáo
                if (filter.BID_ID == null && (filter.BID_IDs == null || filter.BID_IDs.Count == 0))
                    return true;

                //Get loại thuốc
                GetMety();

                //Get thông tin thầu
                GetBid();

                //Get thầu loại thuốc
                GetBidMedicineType();

                //Get lô thuốc
                GetMedi();

                //thông tin gộp chi tiết thầu
                GetGroupBidDetailInfo();

                //Get nhà cung cấp
                GetSupplier();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetSupplier()
        {
            HisSupplierFilterQuery spFilter = new HisSupplierFilterQuery();
            this.ListSupplier = new HisSupplierManager().Get(spFilter);
        }

        private void GetGroupBidDetailInfo()
        {
            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_BID_DETAIL") && this.dicDataFilter["KEY_GROUP_BID_DETAIL"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_BID_DETAIL"].ToString()))
            {
                this.KeyGroupBidDetail = this.dicDataFilter["KEY_GROUP_BID_DETAIL"].ToString();
            }
        }

        private void GetMedi()
        {
            string query = "select * from his_medicine me where me.is_delete=0\n";

            if (filter.BID_ID != null)
            {
                query += string.Format(" AND me.bid_id = {0}\n", filter.BID_ID);
            }
            if (filter.BID_IDs != null)
            {
                query += string.Format(" AND me.bid_id in ({0})\n", string.Join(",", filter.BID_IDs));
            }
            if (filter.MEDICINE_TYPE_IDs != null)
            {
                query += string.Format(" AND me.MEDICINE_TYPE_ID in ({0})\n", string.Join(",", filter.MEDICINE_TYPE_IDs));
            }

            if (filter.SUPPLIER_IDs != null)
            {
                query += string.Format(" AND me.supplier_id in ({0})\n", string.Join(",", filter.SUPPLIER_IDs));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            this.ListMedicine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICINE>(query);
        }

        private void GetBidMedicineType()
        {
            HisBidMedicineTypeViewFilterQuery filterBidMedicineType = new HisBidMedicineTypeViewFilterQuery()
            {
                MEDICINE_TYPE_IDs = filter.MEDICINE_TYPE_IDs,

            };
            if (filter.BID_IDs != null)
            {

                filterBidMedicineType.BID_IDs = filter.BID_IDs;
            }
            if (filter.BID_ID != null)
            {

                filterBidMedicineType.BID_ID = filter.BID_ID;
            }
            ListBidMedicineType = new HisBidMedicineTypeManager(paramGet).GetView(filterBidMedicineType);
            if (filter.SUPPLIER_IDs != null)
            {
                ListBidMedicineType = ListBidMedicineType.Where(o => filter.SUPPLIER_IDs.Contains(o.SUPPLIER_ID)).ToList();
            }
        }

        private void GetBid()
        {
            HisBidViewFilterQuery HbFilter = new HisBidViewFilterQuery();
            HbFilter.ID = filter.BID_ID;
            if (filter.BID_IDs != null)
            {

                HbFilter.IDs = filter.BID_IDs;
            }
            ListHisBid = new HisBidManager(paramGet).GetView(HbFilter);

        }

        private void GetMety()
        {
            HisMedicineTypeViewFilterQuery HmedicineTypeFiltel = new HisMedicineTypeViewFilterQuery();
            HmedicineTypeFiltel.IDs = filter.MEDICINE_TYPE_IDs;
            ListMedicineType = new HisMedicineTypeManager().GetView(HmedicineTypeFiltel);
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                //xử lý theo từng bid
                foreach (var item in ListHisBid)
                {
                    var medicines = ListMedicine.Where(o => o.BID_ID == item.ID).ToList();//lô
                    var bidMedicineTypeHasMedis = ListBidMedicineType.Where(o => o.BID_ID == item.ID && medicines.Exists(p => p.MEDICINE_TYPE_ID == o.MEDICINE_TYPE_ID && p.SUPPLIER_ID == o.SUPPLIER_ID)).ToList();//loại thuốc trong thầu
                    CreateRdoMedi(medicines, item, bidMedicineTypeHasMedis);
                    var bidMedicineTypeHasNotMedis = ListBidMedicineType.Where(o => o.BID_ID == item.ID && !medicines.Exists(p => p.MEDICINE_TYPE_ID == o.MEDICINE_TYPE_ID && p.SUPPLIER_ID == o.SUPPLIER_ID)).ToList();//loại thuốc trong thầu
                    CreateRdoBidMety(bidMedicineTypeHasNotMedis, item);

                }

                // xử lý group
                ProcessGroup();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessGroup()
        {
            var group = listRdo.GroupBy(o => o.KEY_GROUP_BID_DETAIL).ToList();
            listRdo.Clear();
            Mrs00256RDO rdo;
            List<Mrs00256RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00256RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00256RDO();
                listSub = item.ToList<Mrs00256RDO>();
               
                foreach (var field in pi)
                {
                    field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                }
                rdo.BEGIN_AMOUNT = item.Sum(o => o.BEGIN_AMOUNT);
                rdo.TOTAL_IMP = item.Sum(o => o.TOTAL_IMP);
                rdo.TOTAL_PRICE_IMP = item.Sum(o => o.TOTAL_PRICE_IMP);
                rdo.AMOUNT_TO = item.Sum(o => o.AMOUNT_TO);
                rdo.USERED_AMOUNT = item.Sum(o => o.USERED_AMOUNT);
                rdo.BID_AMOUNT_TO = rdo.BID_AMOUNT - rdo.TOTAL_IMP;
                rdo.TOTAL_AMOUNT_TO = rdo.BID_AMOUNT_TO + rdo.AMOUNT_TO;
                rdo.COUNT_METY = item.GroupBy(o => o.CODE).Count();
                listRdo.Add(rdo);
            }
        }

        private Mrs00256RDO IsMeaningful(List<Mrs00256RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").OrderByDescending(p => field.GetValue(p)).FirstOrDefault() ?? new Mrs00256RDO();
        }

        private void CreateRdoBidMety(List<V_HIS_BID_MEDICINE_TYPE> bidMedicineTypeHasNotMedis, V_HIS_BID bid)
        {
            foreach (var item in bidMedicineTypeHasNotMedis)
            {
                //var immm = null;
                //var emmm = null;

                var mety = ListMedicineType.FirstOrDefault(o => o.ID == item.MEDICINE_TYPE_ID);
                Mrs00256RDO rdo = CreateRdo(bid, item, mety, null, null, null);
                listRdo.Add(rdo);
            }
        }

        private void CreateRdoMedi(List<HIS_MEDICINE> medicines, V_HIS_BID bid, List<V_HIS_BID_MEDICINE_TYPE> bidMetys)
        {
            var metyIds = medicines.Select(o => o.MEDICINE_TYPE_ID).Distinct().ToList();//id loại thuốc

            var medicineTypes = ListMedicineType.Where(o => metyIds.Contains(o.ID)).ToList();//loại thuốc

            //nhập thuốc trong thầu
            List<V_HIS_IMP_MEST_MEDICINE> impMestMedicines = GetImpMestMedicine(medicines);

            //xuất thuốc trong thầu
            List<V_HIS_EXP_MEST_MEDICINE> expMestMedicines = GetExpMestMedicine(medicines);

            //xử lý theo từng loại thuốc
            foreach (var itemMedi in medicines)
            {
                var immm = impMestMedicines.Where(o => o.MEDICINE_ID == itemMedi.ID).ToList();
                var emmm = expMestMedicines.Where(o => o.MEDICINE_ID == itemMedi.ID).ToList();
                var mety = medicineTypes.FirstOrDefault(o => o.ID == itemMedi.MEDICINE_TYPE_ID);
                var bidMety = bidMetys.FirstOrDefault(o => itemMedi.MEDICINE_TYPE_ID == o.MEDICINE_TYPE_ID && itemMedi.SUPPLIER_ID == o.SUPPLIER_ID);
                Mrs00256RDO rdo = CreateRdo(bid, bidMety, mety, itemMedi, immm, emmm);
                listRdo.Add(rdo);
            }
        }

        private Mrs00256RDO CreateRdo(V_HIS_BID bid, V_HIS_BID_MEDICINE_TYPE _bidMety, V_HIS_MEDICINE_TYPE _mety, HIS_MEDICINE _itemMedi, List<V_HIS_IMP_MEST_MEDICINE> _immm, List<V_HIS_EXP_MEST_MEDICINE> _emmm)
        {
            Mrs00256RDO result = new Mrs00256RDO();
            if (bid != null)
            {
                result.BID_NUMBER = bid.BID_NUMBER;
                result.BID_NAME = bid.BID_NAME;
                result.BID_YEAR = bid.BID_YEAR;
            
            }
            if (_mety != null)
            {
                result.ACTIVE_INGR_BHYT_CODE = _mety.ACTIVE_INGR_BHYT_CODE;
                result.ACTIVE_INGR_BHYT_NAME = _mety.ACTIVE_INGR_BHYT_NAME;
                result.CONCENTTRA = _mety.CONCENTRA;
                result.NATIONAL_NAME = _mety.NATIONAL_NAME;
                result.MANUFACTURER_NAME = _mety.MANUFACTURER_NAME;
                result.CODE = _mety.MEDICINE_TYPE_CODE;
                result.NAME = _mety.MEDICINE_TYPE_NAME;
                result.UNIT = _mety.SERVICE_UNIT_NAME;
                result.DESCRIPTION = _mety.PACKING_TYPE_NAME;
                result.MANUFACTURER = _mety.MANUFACTURER_NAME;
            }
            //bổ sung trường nhà cung cấp theo ưu tiên thông tin thầu
            long priorityBidSupplierId = _bidMety != null ? _bidMety.SUPPLIER_ID : (_itemMedi != null ? _itemMedi.SUPPLIER_ID ?? 0 : 0);
            var priorityBidSupplier = ListSupplier.FirstOrDefault(o => o.ID == priorityBidSupplierId);
            if (priorityBidSupplier != null)
            {
                result.PRIO_BID_SUPPLIER_NAME = priorityBidSupplier.SUPPLIER_NAME;
                result.PRIO_BID_SUPPLIER_CODE = priorityBidSupplier.SUPPLIER_CODE;
            }
            if (_itemMedi != null)
            {
                var supplier = ListSupplier.FirstOrDefault(o => o.ID == _itemMedi.SUPPLIER_ID);
                if (supplier != null)
                {
                    result.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                    result.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                }
                result.PRICE = _itemMedi.IMP_PRICE * (1 + _itemMedi.IMP_VAT_RATIO);
                result.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, _itemMedi.MEDICINE_TYPE_ID, "THUOC", _itemMedi.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE, result.PRICE, priorityBidSupplierId);
                result.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE,0, 0);
            }
            if (_bidMety != null)
            {
                result.BID_GROUP_CODE = _bidMety.BID_GROUP_CODE;
                result.BID_PACKAGE_CODE = _bidMety.BID_PACKAGE_CODE;
                result.BID_AMOUNT = _bidMety.AMOUNT;

                if (_itemMedi == null)
                {
                    var supplier = ListSupplier.FirstOrDefault(o => o.ID == _bidMety.SUPPLIER_ID);
                    if (supplier != null)
                    {
                        result.SUPPLIER_NAME = supplier.SUPPLIER_NAME;
                        result.SUPPLIER_CODE = supplier.SUPPLIER_CODE;
                    }
                    result.PRICE = (_bidMety.IMP_PRICE ?? 0) * (1 + (_bidMety.IMP_VAT_RATIO ?? 0));
                    result.KEY_GROUP_BID_DETAIL = string.Format(this.KeyGroupBidDetail, bid.ID, _bidMety.MEDICINE_TYPE_ID, "THUOC", _bidMety.SUPPLIER_ID, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE, result.PRICE, priorityBidSupplierId);
                    result.KEY_GROUP_BID = string.Format(this.KeyGroupBidDetail, bid.ID, 0, 0, 0, bid.BID_NUMBER, bid.BID_NAME, bid.BID_TYPE_ID, bid.BID_YEAR, bid.VALID_FROM_TIME, bid.VALID_TO_TIME, bid.BID_EXTRA_CODE, 0, 0);
                }

            }

            if (_immm != null)
            {
                result.TOTAL_IMP = _immm.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC || o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DK).Sum(o => o.AMOUNT);
                result.TOTAL_PRICE_IMP = result.TOTAL_IMP * result.PRICE;
                result.AMOUNT_TO += _immm.Where(o => o.IMP_TIME < filter.TIME_TO).Sum(o => o.AMOUNT);
                result.BEGIN_AMOUNT += _immm.Where(o => o.IMP_TIME < filter.TIME_FROM).Sum(o => o.AMOUNT);

            }
            if (_emmm != null)
            {
                result.USERED_AMOUNT = _emmm.Where(o => o.CK_IMP_MEST_MEDICINE_ID == null).Sum(o => o.AMOUNT) - _immm.Where(o => o.TH_EXP_MEST_MEDICINE_ID != null).Sum(o => o.AMOUNT);
                result.AMOUNT_TO -= _emmm.Where(o => o.EXP_TIME < filter.TIME_TO).Sum(o => o.AMOUNT);
                result.BEGIN_AMOUNT -= _emmm.Where(o => o.EXP_TIME < filter.TIME_FROM).Sum(o => o.AMOUNT);
            }

            result.ANTICIPATE_NAME = "Chưa xác định";
            return result;
        }

        private List<V_HIS_IMP_MEST_MEDICINE> GetImpMestMedicine(List<HIS_MEDICINE> medicines)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            var medicineIds = medicines.Select(p => p.ID).ToList();
            var skip = 0;
            while (medicineIds.Count - skip > 0)
            {
                var listIDs = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisImpMestMedicineViewFilterQuery filterImmm = new HisImpMestMedicineViewFilterQuery();
                filterImmm.MEDICINE_IDs = listIDs;
                filterImmm.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listImmmSub = new HisImpMestMedicineManager(paramGet).GetView(filterImmm);
                if (IsNotNullOrEmpty(listImmmSub))
                {
                    //listImmmSub = listImmmSub.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC || o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DK).ToList();
                    result.AddRange(listImmmSub);
                }
            }
            return result;
        }

        private List<V_HIS_EXP_MEST_MEDICINE> GetExpMestMedicine(List<HIS_MEDICINE> medicines)
        {
            List<V_HIS_EXP_MEST_MEDICINE> result = new List<V_HIS_EXP_MEST_MEDICINE>();
            var medicineIds = medicines.Select(p => p.ID).ToList();
            var skip = 0;
            while (medicineIds.Count - skip > 0)
            {
                var listIDs = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                HisExpMestMedicineViewFilterQuery filterExmm = new HisExpMestMedicineViewFilterQuery();
                filterExmm.MEDICINE_IDs = listIDs;
                filterExmm.IS_EXPORT = true;
                var listExmmSub = new HisExpMestMedicineManager(paramGet).GetView(filterExmm);
                if (IsNotNullOrEmpty(listExmmSub))
                {
                    result.AddRange(listExmmSub);
                }
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00256Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00256Filter)this.reportFilter).TIME_TO));
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Mety", ListMedicineType);
            objectTag.AddObjectData(store, "Medi", ListMedicine);
        }

    }
}
