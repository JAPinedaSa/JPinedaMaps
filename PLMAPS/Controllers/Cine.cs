using Microsoft.AspNetCore.Mvc;

namespace PLMAPS.Controllers
{
    public class Cine : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            cine.Cines = new List<object>();
            ML.Result result = new ML.Result();
            result = BL.Cine.GetAll();
            if (result.Correct)
            {
                cine.Cines = result.Objects;
            }
            return View(cine);
        }
        [HttpGet]
        public IActionResult Form(int? idCine)
        {
            ML.Cine cine = new ML.Cine();
            ML.Result result = new ML.Result();
            cine.Zona = new ML.Zona();
            cine.Zona.Zonas = new List<object>();
            result = BL.Zona.GetAll();
            if (result.Correct)
            {

                cine.Zona.Zonas = result.Objects;
            }
            if (idCine == null)
            {

                return View(cine);
            }
            else
            {
                ML.Result resultGetById = new ML.Result();
                resultGetById = BL.Cine.GetById(idCine.Value);
                if (resultGetById.Correct)
                {
                    cine = (ML.Cine)resultGetById.Object;
                    cine.Zona.Zonas = result.Objects;
                }
                return View(cine);
            }
        }

        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            if (cine.IdCine == 0)
            {
                result = BL.Cine.Add(cine);
                if (result.Correct)
                {
                    ViewBag.Message = "El cine Se Agrego Correctamente";
                }
                else
                {
                    ViewBag.Message = "Ocurrio Un Error Al Agregar El Cine" + result.ErrorMessage;
                }
            }
            else
            {
                result = BL.Cine.Update(cine);
                if (result.Correct)
                {
                    ViewBag.Message = "El cine Se Actualizo Correctamente";
                }
                else
                {
                    ViewBag.Message = "Ocurrio Un Error Al Actualizar El Cine" + result.ErrorMessage;
                }
            }
            return View("Modal");
        }

        public IActionResult Delete(int idCine)
        {
            ML.Result result = new ML.Result();
            result = BL.Cine.Delete(idCine);
            if (result.Correct)
            {
                ViewBag.Message = "El cine Se Elimino Correctamente";
            }
            else
            {
                ViewBag.Message = "Ocurrio Un Error Al Elimar El Cine" + result.ErrorMessage;
            }
            return View("Modal");
        }

        //public ActionResult GetAllInfoVentas()
        //{
        //    //ML.Cine cineVentas = new ML.Cine();
        //    //cineVentas.Cines = new List<object>();
        //    //ML.Result resultCines = BL.Cine.GetAll();
        //    //ML.Result resultVentaTotal = BL.Cine.GetVentasTotales();
        //    //decimal VentaNorte = 0;

        //    //if (resultVentaTotal.Correct)
        //    //{
        //    //    cineVentas.VentasTotaless.VentasList = resultVentaTotal.Objects;
        //    //    cineVentas.Cines = resultCines.Objects;
        //    //    foreach (var item in cineVentas.Cines)
        //    //    {
        //    //        if (cineVentas.Zona.IdZona == 1)
        //    //        {
        //    //            VentaNorte =+ cineVentas.Ventas;
        //    //        }
        //    //    }
        //    //}
        //    return View();
        //}
        [HttpGet]
        public ActionResult GetAllInfoVentas()
        {
            ML.Cine cineVentas = new ML.Cine();
            ML.Cine cineVentasZones = new ML.Cine();
            cineVentasZones.VentasTotaless = new ML.VentasEstadisticas();
            cineVentas.Cines = new List<object>();
            decimal ventaTotal = 0;
            decimal ventaNorte =0;
            decimal ventaSur = 0;
            decimal ventaEste = 0;
            decimal ventaOeste = 0;
            ML.Result resultCines = BL.Cine.GetAll();
            if (resultCines.Correct)
            {
                cineVentas.Cines = resultCines.Objects;
                foreach (ML.Cine cine in cineVentas.Cines)
                {

                    ventaTotal = (ventaTotal + cine.Ventas);
                }

                foreach (ML.Cine cineVentaZona in cineVentas.Cines)
                {
                    if (cineVentaZona.Zona.IdZona == 1)
                    {
                        ventaNorte= (ventaNorte + cineVentaZona.Ventas);
                        //cineVentasZones.VentasTotaless.VentasNorte = ventaNorte;
                    }
                    else
                    {
                        if (cineVentaZona.Zona.IdZona == 2)
                        {
                            ventaSur = (ventaSur + cineVentaZona.Ventas);
                            
                        }
                        else
                        {
                            if (cineVentaZona.Zona.IdZona == 3)
                            {
                                ventaEste = (ventaEste + cineVentaZona.Ventas);
                                
                            }
                            else
                            {
                                ventaOeste = (ventaOeste + cineVentaZona.Ventas);
                               
                            }
                        }
                    }


                }
                cineVentasZones.VentasTotaless.VentasNorte = ((ventaNorte/ventaTotal)*100);
                cineVentasZones.VentasTotaless.VentasSur = ((ventaSur / ventaTotal) * 100);
                cineVentasZones.VentasTotaless.VentasEste = ((ventaEste / ventaTotal) * 100);
                cineVentasZones.VentasTotaless.VentasOeste = ((ventaOeste / ventaTotal) * 100);

                

            }




            return View(cineVentasZones);
        }

    }
}
