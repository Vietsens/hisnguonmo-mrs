using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType
{
    public class IMP_EXP_MEST_TYPE
    {
        public decimal ID__BCS_IMP_AMOUNT { get; set; }// 8;nhập bù cơ số
        public decimal ID__BL_IMP_AMOUNT { get; set; }// 9;nhập bù lẻ
        public decimal ID__CK_IMP_AMOUNT { get; set; }// 1;nhập chuyển kho
        public decimal ID__DK_IMP_AMOUNT { get; set; }// 6;nhập đầu kỳ
        public decimal ID__DMTL_IMP_AMOUNT { get; set; }// 12;nhập đơn máu trả lại
        public decimal ID__DNTTL_IMP_AMOUNT { get; set; }// 10;nhập đơn nội trú trả lại
        public decimal ID__DTTTL_IMP_AMOUNT { get; set; }// 11;nhập đơn tủ trực trả lại
        public decimal ID__HPTL_IMP_AMOUNT { get; set; }// 13;nhập hao phí trả lại
        public decimal ID__KK_IMP_AMOUNT { get; set; }// 5;nhập kiểm kê
        public decimal ID__KHAC_IMP_AMOUNT { get; set; }// 7;nhập khác
        public decimal ID__NCC_IMP_AMOUNT { get; set; }// 2;nhập nhà cung cấp
        public decimal ID__TH_IMP_AMOUNT { get; set; }// 3;nhập thu hồi
        public decimal ID__THT_IMP_AMOUNT { get; set; }// 4;nhập tổng hợp trả
        public decimal ID__BTL_IMP_AMOUNT { get; set; }// 15;nhập bán trả lại
        public decimal ID__BCT_IMP_AMOUNT { get; set; }// 14;nhập bào chế thuốc
        public decimal ID__DONKTL_IMP_AMOUNT { get; set; }// 16  -  Phong Kham Tra Lai;
        public decimal ID__TSD_IMP_AMOUNT { get; set; }// 17  -  Nhap Tai Su Dung;


        public decimal ID__BAN_EXP_AMOUNT { get; set; }// 8;xuất bán
        public decimal ID__BCS_EXP_AMOUNT { get; set; }// 5;xuất bù cơ số
        public decimal ID__BL_EXP_AMOUNT { get; set; }// 13;xuất bù lẻ
        public decimal ID__CK_EXP_AMOUNT { get; set; }// 3;xuất chuyển kho
        public decimal ID__DM_EXP_AMOUNT { get; set; }// 12;xuất đơn máu
        public decimal ID__DNT_EXP_AMOUNT { get; set; }// 9;xuất đơn nội trú
        public decimal ID__BCT_EXP_AMOUNT { get; set; }// 14;xuất báo chế thuốc
        public decimal ID__DPK_EXP_AMOUNT { get; set; }// 1;xuất đơn phòng khám
        public decimal ID__DTT_EXP_AMOUNT { get; set; }// 11;xuất đơn tủ trực
        public decimal ID__HPKP_EXP_AMOUNT { get; set; }// 2;xuất hao phí khoa phòng
        public decimal ID__KHAC_EXP_AMOUNT { get; set; }// 10;xuất khác
        public decimal ID__PL_EXP_AMOUNT { get; set; }// 7;xuất phiếu lĩnh
        public decimal ID__TNCC_EXP_AMOUNT { get; set; }// 4;xuất trả nhà cung cấp
        public decimal ID__PKTH_EXP_AMOUNT { get; set; }// 15 - Don Phong Kham Tong Hop;
        public decimal ID__VTMA_EXP_AMOUNT { get; set; }// 16 - Uong Vitamin A;
        public decimal ID__VACC_EXP_AMOUNT { get; set; }// 17 - Vacxin;
    }
}
