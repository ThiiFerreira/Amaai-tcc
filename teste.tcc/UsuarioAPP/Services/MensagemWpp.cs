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
        private string token = "EAAHBE9UHYEgBAKZAi9JOPKJIHGelWjZCWNgO974AkIm6SWBMUH68ZC96KD20lBqZC3cZBsCRrMjI2X5cyV9deYJMNmZCzjy54XZAyPvGyCKRHm1Y4WzOHK8coojowkY105GXYfn8QODJ1eFtZCDPxAmLPr1lMjgI4JZBD6ouZC2JoAFGq5D8beyiRZCLdSK3L3W3ttBFqbxFtxWBwZDZD";
        private string url = "https://graph.facebook.com/v14.0/105984795618762/messages";

        public MensagemWpp(string token, string url)
        {
            this.token = token;
            this.url = url;
        }
        public Result EnviarMensagemAlertaTarefa(Tarefa tarefa, String telefone)
        {
            WebRequest request = WebRequest.Create(url);
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
            WebRequest request = WebRequest.Create(url);
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
                    Console.WriteLine($"mensagem para realizar tarefa enviada - Tarefa: {tarefa.Id}, Reponsavel: {tarefa.ResponsavelId}, Assistido: {tarefa.IdosoId}");
                }
                else
                {
                    Console.WriteLine($"Falha ao enviar mensagem para realizar tarefa - Tarefa: {tarefa.Id}, Reponsavel: {tarefa.ResponsavelId}, Assistido: {tarefa.IdosoId}");

                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Exececao ao enviar mensagem para realizar tarefa - Tarefa: {tarefa.Id}, Reponsavel: {tarefa.ResponsavelId}, Assistido: {tarefa.IdosoId}");

            }
        }
    }
}
