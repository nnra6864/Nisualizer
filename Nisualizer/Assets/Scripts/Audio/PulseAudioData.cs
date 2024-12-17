using System;
using System.Runtime.InteropServices;

namespace Audio
{
    public class PulseAudioData : AudioData
    {
        public override void Init()
        {
            // Initialize the main loop and api
            var mainLoop = PulseAudioInterop.pa_mainloop_new();
            var api = PulseAudioInterop.pa_mainloop_get_api(mainLoop);

            // Create the context and connect to pa
            var context = PulseAudioInterop.pa_context_new(api, "Nisualizer");
            PulseAudioInterop.pa_context_connect(context, null, 0, IntPtr.Zero);
            
            PulseAudioInterop.pa_context_get_server_info(context, ServerInfoCallback, IntPtr.Zero);
        }

        private void ServerInfoCallback(IntPtr context, IntPtr serverInfo, IntPtr userdata)
        {
            // Assuming serverInfo is parsed to get default_sink_name
            var defaultSinkName = "alsa_output.pci-0000_00_1b.0.analog-stereo.monitor"; // Example name

            var spec = new PulseAudioInterop.PaSampleSpec
            {
                format   = 3, // PA_SAMPLE_S16LE
                rate     = 44100,
                channels = 1
            };

            IntPtr stream = PulseAudioInterop.pa_stream_new(context, "output monitor", ref spec, IntPtr.Zero);

            PulseAudioInterop.pa_stream_set_read_callback(stream, StreamReadCallback, IntPtr.Zero);
            PulseAudioInterop.pa_stream_connect_record(stream, defaultSinkName, IntPtr.Zero, 0);
        }

        private void StreamReadCallback(IntPtr stream, IntPtr data, IntPtr userdata)
        {
            // Process PCM data
            PulseAudioInterop.pa_stream_peek(stream, out var pcmData, out var nbytes);

            if (pcmData != IntPtr.Zero && nbytes > 0)
            {
                byte[] buffer = new byte[nbytes];
                Marshal.Copy(pcmData, buffer, 0, nbytes);

                Console.WriteLine($"Read {nbytes} bytes of PCM data");
            }

            PulseAudioInterop.pa_stream_drop(stream);
        }
    }
}