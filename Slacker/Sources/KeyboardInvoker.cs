using System;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Slacker.Sources
{
    public class KeyboardInvoker
    {
        private uint KeyScanCode;
        private bool FullKeyPress;

        public KeyboardInvoker(bool fullKeyPress, Key keyPressed)
        {
            FullKeyPress = fullKeyPress;

            var convVirtualKey = KeyInterop.VirtualKeyFromKey(keyPressed);
            KeyScanCode = MapVirtualKeyA((uint)convVirtualKey, (uint)MapType.MAPVK_VK_TO_VSC);
        }


        public void SendInputWithAPI()
        {
            INPUT[] Inputs = new INPUT[1];
            INPUT Input = new INPUT();

            Input.type = (int)InputType.Keyboard;
            Input.U.ki.wScan = (short)KeyScanCode;
            Input.U.ki.dwFlags = FullKeyPress ? KEYEVENTF.SCANCODE : KEYEVENTF.KEYUP;
            
            Inputs[0] = Input;
            SendInput(1, Inputs, INPUT.Size);
        }

        /// <summary>
        /// Declaration of external MapVirtualKeyA method
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint MapVirtualKeyA(
            uint uCode,
            uint uMapType);

        // Mappping type for MapVirtualKeyA
        internal enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x00,
            MAPVK_VSC_TO_VK = 0x01,
            MAPVK_VSC_TO_VK_EX = 0x03,
            MAPVK_VK_TO_CHAR = 0x02
        }

        /// <summary>
        /// Declaration of external SendInput method
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern uint SendInput(
            uint nInputs,
            [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
            int cbSize);


        // Declare the INPUT struct
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            internal uint type;
            internal InputUnion U;
            internal static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        // Declare the InputUnion struct
        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal MouseEventDataXButtons mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }

        [Flags]
        internal enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        internal enum MouseEventDataXButtons : uint
        {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [Flags]
        internal enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal short wVk;
            internal short wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }

        [Flags]
        internal enum KEYEVENTF : uint
        {
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            SCANCODE = 0x0008,
            UNICODE = 0x0004
        }

        /// <summary>
        /// Define HARDWAREINPUT struct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }
    }
}
