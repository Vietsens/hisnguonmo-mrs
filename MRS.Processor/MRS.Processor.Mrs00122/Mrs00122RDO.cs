using MOS.EFMODEL.DataModels; 
using System;
using System.Collections.Generic;

namespace MRS.Processor.Mrs00122
{
    class Mrs00122RDO
    {
        public const int MEDICINE = 1;
        public const int MATERIAL = 2;
        public const int CHEMICAL_SUBSTANCE =3;

        public bool IsChms { get; set; } //la nhap chuyen tu kho khac
        public int Type { get; set; } //Xac dinh loai la vat tu hay thuoc. type = 1: thuoc, type = 2: vat tu
        public long? ImpMediStockId { get; set; } //ImpMediStockId
        public long Id { get; set; } //medicine_id hoac material_id
        public long TypeId { get; set; } //medicine_type_id hoac material_type_id
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
        public string ParentName { get; set; }
        public string ParentCode { get; set; }
        public string Concentra { get; set; }
        public string ServiceUnitName { get;  set;  }
        public string ServiceUnitCode { get;  set;  }
        public decimal ImpPrice { get;  set;  }
        public decimal ImpVatRatio { get;  set;  }
        public decimal? InternalPrice { get;  set;  }
        public string PackageNumber { get;  set;  }
        public string SupplierName { get;  set;  }
        public string SupplierCode { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountBhyt { get; set; }
        public decimal AmountVp { get; set; }
        public decimal AmountFree { get; set; }
        public decimal AmountExpend { get; set; }
        public decimal AmountHpKp { get; set; }
        public decimal AmountNuocNgoai { get; set; }
        public decimal AmountTreEm{ get; set; }
        public decimal AmountNguoiNgheo { get; set; }
        public decimal AmountMoba { get;  set;  }
        public decimal AmountTruct { get;  set;  }
        public string BidNumber { get; set; }
        public string ImpTimeStr { get; set; }
        public string ExpTimeStr { get; set; }
        public string ExpiredDateStr { get;  set;  }
        public string ExpMestCodeStr { get;  set;  }
        public long ExpMestId { get;  set;  }
        public string MediStockName { get; set; }
        public long MediStockId { get; set; }
        public string ManuFacturerName { get; set; }

        public decimal AmountMobaInTime { get; set; }
        public string ReqDepartmentCode { get; set; }
        public string ReqDepartmentName { get; set; }
        public long ReqDepartmentId { get; set; }
        public string BietDuoc { get; set; }
        public string MaHoatChat { get; set; }
        public string NationalName { get; set; }
        public long ExpMestTypeId { get; set; }
        public long ExpMestReasonId { get; set; }
        public short IsChemicalSubstance { get; set; }
        public string ImpStockThenExpTypeName { get; set; }
        public Dictionary<string, decimal> DIC_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_EXP_TYPE_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_EXP_REASON_AMOUNT { get; set; }
        public Mrs00122RDO()
        {
            DIC_AMOUNT = new Dictionary<string, decimal>();
            DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_EXP_TYPE_AMOUNT = new Dictionary<string, decimal>();
            DIC_EXP_REASON_AMOUNT = new Dictionary<string, decimal>();
            DIC_IMP_TYPE_AMOUNT = new Dictionary<string, decimal>();
        }

        public decimal AmountChms { get; set; }

        public long ImpMestTypeId { get; set; }

        public Dictionary<string, decimal> DIC_IMP_TYPE_AMOUNT { get; set; }

        public string RegisterNumber { get; set; }

        public decimal AmountMobaInTimeBhyt { get; set; }

        public decimal AmountMobaInTimeVp { get; set; }
    }
    public class MedicineTypeRdo
    {
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_EXP_TYPE_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_EXP_REASON_AMOUNT { get; set; }
        public MedicineTypeRdo()
        {
            DIC_AMOUNT = new Dictionary<string, decimal>();
            DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_EXP_TYPE_AMOUNT = new Dictionary<string, decimal>();
            DIC_EXP_REASON_AMOUNT = new Dictionary<string, decimal>();
            DIC_IMP_TYPE_AMOUNT = new Dictionary<string, decimal>();
        }

        public Dictionary<string, decimal> DIC_IMP_TYPE_AMOUNT { get; set; }
    }

    public class ParentMedicine
    {
        public long MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }
    }

    public class ParentMaterial
    {
        public long MATERIAL_TYPE_ID { get; set; }
        public string PARENT_MATERIAL_TYPE_CODE { get; set; }
        public string PARENT_MATERIAL_TYPE_NAME { get; set; }
    }
}
