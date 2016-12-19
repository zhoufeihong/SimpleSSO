# SimpleSSO
基于Microsoft.Owin.Security.OAuth实现OAuth 2.0所有应用场景，可集成单点登录功能

说明：

    初始化：
  
	第一次打开在SimpleSSO/Config/connectionStrings.config配置数据库链接，第一次会自动生成数据库及测试数据

	使用用户：admin，密码：123选择角色登录
  
    功能：

	菜单-> SimpleSSO应用->应用ShowCase：需要启动SimpleSSOTest项目，OAuth授权Demo

	菜单-> 系统管理-> 秘钥管理：查看授权的秘钥，秘钥暂时存储在缓存(ObjectCache)
 
    关于：

	感兴趣的朋友可以看看代码里面Autofac、AutoMapper、EF实践模式，可根据系统实际应用场景调整为比较好的实践模式。

系统分层：

 	FreeBird.Infrastructure 基础设施层：包含通用功能，主要封装实现了Ioc容器上下文、IRepository、秘钥存储、缓存、拦截器、异常类型、读写锁		
	
	SimpleSSO.Domain 领域层：主要为数据库实体，只依赖于FreeBird.Infrastructure，可扩展IRepository接口给SimpleSSO.Application层使用
        
	SimpleSSO.DTO DTO 展示层业务层桥梁
	
	SimpleSSO.Application 业务层，依赖于上面三层，不能依赖于SimpleSSO.EFRepositories(这个很重要)
	
	SimpleSSO.EFRepositories 领域层使用EF的一种实现，DBContext、Map、EFRepository、UnitOfWork，实现IRepository及领域层扩展的IRepository	
	
	SimpleSSO 站点
	
	SimpleSSOTest OAuth测试站点
	
使用组件：

    后台:

	Autofac IOC容器,mvc,WepApi集成.

	AutoMapper 对象映射

	Katana.Microsoft.Owin.Security.OAuth 实现OAuth2.0规范.NET开源框架.

	Microsoft.AspNet.WebApi.Cors 跨域.

	EF 微软开源ORM框架，系统使用CodeFirst模式，其实个人偏爱使用DBFirst.

	EntityFramework.Extended EF扩展.

	SignalR 及时通信框架.

	部分源码参考借鉴自项目：Nopcommerce.


    前端：

	bootstrap

	admin-lte bootstrap UI模板

	bootstrap-table

	bootstrapValidator

	fileinput

	jquery-ui

	select2

	toastr 

	jquery-linq

	如果是bootstrap新手，建议看看http://www.cnblogs.com/wuhuacong/p/4757984.html的介绍。(PS:我也是第一次用bootstrap)

为什么写这个项目：
这两年工作都是用的VS2008/VS2010开发，技术退步了，脑袋僵了，正好趁这个离职的空隙抽几天时间写了这个项目。一来自己练练手，然后分享出来，希望可以给看这个项目的人节省一点时间。时间比较仓促，日志、资源文件也没加，后续有时间我会把这个项目重构一下。

有什么疑问或者建议可请发送邮件：397353778@qq.com 称呼我周就好了
