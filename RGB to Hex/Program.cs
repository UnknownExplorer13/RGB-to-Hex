using System.Drawing;
using System.Text.RegularExpressions;

namespace RGB_to_Hex
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				ShowUsageText();
				return;
			}

			List<Color> colors = new List<Color>();

			#region Read Input
			Console.WriteLine("Reading input file...");
			using (StreamReader sr = new StreamReader(args[0]))
			{
				try
				{
					int lineCounter = 1;

					while (!sr.EndOfStream)
					{
						var line = sr.ReadLine();

						if (line != null)
						{
							if (StringIsCorrectlyFormatted(line)) // Make sure the line being read is in the correct RGB(A) format
							{
								string[] split = line.Split(','); // Split RGB(A) values of the line
								int r = 0;
								int g = 0;
								int b = 0;
								int a = 0;
								bool includeAlpha = false;

								if (split.Length == 3) // Line is in RGB format
								{
									r = int.Parse(split[0]);
									g = int.Parse(split[1]);
									b = int.Parse(split[2]);
								}
								else if (split.Length == 4) // Line is in RGBA format
								{
									r = int.Parse(split[0]);
									g = int.Parse(split[1]);
									b = int.Parse(split[2]);
									if (int.Parse(split[3]) != 255) // Include alpha if transparent (0-254); Ignore alpha if opaque (255)
									{
										a = int.Parse(split[3]);
										includeAlpha = true;
									}
								}

								// Add new Color to color list
								if (includeAlpha)
									colors.Add(Color.FromArgb(a, r, g, b));
								else
									colors.Add(Color.FromArgb(r, g, b));

								lineCounter++;
							}
							else // Line is incorrectly formatted
							{
								Console.WriteLine($"Skipped line {lineCounter}");
								Console.WriteLine($"   {line}");
								Console.WriteLine();
								lineCounter++;
							}
						}
					}
				}
				catch(Exception e)
				{
					Console.WriteLine($"{e.Message}\n\n{e.StackTrace}");
					Console.ReadLine();
					return;
				}
			}
			#endregion

			#region Write Output
			Console.WriteLine("Writing output file...");
			if (colors.Count > 0)
			{
				using (StreamWriter sw = new StreamWriter("Hex Codes.txt"))
				{
					int colCounter = 1;

					foreach (Color c in colors)
					{
						string hex;
						Console.WriteLine($"Color {colCounter}");

						if (c.A != 255)
						{
							Console.WriteLine($"RGBA: {c.R}, {c.G}, {c.B}, {c.A}");
							hex = ToHex(c, true);
						}
						else
						{
							Console.WriteLine($"RGB: {c.R}, {c.G}, {c.B}");
							hex = ToHex(c);
						}

						Console.WriteLine($"Hex: {hex}");
						Console.WriteLine();

						sw.WriteLine(hex);
						colCounter++;
					}
				}
			}
			#endregion

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		// Usage Text
		static void ShowUsageText()
		{
			Console.WriteLine("USAGE");
			Console.WriteLine("Drag and drop a file containing RGB and/or RGBA values in either format shown below onto the exe.");
			Console.WriteLine("Alpha values of 255 are ignored automatically.");
			Console.WriteLine();
			Console.WriteLine("FORMATS");
			Console.WriteLine("RGB");
			Console.WriteLine("   160, 25, 60");
			Console.WriteLine("   48, 60, 0");
			Console.WriteLine();
			Console.WriteLine("RGBA");
			Console.WriteLine("   20, 127, 30, 127");
			Console.WriteLine("   255, 127, 127, 60");
			Console.ReadLine();
		}

		// Check string to make sure it's formatted as RGB or RGBA
		static bool StringIsCorrectlyFormatted(string line)
		{
			Regex rgbFormat = new Regex(@"^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]), ([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]), ([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
			Regex rgbaFormat = new Regex(@"^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]), ([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]), ([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5]), ([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");

			if (rgbFormat.Match(line).Success) return true;
			else if (rgbaFormat.Match(line).Success) return true;
			else return false;
		}

		// Convert input Color to a hex code
		static string ToHex(Color c, bool formatRGBA = false)
		{
			if (formatRGBA)
			{
				string hex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2") + c.A.ToString("X2");
				return hex;
			}
			else
			{
				string hex = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
				return hex;
			}
		}
	}
}
