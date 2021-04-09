namespace SportStore.WebUI.Models
{
    public class CategoriesSortViewModel
    {
        public CategoriesSortState IdSort { get; set; }
        public CategoriesSortState NameSort { get; set; }
        public CategoriesSortState Current { get; set; }

        public CategoriesSortViewModel(CategoriesSortState sortOrder)
        {
            IdSort = sortOrder == CategoriesSortState.IdAsc ? CategoriesSortState.IdDesc : CategoriesSortState.IdAsc;
            NameSort = sortOrder == CategoriesSortState.NameAsc ? CategoriesSortState.NameDesc : CategoriesSortState.NameAsc;
            Current = sortOrder;
        }
    }
}
