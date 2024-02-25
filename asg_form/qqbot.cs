using asg_form.Controllers;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;
using System.Reactive.Linq;
using Microsoft.EntityFrameworkCore;
using Mirai.Net.Data.Events.Concretes.Bot;
using static asg_form.Controllers.schedule;
using NPOI.OpenXmlFormats.Spreadsheet;
using Manganese.Array;
using asg_form.Migrations;
using static NPOI.HSSF.Util.HSSFColor;
using System.Drawing;
using System.Drawing.Imaging;
using Masuit.Tools.Hardware;
using Masuit.Tools.Win32;
using Masuit.Tools;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Shared;
using ChatGPT.Net;
using Mirai.Net.Utils.Scaffolds;
using ChatGPT.Net.DTO.ChatGPT;
using Mirai.Net.Data.Events.Concretes.Group;
using System.Net;

namespace asg_form
{

    public class qqbot : BackgroundService
    {
        static public DateTime time = new DateTime();
        static public team_game open_team = new team_game();
        private void setTaskAtFixedTime()
        {
            DateTime now = DateTime.Now;
            DateTime oneOClock = DateTime.Today.AddHours(1.0); //凌晨1：00
            if (now > oneOClock)
            {
                // Console.WriteLine("=======================================>");
                oneOClock = oneOClock.AddDays(5.0);
            }
            int msUntilFour = (int)((oneOClock - now).TotalMilliseconds);

            var t = new System.Threading.Timer(doAt1AM);
            t.Change(msUntilFour, Timeout.Infinite);
        }
        public static bool isToday(DateTime dt)
        {
            DateTime today = DateTime.Today;
            DateTime tempToday = new DateTime(dt.Year, dt.Month, dt.Day);
            if (today == tempToday)
                return true;
            else
                return false;
        }

        private async void doAt1AM(object state)
        {
            //执行功能...
            try
            {
                TestDbContext db = new TestDbContext();
                var sh = db.team_Games.ToList();
                var sh1 = sh.Where(a => isToday(a.opentime)).ToList();
                string msg = "";
                if (sh1 == null)
                {
                    msg = "<今日无赛程！>";
                }
                else
                {
                    foreach (var a in sh1)
                    {
                        msg = $"{msg}\r\r{a.team1_name} VS {a.team2_name}";
                    }
                }
                await MessageManager.SendGroupMessageAsync("870248618", $"今日赛程：\r\n{msg}\r\n请有比赛的解说提前准备好。");
                await Task.Delay(3000);
                await MessageManager.SendGroupMessageAsync("456414070", $"今日赛程：\r\n{msg}\r\n。直播地址：\r\nhttps://live.bilibili.com/24208371");

                Console.WriteLine("开始备份数据库");



            }
            catch
            {

            }
            //再次设定
            setTaskAtFixedTime();
        }

public static string UrlToBase64(string imageUrl)
{
    // 创建一个WebClient实例用于下载URL内容
    using (var webClient = new WebClient())
    {
        // 下载URL的内容到字节数组
        byte[] data = webClient.DownloadData(imageUrl);

        // 将字节数组转换为Base64字符串
        string base64String = Convert.ToBase64String(data);

        return base64String;
    }
}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //定时任务
                setTaskAtFixedTime();
                var bot = new MiraiBot
                {
                    Address = "124.223.35.239:80",
                    QQ = "197649191",
                    VerifyKey = "1234567890"
                };
             
                await bot.LaunchAsync();

                bot.EventReceived
                       .OfType<DroppedEvent>()
 .Subscribe(async receiver =>
 {
     await Task.Delay(10000);
     await ExecuteAsync(stoppingToken);
 });



  bot.EventReceived
                       .OfType<MemberJoinedEvent>()
 .Subscribe(async receiver =>
 {
     //等待窜写
     var messageChain = new MessageChainBuilder()
                   .At(receiver.Member.Id)
                  .Plain("欢迎新成员,我是ASG赛事五端之一：ASG机器人。请严格遵守群规。以下图片是我的指令图")
                .ImageFromPath($@"{AppDomain.CurrentDomain.BaseDirectory}bot.png")                 
                  .Build();
     await MessageManager.SendGroupMessageAsync(receiver.Member.Group.Id, messageChain);


 });

