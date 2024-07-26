using BaiThi.Models.Major;
using DTC.DefaultRepository.FromBodyModels;
using Project.Net8.Models.Major;
using Project.Net8.Models.PagingParam;

namespace Project.Net8.Interface.Major
{
    public interface ILichSuTuyenTruyenService
    {
        Task<dynamic> Create(LichSuTuyenTruyenModel model);
        Task<dynamic> Update(LichSuTuyenTruyenModel model);
        Task<PagingModel<LichSuTuyenTruyenModel>> GetPagingLichSu(PagingParamLichSu pagingParam);

    }
}