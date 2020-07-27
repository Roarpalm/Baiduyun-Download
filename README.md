# 感谢该站长造福人类

- 本工具原理是正则匹配，填充，再把下载链接爬取下来

```
http://pan.naifei.cc/?share={链接}%20&pwd={提取码}
```

# Python版
- 利用这个[网站](http://pan.naifei.cc/)写的一个小工具，带图形界面

- 附带一个打包好的exe

![界面](Python/img/baiduyun.png?raw=true)

## 更新日志

2020年7月10日

- 修复了提取码未能正确识别的bug
- 修复了无法完整提取链接的bug

2020年7月13日

- 简化试错代码

2020年7月16日

- 会正确爬取多个文件的下载链接

# C#版

- 功能和Python版相同，但exe体积小很多

- 应用程序在 C#/windows/bin/Release/windows.exe

![界面](GUI.png?raw=true)

## 更新日志

2020年7月18日

- 目前存在的缺点：链接和提取码之间必须有空格，输入的分享链接文字里不能包含回车

- 优化匹配代码

- 爬取到下载链接后会自动打开浏览器

- C#真的奇怪，正则匹配不到直接跳过循环而不是表现为空或否，空字符 "" 和 null 不能等价于 bool false

- 会自动跳过大小为0M的文件

2020年7月21日

- 更好地匹配提取码

2020年7月22日

- 新增UI界面

- 点击跳转而非自动跳转

2020年7月27日

- 给C#程序添加应用图标