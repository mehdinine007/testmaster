using Volo.Abp.Domain.Entities;

namespace Esale.Core.Bases
{
    public class BaseReadOnlyTable : Entity<int>
    {
        public BaseReadOnlyTable(int id, string title_En, string title, int code)
        {
            Title = title;
            Code = code;
            Title_En = title_En;
        }


        public BaseReadOnlyTable()
        {

        }

        public int Id { get; set; }

        public string Title_En { get; set; }

        public string Title { get; set; }

        public int Code { get; set; }
    }

}