                bot.MessageReceived
    .OfType<GroupMessageReceiver>()
    .Subscribe(async x =>
    {
        try
        {
            if (x.MessageChain.GetPlainMessage() == "近期赛程")
            {
                TestDbContext testDb = new TestDbContext();
                int q = testDb.team_Games.Count();
                var a = testDb.team_Games.Where(a => a.team1_name != "TBD").OrderByDescending(a => a.opentime).Take(7);
                string msg = "";
                foreach (var b in a)
                {
                    msg = $"{msg}\r\n{b.team1_name} VS {b.team2_name}\r\n时间:{b.opentime.ToString("f")}\r\n属于:{b.tag}\r\n";
                }
                await MessageManager.SendGroupMessageAsync(x.GroupId, msg);
            }
            if (x.MessageChain.GetPlainMessage() == "后端状态")
            {
                string msg = $"CPU占用:{SystemInfo.CpuLoad.ToString("f2")}\r\n内存占用:{SystemInfo.MemoryAvailable.ToString("f2")}\r\n部署系统:{Windows.GetOsVersion()}";
                await MessageManager.SendGroupMessageAsync(x.GroupId, msg);
            }
            if (x.MessageChain.GetPlainMessage() == "参赛队伍")
            {
                TestDbContext testDb = new TestDbContext();
                var team = testDb.Forms.Select(a => a.team_name);
                string msg = "";
                foreach (var t in team)
                {

                    msg = $"{msg} {t}";
                }
                await MessageManager.SendGroupMessageAsync(x.GroupId, $"在所有比赛中有以下队伍参赛：\r\n{string.Join(" , ", team)}");

            }
            if (x.MessageChain.GetPlainMessage() == "往届冠军")
            {
                TestDbContext ctx = new TestDbContext();

                var teams = ctx.Champions.Include(a => a.events).Include(a => a.form.role).Select(a => new { a.form, a.events }).ToList();
                string msg = "";
                foreach (var t in teams)
                {
                    string role = "";

                    foreach (var t2 in t.form.role)
                    {
                        role = $"{role}  {t2.role_name}";
                    }
                    msg = $"{msg}\r\n队伍名称:{t.form.team_name}\r\n队员：{role}\r\n属于：{t.events.name}\r\n";
                }
                await MessageManager.SendGroupMessageAsync(x.GroupId, $"我们拥有以下冠军:{msg}");

            }
            if (x.MessageChain.GetPlainMessage() == "我的队伍")
            {
                TestDbContext ctx = new TestDbContext();

                var t = await ctx.Forms.Include(a => a.events).Include(a => a.role).FirstOrDefaultAsync(a => a.team_tel == x.Sender.Id);
                if (t == null)
                {
                    await MessageManager.SendGroupMessageAsync(x.GroupId, $"当前qq下没有队伍");


                }
                else
                {
                    string role = "";

                    foreach (var t2 in t.role)
                    {
                        role = $"{role}  {t2.role_name}";
                    }
                    var msg = $"队伍id:{t.Id}\r\n队伍名称:{t.team_name}\r\n队员：{role}\r\n属于：{t.events.name}\r\n";

                    await MessageManager.SendGroupMessageAsync(x.GroupId, $"这是你拥有的队伍:\r\n{msg}");

                }

            }







            if (x.MessageChain.GetPlainMessage() == "近期胜者")
            {
                TestDbContext ctx = new TestDbContext();

                var events = ctx.events.OrderByDescending(a => a.Id).FirstOrDefault();
                var team = ctx.team_Games.Where(a => a.belong == events.name && a.winteam != null).Take(5);
                string msg = "";
                foreach (var t in team)
                {
                    string role = "";
                    msg = $"{msg}\r\n胜者名称:{t.winteam}\r\n比赛详情：\r\n{t.team1_name} VS {t.team2_name}\r\n属于:{t.tag}";
                }
                await MessageManager.SendGroupMessageAsync(x.GroupId, $"（仅展示最近五个）\r\n在比赛： {events.name} 中\r\n有以下队伍获胜:\r\n{msg}");

            }

            if (x.MessageChain.GetPlainMessage() == "关于ASG系统")
            {

                await MessageManager.SendGroupMessageAsync(x.GroupId, $"ASG系统是一个前后端分离，由五端组成的一个巨型系统。\r\n并且，它在大部分地方都是开源的。\r\n此项目的成功感谢以下公司提供的技术栈来简化我们的开发：\r\n1.Microsoft(.NET)\r\n2.Vue(Vue.js)\r\n项目组成员：\r\n罗澜，浊泉，础砜");

            }
        }
        catch (Exception ex)
        {
            await MessageManager.SendGroupMessageAsync(x.GroupId, $"错误:{ex.Message}");

        }


    });
                bot.EventReceived
                    .OfType<NewMemberRequestedEvent>()
                    .Subscribe(async e =>
                    {
                        //同意入群
                        await RequestManager.HandleNewMemberRequestedAsync(e, NewMemberRequestHandlers.Approve);
                    });

