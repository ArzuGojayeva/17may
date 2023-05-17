namespace NEST.ViewModels
{
    public class PaginateVm<T>
    {
        public List<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
