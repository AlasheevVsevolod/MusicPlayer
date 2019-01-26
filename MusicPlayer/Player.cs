using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Extensions;
using System.Xml;
using System.Xml.Serialization;

namespace MusicPlayer
{
	public class Player
	{
		private Skins _playerSkin;

		public Player(Skins tmpSkin)
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
						foreach (var song in Songs)
						{
							PrintColoredSong(song);
							System.Threading.Thread.Sleep(500);
						}
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
					_playerSkin.Render($"Плейлист пустой. Добавьте песни");
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

		private void Add(params Song[] arrOfSongs)
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

		public void ShowAllSongsInfo()
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

			_playerSkin.Render($"Playing: {tmpSong.Name}\nGenre: {tmpSong.Artist.Genre}\n" + $"Duration: {tmpSong.Duration}\n");
			Console.ResetColor();
		}

/*		private Song SongShorten(Song srcSong)
		{
			const int stringLimit = 10;

			Song tmpSong = srcSong;
			tmpSong.Name = tmpSong.Name.StringShorten(stringLimit);

			return tmpSong;
		}*/

		public void FilterByGenre(string filterGenre)
		{
			bool isContainingGenres = true;
//			List<string> genresList =  new List<string>();
			List<Song> tmpList = new List<Song>();

/*			if (filterGenre.Contains(" "))
			{
				//todo
			}
			else
			{
				genresList.Append(filterGenre);
			}*/

			foreach (var song in Songs)
			{
				if (song.Artist.Genre.Contains(filterGenre))
				{
					tmpList.Add(song);
				}
			}
			Songs = tmpList;
		}
		
		public int LoadFiles(string dirPath)
		{
			//Если папки нет - выход
			if (!Directory.Exists(dirPath))
			{
				Console.WriteLine("Папки не существует");
				return 0;
			}

			//Если нет .mp3 файлов - выход
			var newDir = new DirectoryInfo(dirPath);
			var fileList = newDir.GetFiles("*.mp3");
			if (fileList.Length == 0)
			{
				Console.WriteLine(".mp3 файлы не найдены");
				return 0;
			}

			//По каждому файлу из списка вытягиваю инфу о песне(TagLib пакет)
			Song[] songsArr = new Song[fileList.Length];
			for (int i = 0; i < fileList.Length; i++)
			{
				TagLib.File file = TagLib.File.Create(fileList[i].FullName);

				var newSong = new Song()
				{
					Name = file.Tag.Title,
					Duration = (int)file.Properties.Duration.TotalSeconds,
					Album = new Album()
					{ 
						Name = file.Tag.Album,
						Year = (int)file.Tag.Year
					},
					Artist = new Artist()
					{ 
						Name = file.Tag.AlbumArtists.StringArrToString(" & "),
						Genre = file.Tag.Genres.StringArrToString("/")
					}
				};

				Songs.Add(newSong);
			}

			return songsArr.Length;
		}

		public void SavePlaylist(string filePath)
		{
			XmlSerializer tmpSrlzr = new XmlSerializer(typeof(List<Song>));
			filePath = $@"{filePath}\newplaylist.pl";

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			using (var xmlWriter = XmlWriter.Create(filePath))
			{
				tmpSrlzr.Serialize(xmlWriter, Songs);
			}
		}

		public void LoadPlaylist(string filePath)
		{
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Файл не найден");
				return;
			}
			XmlSerializer tmpSrlzr = new XmlSerializer(typeof(List<Song>));

			using (var xmlReader = XmlReader.Create(filePath))
			{
				Songs = (List<Song>)tmpSrlzr.Deserialize(xmlReader);
			}
		}
	}
}
