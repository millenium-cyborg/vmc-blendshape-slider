using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(uOSC.uOscClient))]
public class App : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Text info;
    public UnityEngine.UI.InputField port_input;
    public UnityEngine.UI.InputField blendshape_input;

    uOSC.uOscClient client;

    // Use this for initialization
    void Start()
    {
        client = GetComponent<uOSC.uOscClient>();
    }

    // Update is called once per frame
    void Update()
    {
        int port_maybe = 0;
        if (int.TryParse(port_input.text, out port_maybe)) {
          client.port = port_maybe;
        }
        info.text = "Sending value to blendshape '" + blendshape_input.text + "' on port " + client.port;
        // TODO: I think I may want to send as a bundle with immediate timestamp?
        // var root = new Bundle(Timestamp.Immediate);
        // root.Add(new Message("/VMC/Ext/etc"));
        // root.Add(...);
        // client.Send(root);
        /*
        Frame Period

        V2.3
        /VMC/Ext/Set/Period
        (int){Status}
        (int){Root}
        (int){Bone}
        (int){BlendShape}
        (int){Camera}
        (int){Devices}

        Set the data transmission interval from virtual motion capture.
        Sent at 1 / x Frame intervals.
        */
        // Not 100% sure on this one tbh but I think it may help apply
        // blendshapes immediately.
        client.Send("/VMC/Ext/Set/Period",
            1, 1, 1, 1, 1, 1);

        /*
        VRM BlendShapeProxyValue

        V2.3
        /VMC/Ext/Blend/Val
        (string){name}
        (float){ value}
        
        /VMC/Ext/Blend/Apply

        BlendShapeProxy value
        */
        client.Send("/VMC/Ext/Blend/Val", blendshape_input.text, slider.value);
        client.Send("/VMC/Ext/Blend/Apply");
    }
}

