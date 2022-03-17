﻿using System;
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

        public void Manipular(HttpListenerResponse resposta, string path)
        {
            var partes = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var controllerNome = partes[0];

            var controlerNomeCompleto = $"ByteBank.Controller.{controllerNome}Controller";

            var controllerWrapper = Activator.CreateInstance("Bytebank", controlerNomeCompleto, new object[0]);
            var controller = controllerWrapper.Unwrap();

            //var methordInfo = controller.GetType().GetMethod(actionNome);
            var methodInfo = _actionBinder.ObterActionBindInfo(controller, path);

            var resultadoAction = (string)methodInfo.Invoke(controller);

            var buffer = Encoding.UTF8.GetBytes(resultadoAction);

            resposta.StatusCode = 200;
            resposta.ContentType = "text/html; charset=utf-8";
            resposta.ContentLength64 = buffer.Length;

            resposta.OutputStream.Write(buffer, 0, buffer.Length);
            resposta.OutputStream.Close();
        }
    }
}
