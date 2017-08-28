﻿using System.Net.Http;
using System.Threading.Tasks;
using HotPepper.Console.Integrations.GeoCoordinate;
using Newtonsoft.Json;

namespace HotPepper.Console.Integrations.Gourmet
{
    public class GourmetService : IGourmetService
    {
        private const string GourmetSearchApiEndpoint = "https://webservice.recruit.co.jp/hotpepper/gourmet/v1/";

        public async Task<GourmetSearchResult> SearchGourmetAsync(Position position)
        {
            GourmetSearchResult result;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(
                    $"{GourmetSearchApiEndpoint}" +
                    $"?key={Secrets.HotPepperApiKey}" +
                    $"&lat={position.Latitude}" +
                    $"&lng={position.Longitude}" +
                    $"&format=json&type=lite");
                result = JsonConvert.DeserializeObject<GourmetSearchResult>(json);
            }
            return result;
        }
    }
}