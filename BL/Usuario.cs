using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetByUserName(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {

                using (DL.CineContext context = new DL.CineContext())
                {

                    var query = context.Usuarios.FromSqlRaw($"UsuaroGetByUserName '{UserName}'").AsEnumerable().FirstOrDefault();

                    result.Object = new List<object>();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.UserName = query.UserName;
                        usuario.Contraseña = query.Contraseña;

                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Algo paso :(";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result GetByEmail(string Email)
        {
            ML.Result result = new ML.Result();
            try
            {

                using (DL.CineContext context = new DL.CineContext())
                {

                    var query = context.Usuarios.FromSqlRaw($"UsuaroGetByUserEmail '{Email}'").AsEnumerable().FirstOrDefault();

                    result.Object = new List<object>();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.UserName = query.Email;
                        

                        result.Object = usuario;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Algo paso :(";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.CineContext context = new DL.CineContext
                    ())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.UserName}', '{usuario.Email}', @Password", new SqlParameter("@Password", usuario.Contraseña));

                    if (query >= 1)
                    {
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
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
