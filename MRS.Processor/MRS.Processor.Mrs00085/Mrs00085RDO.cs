using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Core;
using MOS.MANAGER.HisImpMestType;
namespace MRS.Processor.Mrs00085
{
    class Mrs00085RDO
    {
        public long? EXECUTE_TIME { get; set; }
        public string EXECUTE_DATE_STR { get; set; }
        public string IMP_MEST_CODE { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public string IMP_MEST_SUB_CODE { get; set; }
        public string IMP_MEST_TYPE_CODE { get; set; }
        public string IMP_MEST_TYPE_NAME { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public long MATERIAL_ID { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public decimal BEGIN_AMOUNT { get; set; }
        public decimal? IMP_AMOUNT { get; set; }
        public decimal? EXP_AMOUNT { get; set; }
        public decimal END_AMOUNT { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string REQ_ROOM_NAME { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME1 { get; set; }
        public string EXP_MEDI_STOCK_CODE { get; set; }
        public string EXP_MEDI_STOCK_NAME { get; set; }
        public string IMP_MEDI_STOCK_CODE { get; set; }
        public string IMP_MEDI_STOCK_NAME { get; set; }
        public string CLIENT_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long? TREATMENT_ID { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string SECOND_MEST_CODE { get; set; }
        public string VIR_PATIENT_ADDRESS { get; set; }

        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }

        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public long MATERIAL_TYPE_ID { get; set; }

        public long AGGR_IMP_MEST_ID { get; set; }

        public string MATERIAL_TYPE_CODE { get; set; }
        public string RECEIVING_PLACE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }

        public string DESCRIPTION_DETAIL { get; set; }

        public string EXP_MEST_REASON_NAME { get; set; }

        public string DESCRIPTION { get; set; }

        public Mrs00085RDO(V_HIS_MATERIAL_TYPE listMaterial, Mrs00085Filter filter)
        {

            MATERIAL_TYPE_ID = listMaterial.ID;
            MATERIAL_TYPE_CODE = listMaterial.MATERIAL_TYPE_CODE;
            MATERIAL_TYPE_NAME = listMaterial.MATERIAL_TYPE_NAME;
            MANUFACTURER_NAME = listMaterial.MANUFACTURER_NAME;
            
            RECORDING_TRANSACTION = listMaterial.RECORDING_TRANSACTION;
            SERVICE_UNIT_NAME = listMaterial.SERVICE_UNIT_NAME;
                
        }

        public Mrs00085RDO(V_HIS_IMP_MEST_MATERIAL imp, List<V_HIS_IMP_MEST> Listimp, List<V_HIS_EXP_MEST> sourceMest, List<HIS_IMP_MEST_TYPE> impMestTypes)
        {
            try
            {
                AGGR_ID = imp.AGGR_IMP_MEST_ID;
                MATERIAL_TYPE_ID = imp.MATERIAL_TYPE_ID;
                MATERIAL_TYPE_CODE = imp.MATERIAL_TYPE_CODE;
                MATERIAL_TYPE_NAME = imp.MATERIAL_TYPE_NAME;
                EXECUTE_TIME = imp.IMP_TIME;
                PACKAGE_NUMBER = imp.PACKAGE_NUMBER;
                IMP_MEST_CODE = imp.IMP_MEST_CODE;
                EXPIRED_DATE = imp.EXPIRED_DATE;
                IMP_AMOUNT = imp.AMOUNT;
                if (imp.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
                    SUPPLIER_NAME = imp.SUPPLIER_NAME;

                V_HIS_IMP_MEST impMest = Listimp != null? Listimp.Where(o => o.ID == imp.IMP_MEST_ID).FirstOrDefault() : null;
                if (impMest != null)
                {
                    this.DESCRIPTION = impMest.DESCRIPTION;
                    this.CHMS_TYPE_ID = impMest.CHMS_TYPE_ID;
                    this.MOBA_EXP_MEST_ID = impMest.MOBA_EXP_MEST_ID;
                    this.IMP_MEST_SUB_CODE = impMest.IMP_MEST_SUB_CODE;
                    V_HIS_EXP_MEST chmsExpMest = sourceMest != null ? sourceMest.Where(o => o.ID == impMest.CHMS_EXP_MEST_ID).FirstOrDefault() : null;
                    if (chmsExpMest != null)
                    {
                        this.EXP_MEDI_STOCK_CODE = chmsExpMest.MEDI_STOCK_CODE;
                        this.EXP_MEDI_STOCK_NAME = chmsExpMest.MEDI_STOCK_NAME;
                        this.SECOND_MEST_CODE = chmsExpMest.EXP_MEST_CODE;
                        RECEIVING_PLACE = chmsExpMest.RECEIVING_PLACE;
                    }
                }

                var data = impMestTypes != null ? impMestTypes.Where(o => o.ID == imp.IMP_MEST_TYPE_ID).FirstOrDefault() : null;
                if (data != null)
                {
                    IMP_MEST_TYPE_CODE = data.IMP_MEST_TYPE_CODE;
                    IMP_MEST_TYPE_NAME = data.IMP_MEST_TYPE_NAME;
                }

                if (imp.AGGR_IMP_MEST_ID.HasValue)
                {
                    var firstimp = Listimp.FirstOrDefault(o => o.ID == imp.AGGR_IMP_MEST_ID);
                    if (firstimp != null)
                    {
                        data = impMestTypes != null ? impMestTypes.Where(o => o.ID == firstimp.IMP_MEST_TYPE_ID).FirstOrDefault() : null;
                        IMP_MEST_CODE = firstimp.IMP_MEST_CODE;
                        this.IMP_MEST_SUB_CODE = firstimp.IMP_MEST_SUB_CODE;
                        VIR_PATIENT_NAME = "";
                        TREATMENT_CODE = "";
                        TREATMENT_ID = null;
                        VIR_PATIENT_ADDRESS = "";

                        if (data != null)
                        {
                            IMP_MEST_TYPE_CODE = data.IMP_MEST_TYPE_CODE;
                            IMP_MEST_TYPE_NAME = data.IMP_MEST_TYPE_NAME;

                            if (firstimp.REQ_DEPARTMENT_ID.HasValue && HisDepartmentCFG.DEPARTMENTs != null)
                            {
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == firstimp.REQ_DEPARTMENT_ID.Value);
                                if (department != null)
                                {
                                    REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    REQUEST_DEPARTMENT_NAME1 = department.DEPARTMENT_NAME;
                                }
                            }
                        }
                    }
                }

                if (imp.REQ_ROOM_ID.HasValue && HisRoomCFG.HisRooms != null)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == imp.REQ_ROOM_ID.Value);
                    if (room != null)
                    {
                        REQ_ROOM_NAME = MEDI_STOCK_NAME = room.ROOM_NAME;
                    }
                }

                if (imp.REQ_DEPARTMENT_ID.HasValue && HisDepartmentCFG.DEPARTMENTs != null)
                {
                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == imp.REQ_DEPARTMENT_ID.Value);
                    if (department != null)
                    {
                        REQ_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        
                    }

                }

                SetExtendField(this);

                this.IMP_PRICE = imp.IMP_PRICE;
                this.IMP_VAT_RATIO = imp.IMP_VAT_RATIO;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00085RDO(V_HIS_EXP_MEST_MATERIAL exp, List<V_HIS_EXP_MEST> Listexp, List<V_HIS_IMP_MEST> destMest, List<HIS_EXP_MEST_TYPE> expMestTypes)
        {
            try
            {
                AGGR_ID = exp.AGGR_EXP_MEST_ID;
                MATERIAL_TYPE_ID = exp.MATERIAL_TYPE_ID;
                TDL_PATIENT_TYPE_CODE = exp.PATIENT_TYPE_CODE;
                TDL_PATIENT_TYPE_NAME = exp.PATIENT_TYPE_NAME;
                MATERIAL_TYPE_CODE = exp.MATERIAL_TYPE_CODE;
                MATERIAL_TYPE_NAME = exp.MATERIAL_TYPE_NAME;
                EXECUTE_TIME = exp.EXP_TIME;
                PACKAGE_NUMBER = exp.PACKAGE_NUMBER;
                EXPIRED_DATE = exp.EXPIRED_DATE;
                EXP_AMOUNT = exp.AMOUNT;
                EXP_MEST_CODE = exp.EXP_MEST_CODE;
                DESCRIPTION_DETAIL = exp.DESCRIPTION;
                
                V_HIS_EXP_MEST expMest = Listexp != null ? Listexp.Where(o => o.ID == exp.EXP_MEST_ID).FirstOrDefault() : null;
                if (expMest != null)
                {
                    this.DESCRIPTION = expMest.DESCRIPTION;
                    RECEIVING_PLACE = expMest.RECEIVING_PLACE;
                    this.CHMS_TYPE_ID = expMest.CHMS_TYPE_ID;
                    this.EXP_MEST_REASON_NAME = expMest.EXP_MEST_REASON_NAME;
                }
                V_HIS_EXP_MEST chmsExpMest = Listexp != null && exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK ? Listexp.FirstOrDefault(o => o.ID == exp.EXP_MEST_ID) : null;

                if (chmsExpMest != null)
                {
                    V_HIS_IMP_MEST chmsImpMest = destMest != null ? destMest.Where(o => o.CHMS_EXP_MEST_ID == chmsExpMest.ID).FirstOrDefault() : null;
                    this.IMP_MEDI_STOCK_CODE = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == chmsExpMest.IMP_MEDI_STOCK_ID).MEDI_STOCK_CODE ?? "";
                    this.IMP_MEDI_STOCK_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == chmsExpMest.IMP_MEDI_STOCK_ID).MEDI_STOCK_NAME ?? "";
                    this.SECOND_MEST_CODE = chmsImpMest != null ? chmsImpMest.IMP_MEST_CODE : "";
                  
                }

                V_HIS_EXP_MEST saleExpMest = Listexp != null && exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN ? Listexp.FirstOrDefault(o => o.ID == exp.EXP_MEST_ID) : null;
                if (saleExpMest != null)
                {
                    this.CLIENT_NAME = saleExpMest.TDL_PATIENT_NAME;
                    this.TREATMENT_CODE = saleExpMest.TDL_TREATMENT_CODE;
                    this.TREATMENT_ID = saleExpMest.TDL_TREATMENT_ID;
                    this.VIR_PATIENT_NAME = saleExpMest.TDL_PATIENT_NAME;
                    this.VIR_PATIENT_ADDRESS = saleExpMest.TDL_PATIENT_ADDRESS;
                }

                V_HIS_EXP_MEST prescription = Listexp != null && (exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                    || exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT
                    || exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT)
                    ? Listexp.FirstOrDefault(o => o.ID == exp.EXP_MEST_ID) : null;
                if (prescription != null)
                {
                    this.TREATMENT_CODE = prescription.TDL_TREATMENT_CODE;
                    this.TREATMENT_ID = prescription.TDL_TREATMENT_ID;
                    this.VIR_PATIENT_NAME = prescription.TDL_PATIENT_NAME;
                    this.VIR_PATIENT_ADDRESS = prescription.TDL_PATIENT_ADDRESS;
                }

                var data = expMestTypes != null ? expMestTypes.FirstOrDefault(o => o.ID == exp.EXP_MEST_TYPE_ID) : null;
                if (data != null)
                {
                    EXP_MEST_TYPE_NAME = data.EXP_MEST_TYPE_NAME;
                }

                if (exp.AGGR_EXP_MEST_ID.HasValue)
                {
                    var firstexp = Listexp.FirstOrDefault(o => o.ID == exp.AGGR_EXP_MEST_ID);
                    if (firstexp != null)
                    {
                        data = expMestTypes != null ? expMestTypes.FirstOrDefault(o => o.ID == firstexp.EXP_MEST_TYPE_ID) : null;
                        EXP_MEST_CODE = firstexp.EXP_MEST_CODE;
                        this.VIR_PATIENT_NAME = "";
                        this.TREATMENT_CODE = "";
                        this.TREATMENT_ID = null;
                        this.VIR_PATIENT_ADDRESS = "";

                        if (data != null)
                        {
                            EXP_MEST_TYPE_NAME = data.EXP_MEST_TYPE_NAME;

                            if (firstexp.REQ_DEPARTMENT_ID > 0 && HisDepartmentCFG.DEPARTMENTs != null)
                            {
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == firstexp.REQ_DEPARTMENT_ID);
                                if (department != null)
                                {
                                    REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    REQUEST_DEPARTMENT_NAME1 = department.DEPARTMENT_NAME;
                                }
                            }
                        }
                    }
                }

                if (exp.REQ_ROOM_ID > 0 && HisRoomCFG.HisRooms != null)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == exp.REQ_ROOM_ID);
                    if (room != null)
                    {
                        REQ_ROOM_NAME = MEDI_STOCK_NAME = room.ROOM_NAME;
                    }
                }

