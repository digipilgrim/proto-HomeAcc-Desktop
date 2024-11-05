using System.Windows;

namespace PersAcc
{
    /// <summary>
    /// Логика взаимодействия для BucketWindow.xaml
    /// </summary>
    public partial class BucketWindow : Window
    {
        public Bucket BucketW { get; private set; }
        public BucketWindow(Bucket bucket)
        {
            InitializeComponent();
            BucketW = bucket;
            DataContext = BucketW;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
