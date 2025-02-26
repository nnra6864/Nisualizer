<p align="center">
    <img src="/Media/Nisualizer.png" alt="Nisualizer" width=512px height=512px>
</p>

# Nisualizer
This project aims to combine music visualizers, such as [GLava](https://github.com/jarcode-foss/glava) and [Cava](https://github.com/karlstav/cava), with wallpaper engines, such as [Wallpaper Engine](https://www.wallpaperengine.io/en) and [Lively](https://www.rocksdanister.com/lively/), and bring them to the next level. <br/>
A simple example can be found [here](https://youtu.be/V6RLddVE4zE).<br/>

# Showcase
<details>
    <summary><h2>Snowstorm</h2></summary>
    <img src="/Media/Snowstorm.png" alt="Snowstorm">
    <img src="/Media/SimpleSnowstorm.png" alt="Simple Snowstorm">
</details>
<details>
    <summary><h2>Chaotic Cubes</h2></summary>
    <img src="/Media/NeonDust.png" alt="Neon Dust">
    <img src="/Media/IcyCubes.png" alt="Icy Cubes">
</details>

# Features
- Lightweight
- Simple and Powerful JSON Config
- Live Config Reload
- System Fonts
- Digital Clock

# Roadmap
- [ ] Launch on startup
- [x] Arguments(e.g. using different configs for 2 displays)
- [x] Change the config directory for Windows(and their shit system structure)
- [ ] Implement system font support on Windows
- [x] Polish sending window to the background layer on windows
- [x] Implement configurable paths for the file system watcher used in live config reload
- [ ] Config volume profile
- [ ] Slideshow
- [ ] Build more commands into the app itself(e.g. wttr.in, now playing etc.)
- [ ] Completely modularize components such as vfx graphs etc.
- [ ] JSON variables(to reduce code duplication)
- [ ] Font materials
- [x] Picking up system audio on Windows
- [x] Picking up system audio on Linux
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
- [x] Dynamic Text(Weather, DateTime, FPS, Shell etc.)
- [x] Modular UI
- [x] Media Info(via [Player CTL](https://github.com/altdesktop/playerctl))
- [x] Sourcing other files in config

# Usage
Nisualizer is ready to use out of the box with very little setup, and the option of cloning/forking and making your own scenes.</br>
Thanks to the modularity of components, creating your own backgrounds should be relatively easy, more can be read in [Extending Nisualizer](#Extending-Nisualizer).

## Requirements
### Linux Requirements
1. Window Wrapper, to send Nisualizer to the background layer, such as:
   - [hyprwinwrap](https://hyprland.org/plugins/hyprwinwrap/) - Hyprland
   - [xwinwrap](https://github.com/mmhobi7/xwinwrap/) - most X desktop environments <br/>
2. [pipewire-pulse](https://docs.pipewire.org/page_man_pipewire-pulse_1.html) or [Pulse Audio](https://www.freedesktop.org/wiki/Software/PulseAudio/), so Nisualizer can capture audio

### Windows Requirements
1. [Lively](https://www.rocksdanister.com/lively/)(optional), an alternative way to send Nisualizer to the background layer

## Setup
### Linux Setup
1. Download Nisualizer
2. Configure the window wrapper of your choice, e.g. hyprwinwrap:
    - Follow the official [plugins guide](https://wiki.hyprland.org/Plugins/Using-Plugins/) for Hyprland
    - Add the following to your Hyprland config:
        ```
        plugin {
            hyprwinwrap {
                class = Nisualizer.x86_64
            }
        }
        ```
3. Launch Nisualizer to generate the default config
4. [Configure Nisualizer](#configuration)
5. Make Nisualizer launch on startup(native implementation is WIP), e.g. with Hyprland:
    ```
    exec-once = Path/To/Nisualizer.x86_64
    ```

### Windows Setup
1. Download Nisualizer
2. Launch Nisualizer to generate the default config
3. [Configure Nisualizer](#configuration)
4. Make Nisualizer [launch on startup](https://answers.microsoft.com/en-us/windows/forum/all/autostart-a-program-in-windows-10/940682ae-8872-47ce-964d-8b1e820d9a5a)(native implementation is WIP)

## Configuration
WIP

# FAQ
1. Do you plan to support Mac?
    - Fuck no, you are free to make a [PR](#extending-nisualizer) tho.
2. How's the performance?
    - It depends on your config. Nisualizer is just a Unity application with only necessary objects in the scene, you add the rest via config. It shouldn't be any more demanding than [WPE](https://www.wallpaperengine.io/en) whilst achieving way more.
3. Can I fork this project and make my own visualizer?
    - Absolutely, I am really interested to see what the community is capable of creating. After all, there are much more skilled artists and developers than me out there. You are also welcome to make a [PR](#extending-nisualizer) to get your scene merged into the master branch.

# How Nisualizer Works
Nisualizer is just a Unity application that captures system audio and reacts to it.
It supports [pipewire-pulse](https://docs.pipewire.org/page_man_pipewire-pulse_1.html)/[Pulse Audio](https://www.freedesktop.org/wiki/Software/PulseAudio/)(Linux) and [Windows Core Audio](https://learn.microsoft.com/en-us/windows/win32/coreaudio/wasapi)(Windows, most of the code was taken from [NAudio](https://github.com/naudio/NAudio/tree/master/NAudio.Wasapi/CoreAudioApi)).
TODO: Explain how it sends itself to the background layer
Nisualizer utilizes simple JSON configuration with some additional features, such as comments, sourcing other files etc.

# Extending Nisualizer
Nisualizer is built on top of modular components and utilities, allowing anyone to expand it with minimal effort.

## Contributing
If you've made something that would fit into the actual project, feel free to make a pull request and I'll merge it if I like it. Have fun!

### Contributing Instructions
1. Make a fork of Nisualizer
2. Clone the fork recursively `git clone --recurse https://github.com/YourName/Nisualizer`
3. Make changes
4. Create a PR with a detailed description of your contribution

## Creating Scenes
### Automatic(Recommended)
Assets -> Create -> Nisualizer Scene

### Manual(Outdated, painful and prone to changes)
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
