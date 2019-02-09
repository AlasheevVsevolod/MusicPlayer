using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer.Extensions;

namespace MusicPlayer
{
	public class EventHandlers
	{
		public static void ErrorEventHandler(Player player, string errorMsg)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			DrawErrMessage(errorMsg);
		}

		public static void OnErrorEventHandler(string errorMsg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(errorMsg);
			Console.ResetColor();
		}

		public static void OnWarningEventHandler(string errorMsg)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(errorMsg);
			Console.ResetColor();
		}

		public static void PlayerLockEventHandler(Player player)
		{
			bool isLocked = player.IsLocked();
			DrawStatusLine(player.Playing, isLocked, player.Volume);
			DrawInfoMessage(isLocked ? "Плеер заблокирован" : "Плеер разблокирован");
		}

		public static void PlayerStartedEventHandler(Player player, Song song)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);

			foreach (var tmpSong in player.Songs)
			{
				bool isCurrent = tmpSong == song ? true : false;
				PrintSong(tmpSong, isCurrent);
			}
		}

		public static void PlayerStoppedEventHandler(Player player)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			DrawInfoMessage("Плеер остановлен");
		}

		public static void ShowAllSongsInfoEventHandler(Player player)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			PrintAllSongsInfo(player.Songs);
			Console.WriteLine($"Песен в списке: {player.Songs.Count}");
		}

		public static void ShowAllSongsNameEventHandler(Player player)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			ShowAllSongsName(player.Songs);
			Console.WriteLine($"Песен в списке: {player.Songs.Count}");
		}

		public static void SongListChangedEventHandler(Player player)
		{
			ShowAllSongsNameEventHandler(player);
		}

		public static void SongStartedEventHandler(Player player)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			PrintSong(player.Songs.First(), true);
		}

		public static void VolumeChangeEventHandler(Player player)
		{
			DrawStatusLine(player.Playing, player.IsLocked(), player.Volume);
			DrawInfoMessage("Громкость изменена");
		}



		public static void DrawStatusLine(bool isPlaying, bool isLocked, int volumeValue)
		{
			Console.Clear();
			Console.Write("Воспроизведение: ");
			Console.Write(isPlaying ? "Да" : "Нет");
			Console.Write("\t|Блокировка: ");
			Console.Write(isLocked ? "Да" : "Нет");
			Console.Write($"\t|Громкость: {volumeValue}\n");
			Console.WriteLine(new string('-', 63));
		}

		public static void DrawInfoMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void DrawErrMessage(string msg)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ResetColor();
		}

		public static void ShowAllSongsName(List<Song> tmpList)
		{
			foreach (var song in tmpList)
			{
				Console.WriteLine($"Song: {song.Name}");
			}
		}

		public static void PrintSong(Song tmpSong, bool current)
		{
			tmpSong = SongShorten(tmpSong);
			Console.ForegroundColor = current ? ConsoleColor.Green : ConsoleColor.Gray;
			Console.WriteLine($"Song: {tmpSong.Name}\tDuration: {tmpSong.Duration}");
			Console.ResetColor();
		}

		public static void PrintAllSongsInfo(List<Song> tmpList)
		{
			int songNumber = 1;
			foreach (var song in tmpList)
			{
				Console.WriteLine($"№{songNumber++}");
				PrintSongInfo(song);
			}
		}

		public static void PrintSongInfo(Song CurrentSong)
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

		private static Song SongShorten(Song srcSong)
		{
			const int stringLimit = 13;

			Song tmpSong = srcSong;
			tmpSong.Name = tmpSong.Name.StringShorten(stringLimit);

			return tmpSong;
		}
	}
}