                bot.EventReceived
                    .OfType<NewFriendRequestedEvent>()
                    .Subscribe(async e =>
                    {
                        //传统的方式
                        await RequestManager.HandleNewFriendRequestedAsync(e, NewFriendRequestHandlers.Approve);

                    });


                bot.MessageReceived
                     .OfType<GroupMessageReceiver>()
 .Where(a => a.MessageChain.GetPlainMessage().StartsWith("查询战队 "))
                .Subscribe(async x =>
                {

                    try
                    {
                        TestDbContext ctx = new TestDbContext();

                        string result = x.MessageChain.GetPlainMessage().Substring(5); // 截取从'o'之后的字符串
                        Console.WriteLine(result);
                        List<form> teams = ctx.Forms.Include(a => a.role).Where(a => a.team_name.IndexOf(result) >= 0).Take(6).ToList();
                        string msg = "";
                        foreach (var t in teams)
                        {
                            string role = "";
                            foreach (var t2 in t.role)
                            {
                                role = $"{role}  {t2.role_name}";
                            }
                            msg = $"{msg}\r\n队伍ID:{t.Id}\r\n队伍名称:{t.team_name}\r\n平均段位： {t.role.Average(a => a.Historical_Ranks).Round(2) + 1} 阶\r\n队员：{role}\r\n";
                        }
                        await MessageManager.SendGroupMessageAsync(x.GroupId, $"(超过6个的队伍将被折叠)\r\n{msg}\r\n发送“点赞队伍 <队伍id>”可给指定队伍投票");
                    }
                    catch
                    {

                    }

                });

                bot.MessageReceived
                     .OfType<GroupMessageReceiver>()
              .Where(a => a.MessageChain.GetPlainMessage().StartsWith("ai "))
                             .Subscribe(async x =>
                             {

                                 TimeSpan d = DateTime.Now.Subtract(time);
                                 if (d.Seconds >= 10)
                                 {

                                     TestDbContext ctx = new TestDbContext();

                                     string result = x.MessageChain.GetPlainMessage().Substring(3); // 截取从'o'之后的字符串
                                     Console.WriteLine(result);
                                     ChatGpt bot = new ChatGpt("sk-PclVxxonxRNituSFB509Ad32Ef704690Ac6027A68dBc01Fa", new ChatGptOptions
                                     {
                                         BaseUrl = "https://gptkey.nxsir.cn"
                                     });
                                     var msg = await bot.Ask(result);
                                     var messageChain = new MessageChainBuilder()
                    .At(x.Sender.Id)
                   .Plain(msg)
                   .Build();
                                     await MessageManager.SendGroupMessageAsync(x.GroupId, messageChain);


                                 }
                                 else
                                 {
                                     // await MessageManager.SendGroupMessageAsync(x.GroupId, $"点赞失败，触发刷屏防御！\r\n请在 {20 - d.Seconds} 秒后投票！");

                                 }






                            

                             });



