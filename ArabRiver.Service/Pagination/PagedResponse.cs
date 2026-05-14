namespace ArabRiver.Service.Pagination
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public PagedResponse(
            IEnumerable<T> data,
            int pageNumber,
            int pageSize,
            int totalCount,
            string message = "Success")
        {
            Data = data;

            PageNumber = pageNumber;

            PageSize = pageSize;

            TotalCount = totalCount;

            TotalPages =
                (int)Math.Ceiling(
                    totalCount / (double)pageSize);

            Success = true;

            Message = message;
        }
    }
}
