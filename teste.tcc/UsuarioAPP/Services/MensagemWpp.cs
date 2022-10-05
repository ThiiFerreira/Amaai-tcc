using FluentResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class MensagemWpp
    {
        public Result EnviarMensagemAlertaTarefa(Tarefa tarefa, String telefone)
        {
            var token = "EAAHBE9UHYEgBAOGpcnNfdk82zrOnCiZC9C4e2KUZCZBmeEpvZCZAiZCL9n20pzdULF04Kl9Tj94xD55GEEnUTATbNsVdnD5BPhZAA81hkUXKsN0tw1SDmPTxKP67fgtp0BmdkpUj3fhYcYawV10PPqqYhgtElfPaF45hgOGSfShclZCucwZABSNhXVDHuJHvTU9n9Pfj59tZArdRi2H8bNkGqY";
            WebRequest request = WebRequest.Create("https://graph.facebook.com/v14.0/105984795618762/messages");
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";
            
            string hora = $"\"{tarefa.HoraAlerta}\"";
            string data = $"\"{tarefa.DataAlerta}\"";
            telefone = $"\"{55+telefone}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");
            
            var json = "{ \"messaging_product\": \"whatsapp\", \"to\":"+ telefone + " , \"type\": \"template\", \"template\": { \"name\": \"alerta_tarefa_criada_personalizada \", \"language\": { \"code\": \"pt_BR\" }, \"components\": [{ \"type\": \"body\", \"parameters\": [{ \"type\": \"text\", \"text\": " + hora + " },{ \"type\": \"text\", \"text\":" + data + " } ] }] } }";
            var bytearray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytearray.Length;
           
            Stream stream = request.GetRequestStream();
            stream.Write(bytearray, 0, bytearray.Length);
            stream.Close();

            try {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Result.Ok();
                }
                else
                {
                    return Result.Fail("Falha ao enviar mensagem para o whastapp");
                }
            } 
            catch (Exception e)
            {
                return Result.Fail("Não foi possivel enviar mensagem para o whastapp do assistido, verifique se o contato está correto");

            }
        }

        public void enviarMensagemParaRealizarTarefa(Tarefa tarefa, String telefone)
        {
            var token = "EAAHBE9UHYEgBAOGpcnNfdk82zrOnCiZC9C4e2KUZCZBmeEpvZCZAiZCL9n20pzdULF04Kl9Tj94xD55GEEnUTATbNsVdnD5BPhZAA81hkUXKsN0tw1SDmPTxKP67fgtp0BmdkpUj3fhYcYawV10PPqqYhgtElfPaF45hgOGSfShclZCucwZABSNhXVDHuJHvTU9n9Pfj59tZArdRi2H8bNkGqY";
            WebRequest request = WebRequest.Create("https://graph.facebook.com/v14.0/105984795618762/messages");
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";

            string titulo = $"\"{tarefa.Titulo}\"";

            telefone = $"\"{55 + telefone}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");

            var json = "{ \"messaging_product\": \"whatsapp\", \"to\":" + telefone + " , \"type\": \"template\", \"template\": { \"name\": \"alerta_notificacao_realaizar_tarefa \", \"language\": { \"code\": \"pt_BR\" }, \"components\": [{ \"type\": \"body\", \"parameters\": [{ \"type\": \"text\", \"text\": " + titulo + " } ] }]} }";
            var bytearray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = bytearray.Length;

            Stream stream = request.GetRequestStream();
            stream.Write(bytearray, 0, bytearray.Length);
            stream.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("deu certo");
                }
                else
                {
                    Console.WriteLine("deu errado");

                }
            }
            catch (Exception e)
            {

                Console.WriteLine("deu errado ex");

            }
        }
    }
}
