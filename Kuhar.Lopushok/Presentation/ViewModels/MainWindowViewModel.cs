using Kuhar.Lopushok.Domain.Entities;
using Kuhar.Lopushok.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Kuhar.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private List<Product> _products = new();
        private List<Product> _displayingProducts;      
        private List<string> _sortingItemsList = new();
        private List<string> _filteringItemsList = new();
        private string _selectedSortingItem = null!;
        private string _selectedFilteringItem = null!;
        public string _searchingString = "";

        #endregion

        #region Properties

        public List<Product> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public List<Product> DisplayingProducts
        {
            get => _displayingProducts; 
            set
            {
                _displayingProducts = value;
                OnPropertyChanged(nameof(DisplayingProducts));
            } 
        }

        public List<string> SortingItemsList
        {
            get => _sortingItemsList;
            set => _sortingItemsList = value;

        }

        public List<string> FilteringItemsList
        {
            get => _filteringItemsList;
            set => _filteringItemsList = value;
        }

        public string SelectedSortingItem
        {
            get => _selectedSortingItem;
            set
            {
                _selectedSortingItem = value;
                OnPropertyChanged(nameof(SelectedSortingItem));             
            }
        }

        public string SelectedFilteringItem
        {
            get => _selectedFilteringItem;
            set
            {
                _selectedFilteringItem = value;
                Products = FiltProducts(GetProducts());
                OnPropertyChanged(nameof(SelectedFilteringItem));
            }
        }

        public string SearchingString
        {
            get => _searchingString;
            set
            {
                _searchingString = value;
                Products = Searching(GetProducts());
                OnPropertyChanged(nameof(SearchingString));           
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            Products = GetProducts();
            SortingItemsList = GetSortingItems();
            FilteringItemsList = GetFilteringItems();
            SelectedSortingItem = "Без сортировки";
            SelectedFilteringItem = "Без фильтров";
        }

        #region GetMethods

        private List<Product> GetProducts()
        {
            using (ApplicationDbContext context = new())
            {
                var products = context.Products
                    .Include(pt => pt.ProductType)
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .ToList();

                return products;
            }
        }

        private List<string> GetSortingItems()
        {
            List<string> sorters = new();
            sorters.Add("Без сортировки");
            sorters.Add("По названию (возр.)");
            sorters.Add("По названию (убыв.)");
            sorters.Add("По возрастанию стоимости");
            sorters.Add("По убыванию стоимости");

            return sorters;
        }

        private List<string> GetFilteringItems()
        {
            var filters = new List<string>();
            filters.Add("Без фильтров");

            using (ApplicationDbContext context = new())
            {
                foreach (var item in context.ProductTypes)
                {
                    filters.Add(item.Title);
                }
            }
            return filters;
        }

        #endregion

        #region Sorting filtering and searching realisation

        private List<Product> Searching(List<Product> incomingProducts)
        {
            List<Product> SearchList = new();
            if (SearchingString == string.Empty || SearchingString == "")
                return incomingProducts;
            else
                return incomingProducts.Where(p => p.Title.ToLower().Contains(SearchingString.ToLower())).ToList();        
        }

        private List<Product> FiltProducts(List<Product> incomingProducts)
        {
            if (SelectedFilteringItem == FilteringItemsList[0])
                return incomingProducts;
            else
                return incomingProducts.Where(p => p.ProductType.Title == SelectedFilteringItem).ToList();
        }
        
        #endregion

    }
}
