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
//task1
			var player = new Player();

//task2
//			player.Volume = 20;
			player.Songs = GetSongsData();

//			TraceInfo(player);

//task3
//task4
			player.Lock();

			player.VolumeUp();
			player.VolumeDown();

			player.VolumeChange(-300);
			player.VolumeChange(300);

			player.Start();
			Console.WriteLine(player.Playing ? "Player is playing": "Player is stopped");

			player.Stop();
			Console.WriteLine(player.Playing ? "Player is playing" : "Player is stopped\n");

			player.Unlock();
			
			player.VolumeUp();
			player.VolumeDown();

			player.VolumeChange(-300);
			player.VolumeChange(300);

			player.Start();
			Console.WriteLine(player.Playing ? "Player is playing": "Player is stopped");

			player.Stop();
			Console.WriteLine(player.Playing ? "Player is playing" : "Player is stopped\n");


//task6
			Song newsong = CreateSong();
			PrintSongInfo(newsong);

//			Song newsong_copy = CreateDefaultSong2();
//			PrintSongInfo(newsong_copy);

			Song newsong2 = CreateSong("Sahti");
			PrintSongInfo(newsong2);

			Song newsong3 = CreateSong("Uptown Funk", 123, "Bruno Mars", "Dancing", "Funk", 2014);
			PrintSongInfo(newsong3);


//task7
			int TheShortestSong, TheLongestSong = 0, RandomDuration;
			string ShortestSongName = "Shortest song ever";
//			RandomDuration = TotalSongsDuration(out TheShortestSong, ref TheLongestSong, ref ShortestSongName);
			TotalSongsDuration(out TheShortestSong, ref TheLongestSong, ref ShortestSongName, out RandomDuration);

			Console.Write("Длина пяти случайно сгенерированных песен = ");
			Console.WriteLine($"{RandomDuration}с или {RandomDuration / 60}м {RandomDuration % 60}с");
			Console.WriteLine($"Самая короткая песня длится {TheShortestSong}с");
			Console.WriteLine($"Самая длинная песня длится {TheLongestSong}с");

//task8
			int NumOfSongs;

			Console.WriteLine("\nTask 8");

			Console.WriteLine("One song");
			newsong = CreateSong();
			Console.WriteLine($"Song duration {newsong.Duration}\n");
			player.AddSongs(out NumOfSongs, newsong);			
			for(int i = 0; i < NumOfSongs; i++)
			{
//Здесь вывожу длительность сгенерированных песен, чтобы сравнить с теми, которые
// вернутся после player.AddSongs
				PrintSongInfo(player.NewSongList[i]);
			}

			Console.WriteLine("Two songs");
			newsong = CreateSong();
			Console.WriteLine($"Song duration {newsong.Duration}\n");
			newsong2 = CreateSong();
			Console.WriteLine($"Song duration {newsong2.Duration}\n");

			player.AddSongs(out NumOfSongs, newsong, newsong2);			
			for(int i = 0; i < NumOfSongs; i++)
			{
				PrintSongInfo(player.NewSongList[i]);
			}

			Console.WriteLine("Array of songs");
			Song[] SongsArr = new Song[5];
			for(int i = 0; i < SongsArr.Length; i++)
			{
				SongsArr[i] = CreateSong();
				Console.WriteLine($"Song duration {SongsArr[i].Duration}");
			}
			Console.WriteLine();

			player.AddSongs(out NumOfSongs, SongsArr);			
			for(int i = 0; i < NumOfSongs; i++)
			{
				PrintSongInfo(player.NewSongList[i]);
			}

