# README

# XR-Stereoscopic-Viewer

## Introduction

A VisionOS style viewer of Spatial Video and Spatial image in XR devices.

Bring the experience of the Photos App in Apple Vision Pro to the META Quest series headset, with support for Spatial Video and Spatial image. This Project aims to showcase the stereoscopic imaging solution of Apple, allowing us to immerse in its charm in advance.

[image]([https://github.com/Eis4TY/XR-Stereoscopic-Viewer/blob/main/ProjectTrailer/video_thumbnail.png](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/blob/main/ProjectTrailer/video_thumbnail.png))

## Features

- Local spatial Photo / Video view
- IPD adjustment
- MR Seethrough

❓ if you got any question ,Post it [here!](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/10)

## Requirements

- Meta Quest 2 / Pro / 3
- Enable developer mode (you can follow these instructions).
- Device system version V56 or later is recommended

## Download

- Download Quest version [APK](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/releases).
- Download PICO version APK. (coming soon)

## Resource Sharing

- **🧰** [Stereoscopic Toolbox](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/8)
- **📺** [3D media sharing](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/9)

## Import Files

- Put your 3D media files into the "sdcard/Pictures/3dMedia" folder.(If you can’t see this folder, please open the app once, then close it and try again)
- The program currently supports `.jpg`, `.png`, and `.mp4` formats.

## 导入文件 (ZH)

- 将`.jpg`和`.png`和`.mp4`格式的 3D媒体 文件夹放入 设备主储存空间 “/Pictures/3dMedia” 文件夹中 (如果你看不到这个文件夹，请先打开一次 APP，然后关闭再试)。
- 只有无边框的 SBS 左右格式的 3D 媒体可以正常显示

## How to build?

Get the XR-Stereoscopic-Viewer open-source application running on your own devices.

1. Clone this project.
2. Open the project with Unity 2022.3.9f1 (Android Build).
3. From the **Hierarchy** tab, find Canvas > Gallery. and in the **Inspector** tab, find ImageLoader script, Check the box of ”ReadyToBuild”.
4. Navigate to **File > Build Settings...**, select the **Android** platform, then select your Meta Quest Pro as the **Run device** (if it's plugged in) and then click on **Build and Run**.

## Acknowledgements

- Thanks to [@HW君](https://space.bilibili.com/40043075?spm_id_from=333.337.0.0) for the blowing out candles video.
- Thanks to @Mr.Maginary for the stereoscopic photos.
- Thank to [@jetstyle](https://github.com/jetstyle) for the visionOS UI https://github.com/jetstyle/Apple-Vision-Pro-UI-Kit