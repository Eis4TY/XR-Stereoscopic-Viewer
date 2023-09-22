/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using UnityEngine;

internal static class OVREditorUtils
{
    static OVREditorUtils()
    {
        OVRGUIContent.RegisterContentPath(OVRGUIContent.Source.GenericIcons, "Icons");

    }


    public static OVRGUIContent CreateContent(string name, OVRGUIContent.Source source, string tooltip = null)
    {
        return new OVRGUIContent(name, source, tooltip);
    }

    public static bool IsMainEditor()
    {
        // Early Return when the process service is not the Editor itself
#if UNITY_2021_1_OR_NEWER
        return (uint)UnityEditor.MPE.ProcessService.level != (uint)UnityEditor.MPE.ProcessLevel.Secondary;
#else
        return (uint)UnityEditor.MPE.ProcessService.level != (uint)UnityEditor.MPE.ProcessLevel.Slave;
#endif
    }
}
