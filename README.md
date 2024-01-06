# Mystic-Tools
WIP
* Program to control RGB LEDs on msi vigor gk50 low profile tkl jp keyboard.
* LED profiles for each active program, video playback, etc.
  
# Cautions
I am not responsible if your keyboard stops working with this program! **At your own risk!!**
* If msi center is installed, it may not work properly. Please remove the dll after obtaining it.
* Playing videos may load the keyboard and cause it to freeze. If this happens, please unplug the USB.

  
# Requirements
* MsiHid_x64.dll : You can find it somewhere in the directory where the msi center executable binary resides. Or you may be able to find it on the net...
* badapple.mp4 : Download from Youtube in the way you prefer.
* VLC : It is only called in `Process.Start`. If you just want audio, comment it and out `player.Play();`. (KeyboardManager.cs)
* Libraries available at Nuget (check csproj)

