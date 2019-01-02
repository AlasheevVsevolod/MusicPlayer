using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public class Player
	{
		const int MIN_VOLUME = 0;
		const int MAX_VOLUME = 100;

		private bool _locked;

		private bool _playing;
		public bool Playing
		{
			get
			{
				return _playing;
			}
		}

		private int _volume;
		public int Volume
		{
			get
			{
				return _volume;
			}

			private set
			{
				if (value < MIN_VOLUME)
				{
					_volume = MIN_VOLUME;
				}
				else if (value > MAX_VOLUME)
				{
					_volume = MAX_VOLUME;
				}
				else
				{
					_volume = value;
				}
			}
		}

		public List<Song> Songs { get; private set; } = new List<Song>();


		public void VolumeUp()
		{
			if(!_locked)
			{
				Volume++;
				Console.WriteLine($"Громкость увеличена. {_volume}%");
			}
			else
			{
				BlockVolumeChange();
			}
		}

		public void VolumeDown()
		{
			if(!_locked)
			{
				Volume--;
				Console.WriteLine($"Громкость уменьшена. {_volume}%");
			}
			else
			{
				BlockVolumeChange();
			}
		}

		public void VolumeChange( int step)
		{
			if(!_locked)
			{
				Volume += step;
				if (step > 0)
				{
					Console.WriteLine($"Громкость увеличена. {_volume}%");
				}
				else
				{
					Console.WriteLine($"Громкость уменьшена. {_volume}%");
				}
			}
			else
			{
				BlockVolumeChange();
			}
		}

		public bool Start(bool loop = false)
		{
			if(!_locked)
			{
				try
				{
					if (loop)
					{
						foreach (var song in Songs)
						{
							Console.WriteLine($"Playing: {song.Name}, duration: {song.Duration}\n");
							System.Threading.Thread.Sleep(1000);
						}
					}
					else
					{
						_playing = true;
						Console.WriteLine($"Playing: {Songs.First().Name}, duration: {Songs.First().Duration}\n");
						System.Threading.Thread.Sleep(1000);
					}
				}
				catch (System.InvalidOperationException)
				{
					Console.WriteLine($"Плейлист пустой. Добавьте песни\n");
				}
			}
			else
			{
				Console.WriteLine("Нельзя запустить плеер. Он заблокирован");
			}
			return _playing;
		}

		public bool Stop()
		{
			if(!_locked)
			{
				_playing = false;
				Console.WriteLine("Плеер остановлен");
			}
			else
			{
				Console.WriteLine("Нельзя остановить плеер. Он заблокирован");
			}
			return _playing;
		}

		public void Lock()
		{
			_locked = true;
			Console.WriteLine("Плеер заблокирован");
		}

		public void Unlock()
		{
			_locked = false;
			Console.WriteLine("Плеер разблокирован");
		}

		private void BlockVolumeChange()
		{
			Console.WriteLine("Нельзя изменить громкость. Плеер заблокирован");
		}

		public void Add(params Song[] arrOfSongs)
		{
			foreach (var songItem in arrOfSongs)
			{
				Songs.Add(songItem);
			}
		}

		public void SongInfo(Song CurrentSong)
		{
			Console.WriteLine($"Artist: {CurrentSong.Artist.Name}");
			Console.WriteLine($"Song: {CurrentSong.Name}");
			Console.WriteLine($"Duration: {CurrentSong.Duration}");
			Console.WriteLine($"Album: {CurrentSong.Album.Name}");
			Console.WriteLine($"Year: {CurrentSong.Album.Year}");
			Console.WriteLine($"Genre: {CurrentSong.Artist.Genre}\n");
		}

		public void ShowAllSongs()
		{
			int songNumber = 1;
			foreach (var song in Songs)
			{
				Console.WriteLine($"№{songNumber++}");
				SongInfo(song);
			}
		}

		public void ShowAllSongsName()
		{
			foreach (var song in Songs)
			{
				Console.WriteLine($"Song: {song.Name}");
			}
			Console.WriteLine();
		}

		public void Shuffle()
		{
			var newList = new List<Song>();

			var rand = new Random();
			int randSongNum, cntr = Songs.Count;

			for (int i = 0; i < cntr; i++)
			{
				randSongNum = rand.Next(Songs.Count);
				newList.Add(Songs.ElementAt(randSongNum));
				Songs.RemoveAt(randSongNum);
			}
			Songs = newList;
		}
	}
}
