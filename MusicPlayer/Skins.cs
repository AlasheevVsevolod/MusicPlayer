using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
	public interface ISkins
	{
		void Clear();
		void Render(string text);
		void Render();
	}

	public class ClassicSkin: ISkins
	{
		public void Clear()
		{
			Console.Clear();
		}

		public void Render(string text)
		{
			Console.WriteLine(text);
		}

		public void Render()
		{
			Console.WriteLine();
		}
	}

	public class ColoredSkin : ISkins
	{
		private ConsoleColor textColor;

		public ColoredSkin(ConsoleColor tmpColor)
		{
			textColor = tmpColor;
		}

		public void Clear()
		{
			Console.Clear();
		}

		public void Render(string text)
		{
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = textColor;
			Console.WriteLine(text);
			Console.ResetColor();
		}

		public void Render()
		{
			Console.WriteLine();
		}
	}

	public class RandColorSkin : ISkins
	{
		Random rand = new Random();

		public void Clear()
		{
			Console.Clear();
			int i = 30;
			while (i > 0)
			{
				Console.Write("\u058d");
				i--;
			}
			Console.WriteLine();
		}

		public void Render(string text)
		{
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = (ConsoleColor)rand.Next(15);
			string reversedString = new string(
				text.Select(c => char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c)).ToArray());
			Console.WriteLine(reversedString);
			Console.ResetColor();
		}

		public void Render()
		{
			Console.WriteLine();
		}
	}
}
