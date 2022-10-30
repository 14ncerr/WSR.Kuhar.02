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
                SortProducts(GetProducts(), value);
                OnPropertyChanged(nameof(SelectedSortingItem));             
            }
        }

        public string SelectedFilteringItem
        {
            get => _selectedFilteringItem;
            set
            {
                _selectedFilteringItem = value;
                OnPropertyChanged(nameof(SelectedFilteringItem));
            }
        }

        public string SearchingString
        {
            get => _searchingString;
            set
            {
                _searchingString = value;
                Products = Searching(GetProducts(), value);
                OnPropertyChanged(nameof(SearchingString));           
            }
        }

        #endregion

        public MainWindowViewModel()
        {
            Products = GetProducts();
            SortingItemsList = GetSortingItems();
            FilteringItemsList = GetFilteringItems();
            SelectedFilteringItem = "Без фильтров";
            SelectedSortingItem = "Без сортировки";

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
            var sorters = new List<string>();
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

        private List<Product> Searching(List<Product> incomingProducts, string value)
        {
            List<Product> SearchList = new();
            if (value == string.Empty || value == "")
                SearchList = incomingProducts;
            else
                SearchList = incomingProducts.Where(p => p.Title.ToLower().Contains(value.ToLower())).ToList();

            return SearchList;
        }

        private List<Product> SortProducts(List<Product> incomingProducts, string value)
        {
            if (SearchingString != "")
                return SortingWithSearching(incomingProducts, value);
            else
                return SortingWithoutSearching(incomingProducts, value);
          
        }
        private List<Product> SortingWithSearching(List<Product> list, string sortingString)
        {
            List<Product> sortingList = new();

            if (sortingString == "Без сортировки")
                sortingList = GetProducts();

            else if (sortingString == "По названию (возр.)")
            {
                sortingList = list.OrderBy(p => p.Title).ToList();
                sortingList = Searching(sortingList, SearchingString);
            }

            else if (sortingString == "По названию (убыв.)")
            {
                sortingList = list.OrderByDescending(p => p.Title).ToList();
                sortingList = Searching(sortingList, SearchingString);
            }

            else if (sortingString == "По возрастанию стоимости")
            {
                sortingList = list.OrderBy(p => p.TotalCost).ToList();
                sortingList = Searching(sortingList, SearchingString);
            }

            else if (sortingString == "По убыванию стоимости")
            {
                sortingList = list.OrderByDescending(p => p.TotalCost).ToList();
                sortingList = Searching(sortingList, SearchingString);
            }

            return sortingList;
        }
        private List<Product> SortingWithoutSearching(List<Product> list, string sortingString)
        {
            List<Product> sortingList = new();

            if (sortingString == "Без сортировки")
                sortingList = GetProducts();

            else if (sortingString == "По названию (возр.)")
                sortingList = list.OrderBy(p => p.Title).ToList();
           
            else if (sortingString == "По названию (убыв.)")
                sortingList = list.OrderByDescending(p => p.Title).ToList();
          
            else if (sortingString == "По возрастанию стоимости")
                sortingList = list.OrderBy(p => p.TotalCost).ToList();
        
            else if (sortingString == "По убыванию стоимости")
                sortingList = list.OrderByDescending(p => p.TotalCost).ToList();

            return sortingList;
        }



        #endregion

    }
}
