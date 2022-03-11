using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Infraestrutura
{
    public class ManipuladorRequisicaoArquivo
    {
        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var nomeResource = Utilidades.ConverterPathParaNomeAssembly(path);

            var resourceStream = assembly.GetManifestResourceStream(nomeResource);

            if (resourceStream == null)
            {
                resposta.StatusCode = 404;
                resposta.OutputStream.Close();
            }

            else
                using (resourceStream)
                {
                    var bytesRseource = new byte[resourceStream.Length];

                    resourceStream.Read(bytesRseource, 0, (int)resourceStream.Length);


                    resposta.ContentType = Utilidades.ObterTipoDeConteudo(path);
                    resposta.StatusCode = 200;
                    resposta.ContentLength64 = resourceStream.Length;

                    resposta.OutputStream.Write(bytesRseource, 0, bytesRseource.Length);
                    resposta.OutputStream.Close();
                }

        }
    }
}
