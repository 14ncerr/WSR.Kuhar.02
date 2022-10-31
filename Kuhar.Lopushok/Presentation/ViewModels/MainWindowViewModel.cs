using Kuhar.Lopushok.Domain.Entities;
using Kuhar.Lopushok.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Kuhar.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private List<Product> _products;
        private List<Product> _displayingProducts;   
        
        private List<string> _filteringItemsList;

        private string _sortValue;
        private string _filterValue;
        public string _searchingString;

        #endregion

        #region Properties

        public List<Product> Products
        {
            get => _products;
            set => _products = value;
            
        }

        public List<Product> DisplayingProducts
        {
            get => _displayingProducts;
            set => Set(ref _displayingProducts, value, nameof(DisplayingProducts));
        }

        public List<string> SortingItemsList => new List<string>
        {
            "Без сортировки", "По названию (возр)", "По названию (уб)", "По цене (возр)", "По цене (уб)"
        };

        public List<string> FilteringItemsList
        {
            get => _filteringItemsList;
            set => Set(ref _filteringItemsList, value, nameof(FilteringItemsList));
        }

        public string SortValue
        {
            get => _sortValue;
            set
            {
                Set(ref _sortValue, value, nameof(SortValue));
                DisplayProducts();
            }
        }

        public string FilterValue
        {
            get => _filterValue;
            set
            {
                Set(ref _filterValue, value, nameof(FilterValue));
                DisplayProducts();
            }
        }

        public string SearchingString
        {
            get => _searchingString;
            set
            {
                Set(ref _searchingString, value, nameof(SearchingString));
                DisplayProducts();
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            FilteringItemsList = new List<string>();
            FilteringItemsList.Add("Без фильтра");

            FilterValue = FilteringItemsList[0];
            SortValue = SortingItemsList[0];

            using (ApplicationDbContext context = new())
            {
                foreach (var item in context.ProductTypes)
                {
                    FilteringItemsList.Add(item.Title);
                }

                Products = context.Products
                    .Include(pt => pt.ProductType)
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .ToList();
            }
            _displayingProducts = new List<Product>(_products);
        }


        #region Sorting filtering and searching realisation

        private void DisplayProducts()
        {
            DisplayingProducts = Sort(Search(Filter(_products)));
        }
       
        private List<Product> Search(List<Product> products)
        {
            if ((SearchingString == string.Empty) || (SearchingString == null))
                return products;

            var value = SearchingString.ToLower();

            return products.Where(p => p.Title.ToLower().Contains(value)).ToList();
        }

        private List<Product> Filter(List<Product> products)
        {
            if (FilterValue == FilteringItemsList[0])
                return products;
            else
                return products.Where(p => p.ProductType.Title == FilterValue).ToList();
        }

        private List<Product> Sort(List<Product> products)
        {
            if (SortValue == SortingItemsList[1])
                return products.OrderBy(p => p.Title).ToList();
            else if (SortValue == SortingItemsList[2])
                return products.OrderByDescending(p => p.Title).ToList();
            else if (SortValue == SortingItemsList[3])
                return products.OrderBy(p => p.TotalCost).ToList();
            else if (SortValue == SortingItemsList[4])
                return products.OrderByDescending(p => p.TotalCost).ToList();
            else
                return products;
        }
    }
        
        #endregion

    
}
