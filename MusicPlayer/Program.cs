using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	class Program
	{
		static void Main(string[] args)
		{
			Player player;
			bool playerWorking = true, isCmdParamsExists = false;
			string consoleString, consoleCommand, cmdParameter;

			Console.WriteLine("Классический скин: 1");
			Console.WriteLine("Цветной скин: 2");
			Console.WriteLine("Разноцветный скин: 3");
			Console.Write("Выберите скин: ");


			switch (Console.ReadLine())
			{
				case "1":
					player = new Player(new ClassicSkin());
					break;

				case "2":
					player = new Player(new ColoredSkin(ConsoleColor.DarkGreen));
					break;

				case "3":
					player = new Player(new RandColorSkin());
					break;

				default:
					Console.WriteLine("Значит классический");
					player = new Player(new ClassicSkin());
					break;
			}

			Console.WriteLine("help - поддерживаемые комманды");

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
							Console.WriteLine("add [path] - добавить все(пока) песни из папки [path]");
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
							Console.WriteLine("filter [genre] - сортировать список по жанрам");
							break;
						}

						player.FilterByGenre(cmdParameter);
						Console.WriteLine("Список отфильтрован");
						break;

					case "h":
					case "help":
						Console.WriteLine("add [path] - добавить все(пока) песни из папки [path]");
						Console.WriteLine("clear - очистить список");
						Console.WriteLine("filter [genre] - сортировать список по жанрам");
						Console.WriteLine("help - поддерживаемые комманды");
						Console.WriteLine("load [path] - загрузить список");
						Console.WriteLine("play - проиграть весь список");
						Console.WriteLine("playone - проиграть первую(пока) песню");
						Console.WriteLine("quit - выйти из программы");
						Console.WriteLine("save [path] - сохранить список");
						Console.WriteLine("shuffle - перемешать список");
						Console.WriteLine("songsinfo - информация по каждой песне");
						Console.WriteLine("songsname - название каждой песне");
						Console.WriteLine("sort - сортировать список по названию песни");
						break;

					case "l":
					case "load":
						if (!isCmdParamsExists)
						{
							Console.WriteLine("load [path] - загрузить список");
							break;
						}

						player.LoadPlaylist(cmdParameter);
						Console.WriteLine("Список загружен");
						break;

					case "p":
					case "play":
						player.Start(true);
						break;

					case "playone":
						player.Start(false);
						break;

					case "q":
					case "quit":
						playerWorking = false;
						break;

					case "sv":
					case "save":
						if (!isCmdParamsExists)
						{
							Console.WriteLine("save [path] - сохранить список");
							break;
						}

						player.SavePlaylist(cmdParameter);
						Console.WriteLine("Список сохранен");
						break;

					case "sr":
					case "sort":
						player.SortByTitle();
						Console.WriteLine("Список отсортирован");
						break;

					case "si":
					case "songsinfo":
						player.ShowAllSongsInfo();
						break;

					case "sn":
					case "songsname":
						player.ShowAllSongsName();
						break;

					case "sh":
					case "shuffle":
						player.Shuffle();
						Console.WriteLine("Список перемешан");
						break;

					default:
						break;
				}
			}
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
