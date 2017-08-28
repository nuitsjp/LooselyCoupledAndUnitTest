﻿using System;
using System.Device.Location;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using HotPepper.Console.Integrations;
using HotPepper.Console.Integrations.GeoCoordinate;
using HotPepper.Console.Integrations.Gourmet;
using HotPepper.Console.Usecases;
using Newtonsoft.Json;

namespace HotPepper.Console
{
    class Program
    {
        static async Task Main()
        {
            if (Secrets.HotPepperApiKey.StartsWith("Your"))
            {
                System.Console.WriteLine("Secrets.csファイルのHotPepperApiKeyに正しいAPIキーを設定してください。");
                System.Console.WriteLine("キーはつぎのサイトから申請して取得することができます。");
                System.Console.WriteLine("https://webservice.recruit.co.jp/register/index.html");
            }
            else
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<FindRestaurants>().As<IFindRestaurants>();
                builder.RegisterType<GeoCoordinateService>().As<IGeoCoordinateService>();
                builder.RegisterType<GourmetService>().As<IGourmetService>();
                builder.RegisterType<Program>();
                var container = builder.Build();

                var program =  container.Resolve<Program>();
                await program.FindRestaurants();
            }
            System.Console.WriteLine();
            System.Console.WriteLine("Enterキーを押してアプリを終了してください。");
            System.Console.ReadLine();
        }

        private readonly IFindRestaurants _findRestaurants;

        public Program(IFindRestaurants findRestaurants)
        {
            _findRestaurants = findRestaurants;
        }


        private async Task FindRestaurants()
        {
            System.Console.WriteLine("Powered by ホットペッパー Webサービス");
            System.Console.WriteLine();

            // GeoCoordinateWatcherを利用して位置情報を取得する  
            // GeoCoordinateWatcherではStart直後は位置情報が取得できないため
            // 最初のPositionChangedイベントの発生を待つ必要がある
            var timeout = TimeSpan.FromMinutes(1);

            var restaurants = await _findRestaurants.FindNearbyRestaurantsAsync(timeout);

            // 取得結果を出力する
            foreach (var restaurant in restaurants)
            {
                System.Console.WriteLine($"店舗名：{restaurant.Name}\tジャンル：{restaurant.Name}");
            }
        }
    }
}