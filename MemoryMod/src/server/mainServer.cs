using LogicAPI.Server.Components;
using LogicWorld.Server.Circuitry;
using LICC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace Ryan.Memory
{
    public class Memory_Mod
    {
        public static string FlashLocationGlobal = "";
    }

    public class ROM : LogicComponent<ROM.IData>
    {
        public interface IData
        {
			byte[] data { get; set; }
            string ROMFlash { get; set; }
        }

        int DataWidth = 8;

        protected override void DoLogicUpdate()
        {
            //Input base.Inputs[NUM].On
            //Output base.Outputs[NUM].On
            //UseTable Data.data[NUM]

            //if (DataWidth == 0)
            //{
            //    DataWidth = base.Outputs.Count;
			//	LConsole.WriteLine(DataWidth);
            //}
			if (base.Inputs[0].On)
			{
				string dir = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 7);
				if (File.Exists(dir +Data.ROMFlash))
				{
					string data = File.ReadAllText(dir +Data.ROMFlash);
					data = data.ToLower();
					if (data.Substring(0, 5) == "mmrom")
					{
						data = data.Substring(5);
						if (data.Contains(";"))
						{
							int sc = data.IndexOf(";");
							string settingsNS = data.Substring(0, sc);
							while (settingsNS.Contains(" "))
							{
								int first = settingsNS.IndexOf(" ");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\n"))
							{
								int first = settingsNS.IndexOf("\n");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\r"))
							{
								int first = settingsNS.IndexOf("\r");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							string[] settings = settingsNS.Split(',');
							data = data.Substring(sc + 1);

							string readMode = "digit";
							if (settings.Length > 1)
							{
								if (settings[1] == "lb")
								{
									readMode = "lb";

								}
								else if (settings[1] == "lbs")
								{
									readMode = "lbs";

								}
							}

							int format = 0;
							int dpa = 0;
							bool continu = true;
							if (settings[0] == "h")
							{
								format = 16;
								dpa = DataWidth / 4;
							}
							else if (settings[0] == "b")
							{
								format = 2;
								dpa = DataWidth;
							}
							else if (settings[0] == "d")
							{
								format = 10;
								dpa = Math.Pow(2, DataWidth).ToString().Length;
							}
							else if (settings[0] == "o")
							{
								format = 8;
								dpa = (int)Math.Ceiling((double)DataWidth / 3);
							}
							else
							{
								LConsole.WriteLine($"{dir +Data.ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 2");
								continu = false;
							}
							if (continu)
							{
								if (readMode == "digit")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\n"))
									{
										int first = data.IndexOf("\n");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									byte[] NewData = new byte[data.Length / dpa];
									int j = 0;
									try
									{

										for (int i = 0; i < data.Length / dpa; i++)
										{
											NewData[i] = Convert.ToByte(Convert.ToInt32(data.Substring(i * dpa, dpa), format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir +Data.ROMFlash} has {data.Substring(j * dpa, dpa)} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
								else if (readMode == "lb")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									string[] NewDataStr = data.Split('\n');
									byte[] NewData = new byte[NewDataStr.Length];
									int j = 0;
									try
									{
										for (int i = 0; i < NewDataStr.Length; i++)
										{

											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToByte(Convert.ToInt32(dataa, format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}
										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir +Data.ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
								else if (readMode == "lbs")
								{
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									string[] NewDataStr = data.Split(new char[] { '\n', ' ' });
									byte[] NewData = new byte[NewDataStr.Length];
									int j = 0;
									try
									{
										for (int i = 0; i < NewDataStr.Length; i++)
										{
											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToByte(Convert.ToInt32(dataa, format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir +Data.ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
							}
						}
						else
						{
							LConsole.WriteLine($"{dir +Data.ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 1");
						}
					}
					else
					{
						LConsole.WriteLine($"{dir +Data.ROMFlash} is not a Memory Mod ROM file");
					}
				}
				else
				{
					LConsole.WriteLine($"{dir +Data.ROMFlash} is not a real file. REMEMBER: it's looking for that location in the Logic World directory!!!");
				}

			}

			if(base.Inputs[1].On)
			{
				Data.ROMFlash = Memory_Mod.FlashLocationGlobal;
			}
			int Address = 0;
			for (int i = 2; i < base.Inputs.Count; i++)
			{
				Address = Address + (int)Math.Pow(2, i - 2) * Convert.ToInt32(base.Inputs[i].On);
			}
			int DataToOutput = 0;
			bool IndexError = false;
			try
			{
				DataToOutput = Data.data[Address];
			}
			catch (System.IndexOutOfRangeException)
			{
				IndexError = true;
			}
			for (int i = 0; i < base.Outputs.Count; i++)
			{
				if (!IndexError)
				{
					base.Outputs[i].On = Convert.ToBoolean((DataToOutput >> i) % 2);
				}
				else
				{
					base.Outputs[i].On = false;
				}
			}
		}
        private bool _HasPersistentValues = true;
        public override bool HasPersistentValues => _HasPersistentValues;
        protected override void SetDataDefaultValues()
        {
            Data.data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            Data.ROMFlash = "";
        }

    }

	public class RAM : LogicComponent<RAM.IData>
	{
		public interface IData
		{
			byte[] data { get; set; }
			string ROMFlash { get; set; }
		}

		int DataWidth = 0;
		int AddressWidth;

		protected override void DoLogicUpdate()
		{
			//Input base.Inputs[NUM].On
			//Output base.Outputs[NUM].On
			//UseTable Data.data[NUM]

			if (DataWidth == 0)
			{
			    DataWidth = base.Outputs.Count;
				AddressWidth = base.Inputs.Count - 4 - DataWidth;
			}
			if (base.Inputs[1].On)
			{
				string dir = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 7);
				if (File.Exists(dir + Data.ROMFlash))
				{
					string data = File.ReadAllText(dir + Data.ROMFlash);
					data = data.ToLower();
					if (data.Substring(0, 5) == "mmrom")
					{
						data = data.Substring(5);
						if (data.Contains(";"))
						{
							int sc = data.IndexOf(";");
							string settingsNS = data.Substring(0, sc);
							while (settingsNS.Contains(" "))
							{
								int first = settingsNS.IndexOf(" ");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\n"))
							{
								int first = settingsNS.IndexOf("\n");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							while (settingsNS.Contains("\r"))
							{
								int first = settingsNS.IndexOf("\r");
								var Removed = settingsNS.Substring(0, first) + settingsNS.Substring(first + 1);
								settingsNS = Removed;
							}
							string[] settings = settingsNS.Split(',');
							data = data.Substring(sc + 1);

							string readMode = "digit";
							if (settings.Length > 1)
							{
								if (settings[1] == "lb")
								{
									readMode = "lb";

								}
								else if (settings[1] == "lbs")
								{
									readMode = "lbs";

								}
							}

							int format = 0;
							int dpa = 0;
							bool continu = true;
							if (settings[0] == "h")
							{
								format = 16;
								dpa = DataWidth / 4;
							}
							else if (settings[0] == "b")
							{
								format = 2;
								dpa = DataWidth;
							}
							else if (settings[0] == "d")
							{
								format = 10;
								dpa = Math.Pow(2, DataWidth).ToString().Length;
							}
							else if (settings[0] == "o")
							{
								format = 8;
								dpa = (int)Math.Ceiling((double)DataWidth / 3);
							}
							else
							{
								LConsole.WriteLine($"{dir + Data.ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 2");
								continu = false;
							}
							if (continu)
							{
								if (readMode == "digit")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\n"))
									{
										int first = data.IndexOf("\n");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									byte[] NewData = new byte[data.Length / dpa];
									int j = 0;
									try
									{

										for (int i = 0; i < data.Length / dpa; i++)
										{
											NewData[i] = Convert.ToByte(Convert.ToInt32(data.Substring(i * dpa, dpa), format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir + Data.ROMFlash} has {data.Substring(j * dpa, dpa)} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
								else if (readMode == "lb")
								{
									while (data.Contains(" "))
									{
										int first = data.IndexOf(" ");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									string[] NewDataStr = data.Split('\n');
									byte[] NewData = new byte[NewDataStr.Length];
									int j = 0;
									try
									{
										for (int i = 0; i < NewDataStr.Length; i++)
										{

											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToByte(Convert.ToInt32(dataa, format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}
										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir + Data.ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
								else if (readMode == "lbs")
								{
									while (data.Contains("\r"))
									{
										int first = data.IndexOf("\r");
										var Removed = data.Substring(0, first) + data.Substring(first + 1);
										data = Removed;
									}
									if (data.Substring(0, 1) == "\n")
									{
										data = data.Substring(1);
									}
									string[] NewDataStr = data.Split(new char[] { '\n', ' ' });
									byte[] NewData = new byte[NewDataStr.Length];
									int j = 0;
									try
									{
										for (int i = 0; i < NewDataStr.Length; i++)
										{
											string dataa = NewDataStr[i];
											if (dataa.Length > dpa)
											{
												dataa = dataa.Substring(0, dpa);
											}
											NewData[i] = Convert.ToByte(Convert.ToInt32(dataa, format));
											j++;
										}
									}
									catch (Exception ex)
									{
										string basestr = "???";
										if (format == 2)
										{
											basestr = "binary";
										}
										if (format == 8)
										{
											basestr = "octal";
										}
										if (format == 10)
										{
											basestr = "decimal";
										}

										if (format == 16)
										{
											basestr = "hexadecimal";
										}
										LConsole.WriteLine($"{dir + Data.ROMFlash} has {NewDataStr[j]} which is not a valid {basestr} number.");
									}
									Data.data = NewData;
								}
							}
						}
						else
						{
							LConsole.WriteLine($"{dir + Data.ROMFlash} is not a correctly formatted Memory Mod ROM file. Error 1");
						}
					}
					else
					{
						LConsole.WriteLine($"{dir + Data.ROMFlash} is not a Memory Mod ROM file");
					}
				}
				else
				{
					LConsole.WriteLine($"{dir + Data.ROMFlash} is not a real file. REMEMBER: it's looking for that location in the Logic World directory!!!");
				}

			}

			if (base.Inputs[2].On)
			{
				string[] SaveData = new string[Data.data.Length + 2];
				SaveData[0] = "MMROM";
				SaveData[1] = "D, LB;";
				for (int i = 0; i < Data.data.Length; i++)
				{
					SaveData[i + 2] = Data.data[i].ToString();
				}
				File.WriteAllLines(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 7) + Data.ROMFlash, SaveData, Encoding.UTF8);
			}
			if (base.Inputs[3].On)
			{
				Data.ROMFlash = Memory_Mod.FlashLocationGlobal;
			}
			int Address = 0;
			for (int i = 4; i < this.Inputs.Count - DataWidth; i++)
			{
				Address = Address + (int)Math.Pow(2, i - 4) * Convert.ToInt32(this.Inputs[i].On);
			}
			if (base.Inputs[0].On)
			{
				byte DataToWrite = 0;
				for (int i = 4 + AddressWidth; i < base.Inputs.Count; i++)
				{
					DataToWrite = Convert.ToByte(DataToWrite + Math.Pow(2, i - (4 + AddressWidth)) * Convert.ToInt32(this.Inputs[i].On));
				}
				Data.data[Address] = DataToWrite;
			}
			int DataToOutput = 0;
			bool IndexError = false;
			try
			{
				DataToOutput = Data.data[Address];
			}
			catch (System.IndexOutOfRangeException)
			{
				IndexError = true;
			}
			for (int i = 0; i < base.Outputs.Count; i++)
			{
				if (!IndexError)
				{
					base.Outputs[i].On = Convert.ToBoolean((DataToOutput >> i) % 2);
				}
				else
				{
					base.Outputs[i].On = false;
				}
			}
		}
		private bool _HasPersistentValues = true;
		public override bool HasPersistentValues => _HasPersistentValues;
		protected override void SetDataDefaultValues()
		{
			Data.data = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
			Data.ROMFlash = "";
		}

	}
}