using Microsoft.AspNetCore.Mvc;

namespace PLMAPS.Controllers
{
    public class Pelicula : Controller
    {
        [HttpGet] //Peliculas Populares
        public ActionResult GetAll()
        {
            ML.Peliculas resultWebApi = new ML.Peliculas();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                var responseTask = client.GetAsync("movie/popular?api_key=401526372779a487928a18f653d2ee6d&language=en-US&page=2");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    var readTask = result.Content.ReadAsAsync<ML.Peliculas>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Peliculas resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Peliculas>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                    }
                    ML.Peliculas pelicula = new ML.Peliculas();
                    pelicula.PeliculasList = resultWebApi.Objects;




                    return View(pelicula);
                }
            }
            return View(resultWebApi);
        }

        [HttpPost] //Añadir a Favoritos
        public ActionResult Form(ML.Peliculas idPelicula)
        {
            if (idPelicula != null)
            {
                ML.Favorite favorite = new ML.Favorite();
                favorite.media_id = idPelicula.id;
                favorite.media_type = "movie";
                favorite.favorite = true;

                ML.Peliculas resultWebApi = new ML.Peliculas();
                resultWebApi.Objects = new List<object>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19729267/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Favorite>("favorite?" +
                        "session_id=968c620908c4ff2f89b1adc20c5dfddb53112139&api_key=401526372779a487928a18f653d2ee6d", favorite);
                    postTask.Wait();

                    var resultUsuario = postTask.Result;
                    if (resultUsuario.IsSuccessStatusCode)
                    {

                        return RedirectToAction("GetAll");

                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al insertar el registro" + " ";
                    }
                    ViewBag.Message = "El resigistro de Usuario a sido agrgado con exito";
                }
            }

            return View();
        }

        [HttpGet]//Peliculas Favoritas
        public ActionResult GetAllFavorites()
        {
            ML.Peliculas resultWebApi = new ML.Peliculas();
            resultWebApi.Objects = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19729267/");

                var responseTask = client.GetAsync("favorite/movies?language=en-US&page=1&session_id=968c620908c4ff2f89b1adc20c5dfddb53112139&api_key=401526372779a487928a18f653d2ee6d&sort_by=created_at.asc");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsStringAsync();
                    var readTask = result.Content.ReadAsAsync<ML.Peliculas>();
                    //var readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();

                    foreach (var resultItem in readTask.Result.results)
                    {
                        ML.Peliculas resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Peliculas>(resultItem.ToString());
                        resultWebApi.Objects.Add(resultItemList);
                    }
                    ML.Peliculas pelicula = new ML.Peliculas();
                    pelicula.PeliculasList = resultWebApi.Objects;




                    return View(pelicula);
                }
            }
            return View(resultWebApi);
        }

        [HttpPost] //Añadir a Favoritos
        public ActionResult FormDelete(ML.Peliculas idPelicula)
        {
            if (idPelicula != null)
            {
                ML.Favorite favorite = new ML.Favorite();
                favorite.media_id = idPelicula.id;
                favorite.media_type = "movie";
                favorite.favorite = false;

                ML.Peliculas resultWebApi = new ML.Peliculas();
                resultWebApi.Objects = new List<object>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.themoviedb.org/3/account/19729267/");

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Favorite>("favorite?session_id=968c620908c4ff2f89b1adc20c5dfddb53112139&api_key=401526372779a487928a18f653d2ee6d", favorite);
                    postTask.Wait();

                    var resultUsuario = postTask.Result;
                    if (resultUsuario.IsSuccessStatusCode)
                    {

                        return RedirectToAction("GetAllFavorites");

                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al insertar el registro" + " ";
                    }
                    ViewBag.Message = "El resigistro de Usuario a sido agrgado con exito";
                }
            }

            return View();
        }

        [HttpGet]
        public  ActionResult GetAllInfoVentas()
        {
            return View();
        }
    }
}


