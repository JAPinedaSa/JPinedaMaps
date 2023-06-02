using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll").ToList();
                    
                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var obj in query)
                        {
                            ML.Cine cine = new ML.Cine();
                            cine.IdCine = obj.IdCine;
                            cine.Nombre = obj.Nombre;
                            cine.Direccion = obj.Direcccion;
                            cine.Ventas = obj.Ventas.Value;
                          
                           
                            cine.Zona = new ML.Zona();
                            cine.Zona.IdZona = obj.IdZona.Value;
                            cine.Zona.Nombre = obj.Zona;
                            
                            result.Objects.Add(cine);

                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = "ocurrio un error"+ ex.Message;

            }
            return result;

        }

        //public static ML.Result GetVentasTotales()
        //{
        //    ML.Result result = new ML.Result();
        //    try
        //    {
        //        using (DL.CineContext context = new DL.CineContext())
        //        {
        //            var RowsAfected = context.Cines.FromSqlRaw("GetVentas").AsEnumerable().FirstOrDefault();

        //            result.Object = new object();

        //            if (RowsAfected != null)
        //            {
        //                ML.Cine cine = new ML.Cine();

        //                cine.VentasTotaless = new ML.VentasEstadisticas();
        //                cine.VentasTotaless.VentasTotales = RowsAfected.VentasTotales;
                        

        //                result.Correct = true;
        //            }
        //            else
        //            {
        //                result.Correct = false;
        //                result.ErrorMessage = "Ocurrió un error al obtener el registros en la tabla Cine";
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result.Correct = false;
        //        result.ErrorMessage = ex.Message;
        //    }
        //    return result;
        //}

        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    int RowsAfected = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Ventas}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al ingresar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }


        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    int RowsAfected = context.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine}, '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Ventas}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al Actualizar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result Delete(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    int RowsAfected = context.Database.ExecuteSqlRaw($"CineDelete {idCine}");

                    if (RowsAfected > 0)
                    {
                        result.Correct = true; ;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrio un error al Elimar el cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public static ML.Result GetById(int idCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext())
                {
                    var RowsAfected = context.Cines.FromSqlRaw($"CineGetById {idCine}").AsEnumerable().FirstOrDefault();

                    result.Object = new object();

                    if (RowsAfected != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.IdCine = RowsAfected.IdCine;
                        cine.Nombre = RowsAfected.Nombre;
                        cine.Direccion = RowsAfected.Direcccion;
                        cine.Ventas = RowsAfected.Ventas.Value;
                        cine.Zona = new ML.Zona();
                        cine.Zona.IdZona = RowsAfected.IdZona.Value;
                        cine.Zona.Nombre = RowsAfected.Zona;
                        result.Object = cine;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Ocurrió un error al obtener el registros en la tabla Cine";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }


    }
}
