using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using DM;

namespace Servicios
{
    public static class Utilities
    {
        public static string RutaServidor { get; set; }

        public static string EncodePassword(string originalPassword)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(originalPassword);
            byte[] hash = sha1.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }

        public static string FormatNit(string nitOriginal)
        {
            string nitTmp = nitOriginal;
            try
            {
                if (nitOriginal.Contains('-'))
                {
                    string[] nit = nitOriginal.Split('-');
                    nitTmp = (double.Parse(nit[0].Trim())).ToString("N0") + "-" + nit[1].Trim();
                }
                else
                    nitTmp = (double.Parse(nitOriginal)).ToString("N0");
            }
            catch (Exception ex)
            {
                nitTmp = nitOriginal;
            }
            return nitTmp.Replace(",",".");
        }

        public static void Log(Exception ex)
        {
            string s = RutaServidor;
            using (StreamWriter sw = new StreamWriter(RutaServidor + "/Log/" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" +
                                                                               DateTime.Now.Year + "__" + DateTime.Now.Hour + "_" +
                                                                               DateTime.Now.Minute + "_" + DateTime.Now.Second + ".txt", true))
            {
                sw.WriteLine("Message =>    " + ex.Message);
                sw.WriteLine("InnerException =>    " + ex.InnerException);
                sw.WriteLine("Source =>    " + ex.Source);
                sw.WriteLine("Data =>    " + ex.Data);
                sw.WriteLine("Full Error =>    " + ex.ToString());
                sw.Close();
            }
        }

        public static bool EsOperador(string cadena)
        {
            if (cadena == "-" || cadena == "+" || cadena == "*" || cadena == "/" || cadena == "(" || cadena == ")" || cadena == "Ʃ" || 
                cadena == ">" || cadena == "<" || cadena == " case " || cadena == " when " || cadena == " else " || cadena == " end " || cadena == " then ")
                return true;
            else
                return false;
        }

        public static DataTable Select(string sql, string nombreDataTable)
        {
            using (SqlConnection cnn = new SqlConnection())
            {
                cnn.ConnectionString = new EntityConnection("name=ContextoOwner").StoreConnection.ConnectionString;
                cnn.Open();

                SqlDataAdapter adaptador = new SqlDataAdapter(sql, cnn);
                DataTable dtTmp = new DataTable(nombreDataTable);
                adaptador.Fill(dtTmp);

                cnn.Close();

                return dtTmp;
            }
        }

        public static void Insertupdate(string sql)
        {
            using (SqlConnection cnn = new SqlConnection())
            {
                cnn.ConnectionString = new EntityConnection("name=ContextoOwner").StoreConnection.ConnectionString;
                cnn.Open();

                SqlDataAdapter adaptador = new SqlDataAdapter(sql, cnn);
                SqlCommand cmd = new SqlCommand(sql, cnn);
                cmd.ExecuteNonQuery();

                cnn.Close();
            }
        }

        public static void GetEmailByHotel(int idHotel, ref string email, ref string pass)
        {
            if (System.Configuration.ConfigurationManager.AppSettings.GetValues("Bogota")[0].Contains(idHotel.ToString()))
            {
                email = System.Configuration.ConfigurationManager.AppSettings.GetValues("CorreoRemitenteBogota")[0];
                pass = System.Configuration.ConfigurationManager.AppSettings.GetValues("ClaveRemitenteBogota")[0];
            }
            else if (System.Configuration.ConfigurationManager.AppSettings.GetValues("Costa")[0].Contains(idHotel.ToString()))
            {
                email = System.Configuration.ConfigurationManager.AppSettings.GetValues("CorreoRemitenteCosta")[0];
                pass = System.Configuration.ConfigurationManager.AppSettings.GetValues("ClaveRemitenteCosta")[0];
            }
            else if (System.Configuration.ConfigurationManager.AppSettings.GetValues("Medellin")[0].Contains(idHotel.ToString()))
            {
                email = System.Configuration.ConfigurationManager.AppSettings.GetValues("CorreoRemitenteMedellin")[0];
                pass = System.Configuration.ConfigurationManager.AppSettings.GetValues("ClaveRemitenteMedellin")[0];
            }
            else
            {
            }
        }

