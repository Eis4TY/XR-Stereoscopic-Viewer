# README

# XR-Stereoscopic-Viewer

## Introduction

A VisionOS style viewer of Spatial Video and Spatial image in XR devices.

Bring the experience of the Photos App in Apple Vision Pro to the META Quest series headset, with support for Spatial Video and Spatial image. This Project aims to showcase the stereoscopic imaging solution of Apple, allowing us to immerse in its charm in advance.

[[Watch the video]([/ProjectTrailer/video_thumbnail.png](/ProjectTrailer/video_thumbnail.png))]([https://github.com/Eis4TY/XR-Stereoscopic-Viewer/blob/main/ProjectTrailer/Trailer.mp4](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/blob/main/ProjectTrailer/Trailer.mp4))

## Features

- Local spatial Photo / Video view
- IPD adjustment
- MR Seethrough

❓ if you got any question ,Post it [here!](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/10)

## Requirements

- Meta Quest 2 / Pro / 3
- Enable developer mode (you can follow these instructions).
- Device system version V56 or later is recommended

## Resource sharing

- **🧰** [Stereoscopic Toolbox](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/8)
- **📺** [3D media sharing](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/discussions/9)


## Getting Started

1. Download the [APK](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/releases).
2. Connect Quest to computer, use SideQuest or other ADB tools to install it.
3. Download [‘Media’](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/releases/tag/MediaFile) file and Unzip folder to the ‘Media’ folder.
4. "Place the ‘Media’ folder into the ‘\Android\data\com.Eis4TY.XRStereoscopicViewer\files’ directory. (If you can’t see this folder, please open the app once, then close it and try again)
5. Alternatively, you can create a new folder named ‘Media’ here and then add your stereoscopic photos and videos to the folder. The program currently supports `.jpg`, `.png`, and `.mp4` formats”.
6. To use the extrusion depth feature, the black and white depth map must be named '[Original Image Name]_D' and placed in the 'Media' folder as well. The program will automatically recognize it. Photos with a depth map will display the extrusion depth controls in the 'Settings' UI.
7. Open the app and enjoy. (The program is not yet perfect, so you may encounter some bugs)

## Getting Started (ZH)

1. 下载 [APP](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/releases)。
2. 将头显连接到计算机并运行访问文件，使用SideQuest或其他ADB工具来安装 APP。
3. 下载 [‘Media’](https://github.com/Eis4TY/XR-Stereoscopic-Viewer/releases/tag/MediaFile) 文件并解压到 ‘Media’ 文件夹。
4. 将 ‘Media’ 文件夹放入 “\Android\data\com.Eis4TY.XRStereoscopicViewer\files” 文件夹中 (如果你看不到这个文件夹，请先打开一次 APP，然后关闭再试)。
5. 或在此处新建一个名为 “Media” 的文件夹，然后将你的立体照片和立体视频放入文件夹，程序目前支持`.jpg`和`.png`和`.mp4`格式
6. 使用挤出深度功能需要将黑白深度图命名为 “原图名字_D”，同样放入 “Media” 文件夹，程序会自动识别。带有深度图的照片会「Settings」中显示挤出深度控件。
7. 打开 APP 并欣赏。(目前程序还不够完善，可能会遇到一些 bug)

## How to build?

Get the XR-Stereoscopic-Viewer open-source application running on your own devices.

1. Clone this project.
2. Open the project with Unity 2022.3.9f1 (Android Build).
3. Navigate to **File > Build Settings...**, select the **Android** platform, then select your Meta Quest Pro as the **Run device** (if it's plugged in) and then click on **Build and Run**.

## Acknowledgements

- Thanks to [@HW君](https://space.bilibili.com/40043075?spm_id_from=333.337.0.0) for the blowing out candles video.
- Thanks to @Mr.Maginary for the stereoscopic photos.
- Thank to [@jetstyle](https://github.com/jetstyle) for the visionOS UI https://github.com/jetstyle/Apple-Vision-Pro-UI-Kit