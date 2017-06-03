﻿using System;
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
        public MediaPlayer player { get; set; }
        private List<double> volume { get; set; }
        private List<string> songList { get; set; }
        private int songIdx { get; set; }
        private  int previousSong { get; set; }


        public MainWindow()
        {
            Initialise();
        }

        private void Initialise()
        {
            player = new MediaPlayer();
            songIdx = 0;
            songList = new List<string>();
            volume = new List<double>();

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
                tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
            }
            else
            {
                playButton.Content = FindResource("Play");
                player.Pause();
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (songList.Count != 0)
            {
                if (shuffleButton.Background == Brushes.White)
                {
                    songIdx += 1;
                    if (songIdx == songList.Count)//current song was last in playlist
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
                    while (songIdx == previousSong)//thist prevents playing the same song again
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
            if (songList.Count != 0)
            {
                if (shuffleButton.Background == Brushes.White)
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
                } else
                {
                    previousSong = songIdx;
                    player.Open(new Uri(songList[songIdx]));
                    tbPlaying.Text = "Now playing: " + playlist.Items[songIdx];
                    if (playButton.Content == FindResource("Pause"))
                    {
                        player.Play();
                    }
                }
            }
        }

        

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
        

        private void VolumeSlider_ValueChanged(object sender, EventArgs e)
        {
            player.Volume = volumeSlider.Value;
        }

        private void SongSlider_ValueChanged(object sender, EventArgs e)
        {
            
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
                tbPlaying.Text = "First up: " + playlist.Items[songIdx];
                playlist.BorderBrush = Brushes.Gray;

            }

        }

        private void Playlist_DoubleClick(object sender, RoutedEventArgs e)
        {
            songIdx = playlist.SelectedIndex;
            player.Open(new Uri(songList[songIdx]));
            tbPlaying.Text = "Now playing: " + playlist.Items[songIdx]; ;
            player.Play();
        }


    }
}
