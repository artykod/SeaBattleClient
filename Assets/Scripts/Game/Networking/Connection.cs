using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using SimpleJSON;

namespace Networking
{
    public class TempAuthResponse : BaseData
    {
        public string Token { get; private set; }
        public override void FromJson(JSONNode node) { Token = node["token"]; }
        protected override void FillJson(JSONNode node) { node["token"] = Token; }
    }

    public class Connection : SingletonBehaviour<Connection, Connection>
    {
        public static string TempAuthUserName;
        public static string TempAuthUserPassword;
        private static string GameServerAddress;
        private Dictionary<string, string> _requestHeaders = new Dictionary<string, string> { { "Content-Type", "application/json" } };

        public event Action<int> OnErrorReceived;

        private void Awake()
        {
            GameServerAddress = GameConfig.Instance.Config.GameServerUrl;
        }

        public void ForceDisconnect(int code = -1)
        {
            if (OnErrorReceived != null) OnErrorReceived(code);
        }

        public void DisconnectAll()
        {
            _requestHeaders.Remove("TokenAuth");
            StopAllCoroutines();
        }

        public void TempLogin(Action<CharacterData, string> onLogin)
        {
            GetInternal<TempAuthResponse>(GameConfig.Instance.Config.AuthServerUrl, string.Format("/users/login?email={0}&psw={1}", TempAuthUserName, TempAuthUserPassword), (resp, resp_h) => CheckLoginToken(resp.Token, onLogin), null);
        }

        public void TempLogout()
        {
            _requestHeaders.Remove("TokenAuth");
        }

        public void Login(Action<CharacterData, string> onLogin)
        {
            ForceDisconnect(-2);
            //CheckLoginToken("<website token>", onLogin);
        }

        private void CheckLoginToken(string token, Action<CharacterData, string> onLogin)
        {
            GetInternal<CharacterData>(GameServerAddress, "/checkToken/" + token, (resp, resp_h) =>
            {
                _requestHeaders["TokenAuth"] = resp.Token;
                onLogin(resp, resp.Token);
            }, null);
        }

        public static void Get<T>(string method, Action<T> result, Action<int> onError = null) where T : BaseData
        {
            Instance.GetInternal<T>(GameServerAddress, method, (resp, resp_h) => result(resp), onError);
        }

        public static void Post<R, T>(string method, R data, Action<T> result, Action<int> onError = null) where T : BaseData where R : BaseData
        {
            Instance.PostInternal<R, T>(GameServerAddress, method, data, (resp, resp_h) => result(resp), onError);
        }

        private void GetInternal<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : BaseData
        {
            StartCoroutine(GetAsync(host, request, onResponse, onError));
        }

        private IEnumerator GetAsync<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : BaseData
        {
            Debug.LogWarning("GET <<< {0}{1}", host, request);

            using (var www = new WWW(host + request, null, _requestHeaders))
            {
                while (!www.isDone) yield return null;

                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning("GET >>> {0}", www.text);

                    var dataJson = Activator.CreateInstance<T>();
                    dataJson.FromJson(JSON.Parse(www.text));
                    onResponse(dataJson, www.responseHeaders);
                }
                else
                {
                    ThrowNetworkError(www.error, onError);
                }
            }
        }

        private void PostInternal<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : BaseData where R : BaseData
        {
            StartCoroutine(PostAsync(host, request, data, onResponse, onError));
        }

        private IEnumerator PostAsync<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : BaseData where R : BaseData
        {
            var json = data.ToJson().ToString();

            Debug.LogWarning("POST <<< {0}{1}", host, request);
            Debug.Log("POST json = {0}", json);

            using (var www = new WWW(host + request, System.Text.Encoding.UTF8.GetBytes(json), _requestHeaders))
            {
                while (!www.isDone) yield return null;

                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning("POST >>> {0}", www.text);

                    var dataJson = Activator.CreateInstance<T>();
                    dataJson.FromJson(JSON.Parse(www.text));
                    onResponse(dataJson, www.responseHeaders);
                }
                else
                {
                    ThrowNetworkError(www.error, onError);
                }
            }
        }

        private void ThrowNetworkError(string error, Action<int> errorCallback)
        {
            Debug.LogError("Network error: " + error);

            var errorCode = ParseErrorCode(error);
            if (errorCallback != null) errorCallback(errorCode);
            if (OnErrorReceived != null) OnErrorReceived(errorCode);
        }

        private int ParseErrorCode(string error)
        {
            if (error.Contains("400")) return 400;
            if (error.Contains("403")) return 403;
            if (error.Contains("404")) return 404;
            if (error.Contains("409")) return 409;
            if (error.Contains("500")) return 500;
            return -1;
        }
    }
}