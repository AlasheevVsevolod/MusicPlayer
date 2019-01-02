using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public class Album
	{
//		public List<byte> Thumbnail;
		public string Name;
		public int Year;

		public Album()
		{
			Name = "Default album";
			Year = 2000;
		}

		public Album(string name, int year)
		{
			Name = name;
			Year = year;
		}
	}
}
