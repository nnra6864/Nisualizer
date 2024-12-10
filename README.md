# Nisualizer
![Screenshot](/Media/Screenshot.png)
This project aims to bring visualizers, such as [GLava](https://github.com/jarcode-foss/glava) and [Cava](https://github.com/karlstav/cava), to the next level. <br/>
It acts as both a visualizer and a background, eliminating the need for any additional apps. <br/>
A simple example can be found [here](https://youtu.be/V6RLddVE4zE).<br/>

# Features

- Audio Interactive Components
- Digital Clock
- Live Config Reload
- System Fonts

# Roadmap

- [x] Music Reactive Particle System
- [x] Music Reactive VFX Graph
- [x] Music Reactive Volume
- [x] Clock, Date and Day
- [x] Background From Local Files
- [x] Live Reload JSON Config
- [x] Custom Fonts
- [x] Multiple Scenes
- [x] Per Scene Config
- [x] Automatic Scene Creation
- [ ] Weather(via [Metro](https://open-meteo.com/))
- [ ] Async Config Reload
- [ ] Modular UI
- [ ] System Info
- [ ] System Usage
- [ ] Media Info(via [Player CTL](https://github.com/altdesktop/playerctl))

# Usage
Nisualizer is ready to use out of the box with very little setup, and the option of cloning/forking and making your own scecnes.</br>
Thanks to the modularity of components, creating your own backgrounds should be relatively easy, more can be read in [Extending Nisualizer](#Extending-Nisualizer).

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
   
2. Create a virtual input device that captures all the output data(yes, a microphone capturing your system audio). Following example is a fish function that can easily be translated to bash(thanks gpt btw):
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
*If all your output devices are present before starting your de(e.g. Hyprland), you can run this command on startup, e.g. `exec-once = output_input` for Hyprland.*
4. Start the visualizer. This can be done by either running the app directly, or starting it with your de, e.g. Hyprland:
    ```
    exec-once = Path/To/Nisualizer.x86_64
    ```

# FAQ
1. Does it work on Windows?
    - Technically yes, try using [Lively](https://github.com/rocksdanister/lively) to send the app to background, not sure about custom input yet
2. How's the performance?
    - Nisualizer itself should perform just fine on most systems. As for the resource usage, it depends, shouldn't be too bad, but my focus isn't on that, I just wanna create a good looking background.
3. Can I fork this project and make my own visualizer?
    - Absolutely, I am really interested to see what the community is capable of creating. After all, there are much more skilled artists and developers than me, out there. You are also welcome to make a pull request to get your background merged into the master branch.

# How it works
Nisualizer works by capturing the audio data from a virtual input device called OutputInput, created in [Setup](#setup), and processing it in various ways to create interesting effects.

# Extending Nisualizer
Nisualizer is built on top of modular components and utilities, allowing anyone to customize it however they wish.

## Contributing
1. Make a fork of Nisualizer
2. Clone the fork recursively `git clone --recursive https://github.com/YourName/Nisualizer`
3. Make changes
4. Create a pull request with detailed description of your contributation

## Creating Scenes

### Automatic(Recommended)
Right click anywhere in the Project window and navigate to Assets -> Create -> Nisualizer Scene

### Manual(Painful)
Adding your own scenes can easily be done by following these steps:
1. Create a new directory under `Assets/Scenes/`
2. Give it an original, descriptive name, e.g. `Snowstorm`
3. Add a new scene with the same name as the directory
4. Create a new script that derives from the `SceneScript` and call it `SceneNameManager`, e.g. `SnowstormManager`
5. Add a new object to your scene, name it `SceneNameManager`, e.g. `SnowstormManager`, and attach the previously created script
6. Create a `SceneNameConfig.json` file in your scene directory, e.g. `SnowstormConfig.json`, this will be your default scene config
7. Make a new script in your scene directory and call it `SceneNameConfigData`, e.g. `SnowstormConfigData`, that inherits from the `ConfigData` class, this class is used as a container to store values from your JSON config
8. Create a new scriptable object menu item by adding the following above the class name `[CreateAssetMenu(fileName = "SceneNameConfigData", menuName = "Config/SceneNameConfigData")]`
9. Create a new instance of this scriptable object
10. Attach all the components to the `ConfigScript` under your `SceneManager`, and make sure that the `ConfigName` value matches your scene name, scene loading won't work otherwise
11. Drag the `GameManager` prefab to your scene so you can start the playmode directly from your scene without any issues
12. Make sure you added your scene to the build settings
13. Utilize powerful scripts built with modularity in mind, such as `InteractiveVFX`, `InteractiveVolume` etc.

## Contributing
If you've made something that would fit into the actual project, feel free to make a pull request and I'll merge it if I like it. Have fun!
