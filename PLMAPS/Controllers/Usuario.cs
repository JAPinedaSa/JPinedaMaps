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
        public ActionResult Login(ML.Usuario usuario, string Contraseña)
        {
            // Crear una instancia del algoritmo de hash bcrypt
            var bcrypt = new Rfc2898DeriveBytes(Contraseña, new byte[0], 10000, HashAlgorithmName.SHA256);
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
                
                MailMessage mailMessage = new MailMessage(emailOrigen, Email, "Restablecimiento de Contraseña", "<p>Restablece tu Contraseña por este Medio</p>");
                mailMessage.IsBodyHtml = true;
                string contenidoHTML = System.IO.File.ReadAllText(@"C:\Users\digis\OneDrive\Documents\Jose_Alejandro_Pineda_Sanchez\Repositorios\JPinedaMaps\PLMAPS\Views\Usuario\Correo.html");
                mailMessage.Body = contenidoHTML;
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


    }

    
}