                                    bot.MessageReceived
                     .OfType<GroupMessageReceiver>()
              .Where(a => a.MessageChain.GetPlainMessage().StartsWith("绘图 "))
                             .Subscribe(async x =>
                             {

                                                            // 计算当前时间与指定时间的时间差
                                TimeSpan d = DateTime.Now.Subtract(time);
                                if (d.Seconds >= 10)
                                {
                                    // 获取上下文对象
                                    TestDbContext ctx = new TestDbContext();

                                    // 截取字符串
                                    string result = x.MessageChain.GetPlainMessage().Substring(3); // 截取从'o'之后的字符串
                                    Console.WriteLine(result);

                                    // 发送消息给群组
                                    await MessageManager.SendGroupMessageAsync(x.GroupId, "正在绘制，请稍后！");

                                    // 构建消息链
                                    var messageChain = new MessageChainBuilder()
                                        .At(x.Sender.Id)
                                        .ImageFromBase64(UrlToBase64($"https://ai.xiaoluolan.com/?value={result}"))
                                        .Build();

                                    // 发送消息链给群组
                                    await MessageManager.SendGroupMessageAsync(x.GroupId, messageChain);
                                }
                                else
                                {
                                    // 发送消息给群组
                                    await MessageManager.SendGroupMessageAsync(x.GroupId, $"绘画正在冷却期！\r\n请在 {20 - d.Seconds} 秒后再次绘画！");
                                }






                            

                             });



                bot.MessageReceived
 .OfType<AtMessage>()
 .Where(a => a.Target == bot.QQ)
 .OfType<GroupMessageReceiver>()

                .Subscribe(async x =>
                {
                    try
                    {

                        TimeSpan d = DateTime.Now.Subtract(time);
                        if (d.Seconds >= 10)
                        {

                            ChatGpt bot = new ChatGpt("sk-PclVxxonxRNituSFB509Ad32Ef704690Ac6027A68dBc01Fa", new ChatGptOptions
                            {
                                BaseUrl = "https://gptkey.nxsir.cn"
                            });
                            var msg = await bot.Ask(x.MessageChain.GetPlainMessage());
                            var messageChain = new MessageChainBuilder()
          .At(x.Sender.Id)
          .Plain(msg)
          .Build();
                            await MessageManager.SendGroupMessageAsync(x.GroupId, messageChain);

                        }
                        else
                        {
                           // await MessageManager.SendGroupMessageAsync(x.GroupId, $"点赞失败，触发刷屏防御！\r\n请在 {20 - d.Seconds} 秒后投票！");

                        }


                     
                    }
                    catch
                    {
                        await MessageManager.SendGroupMessageAsync(x.GroupId, "出现错误");

                    }

                });


                bot.MessageReceived
.OfType<GroupMessageReceiver>()
.Where(a => a.MessageChain.GetPlainMessage().StartsWith("开始投票 ")).Where(a => a.GroupId == "860395385")
              .Subscribe(async x =>
              {

                  try
                  {
                      TestDbContext ctx = new TestDbContext();

                      long result = x.MessageChain.GetPlainMessage().Substring(5).ToInt64(); // 截取从'o'之后的字符串
                      Console.WriteLine(result);

                      var game = await ctx.team_Games.FirstOrDefaultAsync(a => a.id == result);
                      open_team = game;

                      await MessageManager.SendGroupMessageAsync(x.GroupId, "设置成功！");
                      await Task.Delay(10000);
                      await MessageManager.SendGroupMessageAsync("456414070", $"比赛：\r\n{game.team1_name}VS{game.team2_name}\r\n即将在{game.opentime.ToString("t")}开赛\r\n届时可以发送“竞猜 左”给左边的队伍投票，“竞猜 右”给右边的队伍投票");

                  }
                  catch
                  {

                  }

              });



                bot.MessageReceived
                  .OfType<GroupMessageReceiver>()
                  .Where(a => a.MessageChain.GetPlainMessage().StartsWith("加黑名单 ")).Where(a => a.GroupId == "860395385")
                                 .Subscribe(async x =>
                                 {

                                     try
                                     {
                                         TestDbContext ctx = new TestDbContext();

                                         long result = x.MessageChain.GetPlainMessage().Substring(5).ToInt64(); // 截取从'o'之后的字符串
                                         var form = await ctx.Forms.Include(a => a.role).FirstAsync(a => a.Id == result);
                                         foreach (var item in form.role)
                                         {
                                             item.role_name = "<禁赛选手>";
                                         }
                                         await ctx.SaveChangesAsync();
                                         await MessageManager.SendGroupMessageAsync(x.GroupId, "Success！");

                                     }
                                     catch
                                     {

                                     }

                                 });






