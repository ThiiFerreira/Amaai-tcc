using FluentResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UsuariosApi.Data;
using UsuariosApi.Models;

namespace UsuariosApi.Services
{
    public class MensagemWpp
    {
        private string token = "EAAHBE9UHYEgBAKZAi9JOPKJIHGelWjZCWNgO974AkIm6SWBMUH68ZC96KD20lBqZC3cZBsCRrMjI2X5cyV9deYJMNmZCzjy54XZAyPvGyCKRHm1Y4WzOHK8coojowkY105GXYfn8QODJ1eFtZCDPxAmLPr1lMjgI4JZBD6ouZC2JoAFGq5D8beyiRZCLdSK3L3W3ttBFqbxFtxWBwZDZD";
        private string url = "https://graph.facebook.com/v14.0/105984795618762/messages";
        private UserDbContext _context;

        public MensagemWpp(UserDbContext context)
        {
            _context = context;
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

        public void enviarMensagemParaRealizarTarefa(Tarefa tarefa, String telefone, string nomeAssistido)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";

          

            string titulo = $"\"{tarefa.Titulo}\"";
            string id = $"\"{tarefa.Id}\"";
            string nome = $"\"{nomeAssistido}\"";

            telefone = $"\"{55 + telefone}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");

            //var json = "{ \"messaging_product\": \"whatsapp\", \"to\":" + telefone + " , \"type\": \"template\", \"template\": { \"name\": \"alerta_notificacao_realizar_tarefa\", \"language\": { \"code\": \"pt_BR\" }, \"components\": [{ \"type\": \"body\", \"parameters\": [{ \"type\": \"text\", \"text\": " + nome + " }, { \"type\": \"text\", \"text\": " + titulo + " }, { \"type\": \"text\", \"text\": " + id + " } ] }]} }";

            var json = "{ \"messaging_product\": \"whatsapp\", \"to\":" + telefone + ", \"type\": \"interactive\", \"interactive\": { \"type\": \"button\", \"body\": { \"text\": \"Olá " + nomeAssistido + ", chegou a hora de realizar a terefa " + tarefa.Titulo + ", que está marcada para ser realizada agora, por favor não deixe para depois\" }, \"action\": { \"buttons\": [ { \"type\": \"reply\", \"reply\": { \"id\": \"unique - postback - id\", \"title\": \"Finalizar tarefa-" + tarefa.Id + "\" } } ] } } }";


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

        public void enviarMensagemPedindoCodigoDaTarefa(string telefone)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";

            telefone = $"\"{telefone}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");    

            var json = "{ \"messaging_product\" : \"whatsapp\",\"to\":" + telefone + " ,\"type\": \"template\",\"template\": {\"name\": \"confirmar_finalizar_tarefa\",\"language\": {\"code\": \"pt_BR\"}}}";
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
                    Console.WriteLine($"mensagema enviada");
                }
                else
                {
                    Console.WriteLine($"Falha ao enviar mensagem - local do erro : enviarMensagemPedindoCodigoDaTarefa");

                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Exececao ao enviar mensagem - local do erro : enviarMensagemPedindoCodigoDaTarefa");

            }
        }

        public void enviaFeedbackTarefaFinalizada(string nomeAssistido, Tarefa tarefa, string telefone)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";

            var hora = DateTime.UtcNow.AddHours(-3).ToString("HH:mm");
            var dia = DateTime.UtcNow.AddHours(-3).ToString("dd/MM/yyyy");


            string titulo = $"\"{tarefa.Titulo}\"";
            string nome = $"\"{nomeAssistido}\"";
            telefone = $"\"{55 + telefone}\"";
            hora = $"\"{hora}\"";
            dia = $"\"{dia}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");

            var json = "{ \"messaging_product\": \"whatsapp\", \"to\":" + telefone + " , \"type\": \"template\", \"template\": { \"name\": \"feedback_finalizacao_mensagem\", \"language\": { \"code\": \"pt_BR\" }, \"components\": [{ \"type\": \"body\", \"parameters\": [{ \"type\": \"text\", \"text\": " + nome + " }, { \"type\": \"text\", \"text\": " + titulo + " }, { \"type\": \"text\", \"text\":" + hora + "}, { \"type\": \"text\", \"text\":" + dia + "} ] }]} }";
            
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
                    Console.WriteLine($"mensagem enviada");
                }
                else
                {
                    Console.WriteLine($"Falha ao enviar mensagem - local do erro : enviaFeedbackTarefaFinalizada");

                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Exececao ao enviar mensagem - local do erro : enviaFeedbackTarefaFinalizada");

            }
        }

        public void enviaMensagemDeErro(string telefone)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset-UTF-8";

            telefone = $"\"{telefone}\"";

            request.Headers.Add("Authorization", $"Bearer {token}");

            var json = "{ \"messaging_product\" : \"whatsapp\",\"to\":" + telefone + " ,\"type\": \"template\",\"template\": {\"name\": \"alerta_erro\",\"language\": {\"code\": \"pt_BR\"}}}";
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
                    Console.WriteLine($"mensagema enviada");
                }
                else
                {
                    Console.WriteLine($"Falha ao enviar mensagem - local do erro: enviaMensagemDeErro");

                }
            }
            catch (Exception e)
            {

                Console.WriteLine($"Exececao ao enviar mensagem -  local do erro: enviaMensagemDeErro");

            }
        }

    }
}