//task10
			Artist NewArtist = AddArtist();
			Console.WriteLine($"Без параметров. Артист. {NewArtist.Name}");

			NewArtist = AddArtist("Dance With The Dead");
			Console.WriteLine($"С параметром. Артист. {NewArtist.Name}");

			Album NewAlbum = AddAlbum();
			Console.WriteLine($"Без параметров. Альбом. {NewAlbum.Name}, {NewAlbum.Year}");

			NewAlbum = AddAlbum("Loved to Death", 2018);
			Console.WriteLine($"С параметрами. Альбом. {NewAlbum.Name}, {NewAlbum.Year}");

			NewAlbum = AddAlbum(AlbumYear: 2018, AlbumName: "Loved to Death");
			Console.WriteLine($"Параметры перемешаны. Альбом. {NewAlbum.Name}, {NewAlbum.Year}");
			
			NewAlbum = AddAlbum(AlbumYear: 2018);
			Console.WriteLine($"Один параметр. Альбом. {NewAlbum.Name}, {NewAlbum.Year}");




			Console.ReadLine();
		}






		public static void PrintSongInfo(Song CurrentSong)
		{
			Console.WriteLine($"Album name {CurrentSong.Album.Name}");
			Console.WriteLine($"Album year {CurrentSong.Album.Year}");
			Console.WriteLine($"Artist genre {CurrentSong.Artist.Genre}");
			Console.WriteLine($"Artist name {CurrentSong.Artist.Name}");
			Console.WriteLine($"Song duration {CurrentSong.Duration}");
			Console.WriteLine($"Song name {CurrentSong.Name}\n");
		}




		public static Song[] GetSongsData()
		{
			var artist = new Artist();
			artist.Name = "Powerwolf";
			artist.Genre = "Metal";
			Console.WriteLine($"Artist1 {artist.Name}");
			Console.WriteLine($"Genre1 {artist.Genre}\n");

			var artist2 = new Artist("Lordi");
			Console.WriteLine($"Artist2 {artist2.Name}");
			Console.WriteLine($"Genre2 {artist2.Genre}\n");

			var artist3 = new Artist("Sabaton", "Rock");
			Console.WriteLine($"Artist3 {artist3.Name}");
			Console.WriteLine($"Genre3 {artist3.Genre}\n");

			var album = new Album();
			album.Name = "New Album";
			album.Year = 2018;

			var song = new Song()
			{
				Duration = 100,
				Name = "New song",
				Album = album,
				Artist = artist
			};

			return new Song[] {song};
		}





		public static void TraceInfo(Player player)
		{
			Console.WriteLine(player.Songs[0].Artist.Name);
			Console.WriteLine(player.Songs[0].Duration);
			Console.WriteLine(player.Songs.Length);
			Console.WriteLine(player.Volume);
		}
		





		public static Song CreateSong()
		{
			Random rand = new Random();

			/*Создать строку из символов не выйдет, я так понял, т.к. строки в шарпе неизменяемые, 
поэтому имя будет одним символом*/
/*			Artist DefArtist = new Artist()
			{
				Genre = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
				Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z'))
			};
*/
/*			Album DefAlbum = new Album()
			{
				Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
				Year = rand.Next(1980, DateTime.Today.Year)
			};
*/
			Song DefaultSong = new Song()
			{
				Duration = rand.Next(60, 301),
//				Album = DefAlbum,
				Album = new Album
				{
					Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
					Year = rand.Next(1980, DateTime.Today.Year)
				},
//				Artist = DefArtist,
				Artist = new Artist
				{
					Genre = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
					Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z'))
				},
				Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z'))
			};

//похоже, что Random генерирует числа как-то опираясь на таймеры, т.к. без задержки
// бывало, что два цикла генерировали одинаковые значения
				System.Threading.Thread.Sleep(10);

			return DefaultSong;
		}