                bot.MessageReceived
.OfType<GroupMessageReceiver>()
.Where(a => a.MessageChain.GetPlainMessage().StartsWith("清除投票")).Where(a => a.GroupId == "860395385")
             .Subscribe(async x =>
             {

                 try
                 {
                     TestDbContext ctx = new TestDbContext();

                     long result = x.MessageChain.GetPlainMessage().Substring(5).ToInt64(); // 截取从'o'之后的字符串
                     Console.WriteLine(result);

                     var game = await ctx.team_Games.FirstOrDefaultAsync(a => a.id == result);
                     open_team = null;

                     await MessageManager.SendGroupMessageAsync(x.GroupId, "设置成功！");

                 }
                 catch
                 {

                 }

             });

                bot.MessageReceived
.OfType<GroupMessageReceiver>()
.Where(a => a.MessageChain.GetPlainMessage().StartsWith("竞猜 "))
            .Subscribe(async x =>
            {

                try
                {
                    TestDbContext ctx = new TestDbContext();

                    string result = x.MessageChain.GetPlainMessage().Substring(3);
                    Console.WriteLine(result);
                    long id = open_team.id;
                    //   var game=await ctx.team_Games.FirstOrDefaultAsync(a=>a.id==id);


                    TestDbContext test = new TestDbContext();
                    var team = test.team_Games.Include(a => a.logs).Single(a => a.id == id);
                    bool isckick = team.logs.Any(a => a.userid == $"qq-{x.Sender.Id}");
                    if (isckick)
                    {
                        await MessageManager.SendGroupMessageAsync(x.GroupId, $"请不要重复投票！");

                    }
                    else
                    {
                        if (result == "左")
                        {
                            team.team1_piaoshu++;
                            team.logs.Add(new schedule_log { userid = $"qq-{x.Sender.Id}", chickteam = team.team1_name, team = team, win = null });
                            await MessageManager.SendGroupMessageAsync(x.GroupId, $"投票成功！目前：\r\n队伍:{team.team1_name}\r\n拥有票数：{team.team1_piaoshu}\r\n\r\n队伍:{team.team2_name}\r\n拥有票数：{team.team2_piaoshu}");

                        }
                        else if (result == "右")
                        {
                            team.team2_piaoshu++;
                        
                            team.logs.Add(new schedule_log { userid = $"qq-{x.Sender.Id}", chickteam = team.team2_name, team = team, win = null });
                            await MessageManager.SendGroupMessageAsync(x.GroupId, $"投票成功！目前：\r\n队伍:{team.team1_name}\r\n拥有票数：{team.team1_piaoshu}\r\n\r\n队伍:{team.team2_name}\r\n拥有票数：{team.team2_piaoshu}");

                        }
                        else
                        {

                                await MessageManager.SendGroupMessageAsync(x.GroupId, $"投票失败！不合规的投票");

                            

                        }
                        await test.SaveChangesAsync();

                    }





                    //     await MessageManager.SendGroupMessageAsync(x.GroupId,"设置成功！");

                }
                catch
                {

                }

            });

                bot.MessageReceived
.OfType<GroupMessageReceiver>()
.Where(a => a.MessageChain.GetPlainMessage().StartsWith("查询选手 "))
              .Subscribe(async x =>
              {

                  try
                  {
                      TestDbContext ctx = new TestDbContext();

                      string result = x.MessageChain.GetPlainMessage().Substring(5); // 截取从'o'之后的字符串
                      Console.WriteLine(result);
                      string msg = "";
                      var roles = ctx.Roles.Include(a => a.form).Where(a => a.role_name.IndexOf(result) >= 0).Take(6).ToList();
                      foreach (var role in roles)
                      {
                          msg = $"{msg}\r\n姓名:{role.role_name}\r\n第五人格ID:{role.role_id}\r\n选手id:{role.Id}\r\n阵营:{role.role_lin}\r\n属于队伍:{role.form.team_name}\r\n游戏名称:{role.Game_Name}\r\n最高段位:{role.Historical_Ranks+1}\r\n";
                      }
                      await MessageManager.SendGroupMessageAsync(x.GroupId, $"搜索到以下结果(超过6个的人员将被折叠):\r\n{msg}");
                  }
                  catch
                  {

                  }

              });

