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

