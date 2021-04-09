namespace SportStore.WebUI.Models
{
    public class ProductsSortViewModel
    {
        public ProductsSortState IdSort { get; set; }
        public ProductsSortState NameSort { get; set; }
        public ProductsSortState ProducerSort { get; set; }
        public ProductsSortState PriceSort { get; set; }
        public ProductsSortState Current { get; set; }

        public ProductsSortViewModel(ProductsSortState sortOrder)
        {
            IdSort = sortOrder == ProductsSortState.IdAsc ? ProductsSortState.IdDesc : ProductsSortState.IdAsc;
            NameSort = sortOrder == ProductsSortState.NameAsc ? ProductsSortState.NameDesc : ProductsSortState.NameAsc;
            ProducerSort = sortOrder == ProductsSortState.ProducerAsc ? ProductsSortState.ProducerDesc : ProductsSortState.ProducerAsc;
            PriceSort = sortOrder == ProductsSortState.PriceAsc ? ProductsSortState.PriceDesc : ProductsSortState.PriceAsc;
            Current = sortOrder;
        }
    }
}
