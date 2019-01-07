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
							PrintColoredSong(song);
							System.Threading.Thread.Sleep(500);
						}
						Console.WriteLine();
					}
					else
					{
						_playing = true;
						PrintColoredSong(Songs.First());
						System.Threading.Thread.Sleep(500);
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
			Console.WriteLine($"Genre: {CurrentSong.Artist.Genre}");
			if (CurrentSong.Like == null)
			{
				Console.WriteLine($"Liked: undefined\n");
			}
			else if (CurrentSong.Like == true)
			{
				Console.WriteLine($"Liked: yes\n");
			}
			else
			{
				Console.WriteLine($"Liked: no\n");
			}
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
			var tmpList = new List<Song>();

			var rand = new Random();
			int randSongNum, cntr = Songs.Count;

			for (int i = 0; i < cntr; i++)
			{
				randSongNum = rand.Next(Songs.Count);
				tmpList.Add(Songs.ElementAt(randSongNum));
				Songs.RemoveAt(randSongNum);
			}
			Songs = tmpList;
		}

		public void SortByTitle()
		{
			var tmpList = new List<Song>();
			var songNameList = new List<string>();
			int cntr = Songs.Count;

			foreach (var song in Songs)
			{
				songNameList.Add(song.Name);
			}
			songNameList.Sort();

			foreach (var songName in songNameList)
			{
				for (int i = 0; i < cntr; i++)
				{
					if (Songs.ElementAt(i).Name == songName)
					{
						tmpList.Add(Songs.ElementAt(i));
						Songs.RemoveAt(i);
						break;
					}
				}
			}
			Songs = tmpList;
		}

		private void PrintColoredSong(Song tmpSong)
		{
			if (tmpSong.Like == true)
			{
				Console.ForegroundColor = ConsoleColor.Green;
			}
			else if (tmpSong.Like == false)
			{
				Console.ForegroundColor = ConsoleColor.Red;
			}
			Console.WriteLine($"Playing: {tmpSong.Name}, duration: {tmpSong.Duration}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
