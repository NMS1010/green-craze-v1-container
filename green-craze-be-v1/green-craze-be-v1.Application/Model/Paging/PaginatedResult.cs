namespace green_craze_be_v1.Application.Model.Paging
{
    public class PaginatedResult<T>
    {
        public int CurrentItemCount { get; set; }
        public int ItemsPerPage { get; set; }
        public long TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new List<T>();

        public PaginatedResult(List<T> items, int pageIndex, long totalCount, int pageSize)
        {
            var totalPage = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items = items;
            PageIndex = pageIndex;
            TotalItems = totalCount;
            TotalPages = totalPage;
            ItemsPerPage = pageSize;
            CurrentItemCount = items.Count;
        }
    }
}