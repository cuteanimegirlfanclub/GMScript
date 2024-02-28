# 我的Unity基本项目包

## 目录

- [介绍](#介绍)
  - [Prerequisites](#prerequisites)
  - [特性](#特性)
- [说明&使用](#说明&使用)
  - [Basic Value](#BasicData)
  - [Data Persisitence](#DataPersisitence)
  - [GMNodes](#GMNodes)
  - [Addressables](#Addressables)
  - [Brain](#Brain)
  - [杂项](#杂项)

## 介绍

我为使用Unity引擎进行游戏开发而写的或正在写一些基本工具，包括：
基于Scriptabe Object、编辑器友好的可序列化的基础数值（系统）；
使用Mesh和Collider做的GameObject探测器和缓存器；
用BinaryWriter做的存储系统，支持UnityEditor进行Debug和编辑；
用Scriptable Object做的Runtime ID lookup system；
使用UnityEditor的GraphView，可以使用编辑器操作的行为树图等。

还有其他一些杂项包括：
用SRP写的渲染管线和Unity ShaderLab&URP&RP Library写的Toon Shader（没写完）、之前用xNode做的行为树图、SO的状态机以及为演示和测试用做了一些UI和其他的东东。

### Prerequisites

[UniTask](https://github.com/Cysharp/UniTask)  
[xNode](https://github.com/Siccity/xNode)

### 特性

目标特性是面向对象编程的和编辑器和用户友好，希望可以实现为高度解耦的模块化工具。

## 说明&使用

### Basic Data
使用ScriptableObject来封装某些基础变量，来获取UnityEngine的序列化系统的支持。  
```
    public abstract class ValueVariable<T> : ScriptableObject where T : IComparable{
        public abstract void SetValue(T value);
        public abstract T Value { get; }

    public class FloatVariable : ValueVariable<float>{
        [SerializeField]
        private float value;
```
你可以在Unity编辑器中创建这些变量：  
![UseValueVariables](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/1972e016-4513-4d96-a0ec-24489ffe0a0b)

再用Reference类来封装ValueVariable类，可以安全地实现一些编辑器功能
```
    [Serializable]
    public class FloatReferenceRO : IValueReference{
        [SerializeField]
        private FloatVariable variable;

        //true when you want to use a constantValue instead of ValueVariable if you dont need it
        public bool useConstant = false;
        public float constantValue;
        public float Value => useConstant ? constantValue : variable.Value;

    public class FloatReferenceRW : IValueReferenceRW{

    //在用户类中引用Reference：
    public class MyCollection{
        public FloatReferenceRO walkSpeed;
```
可以在不破坏ValueVariable类的条件下使用PropertyDrawer：
![UseFloatReference1](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/a85a48d2-9ae8-4a92-9d28-57b477b35bbb)![UseFloatReference2](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/c1c2a575-1c64-4a7a-be37-34a80ad45c70)


### Data Persisitence
使用BinaryWriter/Reader实现对硬盘存档文件的读写任务
```
   public class GameDataWriter : IDisposable{
        BinaryWriter writer;
        public GameDataWriter(BinaryWriter writer) { this.writer = writer; }
        public void Write(Quaternion value){
            writer.Write(value.x);
            writer.Write(value.y);
            writer.Write(value.z);
            writer.Write(value.w);
        }
        //....

    public class SaveDataReader : IDisposable{
        BinaryReader reader;
        public SaveDataReader(BinaryReader reader) { this.reader = reader; }
        //...

    //做一个需要保存的数据封装
    public class SaveData : ScriptableObject, IDisposable{

    //example usage codes:
    using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create)))
    using (var Gwriter = new GameDataWriter(writer))
    using (var data = new SaveData()){
        // Pulling data from game object to the game data class
        RaiseOnPullDataRequest(data);
        // Save game data to the file
        data.Save(Gwriter);}
```
可以在编辑器中实现对硬盘中的存档文件的读写&Debug：  
![SaveData2](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/1cefa2db-0677-4df4-8473-2babc4fd9ecb)

### GMNodes
使用UItookit的GraphView命名空间制作的树/图编辑器  
![61ddbaa0f2d639778485326cfefe56a](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/811fa5df-6187-4d1c-9aee-a790ac68d8f3)

```
   //抽象节点类
   public abstract class GMNode : ScriptableObject, IGMNode{
        public ProcessStatus status = ProcessStatus.Running;
        public bool started = false;
        private string guid;
        public virtual GMNode DeepCopy()
        public ProcessStatus Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }
            status = OnUpdate();
            if (status != ProcessStatus.Running)
            {
                OnStop();
                started = false;
            }
            return status;
        }
        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract ProcessStatus OnUpdate();

    //可以继承抽象节点类
    public abstract class GMCompositeNode : GMNode{
        private GMNode parent;
        private List<GMNode> children = new List<GMNode>();

    //编辑器中节点的VisualElement
    public class GMNodeView : GraphView.Node{
        private GMNode node;
        //...

    //想要控制某个具体节点类在编辑器中的行为，可以创建一个继承自GMNodeEditor并贴上CustomNodeEditor(typeof())特性：
    [CustomNodeEditor(typeof(GMNode))]
    public class GMNodeEditor : GMNodeEditorBase<GMNodeEditor, CustomNodeEditorAttribute, GMNode>{

        //在创建这个节点的InspectorElement时调用
        protected virtual InspectorElement CreateGUI(InspectorElement element)

        //详见下图
        protected virtual InspectorElement ModifyBodyGUI(InspectorElement bodyElement)
        protected virtual InspectorElement ModifyInspectorGUI(InspectorElement inspectorElement)

        //在节点创建完毕后调用
        public virtual void Init()
        protected virtual void OnStatusChangeEditor(ProcessStatus newState, Label stateLabel, VisualElement stateContainer)
        public virtual void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        public virtual void OnInputChanged(GMNode node)
        public virtual void OnOutputChanged(GMNode node)
```
![469e86a0847136c75817b17b7afbc28](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/74e882c8-37da-41e9-ad41-704de11945e9)

### Addressables
正在开发的一个工具，用于运行时的IDLookup和编辑器资源管理  

### Brain
用mesh和collider做的GameObject探测器和缓存器，不多赘述了
![bbe1351bda1b4d2e082dffd750c857d](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/10ab153c-dc98-4af1-a7bc-665b88f98af3)

###杂项
1.手写的SRP渲染管线和未完成的ToonShader：  
![toonshader2](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/9472295d-7a23-43e2-bae2-fd831e786f96)
2.基于xNode做的行为树，因为xNode用的是IMGUI且内存和运算开销都比较高就放弃了
3.可以读取、浏览、播放gltf格式模型&动画的浏览器；  
4.测试和演示用的小DEMO，目前有点bug：  
做了人物移动、拾取道具以及从背包UI中丢弃和装备道具；  
存档读档系统和对应的UI，包括messagebox和loading text,现在发现和道具系统有些冲突在debug；  
用UniTask异步实现的逐字显示对话框；  
用GMNodes做的行为树和状态机；  
用RenderTexture实现的光照强度探测系统，可以模拟照射在物体上的光照强度；  
![52b4382d05fb66a236781bb975efcab](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/4c055a20-02dc-4d8a-8c80-07c24a8c07a4)
![beb2237937197a8e3258a5f763c17ea](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/057590cf-a826-4f2f-a791-f0aae8d0d0fb)
![cf65cf0e6452df78e8c77f0c6c0ca13](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/dbd7247b-7179-4d5e-ad09-c1ea678a68de)
![792bbf0a840cbc9e7acf1cb57522a4c](https://github.com/cuteanimegirlfanclub/GMScript/assets/130042920/07f83d21-6cd3-4b03-9cfd-ce7718f2382b)

