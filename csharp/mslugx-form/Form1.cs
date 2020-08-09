using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mslugx_form
{
    public partial class Form1 : Form
    {
        public bool cap = true;
        public static Process p = new Process();
        public Form1()
        {
            InitializeComponent();
            //backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //MoveWindow(Process.GetProcessesByName("csume")[0].MainWindowHandle, 0, 0, 640, 480, true);

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //while (cap)
            //{
            //    //Thread t = new Thread(capture);
            //    //t.Start();
            //    //capture();
            //    try
            //    {
            //        //capture();
            //        pictureBox1.Image = CaptureScreen();
            //        Thread.Sleep(1000);
            //    }
            //    catch (Exception ex)
            //    {

            //        MessageBox.Show("Error during capture : " + ex.Message);
            //    }

            //}
            //using (var i = CaptureScreen())
            //{
            //    while(cap)
            //    {
            //for(int i = 0;i<10;i++)
            //{
            //    pictureBox1.Image = CaptureScreen();

            //}

            //Thread.Sleep(5000);
            //    }
            //}

            //capture();
            if(!backgroundWorker1.IsBusy)
            {
                cap = true;
            backgroundWorker1.RunWorkerAsync();
            //backgroundWorker1.
            }
            //Task.Factory.StartNew(test);
            //Task t = new Task();
            //t.run

        }

        private void test()
        {
            try
            {
                while (cap)
                {
                    capture();
                    GC.Collect();//otherwise memory leak
                    //GC.WaitForPendingFinalizers();
                    //GC.Collect();
                    //
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in test function : " + ex.Message);
            }

        }

        private void capture()
        {
            Bitmap b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(b);
            g.CopyFromScreen(0, 0, 0, 0, b.Size);
            //Action action = () => pictureBox1.Image = b;
            //pictureBox1.BeginInvoke(action);
            pictureBox1.Image = b;
            //Thread.Sleep(1000);
        }



        private void button3_Click(object sender, EventArgs e)
        {
            cap = false;
        }

        #region crap

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);


        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            return img;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (cap)
                {
                    capture();
                    GC.Collect();
                    //
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error in do work function : "+ex.Message);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("opening mslugx");
                Task.Factory.StartNew(open_mslugx);
                //Task.Factory.StartNew(move);
                //open_mslugx();
                //SetParent(process.MainWindowHandle, this.Handle);

            }
            catch (Exception ex)
            {

                MessageBox.Show("Error :" + ex.Message);
            }
            
        }

        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool SetForegroundWindow(IntPtr hWnd);



        public  void open_mslugx()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "D:\\Telechargement\\METAL.SLUG-ALI213\\METAL SLUG\\mslug1.exe";
            //info.FileName = @"D:\RomStation\Emulation\Arcade\csUME\csume.exe";
            info.WorkingDirectory = @"D:\RomStation\Emulation\Arcade\csUME";
            //info.Arguments = @"csume mslugx -resolution 640x480";
            //info.Arguments = @"/D cd RomStation\Emulation\Arcade\csUME csume mslugx -resolution 640x480";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            
            p.StartInfo = info;
            
            try
            {
                p.Start();
                
                //using (StreamWriter sw = p.StandardInput)
                //{
                //    if (sw.BaseStream.CanWrite)
                //    {
                //        sw.WriteLine("csume mslugx -resolution 640x480");//-resolution 640x480
                //    }
                //}
                //StreamWriter sw = p.StandardInput;
                
                    //if (sw.BaseStream.CanWrite)
                    //{
                        //sw.WriteLine("csume mslugx -resolution 640x480");//-resolution 640x480
                                                                         //}
                                                                         //MoveWindow(p.MainWindowHandle, 0, 0, 640, 480, true);

                //p.Refresh();
                //p.WaitForInputIdle();

                //move();
                //Thread.Sleep(1000);
                ////MoveWindow(Process.GetProcessesByName("csume")[0].MainWindowHandle, 0, 0, 640, 480, true);

                //StreamReader sr = p.StandardOutput;
                
                //string result = sr.ReadToEnd();
                //MessageBox.Show(result);
                
                //SetForegroundWindow(this.Handle);
                //if (MoveWindow(Process.GetProcessesByName("csume")[0].MainWindowHandle, 0, 0, 640, 480, true))

                //{
                //    MessageBox.Show("has been moved");
                //}



                Thread.Sleep(1000);
                Process[] pnames = Process.GetProcessesByName("mslug1");
                if (pnames.Length > 0)
                {
                    //MessageBox.Show("found process !");
                    MoveWindow(pnames[0].MainWindowHandle, 0, 0, 640, 480, true);
                }




            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + ex.InnerException);
            }

            //p.WaitForExit();

            //using (StreamWriter sw = p.StandardInput)
            //{
            //    if (sw.BaseStream.CanWrite)
            //    {
            //        sw.WriteLine("csume mslugx");//-resolution 640x480
            //    }
            //}
            //Thread.Sleep(1000);
            //p.Kill();
            //string output = p.StandardOutput.ReadToEnd();
            //string error = p.StandardError.ReadToEnd();
            
            //MessageBox.Show(/*"Test"*/);
            
            
            //p.WaitForExit();

            //Process[] pnames = Process.GetProcessesByName("csume");
            //MessageBox.Show(p.ExitTime.ToString());
            //if (pnames.Length == 0)
            //    //Console.WriteLine("nothing");
            //    MessageBox.Show("nothing");
            //else
            //    //Console.WriteLine("run");
            //    MessageBox.Show("run");

            //MoveWindow(pnames[0].MainWindowHandle, 0, 0, 640, 480, true);

            //Bitmap b = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //Graphics g = Graphics.FromImage(b);
            //g.CopyFromScreen(0, 0, 0, 0, b.Size);




        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            //SVProcess = Process.GetProcessesByName("Stardew Valley")[0];
            //VAMemory vam = new VAMemory("League Of Legends");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //capture();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //p.Kill();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process[] pnames = Process.GetProcessesByName("mslug1");
            //MessageBox.Show(/*"Test"*/);
            if (pnames.Length > 0)
            {
                //MessageBox.Show("found process !");
                MoveWindow(pnames[0].MainWindowHandle, 0, 0, 640, 480, true);
            }

        }
        public static void move()
        {
            Process[] pnames = Process.GetProcessesByName("mslug1");
            if (pnames.Length > 0)
            {
                //MessageBox.Show("found process !");
                if(MoveWindow(pnames[0].MainWindowHandle, 0, 0, 640, 480, true))

                {
                    MessageBox.Show("has been moved");
                } 
            }

        }

        private void button6_Click(object sender, EventArgs e)

        {
            Process[] pnames = Process.GetProcessesByName("mslug1");
            if (pnames.Length > 0)
            {
                textBox1.Text = pnames[0].MainModule.EntryPointAddress.ToString();
                textBox2.Text = pnames[0].MainModule.EntryPointAddress.ToString("X");
            }
                
        }
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        private void button7_Click(object sender, EventArgs e)
        {
            //Process[] pnames = Process.GetProcessesByName("mslug1");
            //if (pnames.Length > 0)
            //{
            //    textBox3.Text = pnames[0].MainModule.BaseAddress.ToString();
            //    textBox4.Text = pnames[0].MainModule.BaseAddress.ToString("X");
            //}

            Process game_process = Process.GetProcessesByName("mslug1")[0];
            IntPtr processHandle = OpenProcess(0x0010, false, game_process.Id);
            textBox3.Text = game_process.MainModule.BaseAddress.ToString();
            textBox4.Text = game_process.MainModule.BaseAddress.ToString("X");



        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process[] pnames = Process.GetProcessesByName("mslug1");
            if (pnames.Length > 0)
            {
                var s = "test";
                var v = pnames[0].Modules;
                //MessageBox.Show(v.Count.ToString());
                MessageBox.Show(v[0].ModuleName);
                MessageBox.Show(v[0].BaseAddress.ToString());
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );
        [DllImport("kernel32", SetLastError = true, EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddressOrdinal(IntPtr hModule, IntPtr procName);

        private void button9_Click(object sender, EventArgs e)
        {
            Process[] pnames = Process.GetProcessesByName("mslug1");
            if (pnames.Length > 0)
            {
                var v = pnames[0].Modules;
                var address = GetProcAddress(pnames[0].Handle, "mslug1");
                MessageBox.Show(address.ToString());
            }

        }

        private void read_score_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MemoryRead.Read_Score()); 
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show((0x00FD0000 + 0x000FE160).ToString("X"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MessageBox.Show(MemoryRead.GetPrisoners());
        }
    }

}

//score : max value is 153, type = byte
//score : 4 addresses are side by side ( all bytes) 
//score addresses were : 1ABBF46A, 1ABBF46B, 1ABBF46C, 1ABBF46D
//1st check
//+		BaseAddress	{1248329728}	System.IntPtr = 4A680000
//offset = 2FFD4B93

//2nd check
//+		BaseAddress	{1245708288}	System.IntPtr = 4A400000
//offset = 2FF34B93


//3rd check
// base address : 49E30000
//offset = 2F1E0B93

//4th check
//base address : 4A710000
//offset = 2FA20B93

//5th check
//entry point : 4A21829A
//offset = 2F628E2D

//6th check
//entry point : 4A32829A
//offset = 2F768E2D




