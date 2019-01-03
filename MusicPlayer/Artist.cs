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
	}
}
