3D_Sphere（3D语音天气球）
=========
通过语音服务，天气服务，Unity3D，Android来制作一个3D语音天气预报项目。

简介：
--------
下图是Unity端在电脑上运行的效果：  
![alt 3D语音天气球](http://img.my.csdn.net/uploads/201412/02/1417452590_9239.gif "3D语音天气球")

下图是Unity结合Android和语音控制之后在手机运行的效果图：  
![alt 3D语音天气球](http://img.my.csdn.net/uploads/201501/02/1420199373_1667.png "3D语音天气球")

项目结构：
--------

首先这个项目的开发分为Android端和Unity3D端：  
**Android端：**  
Android端主要负责的是语音控制模块和4个按钮，并将语音处理后的结果传递到Unity端中做处理。

**Unity3D端：**  
Unity端负责接收Android端语音处理后传递过来的信息，和4个按键的反馈。  
并根据不同的省市情况实时的从网上获取天气信息，解析后显示在我们制作的3D球上。  

**Build：**  
最后将Android端的代码以插件的形式放入Unity端中，然后在Unity端Build成apk文件在手机中运行。

Git Branch:
-----------

* master/develop: 创建可旋转的3D球
* feature/Wather_3D_Sphere: 根据天气服务来动态创建3D球
* feature/Voice_Wather_3D_Sphere: 添加了语音控制和天气服务的完整代码



    
详细介绍请参照我的blog：[http://blog.csdn.net/a396901990/article/details/41653365](http://blog.csdn.net/a396901990/article/details/41653365)