        public static void EnviarCorreo(string correoRemitente,
                                        string claveRemitente,
                                        string nombreRemitente,
                                        string correoOculto,
                                        string textoCuerpo,
                                        string asunto,
                                        string hostSmtp,
                                        int puertoSmtp,
                                        bool enableSsl,
                                        List<string> listaAdjunto,
                                        List<string> listaCorreoDestino,
                                        bool esHtml,
                                        ref bool esConDestinatarios,
                                        bool isPruebas,
                                        int idHotel)
        {
            GetEmailByHotel(idHotel, ref correoRemitente, ref claveRemitente);

            MailMessage miMensaje = new MailMessage();
            miMensaje.From = new MailAddress(correoRemitente.Trim(), nombreRemitente, Encoding.UTF8);

            bool _esConDestinatarios = false;
            foreach (string itemCorreo in listaCorreoDestino)
            {
                if (!string.IsNullOrEmpty(itemCorreo))
                {
                    if (isPruebas)
                        miMensaje.To.Add("manuelzambrano@outlook.com");
                    else
                        miMensaje.To.Add(itemCorreo.Trim());

                    _esConDestinatarios = true;
                }
            }

            if (_esConDestinatarios == false)
            {
                esConDestinatarios = false;
                return;
            }

            if (!string.IsNullOrEmpty(correoOculto))
            {
                MailAddress bcc = new MailAddress(correoOculto);
                miMensaje.Bcc.Add(bcc);
            }

            miMensaje.Subject = asunto;
            miMensaje.Body = textoCuerpo;
            miMensaje.BodyEncoding = Encoding.UTF8;
            miMensaje.IsBodyHtml = esHtml;

            string extension = string.Empty;
            Attachment adjunto = null;

            foreach (string rutaAdjunto in listaAdjunto)
            {
                extension = (Path.GetExtension(rutaAdjunto)).ToLower();
                switch (Path.GetExtension(rutaAdjunto))
                {
                    case ".pdf":
                        adjunto = new Attachment(rutaAdjunto, MediaTypeNames.Application.Pdf);
                        break;

                    case ".xls":
                    case ".xlsx":
                        adjunto = new Attachment(rutaAdjunto, "application/msexcel");
                        break;

                    default:
                        adjunto = new Attachment(rutaAdjunto);
                        break;
                }
                
                miMensaje.Attachments.Add(adjunto);
            }

            //if (rutaAdjunto != string.Empty)
            //{
            //    Attachment adjunto = new Attachment(rutaAdjunto, "application/pdf");
            //    miMensaje.Attachments.Add(adjunto);
            //}

            try
            {
                //Aquí es donde se hace lo especial
                SmtpClient client = new SmtpClient();

                //client.Credentials = new System.Net.NetworkCredential("zambrano.manuelc@gmail.com", "Patricia115z");
                client.Credentials = new System.Net.NetworkCredential(correoRemitente, claveRemitente);
                //client.Credentials = new System.Net.NetworkCredential("propietarios.estelar@hotelesestelar.com", "Este2015");
                //client.UseDefaultCredentials = false;
                client.Port = puertoSmtp; //587;
                //client.Host = "smtp.gmail.com";
                client.Host = hostSmtp; // "outlook.office365.com";
                client.EnableSsl = enableSsl; //Esto es para que vaya a través de SSL que es obligatorio con GMail
                client.Timeout = 1999999;//900000;

                //client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

                client.Send(miMensaje);
                miMensaje.Dispose();
                //client.SendAsync(miMensaje, null);
                //return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                miMensaje.Dispose();
                //return false;
            }
        }

