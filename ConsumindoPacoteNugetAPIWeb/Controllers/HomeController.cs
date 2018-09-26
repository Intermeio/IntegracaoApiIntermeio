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

        [HttpPost]
        public JsonResult GerarToken()
        {
            //Criação de um objeto que vai recuperar o retorno da API
            var retorno = new API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoToken>();
            //Criar uma instancia do Objeto Token Business responsavel por gerar o token
            //Na criação do Objeto e obrigatorio passar o objeto de configuração do cliente
            var tokenBusiness = new API.Intermeio.Business.TokenBusiness(_configuracao);
            //Com o objeto instaciado e passado os parametros de configuração podemos chamar o methodo para recuperar os dados Token
            retorno = tokenBusiness.RetornaToken();

            //Aqui usamos apenas um modelo para retornar um JSON para a view.
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GerarBoleto()
        {

            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao).GerarBoleto(MontarBoleto());

            return Json(boletoBuisiness, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GerarBoletoToken(string token)
        {

            var boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao).GerarBoleto(MontarBoleto(), token);

            return Json(boletoBuisiness, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GerarBoletoLote()
        {
            API.Intermeio.Models.BoletoLoteModel boletolote = new API.Intermeio.Models.BoletoLoteModel();
            List<API.Intermeio.Models.BoletoModel> boletos = new List<API.Intermeio.Models.BoletoModel>();
            for (int i = 0; i < 4; i++)
            {
                boletos.Add(MontarBoleto());
            }

            boletolote.Boletos = boletos;

            API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoLote> boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao).GerarBoletoLote(boletolote);

            return Json(boletoBuisiness, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GerarBoletoLoteToken(string token)
        {

            API.Intermeio.Models.BoletoLoteModel boletolote = new API.Intermeio.Models.BoletoLoteModel();
            List<API.Intermeio.Models.BoletoModel> boletos = new List<API.Intermeio.Models.BoletoModel>();
            for (int i = 0; i < 4; i++)
            {
                boletos.Add(MontarBoleto());
            }

            boletolote.Boletos = boletos;

            API.Intermeio.Models.RetornoAPI<API.Intermeio.Models.RetornoLote> boletoBuisiness = new API.Intermeio.Business.BoletoBusiness(_configuracao).GerarBoletoLote(boletolote, token);

            return Json(boletoBuisiness, JsonRequestBehavior.AllowGet);
        }


        //private API.Intermeio.Models.BoletoModel MontarBoleto()
        //{
        //    var boleto = new API.Intermeio.Models.BoletoModel();

        //    Random random = new Random();


        //    boleto.Boleto = new API.Intermeio.Models.Boleto()
        //    {
        //        DataVencimento = "10/10/2020",
        //        NumeroDocumento = "123456789",
        //        PercentualJuros = "0",
        //        PercentualMulta = "0",
        //        QntDiasJuros = 0,
        //        QntDiasMulta = 0,
        //        Valor = random.Next(1, 100).ToString(),
        //        ValorJuros = 0
        //    };

        //    boleto.Cliente = new API.Intermeio.Models.Cliente()
        //    {
        //        NomeRazao = Guid.NewGuid().ToString(),
        //        ApelidoEndereco = "endereco 01",
        //        Bairro = "Tatuape",
        //        Celular = "11949544145",
        //        CEP = "03633020",
        //        Cidade = "Sao Paulo",
        //        Complemento = "Casa",
        //        CpfCnpj = "43122377861",
        //        DataDeNascimento = "27/02/1987",
        //        Email = "jose@gmail.com",
        //        Endereco = "Francisco Gonzales",
        //        Estado = "SP",
        //        Logradouro = "Rua",
        //        Sexo = "M",
        //        Numero = "94"

        //    };

        //    var split = new API.Intermeio.Models.SplitModel() { AbortarEmCasoDeErro = true };

        //    var clientesSplit = new List<API.Intermeio.Models.SplitCliente>();

        //    var clientSplit = new API.Intermeio.Models.SplitCliente()
        //    {
        //        NomeRazao = "Fabio Santos",
        //        CpfCnpj = "43122377861",
        //        Descricao = "teste",
        //        Email = "fabio.g@inttecnologia.com.br",
        //        Taxa = "16",
        //        NotificarPorEmail = true,
        //        NotificarPorSms = false,
        //        Telefones = new List<API.Intermeio.Models.Telefone>()
        //        {
        //           new API.Intermeio.Models.Telefone()
        //           {
        //            Apelido = "Fabio",
        //            DDD = "11",
        //            Numero = "998789223"
        //           }
        //        },
        //        ClienteContaBancaria = new API.Intermeio.Models.ClienteContaBancaria()
        //        {
        //            Conta = "11111",
        //            DigConta = "1",
        //            Agencia = "2222",
        //            DigAgencia = "2",
        //            CodigoBanco = "12345"
        //        },
        //        TransferenciaAutomatica = new API.Intermeio.Models.TransferenciaAutomatica()
        //        {
        //            ACobrar = true,
        //            EfetuarTefAutomatica = true,
        //            Periodicidade = 1,
        //            ValorMinimo = "100"
        //        }
        //    };

        //    var clientSplit2 = new API.Intermeio.Models.SplitCliente()
        //    {
        //        NomeRazao = Guid.NewGuid().ToString(),
        //        CpfCnpj = "43122377861",
        //        Descricao = "teste",
        //        Email = "fabio@teste.com.br",
        //        Taxa = "20",
        //        NotificarPorEmail = true,
        //        NotificarPorSms = true,
        //        Telefones = new List<API.Intermeio.Models.Telefone>()
        //        {
        //           new API.Intermeio.Models.Telefone()
        //           {
        //            Apelido = "Split 2",
        //            DDD = "11",
        //            Numero = "998789223"
        //           }
        //        },
        //        ClienteContaBancaria = new API.Intermeio.Models.ClienteContaBancaria()
        //        {
        //            Conta = "11111",
        //            DigConta = "1",
        //            Agencia = "2222",
        //            DigAgencia = "2",
        //            CodigoBanco = "12345"
        //        },
        //        TransferenciaAutomatica = new API.Intermeio.Models.TransferenciaAutomatica()
        //        {
        //            ACobrar = true,
        //            EfetuarTefAutomatica = true,
        //            Periodicidade = 1,
        //            ValorMinimo = "100"
        //        }
        //    };

        //    clientesSplit.Add(clientSplit);
        //    clientesSplit.Add(clientSplit2);

        //    split.Clientes = clientesSplit;

        //    boleto.Split = split;

        //    var configuracoes = new API.Intermeio.Models.Configuracoes()
        //    {
        //        EmissaoDigital = new API.Intermeio.Models.EmissaoDigital()
        //        {
        //            Email = "fabio.tecnologia@live.com",
        //            Sms = new API.Intermeio.Models.Sms()
        //            {
        //                Celular = "11998789223",
        //                Msg = "Teste Via API classe de teste"
        //            }
        //        }
        //    };

        //    //  boleto.Configuracoes = configuracoes;

        //    return boleto;
        //}



        //Criando um methodo que vai retornar o Objeto ja preenchido
        private API.Intermeio.Models.BoletoModel MontarBoleto()
        {

            //Criando o objeto modelo onde será passado objetos do tipo Boleto, Cliente, SplitModel, Configuracoes

            var boleto = new API.Intermeio.Models.BoletoModel();

            //Criando um objeto do tipo Boleto e adicionando no modelo
            //Estou colocando um exemplo com dados fictícios
            boleto.Boleto = new API.Intermeio.Models.Boleto()
            {
                DataVencimento = "10/10/2020",
                NumeroDocumento = "123456789",
                PercentualJuros = "0",
                PercentualMulta = "0",
                QntDiasJuros = 0,
                QntDiasMulta = 0,
                Valor = "020", //Ex: R$ 0,20 caso queira colocar um valor de R$ 4500,68 colocar número inteiro sem pontuação Ex.450068
                ValorJuros = 0
            };

            //Criando um objeto do tipo Cliente e adicionando no modelo
            //Estou colocando um exemplo com dados fictícios
            boleto.Cliente = new API.Intermeio.Models.Cliente()
            {
                NomeRazao = "Zezinho Juarez",
                ApelidoEndereco = "endereco 01",
                Bairro = "Tatuape",
                Celular = "11944445555",
                CEP = "03633020",
                Cidade = "Sao Paulo",
                Complemento = "Casa",
                CpfCnpj = "43122377861",
                DataDeNascimento = "27/02/1987",
                Email = "jose@gmail.com",
                Endereco = "Francisco Gonzales",
                Estado = "SP",
                Logradouro = "Rua",
                Sexo = "M",
                Numero = "94"
            };

            //Este Objeto não é obrigatório, caso deseje fazer um split
            //Criando um Objeto Split Model
            var split = new API.Intermeio.Models.SplitModel();
            //Este parametro e muito importante quando está marcado como true, qualquer erro que houver na hora do split ele aborta o processo de geração do boleto
            split.AbortarEmCasoDeErro = true;
            //Dentro do Split um dos objetos que ele recebe é uma lista de clientes, aqui estamos criando uma lista para preencher 2 clientes
            var clientesSplit = new List<API.Intermeio.Models.SplitCliente>();
            //Criando o primeiro objeto do cliente que vai ficar dentro do Split
            var clientSplit = new API.Intermeio.Models.SplitCliente()
            {
                NomeRazao = "Fabio Santos",
                CpfCnpj = "43122377861",
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
                CpfCnpj = "43122377861",
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
            split.Clientes = clientesSplit;
            //Adicionando o objeto split no modelo boleto boleto.Split = split;
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
            boleto.Configuracoes = configuracoes;
            //Retornando o objeto(Boleto) todo preenchido pronto para gerar o boleto
            return boleto;
        }


    }
}