using BaiThi.Models.Major;
using DTC.DefaultRepository.Constants;
using DTC.DefaultRepository.Exceptions;
using DTC.DefaultRepository.FromBodyModels;
using DTC.DefaultRepository.Helpers;
using DTC.MongoDB;
using FluentValidation.Results;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Project.Net8.Constants;
using Project.Net8.Installers;
using Project.Net8.Interface.Major;
using Project.Net8.Models.Major;
using Project.Net8.Models.PagingParam;
using Project.Net8.Service.Base;

namespace Project.Net8.Service.Major
{
    public class LichSuTuyenTruyenService : BaseService, ILichSuTuyenTruyenService
    {
        private DataContext _context;
        private BaseMongoDb<LichSuTuyenTruyenModel, string> BaseMongoDb;
        private readonly IMongoCollection<LichSuTuyenTruyenModel> _collection;
        private readonly IMongoCollection<BaiThiModel> _collectionBaiThi;
        protected ProjectionDefinition<LichSuTuyenTruyenModel, BsonDocument> projectionDefinition = Builders<LichSuTuyenTruyenModel>.Projection
            .Exclude("ModifiedAt")
            .Exclude("CreatedBy")
            .Exclude("ModifiedBy")
            .Exclude("IsDeleted")
            .Exclude("CreatedAtString")
            .Exclude("PasswordSalt")
            .Exclude("PasswordHash")
            .Exclude("Sort")
            .Exclude("UnsignedName");
        public LichSuTuyenTruyenService(
            DataContext context,
            IHttpContextAccessor contextAccessor) :
            base(context, contextAccessor)
        {
            _context = context;
            _collection = context.LICHSUTUYENTRUYEN;
            _collectionBaiThi = context.BAITHI;
            BaseMongoDb = new BaseMongoDb<LichSuTuyenTruyenModel, string>(_context.LICHSUTUYENTRUYEN);
        }


        public async Task<dynamic> Create(LichSuTuyenTruyenModel model)
        {
            try
            {
                if (model == default)
                throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);

             
                var result = await BaseMongoDb.CreateAsync(model);
                if (result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.CREATE_FAILURE);
                }
                    return model;
            }
            catch (ResponseMessageException e)
            {
                    throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception e)
            {
                    throw ExceptionError.Exception(e);
            }
        }

        public async Task<dynamic> Update(LichSuTuyenTruyenModel model)
        {
            try
            {
                if (model == default)
                    throw new ResponseMessageException().WithException(DefaultCode.ERROR_STRUCTURE);
                var result = await BaseMongoDb.UpdateAsync(model);
                if (result.Entity.Id == default || !result.Success)
                {
                    throw new ResponseMessageException()
                        .WithException(DefaultCode.CREATE_FAILURE);
                }
                return model;
            }
            catch (ResponseMessageException e)
            {
                throw new ResponseMessageException().WithCode(DefaultCode.EXCEPTION).WithMessage(e.ResultString).WithDetail(e.Error);
            }
            catch (Exception e)
            {
                throw ExceptionError.Exception(e);
            }
        }
        public async Task<PagingModel<LichSuTuyenTruyenModel>> GetPagingLichSu(PagingParamLichSu pagingParam)
        {
            try
            {
                PagingModel<LichSuTuyenTruyenModel> result = new PagingModel<LichSuTuyenTruyenModel>();
                var builder = Builders<LichSuTuyenTruyenModel>.Filter;
                var filter = builder.Empty;
                filter = builder.And(filter, builder.Eq("IsDeleted", false));

                if (!String.IsNullOrEmpty(pagingParam.Content))
                {
                    filter = builder.And(filter,
                        builder.Regex("Name", new BsonRegularExpression(pagingParam.Content, "i"))
                        | builder.Regex("NoiDungTuyenTruyen", new BsonRegularExpression(pagingParam.Content, "i"))
                        | builder.Regex("TenNguoiMua", new BsonRegularExpression(pagingParam.Content, "i"))
                    );
                }

                if (!string.IsNullOrEmpty(pagingParam.IdBaiThi))
                {

                    var baithiFilter = Builders<BaiThiModel>.Filter.Eq("Id", pagingParam.IdBaiThi);
                    filter = builder.And(filter,
                        builder.Eq("IdBaiThi", pagingParam.IdBaiThi)
                    );
                }

                result.TotalRows = await _collection.CountDocumentsAsync(filter);

                string sortBy = pagingParam.SortBy != null
                    ? FormatterString.HandlerSortBy(pagingParam.SortBy)
                    : "CreatedAt";
                var list = await _collection.Find(filter)
                    .Sort(pagingParam.SortDesc
                        ? Builders<LichSuTuyenTruyenModel>
                            .Sort.Descending(sortBy).Descending("CreatedAt")
                        : Builders<LichSuTuyenTruyenModel>
                            .Sort.Ascending(sortBy).Descending("CreatedAt"))
                    .Skip(pagingParam.Skip)
                    .Limit(pagingParam.Limit)
                    .Project(projectionDefinition)
                    .ToListAsync();

                result.Data = list.Select(x => BsonSerializer.Deserialize<LichSuTuyenTruyenModel>(x)).ToList();

                return result;
            }
            catch (ResponseMessageException e)
            {
                new ResultMessageResponse().WithCode(e.ResultCode)
                    .WithMessage(e.ResultString);
            }
            catch (Exception e)
            {
                throw ExceptionError.Exception(e);
            }

            return null;
        }




    }
}