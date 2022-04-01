using ByteBank.Controller;
using ByteBank.Infraestrutura.IoC;
using ByteBank.Service;
using ByteBank.Service.Cambio;
using ByteBank.Service.Cartao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Infraestrutura
{
    public class CartaoServiceTesteContainer : ICartaoService
    {
        string ICartaoService.ObterCartaoDeCreditoDeDestaque() => "Cartão de crédito. Teste de container.";

        string ICartaoService.ObterCartaoDeDebitoDeDestaque() => "Cartão de débito. Teste de container.";

    }
    public class WebApplication
    {
        private readonly string[] _prefixos;
        private readonly IContainer _container = new ContainerSimples();

        public WebApplication(string[] prefixos)
        {
            if (prefixos == null)
                throw new ArgumentNullException(nameof(prefixos));

            _prefixos = prefixos;

            Configurar();
        }
        public void Iniciar()
        {
            while (true)
                ManipularRequisicao();
        }

        public void Configurar()
        {
            _container.Registrar<ICambioService, CambioTesteService>();
            _container.Registrar<ICartaoService, CartaoServiceTeste>();
        }

        private void ManipularRequisicao()
        {
            var httpListener = new HttpListener();

            foreach (var prefixo in _prefixos)
                httpListener.Prefixes.Add(prefixo);

            httpListener.Start();

            var contexto = httpListener.GetContext();
            var requisicao = contexto.Request;
            var resposta = contexto.Response;

            var path = requisicao.Url.PathAndQuery ;

            if (Utilidades.EhArquivo(path))
            {
                var manipulador = new ManipuladorRequisicaoArquivo();
                manipulador.Manipular(resposta, path);
            }
            else
            {
                var manipulador = new ManipuladorRequisicaoController(_container);
                manipulador.Manipular(resposta, path);
            }

            httpListener.Stop();
        }
    }
}
