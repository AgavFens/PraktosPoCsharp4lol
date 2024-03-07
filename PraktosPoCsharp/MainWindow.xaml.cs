using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Permissions;
using System.Diagnostics.Eventing.Reader;

namespace PraktosPoCsharp
{
    public partial class MainWindow : Window
    {
        List<string> muzikaList = new List<string>();
        MediaPlayer mediaplayer = new MediaPlayer();
        List<string> files;
        int selectedIndex;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PapkaSmuzikoy_Click(object sender, RoutedEventArgs e)
        {
            mediaplayer.Volume = Slider_Zvuka.Value;
            CommonOpenFileDialog dialog = new CommonOpenFileDialog() { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                files = Directory.GetFiles(dialog.FileName, "*.mp3").ToList();
                if (files.Count > 0)
                {
                    foreach (string file in files)
                    {
                        muzikaList.Add(System.IO.Path.GetFileName(file));
                    }
                    ListBoxMuzika.ItemsSource = null;
                    ListBoxMuzika.ItemsSource = muzikaList;
                    mediaplayer.Open(new Uri(files[0]));
                    mediaplayer.Play();
                }
                else
                {
                    MessageBox.Show("Нету файлов с музончиками в выбранной папке");
                }
            }
        }


        private void ListBoxMuzika_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxMuzika.SelectedItem != null)
            {
                selectedIndex = ListBoxMuzika.SelectedIndex;
                mediaplayer.Open(new Uri(files[selectedIndex]));
                mediaplayer.Play();
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaplayer.Position = new TimeSpan(Convert.ToInt64(Slider.Value));

        }
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            Slider.Maximum = mediaplayer.NaturalDuration.TimeSpan.Ticks;
        }

        private void Slider_Zvuka_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaplayer.Volume = Slider_Zvuka.Value;
            Slider_Zvuka.Maximum = 1;
        }

        private void NazadKnopka_Click(object sender, RoutedEventArgs e)
        {
            if (mediaplayer.NaturalDuration.HasTimeSpan && selectedIndex != 0)
            {
                selectedIndex -= 1;
                mediaplayer.Open(new Uri(files[selectedIndex]));
                mediaplayer.Play();
            }
        }

        private void VperedKnopka_Click(object sender, RoutedEventArgs e)
        {
            if (mediaplayer.NaturalDuration.HasTimeSpan && selectedIndex + 1 < files.Count)
            {
                selectedIndex += 1;
                mediaplayer.Open(new Uri(files[selectedIndex]));
                mediaplayer.Play();
            }
            else if (selectedIndex + 1 == files.Count)
            {
                selectedIndex = 0;
                mediaplayer.Open(new Uri(files[selectedIndex]));
                mediaplayer.Play();
            }
        }


        private void Playknopka_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if ((string)button.Content == "Играть")
            {
                button.Content = "Стоп";
                mediaplayer.Play();
            }
            else if ((string)button.Content == "Стоп")
            {
                button.Content = "Играть";
                mediaplayer.Pause();
            }
        }
    }
}