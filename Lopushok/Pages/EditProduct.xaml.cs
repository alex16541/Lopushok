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
using System.Data.Entity;
using Lopushok.Models;

namespace Lopushok.Pages
{
    /// <summary>
    /// Логика взаимодействия для EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Page
    {
        private Product product = new Product();
        public EditProduct(Product newProduct)
        {
            InitializeComponent();
            if(newProduct == null)
            {
                newProduct = product;
            }
            DataContext = newProduct;

            cbType.ItemsSource = DB.db.ProductType.ToList();
            cbType.SelectedValuePath = "ID";
            cbType.DisplayMemberPath = "Title";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Удалить этот продукта?", "Внимание!", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DB.db.Product.Remove(product);
                DB.db.SaveChanges();
                NavigationService.Navigate(new ProductsPage());
            }

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(tbTitle.Text))
                errors.AppendLine("Название");
            if (string.IsNullOrWhiteSpace(tbArticle.Text))
                errors.AppendLine("Артикуль");
            if (string.IsNullOrWhiteSpace(tbWorkshop.Text))
                errors.AppendLine("Номер цеха");
            if (cbType.SelectedIndex == -1)
                errors.AppendLine("Тип агента");
            if (string.IsNullOrWhiteSpace(tbPersonCount.Text))
                errors.AppendLine("Количесто работников на продукт");
            if (string.IsNullOrWhiteSpace(tbCost.Text))
                errors.AppendLine("Минимальная стоимость для агента");
            if (errors.Length > 0)
            {
                MessageBox.Show("Ошибки в:" + errors.ToString());
            }
            else
            {
                Product product = new Product();
                product.Title = tbTitle.Text;
                product.ProductTypeID = int.Parse(cbType.SelectedValue.ToString());
                product.ArticleNumber = tbArticle.Text;
                product.ProductionWorkshopNumber = int.Parse(tbWorkshop.Text);
                product.MinCostForAgent = Decimal.Parse(tbCost.Text.Replace('.',','));
                product.ProductionPersonCount = int.Parse(tbPersonCount.Text);

                DB.db.SaveChanges();
                NavigationService.Navigate(new ProductsPage());
            }
        }
    }
}
