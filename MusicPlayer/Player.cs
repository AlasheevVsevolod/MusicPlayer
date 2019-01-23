using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Extensions;

namespace MusicPlayer
{
	public class Player
	{
		private ISkins _playerSkin;

		public Player(ISkins tmpSkin)
		{
			_playerSkin = tmpSkin;
		}

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
				_playerSkin.Render($"Громкость увеличена. {_volume}%");
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
				_playerSkin.Render($"Громкость уменьшена. {_volume}%");
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
					_playerSkin.Render($"Громкость увеличена. {_volume}%");
				}
				else
				{
					_playerSkin.Render($"Громкость уменьшена. {_volume}%");
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
						_playerSkin.Clear();
						foreach (var song in Songs)
						{
							PrintColoredSong(song);
							System.Threading.Thread.Sleep(500);
						}
						_playerSkin.Render();
					}
					else
					{
						_playerSkin.Clear();
						_playing = true;
						PrintColoredSong(Songs.First());
						System.Threading.Thread.Sleep(500);
					}
				}
				catch (System.InvalidOperationException)
				{
					_playerSkin.Render($"Плейлист пустой. Добавьте песни\n");
				}
			}
			else
			{
				_playerSkin.Render("Нельзя запустить плеер. Он заблокирован");
			}
			return _playing;
		}

		public bool Stop()
		{
			if(!_locked)
			{
				_playing = false;
				_playerSkin.Render("Плеер остановлен");
			}
			else
			{
				_playerSkin.Render("Нельзя остановить плеер. Он заблокирован");
			}
			return _playing;
		}

		public void Lock()
		{
			_locked = true;
			_playerSkin.Render("Плеер заблокирован");
		}

		public void Unlock()
		{
			_locked = false;
			_playerSkin.Render("Плеер разблокирован");
		}

		private void BlockVolumeChange()
		{
			_playerSkin.Render("Нельзя изменить громкость. Плеер заблокирован");
		}

		public void Add(params Song[] arrOfSongs)
		{
			foreach (var songItem in arrOfSongs)
			{
				Songs.Add(songItem);
			}
		}

		public void Clear()
		{
			Songs.Clear();
		}

		public void SongInfo(Song CurrentSong)
		{
			Song songToPrint = SongShorten(CurrentSong);
			_playerSkin.Render($"Artist: {CurrentSong.Artist.Name}");
			_playerSkin.Render($"Song: {CurrentSong.Name}");
			_playerSkin.Render($"Duration: {CurrentSong.Duration}");
			_playerSkin.Render($"Album: {CurrentSong.Album.Name}");
			_playerSkin.Render($"Year: {CurrentSong.Album.Year}");
			_playerSkin.Render($"Genre: {CurrentSong.Artist.Genre}");
			if (CurrentSong.Like == null)
			{
				_playerSkin.Render($"Liked: undefined\n");
			}
			else if (CurrentSong.Like == true)
			{
				_playerSkin.Render($"Liked: yes\n");
			}
			else
			{
				_playerSkin.Render($"Liked: no\n");
			}
		}

		public void ShowAllSongs()
		{
			int songNumber = 1;
			foreach (var song in Songs)
			{
				_playerSkin.Render($"№{songNumber++}");
				SongInfo(song);
			}
		}

		public void ShowAllSongsName()
		{
			foreach (var song in Songs)
			{
				Song songToPrint = SongShorten(song);
				_playerSkin.Render($"Song: {song.Name}");
			}
			_playerSkin.Render();
		}

		public void Shuffle()
		{
			this.Songs = this.Songs.Shuffle();
		}

		public void SortByTitle()
		{
			this.Songs = this.Songs.SortByTitle();
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

			Song songToPrint = SongShorten(tmpSong);
			_playerSkin.Render($"Playing: {songToPrint.Name}\nGenre: {ArtistGenreToString(songToPrint.Artist.Genre)}\n" + $"Duration: {songToPrint.Duration}\n");
			Console.ResetColor();
		}

		private Song SongShorten(Song srcSong)
		{
			const int stringLimit = 10;

			Song tmpSong = srcSong;
			tmpSong.Name = tmpSong.Name.StringShorten(stringLimit);

			return tmpSong;
		}

		public void FilterByGenre(params Genres[] filterGenre)
		{
			bool isContainingGenres = true;
			List<Song> tmpList = new List<Song>();

			foreach (var song in Songs)
			{
				foreach (Genres genre in filterGenre)
				{
					if ((song.Artist.Genre & genre) == genre)
					{
						isContainingGenres = true;
					}
					else
					{
						isContainingGenres = false;
						break;
					}
				}
				if (isContainingGenres)
				{
					tmpList.Add(song);
				}
			}
			Songs = tmpList;
		}

		public string ArtistGenreToString(Genres genres)
		{
			var listGenres = new List<string>();
			int tmpGenre = 0, cntr = 1;
			string strGenres = "";

			if (genres == 0)
			{
				return "Undefined";
			}

			while (tmpGenre != (int)genres)
			{
				if ((genres & (Genres)cntr) == (Genres)cntr)
				{
					listGenres.Add(((Genres)cntr).ToString());
					tmpGenre = tmpGenre | cntr;
				}
				cntr = cntr << 1;
			}

			listGenres.Sort();

			foreach (var str in listGenres)
			{
				strGenres = strGenres + "/" + str;
			}
			//	/qwe/asd/zxc - нужно убрать первый символ
			return strGenres.Substring(1);
		}
	}
}
