

using BaiThi.Models.Major;
using DTC.DefaultRepository.Exceptions;
using DTC.DefaultRepository.FromBodyModels;
using DTC.DefaultRepository.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NKTM.Constants;
using Project.Net8.Controllers.DefaultRepository;
using Project.Net8.Installers;
using Project.Net8.Interface.Major;
using Project.Net8.Models.Major;
using Project.Net8.Models.PagingParam;

namespace Project.Net8.Controllers.Major
{
    [Route("api/v1/[controller]")]
  ///  [Authorize]
    public class LichSuTuyenTruyenController : DefaultReposityController<LichSuTuyenTruyenModel>
    {
        private readonly ILichSuTuyenTruyenService _service;
        private DataContext _dataContext;
        private static string NameCollection = DefaultNameCollection.LICHSUTUYENTRUYEN;

        public LichSuTuyenTruyenController(DataContext context, ILichSuTuyenTruyenService service,  IHttpContextAccessor httpContextAccessor)
            : base(context, NameCollection,httpContextAccessor)
        {
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] LichSuTuyenTruyenModel model)
        {
            try
            {

                var data = await _service.Create(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.CREATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] LichSuTuyenTruyenModel model)
        {
            try
            {

                var data = await _service.Update(model);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(data)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.UPDATE_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString).WithDetail(ex.Error)
                );
            }
        }

        [HttpPost]
        [Route("get-paging-params-lich-su")]
        public  async Task<IActionResult> GetPagingCore([FromBody] PagingParamLichSu pagingParam)
        {
            try
            {
                var response = await _service.GetPagingLichSu(pagingParam);
                return Ok(
                    new ResultMessageResponse()
                        .WithData(response)
                        .WithCode(DefaultCode.SUCCESS)
                        .WithMessage(DefaultMessage.GET_DATA_SUCCESS)
                );
            }
            catch (ResponseMessageException ex)
            {
                return Ok(
                    new ResultMessageResponse().WithCode(ex.ResultCode)
                        .WithMessage(ex.ResultString)
                );
            }
        }










    }
}