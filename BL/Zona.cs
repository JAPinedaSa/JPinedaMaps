using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Zona
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    var query = context.Zonas.FromSqlRaw("ZonaGetAll");
                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var obj in query)
                        {
                            ML.Zona zona = new ML.Zona();
                           
                            zona.IdZona = obj.IdZona;
                            zona.Nombre = obj.Nombre;

                            result.Objects.Add(zona);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error" + ex.Message;
            }
            return result;
        }

    }
}
