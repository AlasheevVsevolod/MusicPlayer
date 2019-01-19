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
			//var player = new Player(new ClassicSkin());
			//var player = new Player(new ColoredSkin(ConsoleColor.DarkGreen));
			var player = new Player(new RandColorSkin());

			player.Add(GetSongsData());


			player.Add(CreateSong("Sahti"));
			player.Add(CreateSong("Kunnia"));
			player.Add(CreateSong("Ievan Polkka"));

			player.Add(CreateSong("Uptown Funk", 123, "Bruno Mars", Genres.Pop, "Uptown Funk", 
			2015, false));

			player.Add(CreateSong("Her Ghost", 174, "Dance With The Dead", 
			Genres.Rock | Genres.Synthwave | Genres.Electronic, "The Shape", 2016));
			player.Add(CreateSong("Robeast", 234, "Dance With The Dead", 
			Genres.Electronic | Genres.Synthwave, "Out Of Body", 2013, true));

			player.Add(CreateSong("Luxtos", 346, "Eluveitie", Genres.Folk | Genres.Metal, 
			"Helvetios", 2012));
			player.Add(CreateSong("A Rose For Epona", 267, "Eluveitie", Genres.Folk, "Helvetios", 
			2012, false));
			player.Add(CreateSong("Inis Mona", 328, "Eluveitie", Genres.Folk | Genres.Rock, "Slania", 
			2008, true));

//			Console.WriteLine("Sorted list:");
			player.SortByTitle();
			player.Start(true);

			List<Song> tmpList = new List<Song>();
			tmpList = player.Songs.ToList();

//			Console.WriteLine("Filtered by genres list: Synthwave");
			player.FilterByGenre(Genres.Synthwave);
			player.Start(true);
			player.Clear();
			player.Add(tmpList.ToArray());

//			Console.WriteLine("Filtered by genres list: Folk & Rock");
			player.FilterByGenre(Genres.Folk, Genres.Rock);
			player.Start(true);
			player.Clear();
			player.Add(tmpList.ToArray());



			Console.ReadLine();
		}


		public static Song GetSongsData()
		{
			var song = new Song()
			{
				Duration = 100,
				Name = "New song",
				Album = new Album
				{
					Name = "New Album",
					Year = 2018
				},
				Artist = new Artist
				{
					Name = "Powerwolf",
					Genre = "Metal"
				}
			};

			return song;
		}


		public static Song CreateSong()
		{
			var rand = new Random();

			var tmpSongName = Convert.ToString((char)rand.Next((int)'A', (int)'Z'));
			var tmpDuration = rand.Next(60, 301);

			var tmpArtistName = Convert.ToString((char)rand.Next((int)'A', (int)'Z'));

			var tmpAlbumName = Convert.ToString((char)rand.Next((int)'A', (int)'Z'));
			var tmpAlbumYear = rand.Next(1980, DateTime.Today.Year);

			System.Threading.Thread.Sleep(20);

			return CreateSong(tmpSongName, tmpDuration, tmpArtistName, 0, tmpAlbumName, tmpAlbumYear);
		}

		public static Song CreateSong(string SongName)
		{
			var rand = new Random();

			var tmpDuration = rand.Next(60, 301);

			var tmpArtistName = "Drfault Artist";

			var tmpAlbumName = "Default Album";
			var tmpAlbumYear = rand.Next(1980, DateTime.Today.Year);

			System.Threading.Thread.Sleep(20);

			return CreateSong(SongName, tmpDuration, tmpArtistName, 0, tmpAlbumName, tmpAlbumYear);
		}

		public static Song CreateSong(string SongName, int SongDuration, string ArtistName,
		Genres ArtistGenre, string AlbumName, int AlbumYear, bool? Like = null)
		{
			var ExplicitSong = new Song()
			{
				Duration = SongDuration,
				Album = new Album()
				{
					Name = AlbumName,
					Year = AlbumYear
				},
				Artist = new Artist()
				{
					Name = ArtistName
				},
				Name = SongName,
				Like = Like
			};
			ExplicitSong.Artist.Genre = ExplicitSong.Artist.GetArtistGenre(ArtistGenre);

			return ExplicitSong;
		}
	}
}
