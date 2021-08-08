using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitHubApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GitRepositoriesController : Controller
    {
      
        [HttpPost]
        public Object Post()
        {
            string response = null;
            try
            {
                 response = (string)getDataGitHub();

            }
            catch (Exception ex) {
                
            }
            

            if (response != null)
            {
                return response;
            }
            else {
                return JObject.Parse("{status: error, message: git API unavailable}");
            }
            
        }

        public string getDataGitHub() {
            string json = null;
            RestClient client = new RestClient("https://api.github.com/orgs/takenet/repos?sort=created&per_page=5&direction=asc");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if((int)response.StatusCode == 200)
            {
                JArray jarray = JArray.Parse(response.Content);
                //return jarray[jarray.Count - 1].ToString();

                List<Repo> filteredRepos = new List<Repo>();
                int sizeResults = jarray.Count;
                for (int i=0;i< sizeResults; i++) {

                    filteredRepos.Add(new Repo(jarray[i]["owner"]["avatar_url"].ToString(), jarray[i]["full_name"].ToString(), jarray[i]["description"].ToString()));
                }

          
                return JsonConvert.SerializeObject(filteredRepos); 
            }
           

            return json;
        }
    }

    class Repo
    {
        public Repo(string avatarUrl, string fullName, string description)
        {
            this.avatarUrl = avatarUrl;
            this.fullName = fullName;
            this.description = description;

        }

        public string  avatarUrl { get; set; }
        public string fullName { get; set; }
        public string description { get; set; }
        
    }
}