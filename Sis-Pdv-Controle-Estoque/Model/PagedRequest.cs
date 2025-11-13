using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class PagedRequest
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber 
        { 
            get => _pageNumber; 
            set => _pageNumber = value < 1 ? 1 : value; 
        }

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize 
        { 
            get => _pageSize; 
            set => _pageSize = value < 1 ? 10 : value > 100 ? 100 : value; 
        }

        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = false;
    }
}