        public static string EncriptarEnVocal(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        public static string DesencriptarEnVocal(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        public static void EliminarArchivo(string ruta)
        {
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
        }

        public static bool EsCorreoValido(string correo)
        {
            return Regex.IsMatch(correo, @"^\w+([-+.']\w+)*([-+.'])*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            // ^[_a-zA-Z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,3})$
        }

        public static string SoloNumeroLetras(string cadena)
        {
            return Regex.Replace(cadena, "^[A-Z0-9 a-z]*$", " ");
        }

        public static string SoloNumero(string cadena)
        {
            return Regex.Replace(cadena, "^[0-9]*$", " ");
        }

        public static string QuitarAcentuaciones(string cadena)
        {
            cadena = cadena.Replace('á', 'a');
            cadena = cadena.Replace('é', 'e');
            cadena = cadena.Replace('í', 'i');
            cadena = cadena.Replace('ó', 'o');
            cadena = cadena.Replace('ú', 'u');
            cadena = cadena.Replace('à', 'a');
            cadena = cadena.Replace('è', 'e');
            cadena = cadena.Replace('ì', 'i');
            cadena = cadena.Replace('ò', 'o');
            cadena = cadena.Replace('ù', 'u');

            cadena = cadena.Replace('Á', 'A');
            cadena = cadena.Replace('É', 'E');
            cadena = cadena.Replace('Í', 'I');
            cadena = cadena.Replace('Ó', 'O');
            cadena = cadena.Replace('Ú', 'U');
            cadena = cadena.Replace('À', 'A');
            cadena = cadena.Replace('È', 'E');
            cadena = cadena.Replace('Ì', 'I');
            cadena = cadena.Replace('Ò', 'O');
            cadena = cadena.Replace('Ù', 'U');

            return cadena;
        }

        public static string ColocarAcentuaciones(string cadena)
        {
            cadena = cadena.Replace('a', 'á');
            cadena = cadena.Replace('e', 'é');
            cadena = cadena.Replace('i', 'í');
            cadena = cadena.Replace('o', 'ó');
            cadena = cadena.Replace('u', 'ú');
            return cadena;
        }

        public static string QuitarCaracteresEspeciales(string cadena)
        {
            return Regex.Replace(cadena, @"[^\w\.@-]", "");
        }

        public static string PadLeft(string cadena, short numCaracteres, char caracterRelleno)
        {
            //cadena = cadena.Replace(".", "");
            return cadena.PadLeft(numCaracteres, caracterRelleno);
        }

        public static string PadRight(string cadena, short numCaracteres, char caracterRelleno)
        {
            cadena = cadena.Replace(".", "");
            cadena = QuitarAcentuaciones(cadena);
            return cadena.PadRight(numCaracteres, caracterRelleno);
        }

        public static bool EsCadenaSoloNumeros(string cadena)
        {
            double num;
            return double.TryParse(cadena, out num);
        }

        public static double PasarPorcentaje(string valor)
        {
            if (valor.Contains("%"))
            {
                double val = double.Parse(valor.Replace('%', ' ').Trim()) / 100;
                return val;
            }
            else
                return double.Parse(valor.Trim());
        }

        public static bool EsIdentificacionExistentePropietario(string numIdentificacion, int idPropietario)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Propietario.Where(P => P.NumIdentificacion == numIdentificacion && P.IdPropietario != idPropietario).Count();

                return (con > 0) ? true : false;
            }
        }

        public static bool EsIdentificacionExistenteUsuario(string numIdentificacion)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Usuario.Where(P => P.Identificacion == numIdentificacion).Count();

                return (con > 0) ? true : false;
            }
        }

        public static bool EsLoginExistenteUsuario(string login)
        {
            using (ContextoOwner Contexto = new ContextoOwner())
            {
                int con = Contexto.Usuario.Where(P => P.Login == login).Count();

                return (con > 0) ? true : false;
            }
        }

        public static DateTime ObtenerUltimoDiaMes(int ano, int mes)
        {
            DateTime fecha = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            return fecha;
        }
    }
}
