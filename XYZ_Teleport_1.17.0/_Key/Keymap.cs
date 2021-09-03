using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace XYZ_Teleport_1._17._0._Keymap
{
    class Keymap
    {
        [DllImport("user32.dll")] static extern bool GetAsyncKeyState(char v);
        [DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();

        public static bool isMinecraftFocused()
        {
            StringBuilder sb = new StringBuilder("Minecraft".Length + 1);
            GetWindowText(GetForegroundWindow(), sb, "Minecraft".Length + 1);
            return sb.ToString().CompareTo("Minecraft") == 0;
        }

        public static Keymap handle;
        public static EventHandler<KeyEvent> keyDown;
        public static EventHandler<KeyEvent> keyHeld;
        public static EventHandler<KeyEvent> keyUp;

        Dictionary<char, uint> dBuff = new Dictionary<char, uint>();
        Dictionary<char, bool> noKey = new Dictionary<char, bool>();

        Dictionary<char, uint> rBuff = new Dictionary<char, uint>();
        Dictionary<char, bool> yesKey = new Dictionary<char, bool>();

        public Keymap()
        {
            handle = this;
            for (char c = (char)0; c < 0xFF; c++)
            {
                dBuff.Add(c, 0);
                rBuff.Add(c, 0);
                noKey.Add(c, true);
                yesKey.Add(c, true);
            }

            new Thread(() => {
                try
                {
                    if (isMinecraftFocused())
                    {
                        for (char c = (char)0; c < 0xFF; c++)
                        {
                            noKey[c] = true;
                            yesKey[c] = false;
                            if (GetAsyncKeyState(c))
                            {
                                if (keyHeld != null)
                                    keyHeld.Invoke(this, new KeyEvent(c));
                                noKey[c] = false;
                                if (dBuff[c] > 0)
                                    continue;
                                dBuff[c]++;
                                try
                                {
                                    if (keyDown != null)
                                        keyDown.Invoke(this, new KeyEvent(c));
                                }
                                catch { }
                            }
                            else
                            {
                                yesKey[c] = true;
                                if (rBuff[c] > 0)
                                    continue;
                                rBuff[c]++;
                                if (keyUp != null)
                                {
                                    try
                                    {
                                        keyUp.Invoke(this, new KeyEvent(c));
                                    }
                                    catch { }
                                }
                            }
                            if (noKey[c])
                                dBuff[c] = 0;
                            if (!yesKey[c])
                                rBuff[c] = 0;
                        }
                    }
                }
                catch { }
            }).Start();
        }
    }
    public class KeyEvent : EventArgs // flare's key events
    {
        public char key;
        public KeyEvent(char v) => key = v;
    }
}
