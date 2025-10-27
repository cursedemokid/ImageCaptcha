using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

using WpfApp2.AppData;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ImageItem> imageItems = new ObservableCollection<ImageItem>();
        private List<Button> buttonsList = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            LoadImagesFromProject();
            AddButtonsInList();
            PicturesLb.ItemsSource = imageItems;

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content is Image image)
            {
                if (image.Source is BitmapImage bitmapImage)
                {
                    string imagePath = bitmapImage.UriSource.LocalPath;
                    string fileName = System.IO.Path.GetFileName(imagePath);
                    ImageItem imageItem = new ImageItem
                    {
                        ImagePath = imagePath,
                        FileName = fileName
                    };
                    imageItems.Add(imageItem);
                    button.Content = null;
                }
            }
        }

        private void LoadImagesFromProject()
        {
            string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string imagesFolder = System.IO.Path.Combine(projectPath, "WpfApp2", "Images");

            string[] imageFiles = Directory.GetFiles(imagesFolder).ToArray();
            var randomFiles = imageFiles.OrderBy(x => Guid.NewGuid()).ToArray();
            foreach (string filePath in randomFiles)
            {
                string fileName = System.IO.Path.GetFileName(filePath);

                imageItems.Add(new ImageItem
                {
                    ImagePath = filePath,
                    FileName = fileName
                });
            }
        }

        private void PicturesLb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PicturesLb.SelectedItem == null) return;
            ImageItem imageItem = PicturesLb.SelectedItem as ImageItem;
            if (imageItem == null) return;
            foreach (Button button in buttonsList)
            {
                if (button.Content == null)
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(imageItem.ImagePath));
                    button.Content = image;
                    button.Tag = imageItem.FileName;
                    imageItems.Remove(imageItem);
                    break;
                }
            }
        }

        private void AddButtonsInList()
        {
            buttonsList.Add(Button1);
            buttonsList.Add(Button2);
            buttonsList.Add(Button3);
            buttonsList.Add(Button4);
            buttonsList.Add(Button5);
            buttonsList.Add(Button6);
            buttonsList.Add(Button7);
            buttonsList.Add(Button8);
            buttonsList.Add(Button9);
        }

        private void CheckCaptchaBtn_Click(object sender, RoutedEventArgs e)
        {
            int count = 1;
            foreach (Button button in buttonsList)
            {
                string imageName = $"{count}.png";

                if (button.Content is Image image)
                {
                    if (image.Source is BitmapImage bitmapImage)
                    {
                        string actualImageName = System.IO.Path.GetFileName(bitmapImage.UriSource.LocalPath);

                        if (actualImageName != imageName)
                        {
                            MessageBox.Show("Капча неверная");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Капча неверная");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Капча неверная");
                    return;
                }
                count++;
            }
            MessageBox.Show("Капча верная!");
        }
    }
}
