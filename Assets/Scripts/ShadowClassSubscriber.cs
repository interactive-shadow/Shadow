using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class ShadowClassSubscriber : Subscriber<MessageTypes.Std.String>
    {
        protected override void ReceiveMessage(MessageTypes.Std.String message)
        {
            string shadowClass = message.data;

            ShadowDetectManager.Instance.GenerateShadow(shadowClass);
        }
    }
}
