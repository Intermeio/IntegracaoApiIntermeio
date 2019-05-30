using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConsumindoPacoteNugetAPIWeb.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Este objeto contem os dados do cliente que ele precisa para fazer todas as requisições da api
        /// </summary>
        private API.Intermeio.Models.ConfiguracaoCliente _configuracao;
        /// <summary>
        /// Construtor da Controle onde já iniciamos o objeto de configuração do cliente.
        /// </summary>
        public HomeController()
        {
            _configuracao = new API.Intermeio.Models.ConfiguracaoCliente()
            {
                Ambiente = "dev",
                Versao = "V3",
                AppKey = "INT67355",
                Signature = "I60dYmrytl0WdAl2IEzmfrSnbBrfaHu6h2M76S6zXXc="
            };
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Responável por gerar um token para passar em alguns methodos
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GerarToken()
        {
            //Criação de um objeto que vai recuperar o retorno da API
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoToken>();

            //Criar uma instancia do Objeto Token Business responsável por gerar o token
            //Na criação do Objeto e obrigatorio passar o objeto de configuração do cliente
            var tokenBusiness = new API.Intermeio.Business.TokenBusiness(_configuracao);

            //Com o objeto instaciado e passado os parametros de configuração podemos chamar o methodo para recuperar os dados Token
            retorno = tokenBusiness.RetornaToken();

            //Aqui retornamos o objeto recebido como resposta ao methodo invocado e convertemos em Json enviamos para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Este exemplo não precisa passar um token, como parametro de entrar ele fica responsável por gerar um token
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GerarBoleto()
        {
            //Criação de um objeto que vai recuperar o retorno da API
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoBoleto>();

            //Criar uma instancia do Objeto Boleto Business responsável por gerar o boleto
            //Na criação do Objeto e obrigatorio passar o objeto de configuração do cliente
            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao);

            //Chamar um methodo que monta o boleto, como explicado no menu Gerar Boleto
            API.Intermeio.Models.BoletoModel boleto = MontarBoleto();

            //Com o objeto instaciado e passado os parâmetros de configuração podemos chamar o methodo para transmitir o boleto
            retorno = boletoBuisiness.GerarBoleto(boleto);

            //Aqui retornamos o objeto recebido como resposta ao methodo invocado e convertemos em Json e enviamos para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Este exemplo é para você que já tem um token e gostaria de usar.
        /// </summary>
        /// <param name="token">Token válido do tipo GUID</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GerarBoletoToken(string token)
        {
            //Criação de um objeto que vai recuperar o retorno da API
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoBoleto>();

            //Criar uma instancia do Objeto Boleto Business responsável por gerar o boleto
            //Na criação do Objeto e obrigatório passar o objeto de configuração do cliente
            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao);

            //Chamar um methodo que monta o boleto, como explicado no menu Gerar Boleto
            API.Intermeio.Models.BoletoModel boleto = MontarBoleto();

            //Com o objeto instaciado e passado os parâmetros de configuração podemos chamar o methodo para transmitir o boleto
            //Aqui passamos um parâmetro a mais que é o token
            retorno = boletoBuisiness.GerarBoleto(boleto, token);

            //Aqui retornamos o objeto recebido como resposta ao methodo invocado e convertemos em Json e enviamos para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Este exemplo não precisa passar um token, como parâmetro de entrar ele fica responsável por gerar um token
        /// </summary>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GerarBoletoLote()
        {
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoLote>();

            //Este Objeto espera uma lista de boletos para fazer a transmissão, ele que será passado como parâmetro
            API.Intermeio.Models.BoletoLoteModel boletolote = new API.Intermeio.Models.BoletoLoteModel();

            //Criamos um objeto do tipo lista de Boleto
            List<API.Intermeio.Models.BoletoModel> boletos = new List<API.Intermeio.Models.BoletoModel>();

            //Este exemplo vai adicionar na lista de boletos 4 itens do tipo boletoModel com todas as informações que um boleto
            //precisa para ser transmitido;
            for (int i = 0; i < 4; i++)
            {
                boletos.Add(MontarBoleto());
            }

            //Vamos adicionar a lista de boletos populada para o objeto que vai fazer a transmissão de boleto em lote
            boletolote.Boletos = boletos;

            //Criar uma instancia do Objeto Boleto Business responsável por gerar o boleto em lote
            //Na criação do Objeto e obrigatorio passar o objeto de configuração do cliente
            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao);

            //Com o objeto instaciado e passado os parametros de configuração podemos chamar o methodo para transmitir o boleto em lote
            retorno = boletoBuisiness.GerarBoletoLote(boletolote);

            //Aqui retornamos o objeto recebido como resposta ao methodo invocado e convertemos em Json enviamos para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Este exemplo é para você que já tem um token e gostaria de usar.
        /// </summary>
        /// <param name="token">Token válido do tipo GUID</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GerarBoletoLoteToken(string token)
        {
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoLote>();

            //Este Objeto espera uma lista de boletos para fazer a transmissão, ele que será passado como parâmetro
            API.Intermeio.Models.BoletoLoteModel boletolote = new API.Intermeio.Models.BoletoLoteModel();

            //Criamos um objeto do tipo lista de Boleto
            List<API.Intermeio.Models.BoletoModel> boletos = new List<API.Intermeio.Models.BoletoModel>();

            //Este exemplo vai adicionar na lista de boletos 4 itens do tipo boletoModel com todas as informações que um boleto
            //precisa para ser transmitido;
            for (int i = 0; i < 4; i++)
            {
                boletos.Add(MontarBoleto());
            }

            //Vamos adicionar a lista de boletos populada para o objeto que vai fazer a transmissão de boleto em lote
            boletolote.Boletos = boletos;

            //Criar uma instancia do Objeto Boleto Business responsável por gerar o boleto em lote
            //Na criação do Objeto e obrigatório passar o objeto de configuração do cliente
            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao);

            //Com o objeto instaciado e passado os parâmetros de configuração podemos chamar o methodo para transmitir o boleto em lote
            //Aqui passamos um parâmetro a mais que é o token
            retorno = boletoBuisiness.GerarBoletoLote(boletolote, token);

            //Aqui retornamos o objeto recebido como resposta ao methodo invocado e convertemos em Json enviamos para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        
        //Criando um methodo que vai retornar o Objeto já preenchido
        private API.Intermeio.Models.BoletoModel MontarBoleto()
        {

            //Criando o objeto modelo onde será passado objetos do tipo Boleto, Cliente, SplitModel, Configuracoes

            var boletoModel = new API.Intermeio.Models.BoletoModel();

            //Criando um objeto do tipo Boleto e adicionando no modelo
            //Estou colocando um exemplo com dados fictícios
            boletoModel.Boleto = new API.Intermeio.Models.Boleto()
            {
                DataVencimento = "10/10/2020",
                NumeroDocumento = "10864619-7",
                PercentualJuros = "0",
                PercentualMulta = "0",
                QntDiasJuros = 0,
                QntDiasMulta = 0,
                Valor = new Random().Next(1, 200).ToString(), //"020" R$ 0,20 caso queira colocar um valor de R$ 4500,68 colocar número inteiro sem pontuação Ex.450068
                ValorJuros = 0
            };

            //Criando um objeto do tipo Cliente e adicionando no modelo
            //Estou colocando um exemplo com dados fictícios
            boletoModel.Cliente = new API.Intermeio.Models.Cliente()
            {
                NomeRazao = "Juarez12",
                ApelidoEndereco = "endereco 01",
                Bairro = "juquia ze",
                Celular = "11944445555",
                CEP = "08830010",
                Cidade = "Sao",
                Complemento = "Casa",
                CpfCnpj = "74136537000110",
                DataDeNascimento = "27/02/1987",
                Email = "joseMaria@gmail.com",
                Endereco = "Francisco",
                Estado = "SP",
                Logradouro = "Rua",
                Sexo = "M",
                Numero = "94"
            };

            //Criando um Objeto Split Model
            var splitModel = new API.Intermeio.Models.SplitModel();
            //Este parametro e muito importante quando está marcado como true, qualquer erro que houver na hora do split ele aborta o processo de geração do boleto
            splitModel.AbortarEmCasoDeErro = true;
            //Dentro do Split um dos objetos que ele recebe é uma lista de clientes, aqui estamos criando uma lista para preencher 2 clientes
            var clientesSplit = new List<API.Intermeio.Models.SplitCliente>();
            //Criando o primeiro objeto do cliente que vai ficar dentro do Split
            var clientSplit = new API.Intermeio.Models.SplitCliente()
            {
                NomeRazao = "Fabio Santos",
                CpfCnpj = "02130040896",
                Descricao = "teste",
                Email = "teste@teste.com.br",
                Taxa = "16",
                NotificarPorEmail = true,
                NotificarPorSms = false,
                //Criando um objeto lista do tipo telefone onde 1 cliente pode ter varios telefones
                Telefones = new List<API.Intermeio.Models.Telefone>()
{
//Criando objeto do tipo telefone
new API.Intermeio.Models.Telefone()
{
    Apelido = "Fabio",
    DDD = "11",
    Numero = "988886666"
}
},
                //Criar o objeto do tipo conta do cliente para o split
                ClienteContaBancaria = new API.Intermeio.Models.ClienteContaBancaria()
                {
                    Conta = "11111",
                    DigConta = "1",
                    Agencia = "2222",
                    DigAgencia = "2",
                    CodigoBanco = "12345"
                },
                //Criando um objeto do tipo transferência automatica
                TransferenciaAutomatica = new API.Intermeio.Models.TransferenciaAutomatica()
                {
                    ACobrar = true,
                    EfetuarTefAutomatica = true,
                    Periodicidade = 1,
                    ValorMinimo = "100"
                }
            };
            //Criando o segundo objeto do tipo cliente split
            var clientSplit2 = new API.Intermeio.Models.SplitCliente()
            {
                NomeRazao = "Fulano da Silva",
                CpfCnpj = "02130040896",
                Descricao = "teste",
                Email = "Fulano@teste.com.br",
                Taxa = "20",
                NotificarPorEmail = true,
                NotificarPorSms = true,
                //Criando objeto do tipo telefone para o segundo cliente split
                Telefones = new List<API.Intermeio.Models.Telefone>()
{
new API.Intermeio.Models.Telefone()
{
    Apelido = "Split 2",
    DDD = "11",
    Numero = "922221111"
}
},
                //Criar o objeto do tipo conta para o segundo cliente split
                ClienteContaBancaria = new API.Intermeio.Models.ClienteContaBancaria()
                {
                    Conta = "11111",
                    DigConta = "1",
                    Agencia = "2222",
                    DigAgencia = "2",
                    CodigoBanco = "12345"
                },
                //Criando um objeto do tipo transferência automatica para o segundo cliente split
                TransferenciaAutomatica = new API.Intermeio.Models.TransferenciaAutomatica()
                {
                    ACobrar = true,
                    EfetuarTefAutomatica = true,
                    Periodicidade = 1,
                    ValorMinimo = "100"
                }
            };
            //Adicionando na lista de split o cliente 1 e 2
            clientesSplit.Add(clientSplit);
            clientesSplit.Add(clientSplit2);
            //Adicionando no obejto Split Model o split preenchido com os dados dos clientes
            splitModel.Clientes = clientesSplit;
            //Adicionando o objeto split no modelo boleto
            boletoModel.Split = splitModel;
            //Criando objeto configurações do modelo boleto
            var configuracoes = new API.Intermeio.Models.Configuracoes()
            {
                //Criando um objeto Emissão Digital para enviar Sms e email para o cliente
                EmissaoDigital = new API.Intermeio.Models.EmissaoDigital()
                {
                    Email = "teste@live.com",
                    //Criando um objeto SMS
                    Sms = new API.Intermeio.Models.Sms()
                    {
                        Celular = "11977446699",
                        Msg = "Teste Via API classe de teste"
                    }
                }
            };
            //Adicionado as configurações do boleto no objeto Boleto
            boletoModel.Configuracoes = configuracoes;
            //Retornando o objeto(Boleto) todo preenchido pronto para gerar o boleto
            return boletoModel;
        }

    }
}