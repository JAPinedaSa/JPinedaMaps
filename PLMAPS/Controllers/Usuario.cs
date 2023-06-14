using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Security.Cryptography;

namespace PLMAPS.Controllers
{
    public class Usuario : Controller
    {
        public ActionResult Login()
        {
            ML.Usuario usuario = new ML.Usuario();
            return View(usuario);
        }
        [HttpPost]
        public ActionResult Login(ML.Usuario usuario, string contraseña)
        {
            // Crear una instancia del algoritmo de hash bcrypt
            var bcrypt = new Rfc2898DeriveBytes(contraseña, new byte[0], 10000, HashAlgorithmName.SHA256);
            // Obtener el hash resultante para la contraseña ingresada 
            var passwordHash = bcrypt.GetBytes(20);

            if (usuario.Email != null)
            {
                // Insertar usuario en la base de datos
                usuario.Contraseña = passwordHash;
                ML.Result result = BL.Usuario.Add(usuario);
                if (result.Correct)
                {
                    ViewBag.Message = "Has Sido Registrado con éxito";
                    return View("ModalLogin");
                }
                else
                {
                    ViewBag.Message = "Ocurrio Un Error al Registrarse, Intentelo Nuevamente";
                    return View("ModalLogin");
                }

            }
            else
            {
                // Proceso de login
                ML.Result result = BL.Usuario.GetByUserName(usuario.UserName);
                usuario = (ML.Usuario)result.Object;
                if (result.Correct)
                {
                    if (usuario.Contraseña.SequenceEqual(passwordHash))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Message = "La contraseña no coincide";
                        return PartialView("ModalLogin");
                    }
                }
                else
                {
                    ViewBag.Message = "El usuario que ingresaste no  existe";
                    return PartialView("ModalLogin");
                }

            }

        }
        [HttpGet]
        public ActionResult OlvideContrasena()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OlvideContrasena(string Email)
        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = BL.Usuario.GetByEmail(Email);
            usuario = (ML.Usuario)result.Object;
            if (result.Correct)
            {
                string emailOrigen = "JA.PinedaSa@gmail.com";

                MailMessage mailMessage = new MailMessage(emailOrigen, Email, "Restablecimiento de contraseña", "<p>Restablece tu contraseña por este Medio</p>");
                mailMessage.IsBodyHtml = true;
                string contenidoHTML = System.IO.File.ReadAllText(@"C:\Users\digis\OneDrive\Documents\Jose_Alejandro_Pineda_Sanchez\Repositorios\JPinedaMaps\PLMAPS\Views\Usuario\Correo_2.cshtml");
                mailMessage.Body = contenidoHTML + "<div class='bajada' style='padding: 40px; text - align: center; color:#585758;font-size:15px'> <p>Hemos recibido una solicitud de recuperación de contraseña. Para generar una nueva, haga click en el siguiente botón.</p>" +
                    " <form action='http://localhost:5181/Usuario/NuevaContraseña' method='post'>" +
                    "<input hidden type ='text' name='userName'  style='display:none;'  value=" + "'" + usuario.UserName + "'" + " />" +
                    //BOTON PARA ENVIAR
                    " <input type='submit' value='Cambiar Contraseña' class='btn_sign_up' style='background: #ff8800;color: white;border-radius: 5px;padding: 15px 40px;text-decoration: none;margin: 15px 0;display:inline-block;'  />" +
                    "</form>" +
                    "</div>" +
                    "<div class='firma' style='text - align:center; max - width:630px; padding - top: 30px'>" +
                    "<p style='margin: 0'>--</p>" +
                    "<p style='margin: 0'>--</p>" +
                    "<div style='font - family: Helvetica; color: gray;'>" +
                    "<p style='margin: 10px 0'>Jose Alejandro Pineda Sanchez</p> " +
                    "<a style='color: #003272;text-decoration:none' href='https://maxxa.cl' target='_blank'>JPineda<span style='color:#e9540d'>MA</span><span style='color:#f28b2d'>PS</span>/Recuperacion de Contraseña</a>" +
                    "<p style='margin: 10px 0'>Apoquindo 6550, piso 16, Las Condes.</p>" +
                    "</div>" +
                    "</div>";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, "rhnyklmeqbxfiqcc");

                smtpClient.Send(mailMessage);
                smtpClient.Dispose();

                ViewBag.Modal = "show";
                ViewBag.Mensaje = "Se ha enviado un correo de confirmación a tu correo electronico";
                return View("ModalLogin");
            }
            else
            {
                ViewBag.Mensaje = "Hubo un error";
                return View("ModalLogin");
            }

        }

        //[HttpPost]
        //public ActionResult Recuperar(string userName)
        //{
        //    return RedirectToAction("NuevaContraseña", "Usuario", userName);
        //}
        //[HttpGet]
        //public ActionResult NuevaContraseña(string userName)
        //{
        //    ML.Result result = BL.Usuario.GetByUserName(userName);
        //    ML.Usuario usuario = new ML.Usuario();
        //    usuario = (ML.Usuario)result.Object;
        //    return View();

        //}

        [HttpPost]
        public ActionResult NuevaContraseña(ML.Usuario usuario,string userName, string password)
        {

            
            if (password == null)
            {
                return View(usuario);
            }
            else
            {
                var bcrypt = new Rfc2898DeriveBytes(password, new byte[0], 10000, HashAlgorithmName.SHA256);
                // Obtener el hash resultante para la contraseña ingresada 
                var passwordHash = bcrypt.GetBytes(20);
                ML.Result result = BL.Usuario.GetByUserName(userName);
                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;
                    usuario.Contraseña = passwordHash;
                    ML.Result resultUpdate = BL.Usuario.Update(usuario);
                }
                ViewBag.Message = "TodoBien";
                return View("ModalLogin");
            }
        }

    }


}



