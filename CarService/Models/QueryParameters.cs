namespace CarService.Models
{
    // Implementation of pagination
    public class QueryParameters
    {
        const int MaxSize = 100;
        private int _pageSize = 20;

        public int Page { get; set; } = 1;

        public int Size
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = Math.Min(_pageSize, value);
            }
        }

    }
}
