using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;

[RequireComponent(typeof(uOSC.uOscClient))]
public class App : MonoBehaviour
{
    // Public instance references
    public UnityEngine.UI.Slider slider;
    public UnityEngine.UI.Text info;
    public UnityEngine.UI.InputField port_input;
    public UnityEngine.UI.InputField blendshape_input;

    // Internal types
    [Serializable]
    public struct Channel
    {
        public string name;
        public float value;
        // public float min;
        // public float max;
    }

    [Serializable]
    public struct Config
    {
        // public string addr;
        public int port;
        public Channel[] channels;
    }

    // Private implementation
    private uOSC.uOscClient client;
    private Config config;
    private string save_path;
    private Timer save_timer;

    // Initialization
    void Start()
    {
        client = GetComponent<uOSC.uOscClient>();

        save_path = Path.Combine(Application.persistentDataPath, "config.json");

        if (!TryLoadConfig(ref config)) {
            config = CreateDefaultConfig();
        }

        UpdateUIFromConfig();

        save_timer = new Timer(15000);
        save_timer.Elapsed += OnSaveTimerElapsed;
        save_timer.AutoReset = true;
        save_timer.Enabled = true;
    }
    
    static Config CreateDefaultConfig() {
        Config new_config = new Config();
        new_config.port = 39540;
        new_config.channels = new Channel[1];
        new_config.channels[0].name = "JOY";
        new_config.channels[0].value = 0;
        return new_config;
    }

    bool TryLoadConfig(ref Config c) {
        try {
            string jsonString = null;
            using (StreamReader reader = new StreamReader(save_path)) {
                jsonString = reader.ReadToEnd();
            }
            Config loaded = JsonUtility.FromJson<Config>(jsonString);
            c = loaded;
            Debug.Log("Config loaded.");
            return true;
        } catch (Exception e) {
            Debug.LogWarning("Couldn't load config file: " + e.Message);
            return false;
        }
    }
    
    void WriteConfig(Config config) {
        string jsonString = JsonUtility.ToJson(config);
        using (StreamWriter writer = new StreamWriter(save_path)) {
            writer.Write(jsonString);
        }
    }

    private void OnSaveTimerElapsed(System.Object obj, ElapsedEventArgs e) {
        WriteConfig(config);
    }

    void UpdateConfigFromUI() {
        int port = 0;
        if (int.TryParse(port_input.text, out port)) {
            config.port = port;
        }

        config.channels[0].name = blendshape_input.text;
        config.channels[0].value = slider.value;
    }

    void UpdateUIFromConfig() {
        info.text = "Sending value to blendshape '" + config.channels[0].name + "' on port " + config.port;
        blendshape_input.text = config.channels[0].name;
        slider.value = config.channels[0].value;
        port_input.text = config.port.ToString();
    }

    void VMCSend() {
        client.port = config.port;

        /* Google Translation of documentation:
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

        /* Google Translation of documentation:
        VRM BlendShapeProxyValue

        V2.3
        /VMC/Ext/Blend/Val
        (string){name}
        (float){ value}
        
        /VMC/Ext/Blend/Apply

        BlendShapeProxy value
        */
        // I'm assuming we can send several blendshapes and apply all at once.
        foreach (Channel c in config.channels) {
            client.Send("/VMC/Ext/Blend/Val", config.channels[0].name, config.channels[0].value);
        }
        client.Send("/VMC/Ext/Blend/Apply");
    }

    void Update()
    {
        UpdateConfigFromUI();
        UpdateUIFromConfig();
        VMCSend();
    }

    private void OnApplicationQuit() {
        WriteConfig(config);
    }
}

