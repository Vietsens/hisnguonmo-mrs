namespace MRS.LibraryBug
{
    public partial class Bug
    {
        private string GetCode(Enum enumBC)
        {
            string code = "";
            switch (enumBC)
            {
                case Enum.Common__KXDDDuLieuCanXuLy:
                    code = CodeResource.Common__KXDDDuLieuCanXuLy;
                    break;
                case Enum.Common__ThieuThongTinBatBuoc:
                    code = CodeResource.Common__ThieuThongTinBatBuoc;
                    break;
                case Enum.Common__LoiCauHinhHeThong:
                    code = CodeResource.Common__LoiCauHinhHeThong;
                    break;
                case Enum.Common__FactoryKhoiTaoDoiTuongThatBai:
                    code = CodeResource.Common__FactoryKhoiTaoDoiTuongThatBai;
                    break;
                case Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu:
                    code = CodeResource.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu;
                    break;
                case Enum.Core_MrsReport_Create__KhongTimThayThongTinBieuMau:
                    code = CodeResource.Core_MrsReport_Create__KhongTimThayThongTinBieuMau;
                    break;
                case Enum.Core_MrsReport_Create__LoaiBaoCaoVaBieuMauKhongCungCap:
                    code = CodeResource.Core_MrsReport_Create__LoaiBaoCaoVaBieuMauKhongCungCap;
                    break;
                case Enum.Core_MrsReport_Create__TaoThuMucChuaBaoCaoThatBai:
                    code = CodeResource.Core_MrsReport_Create__TaoThuMucChuaBaoCaoThatBai;
                    break;
                case Enum.Core_MrsReport_Create__KhongTimThayDuLieuTheoYeuCau:
                    code = CodeResource.Core_MrsReport_Create__KhongTimThayDuLieuTheoYeuCau;
                    break;

                default: code = defaultViMessage; break;
            }
            return code;
        }
    }
}
