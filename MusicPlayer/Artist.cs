using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public class Artist
	{
		public Genres Genre;
		public string Name;

		public Artist()
		{
			Name = "Default artist";
			Genre = Genres.Undefined;
		}

		public Artist(string name)
		{
			Name = name;
			Genre = Genres.Undefined;
		}

		public Artist(string name, Genres genre)
		{
			Name = name;
			Genre = genre;
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
