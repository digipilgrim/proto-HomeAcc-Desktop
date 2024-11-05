using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace PersAcc
{
    /// <summary>
    /// Логика взаимодействия для TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public Transaction TransactionW { get; private set; }
        public TransactionWindow(Transaction transaction)
        {
            InitializeComponent();
            TransactionW = transaction;
            DataContext = TransactionW;
        }


        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            DataContext = TransactionW;
            TransactionW.BucketId = (int) BucketComnboBox.SelectedValue;
        }
    }
}
