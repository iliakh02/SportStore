using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.WebUI.Models
{
    public class UsersSortViewModel
    {
        public UsersSortState IdSort { get; set; } 
        public UsersSortState FirstNameSort { get; set; } 
        public UsersSortState LastNameSort { get; set; }
        public UsersSortState Current { get; set; }

        public UsersSortViewModel(UsersSortState sortOrder)
        {
            IdSort = sortOrder == UsersSortState.IdAsc ? UsersSortState.IdDesk : UsersSortState.IdAsc;
            FirstNameSort = sortOrder == UsersSortState.FirstNameAsc ? UsersSortState.FirstNameDesc : UsersSortState.FirstNameAsc;
            LastNameSort = sortOrder == UsersSortState.LastNameAsc ? UsersSortState.LastNameDesc : UsersSortState.LastNameAsc;
            Current = sortOrder;
        }
    }
}
