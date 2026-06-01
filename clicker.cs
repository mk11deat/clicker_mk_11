using System;
using System.Runtime.InteropServices;

namespace ConsoleMouseApp
{
    class Program
    {
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadConsoleInput(IntPtr hConsoleHandle, out INPUT_RECORD lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        
        private const int STD_INPUT_HANDLE = -10;
        private const uint ENABLE_MOUSE_INPUT = 0x0010;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        private const uint ENABLE_EXTENDED_FLAGS = 0x0080;

        
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)] public ushort EventType;
            
            [FieldOffset(4)] public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)] public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
            [FieldOffset(4)] public MENU_EVENT_RECORD MenuEvent;
            [FieldOffset(4)] public FOCUS_EVENT_RECORD FocusEvent;
        }

        

        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition; public uint dwButtonState; public uint dwControlKeyState; public uint dwEventFlags;
        }

        public struct COORD
        {
            public short X; public short Y;
        }

        public struct WINDOW_BUFFER_SIZE_RECORD
        {
            public COORD dwSize;
        }

        public struct MENU_EVENT_RECORD
        {
            public uint dwCommandId;
        }

        public struct FOCUS_EVENT_RECORD
        {
            public bool bSetFocus;
        }

        static void Main(string[] args)
        {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

            
            GetConsoleMode(consoleHandle, out uint consoleMode);

            
            consoleMode |= ENABLE_MOUSE_INPUT;
            consoleMode &= ~ENABLE_QUICK_EDIT_MODE;
            consoleMode |= ENABLE_EXTENDED_FLAGS;

            SetConsoleMode(consoleHandle, consoleMode);

            Console.WriteLine("Ожидание кликов мыши...");
            int kill_click = 0;

            while (true)
            {
                
                ReadConsoleInput(consoleHandle, out INPUT_RECORD record, 1, out uint eventsRead);

                
                if (record.EventType == 0x0002)
                {
                    uint buttonState = record.MouseEvent.dwButtonState;
                    int x = record.MouseEvent.dwMousePosition.X;
                    int y = record.MouseEvent.dwMousePosition.Y;
                    uint eventFlags = record.MouseEvent.dwEventFlags;

                    
                    if (buttonState == 0x0001)
                    {
                        kill_click = kill_click + 1;
                        Console.WriteLine(kill_click);
                    }
                    
                    else if (buttonState == 0x0002)
                    {
                        kill_click = kill_click + 1;
                        Console.WriteLine(kill_click);
                    }
                }
                
                
                if (Console.KeyAvailable)
                {
                    break;
                }
            }
        }
    }
}
