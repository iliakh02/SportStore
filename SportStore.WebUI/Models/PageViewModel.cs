using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class PageViewModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
        }
        public bool HasPreviousPage
        {
            get
            { 
                return (PageNumber > 1); 
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

        public bool HasPrePoints { 
            get
            {
                return PageNumber > 3;
            }
        }

        public bool HasPostPoints
        {
            get
            {
                return (TotalPages - PageNumber) > 2;
            }
        }

        public bool NeedGoToFirstPage
        {
            get
            {
                return PageNumber > 2;
            }
        }

        public bool NeedGoToLastPage
        {
            get
            {
                return (TotalPages - PageNumber) > 1;
            }
        }
    }
}
