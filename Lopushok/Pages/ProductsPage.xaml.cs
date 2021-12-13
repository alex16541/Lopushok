using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lopushok.Models;
using System.Data.Entity;

namespace Lopushok.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        private List<Product> products = DB.db.Product.ToList();
        public ProductsPage()
        {
            InitializeComponent();
            btnEdit.Visibility = Visibility.Hidden;
            lbProducts.ItemsSource = products;
            List<ProductType> productTypes = DB.db.ProductType.ToList();
            productTypes.Insert(0, new ProductType { Title = "Все типы" });
            cbFilter.ItemsSource = productTypes;
            cbFilter.SelectedValuePath = "ID";
            cbFilter.DisplayMemberPath = "Title";
            cbFilter.SelectedIndex = 0;

            var sort = new List<string>();
            sort.Add("Сортировка");
            sort.Add("По названию");
            sort.Add("По номеру цеха");
            sort.Add("По мин. стоимости");

            cbSort.ItemsSource = sort;
            cbSort.SelectedIndex = 0;

            rbAsc.IsChecked = true;
            rbAsc.IsEnabled = false;
            rbDesc.IsEnabled = false;

        }

        private void rbAsc_Click(object sender, RoutedEventArgs e)
        {
            searchProduct();
        }

        private void rbDesc_Click(object sender, RoutedEventArgs e)
        {
            searchProduct();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProduct());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditProduct(lbProducts.SelectedItem as Product));
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (lbProducts.SelectedItems.Count > 0)
            {
                btnEdit.Visibility = Visibility.Visible;
            }
            else
            {
                btnEdit.Visibility = Visibility.Hidden;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchProduct();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchProduct();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchProduct();
        }

        private void searchProduct()
        {
            if (tbSearch.Text != "")
            {
                products = DB.db.Product.Where(p => (p.Title + p.ProductType.Title).ToLower().Contains(tbSearch.Text)).ToList();

            }
            else
            {
                products = DB.db.Product.ToList();
            }

            if (cbFilter.SelectedIndex != 0)
            {
                products = products.Where(p => p.ProductTypeID == int.Parse(cbFilter.SelectedValue.ToString())).ToList();
            }

            if (cbSort.SelectedIndex != 0)
            {
                rbAsc.IsEnabled = true;
                rbDesc.IsEnabled = true;
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        if (rbAsc.IsChecked == true)
                            products = products.OrderBy(p => p.Title).ToList();
                        else
                            products = products.OrderByDescending(p => p.Title).ToList();
                        break;
                    case 2:
                        if (rbAsc.IsChecked == true)
                            products = products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                        else
                            products = products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                        break;
                    case 3:
                        if (rbAsc.IsChecked == true)
                            products = products.OrderBy(p => p.MinCostForAgent).ToList();
                        else
                            products = products.OrderByDescending(p => p.MinCostForAgent).ToList();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                rbAsc.IsEnabled = false;
                rbDesc.IsEnabled = false;
            }

            lbProducts.ItemsSource = products;
        }
    }
}
