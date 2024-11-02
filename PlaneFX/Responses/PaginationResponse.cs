namespace PlaneFX.Responses
{
    public class PaginationResponse<T>(IEnumerable<T> data, bool isMore)
    {
        public IEnumerable<T> Data { get; set; } = data;

        public bool IsMore { get; set; } = isMore;
    }
}