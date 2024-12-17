using System;
using System.Runtime.InteropServices;

namespace Audio
{
    public class PulseAudioInterop
    {
        [DllImport("libpulse.so.0")]
        public static extern IntPtr pa_mainloop_new();

        [DllImport("libpulse.so.0")]
        public static extern IntPtr pa_mainloop_get_api(IntPtr mainloop);

        [DllImport("libpulse.so.0")]
        public static extern IntPtr pa_context_new(IntPtr mainloopApi, string name);

        [DllImport("libpulse.so.0")]
        public static extern int pa_context_connect(IntPtr context, string server, uint flags, IntPtr api);

        [DllImport("libpulse.so.0")]
        public static extern void pa_context_get_server_info(IntPtr context, ServerInfoCallback callback, IntPtr userdata);

        [DllImport("libpulse.so.0")]
        public static extern IntPtr pa_stream_new(IntPtr context, string name, ref PaSampleSpec spec, IntPtr channelMap);

        [DllImport("libpulse.so.0")]
        public static extern void pa_stream_set_read_callback(IntPtr stream, StreamReadCallback callback, IntPtr userdata);

        [DllImport("libpulse.so.0")]
        public static extern int pa_stream_connect_record(IntPtr stream, string dev, IntPtr bufferAttr, uint flags);

        [DllImport("libpulse.so.0")]
        public static extern int pa_stream_peek(IntPtr stream, out IntPtr data, out int nbytes);

        [DllImport("libpulse.so.0")]
        public static extern void pa_stream_drop(IntPtr stream);

        [StructLayout(LayoutKind.Sequential)]
        public struct PaSampleSpec
        {
            public int format;
            public int rate;
            public int channels;
        }

        // Callbacks
        public delegate void ServerInfoCallback(IntPtr context, IntPtr serverInfo, IntPtr userdata);

        public delegate void StreamReadCallback(IntPtr stream, IntPtr nbytes, IntPtr userdata);
    }
}