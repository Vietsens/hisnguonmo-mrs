using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMaty;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00004
{
    public class Mrs00004Processor : AbstractProcessor
    {
        Mrs00004Filter castFilter = null;
        List<Mrs00004RDO> listRdo = new List<Mrs00004RDO>();
        private decimal beginAmount;
        private decimal ERROR_BEGIN_AMOUNT = -1;
        List<V_HIS_IMP_MEST_MATERIAL> listImp;
        List<V_HIS_EXP_MEST_MATERIAL> listExp;

        public Mrs00004Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00004Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00004Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
                HisImpMestMaterialViewFilterQuery impFilter = new HisImpMestMaterialViewFilterQuery();
                impFilter.MATERIAL_TYPE_ID = castFilter.MATERIAL_TYPE_ID;
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    impFilter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID.Value;
                }
                else
                {
                    impFilter.HAS_MEDI_STOCK_PERIOD = false;
                    if (castFilter.MEDI_STOCK_ID.HasValue)
                    {
                        impFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    }
                    else
                    {
                        ///Loi code, neu khong gui thong tin ky thi phai gui thong tin kho
                        ///Co tinh gan kho "ao" de khong the xuat ra du lieu
                        impFilter.MEDI_STOCK_ID = -1;
                        LogSystem.Error("Khong gui thong tin kho cung nhu thong tin ky. Kiem tra lai frontend.");
                    }
                }
                listImp = new HisImpMestMaterialManager(getParam).GetView(impFilter);

                HisExpMestMaterialViewFilterQuery expFilter = new HisExpMestMaterialViewFilterQuery();
                expFilter.MATERIAL_TYPE_ID = castFilter.MATERIAL_TYPE_ID;
                expFilter.IS_EXPORT = true;
                //expFilter.IN_EXECUTE = true; 
                if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
                {
                    expFilter.MEDI_STOCK_PERIOD_ID = castFilter.MEDI_STOCK_PERIOD_ID.Value;
                }
                else
                {
                    expFilter.HAS_MEDI_STOCK_PERIOD = false;
                    if (castFilter.MEDI_STOCK_ID.HasValue)
                    {
                        expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    }
                    else
                    {
                        ///Loi code, neu khong gui thong tin ky thi phai gui thong tin kho
                        ///Co tinh gan kho "ao" de khong the xuat ra du lieu
                        expFilter.MEDI_STOCK_ID = -1;
                        LogSystem.Error("Khong gui thong tin kho cung nhu thong tin ky. Kiem tra lai frontend.");
                    }
                }
                listExp = new HisExpMestMaterialManager(getParam).GetView(expFilter);

                if (getParam.HasException)
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessBeginAmount()
        {
            if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
            {
                var period = new HisMediStockPeriodManager().GetById(castFilter.MEDI_STOCK_PERIOD_ID.Value);
                if (period != null)
                {
                    if (period.PREVIOUS_ID.HasValue)
                    {
                        SetBeginAmount(period.PREVIOUS_ID.Value);
                    }
                    else
                    {
                        ///Ky dau tien
                        beginAmount = 0;
                    }
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT;
                    Logging("Khong xac dinh duoc period tu id truyen len. Kiem tra lai frontend.", LogType.Warn);
                }
            }
            else
            {
                ///Khong co thong tin ky. NSD muon lay theo ky hien tai (chua chot) ==> Su dung MEDI_STOCK_ID.
                ///Truong hop MEDI_STOCK_ID null thi duoc hieu la truong hop loi cua client.
                if (castFilter.MEDI_STOCK_ID.HasValue)
                {

                    var period = new HisMediStockPeriodManager().GetTheLast(castFilter.MEDI_STOCK_ID.Value);
                    if (period != null)
                    {
                        SetBeginAmount(period.ID);
                    }
                    else
                    {
                        ///Chua co ky nao
                        beginAmount = 0;
                    }
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT;
                    Logging("Khong xac dinh medi_stock_id. Kiem tra lai frontend.", LogType.Warn);
                }
            }
        }

        private void SetBeginAmount(long periodId)
        {
            HisMestPeriodMatyFilterQuery mestPeriodMatyFilter = new HisMestPeriodMatyFilterQuery();
            mestPeriodMatyFilter.MEDI_STOCK_PERIOD_ID = periodId;
            mestPeriodMatyFilter.MATERIAL_TYPE_ID = castFilter.MATERIAL_TYPE_ID;
            var listMestPeriodMaty = new HisMestPeriodMatyManager().Get(mestPeriodMatyFilter);
            if (listMestPeriodMaty != null && listMestPeriodMaty.Count > 0)
            {
                ///Luon co 1 ban ghi neu nhu phan tong hop ky du lieu xu ly dung.
                ///Trong truong hop co > 1 ban ghi thi ghi log dong thoi set so dau ky = -1 de NSD nhan dien va thong bao loi nham xu ly
                if (listMestPeriodMaty.Count == 1)
                {
                    beginAmount = listMestPeriodMaty[0].BEGIN_AMOUNT;
                }
                else
                {
                    beginAmount = ERROR_BEGIN_AMOUNT;
                    Logging("Co nhieu hon 1 ban ghi co cung MATERIAL_TYPE_ID va MEDI_STOCK_PERIOD_ID trong bang HIS_MEST_PERIOD_MATY. Kiem tra lai unique constraint va code phan tong hop chot ky du lieu.", LogType.Error);
                }
            }
            else
            {
                ///Ky truoc do khong co du lieu ve loai thuoc
                beginAmount = 0;
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessBeginAmount();

                List<Mrs00004RDO> listRdoTemp = new List<Mrs00004RDO>();
                foreach (var imp in listImp)
                {
                    Mrs00004RDO rdo = new Mrs00004RDO(imp);
                    listRdoTemp.Add(rdo);
                }
                foreach (var exp in listExp)
                {
                    Mrs00004RDO rdo = new Mrs00004RDO(exp);
                    listRdoTemp.Add(rdo);
                }

                listRdo = listRdoTemp.OrderBy(o => o.EXECUTE_TIME).ToList();
                decimal previousEndAmount = beginAmount;
                foreach (var rdo in listRdo)
                {
                    rdo.CalculateAmount(previousEndAmount);
                    previousEndAmount = rdo.END_AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var materialType = new HisMaterialTypeManager().GetViewById(castFilter.MATERIAL_TYPE_ID);
            if (materialType != null)
            {
                dicSingleTag.Add("MATERIAL_TYPE_CODE", materialType.MATERIAL_TYPE_CODE);
                dicSingleTag.Add("MATERIAL_TYPE_NAME", materialType.MATERIAL_TYPE_NAME);
                dicSingleTag.Add("SERVICE_UNIT_NAME", materialType.MATERIAL_TYPE_NAME);
                dicSingleTag.Add("NATIONAL_NAME", materialType.MATERIAL_TYPE_NAME);
                dicSingleTag.Add("MANUFACTURER_NAME", materialType.MATERIAL_TYPE_NAME);
            }
            if (castFilter.MEDI_STOCK_PERIOD_ID.HasValue)
            {
                var period = new HisMediStockPeriodManager().GetViewById(castFilter.MEDI_STOCK_PERIOD_ID.Value);
                if (period != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_PERIOD_NAME", period.MEDI_STOCK_PERIOD_NAME);
                    dicSingleTag.Add("MEDI_STOCK_CODE", period.MEDI_STOCK_CODE);
                    dicSingleTag.Add("MEDI_STOCK_NAME", period.MEDI_STOCK_NAME);
                    if (period.CREATE_TIME.HasValue)
                    {
                        dicSingleTag.Add("MEDI_STOCK_PERIOD_TO_TIME_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(period.CREATE_TIME.Value));
                    }
                }
            }

            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
