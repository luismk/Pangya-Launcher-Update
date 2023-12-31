﻿using System;
using System.IO;

namespace PangyaAPI.Utilities
{
    //Criado por LuisMK, time=> 24/02/2022 as 19:36 PM
    public class IniHandle
    {
        private readonly string fn;
        private readonly string[] lines;

        // Constructor

        public IniHandle(string _filename)
        {
            try
            {
                //caso o arquivo não existir, é lançado uma exceção
                if (File.Exists(_filename) == false)
                {
                    throw new Exception($"File no Exist: {_filename}");
                }
                else
                {
                    //caso o arquivo existir, é adicionado um local + nome do arquivo
                   var FilePath = AppDomain.CurrentDomain.BaseDirectory + _filename;
                    using (StreamReader stream = new StreamReader(FilePath))
                    {
                        fn = FilePath;
                        lines = stream.ReadToEnd().Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
                    }
                   
                }

            }
            //caso caia no exception
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        // Get group-range
        int[] GroupPos(string group = "")
        {
            if (group == "")
            {
                return new[] { 0, 0 };// No Group.
            }

            string lowerline;
            int[] ret = new[] { -1, -1 };
            for (int i = 0; i < lines.Length; i++)
            {
                lowerline = lines[i].ToLower();

                if (ret[0] < 0)
                {
                    if (lowerline.Contains("[" + group.ToLower() + "]"))
                    {
                        ret[0] = i; // Group found.
                    }

                }
                else
                {
                    if (lowerline.StartsWith("[") || i == lines.Length - 1) // next group or end of file.
                    {
                        ret[1] = lines.Length; // End of group found.
                        return (ret);
                    }
                }
            }
            WriteConsole.Error("Unable to find Group '" + group + "' in configuration file '" + fn + "'.");
            return ret; // Group not found.
        }

        public string ReadString(string group, string key, string _default = "", int min = int.MinValue, int max = int.MaxValue)
        {
            var group_index = GroupPos(group);
            if (group_index[0] < 0 || group_index[1] > lines.Length)
            {
                 WriteConsole.Error("Invalid group index (start: '" + group_index[0].ToString() + "' ; end: '" + group_index[1].ToString() + "') in configuration file '" + fn + "'. Defaulting values...");
                return _default;
            }

            string[] tarr = null;
            for (int i = group_index[0]; i < group_index[1]; i++)
            {
                if (lines[i].StartsWith(key))
                {
                    tarr = lines[i].Split(new[] { "=" }, StringSplitOptions.None);
                    break;
                }
            }

            string ret = "";
            if (tarr == null)
            {
                ret = _default;
            }
            else
            {
                if (tarr[1].Contains(" "))
                {
                    ret = tarr[1].Replace(" ", "");
                }
                else if (tarr[1].Contains("\t"))
                {
                    ret = tarr[1].Replace("\t", "");
                }
            }

            // Assuming integer value and checking min / max values.
            if (min != int.MinValue || max != int.MaxValue)
            {
                int iret = Convert.ToInt32(ret);
                if (iret < min || iret > max)
                {
                     WriteConsole.Warning("Invalid value '" + iret.ToString() + "' (Min: " + min.ToString() + " Max: " + max.ToString() + ") for '" + key + "' in configuration file '" + fn + "'. Defaulting value...");
                    ret = _default;
                }
            }

            return ret;
        }

        /// <summary>
        /// Ler o arquivo .ini e retorna Int32
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public Int32 ReadInt32(string section, string key, int def)
        {
            return Convert.ToInt32(ReadString(section, key, def.ToString()));
        }
        /// <summary>
        /// Ler o arquivo .ini e retorna UInt32
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public UInt32 ReadUInt32(string section, string key, uint def)
        {
            return Convert.ToUInt32(ReadString(section, key, def.ToString()));

        }
        /// <summary>
        /// Ler o arquivo .ini e retorna Int64
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public Int64 ReadInt64(string section, string key, long def)
        {
            return Convert.ToInt64(ReadString(section, key, def.ToString()));

        }
        /// <summary>
        /// Ler o arquivo .ini e retorna UInt64
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public UInt64 ReadUInt64(string section, string key, ulong def)
        {
            return Convert.ToUInt64(ReadString(section, key, def.ToString()));

        }
        /// <summary>
        /// Ler o arquivo .ini e retorna Int16
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public Int16 ReadInt16(string section, string key, short def)
        {
            return Convert.ToInt16(ReadString(section, key, def.ToString()));
        }
        /// <summary>
        /// Ler o arquivo .ini e retorna UInt16
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public UInt16 ReadUInt16(string section, string key, ushort def)
        {
            return Convert.ToUInt16(ReadString(section, key, def.ToString()));
        }
        /// <summary>
        /// Ler o arquivo .ini e retorna Byte
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public Byte ReadByte(string section, string key, Byte def)
        {
            return Convert.ToByte(ReadString(section, key, def.ToString()));
        }
        /// <summary>
        /// Ler o arquivo .ini e retorna SByte
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public SByte ReadSByte(string section, string key, sbyte def)
        {
            return Convert.ToSByte(ReadString(section, key, def.ToString()));
        }

        /// <summary>
        /// Ler o arquivo .ini e retorna bool
        /// </summary>
        /// <param name="section">Seção = cabeçario [Config]</param>
        /// <param name="key">Local = nomedealgo = 0 </param>
        /// <param name="def">padrao = caso não encontre o valor no key, retorna o def</param>
        /// <returns>string</returns>
        public bool ReadBool(string section, string key, bool def)
        {
            var result = ReadString(section, key, def.ToString());
            return Convert.ToBoolean(result);

        }
    }
}
