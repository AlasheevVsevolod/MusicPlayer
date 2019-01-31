using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public class Song
	{
		public string Name;
		public int Duration;
		public Artist Artist;
		public Album Album;
		[NonSerialized]
		public bool? Like = null;
		public string Location;

		public Song()
		{
			Name = "Default song";
			Duration = 100;
			Artist = new Artist();
			Album = new Album();
		}

		public void LikeSong()
		{
			Like = true;
		}

		public void DislikeSong()
		{
			Like = false;
		}
	}
}
