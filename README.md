# VMC Blendshape Slider

## Info

This is a quick prototype. It sends the value 0-1 of the slider over VMC for the specified blendshape on the specified port.

If you want to build it, I'm using Unity 2019.4.21f1.

I'm using https://github.com/hecomi/uOSC/releases to send, idk how to properly package stuff in Unity but I don't think you would need to install it yourself.

Let me know if you use it, if you have an idea for a feature or if you find a bug! I can't easily test a Mac or Linux build but let me know if you'd find one useful.

## How to use

To properly combine this with VSeeFace tracking:

- Settings > General Settings
- Enable 'OSC/VMC Receiver (disables tracking)'
- Enable 'Apply VSeeFace tracking' below that and make sure everything is enabled
- Enable 'Secondary OSC/VMC protocol receiver'. This is where we send the slider value, so the port number should correspond to the one in the slider app.
- Enable 'Apply Blendshapes' beneath that.

## Version History

v0.2 - Settings are now saved on exit.

v0.1 - Initial Release.

## Contact

https://y2kcyb.org
https://twitter.com/y2k_cyborg
https://twitch.tv/millennium_cyborg