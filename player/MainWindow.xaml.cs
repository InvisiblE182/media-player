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
        List<string> songList = new List<string>();
        
        int songIdx = 0;
        int previousSong;
        

        public MainWindow()
        {
            
            InitializeComponent();
        }

        private void Shuffle_Click(object sender, RoutedEventArgs e)
        {
            if (shuffleButton.Background == Brushes.White)
            {
                shuffleButton.Background = Brushes.Aqua;
            } else
            {
                shuffleButton.Background = Brushes.White;
            }
        }

        private void Repeat_Click(object sender, RoutedEventArgs e)
        {
            if (repeatButton.Background == Brushes.White)
            {
                repeatButton.Background = Brushes.Aqua;
            }
            else
            {
                repeatButton.Background = Brushes.White;
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            playButton.Content = FindResource("Play");
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (playButton.Content != FindResource("Pause") && songList.Count != 0)
            {
                playButton.Content = FindResource("Pause");
                player.Play();
            }
            else
            {
                playButton.Content = FindResource("Play");
                player.Pause();
            }
        }

        
            

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (songList.Count == 0)
            {

            } else
            {
                if (shuffleButton.Background == Brushes.White)
                {
                    songIdx += 1;
                    if (songIdx == songList.Count)
                    {
                        songIdx = 0;
                    }
                    player.Open(new Uri(songList[songIdx]));
                    tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Play();
                    }

                }
                else
                {
                    previousSong = songIdx;
                    while (songIdx == previousSong)
                    {
                        Random rnd = new Random();
                        songIdx = rnd.Next(0, songList.Count);
                    }
                    player.Open(new Uri(songList[songIdx]));
                    tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Play();
                    }

                }
            }
            
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (songList.Count == 0)
            {

            } else
            {
                songIdx -= 1;
                if (songIdx < 0)
                {
                    songIdx = songList.Count - 1;
                }
                player.Open(new Uri(songList[songIdx]));
                tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                if (playButton.Content == FindResource("Pause"))
                {
                    player.Play();
                }
            }
            

        }


        List<double> Volume = new List<double>();

        private void Volume_Click(object sender, RoutedEventArgs e)
        {
            
            if (player.IsMuted == false)
            {
                player.IsMuted = true;
                volumeButton.Content = FindResource("Muted");
                volumeButton.Background = Brushes.Aqua;
            }
            else
            {
                player.IsMuted = false;
                volumeButton.Content = FindResource("High Volume");
                volumeButton.Background = Brushes.White;
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            songIdx = 0;
            if (playButton.Content == FindResource("Pause"))
            {
                playButton.Content = FindResource("Play");
                player.Pause();
            }
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            open.Multiselect = true;
            open.Filter = "All Files|*.mp3*";
            if (open.ShowDialog() == true)
            {
                foreach (string file in open.FileNames)
                {
                    songList.Add(file);
                }
                player.Open(new Uri(songList[songIdx]));
                foreach(string s in songList)
                {
                    playlist.Items.Add(System.IO.Path.GetFileName(s).Replace(".mp3", String.Empty));
                }
                tbPlaying.Text = "Now playing: " + playlist.Items[songIdx]; ;
            }
        }

    }
}
