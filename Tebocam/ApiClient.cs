using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

//order of processing
//get token
//loop
//if token is still valid get commands
//if token is not valid get token then get commands
//update status of particular command
//end loop

namespace TeboCam
{

    public static class Token
    {
        public static TokenResponse TokenForSession;
        public static List<CommandQueueElement> commands = new List<CommandQueueElement>();
    }

    public static class AuthenticationSuccess
    {
        public static bool Success;
    }

    public class RenewTokenObj
    {
        public string userName { get; set; }
        public string password { get; set; }
        public Boolean endpoints { get; set; }
        public Boolean commands { get; set; }
        public string application { get; set; }
        public string instance { get; set; }
        public Boolean forceRenewal { get; set; }
    }

    public class EndpointResponse
    {
        public string endpointName { get; set; }
        public string url { get; set; }
    }

    public class CommandAvailable
    {
        public string display { get; set; }
        public string command { get; set; }
        public bool parms { get; set; }
    }

    public class ClearCommandObj
    {
        public string token { get; set; }
        public List<int> commandIdList { get; set; }
        public List<string> commandGuidList = new List<string>();
        public bool setAsRun { get; set; }
    }

    public class TokenResponse
    {
        public string token { get; set; }
        public string validFrom { get; set; }
        public string validTo { get; set; }
        public string currentDateTime { get; set; }
        public List<EndpointResponse> endpoints { get; set; }
        public List<CommandAvailable> commands { get; set; }
    }

    public class TokenModel
    {
        public int idToken { get; set; }
        public int idUser { get; set; }
        public string application { get; set; }
        public string instance { get; set; }
        public string token { get; set; }
        public DateTime? validFrom { get; set; }
        public DateTime? validTo { get; set; }
    }

    public class InstanceSummaryInfoModel
    {
        public DateTime? updatedfromAppdtm { get; set; }
        public string currentState { get; set; }
    }

    public class InstanceSummaryUpdateModel
    {
        public string token { get; set; }
        public string currentState { get; set; }
    }

    public class ResponseObject
    {
        public string token { get; set; }
        public string responseObject { get; set; }
    }

    public class GraphModel
    {
        public string date { get; set; }
        public string location { get; set; }
        public string filename { get; set; }
    }

    public class GraphResponseModel
    {
        public string token { get; set; }
        public List<GraphModel> graphs { get; set; }
    }

    public class CommandResultModel
    {
        public string token { get; set; }
        public string commandGuid { get; set; }
        public object commandResult { get; set; }
    }

    public class ResponseWrapper
    {
        public bool TokenIsValid { get; set; }
        public TokenModel Token { get; set; }
        public object Package { get; set; }

        public ResponseWrapper WrapResponse<T>(T response)
        {
            Package = response;
            return this;
        }
    }

    public class CommandQueueElement
    {
        public int idCommandQueue { get; set; }
        public string commandGuid { get; set; }
        public int idUser { get; set; }
        public string application { get; set; }
        public string instance { get; set; }
        public string commandToRun { get; set; }
        public string parms { get; set; }
        public DateTime? requestedRunDtm { get; set; }
        public DateTime? commandRunDtm { get; set; }
        public DateTime? addedToQueueDtm { get; set; }
        public bool emailConfirmation { get; set; }
    }

    public static class API
    {
        static HttpClient client = new HttpClient();
        //the RemoteURI and LocalURI values are overwritten by xml congig values
        public static string RemoteURI;//"http://www.teboweb.com";
        public static string LocalURI;//"http://localhost:60125";
        public static bool UseRemoteURI = true;

        private static string BaseUri()
        {
            return UseRemoteURI ? RemoteURI : LocalURI;
        }

        //RenewTokenObj requestObj = new RenewTokenObj() { endpoints = true, password = "", userName = "" };

        public static string EndpointFormat(string endpoint)
        {
            return $"{BaseUri()}{endpoint}";
        }

