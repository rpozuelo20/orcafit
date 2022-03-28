using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orcafit.Models;
using orcafit.Models.ViewModels;
using orcafit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace orcafit.Controllers
{
    public class RutinasController : Controller
    {
        //  Sentencias de inyeccion     ⌄⌄⌄
        private ServiceRutinas service;
        public RutinasController(ServiceRutinas service)
        {
            this.service = service;
        }
        //  Sentencias de inyeccion     ˄˄˄
    }
}