/*
//А если рефакторить, то можно так забомбить
		public static Song CreateDefaultSong2()
		{

//параметры для примера указал
//			Song DefaultSong = new Song()
//			{
//				DefDuration = 115,
//				DefAlbum = new Album
//				{
//					Name = "qwe",
//					Year = 2010
//				},
//				DefArtist = new Artist
//				{
//					Genre = "asd",
//					Name = "zxc"
//				},
//				DefName = "qaz"
//			};

			return CreateSong("qaz", 115, "zxc", "asd", "qwe", 2010);
//короче вызвать последний метод со некоторыми рандомными(или по умолчанию) параметрами
// что смотрится гораздо приятнее
		}
*/




		public static Song CreateSong(string SongName)
		{
			Random rand = new Random();

			Song NamedSong = new Song()
			{
				Duration = rand.Next(60, 301),
				Album = new Album()
				{
					Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
					Year = rand.Next(1980, DateTime.Today.Year)
				},
				Artist = new Artist()
				{
					Genre = Convert.ToString((char)rand.Next((int)'A', (int)'Z')),
					Name = Convert.ToString((char)rand.Next((int)'A', (int)'Z'))
				},
				Name = SongName
			};

			return NamedSong;
//а этот метод после рефакторинга тоде будет из одной строки состоять
//			return CreateSong(SongName, 115, "zxc", "asd", "qwe", 2010);
		}






		public static Song CreateSong(string SongName, int SongDuration, string ArtistName, string ArtistGenre, string AlbumName, int AlbumYear)
		{
			Song ExplicitSong = new Song()
			{
				Duration = SongDuration,
				Album = new Album()
				{
					Name = AlbumName,
					Year = AlbumYear
				},
				Artist = new Artist()
				{
					Genre = ArtistGenre,
					Name = ArtistName
				},
				Name = SongName
			};

			return ExplicitSong;
		}






//		public static int TotalSongsDuration(out int TheShortestSong, ref int TheLongestSong,
//			ref string ShortestSongName)
		public static void TotalSongsDuration(out int TheShortestSong, ref int TheLongestSong,
			ref string ShortestSongName, out int TotalDuration)
		{
//Если я всё правильно понимаю, что у нас CreateDefaultSong возвращает объект типа Song
// который уже имеет в себе объекты Album и Artist. Так что сзаново создавать и заполнять
// эти массивы не вижу смысла
			Song[] Songs = new Song[5];
//			Album[] Albums = new Album[5];
//			Artist[] Artists = new Artist[5];

//			int TotalDuration = 0, NumberOfShortestSong = 0;
			int NumberOfShortestSong = 0;
			TotalDuration = 0;


//По общему смыслу, я так понял, ref и out действуют похожим образом - передают переменную
// в вызываемый метод. Разница в том, что, используя ref, переменная уже должна иметь какое-либо
// значение, которое(именно значение переменной, не копия) может изменяться в методе.
// А при модификаторе out переменная в методе считается неинициализированной, но ей обязательно
// должно быть присвоено значение перед выходом.
// Не уверен, что это правильно, но я бы использовал ref именно для тех случаев, когда
// передаваемое значение может  меняться, а out для того, чтобы кроме переменной в return
// возвращать из метода ещё несколько значений

			TheShortestSong = 100000;

			for(int i = 0; i < Songs.Length; i++)
			{
				Songs[i] = CreateSong();
//				Albums[i] = Songs[i].Album;
//				Artists[i] = Songs[i].Artist;

				Console.WriteLine($"i = {i}");
				PrintSongInfo(Songs[i]);

				TotalDuration += Songs[i].Duration;
				if(Songs[i].Duration < TheShortestSong)
				{
					TheShortestSong = Songs[i].Duration;
					NumberOfShortestSong = i;
				}
				if(Songs[i].Duration > TheLongestSong)
				{
					TheLongestSong = Songs[i].Duration;
				}
			}

			Songs[NumberOfShortestSong].Name = ShortestSongName;
			Console.WriteLine($"Самая коротакя песня - №{NumberOfShortestSong}");
			Console.WriteLine($"i = {NumberOfShortestSong}");
			PrintSongInfo(Songs[NumberOfShortestSong]);

//			return TotalDuration;
		}





		public static Artist AddArtist(string ArtistName = "Unknown artist")
//		public static Artist AddArtist(string ArtistName = default(string))
		{
			Artist NewArtist = new Artist()
			{
				Name = ArtistName,
				Genre = "Default genre"
			};

			return NewArtist;
		}



		public static Album AddAlbum(string AlbumName = "Unknown album", int AlbumYear = 0)
//		public static Album AddAlbum(string AlbumName = default(string), int AlbumYear = default(int))
//Во втором случае AlbumName присваивается ничего(символ канца строки?),
// а AlbumYear, как и в первом случае, 0
		{
			Album NewAlbum = new Album()
			{
				Name = AlbumName,
				Year = AlbumYear
			};

			return NewAlbum;
		}
	}
}
