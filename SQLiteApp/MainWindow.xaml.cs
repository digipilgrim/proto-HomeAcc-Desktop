using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Data;

namespace PersAcc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationContext db = new ApplicationContext();

        private CollectionViewSource tramsactionViewSource;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            tramsactionViewSource = (CollectionViewSource)FindResource(nameof(tramsactionViewSource));
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            db.Database.EnsureCreated();
            db.Buckets.Load();
            db.Transactions.Load(); 
            var trans = db.Transactions.Local.ToObservableCollection();
            tramsactionViewSource.Source = trans;
            DataContext = db.Buckets.Local.ToObservableCollection();
            LoadSum();
            Sum.Text = "= " + Math.Round((decimal)db.Buckets.Sum(b => b.Score * b.Multiplier), 3).ToString();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            BucketWindow bucketWindow = new BucketWindow(new Bucket());
            if (bucketWindow.ShowDialog() == true)
            {
                Bucket bucket = bucketWindow.BucketW;
                db.Buckets.Add(bucket);
                db.SaveChanges();
            }
            LoadSum();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Bucket? bucket = bucketsList.SelectedItem as Bucket;
            if (bucket is null) return;

            BucketWindow bucketWindow = new BucketWindow(new Bucket
            {
                Id = bucket.Id,
                UnitName = bucket.UnitName,
                Score = bucket.Score
            });

            if (bucketWindow.ShowDialog() == true)
            {
                bucket = db.Buckets.Find(bucketWindow.BucketW.Id);
                if (bucket != null)
                {
                    bucket.UnitName = bucketWindow.BucketW.UnitName;
                    bucket.Score = bucketWindow.BucketW.Score;
                    db.SaveChanges();
                    bucketsList.Items.Refresh();
                }
            }
            LoadSum();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Bucket? bucket = bucketsList.SelectedItem as Bucket;
            if (bucket is null) return;

            db.Buckets.Remove(bucket);
            var transactions = db.Transactions.Where(t => t.BucketId == bucket.Id);
            foreach (var transaction in transactions)
            {
                db.Transactions.Remove(transaction);
            }
            db.SaveChanges();
            LoadSum();
        }

        private void Right_Add_Click(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = new TransactionWindow(new Transaction());
            transactionWindow.BucketComnboBox.ItemsSource = db.Buckets.ToList();
            if (transactionWindow.ShowDialog() == true)
            {
                Transaction transaction = transactionWindow.TransactionW;
                var bucket = db.Buckets.First(b => b.Id == transaction.BucketId);
                transaction.BucketId = bucket.Id;
                transaction.DateTime = DateTime.Now.Ticks;
                if (transaction.Description == null) transaction.Description = "";
                db.Transactions.Add(transaction);
                bucket.Score = bucket.Score + transaction.Value * bucket.Multiplier;
                db.SaveChanges();
                Sum.Text = "= " + Math.Round((decimal)db.Buckets.Sum(b => b.Score * b.Multiplier), 3).ToString();
            }
            ReloadValues();
        }

        private void Right_Edit_Click(object sender, RoutedEventArgs e)
        {
            Transaction? transaction = transactionsList.SelectedItem as Transaction;
            if (transaction is null) return;

            TransactionWindow transactionWindow = new TransactionWindow(new Transaction
            {
                Id = transaction.Id,
                BucketId = transaction.BucketId,
                Value = transaction.Value
            });
            transactionWindow.BucketComnboBox.ItemsSource = db.Buckets.ToList();
            if (transactionWindow.ShowDialog() == true)
            {
                transaction = db.Transactions.Find(transactionWindow.TransactionW.Id);
                if (transaction != null)
                {
                    transaction.BucketId = transactionWindow.TransactionW.BucketId;
                    transaction.Value = transactionWindow.TransactionW.Value;
                    db.SaveChanges();
                    transactionsList.Items.Refresh();
                }
            }
            ReloadValues();
        }

        private void Right_Delete_Click(object sender, RoutedEventArgs e)
        {
            Transaction? transaction = transactionsList.SelectedItem as Transaction;
            if (transaction is null) return;

            db.Transactions.Remove(transaction);
            var bucket = db.Buckets.First(b => b.Id == transaction.BucketId);
            bucket.Score = bucket.Score - transaction.Value * bucket.Multiplier;            
            db.SaveChanges();
            Sum.Text = "= " + Math.Round((decimal)db.Buckets.Sum(b => b.Score * b.Multiplier), 3).ToString();
            ReloadValues();
        }

        private void LoadSum()
        {
            foreach (var bucket in db.Buckets)
                bucket.Score = db.Transactions.Where(t => t.BucketId == bucket.Id).Sum(t => t.Value);
        }

        private void ReloadValues()
        {
            bucketsList.Items.Refresh();
            LoadSum();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            LeftPanel.Visibility = Visibility.Visible;
            UpdateLayout();
        }

        private void checkushka_Unchecked(object sender, RoutedEventArgs e)
        {
            LeftPanel.Visibility = Visibility.Hidden;
            UpdateLayout();
        }
    }
}
