- Allow changing IP as well as port
- Move settings to a separate screen with a settings button
- Persist settings (slider values, name, port)
    - Load on start
    - Save on exit
- Undo/Redo, if not built-in
- Button to add more sliders
    - Allow creating groups / headings?
- Show slider numerical value, allow typing in
- Add slider/control types for other things than blendshapes e.g. camera
- Allow saving a preset for a group of controls
- Test sending a bundle with tag 'Immediate'?

```
var root = new Bundle(Timestamp.Immediate);
root.Add(new Message("/VMC/Ext/etc"));
root.Add(...);
client.Send(root);
```