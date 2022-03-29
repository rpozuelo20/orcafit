using Newtonsoft.Json;
using orcafit.Helpers;
using orcafit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace orcafit.Services
{
    public class ServiceUsuarios
    {
        //  Sentencias comunes en los services   ⌄⌄⌄
        private HelperTokenCallApi helperApi;
        public ServiceUsuarios(HelperTokenCallApi helperApi)
        {
            this.helperApi = helperApi;
        }
        //  Sentencias comunes en los services   ˄˄˄
    }
}
