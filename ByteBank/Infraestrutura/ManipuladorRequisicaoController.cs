﻿using ByteBank.Infraestrutura.Filtros;
using ByteBank.Infraestrutura.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Infraestrutura
{
    public class ManipuladorRequisicaoController
    {
        private readonly ActionBinder _actionBinder = new ActionBinder();
        private readonly FilterResolver _filterResolver = new FilterResolver();
        private readonly ControllerResolver controllerResolver;

        public ManipuladorRequisicaoController(IContainer container)
        {
            controllerResolver = new ControllerResolver(container);
        }
        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var partes = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerNome = partes[0];

            var controllerNomeCompleto = $"ByteBank.Controller.{controllerNome}Controller";


            // var controllerWrapper = Activator.CreateInstance("Bytebank", controlerNomeCompleto, new object[0]);
            //var controller = controllerWrapper.Unwrap();

            var controller = controllerResolver.ObterController(controllerNomeCompleto);


            var actionBindInfo = _actionBinder.ObterActionBindInfo(controller, path);

            var filterResult = _filterResolver.VerificarFiltros(actionBindInfo);

            if (filterResult.PodeContinuar)
            {
                var resultadoAction = (string)actionBindInfo.Invoke(controller);

                var buffer = Encoding.UTF8.GetBytes(resultadoAction);

                resposta.StatusCode = 200;
                resposta.ContentType = "text/html; charset=utf-8";
                resposta.ContentLength64 = buffer.Length;

                resposta.OutputStream.Write(buffer, 0, buffer.Length);
                resposta.OutputStream.Close();
            }
            else
            {
                resposta.StatusCode = 307;
                resposta.RedirectLocation = "/Erro/Inesperado";
                resposta.OutputStream.Close();
            }
        }
    }
}
