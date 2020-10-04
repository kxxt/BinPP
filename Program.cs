/************************************************
 * This software is licensed under GPL License. *
 * Built By Believers in Science Studio , 2020  *
 *												*
 * THIS SOFTWARE IS PROVIDED "AS-IS" , WITHOUT  *
 * ANY EXPRESSED OR IMPLIED WARRANTY .          *
 *                                              *
 *        USE IT AT YOUR OWN RISK !             *
 ***********************************************/
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SbC51CaoDanBinaryToHex
{
	class Program
	{
		public static int Main(string[] args)
		{
			try
			{
				return Exec(args);
			}
			catch
			{
				Console.WriteLine("Fatal Error in BinPP !");
				return -1;
			}
		}
		static int Exec(string[] args)
		{
			if (args.Length == 0)
			{
				DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory);
				foreach (var x in di.GetFiles())
				{
					if (x.Name.EndsWith(".c") || x.Name.EndsWith(".h"))
					{
						var str = File.ReadAllText(x.FullName);
						if (str.StartsWith("//BINPP\r\n") && str.EndsWith("\r\n//BINPP"))
							x.Delete();
					}
				}
				foreach (var x in di.GetFiles())
				{
					if (x.Name.EndsWith(".binpp"))
					{
						var p = x.FullName.Remove(x.FullName.Length - 6);
						Console.WriteLine(p);
						x.MoveTo(p);
					}
				}
				return 0;
			}

			else
			{
				foreach (var t in args)
				{
					string file = File.ReadAllText(t);
					string raw = file;
					var nf = t + ".binpp";
					File.WriteAllText(nf, raw);
					Regex regex8 = new Regex("0[Bb][0-1]{8}");
					Regex regex16 = new Regex("0[Bb][0-1]{16}");
					Regex regex32 = new Regex("0[Bb][0-1]{32}");

					var ma32 = regex32.Matches(file);
					foreach (Match x in ma32)
					{
						string s = x.Value;
						var bin = s.Substring(2);
						uint val = Convert.ToUInt32(bin, 2);
						string hex = "0x" + val.ToString("x8");
						file = file.Replace(s, hex);
					}

					var ma16 = regex16.Matches(file);
					foreach (Match x in ma16)
					{
						string s = x.Value;
						var bin = s.Substring(2);
						uint val = Convert.ToUInt32(bin, 2);
						string hex = "0x" + val.ToString("x4");
						file = file.Replace(s, hex);
					}
					var ma8 = regex8.Matches(file);
					foreach (Match x in ma8)
					{
						string s = x.Value;
						var bin = s.Substring(2);
						uint val = Convert.ToUInt32(bin, 2);

						string hex = "0x" + val.ToString("x2");
						file = file.Replace(s, hex);
					}

					File.WriteAllText(t, "//BINPP\r\n" + file + "\r\n//BINPP");

				}
				return 0;
			}
		}
	}
}
