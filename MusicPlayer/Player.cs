using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Extensions;
using System.Xml;
using System.Xml.Serialization;
using System.Media;

namespace MusicPlayer
{
	public class Player : IDisposable
	{
		public event Action<Player, string> ErrorEvent;
		public event Action<Player> PlayerLockEvent;
		public event Action<Player, Song> PlayerStartedEvent;
		public event Action<Player> PlayerStoppedEvent;
		public event Action<Player> ShowAllSongsInfoEvent;
		public event Action<Player> ShowAllSongsNameEvent;
		public event Action<Player> SongListChangedEvent;
		public event Action<Player> SongStartedEvent;
		public event Action<Player> VolumeChangeEvent;


		private SoundPlayer _myPlayer = new SoundPlayer();

		private bool _disposed = false;

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
		public string errorString;

		public void VolumeUp()
		{
			if(!_locked)
			{
				Volume++;
				VolumeChangeEvent(this);
			}
			else
			{
				BlockError();
			}
		}

		public void VolumeDown()
		{
			if(!_locked)
			{
				Volume--;
				VolumeChangeEvent(this);
			}
			else
			{
				BlockError();
			}
		}

		public void VolumeChange( int step)
		{
			if(!_locked)
			{
				Volume += step;
				if (step > 0)
				{
					VolumeChangeEvent(this);
				}
				else
				{
					VolumeChangeEvent(this);
				}
			}
			else
			{
				BlockError();
			}
		}

		public async void Start()
		{
			if(!_locked)
			{
				if(Songs.Count > 0)
				{
					_playing = true;
					foreach (var song in Songs)
					{
						if (!_playing) return;
						await PlayOneAsync(song);
					}
				}
				else
				{
					ErrorEvent(this, "Плейлист пустой");
				}
			}
			else
			{
				BlockError();
			}
			return;
		}

		/* Когда вводу stop, плеер не будет воспроизводить следующую песню, а текущую
		 * не остановит.
		 * Переписать на тред, т.к. таск по своей природе один раз запускается и отрабатывает
		 * до победного => на паузу воспроизведение не поставить
		 */
		private Task PlayOneAsync(Song song)
		{
			return Task.Run(() =>
			{
				PlayerStartedEvent(this, song);
				_myPlayer.SoundLocation = song.Location;
				_myPlayer.PlaySync();
			});
		}

		//public void Play()
		//{
		//	if (!_locked)
		//	{
		//		_playing = true;
		//	}
		//	else
		//	{
		//		BlockError();
		//	}
		//	return;
		//}

		public void Stop()
		{
			if(!_locked)
			{
				_playing = false;
				PlayerStoppedEvent(this);
			}
			else
			{
				BlockError();
			}
			return;
		}

		public void Lock()
		{
			_locked = true;
			PlayerLockEvent(this);
		}

		public void Unlock()
		{
			_locked = false;
			PlayerLockEvent(this);
		}

		public bool IsLocked()
		{
			return _locked ? true : false;
		}

		private void BlockError()
		{
			ErrorEvent(this, "Плеер заблокирован");
		}

		private void Add(params Song[] arrOfSongs)
		{
			foreach (var songItem in arrOfSongs)
			{
				Songs.Add(songItem);
			}
			SongListChangedEvent(this);
		}

		public void Clear()
		{
			Songs.Clear();
			SongListChangedEvent(this);
		}

		public void ShowAllSongsInfo()
		{
			ShowAllSongsInfoEvent(this);
		}

		public void ShowAllSongsName()
		{
			ShowAllSongsNameEvent(this);
		}


		public void Shuffle()
		{
			this.Songs = this.Songs.Shuffle();
			SongListChangedEvent(this);
		}

		public void SortByTitle()
		{
			this.Songs = this.Songs.SortByTitle();
			SongListChangedEvent(this);
		}

		public void FilterByGenre(string filterGenre)
		{
			List<Song> tmpList = new List<Song>();

			foreach (var song in Songs)
			{
				if (song.Artist.Genre.Contains(filterGenre))
				{
					tmpList.Add(song);
				}
			}
			Songs = tmpList;
			SongListChangedEvent(this);
		}
		
		public int LoadFiles(string dirPath)
		{
			//Если папки нет - выход
			if (!Directory.Exists(dirPath))
			{
				ErrorEvent(this, "Папки не существует");
				return 0;
			}

			//Если нет .mp3 файлов - выход
			var newDir = new DirectoryInfo(dirPath);
			var fileList = newDir.GetFiles("*.wav");
			if (fileList.Length == 0)
			{
				ErrorEvent(this, ".wav файлы не найдены");
				return 0;
			}

			//По каждому файлу из списка вытягиваю инфу о песне(TagLib пакет)
			Song[] songsArr = new Song[fileList.Length];
			for (int i = 0; i < fileList.Length; i++)
			{
				TagLib.File file = TagLib.File.Create(fileList[i].FullName);

				var newSong = new Song()
				{
					Duration = (int)file.Properties.Duration.TotalSeconds,
					Album = new Album(),
					Artist = new Artist(),
					Location = fileList[i].FullName
				};
				newSong.Name = file.Tag.Title ?? fileList[i].Name;
				newSong.Album.Name = file.Tag.Album ?? newSong.Album.Name;
				newSong.Album.Year = (int)file.Tag.Year;
				newSong.Artist.Name = file.Tag.AlbumArtists.StringArrToString(" & ") ?? newSong.Artist.Name;
				newSong.Artist.Genre = file.Tag.Genres.StringArrToString("/") ?? newSong.Artist.Genre;
				Songs.Add(newSong);
			}
			SongListChangedEvent(this);

			return songsArr.Length;
		}

		public void SavePlaylist(string filePath)
		{
			XmlSerializer tmpSrlzr = new XmlSerializer(typeof(List<Song>));
			filePath = $@"{filePath}\newplaylist.pl";

			//Если файл есть - перезаписываю
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
			//Нет файла - выход
			if (!File.Exists(filePath))
			{
				ErrorEvent(this, "Файл не найден");
				return;
			}

			XmlSerializer tmpSrlzr = new XmlSerializer(typeof(List<Song>));
			using (var xmlReader = XmlReader.Create(filePath))
			{
				Songs = (List<Song>)tmpSrlzr.Deserialize(xmlReader);
			}
			SongListChangedEvent(this);
		}

		//Implement IDisposable.
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					// Free other state (managed objects).
					this._myPlayer = null;
					this.Songs = null;
				}
				// Free your own state (unmanaged objects).
				this._myPlayer.Dispose();
				_disposed = true;
			}
		}
		// Use C# destructor syntax for finalization code.
		~Player()
		{
			// Simply call Dispose(false).
			Dispose(false);
		}
	}
}
