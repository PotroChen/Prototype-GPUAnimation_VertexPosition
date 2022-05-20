# Prototype-GPUAnimation_VertexPosition
将蒙皮动画支持GPUInstancing的一个方案。这是我参考(抄的)**陈嘉栋老师实现的方案**，自己重新实现了一遍**学习用的仓库**。代码几乎一样，只是支持了采样多个SkinnedMeshRenderer的角色。
参考的文章:https://www.cnblogs.com/murongxiaopifu/p/7250772.html
参考的仓库:https://github.com/chenjd/Render-Crowd-Of-Animated-Characters

## 原理
将蒙皮动画播放动画时，每帧模型的顶点位置存在一张贴图上。然后播放时，通过shader顶点着色器采样这张贴图获得每帧不同的顶点位置来播放动画。

## 缺点
生成的贴图文件过大，对于顶点数量多的模型或者动画时长较长的，甚至有可能超过Unity可创建贴图上限。这时候考虑另外一种**储存骨骼位置信息**的方式，在我的另一个仓库里面。
