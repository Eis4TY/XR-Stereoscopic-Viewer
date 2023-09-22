/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;

namespace Meta.WitAi.Data.Info
{
    [Serializable]
    public struct WitCharacterInfo
    {
        public string name;
        public string story;
        public WitVoiceConfig voiceConfig;

    }

    [Serializable]
    public struct WitVoiceConfig
    {
        public string voice;  // eg "Carl"
        public string style; //eg "formal"
        public int pitch;  // from 25-400
        public int speed;  // from 10 - 400
        public string character_sfx; // eg "monster"  //TODO: convert to enum
        public string environment_sfx;//  eg "room"  //TODO: Convert to enum
    }
}
