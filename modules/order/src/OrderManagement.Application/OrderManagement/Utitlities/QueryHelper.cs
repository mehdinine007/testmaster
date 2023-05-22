using OrderManagement.Application.Contracts;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace OrderManagement.Application.OrderManagement.Utitlities
{
    public static class QueryHelper
    {
        public static async Task<List<SaleDetailDto>> MapSaleDetailsToDto(this List<SaleDetail> saleDetails, IRepository<Gallery, int> galleryRepository, IObjectMapper objectMapper, string carTipImageBaseUrls)
        {
            if (galleryRepository == null)
                throw new ArgumentNullException(nameof(galleryRepository));

            var carTipGalleryImageRelations = new Dictionary<int, List<int>>();//cartipId //galleryRecordIds
            var allRelateGalleryImageIds = new List<int>();
            saleDetails.ForEach(x =>
            {
                if (!carTipGalleryImageRelations.TryGetValue(x.CarTipId, out var _))
                {
                    var galleryIds = x.CarTip.CarTip_Gallery_Mappings.Select(y => y.GalleryId).ToList();
                    carTipGalleryImageRelations.Add(x.CarTipId, galleryIds);
                    allRelateGalleryImageIds.AddRange(galleryIds);
                }
            });
            var allReltaedGAlleryImages = galleryRepository.WithDetails().Where(x => allRelateGalleryImageIds.Any(y => y == x.Id)).ToList();
            var saleDetailDtos = objectMapper.Map<List<SaleDetail>, List<SaleDetailDto>>(saleDetails, new List<SaleDetailDto>());
            saleDetailDtos.ForEach(x =>
            {
                if (carTipGalleryImageRelations.TryGetValue(x.CarTipId, out List<int> relatedImageIds))
                {
                    x.CarTipImageUrls = allReltaedGAlleryImages.Where(y => relatedImageIds.Any(z => z == y.Id)).Select(y =>y.ImageUrl).ToList();
                }
            });
            return saleDetailDtos;
        }
    }
}