        public static string HealthCheck(string endpoint)
        {
            var result = GetHealth(endpoint);
            return result.Result;
        }

        public static async Task<string> HealthCheckAsync(string endpoint)
        {
            var result = await GetHealth(endpoint);
            return result;
        }

        public static async Task<string> GetHealth(string healthEndpoint)
        {
            string result = string.Empty;
            string path = $"{healthEndpoint}";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<string>();
            }
            return result;
        }

        public static bool LogOn(string endpoint, string username, string password, string instance, bool renew)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                return false;
            }
            RenewTokenObj renewObj = new RenewTokenObj()
            {
                userName = username,
                password = password,
                application = "tebocam",
                commands = false,
                endpoints = true,
                instance = instance,
                forceRenewal = renew
            };

            Token.TokenForSession = GetTokenAsync(endpoint, renewObj).GetAwaiter().GetResult();
            return Token.TokenForSession.token != null;
        }
        public static async Task<TokenResponse> GetTokenAsync(string endpoint, RenewTokenObj renewTokenObj)
        {
            TokenResponse token = new TokenResponse();
            HttpResponseMessage response = await client.PostAsJsonAsync(endpoint, renewTokenObj);
            if (response.IsSuccessStatusCode)
            {
                token = await response.Content.ReadAsAsync<TokenResponse>();
            }

            return token;
        }

        public class Result
        {
            public string ResultType;
            public object Data;
        }

        public static void UpdateCommandResult(string commandType, object result, string guid)
        {

            var jsonObject = JsonConvert.SerializeObject(
                new Result()
                {
                    ResultType = commandType,
                    Data = result
                }
            );

            var commandResult = new CommandResultModel
            {
                token = Token.TokenForSession.token,
                commandGuid = guid,
                commandResult = jsonObject
            };
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "updateCommandResult").url);
            ResponseWrapper wrappedInstance = UpdateCommandResultAsync(endpoint, commandResult).GetAwaiter().GetResult();
            AuthenticationSuccess.Success = wrappedInstance.TokenIsValid;
        }
        public static async Task<ResponseWrapper> UpdateCommandResultAsync(string commandResultEndpoint, CommandResultModel commandResultModel)
        {
            ResponseWrapper commandResultWrapper = new ResponseWrapper();
            HttpResponseMessage response = await client.PostAsJsonAsync(commandResultEndpoint, commandResultModel);
            if (response.IsSuccessStatusCode)
            {
                commandResultWrapper = await response.Content.ReadAsAsync<ResponseWrapper>();
            }
            return commandResultWrapper;
        }

        public static InstanceSummaryInfoModel RetrieveInstance()
        {
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "getInstance").url);
            ResponseWrapper wrappedInstance = GetInstanceSummaryAsync(Token.TokenForSession.token, endpoint).GetAwaiter().GetResult();
            AuthenticationSuccess.Success = wrappedInstance.TokenIsValid;
            return new InstanceSummaryInfoModel();
            //return (InstanceSummaryInfoModel)wrappedInstance.Package;
        }
        public static async Task<ResponseWrapper> GetInstanceSummaryAsync(string token, string instanceSummaryEndpoint)
        {
            ResponseWrapper instanceSummary = new ResponseWrapper();
            string path = $"{instanceSummaryEndpoint}?token={token}";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                instanceSummary = await response.Content.ReadAsAsync<ResponseWrapper>();
            }
            return instanceSummary;
        }

        public static void UpdateInstance(string status)
        {
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "updateInstance").url);

            InstanceSummaryUpdateModel summary = new InstanceSummaryUpdateModel
            {
                currentState = status,
                token = Token.TokenForSession.token
            };
            ResponseWrapper wrappedInstance = UpdateInstanceSummaryAsync(endpoint, summary).GetAwaiter().GetResult();
            AuthenticationSuccess.Success = wrappedInstance.TokenIsValid;
        }
        public static async Task<ResponseWrapper> UpdateInstanceSummaryAsync(string instanceSummaryEndpoint, InstanceSummaryUpdateModel instanceSummaryUpdateModel)
        {
            ResponseWrapper instanceSummary = new ResponseWrapper();
            //client.Timeout = new TimeSpan(0,0,0,0,5000);
            HttpResponseMessage response = await client.PostAsJsonAsync(instanceSummaryEndpoint, instanceSummaryUpdateModel);
            if (response.IsSuccessStatusCode)
            {
                instanceSummary = await response.Content.ReadAsAsync<ResponseWrapper>();
            }
            return instanceSummary;
        }

        //public static void ReturnObject(object objetToSerialise)
        //{
        //    string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "updateInstance").url);

        //    var responseObject = new ResponseObject
        //    {
        //        responseObject = JsonConvert.SerializeObject(objetToSerialise),
        //        token = Token.TokenForSession.token
        //    };
        //    ResponseWrapper wrappedInstance = ReturnObjectAsync(endpoint, responseObject).GetAwaiter().GetResult();
        //    AuthenticationSuccess.Success = wrappedInstance.TokenIsValid;
        //}

        //public static async Task<ResponseWrapper> ReturnObjectAsync(string instanceSummaryEndpoint, ResponseObject responseObject)
        //{
        //    ResponseWrapper wrappedResponse = new ResponseWrapper();
        //    HttpResponseMessage response = await client.PostAsJsonAsync(instanceSummaryEndpoint, responseObject);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        wrappedResponse = await response.Content.ReadAsAsync<ResponseWrapper>();
        //    }
        //    return wrappedResponse;
        //}

        public static bool RetrieveCommands(int number = 0)
        {
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "getCommand").url);
            ResponseWrapper wrappedCommands = GetCommandsAsync(Token.TokenForSession.token, number, endpoint).GetAwaiter().GetResult();
            List<CommandQueueElement> commands = JsonConvert.DeserializeObject<List<CommandQueueElement>>(wrappedCommands.Package.ToString());
            Token.commands.AddRange(commands);
            AuthenticationSuccess.Success = wrappedCommands.TokenIsValid;
            return AuthenticationSuccess.Success;
        }
        public static async Task<ResponseWrapper> GetCommandsAsync(string token, int number, string getCommandsEndpoint)
        {
            string path = number > 0 ? $"{getCommandsEndpoint}?token={token}&number={number}" : $"{getCommandsEndpoint}?token={token}";
            ResponseWrapper commands = new ResponseWrapper();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode) commands = await response.Content.ReadAsAsync<ResponseWrapper>();
            return commands;
        }

        public static void ClearCommands(List<int> ids)
        {
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "clearCommand").url);
            ClearCommandObj clear = new ClearCommandObj
            {
                token = Token.TokenForSession.token,
                commandIdList = ids,
                setAsRun = true
            };
            ResponseWrapper commandsCleared = ClearCommandAsync(clear, endpoint).GetAwaiter().GetResult();
            AuthenticationSuccess.Success = commandsCleared.TokenIsValid;
        }

        public static void ClearCommands(List<String> guids)
        {
            string endpoint = EndpointFormat(Token.TokenForSession.endpoints.First(x => x.endpointName == "clearCommand").url);
            ClearCommandObj clear = new ClearCommandObj
            {
                token = Token.TokenForSession.token,
                commandGuidList = guids,
                setAsRun = true
            };
            ResponseWrapper commandsCleared = ClearCommandAsync(clear, endpoint).GetAwaiter().GetResult();
            AuthenticationSuccess.Success = commandsCleared.TokenIsValid;
        }

        public static async Task<ResponseWrapper> ClearCommandAsync(ClearCommandObj clearCommandObj, string endpoint)
        {
            ResponseWrapper cleared = new ResponseWrapper();
            HttpResponseMessage response = await client.PostAsJsonAsync(endpoint, clearCommandObj);
            if (response.IsSuccessStatusCode) cleared = await response.Content.ReadAsAsync<ResponseWrapper>();
            return cleared;
        }
    }
}
