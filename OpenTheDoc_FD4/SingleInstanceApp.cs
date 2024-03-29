using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;

namespace OpenTheDoc
{
    // From http://www.codeproject.com/useritems/SingleInstanceApp.asp
    // From PluginCore.Utilities.SingleInstanceApp

    public delegate void MessageEventHandler(object sender, object message);

    public class SingleInstanceApp
    {
        //win32 translation of APIs, message constants and structures
        private class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            public const short WM_COPYDATA = 74;

            public struct COPYDATASTRUCT
            {
                public int dwData;
                public int cbData;
                public IntPtr lpData;
            }
        }

        //a utility window to communicate between application instances
        private class SIANativeWindow : NativeWindow
        {
            public SIANativeWindow()
            {
                CreateParams cp = new CreateParams();
                cp.Caption = _theInstance._id; //The window title is the same as the Id
                CreateHandle(cp);
            }

            //The window procedure that handles notifications from new application instances
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_COPYDATA)
                {
                    //convert the message LParam to the WM_COPYDATA structure
                    NativeMethods.COPYDATASTRUCT data = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
                    object obj = null;
                    if (data.cbData > 0 && data.lpData != IntPtr.Zero)
                    {
                        //copy the native byte array to a .net byte array
                        byte[] buffer = new byte[data.cbData];
                        Marshal.Copy(data.lpData, buffer, 0, buffer.Length);
                        //deserialize the buffer to a new object
                        obj = Deserialize(buffer);
                    }
                    _theInstance.OnMessage(obj);
                }
                else
                    base.WndProc(ref m);
            }
        }

        //Singleton 
        static SingleInstanceApp _theInstance = new SingleInstanceApp();

        //this is a uniqe id used to identify the application
        string _id;
        //The is a named mutex used to determine if another application instance already exists
        Mutex _instanceCounter;
        //Is this the first instance?
        bool _firstInstance;
        //Utility window for communication between apps
        SIANativeWindow _notifcationWindow;

        private void Dispose()
        {
            //release the mutex handle
            if (_instanceCounter != null) 
                _instanceCounter.Close();
            //and destroy the window
            if (_notifcationWindow != null)
                _notifcationWindow.DestroyHandle();
        }

        private void Init()
        {
            _notifcationWindow = new SIANativeWindow();
        }

        //returns a uniqe Id representing the application. This is basically the name of the .exe 
        private static string GetAppId()
        {
            string fileName = Path.GetFileName(Application.ExecutablePath);
            string justName = fileName.Split('.')[0];
            return justName;
        }

        //notify event handler
        private void OnMessage(object message)
        {
            if (Message != null)
                Message(this, message);
        }

        private SingleInstanceApp()
        {
            _id = "OTD_" + GetAppId();
           
            //string identity = "Everyone";
            string identity = Environment.UserDomainName + "\\" + Environment.UserName;

            MutexAccessRule rule = new MutexAccessRule(identity, MutexRights.FullControl, AccessControlType.Allow);

            MutexSecurity security = new MutexSecurity();
            try
            {
                security.AddAccessRule(rule);
                _instanceCounter = new Mutex(false, _id, out _firstInstance, security);
            }
            catch
            {
                _firstInstance = true;
            }
        }

        private bool Exists
        {
            get
            {
                return !_firstInstance;
            }
        }

        //send a notification to the already existing instance that a new instance was started
        private bool NotifyPreviousInstance(object message)
        {
            //First, find the window of the previous instance
            IntPtr handle = NativeMethods.FindWindow(null, _id);
            if (handle != IntPtr.Zero)
            {
                //create a GCHandle to hold the serialized object. 
                GCHandle bufferHandle = new GCHandle();
                try
                {
                    byte[] buffer;
                    NativeMethods.COPYDATASTRUCT data = new NativeMethods.COPYDATASTRUCT();
                    if (message != null)
                    {
                        //serialize the object into a byte array
                        buffer = Serialize(message);
                        //pin the byte array in memory
                        bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                        data.dwData = 0;
                        data.cbData = buffer.Length;
                        //get the address of the pinned buffer
                        data.lpData = bufferHandle.AddrOfPinnedObject();
                    }

                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try
                    {
                        NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, dataHandle.AddrOfPinnedObject());
                        return true;
                    }
                    finally
                    {
                        dataHandle.Free();
                    }
                }
                finally
                {
                    if (bufferHandle.IsAllocated)
                        bufferHandle.Free();
                }
            }
            return false;
        }

        //2 utility methods for object serialization\deserialization
        private static object Deserialize(byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        private static byte[] Serialize(Object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static bool AlreadyExists
        {
            get
            {
                return _theInstance.Exists;
            }
        }

        public static bool NotifyExistingInstance(object message)
        {
            if (_theInstance.Exists)
            {
                return _theInstance.NotifyPreviousInstance(message);
            }
            return false;
        }

        public static bool NotifyExistingInstance()
        {
            return NotifyExistingInstance(null);
        }

        public static void Initialize()
        {
            _theInstance.Init();
        }

        public static void Close()
        {
            _theInstance.Dispose();
        }

        public static event MessageEventHandler Message;
    }
}
