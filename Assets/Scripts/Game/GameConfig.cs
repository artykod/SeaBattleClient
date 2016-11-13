using UnityEngine;
using Newtonsoft.Json;

public class GameConfig : Singleton<GameConfig, GameConfig>
{
    public class ConfigJson
    {
        [JsonProperty]
        public string AuthServerUrl { get; private set; }
        [JsonProperty]
        public string GameServerUrl { get; private set; }
        [JsonProperty]
        public string AvatarsServerUrl { get; private set; }
        [JsonProperty]
        public bool DebugMode { get; private set; }
        [JsonProperty]
        public string BuyGoldUrl { get; private set; }
        [JsonProperty]
        public string BuySilverUrl { get; private set; }
        [JsonProperty]
        public string FacebookUrl { get; private set; }
        [JsonProperty]
        public string VkUrl { get; private set; }
        [JsonProperty]
        public string OdnoklassnikiUrl { get; private set; }
        [JsonProperty]
        public string MyMailUrl { get; private set; }
        [JsonProperty]
        public string TwitterUrl { get; private set; }
    }

    public ConfigJson Config { get; private set; }

    public GameConfig()
    {
        var config = Resources.Load<TextAsset>("Configs/GameConfig");
        if (config != null)
        {
            try
            {
                Config = JsonConvert.DeserializeObject<ConfigJson>(config.text);
            }
            catch
            {
                Debug.LogError("Cannot deserialize game config.");
                Config = new ConfigJson();
            }
        }
        else
        {
            Debug.LogError("Game config not found in Configs/GameConfig.json");
            Config = new ConfigJson();
        }
    }

    public void Load()
    {
    }
}
