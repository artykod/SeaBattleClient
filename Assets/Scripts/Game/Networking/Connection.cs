#define USE_TEMP_AUTH_SERVER
#define NET_DEBUG

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Networking
{
    public class Connection : SingletonBehaviour<Connection, Connection>
    {
        private static string GameServerAddress = "http://45.120.149.115:5000";
        private Dictionary<string, string> _requestHeaders = new Dictionary<string, string> { { "Content-Type", "application/json" } };

        public Connection()
        {
            TempAuthServerAddress = GameConfig.Instance.Config.AuthServerUrl;
            GameServerAddress = GameConfig.Instance.Config.GameServerUrl;
        }

#if USE_TEMP_AUTH_SERVER
        public const bool TempLoginEnabled = true;
        private static string TempAuthServerAddress = "http://45.120.149.115:4000";
        public static string TempAuthUserName = "u1@r.ru"; // u1@r.ru; u2@r.ru;
        public static string TempAuthUserPassword = "111"; // 111; 222;

        private class TempAuthResponse
        {
            [JsonProperty("token")]
            public string Token { get; private set; }
        }

        public void SetAuthCookies(string cookies)
        {
            _requestHeaders["Cookie"] = cookies;
        }

        public string GetAuthCookies()
        {
            return _requestHeaders["Cookie"];
        }

        public void Login(Action<Data.Character> onLogin)
        {
            GetInternal<TempAuthResponse>(TempAuthServerAddress, string.Format("/users/login?email={0}&psw={1}", TempAuthUserName, TempAuthUserPassword), (resp, resp_h) => CheckLoginToken(resp.Token, onLogin), null);
        }

        public void Logout()
        {
            _requestHeaders.Remove("Cookie");
        }
#else
        public const bool TempLoginEnabled = false;

        public void Login(Action<Data.Character> onLogin)
        {
            throw new NotImplementedException();
            //CheckLoginToken("<website token>", onLogin);
        }

        public void Logout()
        {
        }
#endif

        private void CheckLoginToken(string token, Action<Data.Character> onLogin)
        {
            GetInternal<Data.Character>(GameServerAddress, "/checkToken/" + token, (resp, resp_h) =>
            {
                var cookies = resp_h["SET-COOKIE"];
                _requestHeaders["Cookie"] = cookies;
                onLogin(resp);

#if NET_DEBUG
                Debug.Log("Auth cookies: " + cookies);
#endif
            }, null);
        }

        public static void Get<T>(string method, Action<T> result, Action<int> onError = null) where T : class
        {
            Instance.GetInternal<T>(GameServerAddress, method, (resp, resp_h) => result(resp), onError);
        }

        public static void Post<R, T>(string method, R data, Action<T> result, Action<int> onError = null) where T : class
        {
            Instance.PostInternal<R, T>(GameServerAddress, method, data, (resp, resp_h) => result(resp), onError);
        }

        private void GetInternal<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : class
        {
            StartCoroutine(GetAsync(host, request, onResponse, onError));
        }

        private IEnumerator GetAsync<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : class
        {
#if NET_DEBUG
            Debug.LogWarning("GET <<< {0}{1}", host, request);
#endif

            using (var www = new WWW(host + request, null, _requestHeaders))
            {
                while (!www.isDone) yield return null;

                if (string.IsNullOrEmpty(www.error))
                {
#if NET_DEBUG
                    Debug.LogWarning("GET >>> {0}", www.text);
#endif
                    onResponse(JsonConvert.DeserializeObject<T>(www.text), www.responseHeaders);
                }
                else
                {
                    ThrowNetworkError(www.error, onError);
                }
            }
        }

        private void PostInternal<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : class
        {
            StartCoroutine(PostAsync(host, request, data, onResponse, onError));
        }

        private IEnumerator PostAsync<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse, Action<int> onError) where T : class
        {
            var json = JsonConvert.SerializeObject(data);
#if NET_DEBUG
            Debug.LogWarning("POST <<< {0}{1}", host, request);
            Debug.Log("POST json = {0}", json);
#endif

            using (var www = new WWW(host + request, System.Text.Encoding.UTF8.GetBytes(json), _requestHeaders))
            {
                while (!www.isDone) yield return null;

                if (string.IsNullOrEmpty(www.error))
                {
#if NET_DEBUG
                    Debug.LogWarning("POST >>> {0}", www.text);
#endif
                    onResponse(JsonConvert.DeserializeObject<T>(www.text), www.responseHeaders);
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
            if (errorCallback != null) errorCallback(ParseErrorCode(error));
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