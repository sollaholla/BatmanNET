﻿using System;
using System.IO;
using System.Threading;

namespace ScriptCommunicator
{
    public class ScriptCommunicatorHelper
    {
        private readonly string EventName;
        private EventWaitHandle ScriptCommunicatorModMenuHandle;
        private bool ScriptCommunicatorModMenuIsBlocked;

        public ScriptCommunicatorHelper(string EventName)
        {
            this.EventName = EventName;
            MainHandle = new EventWaitHandle(false, EventResetMode.ManualReset, EventName);
            ScriptCommunicatorModMenuHandle =
                new EventWaitHandle(true, EventResetMode.ManualReset, "ScriptCommunicator");
        }

        private EventWaitHandle MainHandle { get; }

        public void Init(string title, string description)
        {
            var path = ".\\scripts\\" + EventName + ".scmod";
            var text = title + Environment.NewLine + description;
            if (File.Exists(path)) text = File.ReadAllText(path);
            File.WriteAllText(path, title + Environment.NewLine + description);
        }

        public bool IsEventTriggered()
        {
            return MainHandle.WaitOne(0);
        }

        public void TriggerEvent()
        {
            MainHandle.Set();
        }

        public void ResetEvent()
        {
            MainHandle.Reset();
        }

        public void BlockScriptCommunicatorModMenu()
        {
            if (!ScriptCommunicatorModMenuIsBlocked)
            {
                var waitHandleExists =
                    EventWaitHandle.TryOpenExisting("ScriptCommunicator", out ScriptCommunicatorModMenuHandle);
                if (waitHandleExists)
                {
                    ScriptCommunicatorModMenuHandle.Reset();
                    ScriptCommunicatorModMenuIsBlocked = true;
                }
            }
            else
            {
                ScriptCommunicatorModMenuHandle.Reset();
            }
        }

        public void UnblockScriptCommunicatorModMenu()
        {
            if (ScriptCommunicatorMenuIsBlocked() && ScriptCommunicatorModMenuIsBlocked)
            {
                ScriptCommunicatorModMenuHandle.Set();
                ScriptCommunicatorModMenuIsBlocked = false;
            }
        }

        public bool ScriptCommunicatorMenuIsBlocked()
        {
            return !ScriptCommunicatorModMenuHandle.WaitOne(0);
        }

        public bool ScriptCommunicatorMenuDllExists()
        {
            return File.Exists(@"scripts\ScriptCommunicator.dll");
        }
    }
}