                bot.MessageReceived
                .OfType<GroupMessageReceiver>()
                .Where(a => a.MessageChain.GetPlainMessage().StartsWith("点赞队伍 "))
                               .Subscribe(async x =>
                               {

                                   try
                                   {
                                       TimeSpan d = DateTime.Now.Subtract(time);
                                       if (d.Seconds >= 10)
                                       {

                                           TestDbContext ctx = new TestDbContext();

                                           long result = x.MessageChain.GetPlainMessage().Substring(5).ToInt64(); // 截取从'o'之后的字符串
                                           Console.WriteLine(result);
                                           var form = await ctx.Forms.FirstOrDefaultAsync(a => a.Id == result);
                                           form.piaoshu++;
                                           await ctx.SaveChangesAsync();
                                           await MessageManager.SendGroupMessageAsync(x.GroupId, $"点赞成功！\r\n点赞队伍:{form.team_name}\r\n票数:{form.piaoshu}");
                                           time = DateTime.Now;
                                       }
                                       else
                                       {
                                           await MessageManager.SendGroupMessageAsync(x.GroupId, $"点赞失败，触发刷屏防御！\r\n请在 {10 - d.Seconds} 秒后投票！");

                                       }
                                   }
                                   catch
                                   {

                                   }

                               });



                bot.MessageReceived
.OfType<GroupMessageReceiver>()
.Where(a => a.MessageChain.GetPlainMessage().StartsWith("查询比赛 "))
              .Subscribe(async x =>
              {
                  try
                  {
                      TestDbContext ctx = new TestDbContext();

                      string result = x.MessageChain.GetPlainMessage().Substring(5);
                      Console.WriteLine(result);
                      string msg = "";
                      var roles = ctx.team_Games.Where(a => a.team1_name.IndexOf(result) >= 0 || a.team2_name.IndexOf(result) >= 0).Take(6).ToList();
                      foreach (var t in roles)
                      {
                          msg = $"{msg}\r\n胜者名称:{t.winteam ?? "<暂无数据>"}\r\n{t.team1_name} VS {t.team2_name}\r\n类型:{t.tag ?? "<暂无数据>"}\r\n赛季:{t.belong ?? "<暂无数据>"}\r\n双方胜比值：{t.team1_piaoshu}:{t.team2_piaoshu}\r\n";
                      }
                      await MessageManager.SendGroupMessageAsync(x.GroupId, $"搜索到以下结果(超过6个的比赛将被折叠):\r\n{msg}");
                  }
                  catch
                  {

                  }

              });




                bot.MessageReceived
               .OfType<GroupMessageReceiver>()
               .Where(a => a.MessageChain.GetPlainMessage().StartsWith("查询队长入群情况 "))
                              .Subscribe(async x =>
                              {

                                  try
                                  {
                                      TestDbContext ctx = new TestDbContext();

                                      string[] msgs = x.MessageChain.GetPlainMessage().Split(' ');
                                      //  Console.WriteLine(result);
                                      string msg = "";
                                      var forms = ctx.Forms.Include(a => a.events).Where(a => a.events.name == msgs[1]).ToList();
                                      var group = await bot.Groups.Value.First(a => a.Id == msgs[2]).GetGroupMembersAsync();
                                      foreach (var form in forms)
                                      {
                                          msg = $"{msg}\r\n队伍名:{form.team_name}\r\n队长是否入群:{group.Any(a => a.Id == form.team_tel)}\r\n";
                                      }
                                      await MessageManager.SendGroupMessageAsync(x.GroupId, $"搜索到以下结果:\r\n{msg}");
                                  }
                                  catch
                                  {

                                  }

                              });











                Console.ReadLine();
                bot.Dispose();
            }
            catch
            {
                Console.WriteLine("bot启动失败  ~~~~~     ");
                await Task.Delay(10000);
            }

        }
    }
}

