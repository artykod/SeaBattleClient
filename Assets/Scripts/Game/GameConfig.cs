using UnityEngine;
using Data;
using SimpleJSON;

public class GameConfig : Singleton<GameConfig, GameConfig>
{
    public class ConfigJson : BaseData
    {
        public bool DebugMode { get; private set; }
        public bool TempLogin { get; private set; }
        public string AuthServerUrl { get; private set; }
        public string GameServerUrl { get; private set; }
        public string AvatarsServerUrl { get; private set; }
        public string BuyGoldUrl { get; private set; }
        public string BuySilverUrl { get; private set; }
        public string FacebookUrl { get; private set; }
        public string VkUrl { get; private set; }
        public string OdnoklassnikiUrl { get; private set; }
        public string MyMailUrl { get; private set; }
        public string TwitterUrl { get; private set; }

        public ConfigJson()
        {
            AuthServerUrl = "http://45.120.149.115:4000";
            GameServerUrl = "http://45.120.149.115:5000";
            TempLogin = true;
            DebugMode = true;
        }

        public override void FromJson(JSONNode node)
        {
            AuthServerUrl = node["AuthServerUrl"];
            GameServerUrl = node["GameServerUrl"];
            AvatarsServerUrl = node["AvatarsServerUrl"];
            DebugMode = node["DebugMode"].AsBool;
            TempLogin = node["TempLogin"].AsBool;
            BuyGoldUrl = node["BuyGoldUrl"];
            BuySilverUrl = node["BuySilverUrl"];
            FacebookUrl = node["FacebookUrl"];
            VkUrl = node["VkUrl"];
            OdnoklassnikiUrl = node["OdnoklassnikiUrl"];
            MyMailUrl = node["MyMailUrl"];
            TwitterUrl = node["TwitterUrl"];
        }

        protected override void FillJson(JSONNode node)
        {
            node["AuthServerUrl"] = AuthServerUrl;
            node["GameServerUrl"] = GameServerUrl;
            node["AvatarsServerUrl"] = AvatarsServerUrl;
            node["DebugMode"].AsBool = DebugMode;
            node["TempLogin"].AsBool = TempLogin;
            node["BuyGoldUrl"] = BuyGoldUrl;
            node["BuySilverUrl"] = BuySilverUrl;
            node["FacebookUrl"] = FacebookUrl;
            node["VkUrl"] = VkUrl;
            node["OdnoklassnikiUrl"] = OdnoklassnikiUrl;
            node["MyMailUrl"] = MyMailUrl;
            node["TwitterUrl"] = TwitterUrl;
        }
    }

    public ConfigJson Config { get; private set; }

    public GameConfig()
    {
        Config = new ConfigJson();

        var config = Resources.Load<TextAsset>("Configs/GameConfig");
        if (config != null)
        {
            try
            {
                Config.FromJson(JSON.Parse(config.text));
            }
            catch
            {
                Debug.LogError("Cannot deserialize game config.");
            }
        }
        else
        {
            Debug.LogError("Game config not found in Configs/GameConfig.json");
        }
    }
}
