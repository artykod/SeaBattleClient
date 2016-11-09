#define USE_TEMP_AUTH_SERVER

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Networking
{
    public class Connection : SingletonBehaviour<Connection, Connection>
    {
        private const string GameServerAddress = "http://45.120.149.115:5000";
        private Dictionary<string, string> _requestHeaders = new Dictionary<string, string> { { "Content-Type", "application/json" } };

#if USE_TEMP_AUTH_SERVER
        private const string TempAuthServerAddress = "http://45.120.149.115:4000";
        private const string TempAuthUserName = "u1@r.ru"; // u1@r.ru; u2@r.ru;
        private const string TempAuthUserPassword = "111"; // 111; 222;

        private class TempAuthResponse
        {
            [JsonProperty("token")]
            public string Token { get; private set; }
        }

        public void SetAuthCookies(string cookies)
        {
            _requestHeaders["Cookie"] = cookies;
        }

        public void Login(Action<Data.Character> onLogin)
        {
            GetInternal<TempAuthResponse>(TempAuthServerAddress, string.Format("/users/login?email={0}&psw={1}", TempAuthUserName, TempAuthUserPassword), (resp, resp_h) => CheckLoginToken(resp.Token, onLogin));
        }
#else
        public void Login(Action<Data.Character> onLogin)
        {
            throw new NotImplementedException();
            //CheckLoginToken("<website token>", onLogin);
        }
#endif

        private void CheckLoginToken(string token, Action<Data.Character> onLogin)
        {
            GetInternal<Data.Character>(GameServerAddress, "/checkToken/" + token, (resp, resp_h) =>
            {
                var cookies = resp_h["SET-COOKIE"];
                Debug.Log("Auth cookies: " + cookies);
                _requestHeaders["Cookie"] = cookies;
                onLogin(resp);
            });
        }

        public static void Get<T>(string method, Action<T> result) where T : class
        {
            Instance.GetInternal<T>(GameServerAddress, method, (resp, resp_h) => result(resp));
        }

        public static void Post<R, T>(string method, R data, Action<T> result) where T : class
        {
            Instance.PostInternal<R, T>(GameServerAddress, method, data, (resp, resp_h) => result(resp));
        }

        private void GetInternal<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse) where T : class
        {
            StartCoroutine(GetAsync(host, request, onResponse));
        }

        private IEnumerator GetAsync<T>(string host, string request, Action<T, Dictionary<string, string>> onResponse) where T : class
        {
            Debug.LogWarning("GET <<< {0}{1}", host, request);

            using (var www = new WWW(host + request, null, _requestHeaders))
            {
                while (!www.isDone)
                {
                    yield return null;
                }

                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning("GET >>> {0}", www.text);
                    onResponse(JsonConvert.DeserializeObject<T>(www.text), www.responseHeaders);
                }
                else
                {
                    Debug.LogError("{0}", www.error);
                }
            }
        }

        private void PostInternal<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse) where T : class
        {
            StartCoroutine(PostAsync(host, request, data, onResponse));
        }

        private IEnumerator PostAsync<R, T>(string host, string request, R data, Action<T, Dictionary<string, string>> onResponse) where T : class
        {
            var json = JsonConvert.SerializeObject(data);
            Debug.LogWarning("POST <<< {0}{1}", host, request);
            Debug.Log("POST json = {0}", json);

            using (var www = new WWW(host + request, System.Text.Encoding.UTF8.GetBytes(json), _requestHeaders))
            {
                while (!www.isDone)
                {
                    yield return null;
                }

                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.LogWarning("POST >>> {0}", www.text);
                    onResponse(JsonConvert.DeserializeObject<T>(www.text), www.responseHeaders);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }
    }
}