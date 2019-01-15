using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public class Artist
	{
		public string Genre;
		public string Name;

		public Artist()
		{
			Name = "Default artist";
			Genre = "Default genre";
		}

		public Artist(string name)
		{
			Name = name;
			Genre = "Default genre";
		}

		public Artist(string name, string genre)
		{
			Name = name;
			Genre = genre;
		}

		public string GetArtistGenre(Genres genres)
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

	public enum Genres
	{
		Undefined		= 0b00000000,
		Rock				= 0b00000001,
		Metal				= 0b00000010,
		Synthwave		= 0b00000100,
		Electronic	= 0b00001000,
		Folk				= 0b00010000,
		Pop					= 0b00100000,
	};
}
