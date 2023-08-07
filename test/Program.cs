// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Discord;

namespace Splatoo2ConsoleApp
{
    class Program
    {
        public static DiscordSocketClient client;
        public static CommandService commands;
        public static IServiceProvider services;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        /// <summary>
        /// 起動時
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent |
                             GatewayIntents.GuildMessages
            };
            client = new DiscordSocketClient(config);
            commands = new CommandService();
            services = new ServiceCollection().BuildServiceProvider();
            client.MessageReceived += CommandRecieved;

            client.Log += Log;
            string? token = ConfigurationManager.AppSettings["token"];
            await commands.AddModulesAsync( Assembly.GetEntryAssembly(),services);
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgParam"></param>
        /// <returns></returns>
        private async Task CommandRecieved(SocketMessage? messageParam)
        {
            var message = messageParam as SocketUserMessage;
            Console.WriteLine($"{message.Channel.Name} {message.Author.Username}:{message.Content}");

            if (message == null) return;
            // user or bot
            if (message.Author.IsBot) return;

            var argPos = 0;

            // 「$」で判定
            if (!(message.HasCharPrefix('$', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;

            var context = new CommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, services);

            if (!result.IsSuccess) { await context.Channel.SendMessageAsync(result.ErrorReason); }

        }

        /// <summary>
        /// コンソール表示処理
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}