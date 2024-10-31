# Nisualizer
This project aims to bring visualizers, such as [GLava](https://github.com/jarcode-foss/glava) and [Cava](https://github.com/karlstav/cava), to the next level. <br/>
It acts as both a visualizer and a background, eliminating the need for any additional apps. <br/>
A simple example can be found [here](https://www.youtube.com/watch?v=4B2wmQrPQB0).

# Usage
Nisualizer requires a little setup in order to work, it should take no more than 5 minutes.

## Requirements
1. Your desktop environment has to support a window wrapper, such as:
   - [hyprwinwrap](https://hyprland.org/plugins/hyprwinwrap/) - Hyprland
   - [xwinwrap](https://github.com/mmhobi7/xwinwrap/) - most X desktop environments <br/>
2. Install and configure [pipewire](https://pipewire.org/) with [pulseaudio](https://www.freedesktop.org/wiki/Software/PulseAudio/), additionally install [qpwgraph](https://github.com/rncbc/qpwgraph) for visualization.

## Setup
1. Configure the window wrapper of your choice, e.g. hyprwinwrap:
    - Follow the official [plugins guide](https://wiki.hyprland.org/Plugins/Using-Plugins/) for Hyprland
    - Add the following to your Hyprland config:
        ```
        plugin {
            hyprwinwrap {
                class = Nisualizer.x86_64
            }
        }
        ```
   
2. Create a virtual input device that captures all the output data(yes, a microphone capturing your system audio), following example is a fish function that can easily be translated to bash(thanks gpt btw):
    ```fish
    function output_input
        # Create a new virtual source that other apps can use as input
        set SOURCE_NAME "OutputInput"
    
        # Create a null sink to serve as the mixing point
        pactl load-module module-null-sink sink_name="$SOURCE_NAME"_mix sink_properties=device.    description="$SOURCE_NAME"_mix
    
        # Create a virtual source that monitors the null sink, making the mixed audio available as input
        pactl load-module module-virtual-source source_name=$SOURCE_NAME master="$SOURCE_NAME"_mix.monitor     source_properties=device.description="OutputInput"
    
        # Get the name of the current default output sink
        set DEFAULT_SINK (pactl info | grep "Default Sink" | awk '{print $3}')
    
        # Create a loopback from the default sink's monitor to our null sink
        if test -n "$DEFAULT_SINK"
            pactl load-module module-loopback source="$DEFAULT_SINK.monitor" sink="$SOURCE_NAME"_mix latency_msec=1
        else
            echo "Error: Could not find default output sink."
        end
    end
    ```
    
3. Reload your fish config, usually done by simply restarting the terminal, and run the `output_input` command. </br>
*If all your output devices are present before starting you de(e.g. Hyprland), you can run this command on startup, e.g. `exec-once = output_input` for Hyprland.*
4. Start the visualizer. This can be done by either running the app directly, or starting it with your de, e.g. Hyprland:
    ```
    exec-once = Path/To/Nisualizer.x86_64
    ```

# FAQ
1. Does it work on Windows?
    - If you find a way to send Nisualizer to the background layer(good luck), and pass the system audio to a virtual device called output_input(should be possible w external apps), sure, it will work, again, good luck.
2. How's the performance?
    - Nisualizer itself should perform just fine on most systems. As for the resource usage, it depends, shouldn't be too bad, but my focus isn't on that, I just wanna create a good looking background.
3. Can I fork this project and make my own visualizer?
    - Absolutely, I am really interested to see what the community is capable of creating, after all, there are much more skilled artists and developers than me out there.

# How it works
Nisualizer works by capturing the audio data from a virtual input device called output_input, created in [Setup](#setup), and processing it in various ways to create interesting effects.
