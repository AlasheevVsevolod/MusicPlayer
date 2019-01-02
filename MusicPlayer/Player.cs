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

		public Song[] Songs;
		public Song[] NewSongList = new Song[5];
		public Song[] AddSongs(out int NumOfSongs, params Song[] NewSongs)
		{
			NumOfSongs = 0;
			foreach(Song NewSong in NewSongs)
			{
				NewSongList[NumOfSongs++] = NewSong;
			}
			return NewSongList;
		}
		


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

		public bool Start()
		{
			if(!_locked)
			{
				_playing = true;
				Console.WriteLine($"Воспроизведение: {Songs[0].Name}");
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
	}
}
