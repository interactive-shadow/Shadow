/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// Adjustments to new Publication Timing and Execution Framework 
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;
using System.Collections;

namespace RosSharp.RosBridgeClient
{
    public class ImagePublish : Publisher<MessageTypes.Sensor.Image>
    {
        public string FrameId = "Camera";
        public int resolutionWidth = 640;
        public int resolutionHeight = 480;

        public bool canPublish = false;
        public float publishDuration = 1f;

        //private MessageTypes.Sensor.CompressedImage message;
        private MessageTypes.Sensor.Image message;

        private RsStreamTextureRendererAndConverter converter;

        protected override void Start()
        {
            base.Start();
            InitializeMessage();

            converter = GetComponent<RsStreamTextureRendererAndConverter>();
            StartCoroutine(TexturePublish());
        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Sensor.Image();
            message.header.frame_id = FrameId;
            message.height = (uint)resolutionHeight;
            message.width = (uint)resolutionWidth;
        }

        IEnumerator TexturePublish()
        {
            if (canPublish)
            {
                message.header.Update();

                Texture2D texture = converter.GetTexture();//ここにカメラのデータがそのまま    
                byte[] data = new byte[message.width * message.height];

                //ここにもろもろの処理を

                //ShadowDetectManager.Instance.AddPosition(shadowPos);
                message.data = data;
                yield return new WaitForSeconds(publishDuration);
            }
            else
            {
                yield return null;
            }
        }
    }
}
