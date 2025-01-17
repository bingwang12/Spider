 **物联网四板斧：[Mars 实时库](https://github.com/cdy816/mars) 、设备采集平台[Spider](https://github.com/cdy816/Spider) 、跨平台界面解决方案[Chameleon](https://github.com/cdy816/Chameleon)、数据分析报警引擎[Ant](https://github.com/cdy816/Ant)**
 <br />
 <br />
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Apache License][license-shield]][license-url]
<br />
[![star](https://gitee.com/chongdaoyang/Spider/badge/star.svg?theme=white)](https://gitee.com/chongdaoyang/Spider/stargazers)
[![fork](https://gitee.com/chongdaoyang/Spider/badge/fork.svg?theme=white)](https://gitee.com/chongdaoyang/Spider/members)

<!-- PROJECT LOGO -->
<br />
<p align="center">

  <h2 align="center">Spider 数据互联</h2>
 
  <p align="center">
    高性能、跨平台设备采集平台!        
    <br />
    <br />
    <a href="https://github.com/cdy816/Spider/tree/master/Doc">帮助文档</a>
    ·
    <a href="https://github.com/cdy816/Spider/issues">Bug 提交</a>
    ·
    <a href="https://github.com/cdy816/Spider/issues">功能申请</a>
  </p>
</p>

# Spider
跨平台(Window、Linux、嵌入式设备)的物联网设备采集平台。Spider 可以配合数据库Mars使用，充当数据库的设备采集模块；同时也可以独立运行，以搭配其他实时数据库软件。

1. Spider 支持多个工程的开发，每个工程是独立运行的一组物理设备采集单元。
2. 多个工程之间支持以树形结构进行级联，以支持数据的汇总。
3. 支持计算型设备以及变量，允许用户通过嵌入自己的脚本代码（C#代码,可以直接访问标准的或自定义的.net类库），实现特定的工程逻辑。

## 支持协议列表
1. MQTT
2. OPC UA
3. Modbus
4. coap

## 运行环境
系统平台采用.net 5 平台开发,实现了跨平台性，可以运行在桌面系统以及嵌入设备中（例如:树莓派等）。
* 运行系统：可部署在window、Linux、IOT等操作系统中,也可以部署在Docker中。 
* 开发系统：采用图形化的配置界面、CS结构,暂定采用桌面的方式实现，运行在Window系统中。

## 帮助文档、接口开发文档
1. [文档](https://github.com/cdy816/Spider/blob/master/Doc)

## 版本发布
* [V0.4](https://github.com/cdy816/Spider/releases/tag/V0.4) 。


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/cdy816/spider.svg?style=for-the-badge
[contributors-url]: https://github.com/cdy816/mars/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/cdy816/spider.svg?style=for-the-badge
[forks-url]:https://github.com/cdy816/mars/network/members
[stars-shield]: https://img.shields.io/github/stars/cdy816/spider.svg?style=for-the-badge
[stars-url]:https://github.com/cdy816/mars/stargazers
[issues-shield]: https://img.shields.io/github/issues/cdy816/spider.svg?style=for-the-badge
[issues-url]:https://github.com/cdy816/mars/issues
[license-shield]: https://img.shields.io/github/license/cdy816/spider.svg?style=for-the-badge
[license-url]: https://github.com/cdy816/spider/blob/master/LICENSE
