﻿/*
 * 由SharpDevelop创建。
 * 用户： Administrator
 * 日期: 2013/12/10
 * 时间: 13:57
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */

using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Routing;

namespace JR.Cms.AspNet
{
    /// <summary>
    /// Description of HttpApplicaiton.
    /// </summary>
    public partial class Application : HttpApplication
    {
        static Application()
        {
            //解决依赖
            // __ResolveAppDomain.Resolve();
        }

        protected virtual void RegisterRoutes(RouteCollection routes)
        {
            // ------------------------------------------------
            //  注册首页路由，可以自由修改首页位置
            //  routes.MapRoute("HomeIndex", ""
            //  , new { controller = "Cms", action = "Index" });
            // ------------------------------------------------


            // ------------------------------------------------
            //     自定义路由放在这里
            //  -----------------------------------------------
        }

        protected virtual void Application_Start()
        {
            AspNetCmsInitializer.Init();
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            return;
            Exception exc = Server.GetLastError();
            if (exc.InnerException != null)
            {
                exc = exc.InnerException;
            }

            //PageUtility.RenderException(Server.GetLastError(),true);
            //Server.ClearError();

            #region 记录并发送错误到邮箱

            if (true)
            {
                new Thread(() =>
                {
                    try
                    {
                        string mailContent = "";
                        string mailSubject = "";


                        HttpRequest req = HttpContext.Current.Request;

                        StringBuilder sb = new StringBuilder();

                        sb.Append("---------------------------------------------------------------------\r\n")
                            .Append("[错误]：IP:").Append(req.UserHostAddress)
                            .Append("\t时间：").Append(DateTime.Now.ToString())
                            .Append("\r\n[信息]：").Append(exc.Message)
                            .Append("\r\n[路径]：").Append(req.Url.PathAndQuery)
                            .Append("  -> 来源：").Append(req.Headers["referer"] ?? "无")
                            .Append("\r\n[堆栈]：").Append(exc.StackTrace)
                            .Append("\r\n\r\n");

                        mailContent = sb.ToString();

                        mailSubject = String.Format("[{0}]异常:{2}", Cms.Context.SiteDomain, exc.Message);


                        //发送邮件
                        SmtpClient smtp = new SmtpClient("smtp.126.com", 25);
                        smtp.Credentials = new NetworkCredential("q12176@126.com", "123000");

                        MailMessage msg = new MailMessage("q12176@126.com", "q12177@126.com");

                        msg.Subject = mailSubject;
                        msg.IsBodyHtml = true;
                        msg.Body = mailContent;

                        //msg.Priority= MailPriority.High;

                        smtp.Send(msg);
                    }
                    catch (Exception ex)
                    {
                    }
                }).Start();
            }

            #endregion
        }

        protected virtual void Application_BeginRequest(object o, EventArgs e)
        {
            //mono 下编码可能会有问题
            if (Cms.RunAtMono)
            {
                Response.Charset = "utf-8";
            }
        }
    }
}