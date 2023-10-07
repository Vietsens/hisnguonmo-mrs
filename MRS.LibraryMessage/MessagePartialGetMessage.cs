namespace MRS.LibraryMessage
{
    public partial class Message
    {
        private string GetMessage(Enum enumBC)
        {
            string message = "";
            if (Language == LanguageEnum.Vietnamese)
            {
                switch (enumBC)
                {
                    case Enum.Common__ThieuThongTinBatBuoc: message = MessageViResource.Common__ThieuThongTinBatBuoc; break;
                    case Enum.Common__MaDaTonTaiTrenHeThong: message = MessageViResource.Common__MaDaTonTaiTrenHeThong; break;
                    case Enum.Common__DuLieuDangBiKhoa: message = MessageViResource.Common__DuLieuDangBiKhoa; break;
                    case Enum.Common__LoiCauHinhHeThong: message = MessageViResource.Common__LoiCauHinhHeThong; break;
                    case Enum.SarReport_SystemName: message = MessageViResource.SarReport_SystemName; break;
                    case Enum.SarReport_MessageTitleComplete: message = MessageViResource.SarReport_MessageTitleError; break;
                    case Enum.SarReport_MessageContentComplete: message = MessageViResource.SarReport_MessageContentComplete; break;
                    case Enum.SarReport_MessageTitleError: message = MessageViResource.SarReport_MessageTitleError; break;
                    case Enum.SarReport_MessageContentError: message = MessageViResource.SarReport_MessageContentError; break;
                    case Enum.Core_MrsReport_Create__BieuMauKhongCoThongTinFile: message = MessageViResource.BieuMauKhongCoThongTinFile; break;
                    case Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu: message = MessageViResource.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu; break;
                    case Enum.Core_MrsReport_Create__KhongTimDuocDLL: message = MessageViResource.Core_MrsReport_Create__KhongTimDuocDLL; break;
                    case Enum.Core_MrsReport_Create__LoiTruyVan: message = MessageViResource.Core_MrsReport_Create__LoiTruyVan; break;
                    case Enum.Core_MrsReport_Create__LoiXuLyDuLieu: message = MessageViResource.Core_MrsReport_Create__LoiXuLyDuLieu; break;
                    case Enum.Core_MrsReport_Create__LoiTemplate: message = MessageViResource.Core_MrsReport_Create__LoiTemplate; break;
                    case Enum.Core_MrsReport_Create__LoiGetData: message = MessageViResource.Core_MrsReport_Create__LoiGetData; break;
                    case Enum.Core_MrsReport_Create__LoiFss: message = MessageViResource.Core_MrsReport_Create__LoiFss; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianTuDen: message = MessageViResource.Core_MrsReport_Create__GoiHanThoiGianTuDen; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianTu: message = MessageViResource.Core_MrsReport_Create__GoiHanThoiGianTu; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianDen: message = MessageViResource.Core_MrsReport_Create__GoiHanThoiGianDen; break;

                    default: message = defaultViMessage; break;
                }
            }
            else if (Language == LanguageEnum.English)
            {
                switch (enumBC)
                {
                    case Enum.Common__ThieuThongTinBatBuoc: message = MessageEnResource.Common__ThieuThongTinBatBuoc; break;
                    case Enum.Common__MaDaTonTaiTrenHeThong: message = MessageEnResource.Common__MaDaTonTaiTrenHeThong; break;
                    case Enum.Common__DuLieuDangBiKhoa: message = MessageEnResource.Common__DuLieuDangBiKhoa; break;
                    case Enum.Common__LoiCauHinhHeThong: message = MessageEnResource.Common__LoiCauHinhHeThong; break;
                    case Enum.SarReport_SystemName: message = MessageEnResource.SarReport_SystemName; break;
                    case Enum.SarReport_MessageTitleComplete: message = MessageEnResource.SarReport_MessageTitleError; break;
                    case Enum.SarReport_MessageContentComplete: message = MessageEnResource.SarReport_MessageContentComplete; break;
                    case Enum.SarReport_MessageTitleError: message = MessageEnResource.SarReport_MessageTitleError; break;
                    case Enum.SarReport_MessageContentError: message = MessageEnResource.SarReport_MessageContentError; break;
                    case Enum.Core_MrsReport_Create__BieuMauKhongCoThongTinFile: message = MessageEnResource.BieuMauKhongCoThongTinFile; break;
                    case Enum.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu: message = MessageEnResource.Core_MrsReport_Create__KhongTimDuocFileBieuMauTrenMayChu; break;
                    case Enum.Core_MrsReport_Create__KhongTimDuocDLL: message = MessageEnResource.Core_MrsReport_Create__KhongTimDuocDLL; break;
                    case Enum.Core_MrsReport_Create__LoiTruyVan: message = MessageEnResource.Core_MrsReport_Create__LoiTruyVan; break;
                    case Enum.Core_MrsReport_Create__LoiXuLyDuLieu: message = MessageEnResource.Core_MrsReport_Create__LoiXuLyDuLieu; break;
                    case Enum.Core_MrsReport_Create__LoiTemplate: message = MessageEnResource.Core_MrsReport_Create__LoiTemplate; break;
                    case Enum.Core_MrsReport_Create__LoiGetData: message = MessageEnResource.Core_MrsReport_Create__LoiGetData; break;
                    case Enum.Core_MrsReport_Create__LoiFss: message = MessageEnResource.Core_MrsReport_Create__LoiFss; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianTuDen: message = MessageEnResource.Core_MrsReport_Create__GoiHanThoiGianTuDen; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianTu: message = MessageEnResource.Core_MrsReport_Create__GoiHanThoiGianTu; break;
                    case Enum.Core_MrsReport_Create__GoiHanThoiGianDen: message = MessageEnResource.Core_MrsReport_Create__GoiHanThoiGianDen; break;

                    default: message = defaultEnMessage; break;
                }
            }
            return message;
        }
    }
}