                if (exp.REQ_DEPARTMENT_ID > 0 && HisDepartmentCFG.DEPARTMENTs != null)
                {
                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == exp.REQ_DEPARTMENT_ID);
                    if (department != null)
                    {
                        REQ_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        
                    }
                    if (exp.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP) REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                }

                SetExtendField(this);

                this.IMP_PRICE = exp.IMP_PRICE;
                this.IMP_VAT_RATIO = exp.IMP_VAT_RATIO;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00085RDO data)
        {
            EXECUTE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.EXECUTE_TIME ?? 0);
            EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.EXPIRED_DATE ?? 0);
        }

        internal void CalculateAmount(decimal previousEndAmount)
        {
            try
            {
                BEGIN_AMOUNT = previousEndAmount;
                END_AMOUNT = BEGIN_AMOUNT + (IMP_AMOUNT.HasValue ? IMP_AMOUNT.Value : 0) - (EXP_AMOUNT.HasValue ? EXP_AMOUNT.Value : 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        internal void CalculateTotalPrice(decimal previousEndTotalPrice)
        {
            try
            {
                BEGIN_TOTAL_PRICE = previousEndTotalPrice;
                END_TOTAL_PRICE = BEGIN_TOTAL_PRICE + (IMP_AMOUNT.HasValue ? IMP_AMOUNT.Value * IMP_PRICE * (1 + IMP_VAT_RATIO) : 0) - (EXP_AMOUNT.HasValue ? EXP_AMOUNT.Value * IMP_PRICE * (1 + IMP_VAT_RATIO) : 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00085RDO() { }

        public decimal BEGIN_TOTAL_PRICE { get; set; }

        public decimal END_TOTAL_PRICE { get; set; }

        public long? CHMS_TYPE_ID { get; set; }

        public long? MOBA_EXP_MEST_ID { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public string NATIONAL_NAME { get; set; }

        public string MANUFACTURER_NAME { get; set; }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }

        public string RECORDING_TRANSACTION { get; set; }


        public long REQ_DEPARTMENT_ID { get; set; }

        public long? AGGR_ID { get; set; }

        
    }
}
