using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mslugx_form
{
    class MemoryRead
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        public static string  Read_Score()
        {

            Process game_process = Process.GetProcessesByName("mslug1")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, game_process.Id);

            int bytesRead = 0;
            byte[] buffer = new byte[14]; //Max score = 9999999 takes 8*2 bytes because of Unicode 

            
            var metalSlugBaseAddress = Convert.ToInt32(game_process.MainModule.BaseAddress.ToString("X"),16);
            var score_offset = 0x000FE160; //using CheatEngine
            var absoluteAddress = metalSlugBaseAddress + score_offset;
            ReadProcessMemory((int)processHandle, absoluteAddress, buffer, buffer.Length, ref bytesRead);
            return (BitConverter.ToInt32(buffer,0).ToString() + " (" + bytesRead.ToString() + "bytes)");
            //Console.WriteLine(Encoding.Unicode.GetString(buffer) + " (" + bytesRead.ToString() + "bytes)");
            //Console.ReadLine();
            //0x010CE160
        }

        public static string GetPrisoners()
        {
            Process game_process = Process.GetProcessesByName("mslug1")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, game_process.Id);
            int bytesRead = 0;
            byte[] buffer = new byte[4]; //Max prisoners = 14 takes 2*2 bytes because of Unicode 
            var metalSlugBaseAddress = Convert.ToInt32(game_process.MainModule.BaseAddress.ToString("X"), 16);
            var prisoners_offset = 0x000FE348;
            var absoluteAddress = metalSlugBaseAddress + prisoners_offset;
            ReadProcessMemory((int)processHandle, absoluteAddress, buffer, buffer.Length, ref bytesRead);
            return (BitConverter.ToInt32(buffer, 0).ToString() + " (" + bytesRead.ToString() + "bytes)");

        }
    }
}
