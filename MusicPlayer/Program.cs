using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusicPlayer.EventHandlers;

namespace MusicPlayer
{
	class Program
	{
		static void Main(string[] args)
		{
			bool playerWorking = true, isCmdParamsExists = false;
			string consoleString, consoleCommand, cmdParameter;

			PrintHelp();

			using (var player = new Player())
			{
				player.ErrorEvent += ErrorEventHandler;
				player.PlayerLockEvent += PlayerLockEventHandler;
				player.PlayerStartedEvent += PlayerStartedEventHandler;
				player.PlayerStoppedEvent += PlayerStoppedEventHandler;
				player.ShowAllSongsInfoEvent += ShowAllSongsInfoEventHandler;
				player.ShowAllSongsNameEvent += ShowAllSongsNameEventHandler;
				player.SongListChangedEvent += SongListChangedEventHandler;
				player.SongStartedEvent += SongStartedEventHandler;
				player.VolumeChangeEvent += VolumeChangeEventHandler;


				while (playerWorking)
				{
					consoleString = Console.ReadLine();
					isCmdParamsExists = IsCmdParam(consoleString, out consoleCommand, out cmdParameter);

					switch (consoleCommand)
					{
						case "a":
						case "add":
							if (!isCmdParamsExists)
							{
								Console.WriteLine("Add [path] - добавить все(пока) песни из папки [path]");
								break;
							}
							int numOfFiles = player.LoadFiles(cmdParameter);
							if (numOfFiles > 0)
							{
								Console.WriteLine($"Файлов добавлено: {numOfFiles}");
							}
							break;

						case "c":
						case "clear":
							player.Clear();
							Console.WriteLine("Список очищен");
							break;

						case "f":
						case "filter":
							if (!isCmdParamsExists)
							{
								Console.WriteLine("Filter [genre] - сортировать список по жанрам");
								break;
							}

							player.FilterByGenre(cmdParameter);
							Console.WriteLine("Список отфильтрован");
							break;

						case "h":
						case "help":
							PrintHelp();
							break;

						case "ld":
						case "load":
							if (!isCmdParamsExists)
							{
								Console.WriteLine("LoaD [path] - загрузить список");
								break;
							}

							player.LoadPlaylist(cmdParameter);
							Console.WriteLine("Список загружен");
							break;

						case "lk":
						case "lock":
							player.Lock();
							break;

						//case "p":
						//case "play":
						//	player.Play();
						//	break;

						case "q":
						case "quit":
							playerWorking = false;
							break;

						case "sv":
						case "save":
							if (!isCmdParamsExists)
							{
								Console.WriteLine("SaVe [path] - сохранить список");
								break;
							}

							player.SavePlaylist(cmdParameter);
							Console.WriteLine("Список сохранен");
							break;

						case "sh":
						case "shuffle":
							player.Shuffle();
							Console.WriteLine("Список перемешан");
							break;

						case "si":
						case "songsinfo":
							player.ShowAllSongsInfo();
							break;

						case "sn":
						case "songsname":
							player.ShowAllSongsName();
							break;

						case "sr":
						case "sort":
							player.SortByTitle();
							Console.WriteLine("Список отсортирован");
							break;

						case "start":
							player.Start();
							break;

						case "stop":
							player.Stop();
							break;

						case "vc":
						case "volumechange":
							if (!isCmdParamsExists)
							{
								Console.WriteLine("VolumeChange [value] - изменить громкость на заданное значение");
								break;
							}

							player.VolumeChange(Convert.ToInt32(cmdParameter));
							break;

						case "vd":
						case "volumedown":
							player.VolumeDown();
							break;

						case "vu":
						case "volumeup":
							player.VolumeUp();
							break;

						case "u":
						case "unlock":
							player.Unlock();
							break;

						default:
							break;
					}
				}
			}
		}

		public static void PrintHelp()
		{
			Console.WriteLine("Add [path] - добавить все(пока) песни из папки [path]");
			Console.WriteLine("Clear - очистить список");
			Console.WriteLine("Filter [genre] - сортировать список по жанрам");
			Console.WriteLine("Help - поддерживаемые комманды");
			Console.WriteLine("LoaD [path] - загрузить список");
			Console.WriteLine("LocK - заблокировать плеер");
			//Console.WriteLine("Play - проиграть весь список");
			Console.WriteLine("Quit - выйти из программы");
			Console.WriteLine("SaVe [path] - сохранить список");
			Console.WriteLine("SHuffle - перемешать список");
			Console.WriteLine("SongsInfo - информация по каждой песне");
			Console.WriteLine("SongsName - название каждой песне");
			Console.WriteLine("SoRt - сортировать список по названию песни");
			Console.WriteLine("start - проиграть список сначала");
			Console.WriteLine("stop - приостановить воспроизведение");
			Console.WriteLine("Unlock - разблокировать плеер");
			Console.WriteLine("VolumeChange [value] - изменить громкость на заданное значение");
			Console.WriteLine("VolumeDown - уменьшить громкость на 1%");
			Console.WriteLine("VolumeUp - увеличить громкость на 1%");
			Console.WriteLine();
		}

		public static bool IsCmdParam(string consoleString, out string consoleCommand, out string cmdParameter)
		{
			cmdParameter = null;

			if (consoleString.Contains(" "))
			{
				consoleCommand = consoleString.Substring(0, consoleString.IndexOf(" "));
				cmdParameter = consoleString.Substring(consoleString.IndexOf(" ") + 1);
				return true;
			}
			else
			{
				consoleCommand = consoleString;
				return false;
			}
		}
	}
}
