using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Birth_First
{
    public class User32
    {
        /// <summary>
        /// マウス関連のメソッドをまとめたクラス
        /// </summary>
        public class Mouse
        {
            [DllImport("User32.dll")]
            private static extern bool SetCursorPos(int X, int Y);

            /// <summary>
            /// マウスカーソルの位置を設定します。
            /// </summary>
            /// <param name="a">X 座標を指定します。</param>
            /// <param name="b">Y 座標を指定します。</param>
            public static void SetPosition(int a, int b)
            {
                SetCursorPos(a, b);
            }
        }
        public class Key
        {
            [DllImport("User32.dll")]
            private static extern short GetAsyncKeyState(int nVirtKey);
            [DllImport("User32.dll")]
            private static extern short GetKeyboardState(byte[] lpKeyState);

            //            public static bool IsKeyPress(System.Windows.Input.Key keyCode)
            public static bool IsKeyPress(System.Windows.Forms.Keys keyCode)
            {
                //Console.WriteLine(GetAsyncKeyState((int)keyCode));
                //return GetAsyncKeyState((int)keyCode) < 0;
                byte[] data = new byte[256];
                GetKeyboardState(data);

                return (data['A'] & 0x80) != 0;

            }
        }

    }
}
