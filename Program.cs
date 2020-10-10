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
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BinPP
{
	public static class Program
	{
		private struct Range
		{
			public int start, end;
			public Range(int st,int ed)
			{
				start = st;end = ed;
			}
		}
		private static bool debug=true;
		private const string StringLiteralMatchRegex = @"
(               # Capturing group for the string
    @""               # verbatim string - match literal at-sign and a quote
    (?:
        [^""]|""""    # match a non-quote character, or two quotes
    )*                # zero times or more
    ""                #literal quote
|               #OR - regular string
    ""              # string literal - opening quote
    (?:
        \\.         # match an escaped character,
        |[^\\""]    # or a character that isn't a quote or a backslash
    )*              # a few times
    ""              # string literal - closing quote
)";
		private static Regex strlmregex = new Regex(StringLiteralMatchRegex,RegexOptions.IgnorePatternWhitespace);
		/// <summary>
		/// https://stackoverflow.com/questions/4953737/regex-for-matching-c-sharp-string-literals
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static MatchCollection MatchStringLiteral(string source)
		{
			return Regex.Matches(source, StringLiteralMatchRegex,
										RegexOptions.IgnorePatternWhitespace);
		}
		public static string ReplaceBinWithHex(this string source, int len)
		{
			var listr = new List<Range>();	
			string ret = source;
			Regex regex = new Regex($"0[Bb][0-1]{{{len}}}");
			Match x = regex.Match(ret);
			while (x != Match.Empty)
			{
				listr.Clear();
				var mc = MatchStringLiteral(ret);
				foreach (Match xx in mc)
				{
					listr.Add(new Range(xx.Index, xx.Index + xx.Value.Length - 1));
				}
				string s = x.Value;
				int st = x.Index, ed = x.Index + x.Length - 1;
				var instr = false;
				foreach (var range in listr)
					if (range.start <= st && ed <= range.end)
					{
						instr = true;
						break;
					}
				if (instr) {
					x = x.NextMatch();
					continue;
				}
				var bin = s.Substring(2);
				uint val = Convert.ToUInt32(bin, 2);
				string hex = "0x" + val.ToString($"x{GetHexLength(len)}");
				string begin = ret.Substring(0, x.Index);
				string end = ret.Substring(x.Index + x.Length);
				ret = begin + hex + end;
				x = regex.Match(ret);
			}
			return ret;
		}
		private static int GetHexLength(int len)
		{
			return len switch
			{
				<=4=>1,
				>4 and <=8=>2,
				>8 and <=16=>4,
				>16 and <=32=>8,
				_=>throw new NotImplementedException("Not Implemented!")
			};
		}
		private static void RestoreCurrentDir()
		{
			RestoreDir(Environment.CurrentDirectory);
		}
		private static void RestoreDir(string dir)
		{
			DirectoryInfo di = new DirectoryInfo(dir);
			RestoreDir(di);
		}
		private static void RestoreDir(DirectoryInfo di)
		{
			Console.WriteLine($"Restoring directory {di.FullName}.");
			foreach (var subdir in di.GetDirectories())
			{
				RestoreDir(subdir.FullName);
			}
			foreach (var x in di.GetFiles())
			{
				if (x.Name.EndsWith(".c") || x.Name.EndsWith(".h"))
				{
					var str = File.ReadAllText(x.FullName);
					if (str.StartsWith("//BINPP\r\n") && str.EndsWith("\r\n//BINPP"))
						if(File.Exists(x.FullName+".binpp"))x.Delete();
						else
						{
							Console.WriteLine($"ERROR : Backup file of {x.Name} not found!Skipping Restoration!\nYou need to manually restore this file!");
							
						}
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
		}
		private static void RestoreFile(string fn)
		{
			Console.WriteLine($"Restoring file {fn} ...");
			var x = new FileInfo(fn);
			if (x.Name.EndsWith(".c") || x.Name.EndsWith(".h"))
			{
				var str = File.ReadAllText(x.FullName);
				if (str.StartsWith("//BINPP\r\n") && str.EndsWith("\r\n//BINPP"))
					x.Delete();
			}
			x = new FileInfo(x.FullName+".binpp");
			var p = x.FullName.Remove(x.FullName.Length - 6);
			Console.WriteLine(p);
			x.MoveTo(p);
		}
		public static int Main(string[] args)
		{
			try
			{
				return Exec(args);
			}
			catch(Exception e)
			{
				Console.WriteLine("Fatal Error in BinPP ! Possibily permission denied.");
				Console.WriteLine($"Technical Details :\n===== Stack Trace =====\n{e.StackTrace}\n===== Error Message =====\n{e.Message}");
				return -1;
			}
		}
		static int Exec(string[] args)
		{
			if (args.Length == 0)
			{
				RestoreCurrentDir();
				return 0;
			}
			else
			{
				if (args[0] == "restore")
				{
					if (args.Length > 1)
					{
						foreach(var entry in args)if(entry!="restore")
						{
							Restore(entry);
						}
					}
					else RestoreCurrentDir();
					return 0;
				}else if (args[0] == "help")
				{
					Console.WriteLine("For usage of this application , please read the README.MD .");
					return 0;
				}
				foreach (var t in args)
				{
					try
					{
						Console.WriteLine($"Processing {t} ...");
						string file = File.ReadAllText(t);
						string raw = file;
						var nf = t + ".binpp";
						File.WriteAllText(nf, raw);
						var mc = MatchStringLiteral(file);
						file = file
							.ReplaceBinWithHex(32)
							.ReplaceBinWithHex(16)
							.ReplaceBinWithHex(8)
							.ReplaceBinWithHex(7)
							.ReplaceBinWithHex(6)
							.ReplaceBinWithHex(5)
							.ReplaceBinWithHex(4)
							.ReplaceBinWithHex(3)
							.ReplaceBinWithHex(2);
						File.WriteAllText(t, "//BINPP\r\n" + file + "\r\n//BINPP");
					}
					catch
					{
						Console.WriteLine($"ERROR : Error occurred while processing file {t} , Skip it .");
						if(debug)throw;
					}
				}
				Console.WriteLine("All done!");
				return 0;
			}
		}

		private static void Restore(string entry)
		{
			if (File.Exists(entry)) RestoreFile(entry);
			else if (Directory.Exists(entry)) RestoreDir(entry);
			else Console.WriteLine($"Skipped the restoration of {entry} , since it's not found.");
		}
	}
}
