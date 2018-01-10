# Ulog
Unity3D Log Extension

使用:

```c#
Ulog.Log("Hello", "Unity", "2018");
// 11:24:09 AM Hello Unity 2018 

Ulog.LogError("Hello", "Unity", 2018);
// 11:24:09 AM Hello Unity 2018 

Ulog.LogFormat("{0}-{1}-{2}", 123, 456, 789);
// 11:24:09 AM 123-456-789

Ulog.LogErrorFormat("{0} like {1}", "I", "Coffee");
// 11:24:09 AM I like Coffee
```

> 使用时请自行编译Dll或者直接使用ReleaseDll下面已经编译好的Dll,放到工程的Plugins目录下即可
>
> 不要直接使用源代码，否则双击日志打印时会跳到Ulog，而不是跳到调用Ulog的地方