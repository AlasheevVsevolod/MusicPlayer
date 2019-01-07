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
			var player = new Player();

			player.Start();

			player.Add(GetSongsData());


			player.Add(CreateSong("Sahti"));
			player.Add(CreateSong("Kunnia"));
			player.Add(CreateSong("Ievan Polkka"));

			player.Add(CreateSong("Uptown Funk", 123, "Bruno Mars", "Pop", "Uptown Funk", 2015, false));
			player.Start();

			player.Add(CreateSong("Her Ghost", 174, "Dance With The Dead", "Electronic", "The Shape", 2016, true));
			player.Add(CreateSong("Robeast", 234, "Dance With The Dead", "Synthwave", "Out Of Body", 2013, true));

			player.Add(CreateSong("Luxtos", 346, "Eluveitie", "Folk Metal", "Helvetios", 2012, false));
			player.Add(CreateSong("A Rose For Epona", 267, "Eluveitie", "Folk Metal", "Helvetios", 2012, false));
			player.Add(CreateSong("Inis Mona", 328, "Eluveitie", "Folk Metal", "Slania", 2008, true));
//			player.Start(true);
			player.Start();

			var songsArr = new Song[3];
			for (int i = 0; i < songsArr.Length; i++)
			{
				songsArr[i] = CreateSong();
			}
			player.Add(songsArr);

			player.ShowAllSongsName();

			Console.WriteLine("Shuffled list:");
			player.Shuffle();
			player.Start(true);

			Console.WriteLine("Sorted list:");
			player.SortByTitle();
			player.Start(true);

			Console.WriteLine("Shuffled list:");
			player.Shuffle();
			player.Start(true);

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
			var tmpArtistGenre = Convert.ToString((char)rand.Next((int)'A', (int)'Z'));

			var tmpAlbumName = Convert.ToString((char)rand.Next((int)'A', (int)'Z'));
			var tmpAlbumYear = rand.Next(1980, DateTime.Today.Year);

			System.Threading.Thread.Sleep(20);

			return CreateSong(tmpSongName, tmpDuration, tmpArtistName, tmpArtistGenre, tmpAlbumName, tmpAlbumYear);
		}

		public static Song CreateSong(string SongName)
		{
			var rand = new Random();

			var tmpDuration = rand.Next(60, 301);

			var tmpArtistName = "Drfault Artist";
			var tmpArtistGenre = "Default Genre";

			var tmpAlbumName = "Default Album";
			var tmpAlbumYear = rand.Next(1980, DateTime.Today.Year);

			System.Threading.Thread.Sleep(20);

			return CreateSong(SongName, tmpDuration, tmpArtistName, tmpArtistGenre, tmpAlbumName, tmpAlbumYear);
		}

		public static Song CreateSong(string SongName, int SongDuration, string ArtistName,
		string ArtistGenre, string AlbumName, int AlbumYear, bool? Like = null)
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
					Genre = ArtistGenre,
					Name = ArtistName
				},
				Name = SongName,
				Like = Like
			};

			return ExplicitSong;
		}
	}
}
