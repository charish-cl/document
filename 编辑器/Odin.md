[Odin归档 - 个人技术笔记 (aihailan.com)](https://aihailan.com/archives/category/odin)







https://aihailan.com/archives/466







自定义Add方法

```cs
100
```







```
把Dictionary替换成List,并添加[TableList]特性,并添加一个功能当我页面修改TextMeshProUGUI的内容是,Selection要选中这个字体,且现在List中的元素为一个自定义的,它包含路径(与Canvas的相对路径),name,和

[LabelText("UI工具/字体工具")]
public class TextMeshWindow:OdinEditorWindow
{
    public  Dictionary<TextMeshProUGUI, string> textMeshDictionary;

    [Button("获取所有字体",ButtonSizes.Large)]
    public void GetAllTextMesh()
    {

        // Initialize dictionary
        textMeshDictionary = new Dictionary<TextMeshProUGUI, string>();
        // Find all TextMesh components under the Canvas object
        
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            TextMeshProUGUI[] textMeshes = canvas.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textMesh in textMeshes)
            {
                textMeshDictionary[textMesh] = textMesh.text;
            }
        }
    }
    [Button("保存",ButtonSizes.Large)]
    public void Save()
    {
        foreach (var  entry in textMeshDictionary)
        {
            entry.Key.text = entry.Value;
        }
    }
}
```