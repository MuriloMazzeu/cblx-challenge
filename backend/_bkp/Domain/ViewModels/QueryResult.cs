
namespace CblxChallenge.Domain.ViewModels
{
    public class QueryResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }
}
