using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class CarTip_Gallery_Mapping : FullAuditedEntity<int>
    {
        public int GalleryId { get; set; }

        public int CarTipId { get; set; }

        public CarTip CarTip { get; set; }

        public Gallery Gallery { get; set; }
    }

}
