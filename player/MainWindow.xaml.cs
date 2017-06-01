using System;
using System.IO;
using System.Runtime.InteropServices;
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



namespace player
{
    public partial class MainWindow : Window
    {
        private MediaPlayer player = new MediaPlayer();
        List<string> playlist = new List<string>();
        int currentSong = 0;
        int playing = 0;
        int shuffle = 0;
        int repeat = 0;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            if (shuffle == 0)
            {
                Shuffle.Background = Brushes.Aqua;
                shuffle = 1;
            } else
            {
                Shuffle.Background = Brushes.White;
                shuffle = 0;
            }
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            if (repeat == 0)
            {
                Repeat.Background = Brushes.Aqua;
                repeat = 1;
            }
            else
            {
                Repeat.Background = Brushes.White;
                repeat = 0;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            PlayButton.Content = FindResource("Play");
            playing = 0;
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            currentSong -= 1;
            if (currentSong < 0)
            {
                currentSong = playlist.Count - 1;
            }
            player.Open(new Uri(playlist[currentSong]));
            player.Play();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playing == 0 && playlist.Count != 0)
                {
                PlayButton.Content = FindResource("Pause");
                player.Play();
                playing = 1;
            }
            else
            {
                PlayButton.Content = FindResource("Play");
                player.Pause();
                playing = 0;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (shuffle == 0)
            {
                currentSong += 1;
                if (currentSong == playlist.Count)
                {
                    currentSong = 0;
                }
                player.Open(new Uri(playlist[currentSong]));
                player.Play();
                playing = 1;
            }
            else
            {
                Random rnd = new Random();
                currentSong = rnd.Next(0, playlist.Count);
                player.Open(new Uri(playlist[currentSong]));
                player.Play();
                playing = 1;
            }
        }


        List<double> Volume = new List<double>();

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            
            if (player.IsMuted == false)
            {
                player.IsMuted = true;
            }
            else
            {
                player.IsMuted = false;
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All Files|*.mp3*";

            if (open.ShowDialog() == true)
            {
                foreach (string file in open.FileNames)
                {
                    playlist.Add(file);
                }
                player.Open(new Uri(playlist[currentSong]));
                tb.Text = playlist[0];
            }
        }
    }
}
